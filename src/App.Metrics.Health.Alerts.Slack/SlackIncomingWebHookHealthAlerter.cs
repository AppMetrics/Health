// <copyright file="SlackIncomingWebHookHealthAlerter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Alerts.Slack.Internal;

namespace App.Metrics.Health.Alerts.Slack
{
    public class SlackIncomingWebHookHealthAlerter : IReportHealthStatus
    {
        private const string DegradingColor = "#FFCC00";
        private const string FailingColor = "#F35A00";
        private readonly SlackHealthAlertOptions _slackOptions;
        private readonly HttpClient _httpClient;

        public SlackIncomingWebHookHealthAlerter(SlackHealthAlertOptions slackOptions)
        {
            _slackOptions = slackOptions ?? throw new ArgumentNullException(nameof(slackOptions));
            _httpClient = new HttpClient();
        }

        /// <inheritdoc />
        public async Task ReportAsync(HealthOptions options, HealthStatus status, CancellationToken cancellationToken = default)
        {
            var applicationName = options.ApplicationName;

            if (Uri.TryCreate(applicationName, UriKind.Absolute, out var appUri))
            {
                applicationName = $"<{appUri}|{appUri}>";
            }

            var slackMessage = new SlackPayload
                               {
                                   Text = $"*{applicationName} Health Alert*",
                                   Channel = _slackOptions.Channel,
                                   UserName = _slackOptions.Username,
                                   IconEmoji = string.IsNullOrWhiteSpace(_slackOptions.EmojiIcon) ? ":broken_heart:" : _slackOptions.EmojiIcon
                               };

            var unhealthyCheck = status.Results.Where(r => r.Check.Status == HealthCheckStatus.Unhealthy).ToList();

            if (unhealthyCheck.Any())
            {
                AddHealthAttachments(status, options.ApplicationName, HealthCheckStatus.Unhealthy, unhealthyCheck, slackMessage);
            }

            if (_slackOptions.AlertOnDegradedChecks)
            {
                var degradedChecks = status.Results.Where(r => r.Check.Status == HealthCheckStatus.Degraded).ToList();

                if (degradedChecks.Any())
                {
                    AddHealthAttachments(status, options.ApplicationName, HealthCheckStatus.Degraded, degradedChecks, slackMessage);
                }
            }

            if (slackMessage.Attachments.Any())
            {
                await _httpClient.PostAsync(_slackOptions.WebhookUrl, new JsonContent(slackMessage), cancellationToken);
            }
        }

        private static void AddHealthAttachments(
            HealthStatus status,
            string applicationName,
            HealthCheckStatus checkStatus,
            IReadOnlyCollection<HealthCheck.Result> checks,
            SlackPayload slackMessage)
        {
            var checkOrChecks = checks.Count > 1 ? "checks" : "check";
            var textEnd = string.Empty;
            var color = string.Empty;

            if (checkStatus == HealthCheckStatus.Unhealthy)
            {
                textEnd = "failing";
                color = FailingColor;
            }

            if (checkStatus == HealthCheckStatus.Degraded)
            {
                textEnd = "degrading";
                color = DegradingColor;
            }

            var attachment = new SlackAttachment
                             {
                                 Text = $"*_{checks.Count} / {status.Results.Count()} {checkOrChecks} {textEnd}_*",
                                 Color = color
            };

            foreach (var result in checks)
            {
                attachment.Fields.Add(
                    new SlackAttachmentFields
                    {
                        Title = $">{result.Name}",
                        Value = $">_{result.Check.Message}_",
                        Short = true
                    });
            }

            slackMessage.Attachments.Add(attachment);
        }
    }
}
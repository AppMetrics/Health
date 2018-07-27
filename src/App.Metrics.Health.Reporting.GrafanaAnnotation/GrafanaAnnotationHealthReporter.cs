// <copyright file="GrafanaAnnotationHealthReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;
using App.Metrics.Health.Reporting.GrafanaAnnotation.Internal;

namespace App.Metrics.Health.Reporting.GrafanaAnnotation
{
    public class GrafanaAnnotationHealthReporter : IReportHealthStatus
    {
        private static readonly Dictionary<string, long> LastUnhealthyCheckCache = new Dictionary<string, long>();
        private static readonly Dictionary<string, long> LastDegradedCheckCache = new Dictionary<string, long>();
        private static readonly ILog Logger = LogProvider.For<GrafanaAnnotationHealthReporter>();
        private readonly GrafanaHealthAnnotationOptions _grafanaAnnotationOptions;
        private readonly HealthCheckResultsStore _store;

        public GrafanaAnnotationHealthReporter(GrafanaHealthAnnotationOptions grafanaAnnotationOptions)
        {
            _grafanaAnnotationOptions = grafanaAnnotationOptions ?? throw new ArgumentNullException(nameof(grafanaAnnotationOptions));

            ReportInterval = grafanaAnnotationOptions.ReportInterval > TimeSpan.Zero
                ? grafanaAnnotationOptions.ReportInterval
                : HealthConstants.Reporting.DefaultReportInterval;

            _store = new HealthCheckResultsStore(_grafanaAnnotationOptions);

            Logger.Trace($"Using Metrics Reporter {this}. Grafana Annotation Endpoint: {grafanaAnnotationOptions.AnnotationEndpoint} ReportInterval: {ReportInterval}");
        }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; set; }

        /// <inheritdoc />
        public async Task ReportAsync(HealthOptions options, HealthStatus status, CancellationToken cancellationToken = default)
        {
            if (!_grafanaAnnotationOptions.Enabled || !options.Enabled)
            {
                Logger.Trace($"Health Status Reporter '{this}' disabled, not reporting.");

                return;
            }

            Logger.Trace($"Health Status Reporter '{this}' reporting health status.");

            var applicationName = options.ApplicationName;

            if (!_grafanaAnnotationOptions.Tags.Contains(applicationName))
            {
                _grafanaAnnotationOptions.Tags.Add(applicationName);
            }

            if (Uri.TryCreate(applicationName, UriKind.Absolute, out var appUri))
            {
                applicationName = $"<a href='{appUri}'>{appUri}</a>";
            }

            var lastResults = await _store.GetAsync();

            var currentUnhealthyChecks = status.Results.Where(r => r.Check.Status == HealthCheckStatus.Unhealthy);
            var currentDegradedChecks = status.Results.Where(r => r.Check.Status == HealthCheckStatus.Degraded);
            var currentHealthyChecks = status.Results.Where(r => r.Check.Status == HealthCheckStatus.Healthy);

            var lastResult = await _store.GetLastResult(cancellationToken);

            if (status.Status.IsHealthy() && lastResult.Status.IsHealthy())
            {
                return;
            }

            if (!status.Status.IsHealthy() && lastResult.Status.IsHealthy())
            {
                // write new annotation with all checks is current status failing/degrading in text, just start time i.e. region = false

                return;
            }

            if (status.Status.IsHealthy() && !lastResult.Status.IsHealthy())
            {
                // end current annotation with now
                // write new annotation with all checks is last result status failing/degrading in text, just start time i.e. region = false
            }

            if (await _store.HasStatusChange(status, cancellationToken))
            {
                // end current annotation with now
                // create new annotation with all failing/degraded checks in text, just start time i.e. region = false
            }

            await _store.SaveAsync(status, cancellationToken);

            // TODO:
            // if overall status is unhealthy or degraded annotate
            // on next interval, delete and update with region = true if same failed checks
            // on next interval, if any checks status change (isdirty) end current annotation and create a new one
            // list all unhealthy and degraded checks in each annotation

            // if (!lastResults.Any())
            // {
            //    foreach (var unhealthyCheck in currentUnhealthyChecks)
            //    {
            //        await _store.WriteStartHealthAnnotationAsync(unhealthyCheck, applicationName, cancellationToken);
            //    }

            // foreach (var degradedCheck in currentDegradedChecks)
            //    {
            //        await _store.WriteStartHealthAnnotationAsync(degradedCheck, applicationName, cancellationToken);
            //    }

            // await _store.SaveAsync(status.Results, cancellationToken);

            // return;
            // }

            // var checksWithStatusChange = await _store.GetWhereStatusChangedAsync(status.Results.ToList());

            // foreach (var checkWithStatusChange in checksWithStatusChange)
            // {
            //    if (checkWithStatusChange.Check.Status.IsHealthy())
            //    {
            //        await _store.UpdateEndHealthAnnotationAsync(checkWithStatusChange, applicationName, cancellationToken);
            //    }
            //    else
            //    {
            //        await _store.WriteStartHealthAnnotationAsync(checkWithStatusChange, applicationName, cancellationToken);
            //    }
            // }
        }
    }
}
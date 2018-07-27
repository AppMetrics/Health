// <copyright file="HealthCheckResultsStore.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
#if !NETSTANDARD1_6
using App.Metrics.Health.Internal;
#endif
using App.Metrics.Health.Logging;
using Newtonsoft.Json;

namespace App.Metrics.Health.Reporting.GrafanaAnnotation.Internal
{
    public class HealthCheckResultsStore
    {
        private static readonly ConcurrentDictionary<string, HealthCheckResultItem> Store = new ConcurrentDictionary<string, HealthCheckResultItem>();
        private static readonly ILog Logger = LogProvider.For<HealthCheckResultsStore>();
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private static HealthStatus _lastResult = new HealthStatus(Enumerable.Empty<HealthCheck.Result>());
        private readonly HttpClient _httpClient;
        private readonly GrafanaHealthAnnotationOptions _grafanaAnnotationOptions;

        public HealthCheckResultsStore(GrafanaHealthAnnotationOptions grafanaAnnotationOptions)
        {
            _grafanaAnnotationOptions = grafanaAnnotationOptions ?? throw new ArgumentNullException(nameof(grafanaAnnotationOptions));
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _grafanaAnnotationOptions.Token);
        }

        public Task SaveAsync(HealthStatus healthStatus, CancellationToken token)
        {
            _lastResult = healthStatus;

#if NETSTANDARD1_6
            return Task.CompletedTask;
#else
            return AppMetricsHealthTaskHelper.CompletedTask();
#endif
        }

        public Task<bool> HasStatusChange(HealthStatus healthStatus, CancellationToken token) { return Task.FromResult(true); }

        public Task<HealthStatus> GetLastResult(CancellationToken token) { return Task.FromResult(_lastResult); }

        public Task SaveAsync(IEnumerable<HealthCheck.Result> results, CancellationToken token)
        {
            foreach (var result in results)
            {
                Store.AddOrUpdate(
                    result.Name,
                    new HealthCheckResultItem(result.Name, result.Check.Status),
                    (name, item) => new HealthCheckResultItem(result.Name, result.Check.Status, item.AnnotationId));
            }

#if NETSTANDARD1_6
            return Task.CompletedTask;
#else
            return AppMetricsHealthTaskHelper.CompletedTask();
#endif
        }

        public Task<List<HealthCheck.Result>> GetWhereStatusChangedAsync(List<HealthCheck.Result> results)
        {
            var result = new List<HealthCheck.Result>();

            foreach (var existingResult in Store)
            {
                if (results.Any(r => r.Name == existingResult.Key && r.Check.Status != existingResult.Value.Status))
                {
                    result.Add(results.Single(r => r.Name == existingResult.Key && r.Check.Status != existingResult.Value.Status));
                }
            }

            return Task.FromResult(result);
        }

        public Task<List<HealthCheckResultItem>> GetAsync()
        {
            return Task.FromResult(Store.Values.ToList());
        }

        public async Task WriteStartHealthAnnotationAsync(HealthCheck.Result checkResult, string appplicationName, CancellationToken cancellationToken)
        {
            var annotation = new GrafanaAnnotationPayload
                             {
                                 Text = $"Health Check: {checkResult.Name} {checkResult.Check.Status}<br />Application: {appplicationName}",
                                 Time = (long)(DateTime.UtcNow - Epoch).TotalMilliseconds,
                                 Tags = _grafanaAnnotationOptions.Tags,
                                 DashboardId = _grafanaAnnotationOptions.DashboardId,
                                 PanelId = _grafanaAnnotationOptions.PanelId
                             };

            try
            {
                var httpResponse = await _httpClient.PostAsync(_grafanaAnnotationOptions.AnnotationEndpoint, new JsonContent(annotation), cancellationToken);

                if (httpResponse.IsSuccessStatusCode)
                {
                    Logger.Trace($"Health Status Reporter '{this}' successfully reported health status.");

                    var jsonResponse = await httpResponse.Content.ReadAsStringAsync();

                    var response = JsonConvert.DeserializeObject<GrafanaCreateAnnotationResponse>(jsonResponse);

                    Store[checkResult.Name] = new HealthCheckResultItem(checkResult.Name, checkResult.Check.Status, response.Id);
                }
                else
                {
                    Logger.Error($"Health Status Reporter '{this}' failed to reported health status with status code: '{httpResponse.StatusCode}' and reason phrase: '{httpResponse.ReasonPhrase}'");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Health Status Reporter '{this}' failed to reported health status");
            }
        }

        public async Task UpdateEndHealthAnnotationAsync(HealthCheck.Result checkResult, string appplicationName, CancellationToken cancellationToken)
        {
            if (!Store.ContainsKey(checkResult.Name))
            {
                return;
            }

            var lastResult = Store[checkResult.Name];

            var annotation = new GrafanaAnnotationPayload
                             {
                                 Text = $"Health Check: {checkResult.Name} {checkResult.Check.Status}<br />Application: {appplicationName}",
                                 Time = lastResult.InitialCheckAt,
                                 TimeEnd = (long)(DateTime.UtcNow - Epoch).TotalMilliseconds,
                                 IsRegion = true,
                                 Tags = _grafanaAnnotationOptions.Tags,
                                 DashboardId = _grafanaAnnotationOptions.DashboardId,
                                 PanelId = _grafanaAnnotationOptions.PanelId
                             };

            try
            {
                await _httpClient.DeleteAsync($"{_grafanaAnnotationOptions.AnnotationEndpoint}/{lastResult.AnnotationId}", cancellationToken);

                var response = await _httpClient.PostAsync($"{_grafanaAnnotationOptions.AnnotationEndpoint}", new JsonContent(annotation), cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    Logger.Trace($"Health Status Reporter '{this}' successfully reported health status.");
                }
                else
                {
                    Logger.Error($"Health Status Reporter '{this}' failed to reported health status with status code: '{response.StatusCode}' and reason phrase: '{response.ReasonPhrase}'");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Health Status Reporter '{this}' failed to reported health status");
            }
        }
    }
}

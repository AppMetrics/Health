// <copyright file="HealthResultsAsMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
#if !NETSTANDARD1_6
using App.Metrics.Health.Internal;
 #endif
using App.Metrics.Health.Reporting.Metrics.Internal;

namespace App.Metrics.Health.Reporting.Metrics
{
    public class HealthResultsAsMetricsReporter : IReportHealthStatus
    {
        private readonly IMetrics _metrics;

        public HealthResultsAsMetricsReporter(IMetrics metrics) { _metrics = metrics; }

        /// <inheritdoc />
        public Task ReportAsync(HealthOptions options, HealthStatus status, CancellationToken cancellationToken = default)
        {
            foreach (var healthResult in status.Results)
            {
                var tags = new MetricTags(HealthReportingConstants.TagKeys.HealthCheckName, healthResult.Name);

                if (healthResult.Check.Status == HealthCheckStatus.Degraded)
                {
                    _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.degraded);
                }
                else if (healthResult.Check.Status == HealthCheckStatus.Unhealthy)
                {
                    _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.unhealthy);
                }
                else if (healthResult.Check.Status == HealthCheckStatus.Healthy)
                {
                    _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.healthy);
                }
            }

            var overallHealthStatus = HealthConstants.HealthScore.healthy;

            if (status.Status == HealthCheckStatus.Unhealthy)
            {
                overallHealthStatus = HealthConstants.HealthScore.unhealthy;
            }
            else if (status.Status == HealthCheckStatus.Degraded)
            {
                overallHealthStatus = HealthConstants.HealthScore.degraded;
            }

            _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.HealthGauge, overallHealthStatus);

#if NETSTANDARD1_6
            return Task.CompletedTask;
#else
            return AppMetricsHealthTaskHelper.CompletedTask();
#endif
        }
    }
}
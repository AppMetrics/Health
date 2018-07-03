// <copyright file="DefaultHealthCheckRunner.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;

namespace App.Metrics.Health.Internal
{
    public sealed class DefaultHealthCheckRunner : IRunHealthChecks
    {
        private static readonly ILog Logger = LogProvider.For<DefaultHealthCheckRunner>();
        private readonly IEnumerable<HealthCheck> _checks;
        private readonly HealthReporterCollection _reporters;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHealthCheckRunner" /> class.
        /// </summary>
        /// <param name="checks">The registered health checks.</param>
        /// <param name="reporters">Reporters to ran after health checks have been executed</param>
        public DefaultHealthCheckRunner(IEnumerable<HealthCheck> checks, HealthReporterCollection reporters)
        {
            _reporters = reporters ?? throw new ArgumentNullException(nameof(reporters));
            _checks = checks ?? Enumerable.Empty<HealthCheck>();
        }

        /// <inheritdoc />
        public async ValueTask<HealthStatus> ReadAsync(CancellationToken cancellationToken = default)
        {
            if (!_checks.Any())
            {
                return default;
            }

            var startTimestamp = Logger.IsTraceEnabled() ? Stopwatch.GetTimestamp() : 0;

            Logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(_checks.OrderBy(v => v.Name).Select(v => v.ExecuteAsync(cancellationToken).AsTask()));

            var healthStatus = new HealthStatus(results);

            Logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            if (!_reporters.Any())
            {
                return healthStatus;
            }

            Logger.HealthCheckResultsReportActionExecuting();

            await Task.WhenAll(_reporters.Select(reporter => RunReporter(healthStatus, cancellationToken, reporter)));

            Logger.HealthCheckResultsReportActionExecuted();

            return healthStatus;
        }

        private async Task RunReporter(HealthStatus healthStatus, CancellationToken cancellationToken, IReportHealthStatus reporter)
        {
            try
            {
                await reporter.ReportAsync(healthStatus, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
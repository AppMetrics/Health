// <copyright file="DefaultHealthProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;

namespace App.Metrics.Health.Internal
{
    public sealed class DefaultHealthProvider : IProvideHealth
    {
        private static readonly ILog Logger = LogProvider.For<DefaultHealthProvider>();
        private readonly IHealthCheckRegistry _healthCheckRegistry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHealthProvider" /> class.
        /// </summary>
        /// <param name="healthCheckRegistry">The health check registry.</param>
        public DefaultHealthProvider(
            IHealthCheckRegistry healthCheckRegistry)
        {
            _healthCheckRegistry = healthCheckRegistry;
        }

        /// <inheritdoc />
        public async ValueTask<HealthStatus> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var startTimestamp = Logger.IsTraceEnabled() ? Stopwatch.GetTimestamp() : 0;

            Logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(
                _healthCheckRegistry.Checks.Values.OrderBy(v => v.Name).Select(v => v.ExecuteAsync(cancellationToken).AsTask()));

            var healthStatus = new HealthStatus(results);

            Logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }
    }
}
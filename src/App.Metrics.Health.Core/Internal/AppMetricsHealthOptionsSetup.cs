// <copyright file="AppMetricsHealthOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Internal
{
    public class AppMetricsHealthOptionsSetup : IConfigureOptions<AppMetricsHealthOptions>
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<AppMetricsHealthOptionsSetup> _logger;

        public AppMetricsHealthOptionsSetup(ILogger<AppMetricsHealthOptionsSetup> logger, IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Configure(AppMetricsHealthOptions options)
        {
            RegisterHealthCheckRegistry(options);
        }

        private void RegisterHealthCheckRegistry(AppMetricsHealthOptions options)
        {
            if (!options.Enabled)
            {
                return;
            }

            var autoScannedHealthChecks = Enumerable.Empty<HealthCheck>();

            try
            {
                autoScannedHealthChecks = _provider.GetRequiredService<IEnumerable<HealthCheck>>();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(
                    new EventId(5000),
                    ex,
                    "Failed to load auto scanned health checks, health checks won't be registered");
            }

            options.Checks.AddChecks(autoScannedHealthChecks);
        }
    }
}
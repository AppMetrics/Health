// <copyright file="HealthOptionsSetup.cs" company="Allan Hardy">
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
    /// <summary>
    ///     Sets up default options for <see cref="HealthOptions"/>.
    /// </summary>
    public class HealthOptionsSetup : IConfigureOptions<HealthOptions>
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<HealthOptionsSetup> _logger;

        public HealthOptionsSetup(ILogger<HealthOptionsSetup> logger, IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Configure(HealthOptions options)
        {
            RegisterHealthCheckRegistry(options);
        }

        private void RegisterHealthCheckRegistry(HealthOptions options)
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
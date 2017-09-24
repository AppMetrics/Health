// <copyright file="HealthBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Internal;
using App.Metrics.Health.Internal.NoOp;
using App.Metrics.Health.Logging;

namespace App.Metrics.Health.Builder
{
    public class HealthBuilder : IHealthBuilder
    {
        private static readonly ILog Logger = LogProvider.For<HealthBuilder>();
        private readonly Dictionary<string, HealthCheck> _checks = new Dictionary<string, HealthCheck>(StringComparer.OrdinalIgnoreCase);
        private readonly HealthFormatterCollection _healthFormatterCollection = new HealthFormatterCollection();
        private IHealthOutputFormatter _defaultMetricsHealthFormatter;
        private HealthOptions _options;

        /// <inheritdoc />
        public IHealthConfigurationBuilder Configuration
        {
            get
            {
                return new HealthConfigurationBuilder(
                    this,
                    _options,
                    options => { _options = options; });
            }
        }

        public IHealthCheckBuilder HealthChecks
        {
            get
            {
                return new HealthCheckBuilder(
                    this,
                    healthCheck =>
                    {
                        try
                        {
                            _checks.Add(healthCheck.Name, healthCheck);
                        }
                        catch (ArgumentException ex)
                        {
                            Logger.Error(ex, $"Attempted to add health checks with duplicates names: {healthCheck.Name}");
                            throw;
                        }
                    });
            }
        }

        /// <inheritdoc />
        public IHealthOutputFormattingBuilder OutputHealth => new HealthOutputFormattingBuilder(
            this,
            (replaceExisting, formatter) =>
            {
                if (_defaultMetricsHealthFormatter == null)
                {
                    _defaultMetricsHealthFormatter = formatter;
                }

                if (replaceExisting)
                {
                    _healthFormatterCollection.TryAdd(formatter);
                }
                else
                {
                    if (_healthFormatterCollection.GetType(formatter.GetType()) == null)
                    {
                        _healthFormatterCollection.Add(formatter);
                    }
                }
            });

        public IHealthRoot Build()
        {
            if (_options == null)
            {
                _options = new HealthOptions();
            }

            if (_healthFormatterCollection.Count == 0)
            {
                _healthFormatterCollection.Add(new HealthStatusTextOutputFormatter());
            }

            IRunHealthChecks healthCheckRunner;

            var health = new DefaultHealth(_checks.Values);
            var defaultMetricsOutputFormatter = _defaultMetricsHealthFormatter ?? _healthFormatterCollection.FirstOrDefault();

            if (_options.Enabled)
            {
                healthCheckRunner = new DefaultHealthCheckRunner(health.Checks);
            }
            else
            {
                healthCheckRunner = new NoOpHealthCheckRunner();
            }

            return new HealthRoot(
                health,
                _options,
                _healthFormatterCollection,
                defaultMetricsOutputFormatter,
                healthCheckRunner);
        }
    }
}
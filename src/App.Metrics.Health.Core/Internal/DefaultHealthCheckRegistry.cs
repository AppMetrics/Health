// <copyright file="DefaultHealthCheckRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Internal
{
    public sealed class DefaultHealthCheckRegistry : IHealthCheckRegistry
    {
        private readonly Dictionary<string, HealthCheck> _checks;

        public DefaultHealthCheckRegistry(IEnumerable<HealthCheck> healthChecks)
        {
            _checks = new Dictionary<string, HealthCheck>(StringComparer.OrdinalIgnoreCase);

            Register(healthChecks);
        }

        public DefaultHealthCheckRegistry()
        {
            _checks = new Dictionary<string, HealthCheck>(StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, HealthCheck> Checks => _checks;

        /// <inheritdoc />
        public void AddCheck(string name, Func<ValueTask<HealthCheckResult>> check)
        {
            Register(new HealthCheck(name, check));
        }

        /// <inheritdoc />
        public void AddCheck(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check)
        {
            Register(new HealthCheck(name, check));
        }

        internal void Register(IEnumerable<HealthCheck> healthChecks)
        {
            foreach (var check in healthChecks)
            {
                _checks.Add(check.Name, check);
            }
        }

        internal void Register(HealthCheck healthCheck)
        {
            _checks.Add(healthCheck.Name, healthCheck);
        }
    }
}
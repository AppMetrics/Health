// <copyright file="DefaultHealth.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Health.Internal
{
    public class DefaultHealth : IHealth
    {
        public DefaultHealth(IEnumerable<HealthCheck> checks)
        {
            Checks = checks ?? Enumerable.Empty<HealthCheck>();
        }

        /// <inheritdoc />
        public IEnumerable<HealthCheck> Checks { get; }
    }
}

// <copyright file="IHealthCheckRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health
{
    public interface IHealthCheckRegistry
    {
        IReadOnlyDictionary<string, HealthCheck> Checks { get; }

        void AddChecks(IEnumerable<HealthCheck> checks);

        void AddCheck(string name, Func<ValueTask<HealthCheckResult>> check);

        void AddCheck(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check);
    }
}
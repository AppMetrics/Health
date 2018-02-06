// <copyright file="HealthTestFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Health.Builder;

namespace App.Metrics.Health.Benchmarks.Fixtures
{
    public class HealthTestFixture : IDisposable
    {
        public HealthTestFixture()
        {
            var health = new HealthBuilder()
                .HealthChecks.AddCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()))
                .Build();

            HealthCheckRunner = health.HealthCheckRunner;
        }

        public IRunHealthChecks HealthCheckRunner { get; }

        public void Dispose() { }
    }
}
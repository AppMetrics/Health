// <copyright file="HealthFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Internal;

namespace App.Metrics.Health.Formatters.Ascii.Facts.Fixtures
{
    public class HealthFixture : IDisposable
    {
        public HealthFixture()
        {
            HealthCheckRegistry = new DefaultHealthCheckRegistry();
            var healthStatusProvider = new DefaultHealthProvider(HealthCheckRegistry);
            Health = healthStatusProvider;
        }

        public IHealthCheckRegistry HealthCheckRegistry { get; }

        public IProvideHealth Health { get; }

        public void Dispose() { }
    }
}
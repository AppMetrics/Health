// <copyright file="HealthFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Health.Facts.Fixtures
{
    public class HealthFixture : IDisposable
    {
        public HealthFixture()
        {
            StartupAssemblyName = typeof(HealthFixture).Assembly.GetName().Name;
        }

        public string StartupAssemblyName { get; }

        public void Dispose() { }
    }
}
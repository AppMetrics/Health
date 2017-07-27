// <copyright file="AppMetricsHealthCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Health.Internal
{
    public class AppMetricsHealthCoreBuilder : IAppMetricsHealthCoreBuilder
    {
        public AppMetricsHealthCoreBuilder(IServiceCollection services) { Services = services ?? throw new ArgumentNullException(nameof(services)); }

        public IServiceCollection Services { get; }
    }
}

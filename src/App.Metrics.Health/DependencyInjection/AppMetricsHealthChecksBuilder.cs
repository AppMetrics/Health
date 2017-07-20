// <copyright file="AppMetricsHealthChecksBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Health.Builder
{
    internal sealed class AppMetricsHealthChecksBuilder : IAppMetricsHealthChecksBuilder
    {
        internal AppMetricsHealthChecksBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = new AppMetricsEnvironment();
        }

        public IAppMetricsEnvironment Environment { get; }

        public IServiceCollection Services { get; }
    }
}
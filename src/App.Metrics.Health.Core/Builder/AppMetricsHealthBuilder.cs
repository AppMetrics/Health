﻿// <copyright file="AppMetricsHealthBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Health.Builder
{
    public sealed class AppMetricsHealthBuilder : IAppMetricsHealthBuilder
    {
        public AppMetricsHealthBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}
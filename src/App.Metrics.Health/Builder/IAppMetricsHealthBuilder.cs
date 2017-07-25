// <copyright file="IAppMetricsHealthBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Health.Builder
{
    public interface IAppMetricsHealthBuilder
    {
        IServiceCollection Services { get; }
    }
}
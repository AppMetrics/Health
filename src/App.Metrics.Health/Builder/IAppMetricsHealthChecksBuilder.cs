// <copyright file="IAppMetricsHealthChecksBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Health.Builder
{
    public interface IAppMetricsHealthChecksBuilder
    {
        IServiceCollection Services { get; }
    }
}
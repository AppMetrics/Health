// <copyright file="HealthCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Health.Internal
{
    public class HealthCoreBuilder : IHealthCoreBuilder
    {
        public HealthCoreBuilder(IServiceCollection services) { Services = services ?? throw new ArgumentNullException(nameof(services)); }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}

// <copyright file="AppMetricsHealthOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Internal
{
    public class AppMetricsHealthOptionsSetup : IConfigureOptions<AppMetricsHealthOptions>
    {
        /// <inheritdoc />
        public void Configure(AppMetricsHealthOptions options)
        {
        }
    }
}
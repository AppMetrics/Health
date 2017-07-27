// <copyright file="AppMetricsHealthOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.Health.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    public class AppMetricsHealthOptions
    {
        public AppMetricsHealthOptions()
        {
            Enabled = true;
            OutputFormatters = new HealthFormatterCollection<IHealthOutputFormatter>();
        }

        public bool Enabled { get; set; }

        /// <summary>
        ///     Gets a list of <see cref="IHealthOutputFormatter" />s that are used by this application to format health check results.
        /// </summary>
        /// <value>
        ///     A list of <see cref="IHealthOutputFormatter" />s that are used by this application.
        /// </value>
        public HealthFormatterCollection<IHealthOutputFormatter> OutputFormatters { get; }
    }
}
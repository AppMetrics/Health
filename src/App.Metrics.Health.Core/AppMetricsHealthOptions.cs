// <copyright file="AppMetricsHealthOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.Health.Formatters;

namespace App.Metrics.Health
{
    [ExcludeFromCodeCoverage]
    public class AppMetricsHealthOptions
    {
        public AppMetricsHealthOptions()
        {
            Enabled = true;
            OutputFormatters = new HealthFormatterCollection();
        }

        /// <summary>
        ///     Gets or sets the default <see cref="IHealthOutputFormatter" /> to use when health checks are attempted to be formatted.
        /// </summary>
        /// <value>
        ///     The default <see cref="IHealthOutputFormatter" />s that is used by this application.
        /// </value>
        public IHealthOutputFormatter DefaultOutputFormatter { get; set; }

        public bool Enabled { get; set; }

        /// <summary>
        ///     Gets a list of <see cref="IHealthOutputFormatter" />s that are used by this application to format health check
        ///     results.
        /// </summary>
        /// <value>
        ///     A list of <see cref="IHealthOutputFormatter" />s that are used by this application.
        /// </value>
        public HealthFormatterCollection OutputFormatters { get; }
    }
}
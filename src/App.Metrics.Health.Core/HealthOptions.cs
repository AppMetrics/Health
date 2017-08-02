// <copyright file="HealthOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Health.Formatters;
using App.Metrics.Health.Internal;

namespace App.Metrics.Health
{
    /// <summary>
    ///     Top level container for all configuration settings of Health
    /// </summary>
    public class HealthOptions
    {
        public HealthOptions()
        {
            Enabled = true;
            OutputFormatters = new HealthFormatterCollection();
            Checks = new DefaultHealthCheckRegistry();
        }

        /// <summary>
        ///     Gets or sets the default <see cref="IHealthOutputFormatter" /> to use when health checks are attempted to be formatted.
        /// </summary>
        /// <value>
        ///     The default <see cref="IHealthOutputFormatter" />s that is used by this application.
        /// </value>
        public IHealthOutputFormatter DefaultOutputFormatter { get; set; }

        public IHealthCheckRegistry Checks { get; set; }

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
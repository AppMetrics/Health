// <copyright file="HealthAsciiOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Health.Formatters.Ascii
{
    public class HealthAsciiOptions
    {
        public HealthAsciiOptions()
        {
            Padding = 20;
            Separator = "=";
        }

        /// <summary>
        ///     Gets or sets the padding to apply on health check labels and messages
        /// </summary>
        /// <value>
        ///     The padding to apply on health check labels and messages
        /// </value>
        public int Padding { get; set; }

        /// <summary>
        ///     Gets or sets the separator to use between on health check labels and messages/status
        /// </summary>
        /// <value>
        ///     The separator to apply between on health check labels and messages/status
        /// </value>
        public string Separator { get; set; }
    }
}
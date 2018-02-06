// <copyright file="HealthOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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
        }

        public bool Enabled { get; set; }
    }
}
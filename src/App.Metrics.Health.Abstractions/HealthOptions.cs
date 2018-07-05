// <copyright file="HealthOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Health
{
    /// <summary>
    ///     Top level container for all configuration settings of Health
    /// </summary>
    public class HealthOptions
    {
        public HealthOptions() { Enabled = true; }

        public string ApplicationName { get; set; }

        public bool Enabled { get; set; }
    }
}
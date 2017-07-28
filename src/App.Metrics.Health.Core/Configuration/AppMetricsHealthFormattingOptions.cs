// <copyright file="AppMetricsHealthFormattingOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Text;

namespace App.Metrics.Health.Configuration
{
    public abstract class AppMetricsHealthFormattingOptions
    {
        public AppMetricsHealthFormattingOptions()
        {
            Encoding = Encoding.UTF8;
        }

        public Encoding Encoding { get; set; }
    }
}

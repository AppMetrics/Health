// <copyright file="AppMetricsHealthJsonOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Health.Configuration;
using Newtonsoft.Json;

namespace App.Metrics.Health.Formatters.Json
{
    public class AppMetricsHealthJsonOptions : AppMetricsHealthFormattingOptions
    {
        public JsonSerializerSettings SerializerSettings { get; } =
            DefaultJsonSerializerSettings.CreateSerializerSettings();
    }
}
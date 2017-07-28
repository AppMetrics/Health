// <copyright file="AppMetricsHealthJsonOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace App.Metrics.Health.Formatters.Json
{
    public class AppMetricsHealthJsonOptions
    {
        public JsonSerializerSettings SerializerSettings { get; } =
            DefaultJsonSerializerSettings.CreateSerializerSettings();
    }
}
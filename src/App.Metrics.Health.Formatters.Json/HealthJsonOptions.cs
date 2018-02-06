// <copyright file="HealthJsonOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace App.Metrics.Health.Formatters.Json
{
    public class HealthJsonOptions
    {
        public JsonSerializerSettings SerializerSettings { get; } =
            DefaultJsonSerializerSettings.CreateSerializerSettings();
    }
}
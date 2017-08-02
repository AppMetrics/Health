// <copyright file="AppMetricsHealthJsonOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Formatters.Json.Internal
{
    public class AppMetricsHealthJsonOptionsSetup : IConfigureOptions<HealthOptions>
    {
        private readonly AppMetricsHealthJsonOptions _jsonOptions;

        public AppMetricsHealthJsonOptionsSetup(IOptions<AppMetricsHealthJsonOptions> asciiOptions)
        {
            _jsonOptions = asciiOptions.Value ?? throw new ArgumentNullException(nameof(asciiOptions));
        }

        public void Configure(HealthOptions options)
        {
            var formatter = new JsonOutputFormatter(_jsonOptions.SerializerSettings);

            if (options.DefaultOutputFormatter == null)
            {
                options.DefaultOutputFormatter = formatter;
            }

            options.OutputFormatters.Add(formatter);
        }
    }
}
// <copyright file="AppMetricsHealthJsonOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Formatters.Json.Internal
{
    public class AppMetricsHealthJsonOptionsSetup : IConfigureOptions<AppMetricsHealthOptions>
    {
        private readonly AppMetricsHealthJsonOptions _asciiOptions;

        public AppMetricsHealthJsonOptionsSetup(IOptions<AppMetricsHealthJsonOptions> asciiOptions)
        {
            _asciiOptions = asciiOptions.Value ?? throw new ArgumentNullException(nameof(asciiOptions));
        }

        public void Configure(AppMetricsHealthOptions options)
        {
            options.OutputFormatters.Add(new JsonOutputFormatter(_asciiOptions.SerializerSettings));
        }
    }
}
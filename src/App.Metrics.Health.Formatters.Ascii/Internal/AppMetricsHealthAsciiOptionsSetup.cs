// <copyright file="AppMetricsHealthAsciiOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Formatters.Ascii.Internal
{
    public class AppMetricsHealthAsciiOptionsSetup : IConfigureOptions<HealthOptions>
    {
        private readonly HealthAsciiOptions _asciiOptions;

        public AppMetricsHealthAsciiOptionsSetup(IOptions<HealthAsciiOptions> asciiOptions)
        {
            _asciiOptions = asciiOptions.Value ?? throw new ArgumentNullException(nameof(asciiOptions));
        }

        public void Configure(HealthOptions options)
        {
            var formatter = new AsciiOutputFormatter(_asciiOptions);

            if (options.DefaultOutputFormatter == null)
            {
                options.DefaultOutputFormatter = formatter;
            }

            options.OutputFormatters.Add(formatter);
        }
    }
}
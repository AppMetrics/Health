// <copyright file="AppMetricsHealthAsciiOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Formatters.Ascii.Internal
{
    public class AppMetricsHealthAsciiOptionsSetup : IConfigureOptions<AppMetricsHealthOptions>
    {
        private readonly IOptions<AppMetricsHealthAsciiOptions> _asciiOptions;

        public AppMetricsHealthAsciiOptionsSetup(IOptions<AppMetricsHealthAsciiOptions> asciiOptions)
        {
            _asciiOptions = asciiOptions ?? throw new ArgumentNullException(nameof(asciiOptions));
        }

        public void Configure(AppMetricsHealthOptions options)
        {
            options.OutputFormatters.Add(new AsciiOutputFormatter(_asciiOptions.Value));
        }
    }
}
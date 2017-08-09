// <copyright file="HealthJsonOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Formatters.Json.Internal
{
    /// <summary>
    ///     Sets up default JSON options for <see cref="HealthOptions"/>.
    /// </summary>
    public class HealthJsonOptionsSetup : IConfigureOptions<HealthOptions>
    {
        private readonly HealthJsonOptions _jsonOptions;

        public HealthJsonOptionsSetup(IOptions<HealthJsonOptions> asciiOptions)
        {
            _jsonOptions = asciiOptions.Value ?? throw new ArgumentNullException(nameof(asciiOptions));
        }

        public void Configure(HealthOptions options)
        {
            var formatter = new HealthStatusJsonOutputFormatter(_jsonOptions.SerializerSettings);

            if (options.DefaultOutputFormatter == null)
            {
                options.DefaultOutputFormatter = formatter;
            }

            options.OutputFormatters.Add(formatter);
        }
    }
}
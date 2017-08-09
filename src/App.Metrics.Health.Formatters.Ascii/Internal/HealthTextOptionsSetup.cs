// <copyright file="HealthTextOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Formatters.Ascii.Internal
{
    /// <summary>
    ///     Sets up default ASCII options for <see cref="HealthOptions"/>.
    /// </summary>
    public class HealthTextOptionsSetup : IConfigureOptions<HealthOptions>
    {
        private readonly HealthTextOptions _textOptions;

        public HealthTextOptionsSetup(IOptions<HealthTextOptions> asciiOptions)
        {
            _textOptions = asciiOptions.Value ?? throw new ArgumentNullException(nameof(asciiOptions));
        }

        public void Configure(HealthOptions options)
        {
            var formatter = new HealthStatusTextOutputFormatter(_textOptions);

            if (options.DefaultOutputFormatter == null)
            {
                options.DefaultOutputFormatter = formatter;
            }

            options.OutputFormatters.Add(formatter);
        }
    }
}
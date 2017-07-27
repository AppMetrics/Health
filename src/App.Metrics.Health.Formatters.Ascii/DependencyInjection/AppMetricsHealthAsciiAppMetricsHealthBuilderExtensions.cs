﻿// <copyright file="AppMetricsHealthAsciiAppMetricsHealthBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Formatters.Ascii;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsHealthAsciiAppMetricsHealthBuilderExtensions
    {
        public static IAppMetricsHealthBuilder AddAsciiOptions(
            this IAppMetricsHealthBuilder builder,
            Action<AppMetricsHealthAsciiOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}
// <copyright file="AppMetricsHealthAsciiAppMetricsHealthCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Ascii.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsHealthAsciiAppMetricsHealthCoreBuilderExtensions
    {
        public static IHealthCoreBuilder AddAsciiFormatter(this IHealthCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddAsciiFormatterServices(builder.Services);

            return builder;
        }

        internal static void AddAsciiFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<HealthOptions>, AppMetricsHealthAsciiOptionsSetup>());
        }
    }
}

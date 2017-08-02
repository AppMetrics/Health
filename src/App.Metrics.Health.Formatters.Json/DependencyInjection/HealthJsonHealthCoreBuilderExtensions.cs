// <copyright file="HealthJsonHealthCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Json.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class HealthJsonHealthCoreBuilderExtensions
    {
        public static IHealthCoreBuilder AddJsonFormatter(this IHealthCoreBuilder builder)
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
                ServiceDescriptor.Transient<IConfigureOptions<HealthOptions>, AppMetricsHealthJsonOptionsSetup>());
        }
    }
}
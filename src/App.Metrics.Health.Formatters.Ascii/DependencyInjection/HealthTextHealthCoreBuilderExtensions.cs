// <copyright file="HealthTextHealthCoreBuilderExtensions.cs" company="Allan Hardy">
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
    public static class HealthTextHealthCoreBuilderExtensions
    {
        public static IHealthCoreBuilder AddTextFormatter(this IHealthCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddTextFormatterServices(builder.Services);

            return builder;
        }

        internal static void AddTextFormatterServices(IServiceCollection services)
        {
            var optionsSetupDescriptor = ServiceDescriptor.Transient<IConfigureOptions<HealthOptions>, HealthTextOptionsSetup>();
            services.TryAddEnumerable(optionsSetupDescriptor);
        }
    }
}

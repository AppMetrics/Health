// <copyright file="AppMetricsHealthCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsHealthCoreBuilderExtensions
    {
        public static IAppMetricsHealthCoreBuilder AddHealthOptions(
            this IAppMetricsHealthCoreBuilder builder,
            Action<AppMetricsHealthOptions> setupAction)
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

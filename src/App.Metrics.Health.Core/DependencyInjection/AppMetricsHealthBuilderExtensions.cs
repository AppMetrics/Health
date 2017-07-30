// <copyright file="AppMetricsHealthBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsHealthBuilderExtensions
    {
        public static IAppMetricsHealthBuilder AddHealthOptions(
            this IAppMetricsHealthBuilder builder,
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

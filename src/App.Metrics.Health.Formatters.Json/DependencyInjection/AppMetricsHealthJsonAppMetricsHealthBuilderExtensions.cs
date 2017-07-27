// <copyright file="AppMetricsHealthJsonAppMetricsHealthBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Formatters.Json;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsHealthJsonAppMetricsHealthBuilderExtensions
    {
        public static IAppMetricsHealthBuilder AddJsonOptions(
            this IAppMetricsHealthBuilder builder,
            Action<AppMetricsHealthJsonOptions> setupAction)
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
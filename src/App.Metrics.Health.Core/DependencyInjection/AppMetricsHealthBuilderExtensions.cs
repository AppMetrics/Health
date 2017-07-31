// <copyright file="AppMetricsHealthBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extensions for configuring App Metrics health using an <see cref="IAppMetricsHealthBuilder" />.
    /// </summary>
    public static class AppMetricsHealthBuilderExtensions
    {
        /// <summary>
        ///     Registers an action to configure <see cref="AppMetricsHealthOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthBuilder" />.</param>
        /// <param name="setupAction">An <see cref="Action{AppMetricsHealthOptions}" />.</param>
        /// <returns>The <see cref="IAppMetricsHealthBuilder" /> instance.</returns>
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

        /// <summary>
        ///     Registers an action to register health checks.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthBuilder" />.</param>
        /// <param name="setupAction">An <see cref="Action{IHealthCheckRegistry}" />.</param>
        /// <returns>The <see cref="IAppMetricsHealthBuilder" /> instance.</returns>
        public static IAppMetricsHealthBuilder AddChecks(
            this IAppMetricsHealthBuilder builder,
            Action<IHealthCheckRegistry> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction != null)
            {
                builder.Services.Configure<AppMetricsHealthOptions>(options => setupAction(options.Checks));
            }

            return builder;
        }
    }
}
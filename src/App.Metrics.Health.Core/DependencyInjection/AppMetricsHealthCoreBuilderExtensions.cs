// <copyright file="AppMetricsHealthCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extensions for configuring App Metrics health using an <see cref="IAppMetricsHealthCoreBuilder" />.
    /// </summary>
    public static class AppMetricsHealthCoreBuilderExtensions
    {
        /// <summary>
        ///     Registers an action to configure <see cref="AppMetricsHealthOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthCoreBuilder" />.</param>
        /// <param name="setupAction">An <see cref="Action{AppMetricsHealthOptions}" />.</param>
        /// <returns>The <see cref="IAppMetricsHealthCoreBuilder" /> instance.</returns>
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

        /// <summary>
        ///     Registers an action to register health checks.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthCoreBuilder" />.</param>
        /// <param name="setupAction">An <see cref="Action{IHealthCheckRegistry}" />.</param>
        /// <returns>The <see cref="IAppMetricsHealthCoreBuilder" /> instance.</returns>
        public static IAppMetricsHealthCoreBuilder AddChecks(
            this IAppMetricsHealthCoreBuilder builder,
            Action<IHealthCheckRegistry> setupAction)
        {
            if (setupAction != null)
            {
                builder.Services.Configure<AppMetricsHealthOptions>(options => setupAction(options.Checks));
            }

            return builder;
        }
    }
}

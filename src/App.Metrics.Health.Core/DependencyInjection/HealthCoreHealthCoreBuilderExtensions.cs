// <copyright file="HealthCoreHealthCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extensions for configuring App Metrics health using an <see cref="IHealthCoreBuilder" />.
    /// </summary>
    public static class HealthCoreHealthCoreBuilderExtensions
    {
        /// <summary>
        ///     Registers an action to configure <see cref="HealthOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthCoreBuilder" />.</param>
        /// <param name="setupAction">An <see cref="Action{HealthOptions}" />.</param>
        /// <returns>The <see cref="IHealthCoreBuilder" /> instance.</returns>
        public static IHealthCoreBuilder AddHealthOptions(
            this IHealthCoreBuilder builder,
            Action<HealthOptions> setupAction)
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
        /// <param name="builder">The <see cref="IHealthCoreBuilder" />.</param>
        /// <param name="setupAction">An <see cref="Action{IHealthCheckRegistry}" />.</param>
        /// <returns>The <see cref="IHealthCoreBuilder" /> instance.</returns>
        public static IHealthCoreBuilder AddChecks(
            this IHealthCoreBuilder builder,
            Action<IHealthCheckRegistry> setupAction)
        {
            if (setupAction != null)
            {
                builder.Services.Configure<HealthOptions>(options => setupAction(options.Checks));
            }

            return builder;
        }
    }
}

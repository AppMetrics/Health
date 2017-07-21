// <copyright file="HealthChecksServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Health.DependencyInjection;
using App.Metrics.Health.Internal.Extensions;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class HealthChecksServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupChecksAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// <returns>The health check checksBuilder</returns>
        public static IAppMetricsHealthChecksBuilder AddHealth(
            this IServiceCollection services,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            var builder = services.AddHealthBuilder();

            builder.AddRequiredPlatformServices();
            builder.AddCoreServices(setupChecksAction);

            return builder;
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load <see cref="AppMetricsHealthOptions">options</see>.
        /// </param>
        /// <param name="setupChecksAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// <returns>The health check checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthChecksBuilder AddHealth(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            services.Configure<AppMetricsHealthOptions>(configuration);

            return services.AddHealth(setupChecksAction);
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupOptionsAction">The <see cref="AppMetricsHealthOptions">options</see> setup action.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load
        ///     <see cref="AppMetricsHealthOptions">options</see>. Any shared configuration options with the options delegate will be
        ///     overridden by using this configuration.
        /// </param>
        /// <param name="setupChecksAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// <returns>The health check checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthChecksBuilder AddHealth(
            this IServiceCollection services,
            Action<AppMetricsHealthOptions> setupOptionsAction,
            IConfiguration configuration,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            services.Configure(setupOptionsAction);
            services.Configure<AppMetricsHealthOptions>(configuration);

            return services.AddHealth(setupChecksAction);
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load
        ///     <see cref="AppMetricsHealthOptions">options</see>.
        /// </param>
        /// <param name="setupOptionsAction">The <see cref="AppMetricsHealthOptions">options</see> setup action.</param>
        /// <param name="setupChecksAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// Any shared configuration options with the options IConfiguration will be overriden by the options delegate.
        /// <returns>The health check checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthChecksBuilder AddHealth(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsHealthOptions> setupOptionsAction,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            services.Configure<AppMetricsHealthOptions>(configuration);
            services.Configure(setupOptionsAction);

            return services.AddHealth(setupChecksAction);
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupOptionsAction">The <see cref="AppMetricsHealthOptions">options</see> setup action.</param>
        /// <param name="setupChecksAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// <returns>The health check checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthChecksBuilder AddHealth(
            this IServiceCollection services,
            Action<AppMetricsHealthOptions> setupOptionsAction,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            services.Configure(setupOptionsAction);

            return services.AddHealth(setupChecksAction);
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <returns>The health check checksBuilder</returns>
        private static IAppMetricsHealthChecksBuilder AddHealthBuilder(this IServiceCollection services) { return new AppMetricsHealthChecksBuilder(services); }
    }
}
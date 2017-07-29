// <copyright file="HealthChecksServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
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
        /// <returns>The health check builder</returns>
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            var startupAssemblyName = setupChecksAction == null ? Assembly.GetEntryAssembly().GetName().Name : setupChecksAction.GetMethodInfo().DeclaringType.GetTypeInfo().Assembly.GetName().Name;

#pragma warning disable CS0612
            return services.AddHealth(startupAssemblyName, setupChecksAction);
#pragma warning restore CS0612
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        ///     DEVNOTE: Workaround for https://github.com/Microsoft/vstest/issues/649
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="startupAssemblyName">The name of the assembly containing the startup type.</param>
        /// <param name="setupChecksAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// <returns>The health check builder</returns>
        [Obsolete]
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            string startupAssemblyName,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            var builder = services.AddHealthCore(startupAssemblyName, setupChecksAction);

            // Add default framework
            builder.AddAsciiFormatter();
            builder.AddJsonFormatter();

            return new AppMetricsHealthBuilder(builder.Services);
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load <see cref="AppMetricsHealthOptions">options</see>.
        /// </param>
        /// <param name="setupChecksAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// <returns>The health check builder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthBuilder AddHealth(
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
        /// <returns>The health check builder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthBuilder AddHealth(
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
        /// <returns>The health check builder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthBuilder AddHealth(
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
        /// <returns>The health check builder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            Action<AppMetricsHealthOptions> setupOptionsAction,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            services.Configure(setupOptionsAction);

            return services.AddHealth(setupChecksAction);
        }
    }
}
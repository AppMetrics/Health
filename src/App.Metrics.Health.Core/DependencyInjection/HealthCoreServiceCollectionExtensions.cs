// <copyright file="HealthCoreServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using App.Metrics.Health;
using App.Metrics.Health.DependencyInjection.Internal;
using App.Metrics.Health.Internal;
using App.Metrics.Health.Internal.NoOp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up essential App Metrics health services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class HealthCoreServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthOptions}" /> to configure the provided
        ///     <see cref="HealthOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            Action<HealthOptions> setupAction)
        {
            var builder = services.AddHealthCore();

            services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IHealthCoreBuilder AddHealthCore(this IServiceCollection services)
        {
            return services.AddHealthCore(Assembly.GetEntryAssembly().GetName().Name);
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="startupAssemblyName">The name of this application's entry assembly.</param>
        /// <returns>
        ///     An <see cref="IHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            string startupAssemblyName)
        {
            AddHealthCoreServices(services, startupAssemblyName);

            return new HealthCoreBuilder(services);
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="startupAssemblyName">The name of this application's entry assembly.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthOptions}" /> to configure the provided
        ///     <see cref="HealthOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            string startupAssemblyName,
            Action<HealthOptions> setupAction)
        {
            var builder = services.AddHealthCore(startupAssemblyName);

            services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="HealthOptions" />
        ///     .
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var builder = services.AddHealthCore();

            services.Configure<HealthOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthOptions}" /> to configure the provided
        ///     <see cref="HealthOptions" />.
        /// </param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="HealthOptions" />
        ///     .
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            Action<HealthOptions> setupAction,
            IConfiguration configuration)
        {
            var builder = services.AddHealthCore();

            services.Configure(setupAction);
            services.Configure<HealthOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="HealthOptions" />.
        /// </param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthOptions}" /> to configure the provided
        ///     <see cref="HealthOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<HealthOptions> setupAction)
        {
            var buidler = services.AddHealthCore();

            services.Configure<HealthOptions>(configuration);
            services.Configure(setupAction);

            return buidler;
        }

        internal static void AddHealthCoreServices(IServiceCollection services, string startupAssemblyName)
        {
            HealthChecksAsServices.AddHealthChecksAsServices(
                services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(startupAssemblyName));

            services.TryAddSingleton<HealthCheckMarkerService>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<HealthOptions>, HealthOptionsSetup>());

            services.TryAddSingleton<IProvideHealth>(
                provider =>
                {
                    var options = provider.GetRequiredService<IOptions<HealthOptions>>();

                    if (!options.Value.Enabled)
                    {
                        return new NoOpHealthProvider();
                    }

                    return new DefaultHealthProvider(provider.GetRequiredService<ILogger<DefaultHealthProvider>>(), options.Value.Checks);
                });
        }
    }
}
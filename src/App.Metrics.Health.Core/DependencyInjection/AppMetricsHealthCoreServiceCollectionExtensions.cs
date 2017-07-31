// <copyright file="AppMetricsHealthCoreServiceCollectionExtensions.cs" company="Allan Hardy">
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
    public static class AppMetricsHealthCoreServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{AppMetricsHealthOptions}" /> to configure the provided
        ///     <see cref="AppMetricsHealthOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IAppMetricsHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IAppMetricsHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            Action<AppMetricsHealthOptions> setupAction)
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
        ///     An <see cref="IAppMetricsHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IAppMetricsHealthCoreBuilder AddHealthCore(this IServiceCollection services)
        {
            return services.AddHealthCore(Assembly.GetEntryAssembly().GetName().Name);
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="startupAssemblyName">The name of this application's entry assembly.</param>
        /// <returns>
        ///     An <see cref="IAppMetricsHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IAppMetricsHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            string startupAssemblyName)
        {
            AddHealthCoreServices(services, startupAssemblyName);

            return new AppMetricsHealthCoreBuilder(services);
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="startupAssemblyName">The name of this application's entry assembly.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{AppMetricsHealthOptions}" /> to configure the provided
        ///     <see cref="AppMetricsHealthOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IAppMetricsHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IAppMetricsHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            string startupAssemblyName,
            Action<AppMetricsHealthOptions> setupAction)
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
        ///     The <see cref="IConfiguration" /> from where to load <see cref="AppMetricsHealthOptions" />
        ///     .
        /// </param>
        /// <returns>
        ///     An <see cref="IAppMetricsHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IAppMetricsHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var builder = services.AddHealthCore();

            services.Configure<AppMetricsHealthOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{AppMetricsHealthOptions}" /> to configure the provided
        ///     <see cref="AppMetricsHealthOptions" />.
        /// </param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="AppMetricsHealthOptions" />
        ///     .
        /// </param>
        /// <returns>
        ///     An <see cref="IAppMetricsHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IAppMetricsHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            Action<AppMetricsHealthOptions> setupAction,
            IConfiguration configuration)
        {
            var builder = services.AddHealthCore();

            services.Configure(setupAction);
            services.Configure<AppMetricsHealthOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="AppMetricsHealthOptions" />.
        /// </param>
        /// <param name="setupAction">
        ///     An <see cref="Action{AppMetricsHealthOptions}" /> to configure the provided
        ///     <see cref="AppMetricsHealthOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IAppMetricsHealthCoreBuilder" /> that can be used to further configure the App Metrics health
        ///     services.
        /// </returns>
        public static IAppMetricsHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsHealthOptions> setupAction)
        {
            var buidler = services.AddHealthCore();

            services.Configure<AppMetricsHealthOptions>(configuration);
            services.Configure(setupAction);

            return buidler;
        }

        internal static void AddHealthCoreServices(IServiceCollection services, string startupAssemblyName)
        {
            HealthChecksAsServices.AddHealthChecksAsServices(
                services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(startupAssemblyName));

            services.TryAddSingleton<HealthCheckMarkerService>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<AppMetricsHealthOptions>, AppMetricsHealthOptionsSetup>());

            services.TryAddSingleton<IProvideHealth>(
                provider =>
                {
                    var options = provider.GetRequiredService<IOptions<AppMetricsHealthOptions>>();

                    if (!options.Value.Enabled)
                    {
                        return new NoOpHealthProvider();
                    }

                    return new DefaultHealthProvider(provider.GetRequiredService<ILogger<DefaultHealthProvider>>(), options.Value.Checks);
                });
        }
    }
}
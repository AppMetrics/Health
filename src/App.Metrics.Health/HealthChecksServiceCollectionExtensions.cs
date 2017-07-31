// <copyright file="HealthChecksServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using App.Metrics.Health;
using App.Metrics.Health.Internal;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension methods for setting up App Metrics health services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class HealthChecksServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="IAppMetricsHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IAppMetricsHealthBuilder AddHealth(this IServiceCollection services)
        {
            return services.AddHealth(Assembly.GetEntryAssembly().GetName().Name);
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="startupAssemblyName">The name of this application's entry assembly.</param>
        /// <returns>An <see cref="IAppMetricsHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            string startupAssemblyName)
        {
            var builder = services.AddHealthCore(startupAssemblyName);

            // Add default framework
            builder.AddJsonFormatter();
            builder.AddAsciiFormatter();

            return new AppMetricsHealthBuilder(builder.Services);
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="startupAssemblyName">The name of this application's entry assembly.</param>
        /// <param name="setupAction">An <see cref="Action{AppMetricsHealthOptions}"/> to configure the provided <see cref="AppMetricsHealthOptions"/>.</param>
        /// <returns>An <see cref="IAppMetricsHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            string startupAssemblyName,
            Action<AppMetricsHealthOptions> setupAction)
        {
            var builder = services.AddHealth(startupAssemblyName);

            services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from where to load <see cref="AppMetricsHealthOptions"/>.</param>
        /// <returns>An <see cref="IAppMetricsHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var builder = services.AddHealth();

            services.Configure<AppMetricsHealthOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{AppMetricsHealthOptions}"/> to configure the provided <see cref="AppMetricsHealthOptions"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from where to load <see cref="AppMetricsHealthOptions"/>.</param>
        /// <returns>An <see cref="IAppMetricsHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            Action<AppMetricsHealthOptions> setupAction,
            IConfiguration configuration)
        {
            var builder = services.AddHealth();

            services.Configure(setupAction);
            services.Configure<AppMetricsHealthOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from where to load <see cref="AppMetricsHealthOptions"/>.</param>
        /// <param name="setupAction">An <see cref="Action{AppMetricsHealthOptions}"/> to configure the provided <see cref="AppMetricsHealthOptions"/>.</param>
        /// <returns>An <see cref="IAppMetricsHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsHealthOptions> setupAction)
        {
            var buidler = services.AddHealth();

            services.Configure<AppMetricsHealthOptions>(configuration);
            services.Configure(setupAction);

            return buidler;
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{AppMetricsHealthOptions}"/> to configure the provided <see cref="AppMetricsHealthOptions"/>.</param>
        /// <returns>An <see cref="IAppMetricsHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IAppMetricsHealthBuilder AddHealth(
            this IServiceCollection services,
            Action<AppMetricsHealthOptions> setupAction)
        {
            var builder = services.AddHealth();

            services.Configure(setupAction);

            return builder;
        }
    }
}
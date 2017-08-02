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
        /// <returns>An <see cref="IHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IHealthBuilder AddHealth(this IServiceCollection services)
        {
            return services.AddHealth(Assembly.GetEntryAssembly().GetName().Name);
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="startupAssemblyName">The name of this application's entry assembly.</param>
        /// <returns>An <see cref="IHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IHealthBuilder AddHealth(
            this IServiceCollection services,
            string startupAssemblyName)
        {
            var builder = services.AddHealthCore(startupAssemblyName);

            // Add default framework
            builder.AddJsonFormatter();
            builder.AddAsciiFormatter();

            return new HealthBuilder(builder.Services);
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="startupAssemblyName">The name of this application's entry assembly.</param>
        /// <param name="setupAction">An <see cref="Action{HealthOptions}"/> to configure the provided <see cref="HealthOptions"/>.</param>
        /// <returns>An <see cref="IHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IHealthBuilder AddHealth(
            this IServiceCollection services,
            string startupAssemblyName,
            Action<HealthOptions> setupAction)
        {
            var builder = services.AddHealth(startupAssemblyName);

            services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from where to load <see cref="HealthOptions"/>.</param>
        /// <returns>An <see cref="IHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IHealthBuilder AddHealth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var builder = services.AddHealth();

            services.Configure<HealthOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{HealthOptions}"/> to configure the provided <see cref="HealthOptions"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from where to load <see cref="HealthOptions"/>.</param>
        /// <returns>An <see cref="IHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IHealthBuilder AddHealth(
            this IServiceCollection services,
            Action<HealthOptions> setupAction,
            IConfiguration configuration)
        {
            var builder = services.AddHealth();

            services.Configure(setupAction);
            services.Configure<HealthOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from where to load <see cref="HealthOptions"/>.</param>
        /// <param name="setupAction">An <see cref="Action{HealthOptions}"/> to configure the provided <see cref="HealthOptions"/>.</param>
        /// <returns>An <see cref="IHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IHealthBuilder AddHealth(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<HealthOptions> setupAction)
        {
            var buidler = services.AddHealth();

            services.Configure<HealthOptions>(configuration);
            services.Configure(setupAction);

            return buidler;
        }

        /// <summary>
        ///     Adds the health check services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{HealthOptions}"/> to configure the provided <see cref="HealthOptions"/>.</param>
        /// <returns>An <see cref="IHealthBuilder"/> that can be used to further configure the App Metrics health services.</returns>
        public static IHealthBuilder AddHealth(
            this IServiceCollection services,
            Action<HealthOptions> setupAction)
        {
            var builder = services.AddHealth();

            services.Configure(setupAction);

            return builder;
        }
    }
}
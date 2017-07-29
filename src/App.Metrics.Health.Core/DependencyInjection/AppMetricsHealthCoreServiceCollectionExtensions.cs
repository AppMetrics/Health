// <copyright file="AppMetricsHealthCoreServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Metrics.Health;
using App.Metrics.Health.DependencyInjection.Internal;
using App.Metrics.Health.Internal;
using App.Metrics.Health.Internal.NoOp;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsHealthCoreServiceCollectionExtensions
    {
        public static IAppMetricsHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            var startupAssemblyName = setupChecksAction == null ? Assembly.GetEntryAssembly().GetName().Name : setupChecksAction.GetMethodInfo().DeclaringType.GetTypeInfo().Assembly.GetName().Name;

            return services.AddHealthCore(startupAssemblyName, setupChecksAction);
        }

        public static IAppMetricsHealthCoreBuilder AddHealthCore(
            this IServiceCollection services,
            string startupAssemblyName,
            Action<IHealthCheckRegistry> setupChecksAction = null)
        {
            HealthChecksAsServices.AddHealthChecksAsServices(
                services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(startupAssemblyName));

            services.TryAddSingleton<HealthCheckMarkerService>();
            services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsHealthOptions>>().Value);
            services.TryAddSingleton<IConfigureOptions<AppMetricsHealthOptions>, AppMetricsHealthOptionsSetup>();
            services.Replace(ServiceDescriptor.Singleton(provider => RegisterHealthCheckRegistry(provider, setupChecksAction)));

            services.TryAddSingleton<IProvideHealth>(
                              provider =>
                              {
                                  var options = provider.GetRequiredService<AppMetricsHealthOptions>();

                                  if (!options.Enabled)
                                  {
                                      return new NoOpHealthProvider();
                                  }

                                  return new DefaultHealthProvider(
                                      provider.GetRequiredService<ILogger<DefaultHealthProvider>>(),
                                      provider.GetRequiredService<IHealthCheckRegistry>());
                              });

            return new AppMetricsHealthCoreBuilder(services);
        }

        private static IHealthCheckRegistry RegisterHealthCheckRegistry(
            IServiceProvider provider,
            Action<IHealthCheckRegistry> setupAction = null)
        {
            var options = provider.GetRequiredService<AppMetricsHealthOptions>();

            if (!options.Enabled)
            {
                return new NoOpHealthCheckRegistry();
            }

            var logFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = logFactory.CreateLogger<DefaultHealthCheckRegistry>();

            var autoScannedHealthChecks = Enumerable.Empty<HealthCheck>();

            try
            {
                autoScannedHealthChecks = provider.GetRequiredService<IEnumerable<HealthCheck>>();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(
                    new EventId(5000),
                    ex,
                    "Failed to load auto scanned health checks, health checks won't be registered");
            }

            var factory = new DefaultHealthCheckRegistry(autoScannedHealthChecks);
            setupAction?.Invoke(factory);
            return factory;
        }
    }
}
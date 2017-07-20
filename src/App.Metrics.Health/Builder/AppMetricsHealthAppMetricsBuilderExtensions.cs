// <copyright file="AppMetricsHealthAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Health.Configuration;
using App.Metrics.Health.DependencyInjection.Internal;
using App.Metrics.Health.Internal;
using App.Metrics.Health.Internal.NoOp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Builder
{
    public static class AppMetricsHealthAppMetricsBuilderExtensions
    {
        internal static void AddCoreServices(
            this IAppMetricsHealthChecksBuilder checksBuilder,
            Action<IHealthCheckRegistry> setupAction = null)
        {
            HealthChecksAsServices.AddHealthChecksAsServices(
                checksBuilder.Services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(checksBuilder.Environment.ApplicationName));

            checksBuilder.Services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsHealthOptions>>().Value);
            checksBuilder.Services.TryAddSingleton<IConfigureOptions<AppMetricsHealthOptions>, ConfigureAppMetricsHealthOptions>();
            checksBuilder.Services.Replace(ServiceDescriptor.Singleton(provider => RegisterHealthCheckRegistry(provider, setupAction)));

            checksBuilder.Services.TryAddSingleton<IProvideHealth>(
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
        }

        internal static void AddRequiredPlatformServices(this IAppMetricsHealthChecksBuilder checksBuilder)
        {
            checksBuilder.Services.TryAddSingleton<HealthCheckMarkerService>();
            checksBuilder.Services.AddOptions();
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
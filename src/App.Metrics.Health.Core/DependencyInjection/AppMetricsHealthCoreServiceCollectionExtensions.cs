// <copyright file="AppMetricsHealthCoreServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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
            this IServiceCollection services)
        {
            var startupAssemblyName = Assembly.GetEntryAssembly().GetName().Name;

            return services.AddHealthCore(startupAssemblyName);
        }

        public static IAppMetricsHealthCoreBuilder AddHealthCore(this IServiceCollection services, string startupAssemblyName)
        {
            HealthChecksAsServices.AddHealthChecksAsServices(
                services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(startupAssemblyName));

            services.TryAddSingleton<HealthCheckMarkerService>();
            services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsHealthOptions>>().Value);
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<AppMetricsHealthOptions>, AppMetricsHealthOptionsSetup>());

            services.TryAddSingleton<IProvideHealth>(
                              provider =>
                              {
                                  var options = provider.GetRequiredService<AppMetricsHealthOptions>();

                                  if (!options.Enabled)
                                  {
                                      return new NoOpHealthProvider();
                                  }

                                  return new DefaultHealthProvider(provider.GetRequiredService<ILogger<DefaultHealthProvider>>(), options.Checks);
                              });

            return new AppMetricsHealthCoreBuilder(services);
        }
    }
}
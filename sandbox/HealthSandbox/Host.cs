// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Builder;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HealthSandbox
{
    public static class Host
    {
        public static IConfigurationRoot Configuration { get; set; }

        // public static async Task Main(string[] args)
        public static void Main(string[] args)
        {
            Init();

            IServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            var provider = serviceCollection.BuildServiceProvider();

            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            Console.WriteLine("Press ESC to stop");

            while (true)
            {
                while (!Console.KeyAvailable)
                {
                    var healthStatus = healthProvider.ReadStatusAsync().GetAwaiter().GetResult();
                    var formatter = new HealthStatusPayloadFormatter();
                    var payloadBuilder = new AsciiHealthStatusPayloadBuilder();
                    formatter.Build(healthStatus, payloadBuilder);
                    Console.WriteLine(payloadBuilder.PayloadFormatted());
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }

                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddLogging();

            services.AddHealthChecks(Configuration.GetSection("AppMetricsHealthOptions")).AddChecks(
                registry =>
                {
                    registry.AddProcessPrivateMemorySizeCheck("Private Memory Size", 200);
                    registry.AddProcessVirtualMemorySizeCheck("Virtual Memory Size", 200);
                    registry.AddProcessPhysicalMemoryCheck("Working Set", 200);

                    registry.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                    registry.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));

                    registry.Register(
                        "DatabaseConnected",
                        () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK")));
                    registry.Register(
                        "DiskSpace",
                        () =>
                        {
                            var freeDiskSpace = GetFreeDiskSpace();

                            return new ValueTask<HealthCheckResult>(
                                freeDiskSpace <= 512
                                    ? HealthCheckResult.Unhealthy("Not enough disk space: {0}", freeDiskSpace)
                                    : HealthCheckResult.Unhealthy("Disk space ok: {0}", freeDiskSpace));
                        });
                });
        }

        private static int GetFreeDiskSpace() { return 1024; }

        private static void Init()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }
    }
}
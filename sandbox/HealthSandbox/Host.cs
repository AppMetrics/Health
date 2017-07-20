// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            RunUntilEsc(
                TimeSpan.FromSeconds(5),
                () =>
                {
                    var healthStatus = healthProvider.ReadStatusAsync().GetAwaiter().GetResult();
                    var formatter = new HealthStatusPayloadFormatter();
                    var payloadBuilder = new AsciiHealthStatusPayloadBuilder();
                    formatter.Build(healthStatus, payloadBuilder);
                    Console.WriteLine(payloadBuilder.PayloadFormatted());
                });
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddHealth(
                Configuration.GetSection("AppMetricsHealthOptions"),
                checksRegistry =>
                {
                    checksRegistry.AddProcessPrivateMemorySizeCheck("Private Memory Size", 200);
                    checksRegistry.AddProcessVirtualMemorySizeCheck("Virtual Memory Size", 200);
                    checksRegistry.AddProcessPhysicalMemoryCheck("Working Set", 200);

                    checksRegistry.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                    checksRegistry.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));

                    checksRegistry.AddCheck(
                        "DatabaseConnected",
                        () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK")));

                    checksRegistry.AddCheck(
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

        private static void RunUntilEsc(TimeSpan delayBetweenRun, Action action)
        {
            Console.WriteLine("Press ESC to stop");

            while (true)
            {
                while (!Console.KeyAvailable)
                {
                    action();
                    Thread.Sleep(delayBetweenRun);
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
    }
}
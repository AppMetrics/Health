﻿// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            var healthOptionsAccessor = provider.GetRequiredService<IOptions<AppMetricsHealthOptions>>();

            var cancellationTokenSource = new CancellationTokenSource();

            RunUntilEsc(
                TimeSpan.FromSeconds(10),
                cancellationTokenSource,
                () =>
                {
                    Console.Clear();

                    var healthStatus = healthProvider.ReadAsync(cancellationTokenSource.Token).GetAwaiter().GetResult();

                    foreach (var formatter in healthOptionsAccessor.Value.OutputFormatters)
                    {
                        Console.WriteLine($"Formatter: {formatter.GetType().FullName}");
                        Console.WriteLine("-------------------------------------------");

                        using (var stream = new MemoryStream())
                        {
                            formatter.WriteAsync(stream, healthStatus, Encoding.UTF8, cancellationTokenSource.Token).GetAwaiter().GetResult();

                            var result = Encoding.UTF8.GetString(stream.ToArray());

                            Console.WriteLine(result);
                        }
                    }
                });
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            // To add additional formatters
            // options => options.OutputFormatters.Add(new AsciiOutputFormatter()),

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
                })
                .AddAsciiOptions(options => { options.Separator = ":"; })
                .AddJsonOptions(options => { });
        }

        private static int GetFreeDiskSpace() { return 1024; }

        private static void Init()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        private static void RunUntilEsc(TimeSpan delayBetweenRun, CancellationTokenSource cancellationTokenSource, Action action)
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
                        cancellationTokenSource.Cancel();
                        return;
                    }
                }
            }
        }
    }
}
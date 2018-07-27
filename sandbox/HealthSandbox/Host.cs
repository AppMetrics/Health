// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Health.Checks.Sql;
using HealthSandbox.HealthChecks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Serilog;
using static System.Console;

namespace HealthSandbox
{
    public static class Host
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static IHealthRoot Health { get; set; }

        public static IMetricsRoot Metrics { get; set; }

        public static async Task Main()
        {
            Init();

            var cancellationTokenSource = new CancellationTokenSource();

            await RunUntilEscAsync(
                TimeSpan.FromSeconds(20),
                cancellationTokenSource,
                async () =>
                {
                    Clear();

                    var healthStatus = await Health.HealthCheckRunner.ReadAsync(cancellationTokenSource.Token);

                    foreach (var formatter in Health.OutputHealthFormatters)
                    {
                        WriteLine($"Formatter: {formatter.GetType().FullName}");
                        WriteLine("-------------------------------------------");

                        using (var stream = new MemoryStream())
                        {
                            await formatter.WriteAsync(stream, healthStatus, cancellationTokenSource.Token);

                            var result = Encoding.UTF8.GetString(stream.ToArray());

                            WriteLine(result);
                        }
                    }

                    foreach (var reporter in Health.Reporters)
                    {
                        WriteLine($"Reporter: {reporter.GetType().FullName}");
                        WriteLine("-------------------------------------------");

                        await reporter.ReportAsync(Health.Options, healthStatus, cancellationTokenSource.Token);
                    }
                });
        }

        private static void Init()
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.LiterateConsole()
                         .WriteTo.Seq("http://localhost:5341")
                         .CreateLogger();

            var healthOptionsDictionary = Configuration.GetSection(nameof(HealthOptions)).GetChildren().ToDictionary(x => $"{nameof(HealthOptions)}:{x.Key}", x => x.Value);

            Metrics = AppMetrics.CreateDefaultBuilder().Build();

            Health = AppMetricsHealth.CreateDefaultBuilder()
                                     .Configuration.Configure(healthOptionsDictionary)
                                     .Report.ToGrafanaAnnotation(
                                         options =>
                                         {
                                             options.AnnotationEndpoint = new Uri("http://localhost:3000/api/annotations");
                                             options.Token =
                                                 "xxx";
                                             options.Tags = new List<string> { "test" };
                                         })
                                     .Report.ToMetrics(Metrics)
                                     .HealthChecks.AddCheck(new SampleHealthCheck())
                                      .Build();
        }

        private static async Task RunUntilEscAsync(TimeSpan delayBetweenRun, CancellationTokenSource cancellationTokenSource, Func<Task> action)
        {
            WriteLine("Press ESC to stop");

            while (true)
            {
                while (!KeyAvailable)
                {
                    await action();
                    Thread.Sleep(delayBetweenRun);
                }

                while (KeyAvailable)
                {
                    var key = ReadKey(false).Key;

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
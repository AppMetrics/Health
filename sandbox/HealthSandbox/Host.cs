// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
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
        private static readonly string ConnectionString = "Data Source=DBHealthCheck;Mode=Memory;Cache=Shared";

        public static IConfigurationRoot Configuration { get; set; }

        public static IHealthRoot Health { get; set; }

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

            Health = AppMetricsHealth.CreateDefaultBuilder()
                .HealthChecks.AddCheck(new SampleHealthCheck())
                .HealthChecks.AddCheck(new SampleCachedHealthCheck())
                .HealthChecks.AddProcessPrivateMemorySizeCheck("Private Memory Size", 200)
                .HealthChecks.AddProcessVirtualMemorySizeCheck("Virtual Memory Size", 200)
                .HealthChecks.AddProcessPhysicalMemoryCheck("Working Set", 200)
                .HealthChecks.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10))
                .HealthChecks.AddPingCachedCheck("google ping cached", "google.com", TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1))
                .HealthChecks.AddHttpGetCachedCheck("invalid http cached", new Uri("https://invalid-asdfadsf-cached.com/"), 3, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1))
                .HealthChecks.AddHttpGetCheck("invalid http", new Uri("https://invalid-asdfadsf.com/"), 3, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1))
                .HealthChecks.AddHttpGetCheck("github", new Uri("https://github.com/"), retries: 3, delayBetweenRetries: TimeSpan.FromMilliseconds(100), timeoutPerRequest: TimeSpan.FromSeconds(5))
                .HealthChecks.AddHttpGetCheck("google", new Uri("https://google.com/"), TimeSpan.FromSeconds(1))
                .HealthChecks.AddCheck("DatabaseConnected", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK")))
                .HealthChecks.AddCachedCheck(
                    "DiskSpace",
                    () =>
                    {
                        var freeDiskSpace = GetFreeDiskSpace();
                        return new ValueTask<HealthCheckResult>(
                            freeDiskSpace <= 512
                                ? HealthCheckResult.Unhealthy("Not enough disk space: {0}", freeDiskSpace)
                                : HealthCheckResult.Unhealthy("Disk space ok: {0}", freeDiskSpace));
                    },
                    cacheDuration: TimeSpan.FromMinutes(1))
                .HealthChecks.AddSqlCheck("DB Connection", () => new SqliteConnection(ConnectionString), TimeSpan.FromSeconds(10))
                .HealthChecks.AddSqlCachedCheck("DB Connection Cached", () => new SqliteConnection(ConnectionString), TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1))
                .Build();

            int GetFreeDiskSpace()
            {
                return 1024;
            }
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
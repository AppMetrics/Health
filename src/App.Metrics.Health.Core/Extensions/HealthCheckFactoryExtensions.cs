// <copyright file="HealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class HealthCheckFactoryExtensions
    {
        private static readonly HttpClient HttpClient = new HttpClient { DefaultRequestHeaders = { { "cache-control", "no-cache" } } };

        public static IHealthCheckRegistry AddHttpGetCheck(
            this IHealthCheckRegistry registry,
            string name,
            Uri uri,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            registry.AddCheck(
                name,
                async cancellationToken =>
                {
                    try
                    {
                        using (var tokenWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                        {
                            tokenWithTimeout.CancelAfter(timeout);

                            var response = await HttpClient.GetAsync(uri, tokenWithTimeout.Token).ConfigureAwait(false);

                            return response.IsSuccessStatusCode
                                ? HealthCheckResult.Healthy($"OK. {uri}")
                                : HealthCheckResultOnError($"FAILED. {uri} status code was {response.StatusCode}", degradedOnError);
                        }
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? HealthCheckResult.Degraded(ex)
                            : HealthCheckResult.Unhealthy(ex);
                    }
                });

            return registry;
        }

        public static IHealthCheckRegistry AddPingCheck(
            this IHealthCheckRegistry registry,
            string name,
            string host,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            registry.AddCheck(
                name,
                async () =>
                {
                    try
                    {
                        var ping = new Ping();
                        var result = await ping.SendPingAsync(host, (int)timeout.TotalMilliseconds).ConfigureAwait(false);

                        return result.Status == IPStatus.Success
                            ? HealthCheckResult.Healthy($"OK. {host}")
                            : HealthCheckResultOnError($"FAILED. {host} ping result was {result.Status}", degradedOnError);
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? HealthCheckResult.Degraded(ex)
                            : HealthCheckResult.Unhealthy(ex);
                    }
                });

            return registry;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of physical memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="registry">The health check registry where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The physical memory threshold in bytes.</param>
        /// <param name="degradedOnError">Return a degraded status instead of unhealthy on error.</param>
        /// <returns>The health check registry instance</returns>
        public static IHealthCheckRegistry AddProcessPhysicalMemoryCheck(
            this IHealthCheckRegistry registry,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            registry.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        var currentSize = Process.GetCurrentProcess().WorkingSet64;
                        return new ValueTask<HealthCheckResult>(
                            currentSize <= thresholdBytes
                                ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                                : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes}", degradedOnError));
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(ex))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(ex));
                    }
                });

            return registry;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of private memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="registry">The health check registry where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The private memory threshold in bytes.</param>
        /// <param name="degradedOnError">Return a degraded status instead of unhealthy on error.</param>
        /// <returns>The health check registry instance</returns>
        public static IHealthCheckRegistry AddProcessPrivateMemorySizeCheck(
            this IHealthCheckRegistry registry,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            registry.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        var currentSize = Process.GetCurrentProcess().PrivateMemorySize64;
                        return new ValueTask<HealthCheckResult>(
                            currentSize <= thresholdBytes
                                ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                                : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes} bytes", degradedOnError));
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(ex))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(ex));
                    }
                });

            return registry;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of virtual memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="registry">The health check registry where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The virtual memory threshold in bytes.</param>
        /// <param name="degradedOnError">Return a degraded status instead of unhealthy on error.</param>
        /// <returns>The health check registry instance</returns>
        public static IHealthCheckRegistry AddProcessVirtualMemorySizeCheck(
            this IHealthCheckRegistry registry,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            registry.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        var currentSize = Process.GetCurrentProcess().VirtualMemorySize64;
                        return new ValueTask<HealthCheckResult>(
                            currentSize <= thresholdBytes
                                ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                                : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes} bytes", degradedOnError));
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(ex))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(ex));
                    }
                });

            return registry;
        }

        /// <summary>
        ///     Create a failure (degraded or unhealthy) status response.
        /// </summary>
        /// <param name="message">Status message.</param>
        /// <param name="degradedOnError">
        ///     If true, create a degraded status response.
        ///     Otherwise create an unhealthy status response. (default: false)
        /// </param>
        /// <returns>Failure status response.</returns>
        private static HealthCheckResult HealthCheckResultOnError(string message, bool degradedOnError)
        {
            return degradedOnError
                ? HealthCheckResult.Degraded(message)
                : HealthCheckResult.Unhealthy(message);
        }
    }
}
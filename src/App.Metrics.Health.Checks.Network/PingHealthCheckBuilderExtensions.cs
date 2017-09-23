// <copyright file="PingHealthCheckBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.NetworkInformation;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class PingHealthCheckBuilderExtensions
    {
        public static IHealthBuilder AddPingCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            string host,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCheck(
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

            return healthCheckBuilder.Builder;
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

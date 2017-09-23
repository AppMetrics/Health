// <copyright file="HttpHealthCheckBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Threading;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class HttpHealthCheckBuilderExtensions
    {
        private static readonly HttpClient HttpClient = new HttpClient { DefaultRequestHeaders = { { "cache-control", "no-cache" } } };

        public static IHealthBuilder AddHttpGetCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Uri uri,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCheck(
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

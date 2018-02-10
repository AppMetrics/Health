// <copyright file="HttpHealthCheckBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class HttpHealthCheckBuilderExtensions
    {
        private static readonly ILog Logger = LogProvider.For<IRunHealthChecks>();
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
                    var sw = new Stopwatch();

                    try
                    {
                        using (var tokenWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                        {
                            tokenWithTimeout.CancelAfter(timeout);

                            sw.Start();

                            var response = await HttpClient.GetAsync(uri, tokenWithTimeout.Token).ConfigureAwait(false);

                            return response.IsSuccessStatusCode
                                ? HealthCheckResult.Healthy($"OK. '{uri}' success. Time taken: {sw.ElapsedMilliseconds}ms.")
                                : HealthCheckResultOnError(
                                    $"FAILED. '{uri}' status code was {response.StatusCode}. Time taken: {sw.ElapsedMilliseconds}ms.",
                                    degradedOnError);
                        }
                    }
                    catch (Exception ex) when (ex is TaskCanceledException)
                    {
                        Logger.ErrorException($"HTTP Health Check '{uri}' did not respond within '{timeout.TotalMilliseconds}'ms.", ex);

                        return HealthCheckResultOnError($"FAILED. '{uri}' did not respond within {timeout.TotalMilliseconds}ms", degradedOnError);
                    }
                    catch (Exception ex) when (ex is TimeoutException)
                    {
                        Logger.ErrorException($"HTTP Health Check '{uri}' timed out. Time taken: {sw.ElapsedMilliseconds}ms.", ex);

                        return HealthCheckResultOnError($"FAILED. '{uri}' timed out. Time taken: {sw.ElapsedMilliseconds}ms.", degradedOnError);
                    }
                    catch (Exception ex) when (ex is HttpRequestException)
                    {
                        Logger.ErrorException($"HTTP Health Check '{uri}' failed. Time taken: {sw.ElapsedMilliseconds}ms.", ex);

                        return HealthCheckResultOnError($"FAILED. '{uri}' request failed with an unexpected error. Time taken: {sw.ElapsedMilliseconds}ms.", degradedOnError);
                    }
                    catch (Exception ex)
                    {
                        var message = $"HTTP Health Check failed to request '{uri}'. Time taken: {sw.ElapsedMilliseconds}ms.";

                        Logger.ErrorException(message, ex);

                        return HealthCheckResultOnError($"FAILED. {message}", degradedOnError);
                    }
                    finally
                    {
                        sw.Stop();
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

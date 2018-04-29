// <copyright file="HealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Concurrency;

namespace App.Metrics.Health
{
    public class HealthCheck
    {
        private readonly TimeSpan _cacheDuration = TimeSpan.Zero;
        private readonly Func<CancellationToken, ValueTask<HealthCheckResult>> _check;
        private Result _cachedResult;
        private AtomicLong _reCheckAt = new AtomicLong(0);

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
        public HealthCheck(string name, Func<ValueTask<HealthCheckResult>> check)
        {
            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check();

            _check = CheckWithToken;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
        /// <param name="cacheDuration">The duration of which to cache the health check result.</param>
        public HealthCheck(
            string name,
            Func<ValueTask<HealthCheckResult>> check,
            TimeSpan cacheDuration)
        {
            EnsureValidCacheDuration(cacheDuration);

            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check();

            _check = CheckWithToken;
            _cacheDuration = cacheDuration;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
        public HealthCheck(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check)
        {
            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check(token);

            _check = CheckWithToken;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
        /// <param name="cacheDuration">The duration of which to cache the health check result.</param>
        public HealthCheck(
            string name,
            Func<CancellationToken, ValueTask<HealthCheckResult>> check,
            TimeSpan cacheDuration)
        {
            EnsureValidCacheDuration(cacheDuration);

            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check(token);

            _check = CheckWithToken;
            _cacheDuration = cacheDuration;
        }

        protected HealthCheck(string name)
        {
            Name = name;
            _check = token => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
        }

        protected HealthCheck(string name, TimeSpan cacheDuration)
        {
            EnsureValidCacheDuration(cacheDuration);

            Name = name;
            _cacheDuration = cacheDuration;
            _check = token => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
        }

        /// <summary>
        ///     Gets the descriptive name for the health check.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        ///     Executes the health check asynchrously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     The <see cref="Result" /> of running the health check
        /// </returns>
        public async ValueTask<Result> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (HasCacheDuration())
                {
                    return await ExecuteWithCachingAsync(cancellationToken);
                }

                var checkResult = await CheckAsync(cancellationToken);
                return new Result(Name, checkResult);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                return new Result(Name, HealthCheckResult.Unhealthy(ex));
            }
        }

        protected virtual ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default) { return _check(cancellationToken); }

        private static void EnsureValidCacheDuration(TimeSpan cacheDuration)
        {
            if (cacheDuration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be greater than zero", nameof(cacheDuration));
            }
        }

        private async Task<Result> ExecuteWithCachingAsync(CancellationToken cancellationToken)
        {
            if (_reCheckAt.GetValue() >= DateTime.UtcNow.Ticks)
            {
                return _cachedResult;
            }

            var checkResult = await CheckAsync(cancellationToken);
            _cachedResult = new Result(Name, checkResult, true);

            _reCheckAt.SetValue(DateTime.UtcNow.Ticks + _cacheDuration.Ticks);

            return new Result(Name, checkResult);
        }

        private bool HasCacheDuration() { return _cacheDuration > TimeSpan.Zero; }

        /// <summary>
        ///     Represents the result of running a <see cref="HealthCheck" />
        /// </summary>
        public struct Result
        {
            public readonly HealthCheckResult Check;
            public readonly bool IsFromCache;
            public readonly string Name;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Result" /> struct.
            /// </summary>
            /// <param name="name">A descriptive name for the health check</param>
            /// <param name="check">The result of executing a health check.</param>
            public Result(string name, HealthCheckResult check)
            {
                Name = name;
                Check = check;
                IsFromCache = false;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Result" /> struct.
            /// </summary>
            /// <param name="name">A descriptive name for the health check</param>
            /// <param name="check">The result of executing a health check.</param>
            /// <param name="isFromCache">
            ///     <value>true</value>
            ///     if the result was cached.
            /// </param>
            public Result(string name, HealthCheckResult check, bool isFromCache)
            {
                Name = name;
                Check = check;
                IsFromCache = isFromCache;
            }
        }
    }
}
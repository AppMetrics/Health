﻿// <copyright file="HealthCheckFactoryExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Health.Facts
{
    public class HealthCheckFactoryExtensionsTests
    {
        private readonly ILogger<DefaultHealthCheckRegistry> _logger;

        public HealthCheckFactoryExtensionsTests() { _logger = new LoggerFactory().CreateLogger<DefaultHealthCheckRegistry>(); }

        [Theory]
        [Trait("Category", "Requires Connectivity")]
        [InlineData(HealthCheckStatus.Healthy, "https://github.com", false)]
        [InlineData(HealthCheckStatus.Degraded, "https://github.unknown", true)]
        [InlineData(HealthCheckStatus.Unhealthy, "https://github.unknown", false)]
        public async Task Can_execute_http_get_check(HealthCheckStatus expectedResult, string uriString, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "github home";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.AddHttpGetCheck(name, new Uri(uriString), TimeSpan.FromSeconds(10), degradedOnError: degradedOnError);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

#pragma warning disable xUnit1004 // Test methods should not be skipped
        [Theory(Skip = "Mock HTTP Call")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
        [Trait("Category", "Requires Connectivity")]
        [InlineData(HealthCheckStatus.Healthy, "github.com", false)]
        [InlineData(HealthCheckStatus.Degraded, "github.unknown", true)]
        [InlineData(HealthCheckStatus.Unhealthy, "github.unknown", false)]
        public async Task Can_execute_ping_check(HealthCheckStatus expectedResult, string host, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "github ping";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.AddPingCheck(name, host, TimeSpan.FromSeconds(10), degradedOnError: degradedOnError);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HealthCheckStatus.Healthy, int.MaxValue, false)]
        [InlineData(HealthCheckStatus.Degraded, int.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, int.MinValue, false)]
        public async Task Can_execute_process_physical_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.AddProcessPhysicalMemoryCheck(name, thresholdBytes, degradedOnError: degradedOnError);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HealthCheckStatus.Healthy, int.MaxValue, false)]
        [InlineData(HealthCheckStatus.Degraded, int.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, int.MinValue, false)]
        public async Task Can_execute_process_private_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.AddProcessPrivateMemorySizeCheck(name, thresholdBytes, degradedOnError: degradedOnError);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HealthCheckStatus.Healthy, long.MaxValue, false)]
        [InlineData(HealthCheckStatus.Degraded, long.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, long.MinValue, false)]
        public async Task Can_execute_process_virtual_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.AddProcessVirtualMemorySizeCheck(name, thresholdBytes, degradedOnError: degradedOnError);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Fact]
        public void Can_register_process_physical_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.AddProcessPhysicalMemoryCheck(name, 100);

            registry.Checks.Should().NotBeEmpty();
            registry.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void Can_register_process_private_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.AddProcessPrivateMemorySizeCheck(name, 100);

            registry.Checks.Should().NotBeEmpty();
            registry.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void Can_register_process_virtual_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.AddProcessVirtualMemorySizeCheck(name, 100);

            registry.Checks.Should().NotBeEmpty();
            registry.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public async Task Should_be_unhealthy_when_task_is_cancelled()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "custom with cancellation token";

            var registry = new DefaultHealthCheckRegistry(healthChecks);

            registry.Register(
                name,
                async cancellationToken =>
                {
                    await Task.Delay(2000, cancellationToken);
                    return HealthCheckResult.Healthy();
                });

            var token = new CancellationTokenSource();
            token.CancelAfter(200);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync(token.Token).ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }
    }
}
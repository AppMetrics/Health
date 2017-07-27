// <copyright file="HealthCheckRegistryTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.Health.Facts.Fixtures;
using App.Metrics.Health.Facts.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace App.Metrics.Health.Facts.DependencyInjection
{
    public class HealthCheckRegistryTests : IClassFixture<HealthFixture>
    {
#pragma warning disable CS0612
        private readonly HealthFixture _fixture;

        public HealthCheckRegistryTests(HealthFixture fixture) { _fixture = fixture; }

        [Fact]
        public async Task Can_register_inline_health_checks()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IDatabase, Database>();

            services
                .AddHealth(
                    _fixture.StartupAssemblyName,
                    checksRegistry =>
                    {
                        checksRegistry.AddCheck("DatabaseConnected", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK")));
                    });

            var provider = services.BuildServiceProvider();
            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            var result = await healthProvider.ReadAsync();

            result.HasRegisteredChecks.Should().BeTrue();
            result.Results.Should().HaveCount(2);
        }

        [Fact]
        public async Task Should_report_healthy_when_all_checks_pass()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IDatabase, Database>();
            services.AddHealth(startupAssemblyName: _fixture.StartupAssemblyName);

            var provider = services.BuildServiceProvider();
            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            var result = await healthProvider.ReadAsync();

            result.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task Should_report_unhealthy_when_all_checks_pass()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IDatabase, Database>();

            services
                .AddHealth(
                    _fixture.StartupAssemblyName,
                    checksRegistry =>
                    {
                        checksRegistry.AddCheck(
                            "DatabaseConnected",
                            () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy("Failed")));
                    });

            var provider = services.BuildServiceProvider();
            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            var result = await healthProvider.ReadAsync();

            result.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }

        [Fact]
        public async Task Should_scan_assembly_and_register_health_checks_and_ignore_obsolete_checks()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IDatabase, Database>();
            services.AddHealth(startupAssemblyName: _fixture.StartupAssemblyName);

            var provider = services.BuildServiceProvider();
            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            var result = await healthProvider.ReadAsync();

            result.HasRegisteredChecks.Should().BeTrue();
            result.Results.Should().HaveCount(1);
        }
    }
#pragma warning restore CS0612
}
// <copyright file="HealthStatusTextWriterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;
using App.Metrics.Health.Formatters.Ascii.Facts.Fixtures;
using App.Metrics.Health.Serialization;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Formatters.Ascii.Facts
{
    public class HealthStatusTextWriterTests
    {
        private readonly HealthFixture _fixture;

        public HealthStatusTextWriterTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new HealthFixture();
        }

        [Fact]
        public async Task Can_apply_ascii_health_formatting()
        {
            // Arrange
            _fixture.HealthCheckRegistry.AddCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var serializer = new HealthStatusSerializer();

            // Act
            var healthStatus = await _fixture.Health.ReadAsync();

            using (var sw = new StringWriter())
            {
                using (var writer = new HealthStatusTextWriter(sw))
                {
                    serializer.Serialize(writer, healthStatus);
                }

                // Assert
                sw.ToString().Should().Be(
                    "# OVERALL STATUS: Healthy\n--------------------------------------------------------------\n# CHECK: test\n\n           MESSAGE = OK\n            STATUS = Healthy\n--------------------------------------------------------------\n");
            }
        }
    }
}
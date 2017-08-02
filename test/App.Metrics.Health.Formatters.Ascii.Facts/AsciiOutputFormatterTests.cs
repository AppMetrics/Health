// <copyright file="AsciiOutputFormatterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Health.Formatters.Ascii.Facts.Fixtures;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Formatters.Ascii.Facts
{
    public class AsciiOutputFormatterTests
    {
        private readonly HealthFixture _fixture;

        public AsciiOutputFormatterTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new HealthFixture();
        }

        [Fact]
        public async Task Can_apply_ascii_health_formatting()
        {
            // Arrange
            string result;
            _fixture.HealthCheckRegistry.AddCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var formatter = new AsciiOutputFormatter(new HealthAsciiOptions());

            // Act
            var healthStatus = await _fixture.Health.ReadAsync();

            using (var stream = new MemoryStream())
            {
                formatter.WriteAsync(stream, healthStatus, Encoding.UTF8).GetAwaiter().GetResult();

                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            // Assert
            result.Should().Be(
                "# OVERALL STATUS: Healthy\n--------------------------------------------------------------\n# CHECK: test\n\n           MESSAGE = OK\n            STATUS = Healthy\n--------------------------------------------------------------\n");
        }
    }
}
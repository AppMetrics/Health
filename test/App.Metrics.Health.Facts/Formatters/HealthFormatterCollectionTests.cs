// <copyright file="HealthFormatterCollectionTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatters.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts.Formatters
{
    public class HealthFormatterCollectionTests
    {
        [Fact]
        public void Can_remove_type()
        {
            // Arrange
            var formatters = new HealthFormatterCollection<IHealthOutputFormatter> { new AsciiOutputFormatter(new AppMetricsHealthAsciiOptions()) };

            // Act
            formatters.RemoveType<AsciiOutputFormatter>();

            // Assert
            formatters.Count.Should().Be(0);
        }

        [Fact]
        public void Can_get_type()
        {
            // Arrange
            var formatters =
                new HealthFormatterCollection<IHealthOutputFormatter>
                {
                    new AsciiOutputFormatter(new AppMetricsHealthAsciiOptions()),
                    new JsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType<AsciiOutputFormatter>();

            // Assert
            formatter.Should().NotBeNull();
            formatter.Should().BeOfType<AsciiOutputFormatter>();
        }

        [Fact]
        public void Returns_default_when_attempting_to_get_type_not_added()
        {
            // Arrange
            var formatters =
                new HealthFormatterCollection<IHealthOutputFormatter>
                {
                    new JsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType<AsciiOutputFormatter>();

            // Assert
            formatter.Should().BeNull();
        }
    }
}

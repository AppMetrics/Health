// <copyright file="HealthFormatterCollectionTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
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
        public void Can_get_by_mediatype()
        {
            // Arrange
            var mediaType = new AppMetricsHealthMediaTypeValue("application", "vnd.appmetrics.health", "v1", "json");
            var formatters =
                new HealthFormatterCollection
                {
                    new AsciiOutputFormatter(new HealthAsciiOptions()),
                    new JsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType(mediaType);

            // Assert
            formatter.Should().NotBeNull();
            formatter.Should().BeOfType<JsonOutputFormatter>();
        }

        [Fact]
        public void Can_get_type()
        {
            // Arrange
            var formatters =
                new HealthFormatterCollection
                {
                    new AsciiOutputFormatter(new HealthAsciiOptions()),
                    new JsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType<AsciiOutputFormatter>();

            // Assert
            formatter.Should().NotBeNull();
            formatter.Should().BeOfType<AsciiOutputFormatter>();
        }

        [Fact]
        public void Can_get_type_passing_in_type()
        {
            // Arrange
            var formatters =
                new HealthFormatterCollection
                {
                    new AsciiOutputFormatter(new HealthAsciiOptions()),
                    new JsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType(typeof(AsciiOutputFormatter));

            // Assert
            formatter.Should().NotBeNull();
            formatter.Should().BeOfType<AsciiOutputFormatter>();
        }

        [Fact]
        public void Can_remove_by_mediatype()
        {
            // Arrange
            var mediaType = new AppMetricsHealthMediaTypeValue("text", "vnd.appmetrics.health", "v1", "plain");
            var formatters = new HealthFormatterCollection(
                new List<IHealthOutputFormatter> { new AsciiOutputFormatter(new HealthAsciiOptions()) });

            // Act
            formatters.RemoveType(mediaType);

            // Assert
            formatters.Count.Should().Be(0);
        }

        [Fact]
        public void Can_remove_type()
        {
            // Arrange
            var formatters = new HealthFormatterCollection { new AsciiOutputFormatter(new HealthAsciiOptions()) };

            // Act
            formatters.RemoveType<AsciiOutputFormatter>();

            // Assert
            formatters.Count.Should().Be(0);
        }

        [Fact]
        public void Can_remove_type_passing_in_type()
        {
            // Arrange
            var formatters = new HealthFormatterCollection { new AsciiOutputFormatter(new HealthAsciiOptions()) };

            // Act
            formatters.RemoveType(typeof(AsciiOutputFormatter));

            // Assert
            formatters.Count.Should().Be(0);
        }

        [Fact]
        public void Returns_default_when_attempting_to_get_type_not_added()
        {
            // Arrange
            var formatters =
                new HealthFormatterCollection
                {
                    new JsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType<AsciiOutputFormatter>();

            // Assert
            formatter.Should().BeNull();
        }

        [Fact]
        public void Returns_default_when_attempting_to_get_type_with_mediatype_not_added()
        {
            // Arrange
            var mediaType = new AppMetricsHealthMediaTypeValue("text", "vnd.appmetrics.health", "v1", "plain");
            var formatters =
                new HealthFormatterCollection
                {
                    new JsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType(mediaType);

            // Assert
            formatter.Should().BeNull();
        }
    }
}
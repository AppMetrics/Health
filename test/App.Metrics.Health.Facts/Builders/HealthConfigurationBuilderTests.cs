// <copyright file="HealthConfigurationBuilderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Health.Builder;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts.Builders
{
    public class HealthConfigurationBuilderTests
    {
        [Fact]
        public void Can_override_option_instance_values_with_key_value_pair_options()
        {
            // Arrange
            var options = new HealthOptions
                          {
                              Enabled = true,
                          };

            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "HealthOptions:Enabled", "false" }
                                };

            // Act
            var health = new HealthBuilder().Configuration.Configure(options, keyValuePairs).Build();

            // Assert
            health.Options.Enabled.Should().BeFalse();
        }

        [Fact]
        public void Can_set_options_via_key_value_pairs()
        {
            // Arrange
            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "HealthOptions:Enabled", "false" }
                                };

            // Act
            var health = new HealthBuilder().Configuration.Configure(keyValuePairs).Build();

            // Assert
            health.Options.Enabled.Should().BeFalse();
        }

        [Fact]
        public void Can_set_options_via_setup_action()
        {
            // Arrange
            void SetupAction(HealthOptions options)
            {
                options.Enabled = false;
            }

            // Act
            var health = new HealthBuilder().Configuration.Configure(SetupAction).Build();

            // Assert
            health.Options.Enabled.Should().BeFalse();
        }

        [Fact]
        public void Can_set_options_with_instance()
        {
            // Arrange
            var options = new HealthOptions { Enabled = true };

            // Act
            var health = new HealthBuilder().Configuration.Configure(options).Build();

            // Assert
            health.Options.Enabled.Should().BeTrue();
        }
    }
}
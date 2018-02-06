﻿// <copyright file="BenchmarkTestExecutor.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Health.Benchmarks.Support
{
    // ReSharper disable UnusedMember.Global
    public class BenchmarkTestExecutor
        // ReSharper restore UnusedMember.Global
    {
        private readonly ITestOutputHelper _output;

        public BenchmarkTestExecutor() { }

        protected BenchmarkTestExecutor(ITestOutputHelper output) { _output = output; }

        /// <summary>
        ///     Runs Benchmarks with the most simple config (SingleRunFastConfig)
        ///     combined with any benchmark config applied to TBenchmark (via an attribute)
        ///     By default will verify if every benchmark was successfully executed
        /// </summary>
        /// <typeparam name="TBenchmark">type that defines Benchmarks</typeparam>
        /// <param name="config">Optional custom config to be used instead of the default</param>
        /// <param name="fullValidation">Optional: disable validation (default = true/enabled)</param>
        /// <returns>The summary from the benchmark run</returns>
        // ReSharper disable UnusedMember.Global
        internal Summary CanExecute<TBenchmark>(IConfig config = null, bool fullValidation = true)
            // ReSharper restore UnusedMember.Global
        {
            return CanExecute(typeof(TBenchmark), config, fullValidation);
        }

        /// <summary>
        ///     Runs Benchmarks with the most simple config (SingleRunFastConfig)
        ///     combined with any benchmark config applied to Type (via an attribute)
        ///     By default will verify if every benchmark was successfully executed
        /// </summary>
        /// <param name="type">type that defines Benchmarks</param>
        /// <param name="config">Optional custom config to be used instead of the default</param>
        /// <param name="fullValidation">Optional: disable validation (default = true/enabled)</param>
        /// <returns>The summary from the benchmark run</returns>
        private Summary CanExecute(Type type, IConfig config = null, bool fullValidation = true)
        {
            // Add logging, so the Benchmark execution is in the TestRunner output (makes Debugging easier)
            if (config == null)
            {
                config = CreateSimpleConfig();
            }

            if (!config.GetLoggers().OfType<OutputLogger>().Any())
            {
                config = config.With(_output != null ? new OutputLogger(_output) : ConsoleLogger.Default);
            }

            if (!config.GetColumnProviders().Any())
            {
                config = config.With(DefaultColumnProviders.Instance);
            }

            // Make sure we ALWAYS combine the Config (default or passed in) with any Config applied to the Type/Class
            var summary = BenchmarkRunner.Run(type, BenchmarkConverter.GetFullConfig(type, config));

            if (!fullValidation)
            {
                return summary;
            }

            Assert.False(summary.HasCriticalValidationErrors, "The \"Summary\" should have NOT \"HasCriticalValidationErrors\"");

            Assert.True(summary.Reports.Any(), "The \"Summary\" should contain at least one \"BenchmarkReport\" in the \"Reports\" collection");

            Assert.True(
                summary.Reports.All(r => r.BuildResult.IsBuildSuccess),
                "The following benchmarks are failed to build: " + string.Join(", ", summary.Reports.Where(r => !r.BuildResult.IsBuildSuccess).Select(r => r.Benchmark.DisplayInfo)));

            Assert.True(
                summary.Reports.All(r => r.ExecuteResults.Any(er => er.FoundExecutable && er.Data.Any())),
                "All reports should have at least one \"ExecuteResult\" with \"FoundExecutable\" = true and at least one \"Data\" item");

            Assert.True(
                summary.Reports.All(report => report.AllMeasurements.Any()),
                "All reports should have at least one \"Measurement\" in the \"AllMeasurements\" collection");

            return summary;
        }

        private IConfig CreateSimpleConfig(OutputLogger logger = null)
        {
            return new SingleRunFastConfig()
                .With(logger ?? (_output != null ? new OutputLogger(_output) : ConsoleLogger.Default))
                .With(DefaultColumnProviders.Instance);
        }
    }
}
﻿// <copyright file="Health.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Health.Benchmarks.BenchmarkDotNetBenchmarks;
using App.Metrics.Health.Benchmarks.Support;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Health.Benchmarks.XunitHarness
{
    [Trait("Benchmark-Health", "ReadStatus")]
    public class Health
    {
        private readonly ITestOutputHelper _output;

        public Health(ITestOutputHelper output) { _output = output; }

        [Fact]
        public void CostOfReadingHealth() { BenchmarkTestRunner.CanCompileAndRun<MeasureReadHealthStatusBenchmark>(_output); }
    }
}

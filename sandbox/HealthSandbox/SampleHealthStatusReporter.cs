// <copyright file="SampleHealthStatusReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace HealthSandbox
{
    public class SampleHealthStatusReporter : IReportHealthStatus
    {
        /// <inheritdoc />
        public Task ReportAsync(HealthStatus status, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Overall status: {status.Status}");

            return Task.CompletedTask;
        }
    }
}
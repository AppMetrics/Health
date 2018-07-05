// <copyright file="IReportHealthStatus.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health
{
    public interface IReportHealthStatus
    {
        Task ReportAsync(HealthOptions options, HealthStatus status, CancellationToken cancellationToken = default);
    }
}
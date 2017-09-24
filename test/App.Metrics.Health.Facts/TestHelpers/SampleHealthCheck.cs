// <copyright file="SampleHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Facts.TestHelpers
{
    public class SampleHealthCheck : HealthCheck
    {
        public SampleHealthCheck()
            : base("SampleHealthCheck") { }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken token = default)
        {
            return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("OK"));
        }
    }
}
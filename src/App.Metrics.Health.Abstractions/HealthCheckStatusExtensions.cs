﻿// <copyright file="HealthCheckStatusExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Health
{
    public static class HealthCheckStatusExtensions
    {
        public static bool IsDegraded(this HealthCheckStatus status) { return status == HealthCheckStatus.Degraded; }

        public static bool IsHealthy(this HealthCheckStatus status) { return status == HealthCheckStatus.Healthy; }

        public static bool IsIgnored(this HealthCheckStatus status) { return status == HealthCheckStatus.Ignored; }

        public static bool IsUnhealthy(this HealthCheckStatus status) { return status == HealthCheckStatus.Unhealthy; }
    }
}
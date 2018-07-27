// <copyright file="HealthCheckResultItem.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Health.Reporting.GrafanaAnnotation.Internal
{
    public class HealthCheckResultItem
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public HealthCheckResultItem(string checkName, HealthCheckStatus lastCheckStatus, int? annotationId)
        {
            Name = checkName;
            Status = lastCheckStatus;
            InitialCheckAt = (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
            AnnotationId = annotationId;
        }

        public HealthCheckResultItem(string checkName, HealthCheckStatus lastCheckStatus)
        {
            Name = checkName;
            Status = lastCheckStatus;
            InitialCheckAt = (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
            AnnotationId = null;
        }

        public int? AnnotationId { get; }

        public string Name { get; }

        public HealthCheckStatus Status { get; }

        public long InitialCheckAt { get; set; }
    }
}
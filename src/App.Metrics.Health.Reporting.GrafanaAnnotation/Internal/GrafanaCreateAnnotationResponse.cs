// <copyright file="GrafanaCreateAnnotationResponse.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Health.Reporting.GrafanaAnnotation.Internal
{
    public class GrafanaCreateAnnotationResponse
    {
        public int EndId { get; set; }

        public int Id { get; set; }

        public string Message { get; set; }
    }
}
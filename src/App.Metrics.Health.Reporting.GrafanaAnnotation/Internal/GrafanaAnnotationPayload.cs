// <copyright file="GrafanaAnnotationPayload.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Health.Reporting.GrafanaAnnotation.Internal
{
    internal class GrafanaAnnotationPayload
    {
        public int? DashboardId { get; set; }

        public bool IsRegion { get; set; } = false;

        public int? PanelId { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public string Text { get; set; }

        public long Time { get; set; }

        public long? TimeEnd { get; set; }
    }
}
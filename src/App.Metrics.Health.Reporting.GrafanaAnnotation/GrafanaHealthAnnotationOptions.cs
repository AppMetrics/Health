// <copyright file="GrafanaHealthAnnotationOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace App.Metrics.Health.Reporting.GrafanaAnnotation
{
    public class GrafanaHealthAnnotationOptions
    {
        public bool AlertOnDegradedChecks { get; set; } = true;

        public List<string> Tags { get; set; }

        public bool Enabled { get; set; } = true;

        public Uri AnnotationEndpoint { get; set; }

        public string Token { get; set; }

        public int? DashboardId { get; set; }

        public int? PanelId { get; set; }

        /// <summary>
        ///     Gets or sets the health status reporting interval.
        /// </summary>
        /// <remarks>
        ///     If not set reporting interval will be set to the <see cref="HealthConstants.Reporting.DefaultReportInterval" />.
        /// </remarks>
        /// <value>
        ///     The <see cref="TimeSpan" /> to wait between reporting health status.
        /// </value>
        public TimeSpan ReportInterval { get; set; }

        /// <summary>
        ///     Gets or sets the number of report runs before re-alerting checks that have re-failed. If set to 0, failed checks will only be reported once until healthy again.
        /// </summary>
        /// <remarks>
        ///     If not set number of runs will be set to the <see cref="HealthConstants.Reporting.DefaultNumberOfRunsBeforeReAlerting" />.
        /// </remarks>
        public int RunsBeforeReportExistingFailures { get; set; } = HealthConstants.Reporting.DefaultNumberOfRunsBeforeReAlerting;
    }
}
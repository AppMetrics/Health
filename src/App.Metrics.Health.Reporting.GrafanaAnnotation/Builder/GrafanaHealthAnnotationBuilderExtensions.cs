// <copyright file="GrafanaHealthAnnotationBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Reporting.GrafanaAnnotation;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class GrafanaHealthAnnotationBuilderExtensions
    {
        public static IHealthBuilder ToGrafanaAnnotation(
            this IHealthReportingBuilder healthReportingBuilder,
            Action<GrafanaHealthAnnotationOptions> optionsSetup)
        {
            var options = new GrafanaHealthAnnotationOptions();

            optionsSetup(options);

            healthReportingBuilder.Using(new GrafanaAnnotationHealthReporter(options));

            return healthReportingBuilder.Builder;
        }

        public static IHealthBuilder ToGrafanaAnnotation(
            this IHealthReportingBuilder healthReportingBuilder,
            GrafanaHealthAnnotationOptions options)
        {
            healthReportingBuilder.Using(new GrafanaAnnotationHealthReporter(options));

            return healthReportingBuilder.Builder;
        }
    }
}
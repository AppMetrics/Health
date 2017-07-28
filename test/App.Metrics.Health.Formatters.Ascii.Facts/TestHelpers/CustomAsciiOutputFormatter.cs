// <copyright file="CustomAsciiOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Formatters.Ascii.Facts.TestHelpers
{
    public class CustomAsciiOutputFormatter : IHealthOutputFormatter
    {
        public AppMetricsHealthMediaTypeValue MediaType => new AppMetricsHealthMediaTypeValue("text", "vnd.custom.health", "v1", "plain");

        public Task WriteAsync(
            Stream output,
            HealthStatus healthStatus,
            Encoding encoding,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            string formattedResult;

            using (var writer = new StringWriter())
            {
                var status = GetOverallStatus(healthStatus.Results);

                writer.Write($"Overall: {status}");
                writer.Write('\n');

                foreach (var result in healthStatus.Results.OrderBy(r => (int)r.Check.Status))
                {
                    WriteCheckResult(writer, result);
                }

                formattedResult = writer.ToString();
            }

            var bytes = encoding.GetBytes(formattedResult);

            return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }

        private static string GetOverallStatus(HealthCheck.Result[] results)
        {
            var status = HealthConstants.DegradedStatusDisplay;

            var failed = results.Any(c => c.Check.Status == HealthCheckStatus.Unhealthy);
            var degraded = results.Any(c => c.Check.Status == HealthCheckStatus.Degraded);

            if (!degraded && !failed)
            {
                status = HealthConstants.HealthyStatusDisplay;
            }

            if (failed)
            {
                status = HealthConstants.UnhealthyStatusDisplay;
            }

            return status;
        }

        private static void WriteCheckResult(StringWriter writer, HealthCheck.Result checkResult)
        {
            writer.Write(checkResult.Name);
            writer.Write(' ');
            writer.Write(checkResult.Check.Message);
            writer.Write(' ');
            writer.Write(HealthConstants.HealthStatusDisplay[checkResult.Check.Status]);
            writer.Write('\n');
        }
    }
}
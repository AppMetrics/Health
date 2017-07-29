// <copyright file="AsciiOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Formatters.Ascii
{
    public class AsciiOutputFormatter : IHealthOutputFormatter
    {
        private readonly AppMetricsHealthAsciiOptions _options;

        public AsciiOutputFormatter(AppMetricsHealthAsciiOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        public AppMetricsHealthMediaTypeValue MediaType => new AppMetricsHealthMediaTypeValue("text", "vnd.appmetrics.health", "v1", "plain");

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

                writer.Write($"# OVERALL STATUS: {status}");
                writer.Write("\n--------------------------------------------------------------\n");

                foreach (var result in healthStatus.Results.OrderBy(r => (int)r.Check.Status))
                {
                    WriteCheckResult(writer, result);
                }

                formattedResult = writer.ToString();
            }

            var bytes = encoding.GetBytes(formattedResult);

            return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }

        private string FormatReadable(string label, string value)
        {
            var pad = string.Empty;

            if (label.Length + 2 + _options.Separator.Length < _options.Padding)
            {
                pad = new string(' ', _options.Padding - label.Length - 1 - _options.Separator.Length);
            }

            return $"{pad}{label} {_options.Separator} {value}";
        }

        private string GetOverallStatus(IEnumerable<HealthCheck.Result> results)
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

        private void WriteCheckResult(StringWriter writer, HealthCheck.Result checkResult)
        {
            writer.Write("# CHECK: ");
            writer.Write(checkResult.Name);
            writer.Write('\n');
            writer.Write('\n');
            writer.Write(FormatReadable("MESSAGE", checkResult.Check.Message));
            writer.Write('\n');
            writer.Write(FormatReadable("STATUS", HealthConstants.HealthStatusDisplay[checkResult.Check.Status]));
            writer.Write("\n--------------------------------------------------------------");
            writer.Write('\n');
        }
    }
}
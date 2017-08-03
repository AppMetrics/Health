// <copyright file="JsonOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Metrics.Health.Formatters.Json
{
    public class JsonOutputFormatter : IHealthOutputFormatter
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonOutputFormatter() { _serializerSettings = DefaultJsonSerializerSettings.CreateSerializerSettings(); }

        public JsonOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));
        }

        public HealthMediaTypeValue MediaType => new HealthMediaTypeValue("application", "vnd.appmetrics.health", "v1", "json");

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

            var json = JsonConvert.SerializeObject(healthStatus, _serializerSettings);

            var bytes = encoding.GetBytes(json);

            return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }
    }
}
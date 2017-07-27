// <copyright file="IHealthOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Formatters
{
    public interface IHealthOutputFormatter
    {
        Task WriteAsync(Stream output, HealthStatus healthStatus, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));
    }
}
// <copyright file="NoOpHealthProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public sealed class NoOpHealthProvider : IProvideHealth
    {
        /// <inheritdoc />
        public ValueTask<HealthStatus> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new ValueTask<HealthStatus>(new HealthStatus(Enumerable.Empty<HealthCheck.Result>()));
        }

        public Task FormatAsync(Stream output, HealthStatus status, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}
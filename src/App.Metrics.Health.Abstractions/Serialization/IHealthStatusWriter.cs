// <copyright file="IHealthStatusWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Health.Serialization
{
    public interface IHealthStatusWriter : IDisposable
    {
        /// <summary>
        /// Writes the specified <see cref="HealthStatus"/>.
        /// </summary>
        /// <param name="healthStatus">The health status to write.</param>
        void Write(HealthStatus healthStatus);
    }
}

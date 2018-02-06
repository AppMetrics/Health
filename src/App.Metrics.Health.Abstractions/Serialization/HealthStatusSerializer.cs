// <copyright file="HealthStatusSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Health.Serialization
{
    /// <summary>
    ///     Serializes <see cref="HealthStatus" /> into the different formats.
    /// </summary>
    public class HealthStatusSerializer
    {
        /// <summary>
        ///     Serializes the specified <see cref="HealthStatus" /> and writes the health status using the specified
        ///     <see cref="IHealthStatusWriter" />.
        /// </summary>
        /// <param name="writer">The <see cref="IHealthStatusWriter" /> used to write the health status.</param>
        /// <param name="healthStatus">The <see cref="HealthStatus" /> to serilize.</param>
        public void Serialize(IHealthStatusWriter writer, HealthStatus healthStatus) { writer.Write(healthStatus); }
    }
}

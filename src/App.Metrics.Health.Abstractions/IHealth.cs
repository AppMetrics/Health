// <copyright file="IHealth.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Health
{
    public interface IHealth
    {
        IEnumerable<HealthCheck> Checks { get; }
    }
}

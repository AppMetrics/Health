﻿// <copyright file="IHealthCheckRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics
{
    public interface IHealthCheckRegistry
    {
        Dictionary<string, HealthCheck> Checks { get; }

        void AddCheck(string name, Func<ValueTask<HealthCheckResult>> check);

        void AddCheck(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check);
    }
}
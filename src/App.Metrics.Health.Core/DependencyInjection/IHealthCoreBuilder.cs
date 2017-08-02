// <copyright file="IHealthCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public interface IHealthCoreBuilder
    {
        IServiceCollection Services { get; }
    }
}
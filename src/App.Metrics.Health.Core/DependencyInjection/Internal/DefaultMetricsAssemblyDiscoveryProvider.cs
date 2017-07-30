// <copyright file="DefaultMetricsAssemblyDiscoveryProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace App.Metrics.Health.DependencyInjection.Internal
{
    internal static class DefaultMetricsAssemblyDiscoveryProvider
    {
        private static readonly string ReferenceAssembliesPrefix = "App.Metrics";

        // ReSharper disable MemberCanBePrivate.Global
        internal static IEnumerable<Assembly> DiscoverAssemblies(string entryPointAssemblyName)
        {
            var entryAssembly = Assembly.Load(new AssemblyName(entryPointAssemblyName));
            var context = DependencyContext.Load(Assembly.Load(new AssemblyName(entryPointAssemblyName)));

            return GetCandidateAssemblies(entryAssembly, context);
        }

        internal static IEnumerable<Assembly> GetCandidateAssemblies(Assembly entryAssembly, DependencyContext dependencyContext)
        {
            if (dependencyContext == null)
            {
                // Use the entry assembly as the sole candidate.
                return new[] { entryAssembly };
            }

            return GetCandidateLibraries(dependencyContext).
                SelectMany(library => library.GetDefaultAssemblyNames(dependencyContext)).
                Select(Assembly.Load);
        }

        internal static IEnumerable<RuntimeLibrary> GetCandidateLibraries(DependencyContext dependencyContext)
        {
            return dependencyContext.RuntimeLibraries.Where(IsCandidateLibrary);
        }

        private static bool IsCandidateLibrary(RuntimeLibrary library)
        {
            return library.Name.StartsWith(ReferenceAssembliesPrefix) || library.Dependencies.Any(d => d.Name.StartsWith(ReferenceAssembliesPrefix));
        }

        // ReSharper restore MemberCanBePrivate.Global
    }
}
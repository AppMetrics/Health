// <copyright file="HealthOptionsExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Health.Internal.Extensions
{
    public static class HealthOptionsExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValue(this HealthOptions options)
        {
            var result = new Dictionary<string, string>
                         {
                             [KeyValuePairHealthOptions.EnabledDirective] = options.Enabled.ToString()
                         };

            return result;
        }
    }
}

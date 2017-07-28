// <copyright file="HealthFormatterCollection.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Metrics.Health.Formatters
{
    public class HealthFormatterCollection : Collection<IHealthOutputFormatter>
    {
        public HealthFormatterCollection() { }

        public HealthFormatterCollection(IList<IHealthOutputFormatter> list)
            : base(list)
        {
        }

        public IHealthOutputFormatter GetType<T>()
            where T : IHealthOutputFormatter
        {
            return GetType(typeof(T));
        }

        public IHealthOutputFormatter GetType(Type formatterType)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.GetType() == formatterType)
                {
                    return formatter;
                }
            }

            return default(IHealthOutputFormatter);
        }

        public IHealthOutputFormatter GetType(AppMetricsHealthMediaTypeValue mediaTypeValue)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.MediaType == mediaTypeValue)
                {
                    return formatter;
                }
            }

            return default(IHealthOutputFormatter);
        }

        public void RemoveType<T>()
            where T : IHealthOutputFormatter
        {
            RemoveType(typeof(T));
        }

        public void RemoveType(Type formatterType)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.GetType() == formatterType)
                {
                    RemoveAt(i);
                }
            }
        }

        public void RemoveType(AppMetricsHealthMediaTypeValue mediaTypeValue)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.MediaType == mediaTypeValue)
                {
                    RemoveAt(i);
                }
            }
        }
    }
}
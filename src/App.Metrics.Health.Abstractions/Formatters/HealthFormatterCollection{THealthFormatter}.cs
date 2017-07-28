// <copyright file="HealthFormatterCollection{THealthFormatter}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Metrics.Health.Formatters
{
    public class HealthFormatterCollection<THealthFormatter> : Collection<THealthFormatter>
    {
        public HealthFormatterCollection() { }

        public HealthFormatterCollection(IList<THealthFormatter> list)
            : base(list)
        {
        }

        public void RemoveType<T>()
            where T : THealthFormatter
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

        public THealthFormatter GetType<T>()
            where T : THealthFormatter
        {
            return GetType(typeof(T));
        }

        public THealthFormatter GetType(Type formatterType)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.GetType() == formatterType)
                {
                    return formatter;
                }
            }

            return default(THealthFormatter);
        }
    }
}
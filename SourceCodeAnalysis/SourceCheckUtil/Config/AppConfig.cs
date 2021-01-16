using System;
using System.Collections.Generic;

namespace SourceCheckUtil.Config
{
    internal class AppConfig
    {
        public AppConfig(AppUsageMode mode, IDictionary<String, Object> values)
        {
            Mode = mode;
            Values = values;
        }

        public AppUsageMode Mode { get; }

        public IDictionary<String, Object> Values { get; }

        public const String Source = "source";
        public const String Config = "config";
        public const String Verbose = "verbose";
    }
}
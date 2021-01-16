using System;

namespace SourceCheckUtil.Config
{
    internal class AnalysisConfig
    {
        public AnalysisConfig(String source, String config, Boolean verbose)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));
            Source = source;
            Config = config;
            Verbose = verbose;
        }

        public AnalysisConfig(AppConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (config.Mode != AppUsageMode.Analysis)
                throw new ArgumentException(nameof(config));
            Source = (String) config.Values[AppConfig.Source];
            Config = (String) config.Values[AppConfig.Config];
            Verbose = (Boolean) config.Values[AppConfig.Verbose];
        }

        public String Source { get; }

        public String Config { get; }

        public Boolean Verbose { get; }
    }
}
using System;
using System.Collections.Generic;

namespace SourceCheckUtil.Config
{
    internal static class AppConfigParser
    {
        public static AppConfig Parse(String[] args)
        {
            // TODO (std_string) : rewrite this with help of FSM
            const String helpOption = "--help";
            const String versionOption = "--version";
            const String sourceOption = "--source";
            const String configOption = "--config";
            const String verboseOption = "--verbose";
            if (args.Length == 0)
                return new AppConfig(AppUsageMode.Help, new Dictionary<String, Object>());
            if (args.Length == 1 && String.Equals(helpOption, args[0]))
                return new AppConfig(AppUsageMode.Help, new Dictionary<String, Object>());
            if (args.Length == 1 && String.Equals(versionOption, args[0]))
                return new AppConfig(AppUsageMode.Version, new Dictionary<String, Object>());
            if (args.Length == 2 && String.Equals(sourceOption, args[0]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Source, args[1]}, {AppConfig.Config, null}, {AppConfig.Verbose, false}});
            if (args.Length == 3 && String.Equals(sourceOption, args[0]) && String.Equals(verboseOption, args[2]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Source, args[1]}, {AppConfig.Verbose, true}, {AppConfig.Config, null}});
            if (args.Length == 3 && String.Equals(verboseOption, args[0]) && String.Equals(sourceOption, args[1]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Verbose, true}, {AppConfig.Source, args[2]}, {AppConfig.Config, null}});
            if (args.Length == 4 && String.Equals(sourceOption, args[0]) && String.Equals(configOption, args[2]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Source, args[1]}, {AppConfig.Config, args[3]}, {AppConfig.Verbose, false}});
            if (args.Length == 4 && String.Equals(configOption, args[0]) && String.Equals(sourceOption, args[2]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Config, args[1]}, {AppConfig.Source, args[3]}, {AppConfig.Verbose, false}});
            if (args.Length == 5 && String.Equals(sourceOption, args[0]) && String.Equals(configOption, args[2]) && String.Equals(verboseOption, args[4]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Source, args[1]}, {AppConfig.Config, args[3]}, {AppConfig.Verbose, true}});
            if (args.Length == 5 && String.Equals(sourceOption, args[0]) && String.Equals(verboseOption, args[2]) && String.Equals(configOption, args[3]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Source, args[1]}, {AppConfig.Verbose, true}, {AppConfig.Config, args[4]}});
            if (args.Length == 5 && String.Equals(verboseOption, args[0]) && String.Equals(sourceOption, args[1]) && String.Equals(configOption, args[3]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Verbose, true}, {AppConfig.Source, args[2]}, {AppConfig.Config, args[4]}});
            if (args.Length == 5 && String.Equals(configOption, args[0]) && String.Equals(sourceOption, args[2]) && String.Equals(verboseOption, args[4]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Config, args[1]}, {AppConfig.Source, args[3]}, {AppConfig.Verbose, true}});
            if (args.Length == 5 && String.Equals(configOption, args[0]) && String.Equals(verboseOption, args[2]) && String.Equals(sourceOption, args[3]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Config, args[1]}, {AppConfig.Verbose, true}, {AppConfig.Source, args[4]}});
            if (args.Length == 5 && String.Equals(verboseOption, args[0]) && String.Equals(configOption, args[1]) && String.Equals(sourceOption, args[3]))
                return new AppConfig(AppUsageMode.Analysis, new Dictionary<String, Object> {{AppConfig.Verbose, true}, {AppConfig.Config, args[2]}, {AppConfig.Source, args[4]}});
            return new AppConfig(AppUsageMode.BadAppUsage, new Dictionary<String, Object>());
        }
    }
}
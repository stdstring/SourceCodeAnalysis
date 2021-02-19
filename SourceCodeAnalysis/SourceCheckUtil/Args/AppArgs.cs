using System;

namespace SourceCheckUtil.Args
{
    internal class AppArgs
    {
        public AppArgs(AppUsageMode mode)
        {
            Mode = mode;
            Source = null;
            Config = null;
            Verbose = VerboseDefaultValue;
        }

        public AppUsageMode Mode { get; }

        public String Source { get; set; }

        public String Config { get; set; }

        public Boolean Verbose { get; set; }

        public const Boolean VerboseDefaultValue = false;
    }
}
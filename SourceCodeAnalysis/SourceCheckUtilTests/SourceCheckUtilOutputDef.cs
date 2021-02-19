using System;

namespace SourceCheckUtilTests
{
    internal static class SourceCheckUtilOutputDef
    {
        public const String BadUsageMessage = "[ERROR]: Bad usage of the application.\r\n";
        public const String BadSourceMessage = "[ERROR]: Bad/empty/unknown source path.\r\n";
        public const String BadConfigMessage = "[ERROR]: Bad/empty/unknown config path.\r\n";
        public const String AppDescription = "Application usage:\r\n" +
                                             "1. {APP} --source={solution-filename.sln|project-filename.csproj|cs-filename.cs} [--config={config-file|config-dir}] [--verbose]\r\n" +
                                             "2. {APP} --help\r\n" +
                                             "3. {APP} --version\r\n";
    }
}
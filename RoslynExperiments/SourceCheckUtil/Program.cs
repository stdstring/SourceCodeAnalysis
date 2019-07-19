using System;
using System.Collections.Generic;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Config;
using SourceCheckUtil.Processors;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil
{
    public class Program
    {
        public static Int32 Main(String[] args)
        {
            Boolean result = MainImpl(args);
            return result ? 0 : -1;
        }

        private static Boolean MainImpl(String[] args)
        {
            AppConfig config = AppConfigParser.Parse(args);
            switch (config.Mode)
            {
                case AppUsageMode.Help:
                    Console.WriteLine(AppDescription);
                    return true;
                case AppUsageMode.Version:
                    Console.WriteLine(VersionNumber);
                    return true;
                case AppUsageMode.Analysis:
                    AnalysisConfig analysisConfig = new AnalysisConfig(config);
                    OutputImpl output = new OutputImpl(Console.Out, Console.Error, analysisConfig.Verbose);
                    ISourceProcessor processor = SourceProcessorFactory.Create(analysisConfig.Source, output);
                    IList<IFileAnalyzer> analyzers = AnalyzersFactory.Create(output);
                    Boolean processResult = processor.Process(analyzers);
                    output.WriteOutputLine($"Result of analysis: analysis is {(processResult ? "succeeded" : "failed")}");
                    return processResult;
                case AppUsageMode.BadAppUsage:
                case AppUsageMode.Unknown:
                    Console.WriteLine(BadUsageMessage);
                    Console.WriteLine(AppDescription);
                    return false;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static Boolean IsShowVersion(String[] args)
        {
            return args.Length == 1 && String.Equals("--version", args[0]);
        }

        private static Boolean IsShowHelp(String[] args)
        {
            return (args.Length == 0) || (args.Length == 1 && String.Equals("--help", args[0]));
        }

        private const String AppDescription = "Application usage:\r\n" +
                                              "1. {APP} --source {solution-filename.sln|project-filename.csproj|cs-filename.cs}\r\n [--config {config-dir}\r\n] [--verbose]" +
                                              "2. {APP} --help\r\n" +
                                              "3. {APP} --version";
        private const String BadUsageMessage = "Bad usage of the application.";
        private const String VersionNumber = "0.1";
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Processors;

namespace SourceCheckUtil
{
    internal class AppConfig
    {
        public AppConfig(String source, String config)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));
            Source = source;
            Config = config;
        }

        public String Source { get; }

        public String Config { get; }
    }

    public class Program
    {
        public static Int32 Main(String[] args)
        {
            Boolean result = MainImpl(args);
            return result ? 0 : -1;
        }

        private static Boolean MainImpl(String[] args)
        {
            if (IsShowVersion(args))
            {
                Console.WriteLine(VersionNumber);
                return true;
            }
            if (IsShowHelp(args))
            {
                Console.WriteLine(AppDescription);
                return true;
            }
            AppConfig config = TryGetConfig(args);
            if (config != null)
            {
                TextWriter output = Console.Out;
                ISourceProcessor processor = SourceProcessorFactory.Create(config.Source, output);
                IList<IFileAnalyzer> analyzers = AnalyzersFactory.Create(output);
                Boolean processResult = processor.Process(analyzers);
                output.WriteLine($"Result of analysis: analysis is {(processResult ? "succeeded" : "failed")}");
                return processResult;
            }
            Console.WriteLine(BadUsageMessage);
            Console.WriteLine(AppDescription);
            return false;
        }

        private static Boolean IsShowVersion(String[] args)
        {
            return args.Length == 1 && String.Equals("--version", args[0]);
        }

        private static Boolean IsShowHelp(String[] args)
        {
            return (args.Length == 0) || (args.Length == 1 && String.Equals("--help", args[0]));
        }

        private static AppConfig TryGetConfig(String[] args)
        {
            if (args.Length == 2 && String.Equals("--source", args[0]))
                return new AppConfig(args[1], null);
            if (args.Length == 4 && String.Equals("--source", args[0]) && String.Equals("--config", args[2]))
                return new AppConfig(args[1], args[3]);
            if (args.Length == 4 && String.Equals("--source", args[2]) && String.Equals("--config", args[0]))
                return new AppConfig(args[3], args[1]);
            return null;
        }

        private const String AppDescription = "Application usage:\r\n" +
                                              "1. {APP} --source {solution-filename.sln|project-filename.csproj|cs-filename.cs}\r\n" +
                                              "2. {APP} --source {solution-filename.sln|project-filename.csproj|cs-filename.cs} --config {config-dir}\r\n" +
                                              "3. {APP} --help\r\n" +
                                              "4. {APP} --version";
        private const String BadUsageMessage = "Bad usage of the application.";
        private const String VersionNumber = "0.1";
    }
}

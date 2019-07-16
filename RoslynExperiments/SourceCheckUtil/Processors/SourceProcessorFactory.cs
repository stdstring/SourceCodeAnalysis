using System;
using System.Collections.Generic;
using System.IO;

namespace SourceCheckUtil.Processors
{
    internal static class SourceProcessorFactory
    {
        public static ISourceProcessor Create(String source, TextWriter output)
        {
            String sourceExtension = Path.GetExtension(source);
            if (String.IsNullOrEmpty(sourceExtension) || !ProcessorsMap.ContainsKey(sourceExtension))
                throw new ArgumentException(nameof(source));
            return ProcessorsMap[sourceExtension](source, output);
        }

        private static readonly IDictionary<String, Func<String, TextWriter, ISourceProcessor>> ProcessorsMap = new Dictionary<String, Func<String, TextWriter, ISourceProcessor>>
        {
            {".sln", (source, output) => new SolutionProcessor(source, output)},
            {".csproj", (source, output) => new ProjectProcessor(source, output)},
            {".cs", (source, output) => new FileProcessor(source, output)}
        };
    }
}
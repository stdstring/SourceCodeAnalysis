﻿using System;
using System.Collections.Generic;
using System.IO;
using SourceCheckUtil.ExternalConfig;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Processors
{
    internal static class SourceProcessorFactory
    {
        public static ISourceProcessor Create(String source, IExternalConfig externalConfig, OutputImpl output)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            String sourceExtension = Path.GetExtension(source);
            if (String.IsNullOrEmpty(sourceExtension) || !ProcessorsMap.ContainsKey(sourceExtension))
                throw new ArgumentException(nameof(source));
            return ProcessorsMap[sourceExtension](source, externalConfig, output);
        }

        private static readonly IDictionary<String, Func<String, IExternalConfig, OutputImpl, ISourceProcessor>> ProcessorsMap = new Dictionary<String, Func<String, IExternalConfig, OutputImpl, ISourceProcessor>>
        {
            {".sln", (source, config, output) => new SolutionProcessor(source, output)},
            {".csproj", (source, config, output) => new ProjectProcessor(source, output)},
            {".cs", (source, config, output) => new FileProcessor(source, output)}
        };
    }
}
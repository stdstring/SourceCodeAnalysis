using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Config;

namespace SourceCheckUtil.Processors
{
    internal class FileProcessorHelper
    {
        public Boolean Process(String filename, SyntaxTree tree, SemanticModel model, IList<IFileAnalyzer> analyzers, ConfigData externalData)
        {
            Boolean result = true;
            foreach (IFileAnalyzer analyzer in analyzers)
            {
                result &= analyzer.Process(filename, tree, model, externalData);
            }
            return result;
        }

    }
}
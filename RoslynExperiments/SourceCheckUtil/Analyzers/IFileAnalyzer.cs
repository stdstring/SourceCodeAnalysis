using System;
using Microsoft.CodeAnalysis;
using SourceCheckUtil.ExternalConfig;

namespace SourceCheckUtil.Analyzers
{
    public interface IFileAnalyzer
    {
        // TODO (std_string) : think about parameters & return value
        Boolean Process(String filename, SyntaxTree tree, SemanticModel model, ExternalConfigData externalData);
    }
}
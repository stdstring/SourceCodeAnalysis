using System;
using Microsoft.CodeAnalysis;
using SourceCheckUtil.Config;

namespace SourceCheckUtil.Analyzers
{
    public interface IFileAnalyzer
    {
        // TODO (std_string) : think about parameters & return value
        Boolean Process(String filePath, SyntaxTree tree, SemanticModel model, ConfigData externalData);
    }
}
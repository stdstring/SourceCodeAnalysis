using System;
using Microsoft.CodeAnalysis;

namespace SourceCheckUtil.Analyzers
{
    public interface IFileAnalyzer
    {
        // TODO (std_string) : think about parameters & return value
        Boolean Process(String filename, SyntaxTree tree, SemanticModel model);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Analyzers
{
    internal class NonAsciiIdentifiersAnalyzer : IFileAnalyzer
    {
        public NonAsciiIdentifiersAnalyzer(TextWriter output)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            _output = output;
        }

        public Boolean Process(String filename, SyntaxTree tree, SemanticModel model)
        {
            _output.WriteLine($"Execution of NonAsciiIdentifiersAnalyzer started");
            Regex identifierRegex = new Regex("^[a-zA-Z0-9_]+$");
            NonConsistentIdentifiersDetector detector = new NonConsistentIdentifiersDetector(model, identifierRegex);
            detector.Visit(tree.GetRoot());
            Boolean hasErrors = Process(detector.Data);
            _output.WriteLine($"Execution of NonAsciiIdentifiersAnalyzer finished");
            return !hasErrors;
        }

        private Boolean Process(IList<CollectedData<String>> errors)
        {
            Console.WriteLine($"Found {errors.Count} non-ASCII identifiers leading to errors in the ported C++ code");
            foreach (CollectedData<String> error in errors)
            {
                Console.WriteLine($"[ERROR]: Found the following non-ASCII identifier \"{error.Data}\" which are started at {error.StartPosition} and finished at {error.EndPosition}");
            }
            return errors.Count == 0;
        }

        private readonly TextWriter _output;

        private class NonConsistentIdentifiersDetector : CSharpSyntaxWalker
        {
            public NonConsistentIdentifiersDetector(SemanticModel model, Regex identifierRegex) : base(SyntaxWalkerDepth.Token)
            {
                Model = model;
                Data = new List<CollectedData<String>>();
                _identifierRegex = identifierRegex;
            }

            public override void VisitToken(SyntaxToken token)
            {
                FileLinePositionSpan span = token.SyntaxTree.GetLineSpan(token.Span);
                if (token.Kind() == SyntaxKind.IdentifierToken && !_identifierRegex.IsMatch(token.ValueText))
                    Data.Add(new CollectedData<String>(token.ValueText, span.StartLinePosition, span.EndLinePosition));
                base.VisitToken(token);
            }

            public SemanticModel Model { get; }

            public IList<CollectedData<String>> Data { get; }

            private readonly Regex _identifierRegex;
        }
    }
}

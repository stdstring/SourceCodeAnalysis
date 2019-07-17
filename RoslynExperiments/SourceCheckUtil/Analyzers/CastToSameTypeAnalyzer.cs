using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Analyzers
{
    internal class CastToSameTypeAnalyzer : IFileAnalyzer
    {
        public CastToSameTypeAnalyzer(TextWriter output)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            _output = output;
        }

        public Boolean Process(String filename, SyntaxTree tree, SemanticModel model)
        {
            _output.WriteLine($"Execution of CastToSameTypeAnalyzer started");
            CastToSameTypeDetector detector = new CastToSameTypeDetector(model);
            detector.Visit(tree.GetRoot());
            Boolean hasErrors = ProcessErrors(detector.Data);
            ProcessWarnings(detector.Data);
            _output.WriteLine($"Execution of CastToSameTypeAnalyzer finished");
            return !hasErrors;
        }

        private Boolean ProcessErrors(IList<CollectedData<String>> data)
        {
            IList<CollectedData<String>> errors = data.Where(item => _errorCastTypes.Contains(item.Data)).ToList();
            Console.WriteLine($"Found {errors.Count} casts leading to errors in the ported C++ code");
            foreach (CollectedData<String> error in errors)
            {
                Console.WriteLine($"[ERROR]: Found cast to the same type {error.Data} which are started at {error.StartPosition} and finished at {error.EndPosition}");
            }
            return errors.Count > 0;
        }

        private void ProcessWarnings(IList<CollectedData<String>> data)
        {
            IList<CollectedData<String>> warnings = data.Where(item => !_errorCastTypes.Contains(item.Data)).ToList();
            Console.WriteLine($"Found {warnings.Count} casts to the same type not leading to errors in the ported C++ code");
            foreach (CollectedData<String> warning in warnings)
            {
                Console.WriteLine($"[WARNING]: Found cast to the same type {warning.Data} which are started at {warning.StartPosition} and finished at {warning.EndPosition}");
            }
        }

        private readonly TextWriter _output;

        private readonly String[] _errorCastTypes = {"string", "System.String"};

        private class CastToSameTypeDetector : CSharpSyntaxWalker
        {
            public CastToSameTypeDetector(SemanticModel model)
            {
                Model = model;
                Data = new List<CollectedData<String>>();
            }

            public override void VisitCastExpression(CastExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                TypeSyntax typeSyntax = node.Type;
                ExpressionSyntax expressionSyntax = node.Expression;
                TypeInfo typeModel = Model.GetTypeInfo(typeSyntax);
                TypeInfo expressionTypeModel = Model.GetTypeInfo(expressionSyntax);
                if (typeModel.Equals(expressionTypeModel))
                    Data.Add(new CollectedData<String>(typeModel.Type.ToDisplayString(), span.StartLinePosition, span.EndLinePosition));
                base.VisitCastExpression(node);
            }

            public SemanticModel Model { get; }

            public IList<CollectedData<String>> Data { get; }
        }
    }
}

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
    internal class BadFilenameCaseAnalyzer : IFileAnalyzer
    {
        public BadFilenameCaseAnalyzer(TextWriter output)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            _output = output;
        }

        public Boolean Process(String filename, SyntaxTree tree, SemanticModel model)
        {
            _output.WriteLine($"Execution of BadFilenameCaseAnalyzer started");
            TopLevelTypeNamesCollector collector = new TopLevelTypeNamesCollector(model);
            collector.Visit(tree.GetRoot());
            Boolean result = Process(filename, collector.Data);
            _output.WriteLine($"Execution of BadFilenameCaseAnalyzer finished");
            return result;
        }

        private Boolean Process(String filename, IList<CollectedData<String>> data)
        {
            String expectedTypeName = Path.GetFileNameWithoutExtension(filename);
            IList<CollectedData<String>> typeWrongNameCaseList = new List<CollectedData<String>>();
            Boolean exactMatch = false;
            foreach (CollectedData<String> item in data)
            {
                String actualTypeName = item.Data.Split('.').Last();
                if (String.Equals(expectedTypeName, actualTypeName, StringComparison.InvariantCulture))
                    exactMatch = true;
                else if (String.Equals(expectedTypeName, actualTypeName, StringComparison.InvariantCultureIgnoreCase))
                    typeWrongNameCaseList.Add(item);
            }
            if (!exactMatch && typeWrongNameCaseList.Count == 0)
            {
                _output.WriteLine($"[WARNING]: File {filename} doesn't contain any types with names corresponding to the name of this file");
                return true;
            }
            if (!exactMatch && typeWrongNameCaseList.Count == 0)
            {
                _output.WriteLine($"File {filename} doesn't contain any type with name exact match to the filename, but contains {typeWrongNameCaseList.Count} types with names match to the filename with ignoring case");
                foreach (CollectedData<String> typeWrongNameCase in typeWrongNameCaseList)
                {
                    _output.WriteLine($"[ERROR]: File {filename} contains type named {typeWrongNameCase.Data} with name match to the filename with ignoring case");
                }
                return false;
            }
            _output.WriteLine($"File {filename} contains {typeWrongNameCaseList.Count} types with names match to the filename with ignoring case");
            foreach (CollectedData<String> typeWrongNameCase in typeWrongNameCaseList)
            {
                _output.WriteLine($"[WARNING]: File {filename} contains type named {typeWrongNameCase.Data} with name match to the filename with ignoring case");
            }
            return true;
        }

        private readonly TextWriter _output;

        internal class TopLevelTypeNamesCollector : CSharpSyntaxWalker
        {
            public TopLevelTypeNamesCollector(SemanticModel model)
            {
                Model = model;
                Data = new List<CollectedData<String>>();
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, Model.GetDeclaredSymbol(node));
                base.VisitClassDeclaration(node);
            }

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, Model.GetDeclaredSymbol(node));
                base.VisitStructDeclaration(node);
            }

            public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, Model.GetDeclaredSymbol(node));
                base.VisitInterfaceDeclaration(node);
            }

            public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, Model.GetDeclaredSymbol(node));
                base.VisitDelegateDeclaration(node);
            }

            private void VisitTypeDeclarationImpl(MemberDeclarationSyntax node, INamedTypeSymbol type)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (type.ContainingType == null)
                    Data.Add(new CollectedData<String>(type.ToDisplayString(), span.StartLinePosition, span.EndLinePosition));
            }

            public SemanticModel Model { get; }

            public IList<CollectedData<String>> Data { get; }
        }
    }
}

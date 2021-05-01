using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AnalysisExperimentsTests.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AnalysisExperimentsTests
{
    [TestFixture]
    public class BadTypeNameCaseTests
    {
        [TestCase("SomeUtils\\SomeClass.cs", SimpleSource, false, false)]
        [TestCase("SomeUtils\\Someclass.cs", SimpleSource, true, false)]
        [TestCase("SomeUtils\\SomeClass.cs", ComplexSource, false, true)]
        [TestCase("SomeUtils\\Someclass.cs", ComplexSource, true, true)]
        public void ProcessCodeInFile(String filename, String source, Boolean hasError, Boolean hasWarnings)
        {
            IList<CollectedData<String>> topLevelTypeNames = GetTopLevelTypeNames(source);
            String expectedTypeName = Path.GetFileNameWithoutExtension(filename);
            Boolean exactMatch = false;
            Boolean ignoreCaseMatch = false;
            foreach (CollectedData<String> typeName in topLevelTypeNames)
            {
                String actualTypeName = typeName.Data.Split('.').Last();
                if (String.Equals(expectedTypeName, actualTypeName, StringComparison.InvariantCulture))
                    exactMatch = true;
                else if (String.Equals(expectedTypeName, actualTypeName, StringComparison.InvariantCultureIgnoreCase))
                    ignoreCaseMatch = true;
            }
            CheckResult(hasError, hasWarnings, topLevelTypeNames, exactMatch, ignoreCaseMatch);
        }

        private IList<CollectedData<String>> GetTopLevelTypeNames(String source)
        {
            SemanticModel model = PreparationHelper.Prepare(source, "BadTypeNameCaseCheck");
            TopLevelTypeNamesCollector collector = new TopLevelTypeNamesCollector(model);
            collector.Visit(model.SyntaxTree.GetRoot());
            return collector.Data;
        }

        private void CheckResult(Boolean hasError, Boolean hasWarnings, IList<CollectedData<String>> topLevelTypeNames, Boolean exactMatch, Boolean ignoreCaseMatch)
        {
            if (exactMatch)
            {
                Assert.AreEqual(false, hasError);
                Assert.AreEqual(hasWarnings, topLevelTypeNames.Count > 1);
            }
            else if (ignoreCaseMatch)
            {
                Assert.AreEqual(true, hasError);
                Assert.AreEqual(hasWarnings, topLevelTypeNames.Count > 1);
            }
            else
            {
                Assert.AreEqual(false, hasError);
                Assert.IsTrue(hasWarnings);
            }
        }

        private const String SimpleSource = "namespace SomeNamespace\r\n" +
                                             "{\r\n" +
                                             "    public class SomeClass\r\n" +
                                             "    {\r\n" +
                                             "    }\r\n" +
                                             "}";

        private const String ComplexSource = "namespace SomeNamespace\r\n" +
                                             "{\r\n" +
                                             "    public class SomeClass\r\n" +
                                             "    {\r\n" +
                                             "        public class SomeInnerClassA\r\n" +
                                             "        {\r\n" +
                                             "        }\r\n" +
                                             "        public struct SomeInnerStructA\r\n" +
                                             "        {\r\n" +
                                             "        }\r\n" +
                                             "        public interface ISomeInnerInterfaceA\r\n" +
                                             "        {\r\n" +
                                             "        }\r\n" +
                                             "        public delegate void SomeInnerDelegateA(int i);\r\n" +
                                             "    }\r\n" +
                                             "    public class SomeOtherClass\r\n" +
                                             "    {\r\n" +
                                             "    }\r\n" +
                                             "    public struct SomeStruct\r\n" +
                                             "    {\r\n" +
                                             "        public class SomeInnerClassB\r\n" +
                                             "        {\r\n" +
                                             "        }\r\n" +
                                             "        public struct SomeInnerStructB\r\n" +
                                             "        {\r\n" +
                                             "        }\r\n" +
                                             "        public interface ISomeInnerInterfaceB\r\n" +
                                             "        {\r\n" +
                                             "        }\r\n" +
                                             "        public delegate void SomeInnerDelegateB(int i);\r\n" +
                                             "    }\r\n" +
                                             "    public interface ISomeInterface\r\n" +
                                             "    {\r\n" +
                                             "    }\r\n" +
                                             "    public delegate void SomeDelegate(int i);\r\n" +
                                             "}";
    }
}

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
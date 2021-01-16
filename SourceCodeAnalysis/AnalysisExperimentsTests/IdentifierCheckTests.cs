using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AnalysisExperimentsTests.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace AnalysisExperimentsTests
{
    [TestFixture]
    public class IdentifierCheckTests
    {
        [Test]
        public void CheckEnglishIdentifiersOnly()
        {
            const String source = "using System;\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface<TParam1, TParam2>\r\n" +
                                  "    {\r\n" +
                                  "        int SomeProperty { get; set; }\r\n" +
                                  "        void SomeMethod(int iParam, string sParam);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface<Int32, String>\r\n" +
                                  "    {\r\n" +
                                  "        public SomeClass(int someValue)\r\n" +
                                  "        {\r\n" +
                                  "            _someField = someValue;\r\n" +
                                  "        }\r\n" +
                                  "        public int SomeProperty { get; set; }\r\n" +
                                  "        public void SomeMethod(int iParam, string sParam)\r\n" +
                                  "        {\r\n" +
                                  "            int localI = iParam;\r\n" +
                                  "            string localS = sParam;\r\n" +
                                  "            Action<int, string> a = (iiParam, ssParam) => { };\r\n" +
                                  "        }\r\n" +
                                  "        private readonly int _someField;\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new CollectedData<String>[0]);
        }

        [Test]
        public void CheckNonEnglishIdentifiers()
        {
            const String source = "using System;\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    // TPаrаm1 has some russian letters\r\n" +
                                  "    public interface ISomeInterface<TPаrаm1, TParam2>\r\n" +
                                  "    {\r\n" +
                                  "        // SоmеProperty has some russian letters\r\n" +
                                  "        int SоmеProperty { get; set; }\r\n" +
                                  "        void SomeMethod(int iParam, string sParam);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface<Int32, String>\r\n" +
                                  "    {\r\n" +
                                  "        public SomeClass(int someValue)\r\n" +
                                  "        {\r\n" +
                                  "            _someField = someValue;\r\n" +
                                  "        }\r\n" +
                                  "        // SоmеProperty has some russian letters\r\n" +
                                  "        public int SоmеProperty { get; set; }\r\n" +
                                  "        public void SomeMethod(int iParam, string sParam)\r\n" +
                                  "        {\r\n" +
                                  "            // локальнаяI has some russian letters\r\n" +
                                  "            int локальнаяI = iParam;\r\n" +
                                  "            string localS = sParam;\r\n" +
                                  "            Action<int, string> a = (iiParam, ssParam) => { };\r\n" +
                                  "        }\r\n" +
                                  "        private readonly int _someField;\r\n" +
                                  "    }\r\n" +
                                  "}";
            CollectedData<String>[] expectedData =
            {
                new CollectedData<String>("TPаrаm1", new LinePosition(4, 36), new LinePosition(4, 43)),
                new CollectedData<String>("SоmеProperty", new LinePosition(7, 12), new LinePosition(7, 24)),
                new CollectedData<String>("SоmеProperty", new LinePosition(17, 19), new LinePosition(17, 31)),
                new CollectedData<String>("локальнаяI", new LinePosition(21, 16), new LinePosition(21, 26))
            };
            CheckImpl(source, expectedData);
        }

        private void CheckImpl(String source, CollectedData<String>[] expectedData)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            CSharpCompilation compilation = CSharpCompilation.Create("BadIdentifiersCheck")
                .AddReferences(MetadataReference.CreateFromFile(typeof(String).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            AnalysisHelper.CheckCompilationErrors(compilation);
            SemanticModel model = compilation.GetSemanticModel(tree);
            BadIdentifiersDetector detector = new BadIdentifiersDetector(model);
            detector.Visit(root);
            Assert.AreEqual(expectedData, detector.Data);
        }
    }

    internal class BadIdentifiersDetector : CSharpSyntaxWalker
    {
        public BadIdentifiersDetector(SemanticModel model) : base(SyntaxWalkerDepth.Token)
        {
            Model = model;
            Data = new List<CollectedData<String>>();
            _identifierRegex = new Regex("^[a-zA-Z0-9_]+$");
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
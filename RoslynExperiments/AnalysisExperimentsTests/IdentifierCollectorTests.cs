using System;
using System.Collections.Generic;
using AnalysisExperimentsTests.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace AnalysisExperimentsTests
{
    [TestFixture]
    public class IdentifierCollectorTests
    {
        [Test]
        public void CollectEnglishIdentifiersOnly()
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
            CollectedIdentifierData[] expectedData =
            {
                new CollectedIdentifierData("System", new LinePosition(0, 6), new LinePosition(0, 12)),
                new CollectedIdentifierData("SomeNamespace", new LinePosition(1, 10), new LinePosition(1, 23)),
                new CollectedIdentifierData("ISomeInterface", new LinePosition(3, 21), new LinePosition(3, 35)),
                new CollectedIdentifierData("TParam1", new LinePosition(3, 36), new LinePosition(3, 43)),
                new CollectedIdentifierData("TParam2", new LinePosition(3, 45), new LinePosition(3, 52)),
                new CollectedIdentifierData("SomeProperty", new LinePosition(5, 12), new LinePosition(5, 24)),
                new CollectedIdentifierData("SomeMethod", new LinePosition(6, 13), new LinePosition(6, 23)),
                new CollectedIdentifierData("iParam", new LinePosition(6, 28), new LinePosition(6, 34)),
                new CollectedIdentifierData("sParam", new LinePosition(6, 43), new LinePosition(6, 49)),
                new CollectedIdentifierData("SomeClass", new LinePosition(8, 17), new LinePosition(8, 26)),
                new CollectedIdentifierData("ISomeInterface", new LinePosition(8, 29), new LinePosition(8, 43)),
                new CollectedIdentifierData("Int32", new LinePosition(8, 44), new LinePosition(8, 49)),
                new CollectedIdentifierData("String", new LinePosition(8, 51), new LinePosition(8, 57)),
                new CollectedIdentifierData("SomeClass", new LinePosition(10, 15), new LinePosition(10, 24)),
                new CollectedIdentifierData("someValue", new LinePosition(10, 29), new LinePosition(10, 38)),
                new CollectedIdentifierData("_someField", new LinePosition(12, 12), new LinePosition(12, 22)),
                new CollectedIdentifierData("someValue", new LinePosition(12, 25), new LinePosition(12, 34)),
                new CollectedIdentifierData("SomeProperty", new LinePosition(14, 19), new LinePosition(14, 31)),
                new CollectedIdentifierData("SomeMethod", new LinePosition(15, 20), new LinePosition(15, 30)),
                new CollectedIdentifierData("iParam", new LinePosition(15, 35), new LinePosition(15, 41)),
                new CollectedIdentifierData("sParam", new LinePosition(15, 50), new LinePosition(15, 56)),
                new CollectedIdentifierData("localI", new LinePosition(17, 16), new LinePosition(17, 22)),
                new CollectedIdentifierData("iParam", new LinePosition(17, 25), new LinePosition(17, 31)),
                new CollectedIdentifierData("localS", new LinePosition(18, 19), new LinePosition(18, 25)),
                new CollectedIdentifierData("sParam", new LinePosition(18, 28), new LinePosition(18, 34)),
                new CollectedIdentifierData("Action", new LinePosition(19, 12), new LinePosition(19, 18)),
                new CollectedIdentifierData("a", new LinePosition(19, 32), new LinePosition(19, 33)),
                new CollectedIdentifierData("iiParam", new LinePosition(19, 37), new LinePosition(19, 44)),
                new CollectedIdentifierData("ssParam", new LinePosition(19, 46), new LinePosition(19, 53)),
                new CollectedIdentifierData("_someField", new LinePosition(21, 29), new LinePosition(21, 39))
            };
            CheckImpl(source, expectedData);
        }

        [Test]
        public void CollectNonEnglishIdentifiers()
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
            CollectedIdentifierData[] expectedData =
            {
                new CollectedIdentifierData("System", new LinePosition(0, 6), new LinePosition(0, 12)),
                new CollectedIdentifierData("SomeNamespace", new LinePosition(1, 10), new LinePosition(1, 23)),
                new CollectedIdentifierData("ISomeInterface", new LinePosition(4, 21), new LinePosition(4, 35)),
                new CollectedIdentifierData("TPаrаm1", new LinePosition(4, 36), new LinePosition(4, 43)),
                new CollectedIdentifierData("TParam2", new LinePosition(4, 45), new LinePosition(4, 52)),
                new CollectedIdentifierData("SоmеProperty", new LinePosition(7, 12), new LinePosition(7, 24)),
                new CollectedIdentifierData("SomeMethod", new LinePosition(8, 13), new LinePosition(8, 23)),
                new CollectedIdentifierData("iParam", new LinePosition(8, 28), new LinePosition(8, 34)),
                new CollectedIdentifierData("sParam", new LinePosition(8, 43), new LinePosition(8, 49)),
                new CollectedIdentifierData("SomeClass", new LinePosition(10, 17), new LinePosition(10, 26)),
                new CollectedIdentifierData("ISomeInterface", new LinePosition(10, 29), new LinePosition(10, 43)),
                new CollectedIdentifierData("Int32", new LinePosition(10, 44), new LinePosition(10, 49)),
                new CollectedIdentifierData("String", new LinePosition(10, 51), new LinePosition(10, 57)),
                new CollectedIdentifierData("SomeClass", new LinePosition(12, 15), new LinePosition(12, 24)),
                new CollectedIdentifierData("someValue", new LinePosition(12, 29), new LinePosition(12, 38)),
                new CollectedIdentifierData("_someField", new LinePosition(14, 12), new LinePosition(14, 22)),
                new CollectedIdentifierData("someValue", new LinePosition(14, 25), new LinePosition(14, 34)),
                new CollectedIdentifierData("SоmеProperty", new LinePosition(17, 19), new LinePosition(17, 31)),
                new CollectedIdentifierData("SomeMethod", new LinePosition(18, 20), new LinePosition(18, 30)),
                new CollectedIdentifierData("iParam", new LinePosition(18, 35), new LinePosition(18, 41)),
                new CollectedIdentifierData("sParam", new LinePosition(18, 50), new LinePosition(18, 56)),
                new CollectedIdentifierData("локальнаяI", new LinePosition(21, 16), new LinePosition(21, 26)),
                new CollectedIdentifierData("iParam", new LinePosition(21, 29), new LinePosition(21, 35)),
                new CollectedIdentifierData("localS", new LinePosition(22, 19), new LinePosition(22, 25)),
                new CollectedIdentifierData("sParam", new LinePosition(22, 28), new LinePosition(22, 34)),
                new CollectedIdentifierData("Action", new LinePosition(23, 12), new LinePosition(23, 18)),
                new CollectedIdentifierData("a", new LinePosition(23, 32), new LinePosition(23, 33)),
                new CollectedIdentifierData("iiParam", new LinePosition(23, 37), new LinePosition(23, 44)),
                new CollectedIdentifierData("ssParam", new LinePosition(23, 46), new LinePosition(23, 53)),
                new CollectedIdentifierData("_someField", new LinePosition(25, 29), new LinePosition(25, 39))
            };
            CheckImpl(source, expectedData);
        }

        private void CheckImpl(String source, CollectedIdentifierData[] expectedData)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            CSharpCompilation compilation = CSharpCompilation.Create("CollectIdentifiersCheck")
                .AddReferences(MetadataReference.CreateFromFile(typeof(String).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            AnalysisHelper.CheckCompilationErrors(compilation);
            SemanticModel model = compilation.GetSemanticModel(tree);
            IdentifiersCollector detector = new IdentifiersCollector(model);
            detector.Visit(root);
            Assert.AreEqual(expectedData, detector.Data);
        }
    }

    internal class IdentifiersCollector : CSharpSyntaxWalker
    {
        public IdentifiersCollector(SemanticModel model) : base(SyntaxWalkerDepth.Token)
        {
            Model = model;
            Data = new List<CollectedIdentifierData>();
        }

        public override void VisitToken(SyntaxToken token)
        {
            FileLinePositionSpan span = token.SyntaxTree.GetLineSpan(token.Span);
            if (token.Kind() == SyntaxKind.IdentifierToken)
                Data.Add(new CollectedIdentifierData(token.ValueText, span.StartLinePosition, span.EndLinePosition));
            base.VisitToken(token);
        }

        public SemanticModel Model { get; }

        public IList<CollectedIdentifierData> Data { get; }
    }
}

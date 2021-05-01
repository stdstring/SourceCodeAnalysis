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
    public class CastToSameTypeTests
    {
        [Test]
        public void CheckStringToStringCast()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string)s1;\r\n" +
                                  "            string s3 = (string)\"IDKFA\";\r\n" +
                                  "            string s4 = (string)(\"666\" + s1);\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CollectedData<String>[] expectedData =
            {
                new CollectedData<String>("string", new LinePosition(7, 24), new LinePosition(7, 34)),
                new CollectedData<String>("string", new LinePosition(8, 24), new LinePosition(8, 39)),
                new CollectedData<String>("string", new LinePosition(9, 24), new LinePosition(9, 44))
            };
            CheckImpl(source, expectedData);
        }

        [Test]
        public void CheckIntToIntCast()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            int i2 = (int)i1;\r\n" +
                                  "            int i3 = (int)13;\r\n" +
                                  "            int i4 = (int)(19 + i1);\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CollectedData<String>[] expectedData =
            {
                new CollectedData<String>("int", new LinePosition(7, 21), new LinePosition(7, 28)),
                new CollectedData<String>("int", new LinePosition(8, 21), new LinePosition(8, 28)),
                new CollectedData<String>("int", new LinePosition(9, 21), new LinePosition(9, 35))
            };
            CheckImpl(source, expectedData);
        }

        [Test]
        public void CheckToIntCast()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int value = 13;\r\n" +
                                  "            byte bValue = (byte)value;\r\n" +
                                  "            sbyte sbValue = (sbyte)value;\r\n" +
                                  "            short sValue = (short)value;\r\n" +
                                  "            ushort usValue = (ushort)value;\r\n" +
                                  "            int iValue = (int)value;\r\n" +
                                  "            uint uiValue = (uint)value;\r\n" +
                                  "            long lValue = (long)value;\r\n" +
                                  "            ulong ulValue = (ulong)value;\r\n" +
                                  "            float fValue = (float)value;\r\n" +
                                  "            double dValue = (double)value;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new[] {new CollectedData<String>("int", new LinePosition(11, 25), new LinePosition(11, 35))});
        }

        [Test]
        public void CheckWithoutSameTypeCast()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            object value1 = 13;\r\n" +
                                  "            object value2 = \"IDDQD\";\r\n" +
                                  "            object value3 = false;\r\n" +
                                  "            int iValue = (int)value1;\r\n" +
                                  "            string sValue = (string)value2;\r\n" +
                                  "            bool bValue = (bool)value3;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new CollectedData<String>[0]);
        }

        [Test]
        public void CheckFromDynamicCast()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            dynamic value1 = 13;\r\n" +
                                  "            dynamic value2 = \"IDDQD\";\r\n" +
                                  "            dynamic value3 = false;\r\n" +
                                  "            int iValue = (int)value1;\r\n" +
                                  "            string sValue = (string)value2;\r\n" +
                                  "            bool bValue = (bool)value3;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new CollectedData<String>[0]);
        }

        [Test]
        public void CheckForCustomClasses()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}\r\n" +
                                  "namespace SomeOtherNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            object data1 = new SomeNamespace.SomeDerivedClass();\r\n" +
                                  "            SomeNamespace.SomeDerivedClass data2 = (SomeNamespace.SomeDerivedClass)data1;\r\n" +
                                  "            SomeNamespace.SomeDerivedClass data3 = (SomeNamespace.SomeDerivedClass)data2;\r\n" +
                                  "            SomeNamespace.SomeBaseClass data4 = (SomeNamespace.SomeBaseClass)data3;\r\n" +
                                  "            SomeNamespace.SomeDerivedClass data5 = (SomeNamespace.SomeDerivedClass)data4;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new[] {new CollectedData<String>("SomeNamespace.SomeDerivedClass", new LinePosition(17, 51), new LinePosition(17, 88))});
        }

        private void CheckImpl(String source, CollectedData<String>[] expectedData)
        {
            SemanticModel model = PreparationHelper.Prepare(source, "CastToSameType");
            CastToSameTypeDetector detector = new CastToSameTypeDetector(model);
            detector.Visit(model.SyntaxTree.GetRoot());
            Assert.AreEqual(expectedData, detector.Data);
        }
    }

    internal class CastToSameTypeDetector : CSharpSyntaxWalker
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
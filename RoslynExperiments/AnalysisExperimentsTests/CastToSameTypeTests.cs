using System;
using System.Collections.Generic;
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
            CastToSameTypeData[] expectedData =
            {
                new CastToSameTypeData("string", new LinePosition(7, 24), new LinePosition(7, 34)),
                new CastToSameTypeData("string", new LinePosition(8, 24), new LinePosition(8, 39)),
                new CastToSameTypeData("string", new LinePosition(9, 24), new LinePosition(9, 44))
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
            CastToSameTypeData[] expectedData =
            {
                new CastToSameTypeData("int", new LinePosition(7, 21), new LinePosition(7, 28)),
                new CastToSameTypeData("int", new LinePosition(8, 21), new LinePosition(8, 28)),
                new CastToSameTypeData("int", new LinePosition(9, 21), new LinePosition(9, 35))
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
            CheckImpl(source, new[] {new CastToSameTypeData("int", new LinePosition(11, 25), new LinePosition(11, 35))});
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
            CheckImpl(source, new CastToSameTypeData[0]);
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
            CheckImpl(source, new CastToSameTypeData[0]);
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
            CheckImpl(source, new[] {new CastToSameTypeData("SomeNamespace.SomeDerivedClass", new LinePosition(17, 51), new LinePosition(17, 88))});
        }

        private void CheckImpl(String source, CastToSameTypeData[] expectedData)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            CSharpCompilation compilation = CSharpCompilation.Create("CastToSameType")
                .AddReferences(MetadataReference.CreateFromFile(typeof(String).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            AnalysisHelper.CheckCompilationErrors(compilation);
            SemanticModel model = compilation.GetSemanticModel(tree);
            CastToSameTypeDetector detector = new CastToSameTypeDetector(model);
            detector.Visit(root);
            Assert.AreEqual(expectedData, detector.Data);
        }
    }

    internal class CastToSameTypeData : IEquatable<CastToSameTypeData>
    {
        public CastToSameTypeData(String typeName, LinePosition startPosition, LinePosition endPosition)
        {
            TypeName = typeName;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        public String TypeName { get; }

        public LinePosition StartPosition { get; }

        public LinePosition EndPosition { get; }

        public Boolean Equals(CastToSameTypeData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return String.Equals(TypeName, other.TypeName) && StartPosition.Equals(other.StartPosition) && EndPosition.Equals(other.EndPosition);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CastToSameTypeData) obj);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = (TypeName != null ? TypeName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ StartPosition.GetHashCode();
                hashCode = (hashCode * 397) ^ EndPosition.GetHashCode();
                return hashCode;
            }
        }
    }

    internal class CastToSameTypeDetector : CSharpSyntaxWalker
    {
        public CastToSameTypeDetector(SemanticModel model)
        {
            Model = model;
            Data = new List<CastToSameTypeData>();
        }

        public override void VisitCastExpression(CastExpressionSyntax node)
        {
            FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
            TypeSyntax typeSyntax = node.Type;
            ExpressionSyntax expressionSyntax = node.Expression;
            TypeInfo typeModel = Model.GetTypeInfo(typeSyntax);
            TypeInfo expressionTypeModel = Model.GetTypeInfo(expressionSyntax);
            if (typeModel.Equals(expressionTypeModel))
                Data.Add(new CastToSameTypeData(typeModel.Type.ToDisplayString(), span.StartLinePosition, span.EndLinePosition));
            base.VisitCastExpression(node);
        }

        public SemanticModel Model { get; }

        public IList<CastToSameTypeData> Data { get; }
    }
}
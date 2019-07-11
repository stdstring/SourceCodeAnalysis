using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AnalysisExperimentsTests
{
    [TestFixture]
    public class VirtualInheritanceCheckTests
    {
        [Test]
        public void CheckInterfacesWithoutVirtualInheritanceAttrs()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeBaseInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeDerivedInterface : ISomeBaseInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeMultiDerivedInterface : ISomeDerivedInterface, ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface IOtherInterface : System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface IOtherMultiDerivedInterface : System.ICloneable, System.IComparable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface IAnotherInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface IAnotherMultiDerivedInterface : IAnotherInterface, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public interface ISomeInnerInterface\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public interface ISomeBaseInnerInterface\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public interface ISomeDerivedInnerInterface : ISomeBaseInnerInterface\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public interface ISomeMultiDerivedInnerInterface : ISomeDerivedInnerInterface, ISomeInnerInterface\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            String[] expectedData =
            {
                "SomeNamespace.ISomeBaseInterface",
                "SomeNamespace.ISomeInterface",
                "SomeNamespace.IAnotherInterface",
                "SomeNamespace.SomeClass.ISomeBaseInnerInterface",
                "SomeNamespace.SomeClass.ISomeInnerInterface"
            };
            CheckImpl(source, expectedData);
        }

        [Test]
        public void CheckInterfacesWithVirtualInheritanceAttrs()
        {
            const String source = "namespace CsToCppPorter\r\n" +
                                  "{\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface)]\r\n" +
                                  "    public class CppVirtualInheritance : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeDerivedInterface : ISomeBaseInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeMultiDerivedInterface : ISomeDerivedInterface, ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface IOtherInterface : System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface IOtherMultiDerivedInterface : System.ICloneable, System.IComparable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface IAnotherInterface\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface IAnotherMultiDerivedInterface : IAnotherInterface, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "        public interface ISomeInnerInterface\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "        public interface ISomeBaseInnerInterface\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public interface ISomeDerivedInnerInterface : ISomeBaseInnerInterface\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public interface ISomeMultiDerivedInnerInterface : ISomeDerivedInnerInterface, ISomeInnerInterface\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new String[0]);
        }

        [Test]
        public void CheckInterfacesWithSomeVirtualInheritanceAttrs()
        {
            const String source = "namespace CsToCppPorter\r\n" +
                                  "{\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface)]\r\n" +
                                  "    public class CppVirtualInheritance : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeGrandParentInterfaceA\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeGrandParentInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeGrandParentInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeParentInterfaceA : ISomeGrandParentInterfaceA, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeParentInterfaceB : ISomeGrandParentInterfaceB, ISomeGrandParentInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeChildInterfaceA : ISomeGrandParentInterfaceA, ISomeParentInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeChildInterfaceB : System.ICloneable, ISomeParentInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeChildInterfaceC : ISomeGrandParentInterfaceA, System.ICloneable, ISomeGrandParentInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new[] {"SomeNamespace.ISomeGrandParentInterfaceB"});
        }

        [Test]
        public void CheckClassesOnly()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new String[0]);
        }

        [Test]
        public void CheckClassesInterfacesWithoutVirtualInheritanceAttrs()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeBaseInterfaceA\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeBaseInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeDerivedInterface : ISomeBaseInterfaceB, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeBaseClass : ISomeBaseInterfaceA, ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass, ISomeDerivedInterface\r\n" +
                                  "    {\r\n" +
                                  "        public System.Object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new[] {"SomeNamespace.ISomeBaseInterfaceB", "SomeNamespace.ISomeBaseInterfaceA","SomeNamespace.ISomeBaseInterfaceC"});
        }

        [Test]
        public void CheckClassesInterfacesWithVirtualInheritanceAttrs()
        {
            const String source = "namespace CsToCppPorter\r\n" +
                                  "{\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface)]\r\n" +
                                  "    public class CppVirtualInheritance : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterfaceA\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeDerivedInterface : ISomeBaseInterfaceB, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeBaseClass : ISomeBaseInterfaceA, ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass, ISomeDerivedInterface\r\n" +
                                  "    {\r\n" +
                                  "        public System.Object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new String[0]);
        }

        [Test]
        public void CheckClassesInterfacesWitSomeVirtualInheritanceAttrs()
        {
            const String source = "namespace CsToCppPorter\r\n" +
                                  "{\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface)]\r\n" +
                                  "    public class CppVirtualInheritance : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeBaseInterfaceA\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeBaseInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeDerivedInterface : ISomeBaseInterfaceB, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeBaseClass : ISomeBaseInterfaceA, ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass, ISomeDerivedInterface\r\n" +
                                  "    {\r\n" +
                                  "        public System.Object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new[] {"SomeNamespace.ISomeBaseInterfaceB", "SomeNamespace.ISomeBaseInterfaceA"});
        }

        [Test]
        public void CheckStructsOnly()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public struct SomeStructA\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new String[0]);
        }

        [Test]
        public void CheckStructsInterfacesWithoutVirtualInheritanceAttrs()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeBaseInterfaceA\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeBaseInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeDerivedInterface : ISomeBaseInterfaceB, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructA : ISomeBaseInterfaceA, ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructB : ISomeDerivedInterface\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructC : System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new[] {"SomeNamespace.ISomeBaseInterfaceB", "SomeNamespace.ISomeBaseInterfaceA", "SomeNamespace.ISomeBaseInterfaceC"});
        }

        [Test]
        public void CheckStructsInterfacesWithVirtualInheritanceAttrs()
        {
            const String source = "namespace CsToCppPorter\r\n" +
                                  "{\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface)]\r\n" +
                                  "    public class CppVirtualInheritance : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterfaceA\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeDerivedInterface : ISomeBaseInterfaceB, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructA : ISomeBaseInterfaceA, ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructB : ISomeDerivedInterface\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructC : System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new String[0]);
        }

        [Test]
        public void CheckStructsInterfacesWithSomeVirtualInheritanceAttrs()
        {
            const String source = "namespace CsToCppPorter\r\n" +
                                  "{\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface)]\r\n" +
                                  "    public class CppVirtualInheritance : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeBaseInterfaceA\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeBaseInterfaceB\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [CsToCppPorter.CppVirtualInheritance]\r\n" +
                                  "    public interface ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public interface ISomeDerivedInterface : ISomeBaseInterfaceB, System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructA : ISomeBaseInterfaceA, ISomeBaseInterfaceC\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructB : ISomeDerivedInterface\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public struct SomeStructC : System.ICloneable\r\n" +
                                  "    {\r\n" +
                                  "        public object Clone()\r\n" +
                                  "        {\r\n" +
                                  "            throw new System.NotImplementedException();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CheckImpl(source, new[] {"SomeNamespace.ISomeBaseInterfaceB", "SomeNamespace.ISomeBaseInterfaceA"});
        }

        private void CheckImpl(String source, String[] expectedData)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            CSharpCompilation compilation = CSharpCompilation.Create("VirtualInheritanceCheck")
                .AddReferences(MetadataReference.CreateFromFile(typeof(String).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            AnalysisHelper.CheckCompilationErrors(compilation);
            SemanticModel model = compilation.GetSemanticModel(tree);
            InterfaceInheritanceCollector collector = new InterfaceInheritanceCollector(model);
            collector.Visit(root);
            /*foreach (var data in collector.Data)
            {
                Console.WriteLine(data);
            }*/
            Assert.AreEqual(expectedData, collector.Data);
        }
    }

    internal class InterfaceInheritanceCollector : CSharpSyntaxWalker
    {
        public InterfaceInheritanceCollector(SemanticModel model)
        {
            Model = model;
            Data = new List<String>();
        }


        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            INamedTypeSymbol type = Model.GetDeclaredSymbol(node);
            IList<INamedTypeSymbol> baseInterfaces = new List<INamedTypeSymbol>();
            CollectBaseInterfaces(type.Interfaces, baseInterfaces);
            ProcessBaseInterfaces(baseInterfaces);
            base.VisitClassDeclaration(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            INamedTypeSymbol type = Model.GetDeclaredSymbol(node);
            IList<INamedTypeSymbol> baseInterfaces = new List<INamedTypeSymbol>();
            CollectBaseInterfaces(type.Interfaces, baseInterfaces);
            ProcessBaseInterfaces(baseInterfaces);
            base.VisitStructDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            INamedTypeSymbol type = Model.GetDeclaredSymbol(node);
            IList<INamedTypeSymbol> baseInterfaces = new List<INamedTypeSymbol>();
            CollectBaseInterfaces(type, baseInterfaces);
            ProcessBaseInterfaces(baseInterfaces);
            base.VisitInterfaceDeclaration(node);
        }

        public SemanticModel Model { get; }

        public IList<String> Data { get; }

        // TODO (std_string) : probably move into extension methods
        private Boolean IsSystemType(INamedTypeSymbol type)
        {
            // TODO (std_string) : think about approach
            return type.ToDisplayString().StartsWith(SystemPrefix);
        }

        // TODO (std_string) : probably move into extension methods
        private Boolean HasVirtualInheritanceAttr(INamedTypeSymbol type)
        {
            return type.GetAttributes().Any(attr => String.Equals(attr.AttributeClass.ToDisplayString(), VirtualInheritanceAttribute));
        }

        // TODO (std_string) : create non-recursive version
        private void CollectBaseInterfaces(IList<INamedTypeSymbol> symbols, IList<INamedTypeSymbol> dest)
        {
            foreach (INamedTypeSymbol symbol in symbols)
                CollectBaseInterfaces(symbol, dest);
        }

        private void CollectBaseInterfaces(INamedTypeSymbol symbol, IList<INamedTypeSymbol> dest)
        {
            if (symbol.Interfaces.Length == 0)
                dest.Add(symbol);
            CollectBaseInterfaces(symbol.Interfaces, dest);
        }

        private void ProcessBaseInterfaces(IList<INamedTypeSymbol> baseInterfaces)
        {
            IList<INamedTypeSymbol> systemBaseInterfaces = baseInterfaces.Where(IsSystemType).ToList();
            IList<INamedTypeSymbol> nonsystemBaseInterfaces = baseInterfaces.Where(i => !IsSystemType(i)).ToList();
            if (nonsystemBaseInterfaces.Count == 0)
                return;
            if (systemBaseInterfaces.Count == 0 && nonsystemBaseInterfaces.Count == 1)
                return;
            foreach (INamedTypeSymbol type in nonsystemBaseInterfaces.Where(i => !HasVirtualInheritanceAttr(i)))
            {
                String typename = type.ToDisplayString();
                if (!Data.Contains(typename))
                    Data.Add(typename);
            }
        }

        private const String SystemPrefix = "System.";

        private const String VirtualInheritanceAttribute = "CsToCppPorter.CppVirtualInheritance";
    }
}
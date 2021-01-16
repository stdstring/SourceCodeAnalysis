using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AnalysisExperimentsTests
{
    [TestFixture]
    public class SimpleParseExampleTests
    {
        [Test]
        public void ParseSimpleClass()
        {
            const String source = "using System;\r\n" +
                                  "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod(Int32 i)\r\n" +
                                  "        {\r\n" +
                                  "            // Some method body\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            CSharpCompilation compilation = CSharpCompilation.Create("SimpleParseExample")
                .AddReferences(MetadataReference.CreateFromFile(typeof(String).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            AnalysisHelper.CheckCompilationErrors(compilation);
            SemanticModel model = compilation.GetSemanticModel(tree);
            Console.WriteLine($"root.Kind = {root.Kind()}");
            Console.WriteLine();
            Console.WriteLine("Usings:");
            foreach (UsingDirectiveSyntax usingDirective in root.Usings)
            {
                INamespaceSymbol usingModel = (INamespaceSymbol) model.GetSymbolInfo(usingDirective.Name).Symbol;
                Console.WriteLine($"namepsace = {usingDirective.Name}, assembly = {usingModel.ContainingAssembly.Name}");
            }
            Console.WriteLine();
            NamespaceDeclarationSyntax namespaceDeclaration = (NamespaceDeclarationSyntax) root.Members[0];
            INamespaceSymbol namespaceModel = (INamespaceSymbol) model.GetSymbolInfo(namespaceDeclaration.Name).Symbol;
            Console.WriteLine($"NamespaceDeclaration: namespace = {namespaceDeclaration.Name}, assembly = {namespaceModel.ContainingAssembly.Name}");
            Console.WriteLine();
            ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax) namespaceDeclaration.Members[0];
            INamedTypeSymbol classModel = model.GetDeclaredSymbol(classDeclaration);
            INamedTypeSymbol baseClassModel = classModel.BaseType;
            Console.WriteLine($"ClassDeclaration: modifiers = {classDeclaration.Modifiers}, name = {classModel.ToDisplayString()}, base type = {baseClassModel.ToDisplayString()}");
            Console.WriteLine();
            MethodDeclarationSyntax methodDeclaration = (MethodDeclarationSyntax) classDeclaration.Members[0];
            IMethodSymbol methodModel = model.GetDeclaredSymbol(methodDeclaration);
            String parameters = String.Join(", ", methodModel.Parameters.Select(p => $"type = {p.Type.ToDisplayString()}, name = {p.Name}"));
            Console.WriteLine($"MethodDeclaration: modifiers = {methodDeclaration.Modifiers}, name = {methodModel.Name}, return type ={methodModel.ReturnType.ToDisplayString()}, params = ({parameters})");
            Console.WriteLine("Method body:");
            Console.WriteLine(methodDeclaration.Body.ToFullString());
            Console.WriteLine();
        }
    }
}
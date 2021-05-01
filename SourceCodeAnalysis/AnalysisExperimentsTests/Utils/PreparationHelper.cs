using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace AnalysisExperimentsTests.Utils
{
    // TODO (std_string) : think about move into some common lib
    public static class PreparationHelper
    {
        public static void CheckCompilationErrors(CSharpCompilation compilation)
        {
            Console.WriteLine("Checking compilation for errors, warnings and infos:");
            IList<Diagnostic> diagnostics = compilation.GetDiagnostics();
            /*IList<Diagnostic> declarationDiagnostics = compilation.GetDeclarationDiagnostics();
            IList<Diagnostic> methodDiagnostics = compilation.GetMethodBodyDiagnostics();
            IList<Diagnostic> parseDiagnostics = compilation.GetParseDiagnostics();*/
            Boolean hasErrors = false;
            foreach (Diagnostic diagnostic in diagnostics)
            {
                Console.WriteLine($"Diagnostic message: severity = {diagnostic.Severity}, message = \"{diagnostic.GetMessage()}\"");
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                    hasErrors = true;
            }
            Assert.IsFalse(hasErrors);
            if (diagnostics.Count == 0)
                Console.WriteLine("No any errors, warnings and infos");
            Console.WriteLine();
        }

        public static SemanticModel Prepare(String source, String assemblyName)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            CSharpCompilation compilation = CSharpCompilation.Create(assemblyName)
                .AddReferences(MetadataReference.CreateFromFile(typeof(String).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            CheckCompilationErrors(compilation);
            return compilation.GetSemanticModel(tree);
        }
    }
}

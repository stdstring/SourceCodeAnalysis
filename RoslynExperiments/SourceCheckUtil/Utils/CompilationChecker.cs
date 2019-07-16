using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;

namespace SourceCheckUtil.Utils
{
    internal static class CompilationChecker
    {
        public static Boolean CheckCompilationErrors(Compilation compilation, TextWriter output)
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
            if (diagnostics.Count == 0)
                Console.WriteLine("No any errors, warnings and infos");
            Console.WriteLine();
            return !hasErrors;
        }
    }
}
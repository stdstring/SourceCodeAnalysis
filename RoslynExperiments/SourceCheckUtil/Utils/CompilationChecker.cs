using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace SourceCheckUtil.Utils
{
    internal static class CompilationChecker
    {
        public static Boolean CheckCompilationErrors(Compilation compilation, OutputImpl output)
        {
            output.WriteOutputLine("Checking compilation for errors, warnings and infos:");
            IList<Diagnostic> diagnostics = compilation.GetDiagnostics();
            /*IList<Diagnostic> declarationDiagnostics = compilation.GetDeclarationDiagnostics();
            IList<Diagnostic> methodDiagnostics = compilation.GetMethodBodyDiagnostics();
            IList<Diagnostic> parseDiagnostics = compilation.GetParseDiagnostics();*/
            Boolean hasErrors = false;
            foreach (Diagnostic diagnostic in diagnostics)
            {
                output.WriteOutputLine($"Diagnostic message: severity = {diagnostic.Severity}, message = \"{diagnostic.GetMessage()}\"");
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                    hasErrors = true;
            }
            if (diagnostics.Count == 0)
                output.WriteOutputLine("No any errors, warnings and infos");
            output.WriteOutputLine();
            return !hasErrors;
        }
    }
}
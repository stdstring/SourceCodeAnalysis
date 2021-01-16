using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SourceCheckUtil.Utils
{
    internal static class CompilationChecker
    {
        public static Boolean CheckCompilationErrors(String filename, Compilation compilation, OutputImpl output)
        {
            output.WriteOutputLine("Checking compilation for errors, warnings and infos:");
            IList<Diagnostic> diagnostics = compilation.GetDiagnostics();
            /*IList<Diagnostic> declarationDiagnostics = compilation.GetDeclarationDiagnostics();
            IList<Diagnostic> methodDiagnostics = compilation.GetMethodBodyDiagnostics();
            IList<Diagnostic> parseDiagnostics = compilation.GetParseDiagnostics();*/
            Diagnostic[] diagnosticErrors = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
            Diagnostic[] diagnosticWarnings = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).ToArray();
            Diagnostic[] diagnosticInfos = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info || d.Severity == DiagnosticSeverity.Hidden).ToArray();
            Boolean hasErrors = false;
            output.WriteOutputLine($"Found {diagnosticErrors.Length} errors in the compilation");
            foreach (Diagnostic diagnostic in diagnosticErrors)
            {
                output.WriteErrorLine($"[ERROR]: Found following error in the compilation of the {filename} entity: {diagnostic.GetMessage()}");
                hasErrors = true;
            }
            output.WriteOutputLine($"Found {diagnosticWarnings.Length} warnings in the compilation");
            foreach (Diagnostic diagnostic in diagnosticWarnings)
            {
                output.WriteOutputLine($"[WARNING]: Found following warning in the compilation: {diagnostic.GetMessage()}");
            }
            output.WriteOutputLine($"Found {diagnosticInfos.Length} info entries in the compilation");
            foreach (Diagnostic diagnostic in diagnosticInfos)
            {
                output.WriteOutputLine($"Found following info entry in the compilation: {diagnostic.GetMessage()}");
            }
            if (diagnostics.Count == 0)
                output.WriteOutputLine("No any errors, warnings and infos");
            output.WriteOutputLine();
            return !hasErrors;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Processors
{
    internal class SolutionProcessor : ISourceProcessor
    {
        public SolutionProcessor(String solutionFilename, TextWriter output)
        {
            if (String.IsNullOrEmpty(solutionFilename))
                throw new ArgumentNullException(nameof(solutionFilename));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            _solutionFilename = solutionFilename;
            _output = output;
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteLine($"Processing of the solution {_solutionFilename} is started");
            _output.WriteLine();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(_solutionFilename).Result;
            Boolean result = true;
            foreach (Project project in solution.Projects)
            {
                result &= Process(project, analyzers);
            }
            _output.WriteLine($"Processing of the solution {_solutionFilename} is finished");
            _output.WriteLine();
            return result;
        }

        private Boolean Process(Project project, IList<IFileAnalyzer> analyzers)
        {
            _output.WriteLine($"Processing of the project {project.FilePath} is started");
            _output.WriteLine();
            Compilation compilation = project.GetCompilationAsync().Result;
            if (!CompilationChecker.CheckCompilationErrors(compilation, _output))
                return false;
            Boolean result = true;
            foreach (Document file in project.Documents.Where(doc => doc.SourceCodeKind == SourceCodeKind.Regular && !ProjectIgnoredFiles.IgnoreFile(doc.FilePath)))
            {
                result &= Process(file, compilation, analyzers);
            }
            _output.WriteLine($"Processing of the project {project.FilePath} is finished");
            _output.WriteLine();
            return result;
        }

        private Boolean Process(Document file, Compilation compilation, IList<IFileAnalyzer> analyzers)
        {
            Boolean result = true;
            _output.WriteLine($"Processing of the file {file.FilePath} is started");
            _output.WriteLine();
            SyntaxTree tree = file.GetSyntaxTreeAsync().Result;
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (IFileAnalyzer analyzer in analyzers)
            {
                result &= analyzer.Process(file.FilePath, tree, model);
            }
            _output.WriteLine($"Processing of the file {file.FilePath} is finished");
            _output.WriteLine();
            return result;
        }

        private readonly String _solutionFilename;
        private readonly TextWriter _output;
    }
}
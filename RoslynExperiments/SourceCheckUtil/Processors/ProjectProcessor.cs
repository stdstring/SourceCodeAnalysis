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
    internal class ProjectProcessor : ISourceProcessor
    {
        public ProjectProcessor(String projectFilename, TextWriter output)
        {
            if (String.IsNullOrEmpty(projectFilename))
                throw new ArgumentNullException(nameof(projectFilename));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            _projectFilename = projectFilename;
            _output = output;
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteLine($"Processing project {_projectFilename}");
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Project project = workspace.OpenProjectAsync(_projectFilename).Result;
            Compilation compilation = project.GetCompilationAsync().Result;
            if (!CompilationChecker.CheckCompilationErrors(compilation, _output))
                return false;
            Boolean result = true;
            foreach (Document file in project.Documents.Where(doc => doc.SourceCodeKind == SourceCodeKind.Regular && !ProjectIgnoredFiles.IgnoreFile(doc.FilePath)))
            {
                result &= Process(file, compilation, analyzers);
            }
            return result;
        }

        private Boolean Process(Document file, Compilation compilation, IList<IFileAnalyzer> analyzers)
        {
            Boolean result = true;
            _output.WriteLine($"Processing file {file.FilePath}");
            SyntaxTree tree = file.GetSyntaxTreeAsync().Result;
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (IFileAnalyzer analyzer in analyzers)
            {
                result &= analyzer.Process(file.FilePath, tree, model);
            }
            return result;
        }

        private readonly String _projectFilename;
        private readonly TextWriter _output;
    }
}
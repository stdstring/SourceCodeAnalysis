using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using SourceCheckUtil.Analyzers;

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
            _processorHelper = new ProcessorHelper(output);
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteLine($"Processing of the project {_projectFilename} is started");
            _output.WriteLine();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Project project = workspace.OpenProjectAsync(_projectFilename).Result;
            Boolean result = _processorHelper.ProcessProject(project, analyzers, Process);
            _output.WriteLine($"Processing of the project {_projectFilename} is finished");
            _output.WriteLine();
            return result;
        }

        private Boolean Process(Document file, Compilation compilation, IList<IFileAnalyzer> analyzers)
        {
            _output.WriteLine($"Processing of the file {file.FilePath} is started");
            _output.WriteLine();
            SyntaxTree tree = file.GetSyntaxTreeAsync().Result;
            SemanticModel model = compilation.GetSemanticModel(tree);
            Boolean result = _processorHelper.ProcessFile(file.FilePath, tree, model, analyzers);
            _output.WriteLine($"Processing of the file {file.FilePath} is finished");
            _output.WriteLine();
            return result;
        }

        private readonly String _projectFilename;
        private readonly TextWriter _output;
        private readonly ProcessorHelper _processorHelper;
    }
}
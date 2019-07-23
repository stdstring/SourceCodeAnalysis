﻿using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.ExternalConfig;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Processors
{
    internal class ProjectProcessor : ISourceProcessor
    {
        public ProjectProcessor(String projectFilename, IExternalConfig externalConfig, OutputImpl output)
        {
            if (String.IsNullOrEmpty(projectFilename))
                throw new ArgumentNullException(nameof(projectFilename));
            if (externalConfig == null)
                throw new ArgumentNullException(nameof(externalConfig));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            _projectFilename = projectFilename;
            _output = output;
            _processorHelper = new ProjectProcessorHelper(externalConfig, output);
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteOutputLine($"Processing of the project {_projectFilename} is started");
            _output.WriteOutputLine();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Project project = workspace.OpenProjectAsync(_projectFilename).Result;
            Boolean result = _processorHelper.ProcessProject(project, analyzers, Process);
            _output.WriteOutputLine($"Processing of the project {_projectFilename} is finished");
            _output.WriteOutputLine();
            return result;
        }

        private Boolean Process(Document file, Compilation compilation, ExternalConfigData externalData, IList<IFileAnalyzer> analyzers)
        {
            _output.WriteOutputLine($"Processing of the file {file.FilePath} is started");
            _output.WriteOutputLine();
            SyntaxTree tree = file.GetSyntaxTreeAsync().Result;
            SemanticModel model = compilation.GetSemanticModel(tree);
            Boolean result = _processorHelper.ProcessFile(file.FilePath, tree, model, externalData, analyzers);
            _output.WriteOutputLine($"Processing of the file {file.FilePath} is finished");
            _output.WriteOutputLine();
            return result;
        }

        private readonly String _projectFilename;
        private readonly OutputImpl _output;
        private readonly ProjectProcessorHelper _processorHelper;
    }
}
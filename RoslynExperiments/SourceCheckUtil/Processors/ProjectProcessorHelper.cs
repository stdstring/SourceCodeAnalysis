using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.ExternalConfig;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Processors
{
    internal class ProjectProcessorHelper
    {
        public ProjectProcessorHelper(IExternalConfig externalConfig, OutputImpl output)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            if (externalConfig == null)
                throw new ArgumentNullException(nameof(externalConfig));
            _externalConfig = externalConfig;
            _output = output;
            _fileProcessor = new FileProcessorHelper();
        }

        public Boolean ProcessProject(Project project, IList<IFileAnalyzer> analyzers, Func<Document, Compilation, ExternalConfigData, IList<IFileAnalyzer>, Boolean> fileProcessor)
        {
            Compilation compilation = project.GetCompilationAsync().Result;
            if (!CompilationChecker.CheckCompilationErrors(project.FilePath, compilation, _output))
                return false;
            Boolean result = true;
            ExternalConfigData configData = _externalConfig.Load(project.Name);
            foreach (Document file in project.Documents.Where(doc => doc.SourceCodeKind == SourceCodeKind.Regular && !ProjectIgnoredFiles.IgnoreFile(doc.FilePath)))
            {
                result &= fileProcessor(file, compilation, configData, analyzers);
            }
            return result;
        }

        public Boolean ProcessFile(String filename, SyntaxTree tree, SemanticModel model, ExternalConfigData externalData, IList<IFileAnalyzer> analyzers)
        {
            return _fileProcessor.Process(filename, tree, model, analyzers, externalData);
        }

        private readonly IExternalConfig _externalConfig;
        private readonly OutputImpl _output;
        private readonly FileProcessorHelper _fileProcessor;
    }
}
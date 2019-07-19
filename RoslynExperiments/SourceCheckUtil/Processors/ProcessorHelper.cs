using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Processors
{
    internal class ProcessorHelper
    {
        public ProcessorHelper(OutputImpl output)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            _output = output;
        }

        public Boolean ProcessProject(Project project, IList<IFileAnalyzer> analyzers, Func<Document, Compilation, IList<IFileAnalyzer>, Boolean> fileProcessor)
        {
            Compilation compilation = project.GetCompilationAsync().Result;
            if (!CompilationChecker.CheckCompilationErrors(project.FilePath, compilation, _output))
                return false;
            Boolean result = true;
            foreach (Document file in project.Documents.Where(doc => doc.SourceCodeKind == SourceCodeKind.Regular && !ProjectIgnoredFiles.IgnoreFile(doc.FilePath)))
            {
                result &= fileProcessor(file, compilation, analyzers);
            }
            return result;
        }

        public Boolean ProcessFile(String filename, SyntaxTree tree, SemanticModel model, IList<IFileAnalyzer> analyzers)
        {
            Boolean result = true;
            foreach (IFileAnalyzer analyzer in analyzers)
            {
                result &= analyzer.Process(filename, tree, model);
            }
            return result;
        }

        private readonly OutputImpl _output;
    }
}
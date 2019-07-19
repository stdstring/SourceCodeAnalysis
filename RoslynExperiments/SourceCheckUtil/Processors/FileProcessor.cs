using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Processors
{
    internal class FileProcessor : ISourceProcessor
    {
        public FileProcessor(String filename, OutputImpl output)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            _filename = filename;
            _output = output;
            _processorHelper = new ProcessorHelper(output);
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteOutputLine($"Processing of the file {_filename} is started");
            _output.WriteOutputLine();
            String source = File.ReadAllText(_filename);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            Compilation compilation = CreateCompilation(tree);
            if (!CompilationChecker.CheckCompilationErrors(_filename, compilation, _output))
                return false;
            SemanticModel model = compilation.GetSemanticModel(tree);
            Boolean result = _processorHelper.ProcessFile(_filename, tree, model, analyzers);
            _output.WriteOutputLine($"Processing of the file {_filename} is finished");
            _output.WriteOutputLine();
            return result;
        }

        private CSharpCompilation CreateCompilation(SyntaxTree tree)
        {
            String assemblyName = Path.GetFileNameWithoutExtension(_filename);
            return CSharpCompilation.Create(assemblyName)
                // mscorlib
                .AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location))
                // System.Core.dll
                .AddReferences(MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        private readonly String _filename;
        private readonly OutputImpl _output;
        private readonly ProcessorHelper _processorHelper;
    }
}
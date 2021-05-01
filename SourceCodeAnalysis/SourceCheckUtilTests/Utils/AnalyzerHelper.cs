using System;
using System.IO;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Config;
using SourceCheckUtil.Utils;

namespace SourceCheckUtilTests.Utils
{
    internal static class AnalyzerHelper
    {
        public static void Process(Func<OutputImpl, IFileAnalyzer> analyzerFactory, String source, String assemblyName, String filePath, Boolean verboseOutput, Boolean expectedResult, String expectedOutput, String expectedError)
        {
            SemanticModel model = PreparationHelper.Prepare(source, assemblyName);
            ConfigData externalData = new ConfigData();
            using (TextWriter outputWriter = new StringWriter())
            using (TextWriter errorWriter = new StringWriter())
            {
                OutputImpl output = new OutputImpl(outputWriter, errorWriter, verboseOutput);
                IFileAnalyzer analyzer = analyzerFactory(output);
                Boolean actualResult = analyzer.Process(filePath, model.SyntaxTree, model, externalData);
                Assert.AreEqual(expectedResult, actualResult);
                Assert.AreEqual(expectedOutput, outputWriter.ToString());
                Assert.AreEqual(expectedError, errorWriter.ToString());
            }
        }
    }
}
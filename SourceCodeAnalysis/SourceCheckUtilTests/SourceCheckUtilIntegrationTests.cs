using System;
using System.IO;
using NUnit.Framework;
using SourceCheckUtil.Output;
using SourceCheckUtilTests.Utils;

namespace SourceCheckUtilTests
{
    [TestFixture]
    public class SourceCheckUtilIntegrationTests
    {
        [TearDown]
        public void TearDown()
        {
            EnvironmentHelper.RemoveAuxiliaryEntities();
        }

        [Test]
        public void ProcessEmptyArgs()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("");
            ExecutionChecker.Check(executionResult, 0, SourceCheckUtilOutputDef.AppDescription, "");
        }

        [Test]
        public void ProcessHelp()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--help");
            ExecutionChecker.Check(executionResult, 0, SourceCheckUtilOutputDef.AppDescription, "");
        }

        [Test]
        public void ProcessVersion()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--version");
            ExecutionChecker.Check(executionResult, 0, "0.9\r\n", "");
        }

        [TestCase("--some-strange-option")]
        [TestCase("--source=\"..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj\" --some-strange-option")]
        public void ProcessUnknownArg(String args)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--some-strange-option");
            ExecutionChecker.Check(executionResult, -1, SourceCheckUtilOutputDef.AppDescription, SourceCheckUtilOutputDef.BadUsageMessage);
        }

        [Test]
        public void ProcessSourceWithoutValue()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--source= --output-level=Error");
            ExecutionChecker.Check(executionResult, -1, "", SourceCheckUtilOutputDef.BadSourceMessage);
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessUnknownSource(OutputLevel outputLevel)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("SomeUnknownExample.csproj", null, outputLevel);
            ExecutionChecker.Check(executionResult, -1, "", "[ERROR]: Bad (unknown) target SomeUnknownExample.csproj\r\n");
        }

        [Test]
        public void ProcessUnknownSourceInfo()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("SomeUnknownExample.csproj", null, OutputLevel.Info);
            const String expectedOutput = "Processing of the project SomeUnknownExample.csproj is started\r\nResult of analysis: analysis is failed\r\n";
            ExecutionChecker.Check(executionResult, -1, expectedOutput, "[ERROR]: Bad (unknown) target SomeUnknownExample.csproj\r\n");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        [TestCase(OutputLevel.Info)]
        public void ProcessUnknownConfig(OutputLevel outputLevel)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj", "..\\SomeConfig.config", outputLevel);
            ExecutionChecker.Check(executionResult, -1, "", SourceCheckUtilOutputDef.BadConfigMessage);
        }

        [Test]
        public void ProcessConfigWithoutSource()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\Examples\\ConfigUsageExample\\Config\"");
            ExecutionChecker.Check(executionResult, -1, SourceCheckUtilOutputDef.AppDescription, SourceCheckUtilOutputDef.BadUsageMessage);
        }

        [Test]
        public void ProcessConfigWithoutValue()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--source=\"..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln\" --config=");
            ExecutionChecker.Check(executionResult, -1, "", SourceCheckUtilOutputDef.BadConfigMessage);
        }

        [Test]
        public void ProcessBadOutputLevel()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--source=\"..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln\" --output-value=XXX");
            ExecutionChecker.Check(executionResult, -1, SourceCheckUtilOutputDef.AppDescription, SourceCheckUtilOutputDef.BadUsageMessage);
        }

        [Test]
        public void ProcessOutputLevelWithoutValue()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--source=\"..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln\" --output-value=");
            ExecutionChecker.Check(executionResult, -1, SourceCheckUtilOutputDef.AppDescription, SourceCheckUtilOutputDef.BadUsageMessage);
        }

        [Test]
        public void ProcessGoodExampleProjectError()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj", null, OutputLevel.Error);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessGoodExampleProjectWarning()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj", null, OutputLevel.Warning);
            String projectPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\GoodExample"));
            const String expectedOutputTemplate = "{0}\\CastsExample.cs(24): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                                  "{0}\\CastsExample.cs(28): [WARNING]: Found cast to the same type \"GoodExample.SomeBaseClass\"\r\n" +
                                                  "{0}\\ClassNameExample.cs(7): [WARNING]: Found type named \"GoodExample.Classnameexample\" which corresponds the filename \"ClassNameExample.cs\" only at ignoring case\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, projectPath);
            ExecutionChecker.Check(executionResult, 0, expectedOutput, "");
        }

        [Test]
        public void ProcessGoodExampleProjectInfo()
        {
            const String projectFilename = "..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj";
            ExecutionResult executionResult = ExecutionHelper.Execute(projectFilename, null, OutputLevel.Info);
            String projectDirPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\GoodExample"));
            const String expectedOutputTemplate = "Processing of the project {0} is started\r\n" +
                                                  SourceCheckUtilOutputDef.CompilationCheckSuccessOutput +
                                                  "Processing of the file {1}\\CastsExample.cs is started\r\n" +
                                                  SourceCheckUtilOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  "Execution of CastToSameTypeAnalyzer started\r\n" +
                                                  "Found 0 casts leading to errors in the ported C++ code\r\n" +
                                                  "Found 2 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                                  "{1}\\CastsExample.cs(24): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                                  "{1}\\CastsExample.cs(28): [WARNING]: Found cast to the same type \"GoodExample.SomeBaseClass\"\r\n" +
                                                  "Execution of CastToSameTypeAnalyzer finished\r\n" +
                                                  SourceCheckUtilOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  "Processing of the file {1}\\CastsExample.cs is finished\r\n" +
                                                  "Processing of the file {1}\\ClassNameExample.cs is started\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                                  "File contains 1 types with names match to the filename with ignoring case\r\n" +
                                                  "{1}\\ClassNameExample.cs(7): [WARNING]: Found type named \"GoodExample.Classnameexample\" which corresponds the filename \"ClassNameExample.cs\" only at ignoring case\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer finished\r\n" +
                                                  SourceCheckUtilOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  "Processing of the file {1}\\ClassNameExample.cs is finished\r\n" +
                                                  "Processing of the file {1}\\IdentifiersExample.cs is started\r\n" +
                                                  SourceCheckUtilOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  "Processing of the file {1}\\IdentifiersExample.cs is finished\r\n" +
                                                  "Processing of the project {0} is finished\r\n" +
                                                  "Result of analysis: analysis is succeeded\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, projectFilename, projectDirPath);
            ExecutionChecker.Check(executionResult, 0, expectedOutput, "");
        }

        [Test]
        public void ProcessBadExampleProjectError()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\BadExample\\BadExample.csproj", null, OutputLevel.Error);
            const String expectedOutputTemplate = "{0}\\CastsExample.cs(26): [ERROR]: Found cast to the same type \"string\"\r\n" +
                                                  "{0}\\ClassnameExample.cs(3): [ERROR]: Found type named \"BadExample.ClassNameExample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\ClassnameExample.cs(7): [ERROR]: Found type named \"BadExample.Classnameexample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\IdentifiersExample.cs(7): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(15): [ERROR]: Found non-ASCII identifier \"TPаrаm1\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(19): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(23): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(30): [ERROR]: Found non-ASCII identifier \"somеObjB\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(32): [ERROR]: Found non-ASCII identifier \"someImplОbj\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"локальноеДействие\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"парам1\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"парам2\"\r\n";
            String projectPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\BadExample"));
            String expectedOutput = String.Format(expectedOutputTemplate, projectPath);
            ExecutionChecker.Check(executionResult, -1, expectedOutput, "");
        }

        [Test]
        public void ProcessBadExampleProjectWarning()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\BadExample\\BadExample.csproj", null, OutputLevel.Warning);
            const String expectedOutputTemplate = "{0}\\CastsExample.cs(26): [ERROR]: Found cast to the same type \"string\"\r\n" +
                                                  "{0}\\CastsExample.cs(24): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                                  "{0}\\CastsExample.cs(29): [WARNING]: Found cast to the same type \"BadExample.SomeBaseClass\"\r\n" +
                                                  "{0}\\ClassnameExample.cs(3): [ERROR]: Found type named \"BadExample.ClassNameExample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\ClassnameExample.cs(7): [ERROR]: Found type named \"BadExample.Classnameexample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\IdentifiersExample.cs(7): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(15): [ERROR]: Found non-ASCII identifier \"TPаrаm1\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(19): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(23): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(30): [ERROR]: Found non-ASCII identifier \"somеObjB\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(32): [ERROR]: Found non-ASCII identifier \"someImplОbj\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"локальноеДействие\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"парам1\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"парам2\"\r\n";
            String projectPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\BadExample"));
            String expectedOutput = String.Format(expectedOutputTemplate, projectPath);
            ExecutionChecker.Check(executionResult, -1, expectedOutput, "");
        }

        [Test]
        public void ProcessBadExampleProjectInfo()
        {
            const String projectFilename = "..\\..\\..\\Examples\\BadExample\\BadExample.csproj";
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\BadExample\\BadExample.csproj", null, OutputLevel.Info);
            const String expectedOutputTemplate = "Processing of the project {0} is started\r\n" +
                                                  SourceCheckUtilOutputDef.CompilationCheckSuccessOutput +
                                                  "Processing of the file {1}\\CastsExample.cs is started\r\n" +
                                                  SourceCheckUtilOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  "Execution of CastToSameTypeAnalyzer started\r\n" +
                                                  "Found 1 casts leading to errors in the ported C++ code\r\n" +
                                                  "{1}\\CastsExample.cs(26): [ERROR]: Found cast to the same type \"string\"\r\n" +
                                                  "Found 2 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                                  "{1}\\CastsExample.cs(24): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                                  "{1}\\CastsExample.cs(29): [WARNING]: Found cast to the same type \"BadExample.SomeBaseClass\"\r\n" +
                                                  "Execution of CastToSameTypeAnalyzer finished\r\n" +
                                                  SourceCheckUtilOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  "Processing of the file {1}\\CastsExample.cs is finished\r\n" +
                                                  "Processing of the file {1}\\ClassnameExample.cs is started\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                                  "File doesn't contain any type with name exact match to the filename, but contains 2 types with names match to the filename with ignoring case\r\n" +
                                                  "{1}\\ClassnameExample.cs(3): [ERROR]: Found type named \"BadExample.ClassNameExample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{1}\\ClassnameExample.cs(7): [ERROR]: Found type named \"BadExample.Classnameexample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer finished\r\n" +
                                                  SourceCheckUtilOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  "Processing of the file {1}\\ClassnameExample.cs is finished\r\n" +
                                                  "Processing of the file {1}\\IdentifiersExample.cs is started\r\n" +
                                                  SourceCheckUtilOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  "Execution of NonAsciiIdentifiersAnalyzer started\r\n" +
                                                  "Found 14 non-ASCII identifiers leading to errors in the ported C++ code\r\n" +
                                                  "{1}\\IdentifiersExample.cs(7): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(15): [ERROR]: Found non-ASCII identifier \"TPаrаm1\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(19): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(23): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(30): [ERROR]: Found non-ASCII identifier \"somеObjB\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(32): [ERROR]: Found non-ASCII identifier \"someImplОbj\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"локальноеДействие\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"парам1\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(33): [ERROR]: Found non-ASCII identifier \"парам2\"\r\n" +
                                                  "Execution of NonAsciiIdentifiersAnalyzer finished\r\n" +
                                                  "Processing of the file {1}\\IdentifiersExample.cs is finished\r\n" +
                                                  "Processing of the project {0} is finished\r\n" +
                                                  "Result of analysis: analysis is failed\r\n";
            String projectDirPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\BadExample"));
            String expectedOutput = String.Format(expectedOutputTemplate, projectFilename, projectDirPath);
            ExecutionChecker.Check(executionResult, -1, expectedOutput, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessSolutionWithDependenciesBetweenProjects(OutputLevel outputLevel)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\LibraryDependenciesExample\\LibraryDependenciesExample.sln", null, outputLevel);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessSolutionWithDependenciesBetweenProjectsInfo()
        {
            const String solutionFilename = "..\\..\\..\\Examples\\LibraryDependenciesExample\\LibraryDependenciesExample.sln";
            ExecutionResult executionResult = ExecutionHelper.Execute(solutionFilename, null, OutputLevel.Info);
            String solutionDirPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\LibraryDependenciesExample"));
            const String expectedOutputTemplate = "Processing of the solution {0} is started\r\n" +
                                                  "Processing of the project {1}\\SomeLibrary\\SomeLibrary.csproj is started\r\n" +
                                                  SourceCheckUtilOutputDef.CompilationCheckSuccessOutput +
                                                  "Processing of the file {1}\\SomeLibrary\\SomeBaseClass.cs is started\r\n" +
                                                  SourceCheckUtilOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  "Processing of the file {1}\\SomeLibrary\\SomeBaseClass.cs is finished\r\n" +
                                                  "Processing of the project {1}\\SomeLibrary\\SomeLibrary.csproj is finished\r\n" +
                                                  "Processing of the project {1}\\DependentLibrary\\DependentLibrary.csproj is started\r\n" +
                                                  SourceCheckUtilOutputDef.CompilationCheckSuccessOutput +
                                                  "Processing of the file {1}\\DependentLibrary\\SomeDerivedClass.cs is started\r\n" +
                                                  SourceCheckUtilOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  "Processing of the file {1}\\DependentLibrary\\SomeDerivedClass.cs is finished\r\n" +
                                                  "Processing of the project {1}\\DependentLibrary\\DependentLibrary.csproj is finished\r\n" +
                                                  "Processing of the solution {0} is finished\r\n" +
                                                  "Result of analysis: analysis is succeeded\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, solutionFilename, solutionDirPath);
            ExecutionChecker.Check(executionResult, 0, expectedOutput, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessProjectWithDependencies(OutputLevel outputLevel)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\LibraryDependenciesExample\\DependentLibrary\\DependentLibrary.csproj", null, outputLevel);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessProjectWithDependenciesInfo()
        {
            const String projectFilename = "..\\..\\..\\Examples\\LibraryDependenciesExample\\DependentLibrary\\DependentLibrary.csproj";
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\LibraryDependenciesExample\\DependentLibrary\\DependentLibrary.csproj", null, OutputLevel.Info);
            String projectDirPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\LibraryDependenciesExample\\DependentLibrary"));
            const String expectedOutputTemplate = "Processing of the project {0} is started\r\n" +
                                                  SourceCheckUtilOutputDef.CompilationCheckSuccessOutput +
                                                  "Processing of the file {1}\\SomeDerivedClass.cs is started\r\n" +
                                                  SourceCheckUtilOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCheckUtilOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  "Processing of the file {1}\\SomeDerivedClass.cs is finished\r\n" +
                                                  "Processing of the project {0} is finished\r\n" +
                                                  "Result of analysis: analysis is succeeded\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, projectFilename, projectDirPath);
            ExecutionChecker.Check(executionResult, 0, expectedOutput, "");
        }
    }
}

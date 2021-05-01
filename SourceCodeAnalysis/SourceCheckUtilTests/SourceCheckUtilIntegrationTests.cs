using System;
using System.IO;
using NUnit.Framework;
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
            ExecutionChecker.Check(executionResult, 0, "0.2\r\n", "");
        }

        [Test]
        public void ProcessUnknownArg()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--some-strange-option");
            ExecutionChecker.Check(executionResult, -1, SourceCheckUtilOutputDef.AppDescription, SourceCheckUtilOutputDef.BadUsageMessage);
        }

        [Test]
        public void ProcessAnalysisUnknownArg()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--source=\"..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj\" --some-strange-option");
            ExecutionChecker.Check(executionResult, -1, SourceCheckUtilOutputDef.AppDescription, SourceCheckUtilOutputDef.BadUsageMessage);
        }

        [Test]
        public void ProcessAnalysisForSourceWithoutValue()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--source= --verbose");
            ExecutionChecker.Check(executionResult, -1, "", SourceCheckUtilOutputDef.BadSourceMessage);
        }

        [Test]
        public void ProcessAnalysisForUnknownSource()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("SomeUnknownExample.csproj", null, false);
            ExecutionChecker.Check(executionResult, -1, "", "[ERROR]: Bad (unknown) target SomeUnknownExample.csproj\r\n");
        }

        [Test]
        public void ProcessAnalysisForUnknownConfig()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj", "..\\SomeConfig.config", false);
            ExecutionChecker.Check(executionResult, -1, "", SourceCheckUtilOutputDef.BadConfigMessage);
        }

        [Test]
        public void ProcessAnalysisForConfigWithoutSource()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\Examples\\ConfigUsageExample\\Config\"");
            ExecutionChecker.Check(executionResult, -1, SourceCheckUtilOutputDef.AppDescription, SourceCheckUtilOutputDef.BadUsageMessage);
        }

        [Test]
        public void ProcessAnalysisForConfigWithoutValue()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--source=\"..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln\" --config=");
            ExecutionChecker.Check(executionResult, -1, "", SourceCheckUtilOutputDef.BadConfigMessage);
        }

        [Test]
        public void ProcessGoodExampleProjectAnalysis()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj", null, false);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessBadExampleProjectAnalysis()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\BadExample\\BadExample.csproj", null, false);
            const String expectedErrorTemplate = "{0}\\CastsExample.cs(26): [ERROR]: Found cast to the same type \"string\"\r\n" +
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
            String projectDirectoryFullPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\BadExample"));
            String expectedError = String.Format(expectedErrorTemplate, projectDirectoryFullPath);
            ExecutionChecker.Check(executionResult, -1, "", expectedError);
        }

        [Test]
        public void ProcessSolutionWithDependenciesBetweenProjectsAnalysis()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\LibraryDependenciesExample\\LibraryDependenciesExample.sln", null, false);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessProjectWithDependenciesAnalysis()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\LibraryDependenciesExample\\DependentLibrary\\DependentLibrary.csproj", null, false);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }
    }
}

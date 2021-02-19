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
            const String expectedErrorTemplate = "[ERROR]: File {0}\\CastsExample.cs contains the cast to the same type string which are started at 25,28 and finished at 25,44\r\n" +
                                                 "[ERROR]: File {0}\\ClassnameExample.cs contains type named BadExample.ClassNameExample with name match to the filename with ignoring case\r\n" +
                                                 "[ERROR]: File {0}\\ClassnameExample.cs contains type named BadExample.Classnameexample with name match to the filename with ignoring case\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"SоmeSimpleClassA\" which are started at 6,21 and finished at 6,37\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"TPаrаm1\" which are started at 14,31 and finished at 14,38\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"ISomеInterface\" which are started at 18,25 and finished at 18,39\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"ISomеInterface\" which are started at 22,49 and finished at 22,63\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"SоmeSimpleClassA\" which are started at 28,12 and finished at 28,28\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"SоmeSimpleClassA\" which are started at 28,44 and finished at 28,60\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"somеObjB\" which are started at 29,29 and finished at 29,37\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"SоmeSimpleClassA\" which are started at 30,22 and finished at 30,38\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"SоmeSimpleClassA\" which are started at 30,71 and finished at 30,87\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"someImplОbj\" which are started at 31,48 and finished at 31,59\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"SоmeSimpleClassA\" which are started at 32,27 and finished at 32,43\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"локальноеДействие\" which are started at 32,45 and finished at 32,62\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"парам1\" which are started at 32,66 and finished at 32,72\r\n" +
                                                 "[ERROR]: File {0}\\IdentifiersExample.cs contains the following non-ASCII identifier \"парам2\" which are started at 32,74 and finished at 32,80\r\n";
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

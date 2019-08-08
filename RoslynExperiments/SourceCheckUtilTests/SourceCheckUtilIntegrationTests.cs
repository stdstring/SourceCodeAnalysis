﻿using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SourceCheckUtilTests.Utils;

namespace SourceCheckUtilTests
{
    [TestFixture]
    public class SourceCheckUtilIntegrationTests
    {
        [Test]
        public void ProcessEmptyArgs()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("");
            CheckExecutionResult(executionResult, 0, AppDescription, "");
        }

        [Test]
        public void ProcessHelp()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--help");
            CheckExecutionResult(executionResult, 0, AppDescription, "");
        }

        [Test]
        public void ProcessVersion()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--version");
            CheckExecutionResult(executionResult, 0, "0.1\r\n", "");
        }

        [Test]
        public void ProcessUnknownArg()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--some-strange-option");
            CheckExecutionResult(executionResult, -1, BadUsageMessage + AppDescription, "");
        }

        [Test]
        public void ProcessAnalysisUnknownArg()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--source \"..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj\" --some-strange-option");
            CheckExecutionResult(executionResult, -1, BadUsageMessage + AppDescription, "");
        }

        [Test]
        public void ProcessAnalysisForUnknownSource()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("SomeUnknownExample.csproj", null, false);
            CheckExecutionResult(executionResult, -1, "", "[ERROR]: Bad (unknown) target SomeUnknownExample.csproj\r\n");
        }

        [Test]
        public void ProcessAnalysisForUnknownConfig()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj", "..\\SomeConfig.config", false);
            CheckExecutionResult(executionResult, -1, "", "[ERROR]: Bad (unknown) config ..\\SomeConfig.config\r\n");
        }

        [Test]
        public void ProcessGoodExampleProjectAnalysis()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\GoodExample\\GoodExample.csproj", null, false);
            CheckExecutionResult(executionResult, 0, "", "");
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
            String projectDirectoryFullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\Examples\\BadExample"));
            String expectedError = String.Format(expectedErrorTemplate, projectDirectoryFullPath);
            CheckExecutionResult(executionResult, -1, "", expectedError);
        }

        [Test]
        public void ProcessSolutionWithDependenciesBetweenProjectsAnalysis()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\LibraryDependenciesExample\\LibraryDependenciesExample.sln", null, false);
            CheckExecutionResult(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessProjectWithDependenciesAnalysis()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\LibraryDependenciesExample\\DependentLibrary\\DependentLibrary.csproj", null, false);
            CheckExecutionResult(executionResult, 0, "", "");
        }

        private void CheckExecutionResult(ExecutionResult result, Int32 exitCode, String outputData, String errorData)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(exitCode, result.ExitCode);
            Assert.AreEqual(outputData, result.OutputData);
            Assert.AreEqual(errorData, result.ErrorData);
        }

        private const String BadUsageMessage = "Bad usage of the application.\r\n";
        private const String AppDescription = "Application usage:\r\n" +
                                              "1. {APP} --source {solution-filename.sln|project-filename.csproj|cs-filename.cs} [--config {config-file|config-dir}] [--verbose]\r\n" +
                                              "2. {APP} --help\r\n" +
                                              "3. {APP} --version\r\n";
    }
}

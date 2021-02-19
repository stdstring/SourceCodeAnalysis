using System;
using System.Collections.Generic;
using NUnit.Framework;
using SourceCheckUtilTests.Utils;

namespace SourceCheckUtilTests
{
    [TestFixture]
    public class SourceCheckUtilEnvironmentVariablesTests
    {
        [Test]
        public void ProcessWithConfigWithEnvVarImport()
        {
            const String environmentVariableName = "EXAMPLE_CONFIG";
            IDictionary<String, String> environmentVariables = new Dictionary<String, String> {{environmentVariableName, "..\\..\\..\\Examples\\ConfigUsageExample\\Config"}};
            EnvironmentHelper.CreateDefaultConfig($"<import config=\"%{environmentVariableName}%\\FilesProcessingExample.config\" />");
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\FilesProcessingExample\\FilesProcessingExample.csproj",
                                                                      "porter.config",
                                                                      false,
                                                                      environmentVariables);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessWithConfigWithUnknownEnvVarImport()
        {
            const String environmentVariableName = "EXAMPLE_CONFIG";
            EnvironmentHelper.CreateDefaultConfig($"<import config=\"%{environmentVariableName}%\\FilesProcessingExample.config\" />");
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\FilesProcessingExample\\FilesProcessingExample.csproj", "porter.config", false);
            String errorData = $"Bad import name value \"%{environmentVariableName}%\\FilesProcessingExample.config\" in the config \"porter.config\"\r\n";
            ExecutionChecker.Check(executionResult, -1, "", errorData);
        }

        [Test]
        public void ProcessWithEnvVarInCommandLine()
        {
            const String environmentVariableName = "EXAMPLE";
            IDictionary<String, String> environmentVariables = new Dictionary<String, String> {{environmentVariableName, "..\\..\\..\\Examples\\ConfigUsageExample"}};
            ExecutionResult executionResult = ExecutionHelper.Execute($"%{environmentVariableName}%\\FilesProcessingExample\\FilesProcessingExample.csproj",
                                                                      $"%{environmentVariableName}%\\Config",
                                                                      false,
                                                                      environmentVariables);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessWithUnknownEnvVarInSource()
        {
            const String sourceVariableName = "SOURCE";
            const String configVariableName = "CONFIG";
            IDictionary<String, String> environmentVariables = new Dictionary<String, String> {{configVariableName, "..\\..\\..\\Examples\\ConfigUsageExample"}};
            ExecutionResult executionResult = ExecutionHelper.Execute($"%{sourceVariableName}%\\FilesProcessingExample\\FilesProcessingExample.csproj",
                                                                      $"%{configVariableName}%\\Config",
                                                                      false,
                                                                      environmentVariables);
            ExecutionChecker.Check(executionResult, -1, "", SourceCheckUtilOutputDef.BadSourceMessage);
        }

        [Test]
        public void ProcessWithUnknownEnvVarInConfig()
        {
            const String sourceVariableName = "SOURCE";
            const String configVariableName = "CONFIG";
            IDictionary<String, String> environmentVariables = new Dictionary<String, String> {{sourceVariableName, "..\\..\\..\\Examples\\ConfigUsageExample"}};
            ExecutionResult executionResult = ExecutionHelper.Execute($"%{sourceVariableName}%\\FilesProcessingExample\\FilesProcessingExample.csproj",
                                                                      $"%{configVariableName}%\\Config",
                                                                      false,
                                                                      environmentVariables);
            ExecutionChecker.Check(executionResult, -1, "", SourceCheckUtilOutputDef.BadConfigMessage);
        }
    }
}
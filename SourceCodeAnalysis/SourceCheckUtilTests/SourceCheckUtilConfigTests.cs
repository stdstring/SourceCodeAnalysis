using System;
using System.IO;
using NUnit.Framework;
using SourceCheckUtilTests.Utils;

namespace SourceCheckUtilTests
{
    [TestFixture]
    public class SourceCheckUtilConfigTests
    {
        [TearDown]
        public void TearDown()
        {
            EnvironmentHelper.RemoveAuxiliaryEntities();
        }

        [TestCase("..\\..\\..\\Examples\\ConfigUsageExample\\DefaultConfig")]
        [TestCase("..\\..\\..\\Examples\\ConfigUsageExample\\DefaultConfig\\porter.config")]
        public void ProcessWithDefaultConfig(String configPath)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln", configPath, false);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [TestCase("..\\..\\..\\Examples\\ConfigUsageExample\\Config")]
        [TestCase("..\\..\\..\\Examples\\ConfigUsageExample\\Config\\porter.config")]
        public void ProcessWithConfig(String configPath)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln", configPath, false);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessWithoutConfig()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln", null, false);
            const String expectedErrorTemplate = "[ERROR]: File {0}\\FilesProcessingExample\\Exclude\\BadIdentifierExample.cs contains the following non-ASCII identifier \"sоmеVаr\" which are started at 6,16 and finished at 6,23\r\n" +
                                                 "[ERROR]: File {0}\\FilesProcessingExample\\Exclude\\BadIdentifierExample.cs contains the following non-ASCII identifier \"sоmеОthеrVаr\" which are started at 7,19 and finished at 7,31\r\n" +
                                                 "[ERROR]: File {0}\\FilesProcessingExample\\Exclude\\BadIdentifierExample.cs contains the following non-ASCII identifier \"переменная\" which are started at 8,17 and finished at 8,27\r\n" +
                                                 "[ERROR]: File {0}\\FilesProcessingExample\\Include\\BadCastsExample.cs contains the cast to the same type string which are started at 7,26 and finished at 7,41\r\n" +
                                                 "[ERROR]: File {0}\\FilesProcessingExample\\Only\\BadClassNameExample.cs contains type named FilesProcessingExample.Only.BadClassnameExample with name match to the filename with ignoring case\r\n" +
                                                 "[ERROR]: File {0}\\FilesProcessingExample\\Only\\BadClassNameExample.cs contains type named FilesProcessingExample.Only.BadClassNameexample with name match to the filename with ignoring case\r\n";
            String projectDirectoryFullPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\ConfigUsageExample"));
            String expectedError = String.Format(expectedErrorTemplate, projectDirectoryFullPath);
            ExecutionChecker.Check(executionResult, -1, "", expectedError);
        }
    }
}

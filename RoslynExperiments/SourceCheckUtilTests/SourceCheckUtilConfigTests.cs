using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SourceCheckUtilTests.Utils;

namespace SourceCheckUtilTests
{
    [TestFixture]
    public class SourceCheckUtilConfigTests
    {
        [Test]
        public void ProcessWithConfig()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln", "..\\..\\..\\Examples\\ConfigUsageExample\\config", false);
            CheckExecutionResult(executionResult, 0, "", "");
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
            String projectDirectoryFullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\Examples\\ConfigUsageExample"));
            String expectedError = String.Format(expectedErrorTemplate, projectDirectoryFullPath);
            CheckExecutionResult(executionResult, -1, "", expectedError);
        }

        private void CheckExecutionResult(ExecutionResult result, Int32 exitCode, String outputData, String errorData)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(exitCode, result.ExitCode);
            Assert.AreEqual(outputData, result.OutputData);
            Assert.AreEqual(errorData, result.ErrorData);
        }
    }
}

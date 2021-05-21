using System;
using System.IO;
using NUnit.Framework;
using SourceCheckUtil.Output;
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
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln", configPath, OutputLevel.Error);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [TestCase("..\\..\\..\\Examples\\ConfigUsageExample\\Config")]
        [TestCase("..\\..\\..\\Examples\\ConfigUsageExample\\Config\\porter.config")]
        public void ProcessWithConfig(String configPath)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln", configPath, OutputLevel.Error);
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessWithoutConfig()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("..\\..\\..\\Examples\\ConfigUsageExample\\ConfigUsageExample.sln", null, OutputLevel.Error);
            const String expectedOutputTemplate = "{0}\\Exclude\\BadIdentifierExample.cs(7): [ERROR]: Found non-ASCII identifier \"sоmеVаr\"\r\n" +
                                                  "{0}\\Exclude\\BadIdentifierExample.cs(8): [ERROR]: Found non-ASCII identifier \"sоmеОthеrVаr\"\r\n" +
                                                  "{0}\\Exclude\\BadIdentifierExample.cs(9): [ERROR]: Found non-ASCII identifier \"переменная\"\r\n" +
                                                  "{0}\\Include\\BadCastsExample.cs(8): [ERROR]: Found cast to the same type \"string\"\r\n" +
                                                  "{0}\\Only\\BadClassNameExample.cs(3): [ERROR]: Found type named \"FilesProcessingExample.Only.BadClassnameExample\" which corresponds the filename \"BadClassNameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\Only\\BadClassNameExample.cs(7): [ERROR]: Found type named \"FilesProcessingExample.Only.BadClassNameexample\" which corresponds the filename \"BadClassNameExample.cs\" only at ignoring case\r\n";
            String projectDirectoryFullPath = Path.GetFullPath(Path.Combine(EnvironmentHelper.GetContainedDirectory(), "..\\..\\..\\Examples\\ConfigUsageExample\\FilesProcessingExample"));
            String expectedOutput = String.Format(expectedOutputTemplate, projectDirectoryFullPath);
            ExecutionChecker.Check(executionResult, -1, expectedOutput, "");
        }
    }
}

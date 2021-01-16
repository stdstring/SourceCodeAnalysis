using System;
using NUnit.Framework;

namespace SourceCheckUtilTests.Utils
{
    internal static class ExecutionChecker
    {
        public static void Check(ExecutionResult result, Int32 exitCode, String outputData, String errorData)
        {
            Assert.IsNotNull(result);
            Int32 actualExitCode = result.ExitCode;
            String actualOutputData = result.OutputData;
            String actualErrorData = result.ErrorData;
            if (exitCode != actualExitCode)
            {
                Console.WriteLine($"Expected exit code is {exitCode}, but actual exit code is {actualExitCode}");
                Console.WriteLine($"Actual output: {actualOutputData}");
                Console.WriteLine($"Actual error: {actualErrorData}");
            }
            Assert.AreEqual(exitCode, actualExitCode);
            Assert.AreEqual(outputData, actualOutputData);
            Assert.AreEqual(errorData, actualErrorData);
        }
    }
}
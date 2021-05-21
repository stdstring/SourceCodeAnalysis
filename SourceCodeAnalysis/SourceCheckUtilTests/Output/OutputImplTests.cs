using System;
using System.IO;
using NUnit.Framework;
using SourceCheckUtil.Output;

namespace SourceCheckUtilTests.Output
{
    [TestFixture]
    public class OutputImplTests
    {
        [SetUp]
        public void SetUp()
        {
            _errorOutput = new OutputImpl(new StringWriter(), new StringWriter(), OutputLevel.Error);
            _warningOutput = new OutputImpl(new StringWriter(), new StringWriter(), OutputLevel.Warning);
            _infoOutput = new OutputImpl(new StringWriter(), new StringWriter(), OutputLevel.Info);
        }

        [TearDown]
        public void TearDown()
        {
            _errorOutput?.Output.Dispose();
            _errorOutput?.Error.Dispose();
            _warningOutput?.Output.Dispose();
            _warningOutput?.Error.Dispose();
            _infoOutput?.Output.Dispose();
            _infoOutput?.Error.Dispose();
        }

        [Test]
        public void WriteInfoLine()
        {
            _errorOutput.WriteInfoLine("IDDQD");
            _errorOutput.WriteInfoLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            _warningOutput.WriteInfoLine("IDDQD");
            _warningOutput.WriteInfoLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            _infoOutput.WriteInfoLine("IDDQD");
            _infoOutput.WriteInfoLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            const String expectedOutput = "IDDQD\r\nC:\\iddqd\\idkfa.doom(666): IDCLIP\r\n";
            CheckOutput(_errorOutput, "", "");
            CheckOutput(_warningOutput, "", "");
            CheckOutput(_infoOutput, expectedOutput, "");
        }

        [Test]
        public void WriteWarningLine()
        {
            _errorOutput.WriteWarningLine("IDDQD");
            _errorOutput.WriteWarningLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            _warningOutput.WriteWarningLine("IDDQD");
            _warningOutput.WriteWarningLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            _infoOutput.WriteWarningLine("IDDQD");
            _infoOutput.WriteWarningLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            const String expectedOutput = "[WARNING]: IDDQD\r\nC:\\iddqd\\idkfa.doom(666): [WARNING]: IDCLIP\r\n";
            CheckOutput(_errorOutput, "", "");
            CheckOutput(_warningOutput, expectedOutput, "");
            CheckOutput(_infoOutput, expectedOutput, "");
        }

        [Test]
        public void WriteErrorLine()
        {
            _errorOutput.WriteErrorLine("IDDQD");
            _errorOutput.WriteErrorLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            _warningOutput.WriteErrorLine("IDDQD");
            _warningOutput.WriteErrorLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            _infoOutput.WriteErrorLine("IDDQD");
            _infoOutput.WriteErrorLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            const String expectedOutput = "[ERROR]: IDDQD\r\nC:\\iddqd\\idkfa.doom(666): [ERROR]: IDCLIP\r\n";
            CheckOutput(_errorOutput, expectedOutput, "");
            CheckOutput(_warningOutput, expectedOutput, "");
            CheckOutput(_infoOutput, expectedOutput, "");
        }

        [Test]
        public void WriteFailLine()
        {
            _errorOutput.WriteFailLine("IDDQD");
            _errorOutput.WriteFailLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            _warningOutput.WriteFailLine("IDDQD");
            _warningOutput.WriteFailLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            _infoOutput.WriteFailLine("IDDQD");
            _infoOutput.WriteFailLine("C:\\iddqd\\idkfa.doom", 665, "IDCLIP");
            const String expectedError = "[ERROR]: IDDQD\r\nC:\\iddqd\\idkfa.doom(666): [ERROR]: IDCLIP\r\n";
            CheckOutput(_errorOutput, "", expectedError);
            CheckOutput(_warningOutput, "", expectedError);
            CheckOutput(_infoOutput, "", expectedError);
        }

        private void CheckOutput(OutputImpl output, String expectedOutput, String expectedError)
        {
            String actualOutput = output.Output.ToString();
            String actualError = output.Error.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);
            Assert.AreEqual(expectedError, actualError);
        }

        private OutputImpl _errorOutput;
        private OutputImpl _warningOutput;
        private OutputImpl _infoOutput;
    }
}

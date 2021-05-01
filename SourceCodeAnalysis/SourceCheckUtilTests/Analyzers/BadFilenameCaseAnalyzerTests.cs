using System;
using NUnit.Framework;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Utils;
using SourceCheckUtilTests.Utils;

namespace SourceCheckUtilTests.Analyzers
{
    [TestFixture]
    public class BadFilenameCaseAnalyzerTests
    {
        [Test]
        public void ProcessExactMatch()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "BadFilenameCase", filePath, false, true, "", "");
        }

        [Test]
        public void ProcessExactMatchWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                          "File contains 0 types with names match to the filename with ignoring case\r\n" +
                                          "Execution of BadFilenameCaseAnalyzer finished\r\n\r\n";
            Func <OutputImpl, IFileAnalyzer> analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "BadFilenameCase", filePath, true, true, expectedOutput, "");
        }

        [Test]
        public void ProcessExactMatchWithWarnings()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "BadFilenameCase", filePath, false, true, "", "");
        }

        [Test]
        public void ProcessExactMatchWithWarningsWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                          "File contains 2 types with names match to the filename with ignoring case\r\n" +
                                          filePath + "(9): [WARNING]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                          filePath + "(12): [WARNING]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                          "Execution of BadFilenameCaseAnalyzer finished\r\n\r\n";
            Func <OutputImpl, IFileAnalyzer> analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "BadFilenameCase", filePath, true, true, expectedOutput, "");
        }

        [Test]
        public void ProcessWithoutExactMatch()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SoMeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedError = filePath + "(6): [ERROR]: Found type named \"SomeNamespace.SoMeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                         filePath + "(9): [ERROR]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                         filePath + "(12): [ERROR]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n";
            Func <OutputImpl, IFileAnalyzer> analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "BadFilenameCase", filePath, false, false, "", expectedError);
        }

        [Test]
        public void ProcessWithoutExactMatchWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SoMeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                          "File doesn't contain any type with name exact match to the filename, but contains 3 types with names match to the filename with ignoring case\r\n" +
                                          "Execution of BadFilenameCaseAnalyzer finished\r\n\r\n";
            const String expectedError = filePath + "(6): [ERROR]: Found type named \"SomeNamespace.SoMeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                         filePath + "(9): [ERROR]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                         filePath + "(12): [ERROR]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "BadFilenameCase", filePath, true, false, expectedOutput, expectedError);
        }

        [Test]
        public void ProcessWithoutMatch()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "BadFilenameCase", filePath, false, true, "", "");
        }

        [Test]
        public void ProcessWithoutMatchWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                          "[WARNING]: File doesn't contain any types with names corresponding to the name of this file\r\n" +
                                          "Execution of BadFilenameCaseAnalyzer finished\r\n\r\n";
            Func <OutputImpl, IFileAnalyzer> analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "BadFilenameCase", filePath, true, true, expectedOutput, "");
        }
    }
}

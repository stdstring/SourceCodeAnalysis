using System;
using NUnit.Framework;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Utils;
using SourceCheckUtilTests.Utils;

namespace SourceCheckUtilTests.Analyzers
{
    [TestFixture]
    public class NonAsciiIdentifiersAnalyzerTests
    {
        [Test]
        public void ProcessNonAsciiIdentifiers()
        {
            const String source = "namespace SomeНеймспейс\r\n" +
                                  "{\r\n" +
                                  "    public class ДругойClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeКласс\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeМетод()\r\n" +
                                  "        {\r\n" +
                                  "            int intПеременная = 666;\r\n" +
                                  "            string строковаяVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            ДругойClass другойObj = new ДругойClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedError = filePath + "(1): [ERROR]: Found non-ASCII identifier \"SomeНеймспейс\"\r\n" +
                                         filePath + "(3): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                         filePath + "(6): [ERROR]: Found non-ASCII identifier \"SomeКласс\"\r\n" +
                                         filePath + "(8): [ERROR]: Found non-ASCII identifier \"SomeМетод\"\r\n" +
                                         filePath + "(10): [ERROR]: Found non-ASCII identifier \"intПеременная\"\r\n" +
                                         filePath + "(11): [ERROR]: Found non-ASCII identifier \"строковаяVar1\"\r\n" +
                                         filePath + "(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                         filePath + "(13): [ERROR]: Found non-ASCII identifier \"другойObj\"\r\n" +
                                         filePath + "(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new NonAsciiIdentifiersAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "NonAsciiIdentifiers", filePath, false, false, "", expectedError);
        }

        [Test]
        public void ProcessNonAsciiIdentifiersWithVerbose()
        {
            const String source = "namespace SomeНеймспейс\r\n" +
                                  "{\r\n" +
                                  "    public class ДругойClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeКласс\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeМетод()\r\n" +
                                  "        {\r\n" +
                                  "            int intПеременная = 666;\r\n" +
                                  "            string строковаяVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            ДругойClass другойObj = new ДругойClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of NonAsciiIdentifiersAnalyzer started\r\n" +
                                          "Found 9 non-ASCII identifiers leading to errors in the ported C++ code\r\n" +
                                          "Execution of NonAsciiIdentifiersAnalyzer finished\r\n\r\n";
            const String expectedError = filePath + "(1): [ERROR]: Found non-ASCII identifier \"SomeНеймспейс\"\r\n" +
                                         filePath + "(3): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                         filePath + "(6): [ERROR]: Found non-ASCII identifier \"SomeКласс\"\r\n" +
                                         filePath + "(8): [ERROR]: Found non-ASCII identifier \"SomeМетод\"\r\n" +
                                         filePath + "(10): [ERROR]: Found non-ASCII identifier \"intПеременная\"\r\n" +
                                         filePath + "(11): [ERROR]: Found non-ASCII identifier \"строковаяVar1\"\r\n" +
                                         filePath + "(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                         filePath + "(13): [ERROR]: Found non-ASCII identifier \"другойObj\"\r\n" +
                                         filePath + "(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new NonAsciiIdentifiersAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "NonAsciiIdentifiers", filePath, true, false, expectedOutput, expectedError);
        }

        [Test]
        public void ProcessAsciiIdentifiers()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int intVar = 666;\r\n" +
                                  "            string strVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            OtherClass otherObj = new OtherClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new NonAsciiIdentifiersAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "NonAsciiIdentifiers", filePath, false, true, "", "");
        }

        [Test]
        public void ProcessAsciiIdentifiersWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int intVar = 666;\r\n" +
                                  "            string strVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            OtherClass otherObj = new OtherClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of NonAsciiIdentifiersAnalyzer started\r\n" +
                                          "Found 0 non-ASCII identifiers leading to errors in the ported C++ code\r\n" +
                                          "Execution of NonAsciiIdentifiersAnalyzer finished\r\n\r\n";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new NonAsciiIdentifiersAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "NonAsciiIdentifiers", filePath, true, true, expectedOutput, "");
        }
    }
}

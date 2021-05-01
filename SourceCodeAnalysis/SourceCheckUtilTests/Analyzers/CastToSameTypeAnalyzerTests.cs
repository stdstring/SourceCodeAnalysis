using System;
using NUnit.Framework;
using SourceCheckUtil.Analyzers;
using SourceCheckUtil.Utils;
using SourceCheckUtilTests.Utils;

namespace SourceCheckUtilTests.Analyzers
{
    [TestFixture]
    public class CastToSameTypeAnalyzerTests
    {
        [Test]
        public void ProcessErrorCasts()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string)s1;\r\n" +
                                  "            string s3 = (System.String)\"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedError = filePath + "(8): [ERROR]: Found cast to the same type \"string\"\r\n" +
                                         filePath + "(9): [ERROR]: Found cast to the same type \"string\"\r\n";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new CastToSameTypeAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "CastToSameType", filePath, false, false, "", expectedError);
        }

        [Test]
        public void ProcessErrorCastsWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string)s1;\r\n" +
                                  "            string s3 = (System.String)\"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of CastToSameTypeAnalyzer started\r\n" +
                                          "Found 2 casts leading to errors in the ported C++ code\r\n" +
                                          "Found 0 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                          "Execution of CastToSameTypeAnalyzer finished\r\n\r\n";
            const String expectedError = filePath + "(8): [ERROR]: Found cast to the same type \"string\"\r\n" +
                                         filePath + "(9): [ERROR]: Found cast to the same type \"string\"\r\n";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new CastToSameTypeAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "CastToSameType", filePath, true, false, expectedOutput, expectedError);
        }

        [Test]
        public void ProcessWarningCasts()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            int i2 = (int)i1;\r\n" +
                                  "            int i3 = (int)13;\r\n" +
                                  "            double d1 = 3.14;\r\n" +
                                  "            double d2 = (double)d1;\r\n" +
                                  "            object obj1 = new object();\r\n" +
                                  "            object obj2 = (object)obj1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeDerivedClass someObj2 = (SomeDerivedClass)someObj1;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new CastToSameTypeAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "CastToSameType", filePath, false, true, "", "");
        }

        [Test]
        public void ProcessWarningCastsWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            int i2 = (int)i1;\r\n" +
                                  "            int i3 = (int)13;\r\n" +
                                  "            double d1 = 3.14;\r\n" +
                                  "            double d2 = (double)d1;\r\n" +
                                  "            object obj1 = new object();\r\n" +
                                  "            object obj2 = (object)obj1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeDerivedClass someObj2 = (SomeDerivedClass)someObj1;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of CastToSameTypeAnalyzer started\r\n" +
                                          "Found 0 casts leading to errors in the ported C++ code\r\n" +
                                          "Found 5 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                          filePath + "(14): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                          filePath + "(15): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                          filePath + "(17): [WARNING]: Found cast to the same type \"double\"\r\n" +
                                          filePath + "(19): [WARNING]: Found cast to the same type \"object\"\r\n" +
                                          filePath + "(21): [WARNING]: Found cast to the same type \"SomeNamespace.SomeDerivedClass\"\r\n" +
                                          "Execution of CastToSameTypeAnalyzer finished\r\n\r\n";
            Func <OutputImpl, IFileAnalyzer> analyzerFactory = output => new CastToSameTypeAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "CastToSameType", filePath, true, true, expectedOutput, "");
        }

        [Test]
        public void ProcessOtherCasts()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            long i2 = (long) i1;\r\n" +
                                  "            short i3 = (short) i1;\r\n" +
                                  "            uint i4 = (uint) i1;\r\n" +
                                  "            object s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string) s1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeBaseClass someObj2 = (SomeBaseClass) someObj1;\r\n" +
                                  "            SomeDerivedClass someObj3 = (SomeDerivedClass) someObj2;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new CastToSameTypeAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "CastToSameType", filePath, false, true, "", "");
        }

        [Test]
        public void ProcessOtherCastsWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            long i2 = (long) i1;\r\n" +
                                  "            short i3 = (short) i1;\r\n" +
                                  "            uint i4 = (uint) i1;\r\n" +
                                  "            object s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string) s1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeBaseClass someObj2 = (SomeBaseClass) someObj1;\r\n" +
                                  "            SomeDerivedClass someObj3 = (SomeDerivedClass) someObj2;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of CastToSameTypeAnalyzer started\r\n" +
                                          "Found 0 casts leading to errors in the ported C++ code\r\n" +
                                          "Found 0 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                          "Execution of CastToSameTypeAnalyzer finished\r\n\r\n";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new CastToSameTypeAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "CastToSameType", filePath, true, true, expectedOutput, "");
        }

        [Test]
        public void ProcessWithoutCasts()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i = 0;\r\n" +
                                  "            string s = \"IDDQD\";\r\n" +
                                  "            bool b = true;\r\n" +
                                  "            object obj = new object();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            Func<OutputImpl, IFileAnalyzer> analyzerFactory = output => new CastToSameTypeAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "CastToSameType", filePath, false, true, "", "");
        }

        [Test]
        public void ProcessWithoutCastsWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i = 0;\r\n" +
                                  "            string s = \"IDDQD\";\r\n" +
                                  "            bool b = true;\r\n" +
                                  "            object obj = new object();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutput = "Execution of CastToSameTypeAnalyzer started\r\n" +
                                          "Found 0 casts leading to errors in the ported C++ code\r\n" +
                                          "Found 0 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                          "Execution of CastToSameTypeAnalyzer finished\r\n\r\n";
            Func <OutputImpl, IFileAnalyzer> analyzerFactory = output => new CastToSameTypeAnalyzer(output);
            AnalyzerHelper.Process(analyzerFactory, source, "CastToSameType", filePath, true, true, expectedOutput, "");
        }
    }
}

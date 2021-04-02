using System;
using System.Collections.Generic;
using NUnit.Framework;
using SourceCodeAnalysisVSExtension.Config;

namespace SourceCodeAnalysisVSExtensionTests.Config
{
    [TestFixture]
    public class ConfigFinderTests
    {
        [Test]
        public void FindDirectoryConfig()
        {
            const String someRepoPath = "C:\\SomeRepo";
            const String someRepoConfigPath = "C:\\Configs\\SomeRepoConfig";
            const String someProjectPath = "C:\\SomeRepo\\SomeProject";
            const String someSubprojectPath = "C:\\SomeRepo\\SomeProject\\SomeSubproject";
            const String someSubprojectConfigPath = "C:\\Configs\\SomeRepoConfig\\SomeSubprojectConfig";
            IList<SourceEntry> entries = new List<SourceEntry>
            {
                new SourceEntry(someRepoPath, someRepoConfigPath),
                new SourceEntry(someProjectPath, ""),
                new SourceEntry(someSubprojectPath, someSubprojectConfigPath),
                new SourceEntry("C:\\OtherRepo", "C:\\Configs\\OtherRepoConfig")
            };
            IConfigDataProvider configDataProvider = new ReadOnlyConfigDataProvider("C:\\AppSomeLocation", entries);
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, someRepoPath));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, someProjectPath));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, someSubprojectPath));
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, someRepoPath + "\\OtherProject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, someProjectPath + "\\OtherSubproject"));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, someSubprojectPath + "\\SomeInnerSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, "C:\\AnotherRepo"));
        }

        [Test]
        public void FindDirectoryConfigWithSourceEndedByBackslash()
        {
            const String someRepoPath = "C:\\SomeRepo";
            const String someRepoConfigPath = "C:\\Configs\\SomeRepoConfig";
            const String someProjectPath = "C:\\SomeRepo\\SomeProject";
            const String someSubprojectPath = "C:\\SomeRepo\\SomeProject\\SomeSubproject";
            const String someSubprojectConfigPath = "C:\\Configs\\SomeRepoConfig\\SomeSubprojectConfig";
            IList<SourceEntry> entries = new List<SourceEntry>
            {
                new SourceEntry($"{someRepoPath}\\", someRepoConfigPath),
                new SourceEntry($"{someProjectPath}\\", ""),
                new SourceEntry($"{someSubprojectPath}\\", someSubprojectConfigPath),
                new SourceEntry("C:\\OtherRepo", "C:\\Configs\\OtherRepoConfig")
            };
            IConfigDataProvider configDataProvider = new ReadOnlyConfigDataProvider("C:\\AppSomeLocation", entries);
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, someRepoPath));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, someProjectPath));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, someSubprojectPath));
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, someRepoPath + "\\OtherProject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, someProjectPath + "\\OtherSubproject"));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, someSubprojectPath + "\\SomeInnerSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, "C:\\AnotherRepo"));
        }

        [Test]
        public void FindFileConfig()
        {
            const String someRepoPath = "C:\\SomeRepo";
            const String someRepoConfigPath = "C:\\Configs\\SomeRepoConfig\\porter.config";
            const String someProjectPath = "C:\\SomeRepo\\SomeProject";
            const String someSubprojectPath = "C:\\SomeRepo\\SomeProject\\SomeSubproject";
            const String someSubprojectConfigPath = "C:\\Configs\\SomeRepoConfig\\SomeSubprojectConfig\\porter.config";
            IList<SourceEntry> entries = new List<SourceEntry>
            {
                new SourceEntry(someRepoPath, someRepoConfigPath),
                new SourceEntry(someProjectPath, ""),
                new SourceEntry(someSubprojectPath, someSubprojectConfigPath),
                new SourceEntry("C:\\OtherRepo", "C:\\Configs\\OtherRepoConfig\\porter.config")
            };
            IConfigDataProvider configDataProvider = new ReadOnlyConfigDataProvider("C:\\AppSomeLocation", entries);
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, someRepoPath));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, someProjectPath));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, someSubprojectPath));
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, someRepoPath + "\\OtherProject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, someProjectPath + "\\OtherSubproject"));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, someSubprojectPath + "\\SomeInnerSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, "C:\\AnotherRepo"));
        }

        [Test]
        public void FindDirectoryFileConfig()
        {
            const String someRepoPath = "C:\\SomeRepo";
            const String someRepoConfigDirPath = "C:\\Configs\\SomeRepoConfig";
            const String someRepoConfigFilePath = "C:\\Configs\\SomeRepoConfig\\porter.config";
            const String someProjectPath = "C:\\SomeRepo\\SomeProject";
            const String someSubprojectPath = "C:\\SomeRepo\\SomeProject\\SomeSubproject";
            const String someSubprojectConfigDirPath = "C:\\Configs\\SomeRepoConfig\\SomeSubprojectConfig";
            const String someSubprojectConfigFilePath = "C:\\Configs\\SomeRepoConfig\\SomeSubprojectConfig\\porter.config";
            IList<SourceEntry> entries = new List<SourceEntry>
            {
                new SourceEntry(someRepoPath, someRepoConfigDirPath),
                new SourceEntry(someRepoPath, someRepoConfigFilePath),
                new SourceEntry(someProjectPath, ""),
                new SourceEntry(someSubprojectPath, someSubprojectConfigFilePath),
                new SourceEntry(someSubprojectPath, someSubprojectConfigDirPath),
                new SourceEntry("C:\\OtherRepo", "C:\\Configs\\OtherRepoConfig"),
                new SourceEntry("C:\\OtherRepo", "C:\\Configs\\OtherRepoConfig\\porter.config")
            };
            IConfigDataProvider configDataProvider = new ReadOnlyConfigDataProvider("C:\\AppSomeLocation", entries);
            Assert.AreEqual(someRepoConfigDirPath, ConfigFinder.FindConfig(configDataProvider, someRepoPath));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, someProjectPath));
            Assert.AreEqual(someSubprojectConfigFilePath, ConfigFinder.FindConfig(configDataProvider, someSubprojectPath));
            Assert.AreEqual(someRepoConfigDirPath, ConfigFinder.FindConfig(configDataProvider, someRepoPath + "\\OtherProject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, someProjectPath + "\\OtherSubproject"));
            Assert.AreEqual(someSubprojectConfigFilePath, ConfigFinder.FindConfig(configDataProvider, someSubprojectPath + "\\SomeInnerSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, "C:\\AnotherRepo"));
        }
    }
}

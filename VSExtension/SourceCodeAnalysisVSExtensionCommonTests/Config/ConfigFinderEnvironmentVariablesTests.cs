using System;
using System.Collections.Generic;
using NUnit.Framework;
using SourceCodeAnalysisVSExtensionCommon.Config;

namespace SourceCodeAnalysisVSExtensionCommonTests.Config
{
    [TestFixture]
    public class ConfigFinderEnvironmentVariablesTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            Environment.SetEnvironmentVariable(SomeRepoEnvVariableName, SomeRepoEnvVariableValue);
            Environment.SetEnvironmentVariable(ConfigPathEnvVariableName, ConfigPathEnvVariableValue);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Environment.SetEnvironmentVariable(SomeRepoEnvVariableName, null);
            Environment.SetEnvironmentVariable(ConfigPathEnvVariableName, null);
        }

        [Test]
        public void FindConfig()
        {
            String someRepoPath = $"%{SomeRepoEnvVariableName}%";
            String someRepoConfigPath = $"%{ConfigPathEnvVariableName}%\\SomeRepoConfig";
            String someProjectPath = $"%{SomeRepoEnvVariableName}%\\SomeProject";
            String someSubprojectPath = $"%{SomeRepoEnvVariableName}%\\SomeProject\\SomeSubproject";
            String someSubprojectConfigPath = $"%{ConfigPathEnvVariableName}%\\SomeRepoConfig\\SomeSubprojectConfig";
            IList<SourceEntry> entries = new List<SourceEntry>
            {
                new SourceEntry(someRepoPath, someRepoConfigPath),
                new SourceEntry(someProjectPath, ""),
                new SourceEntry(someSubprojectPath, someSubprojectConfigPath),
                new SourceEntry("C:\\OtherRepo", "C:\\Configs\\OtherRepoConfig")
            };
            IConfigDataProvider configDataProvider = new ReadOnlyConfigDataProvider("C:\\AppSomeLocation", entries);
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject"));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\SomeSubproject"));
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\OtherProject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\OtherSubproject"));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\SomeSubproject\\SomeInnerSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, "C:\\AnotherRepo"));
        }

        [Test]
        public void FindConfigWithUnknownSourceVar()
        {
            String someRepoPath = $"%{UnknownRepoEnvVariableName}%";
            String someRepoConfigPath = $"%{ConfigPathEnvVariableName}%\\SomeRepoConfig";
            String someProjectPath = $"%{UnknownRepoEnvVariableName}%\\SomeProject";
            String someSubprojectPath = $"%{UnknownRepoEnvVariableName}%\\SomeProject\\SomeSubproject";
            String someSubprojectConfigPath = $"%{ConfigPathEnvVariableName}%\\SomeRepoConfig\\SomeSubprojectConfig";
            IList<SourceEntry> entries = new List<SourceEntry>
            {
                new SourceEntry(someRepoPath, someRepoConfigPath),
                new SourceEntry(someProjectPath, ""),
                new SourceEntry(someSubprojectPath, someSubprojectConfigPath),
                new SourceEntry("C:\\OtherRepo", "C:\\Configs\\OtherRepoConfig")
            };
            IConfigDataProvider configDataProvider = new ReadOnlyConfigDataProvider("C:\\AppSomeLocation", entries);
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\SomeSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\OtherProject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\OtherSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\SomeSubproject\\SomeInnerSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, "C:\\AnotherRepo"));
        }

        [Test]
        public void FindConfigWithUnknownConfigVar()
        {
            String someRepoPath = $"%{SomeRepoEnvVariableName}%";
            String someRepoConfigPath = $"%{UnknownConfigPathEnvVariableName}%\\SomeRepoConfig";
            String someProjectPath = $"%{SomeRepoEnvVariableName}%\\SomeProject";
            String someSubprojectPath = $"%{SomeRepoEnvVariableName}%\\SomeProject\\SomeSubproject";
            String someSubprojectConfigPath = $"%{UnknownConfigPathEnvVariableName}%\\SomeRepoConfig\\SomeSubprojectConfig";
            IList<SourceEntry> entries = new List<SourceEntry>
            {
                new SourceEntry(someRepoPath, someRepoConfigPath),
                new SourceEntry(someProjectPath, ""),
                new SourceEntry(someSubprojectPath, someSubprojectConfigPath),
                new SourceEntry("C:\\OtherRepo", "C:\\Configs\\OtherRepoConfig")
            };
            IConfigDataProvider configDataProvider = new ReadOnlyConfigDataProvider("C:\\AppSomeLocation", entries);
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject"));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\SomeSubproject"));
            Assert.AreEqual(someRepoConfigPath, ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\OtherProject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\OtherSubproject"));
            Assert.AreEqual(someSubprojectConfigPath, ConfigFinder.FindConfig(configDataProvider, $"{SomeRepoEnvVariableValue}\\SomeProject\\SomeSubproject\\SomeInnerSubproject"));
            Assert.AreEqual("", ConfigFinder.FindConfig(configDataProvider, "C:\\AnotherRepo"));
        }

        private const String SomeRepoEnvVariableName = "ConfigFinder_SomeRepo";
        private const String SomeRepoEnvVariableValue = "C:\\SomeRepo";
        private const String UnknownRepoEnvVariableName = "ConfigFinder_UnknownRepo";
        private const String ConfigPathEnvVariableName = "ConfigFinder_Config";
        private const String ConfigPathEnvVariableValue = "C:\\Configs";
        private const String UnknownConfigPathEnvVariableName = "ConfigFinder_UnknownConfig";
    }
}
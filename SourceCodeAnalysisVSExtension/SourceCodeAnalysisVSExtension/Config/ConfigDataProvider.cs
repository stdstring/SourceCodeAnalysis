using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SourceCodeAnalysisVSExtension.Config
{
    public interface IConfigDataProvider
    {
        String GetAppPath();

        void SaveAppPath(String appPath);

        IList<SourceEntry> GetEntries();

        SourceEntry GetEntry(Int32 index);

        void CreateSourceEntry(String source, String config);

        void SaveSourceEntry(Int32 index, String source, String config);

        void RemoveSourceEntry(Int32 index);
    }

    internal class AppDataConfigDataProvider : IConfigDataProvider
    {
        public AppDataConfigDataProvider()
        {
            String directoryName = Environment.ExpandEnvironmentVariables(ConfigDirectory);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            _configPath = Path.Combine(directoryName, ConfigFilename);
            if (File.Exists(_configPath))
                _configData = LoadConfigData();
        }

        public String GetAppPath()
        {
            return _configData.AppConfig.AppPath;
        }

        public void SaveAppPath(String appPath)
        {
            _configData.AppConfig.AppPath = appPath;
            SaveConfigData();
        }

        public IList<SourceEntry> GetEntries()
        {
            return _configData.SourceEntries;
        }

        public SourceEntry GetEntry(Int32 index)
        {
            return _configData.SourceEntries[index];
        }

        public void CreateSourceEntry(String source, String config)
        {
            _configData.SourceEntries.Add(new SourceEntry(source, config));
            SaveConfigData();
        }

        public void SaveSourceEntry(Int32 index, String source, String config)
        {
            _configData.SourceEntries[index] = new SourceEntry(source, config);
            SaveConfigData();
        }

        public void RemoveSourceEntry(Int32 index)
        {
            _configData.SourceEntries.RemoveAt(index);
            SaveConfigData();
        }

        private ConfigData LoadConfigData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            using (Stream stream = new FileStream(_configPath, FileMode.Open))
            {
                return (ConfigData) serializer.Deserialize(stream);
            }
        }

        private void SaveConfigData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            using (Stream stream = new FileStream(_configPath, FileMode.Create))
            {
                serializer.Serialize(stream, _configData);
            }
        }

        private readonly String _configPath;
        private readonly ConfigData _configData = new ConfigData();

        private const String ConfigDirectory = "%appdata%\\Aspose.Words.Cpp\\SourceCodeAnalysis\\";
        private const String ConfigFilename = "SourceCodeAnalysisVSExtension.config";
    }
}

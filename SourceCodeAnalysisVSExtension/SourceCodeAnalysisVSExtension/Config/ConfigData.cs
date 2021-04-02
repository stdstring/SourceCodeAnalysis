using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SourceCodeAnalysisVSExtension.Config
{
    [XmlRoot("AppConfig")]
    public class AppData
    {
        [XmlAttribute("path")]
        public String AppPath { get; set; } = "";
    }

    [XmlRoot("SourceConfigEntry")]
    public class SourceEntry
    {
        public SourceEntry()
        {
        }

        public SourceEntry(String source, String config)
        {
            Source = source;
            Config = config;
        }

        [XmlAttribute("source")]
        public String Source { get; set; } = "";

        [XmlAttribute("config")]
        public String Config { get; set; } = "";
    }

    [XmlRoot("Config")]
    public class ConfigData
    {
        [XmlElement("AppConfig")]
        public AppData AppConfig { get; set; } = new AppData();

        [XmlArray("SourceConfig")]
        [XmlArrayItem("SourceConfigEntry")]
        public List<SourceEntry> SourceEntries { get; set; } = new List<SourceEntry>();
    }
}

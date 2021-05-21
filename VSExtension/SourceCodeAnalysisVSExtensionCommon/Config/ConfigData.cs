using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SourceCodeAnalysisVSExtensionCommon.Config
{
    public enum OutputLevel
    {
        Error = 0,
        Warning = 1,
        Info = 2
    }

    [XmlRoot("GeneralConfig")]
    public class GeneralConfig
    {
        [XmlElement("AppPath")]
        public String AppPath { get; set; } = "";

        [XmlElement("OutputLevel")]
        public OutputLevel OutputLevel { get; set; } = OutputLevel.Error;
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
        [XmlElement("GeneralConfig")]
        public GeneralConfig GeneralConfig { get; set; } = new GeneralConfig();

        [XmlArray("SourceConfig")]
        [XmlArrayItem("SourceConfigEntry")]
        public List<SourceEntry> SourceEntries { get; set; } = new List<SourceEntry>();
    }
}

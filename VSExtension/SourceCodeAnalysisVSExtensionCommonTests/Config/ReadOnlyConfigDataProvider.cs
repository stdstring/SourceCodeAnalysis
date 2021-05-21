using System;
using System.Collections.Generic;
using SourceCodeAnalysisVSExtensionCommon.Config;

namespace SourceCodeAnalysisVSExtensionCommonTests.Config
{
    internal class ReadOnlyConfigDataProvider : IConfigDataProvider
    {
        public ReadOnlyConfigDataProvider(String appPath, OutputLevel outputLevel, IList<SourceEntry> entries)
        {
            _appPath = appPath;
            _outputLevel = outputLevel;
            _entries = entries;
        }

        public String GetAppPath()
        {
            return _appPath;
        }

        public void SaveAppPath(string appPath)
        {
            throw new NotSupportedException();
        }

        public OutputLevel GetOutputLevel()
        {
            return _outputLevel;
        }

        public void SaveOutputLevel(OutputLevel outputLevel)
        {
            throw new NotSupportedException();
        }

        public IList<SourceEntry> GetEntries()
        {
            return _entries;
        }

        public SourceEntry GetEntry(Int32 index)
        {
            return _entries[index];
        }

        public void CreateSourceEntry(String source, String config)
        {
            throw new NotSupportedException();
        }

        public void SaveSourceEntry(Int32 index, String source, String config)
        {
            throw new NotSupportedException();
        }

        public void RemoveSourceEntry(Int32 index)
        {
            throw new NotSupportedException();
        }

        private readonly String _appPath;
        private readonly OutputLevel _outputLevel;
        private readonly IList<SourceEntry> _entries;
    }
}
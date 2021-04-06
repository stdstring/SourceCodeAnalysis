using System;

namespace SourceCodeAnalysisVSExtensionCommon.Config
{
    public static class ConfigFinder
    {
        public static String FindConfig(IConfigDataProvider dataProvider, String sourcePath)
        {
            String bestSource = "";
            String bestConfig = "";
            foreach (SourceEntry entry in dataProvider.GetEntries())
            {
                String source = Environment.ExpandEnvironmentVariables(entry.Source).TrimEnd('\\');
                if (sourcePath.StartsWith(source) && source.Length > bestSource.Length)
                {
                    bestSource = source;
                    bestConfig = entry.Config;
                }
            }
            return bestConfig;
        }
    }
}

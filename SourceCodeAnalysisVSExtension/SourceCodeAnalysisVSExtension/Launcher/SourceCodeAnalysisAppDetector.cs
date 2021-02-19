using System;
using System.IO;

namespace SourceCodeAnalysisVSExtension.Launcher
{
    internal static class SourceCodeAnalysisAppDetector
    {
        public static String FindSourceCodeAnalysisApp(String appName)
        {
            // TODO (std_string) : move these paths into external source
            const String sourceCodeAnalysisAppPath = "%SOURCE_CODE_ANALYSIS_APP_ROOT%";
            const String wordscppPath = "%AWCPP_ROOT%\\AdditionalTestsAndTools\\awnet.check\\SourceCodeAnalysis\\SourceCheckUtil";
            String sourceCodeAnalysisAppResult = FindSourceCodeAnalysisApp(appName, sourceCodeAnalysisAppPath);
            if (!String.IsNullOrEmpty(sourceCodeAnalysisAppResult))
                return sourceCodeAnalysisAppResult;
            String wordscppResult = FindSourceCodeAnalysisApp(appName, wordscppPath);
            if (!String.IsNullOrEmpty(wordscppResult))
                return wordscppResult;
            return null;
        }

        private static String FindSourceCodeAnalysisApp(String appName, String path)
        {
            const Char environmentBorderChar = '%';
            String expandedPath = Environment.ExpandEnvironmentVariables(path);
            if (expandedPath.IndexOf(environmentBorderChar) != -1)
                return null;
            String[] files = Directory.GetFiles(expandedPath, appName, SearchOption.AllDirectories);
            return files.Length > 0 ? files[0] : null;
        }
    }
}

using System;
using System.IO;

namespace SourceCodeAnalysisVSExtension.Launcher
{
    internal enum DetectType {Application, PrepareScript}

    internal static class SourceCodeAnalysisAppDetector
    {
        public static String DetectApp(String appPath, DetectType detectType)
        {
            if (String.IsNullOrEmpty(appPath))
                return null;
            const Char environmentBorderChar = '%';
            String expandedPath = Environment.ExpandEnvironmentVariables(appPath);
            if (expandedPath.IndexOf(environmentBorderChar) != -1)
                return null;
            if (File.Exists(appPath))
                return ProcessFile(appPath, detectType);
            if (Directory.Exists(appPath))
                return ProcessDirectory(appPath, detectType);
            return null;
        }

        private static String ProcessFile(String appPath, DetectType detectType)
        {
            String appFilename = Path.GetFileName(appPath);
            switch (detectType)
            {
                case DetectType.Application:
                    return String.Equals(appFilename, SourceCodeAnalysisCommonDefs.AppFilename) ? appPath : null;
                case DetectType.PrepareScript:
                    return String.Equals(appFilename, SourceCodeAnalysisCommonDefs.AppPrepareFilename) ? appPath : null;
                default:
                    return null;
            }
        }

        private static String ProcessDirectory(String appPath, DetectType detectType)
        {
            switch (detectType)
            {
                case DetectType.Application:
                    return Search(appPath, SourceCodeAnalysisCommonDefs.AppFilename);
                case DetectType.PrepareScript:
                    return Search(appPath, SourceCodeAnalysisCommonDefs.AppPrepareFilename);
                default:
                    return null;
            }
        }

        private static String Search(String directory, String appFilename)
        {
            String[] files = Directory.GetFiles(directory, appFilename, SearchOption.AllDirectories);
            return files.Length > 0 ? files[0] : null;
        }
    }
}

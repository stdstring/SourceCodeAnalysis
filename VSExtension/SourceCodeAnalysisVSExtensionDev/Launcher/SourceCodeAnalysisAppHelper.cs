using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using SourceCodeAnalysisVSExtensionCommon.Launcher;
using SourceCodeAnalysisVSExtensionDev.Utils;

namespace SourceCodeAnalysisVSExtensionDev.Launcher
{
    internal static class SourceCodeAnalysisAppHelper
    {
        public static async Task<String> PrepareAnalysisAppAsync(AsyncPackage package, String appPath)
        {
            await OutputHelper.OutputMessageAsync(package, $"Search of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is started");
            String buildScriptPath = SourceCodeAnalysisAppDetector.DetectApp(appPath, DetectType.PrepareScript);
            if (!String.IsNullOrEmpty(buildScriptPath))
                return await BuildAnalysisAppAsync(package, buildScriptPath);
            String analysisAppPath = SourceCodeAnalysisAppDetector.DetectApp(appPath, DetectType.Application);
            if (!String.IsNullOrEmpty(analysisAppPath))
            {
                await OutputHelper.OutputMessageAsync(package, $"Application \"{SourceCodeAnalysisCommonDefs.AppFilename}\" is found");
                return analysisAppPath;
            }
            await OutputHelper.OutputMessageAsync(package, $"Analysis application \"{SourceCodeAnalysisCommonDefs.AppFilename}\" is missing");
            await UIHelper.ShowMessageAsync(package, "Missing analysis app", $"{SourceCodeAnalysisCommonDefs.AppFilename} app is missing");
            return null;
        }

        private static async Task<String> BuildAnalysisAppAsync(AsyncPackage package, String buildScriptPath)
        {
            await OutputHelper.OutputMessageAsync(package, $"Build script \"{SourceCodeAnalysisCommonDefs.AppPrepareFilename}\" is found");
            await OutputHelper.OutputMessageAsync(package, $"Build of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is started");
            ExecutionResult buildResult = await ExecutionHelper.ExecuteBuildAnalysisAppAsync(buildScriptPath);
            if (buildResult.ExitCode == 0)
            {
                await OutputHelper.OutputMessageAsync(package, $"Build of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is completed");
                return buildResult.OutputData.Trim();
            }
            await OutputHelper.OutputMessageAsync(package, $"Build of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is failed");
            await UIHelper.ShowMessageAsync(package, "Build fails of analysis app", $"Build of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is failed");
            return null;
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using SourceCodeAnalysisVSExtension.Utils;

namespace SourceCodeAnalysisVSExtension.Launcher
{
    internal static class SourceCodeAnalysisAppHelper
    {
        public static async Task<String> PrepareAnalysisAppAsync(AsyncPackage package)
        {
            await OutputHelper.OutputMessageAsync(package, $"Search of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is started");
            String appPath = SourceCodeAnalysisAppDetector.FindSourceCodeAnalysisApp(SourceCodeAnalysisCommonDefs.AppFilename);
            if (!String.IsNullOrEmpty(appPath))
                return appPath;
            String prepareAppPath = SourceCodeAnalysisAppDetector.FindSourceCodeAnalysisApp(SourceCodeAnalysisCommonDefs.AppPrepareFilename);
            if (String.IsNullOrEmpty(prepareAppPath))
            {
                await OutputHelper.OutputMessageAsync(package, $"\"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is missing");
                await UIHelper.ShowMessageAsync(package, "Missing analysis app", $"{SourceCodeAnalysisCommonDefs.AppFilename} app is missing");
                return null;
            }
            await OutputHelper.OutputMessageAsync(package, $"Build of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is started");
            ExecutionResult buildResult = await ExecutionHelper.ExecuteAsync("python.exe", prepareAppPath);
            if (buildResult.ExitCode == 0)
            {
                await OutputHelper.OutputMessageAsync(package, $"Build of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is completed");
                return SourceCodeAnalysisAppDetector.FindSourceCodeAnalysisApp(SourceCodeAnalysisCommonDefs.AppFilename);
            }
            await OutputHelper.OutputBadBuildAnalysisAppAsync(package, buildResult);
            await UIHelper.ShowMessageAsync(package, "Build fails of analysis app", $"Build of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is failed");
            return null;
        }
    }
}
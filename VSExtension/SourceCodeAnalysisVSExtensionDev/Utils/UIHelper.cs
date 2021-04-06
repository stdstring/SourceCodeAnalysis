using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using SourceCodeAnalysisVSExtensionCommon.Launcher;
using Task = System.Threading.Tasks.Task;

namespace SourceCodeAnalysisVSExtensionDev.Utils
{
    internal static class UIHelper
    {

        public static async Task ShowMessageAsync(IServiceProvider serviceProvider, String title, String message)
        {
            await TaskScheduler.Default;
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            VsShellUtilities.ShowMessageBox(serviceProvider,
                                            message,
                                            title,
                                            OLEMSGICON.OLEMSGICON_INFO,
                                            OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public static async Task ShowProjectsSummaryAsync(IServiceProvider serviceProvider, ProjectsAnalysisStat statistics)
        {
            String projectNames = statistics.ProjectNames;
            Int32 totalCount = statistics.TotalCount;
            Int32 successCount = statistics.SuccessCount;
            Int32 failedCount = statistics.FailedCount;
            Int32 skippedCount = statistics.SkippedCount;
            String title = $"Result of source code analysis for projects named {projectNames}";
            String message = $"Result of source code analysis of selected projects: {totalCount} total, {successCount} successful, {failedCount} failed, {skippedCount} skipped (the details see in the output pane)";
            await ShowMessageAsync(serviceProvider, title, message);
        }

        public static async Task ShowSolutionSummaryAsync(IServiceProvider serviceProvider, ExecutionResult result, String target)
        {
            String title = $"Result of source code analysis for solution named {Path.GetFileNameWithoutExtension(target)}";
            String message = result.ExitCode == 0 ? "Source code analysis is succeed" : "Source code analysis is failed (the details see in the output pane)";
            await ShowMessageAsync(serviceProvider, title, message);
        }
    }
}
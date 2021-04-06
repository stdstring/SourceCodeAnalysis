using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using SourceCodeAnalysisVSExtensionCommon.Launcher;
using SourceCodeAnalysisVSExtensionDev.Launcher;
using Task = System.Threading.Tasks.Task;

namespace SourceCodeAnalysisVSExtensionDev.Utils
{
    internal static class OutputHelper
    {
        public static async Task OutputMessageAsync(IAsyncServiceProvider serviceProvider, String message)
        {
            await TaskScheduler.Default;
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            OutputPane outputPane = OutputPane.Create(serviceProvider);
            OutputTimestamp(outputPane);
            outputPane.WriteLine(message);
            outputPane.WriteLine("");
        }

        public static async Task OutputExecutionResultAsync(IAsyncServiceProvider serviceProvider, String message, ExecutionResult result)
        {
            await TaskScheduler.Default;
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            OutputPane outputPane = OutputPane.Create(serviceProvider);
            OutputTimestamp(outputPane);
            outputPane.WriteLine(message);
            if (!String.IsNullOrEmpty(result.OutputData))
            {
                outputPane.WriteLine("Output:");
                outputPane.WriteLine(result.OutputData);
            }
            if (!String.IsNullOrEmpty(result.ErrorData))
            {
                outputPane.WriteLine("Error:");
                outputPane.WriteLine(result.ErrorData);
            }
            outputPane.WriteLine("");
        }

        public static async Task OutputBadBuildAnalysisAppAsync(IAsyncServiceProvider serviceProvider, ExecutionResult result)
        {
            String message = $"Build of \"{SourceCodeAnalysisCommonDefs.AppFilename}\" app is failed";
            await OutputExecutionResultAsync(serviceProvider, message, result);
        }

        public static async Task OutputTargetAnalysisResultAsync(IAsyncServiceProvider serviceProvider, ExecutionResult result, String target, String targetType)
        {
            String message = $"Source code analysis for {targetType} named {target} is {(result.ExitCode == 0 ? "succeed" : "failed")}";
            await OutputExecutionResultAsync(serviceProvider, message, result);
        }

        private static void OutputTimestamp(OutputPane outputPane)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            outputPane.Write($"[{DateTime.Now:HH:mm:ss}] ");
        }
    }
}

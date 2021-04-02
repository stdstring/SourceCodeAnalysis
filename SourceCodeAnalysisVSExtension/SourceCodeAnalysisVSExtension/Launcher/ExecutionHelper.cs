using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;

namespace SourceCodeAnalysisVSExtension.Launcher
{
    internal static class ExecutionHelper
    {
        public static async Task<ExecutionResult> ExecuteSourceCodeAnalysisAsync(String app, String target, String config)
        {
            StringBuilder args = new StringBuilder($"--source=\"{target}\"");
            if (!String.IsNullOrEmpty(config))
                args.Append($" --config=\"{config}{(config.EndsWith("\\") ? "\\" : "")}\"");
            return await ExecuteAsync(app, args.ToString());
        }

        public static async Task<ExecutionResult> ExecuteBuildAnalysisAppAsync(String buildScriptPath)
        {
            return await ExecuteAsync("python.exe", buildScriptPath);
        }

        public static async Task<ExecutionResult> ExecuteAsync(String app, String args)
        {
            using (Process appProcess = new Process())
            {
                appProcess.StartInfo.FileName = app;
                appProcess.StartInfo.Arguments = args;
                appProcess.StartInfo.UseShellExecute = false;
                appProcess.StartInfo.CreateNoWindow = true;
                appProcess.StartInfo.RedirectStandardError = true;
                appProcess.StartInfo.RedirectStandardOutput = true;
                appProcess.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                appProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                appProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(app) ?? "";
                IList<String> output = new List<String>();
                IList<String> error = new List<String>();
                appProcess.OutputDataReceived += (sender, e) => { output.Add(e.Data); };
                appProcess.ErrorDataReceived += (sender, e) => { error.Add(e.Data); };
                appProcess.Start();
                appProcess.BeginErrorReadLine();
                appProcess.BeginOutputReadLine();
                await appProcess.WaitForExitAsync();
                return new ExecutionResult(appProcess.ExitCode, String.Join("\r\n", output), String.Join("\r\n", error));
            }
        }
    }
}
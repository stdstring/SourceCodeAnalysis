using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace SourceCheckUtilTests.Utils
{
    internal class ExecutionResult
    {
        public ExecutionResult(Int32 exitCode, String outputData, String errorData)
        {
            ExitCode = exitCode;
            OutputData = outputData;
            ErrorData = errorData;
        }

        public Int32 ExitCode { get; }

        public String OutputData { get; }

        public String ErrorData { get; }
    }

    internal static class ExecutionHelper
    {
        public static ExecutionResult Execute(String target, String config, Boolean verbose)
        {
            if (String.IsNullOrEmpty(target))
                throw new ArgumentNullException(nameof(target));
            String arguments = CreateArgList(target, config, verbose);
            return Execute(arguments);
        }

        public static ExecutionResult Execute(String arguments)
        {
            using (Process utilProcess = new Process())
            {
                String currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
                utilProcess.StartInfo.FileName = Path.Combine(currentDir, UtilFilename);
                utilProcess.StartInfo.Arguments = arguments;
                utilProcess.StartInfo.UseShellExecute = false;
                utilProcess.StartInfo.CreateNoWindow = true;
                utilProcess.StartInfo.RedirectStandardError = true;
                utilProcess.StartInfo.RedirectStandardOutput = true;
                utilProcess.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                utilProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                utilProcess.StartInfo.WorkingDirectory = currentDir;
                IList<String> output = new List<String>();
                IList<String> error = new List<String>();
                utilProcess.ErrorDataReceived += (sender, e) => { error.Add(e.Data); };
                utilProcess.OutputDataReceived += (sender, e) => { output.Add(e.Data); };
                utilProcess.Start();
                utilProcess.BeginErrorReadLine();
                utilProcess.BeginOutputReadLine();
                utilProcess.WaitForExit();
                return new ExecutionResult(utilProcess.ExitCode, String.Join("\r\n", output), String.Join("\r\n", error));
            }
        }

        private static String CreateArgList(String target, String config, Boolean verbose)
        {
            StringBuilder dest = new StringBuilder();
            dest.AppendFormat("--source \"{0}\"", target);
            if (!String.IsNullOrEmpty(config))
                dest.AppendFormat(" --config \"{0}\"", target);
            if (verbose)
                dest.Append(" --verbose");
            return dest.ToString();
        }

        private const String UtilFilename = "SourceCheckUtil.exe";
    }
}
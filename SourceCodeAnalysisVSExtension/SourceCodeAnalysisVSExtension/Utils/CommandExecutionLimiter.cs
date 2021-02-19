using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace SourceCodeAnalysisVSExtension.Utils
{
    // We use instance of this class in the UI thread only !!!!!!
    internal class CommandExecutionLimiter
    {
        public CommandExecutionLimiter()
        {
            _isCommandExecuted = false;
        }

        public Boolean IsCommandExecuted => _isCommandExecuted;

        public void StartCommandExec()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _isCommandExecuted = true;
        }

        public void StopCommandExec()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _isCommandExecuted = false;
        }

        public async Task StopCommandExecAsync()
        {
            await ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await TaskScheduler.Default;
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                StopCommandExec();
            });
        }

        private volatile Boolean _isCommandExecuted;
    }
}

using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SourceCodeAnalysisVSExtension.Utils
{
    internal class OutputPane
    {
        public static OutputPane Create(Microsoft.VisualStudio.Shell.IAsyncServiceProvider serviceProvider)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IVsOutputWindow output = (IVsOutputWindow) ThreadHelper.JoinableTaskFactory.Run(() => serviceProvider.GetServiceAsync(typeof(SVsOutputWindow)));
            Guid paneId = Guid.Parse(OutputPaneId);
            IVsOutputWindowPane pane = null;
            if (output.GetPane(ref paneId, out pane) != VSConstants.S_OK)
            {
                // create pane
                const Boolean visible = true;
                const Boolean clearWithSolution = false;
                output.CreatePane(ref paneId, OutputPaneTitle, Convert.ToInt32(visible), Convert.ToInt32(clearWithSolution));
                output.GetPane(ref paneId, out pane);
            }
            pane.Activate();
            return new OutputPane(pane);
        }

        public void Write(String data)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _pane.OutputString(data);
        }

        public void WriteLine(String data)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _pane.OutputString(data);
            _pane.OutputString("\r\n");
        }

        private OutputPane(IVsOutputWindowPane pane)
        {
            _pane = pane;
        }

        private readonly IVsOutputWindowPane _pane;

        private const String OutputPaneId = "C1C54C4F-D727-4C7C-8EDB-1DD0394AD767";
        private const String OutputPaneTitle = "SourceCodeAnalysis";
    }
}

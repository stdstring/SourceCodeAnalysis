using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using SourceCodeAnalysisVSExtensionCommon.Config;
using SourceCodeAnalysisVSExtensionCommon.Launcher;
using SourceCodeAnalysisVSExtensionDev.Config;
using SourceCodeAnalysisVSExtensionDev.Launcher;
using SourceCodeAnalysisVSExtensionDev.Utils;
using Task = System.Threading.Tasks.Task;

namespace SourceCodeAnalysisVSExtensionDev
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RunSolutionAnalysisCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("d05bac42-50ab-432d-b353-4693876523a4");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage _package;

        private RunSolutionAnalysisCommand(AsyncPackage package, OleMenuCommandService commandService, CommandExecutionLimiter commandExecutionLimiter)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            _commandExecutionLimiter = commandExecutionLimiter;
            CommandID menuCommandId = new CommandID(CommandSet, CommandId);
            OleMenuCommand menuItem = new OleMenuCommand(Execute, menuCommandId);
            menuItem.BeforeQueryStatus += (sender, e) => { ((OleMenuCommand) sender).Enabled = !_commandExecutionLimiter.IsCommandExecuted; };
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static RunSolutionAnalysisCommand Instance { get; private set; }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider => _package;

        public static async Task InitializeAsync(AsyncPackage package, CommandExecutionLimiter commandExecutionLimiter)
        {
            // Switch to the main thread - the call to AddCommand in RunSolutionAnalysisCommand's constructor requires the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new RunSolutionAnalysisCommand(package, commandService, commandExecutionLimiter);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _commandExecutionLimiter.StartCommandExec();
            EnvDTE.DTE dte = (EnvDTE.DTE) ThreadHelper.JoinableTaskFactory.Run(() => ServiceProvider.GetServiceAsync(typeof(EnvDTE.DTE)));
            EnvDTE.Solution solution = dte.Solution;
            String target = solution.FileName;
            ThreadHelper.JoinableTaskFactory.RunAsync(() => ExecuteSolutionAnalysisAsync(target));
        }

        private async Task ExecuteSolutionAnalysisAsync(String target)
        {
            IConfigDataProvider configDataProvider = new AppDataConfigDataProvider(ConfigDefs.ConfigDirectory, ConfigDefs.ConfigFilename);
            String appPath = await SourceCodeAnalysisAppHelper.PrepareAnalysisAppAsync(_package, configDataProvider.GetAppPath());
            if (!String.IsNullOrEmpty(appPath))
                await ExecuteSolutionAnalysisImplAsync(target, appPath, configDataProvider);
            await _commandExecutionLimiter.StopCommandExecAsync();
        }

        private async Task ExecuteSolutionAnalysisImplAsync(String target, String appPath, IConfigDataProvider configDataProvider)
        {
            await OutputHelper.OutputMessageAsync(ServiceProvider, $"Source code analysis for solution named \"{target}\" is started");
            String configPath = ConfigFinder.FindConfig(configDataProvider, target);
            ExecutionResult result = await ExecutionHelper.ExecuteSourceCodeAnalysisAsync(appPath, target, configPath);
            await OutputHelper.OutputTargetAnalysisResultAsync(ServiceProvider, result, target, "solution");
            await OutputHelper.OutputMessageAsync(ServiceProvider, $"Source code analysis for solution named \"{target}\" is finished");
            await UIHelper.ShowSolutionSummaryAsync(_package, result, target);
        }

        private readonly CommandExecutionLimiter _commandExecutionLimiter;
    }
}

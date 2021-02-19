using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using SourceCodeAnalysisVSExtension.Launcher;
using SourceCodeAnalysisVSExtension.Utils;
using Task = System.Threading.Tasks.Task;

namespace SourceCodeAnalysisVSExtension
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RunProjectAnalysisCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4129;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("d05bac42-50ab-432d-b353-4693876523a5");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage _package;

        private RunProjectAnalysisCommand(AsyncPackage package, OleMenuCommandService commandService, CommandExecutionLimiter commandExecutionLimiter)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            _commandExecutionLimiter = commandExecutionLimiter;
            CommandID menuCommandId = new CommandID(CommandSet, CommandId);
            OleMenuCommand menuItem = new OleMenuCommand(Execute, menuCommandId);
            menuItem.BeforeQueryStatus += (sender, e) => { ((OleMenuCommand)sender).Enabled = !_commandExecutionLimiter.IsCommandExecuted; };
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static RunProjectAnalysisCommand Instance { get; private set; }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider => _package;

        public static async Task InitializeAsync(AsyncPackage package, CommandExecutionLimiter commandExecutionLimiter)
        {
            // Switch to the main thread - the call to AddCommand in RunProjectAnalysisCommand's constructor requires the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new RunProjectAnalysisCommand(package, commandService, commandExecutionLimiter);
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
            IList<ProjectData> projects = (dte.ActiveSolutionProjects as Object[] ?? new Object[0]).OfType<EnvDTE.Project>().Select(project => new ProjectData(project)).ToList();
            ProjectsAnalysisStat statistics = new ProjectsAnalysisStat(String.Join(",", projects.Select(project => project.Name)), projects.Count);
            ThreadHelper.JoinableTaskFactory.RunAsync(() => ExecuteProjectsAnalysisAsync(projects, statistics));
        }

        private async Task ExecuteProjectsAnalysisAsync(IList<ProjectData> projects, ProjectsAnalysisStat statistics)
        {
            String appPath = await SourceCodeAnalysisAppHelper.PrepareAnalysisAppAsync(_package);
            if (!String.IsNullOrEmpty(appPath))
                await ExecuteProjectsAnalysisImplAsync(projects, statistics, appPath);
            await _commandExecutionLimiter.StopCommandExecAsync();
        }

        private async Task ExecuteProjectsAnalysisImplAsync(IList<ProjectData> projects, ProjectsAnalysisStat statistics, String appPath)
        {
            await OutputHelper.OutputMessageAsync(ServiceProvider, $"Source code analysis for {statistics.TargetType} named \"{statistics.ProjectNames}\" is started");
            foreach (ProjectData project in projects)
            {
                if (String.Equals(project.LanguageId, LanguageCSharpId))
                {
                    ExecutionResult result = await ExecutionHelper.ExecuteSourceCodeAnalysisAsync(appPath, project.FileName);
                    if (result.ExitCode == 0)
                        ++statistics.SuccessCount;
                    else
                        ++statistics.FailedCount;
                    await OutputHelper.OutputTargetAnalysisResultAsync(ServiceProvider, result, project.FileName, "project");
                }
                else
                {
                    ++statistics.SkippedCount;
                    await OutputHelper.OutputMessageAsync(ServiceProvider, $"Project \"{project.FileName}\" can't be processed due to unsupported language");
                }
            }
            await OutputHelper.OutputMessageAsync(ServiceProvider, $"Source code analysis for {statistics.TargetType} named \"{statistics.ProjectNames}\" is finished");
            await UIHelper.ShowProjectsSummaryAsync(_package, statistics);
        }

        private readonly CommandExecutionLimiter _commandExecutionLimiter;

        private const String LanguageCSharpId = "{B5E9BD34-6D3E-4B5D-925E-8A43B79820B4}";
    }
}

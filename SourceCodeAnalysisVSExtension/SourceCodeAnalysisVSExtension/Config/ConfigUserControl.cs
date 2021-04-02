using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SourceCodeAnalysisVSExtension.Config
{
    public partial class ConfigUserControl : UserControl
    {
        public ConfigUserControl(Func<IConfigDataProvider> configProviderFactory)
        {
            InitializeComponent();
            PrepareToolTip();
            _configProviderFactory = configProviderFactory;
        }

        private void ConfigUserControl_Load(Object sender, EventArgs e)
        {
            _configProvider = _configProviderFactory();
            ClearAllErrorProviders();
            textBoxAppPath.Text = _configProvider.GetAppPath();
            listBoxSources.Items.Clear();
            foreach (SourceEntry entry in _configProvider.GetEntries())
                listBoxSources.Items.Add(entry.Source);
            listBoxSources.SelectedIndex = -1;
            textBoxSourcePath.Text = "";
            textBoxConfigPath.Text = "";
            panelSelectedSourceEntry.Enabled = false;
        }

        private void buttonShowAppDialog_Click(Object sender, EventArgs e)
        {
            String appPath = Environment.ExpandEnvironmentVariables(textBoxAppPath.Text.Trim());
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;
                if (Directory.Exists(appPath))
                    dialog.InitialDirectory = appPath;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    textBoxAppPath.Text = dialog.FileName;
            }
            /*using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (Directory.Exists(appPath))
                    dialog.SelectedPath = appPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxAppPath.Text = dialog.SelectedPath;
            }*/
        }

        private void buttonUpdateAppLocation_Click(Object sender, EventArgs e)
        {
            String appPath = textBoxAppPath.Text.Trim();
            if (!ValidateAppLocation(appPath))
                return;
            _configProvider.SaveAppPath(appPath);
        }

        private void buttonAddSourceEntry_Click(Object sender, EventArgs e)
        {
            if (!_hasNewRow)
            {
                listBoxSources.Items.Add(Empty);
                _hasNewRow = true;
            }
            listBoxSources.SelectedIndex = listBoxSources.Items.Count - 1;
        }

        private void buttonRemoveSourceEntry_Click(Object sender, EventArgs e)
        {
            if (listBoxSources.SelectedIndex == -1)
                return;
            if (IsNewEntrySelected())
            {
                listBoxSources.Items.RemoveAt(listBoxSources.SelectedIndex);
                _hasNewRow = false;
                return;
            }
            _configProvider.RemoveSourceEntry(listBoxSources.SelectedIndex);
            listBoxSources.Items.RemoveAt(listBoxSources.SelectedIndex);
        }

        private void buttonShowSourceDialog_Click(Object sender, EventArgs e)
        {
            String sourcePath = Environment.ExpandEnvironmentVariables(textBoxSourcePath.Text.Trim());
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;
                if (Directory.Exists(sourcePath))
                    dialog.InitialDirectory = sourcePath;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    textBoxAppPath.Text = dialog.FileName;
            }
            /*using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (Directory.Exists(sourcePath))
                    dialog.SelectedPath = sourcePath;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxSourcePath.Text = dialog.SelectedPath;
            }*/
        }

        private void buttonShowConfigDialog_Click(Object sender, EventArgs e)
        {
            String config = Environment.ExpandEnvironmentVariables(textBoxConfigPath.Text.Trim());
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                //if (Directory.Exists(config))
                //    dialog.InitialDirectory = config;
                if (File.Exists(config))
                    dialog.InitialDirectory = Path.GetDirectoryName(config);
                dialog.Filter = "config files (*.config)|*.config";
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxConfigPath.Text = dialog.FileName;
            }
        }

        private void buttonUpdateSourceEntry_Click(Object sender, EventArgs e)
        {
            String source = textBoxSourcePath.Text.Trim();
            String config = textBoxConfigPath.Text.Trim();
            if (!ValidateSourceEntry(source, config))
                return;
            if (IsNewEntrySelected())
            {
                _configProvider.CreateSourceEntry(source, config);
                _hasNewRow = false;
            }
            else
                _configProvider.SaveSourceEntry(listBoxSources.SelectedIndex, source, config);
            listBoxSources.Items[listBoxSources.SelectedIndex] = source;
        }

        private void listBoxSources_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (listBoxSources.SelectedIndex == -1)
            {
                textBoxSourcePath.Text = "";
                textBoxConfigPath.Text = "";
                panelSelectedSourceEntry.Enabled = false;
            }
            else if (IsNewEntrySelected())
            {
                textBoxSourcePath.Text = "";
                textBoxConfigPath.Text = "";
                panelSelectedSourceEntry.Enabled = true;
            }
            else
            {
                SourceEntry entry = _configProvider.GetEntry(listBoxSources.SelectedIndex);
                textBoxSourcePath.Text = entry.Source;
                textBoxConfigPath.Text = entry.Config;
                panelSelectedSourceEntry.Enabled = true;
            }
        }

        private void textBoxAppPath_TextChanged(Object sender, EventArgs e)
        {
            errorProviderAppPath.Clear();
            SetWarning(errorProviderAppPathWarning, textBoxAppPath, _configProvider.GetAppPath(), "App path is changed");
        }

        private void textBoxSourcePath_TextChanged(Object sender, EventArgs e)
        {
            errorProviderSourcePath.Clear();
            if (listBoxSources.SelectedIndex == -1)
                return;
            String sourceValue =  IsNewEntrySelected() ? "" : _configProvider.GetEntry(listBoxSources.SelectedIndex).Source;
            SetWarning(errorProviderSourcePathWarning, textBoxSourcePath, sourceValue, "Source path is changed");
        }

        private void textBoxConfigPath_TextChanged(object sender, EventArgs e)
        {
            if (listBoxSources.SelectedIndex == -1)
                return;
            String sourceValue = IsNewEntrySelected() ? "" : _configProvider.GetEntry(listBoxSources.SelectedIndex).Config;
            SetWarning(errorProviderConfigPathWarning, textBoxConfigPath, sourceValue, "Config path is changed");
        }

        private void PrepareToolTip()
        {
            toolTipController.SetToolTip(buttonAddSourceEntry, "Add");
            toolTipController.SetToolTip(buttonRemoveSourceEntry, "Remove");
            toolTipController.SetToolTip(buttonShowAppDialog, "Show file selection dialog");
            toolTipController.SetToolTip(buttonShowSourceDialog, "Show file selection dialog");
            toolTipController.SetToolTip(buttonShowConfigDialog, "Show file selection dialog");
        }

        private void SetError(ErrorProvider errorProvider, TextBox textBox, String errorText)
        {
            errorProvider.SetError(textBox, errorText);
            errorProvider.SetIconPadding(textBox, 4);
        }

        private void SetWarning(ErrorProvider errorProvider, TextBox textBox, String sourceValue, String warningText)
        {
            if (String.Equals(textBox.Text, sourceValue))
                errorProvider.Clear();
            else
                SetError(errorProvider, textBox, warningText);
        }

        private Boolean ValidateAppLocation(String appPath)
        {
            errorProviderAppPathWarning.Clear();
            if (String.IsNullOrEmpty(appPath))
            {
                SetError(errorProviderAppPath, textBoxAppPath, "Empty app path");
                return false;
            }
            return true;
        }

        private Boolean ValidateSourceEntry(String sourcePath, String configPath)
        {
            errorProviderSourcePathWarning.Clear();
            errorProviderConfigPathWarning.Clear();
            if (String.IsNullOrEmpty(sourcePath))
            {
                SetError(errorProviderSourcePath, textBoxSourcePath, "Empty source path");
                return false;
            }
            return true;
        }

        private void ClearAllErrorProviders()
        {
            // error providers
            errorProviderAppPath.Clear();
            errorProviderSourcePath.Clear();
            // warn providers
            errorProviderAppPathWarning.Clear();
            errorProviderSourcePathWarning.Clear();
            errorProviderConfigPathWarning.Clear();
        }

        private Boolean IsNewEntrySelected()
        {
            if (listBoxSources.SelectedIndex == -1)
                return false;
            return _hasNewRow && listBoxSources.SelectedIndex == (listBoxSources.Items.Count - 1);
        }

        private Boolean _hasNewRow;
        private readonly Func<IConfigDataProvider> _configProviderFactory;
        private IConfigDataProvider _configProvider;

        private const String Empty = "[Empty]";
    }
}

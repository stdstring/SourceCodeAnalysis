using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SourceCodeAnalysisVSExtensionCommon.Config;

namespace SourceCodeAnalysisVSExtensionDev.Config
{
    public partial class ConfigUserControl : UserControl
    {
        public ConfigUserControl(Func<IConfigDataProvider> configProviderFactory)
        {
            InitializeComponent();
            PrepareToolTip();
            _configProviderFactory = configProviderFactory;
        }

        // Common
        private void ConfigUserControl_Load(Object sender, EventArgs e)
        {
            _configProvider = _configProviderFactory();
            comboBoxOutputLevel.DataSource = Enum.GetValues(typeof(OutputLevel));
            ClearAllErrorProviders();
            textBoxAppPath.Text = _configProvider.GetAppPath();
            comboBoxOutputLevel.SelectedItem = _configProvider.GetOutputLevel();
            listBoxSources.Items.Clear();
            foreach (SourceEntry entry in _configProvider.GetEntries())
                listBoxSources.Items.Add(entry.Source);
            listBoxSources.SelectedIndex = -1;
            textBoxSourcePath.Text = "";
            textBoxConfigPath.Text = "";
            panelSelectedSourceEntry.Enabled = false;
        }

        // General config
        private void textBoxAppPath_TextChanged(Object sender, EventArgs e)
        {
            errorProviderAppPath.Clear();
            buttonUpdateGeneralConfig.Enabled = IsGeneralConfigChanged();
        }

        private void comboBoxOutputLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonUpdateGeneralConfig.Enabled = IsGeneralConfigChanged();
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
        }

        private void buttonUpdateGeneralConfig_Click(Object sender, EventArgs e)
        {
            String appPath = textBoxAppPath.Text.Trim();
            if (ConfigDataValidator.ValidateAppLocation(appPath, errorProviderAppPath, buttonShowAppDialog))
            {
                _configProvider.SaveAppPath(appPath);
                _configProvider.SaveOutputLevel((OutputLevel) comboBoxOutputLevel.SelectedItem);
            }
            buttonUpdateGeneralConfig.Enabled = false;
        }

        private void buttonClearGeneralConfig_Click(object sender, EventArgs e)
        {
            textBoxAppPath.Text = _configProvider.GetAppPath();
            comboBoxOutputLevel.SelectedItem = _configProvider.GetOutputLevel();
        }

        // Configs locations
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

        private void textBoxSourcePath_TextChanged(Object sender, EventArgs e)
        {
            errorProviderSourcePath.Clear();
            buttonUpdateSourceEntry.Enabled = IsSourceEntryChanged();
        }

        private void textBoxConfigPath_TextChanged(object sender, EventArgs e)
        {
            buttonUpdateSourceEntry.Enabled = IsSourceEntryChanged();
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
                    textBoxSourcePath.Text = dialog.FileName;
            }
        }

        private void buttonShowConfigDialog_Click(Object sender, EventArgs e)
        {
            String config = Environment.ExpandEnvironmentVariables(textBoxConfigPath.Text.Trim());
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
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
            if (!ConfigDataValidator.ValidateSourceEntry(source, config, errorProviderSourcePath, buttonShowSourceDialog))
                return;
            if (IsNewEntrySelected())
            {
                _configProvider.CreateSourceEntry(source, config);
                _hasNewRow = false;
            }
            else
                _configProvider.SaveSourceEntry(listBoxSources.SelectedIndex, source, config);
            listBoxSources.Items[listBoxSources.SelectedIndex] = source;
            buttonUpdateSourceEntry.Enabled = false;
        }

        private void buttonClearSourceEntry_Click(object sender, EventArgs e)
        {
            if (listBoxSources.SelectedIndex == -1)
                return;
            bool isNewEntry = IsNewEntrySelected();
            textBoxSourcePath.Text = isNewEntry ? "" : _configProvider.GetEntry(listBoxSources.SelectedIndex).Source;
            textBoxConfigPath.Text = isNewEntry ? "" : _configProvider.GetEntry(listBoxSources.SelectedIndex).Config;
        }

        // Utility methods
        private void PrepareToolTip()
        {
            toolTipController.SetToolTip(buttonAddSourceEntry, "Add");
            toolTipController.SetToolTip(buttonRemoveSourceEntry, "Remove");
            toolTipController.SetToolTip(buttonShowAppDialog, "Show file selection dialog");
            toolTipController.SetToolTip(buttonShowSourceDialog, "Show file selection dialog");
            toolTipController.SetToolTip(buttonShowConfigDialog, "Show file selection dialog");
        }

        private void ClearAllErrorProviders()
        {
            errorProviderAppPath.Clear();
            errorProviderSourcePath.Clear();
        }

        private Boolean IsGeneralConfigChanged()
        {
            OutputLevel outputLevel = (OutputLevel) comboBoxOutputLevel.SelectedItem;
            return !String.Equals(textBoxAppPath.Text, _configProvider.GetAppPath()) || outputLevel != _configProvider.GetOutputLevel();
        }

        private Boolean IsSourceEntryChanged()
        {
            if (listBoxSources.SelectedIndex == -1)
                return false;
            if (IsNewEntrySelected())
                return !String.IsNullOrEmpty(textBoxSourcePath.Text) || !String.IsNullOrEmpty(textBoxConfigPath.Text);
            SourceEntry entry = _configProvider.GetEntry(listBoxSources.SelectedIndex);
            return !String.Equals(textBoxSourcePath.Text, entry.Source) ||
                   !String.Equals(textBoxConfigPath.Text, entry.Config);
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

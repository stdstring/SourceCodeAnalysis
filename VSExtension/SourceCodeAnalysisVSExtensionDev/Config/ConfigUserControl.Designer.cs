
namespace SourceCodeAnalysisVSExtensionDev.Config
{
    partial class ConfigUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigUserControl));
            this.groupGeneralConfig = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelUpdateGeneralConfig = new System.Windows.Forms.TableLayoutPanel();
            this.buttonUpdateGeneralConfig = new System.Windows.Forms.Button();
            this.buttonClearGeneralConfig = new System.Windows.Forms.Button();
            this.tableLayoutOutputLevel = new System.Windows.Forms.TableLayoutPanel();
            this.labelOutputLevel = new System.Windows.Forms.Label();
            this.comboBoxOutputLevel = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelAppLocation = new System.Windows.Forms.TableLayoutPanel();
            this.labelAppPath = new System.Windows.Forms.Label();
            this.textBoxAppPath = new System.Windows.Forms.TextBox();
            this.buttonShowAppDialog = new System.Windows.Forms.Button();
            this.groupConfigsLocations = new System.Windows.Forms.GroupBox();
            this.panelSourcesList = new System.Windows.Forms.Panel();
            this.listBoxSources = new System.Windows.Forms.ListBox();
            this.panelSelectedSourceEntry = new System.Windows.Forms.Panel();
            this.tableLayoutPanelSourceEntry = new System.Windows.Forms.TableLayoutPanel();
            this.buttonShowSourceDialog = new System.Windows.Forms.Button();
            this.buttonShowConfigDialog = new System.Windows.Forms.Button();
            this.labelSource = new System.Windows.Forms.Label();
            this.labelConfig = new System.Windows.Forms.Label();
            this.textBoxSourcePath = new System.Windows.Forms.TextBox();
            this.textBoxConfigPath = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelUpdateSourceEntry = new System.Windows.Forms.TableLayoutPanel();
            this.buttonUpdateSourceEntry = new System.Windows.Forms.Button();
            this.buttonClearSourceEntry = new System.Windows.Forms.Button();
            this.tableLayoutPanelSourcesHeader = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddSourceEntry = new System.Windows.Forms.Button();
            this.buttonRemoveSourceEntry = new System.Windows.Forms.Button();
            this.labelSources = new System.Windows.Forms.Label();
            this.toolTipController = new System.Windows.Forms.ToolTip(this.components);
            this.errorProviderAppPath = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProviderSourcePath = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupGeneralConfig.SuspendLayout();
            this.tableLayoutPanelUpdateGeneralConfig.SuspendLayout();
            this.tableLayoutOutputLevel.SuspendLayout();
            this.tableLayoutPanelAppLocation.SuspendLayout();
            this.groupConfigsLocations.SuspendLayout();
            this.panelSourcesList.SuspendLayout();
            this.panelSelectedSourceEntry.SuspendLayout();
            this.tableLayoutPanelSourceEntry.SuspendLayout();
            this.tableLayoutPanelUpdateSourceEntry.SuspendLayout();
            this.tableLayoutPanelSourcesHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderAppPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderSourcePath)).BeginInit();
            this.SuspendLayout();
            // 
            // groupGeneralConfig
            // 
            this.groupGeneralConfig.Controls.Add(this.tableLayoutPanelUpdateGeneralConfig);
            this.groupGeneralConfig.Controls.Add(this.tableLayoutOutputLevel);
            this.groupGeneralConfig.Controls.Add(this.tableLayoutPanelAppLocation);
            this.groupGeneralConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupGeneralConfig.Location = new System.Drawing.Point(0, 0);
            this.groupGeneralConfig.Name = "groupGeneralConfig";
            this.groupGeneralConfig.Size = new System.Drawing.Size(599, 106);
            this.groupGeneralConfig.TabIndex = 0;
            this.groupGeneralConfig.TabStop = false;
            this.groupGeneralConfig.Text = "General config";
            // 
            // tableLayoutPanelUpdateGeneralConfig
            // 
            this.tableLayoutPanelUpdateGeneralConfig.ColumnCount = 3;
            this.tableLayoutPanelUpdateGeneralConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUpdateGeneralConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanelUpdateGeneralConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanelUpdateGeneralConfig.Controls.Add(this.buttonUpdateGeneralConfig, 1, 0);
            this.tableLayoutPanelUpdateGeneralConfig.Controls.Add(this.buttonClearGeneralConfig, 2, 0);
            this.tableLayoutPanelUpdateGeneralConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelUpdateGeneralConfig.Location = new System.Drawing.Point(3, 72);
            this.tableLayoutPanelUpdateGeneralConfig.Name = "tableLayoutPanelUpdateGeneralConfig";
            this.tableLayoutPanelUpdateGeneralConfig.RowCount = 1;
            this.tableLayoutPanelUpdateGeneralConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUpdateGeneralConfig.Size = new System.Drawing.Size(593, 31);
            this.tableLayoutPanelUpdateGeneralConfig.TabIndex = 3;
            // 
            // buttonUpdateGeneralConfig
            // 
            this.buttonUpdateGeneralConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateGeneralConfig.Location = new System.Drawing.Point(434, 4);
            this.buttonUpdateGeneralConfig.Name = "buttonUpdateGeneralConfig";
            this.buttonUpdateGeneralConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdateGeneralConfig.TabIndex = 0;
            this.buttonUpdateGeneralConfig.Text = "Update";
            this.buttonUpdateGeneralConfig.UseVisualStyleBackColor = true;
            this.buttonUpdateGeneralConfig.Click += new System.EventHandler(this.buttonUpdateGeneralConfig_Click);
            // 
            // buttonClearGeneralConfig
            // 
            this.buttonClearGeneralConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearGeneralConfig.Location = new System.Drawing.Point(515, 4);
            this.buttonClearGeneralConfig.Name = "buttonClearGeneralConfig";
            this.buttonClearGeneralConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonClearGeneralConfig.TabIndex = 1;
            this.buttonClearGeneralConfig.Text = "Clear";
            this.buttonClearGeneralConfig.UseVisualStyleBackColor = true;
            this.buttonClearGeneralConfig.Click += new System.EventHandler(this.buttonClearGeneralConfig_Click);
            // 
            // tableLayoutOutputLevel
            // 
            this.tableLayoutOutputLevel.ColumnCount = 3;
            this.tableLayoutOutputLevel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutOutputLevel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutOutputLevel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutOutputLevel.Controls.Add(this.labelOutputLevel, 0, 0);
            this.tableLayoutOutputLevel.Controls.Add(this.comboBoxOutputLevel, 1, 0);
            this.tableLayoutOutputLevel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutOutputLevel.Location = new System.Drawing.Point(3, 44);
            this.tableLayoutOutputLevel.Name = "tableLayoutOutputLevel";
            this.tableLayoutOutputLevel.RowCount = 1;
            this.tableLayoutOutputLevel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutOutputLevel.Size = new System.Drawing.Size(593, 28);
            this.tableLayoutOutputLevel.TabIndex = 2;
            // 
            // labelOutputLevel
            // 
            this.labelOutputLevel.AutoSize = true;
            this.labelOutputLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutputLevel.Location = new System.Drawing.Point(3, 3);
            this.labelOutputLevel.Margin = new System.Windows.Forms.Padding(3);
            this.labelOutputLevel.Name = "labelOutputLevel";
            this.labelOutputLevel.Size = new System.Drawing.Size(114, 22);
            this.labelOutputLevel.TabIndex = 0;
            this.labelOutputLevel.Text = "Output Level";
            this.labelOutputLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxOutputLevel
            // 
            this.comboBoxOutputLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxOutputLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOutputLevel.FormattingEnabled = true;
            this.comboBoxOutputLevel.Location = new System.Drawing.Point(123, 3);
            this.comboBoxOutputLevel.Name = "comboBoxOutputLevel";
            this.comboBoxOutputLevel.Size = new System.Drawing.Size(445, 21);
            this.comboBoxOutputLevel.TabIndex = 1;
            this.comboBoxOutputLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxOutputLevel_SelectedIndexChanged);
            // 
            // tableLayoutPanelAppLocation
            // 
            this.tableLayoutPanelAppLocation.ColumnCount = 4;
            this.tableLayoutPanelAppLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanelAppLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAppLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAppLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelAppLocation.Controls.Add(this.labelAppPath, 0, 0);
            this.tableLayoutPanelAppLocation.Controls.Add(this.textBoxAppPath, 1, 0);
            this.tableLayoutPanelAppLocation.Controls.Add(this.buttonShowAppDialog, 2, 0);
            this.tableLayoutPanelAppLocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelAppLocation.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelAppLocation.Name = "tableLayoutPanelAppLocation";
            this.tableLayoutPanelAppLocation.RowCount = 1;
            this.tableLayoutPanelAppLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAppLocation.Size = new System.Drawing.Size(593, 28);
            this.tableLayoutPanelAppLocation.TabIndex = 0;
            // 
            // labelAppPath
            // 
            this.labelAppPath.AutoSize = true;
            this.labelAppPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAppPath.Location = new System.Drawing.Point(3, 3);
            this.labelAppPath.Margin = new System.Windows.Forms.Padding(3);
            this.labelAppPath.Name = "labelAppPath";
            this.labelAppPath.Size = new System.Drawing.Size(114, 22);
            this.labelAppPath.TabIndex = 1;
            this.labelAppPath.Text = "App path:";
            this.labelAppPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxAppPath
            // 
            this.textBoxAppPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAppPath.Location = new System.Drawing.Point(123, 3);
            this.textBoxAppPath.Name = "textBoxAppPath";
            this.textBoxAppPath.Size = new System.Drawing.Size(413, 20);
            this.textBoxAppPath.TabIndex = 0;
            this.textBoxAppPath.TextChanged += new System.EventHandler(this.textBoxAppPath_TextChanged);
            // 
            // buttonShowAppDialog
            // 
            this.buttonShowAppDialog.Location = new System.Drawing.Point(542, 3);
            this.buttonShowAppDialog.Name = "buttonShowAppDialog";
            this.buttonShowAppDialog.Size = new System.Drawing.Size(26, 22);
            this.buttonShowAppDialog.TabIndex = 2;
            this.buttonShowAppDialog.Text = "...";
            this.buttonShowAppDialog.UseVisualStyleBackColor = true;
            this.buttonShowAppDialog.Click += new System.EventHandler(this.buttonShowAppDialog_Click);
            // 
            // groupConfigsLocations
            // 
            this.groupConfigsLocations.Controls.Add(this.panelSourcesList);
            this.groupConfigsLocations.Controls.Add(this.panelSelectedSourceEntry);
            this.groupConfigsLocations.Controls.Add(this.tableLayoutPanelSourcesHeader);
            this.groupConfigsLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupConfigsLocations.Location = new System.Drawing.Point(0, 106);
            this.groupConfigsLocations.Name = "groupConfigsLocations";
            this.groupConfigsLocations.Size = new System.Drawing.Size(599, 390);
            this.groupConfigsLocations.TabIndex = 1;
            this.groupConfigsLocations.TabStop = false;
            this.groupConfigsLocations.Text = "Configs locations";
            // 
            // panelSourcesList
            // 
            this.panelSourcesList.Controls.Add(this.listBoxSources);
            this.panelSourcesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSourcesList.Location = new System.Drawing.Point(3, 45);
            this.panelSourcesList.Name = "panelSourcesList";
            this.panelSourcesList.Size = new System.Drawing.Size(593, 252);
            this.panelSourcesList.TabIndex = 2;
            // 
            // listBoxSources
            // 
            this.listBoxSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSources.FormattingEnabled = true;
            this.listBoxSources.Location = new System.Drawing.Point(0, 0);
            this.listBoxSources.Name = "listBoxSources";
            this.listBoxSources.Size = new System.Drawing.Size(593, 252);
            this.listBoxSources.TabIndex = 0;
            this.listBoxSources.SelectedIndexChanged += new System.EventHandler(this.listBoxSources_SelectedIndexChanged);
            // 
            // panelSelectedSourceEntry
            // 
            this.panelSelectedSourceEntry.Controls.Add(this.tableLayoutPanelSourceEntry);
            this.panelSelectedSourceEntry.Controls.Add(this.tableLayoutPanelUpdateSourceEntry);
            this.panelSelectedSourceEntry.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSelectedSourceEntry.Enabled = false;
            this.panelSelectedSourceEntry.Location = new System.Drawing.Point(3, 297);
            this.panelSelectedSourceEntry.Name = "panelSelectedSourceEntry";
            this.panelSelectedSourceEntry.Size = new System.Drawing.Size(593, 90);
            this.panelSelectedSourceEntry.TabIndex = 1;
            // 
            // tableLayoutPanelSourceEntry
            // 
            this.tableLayoutPanelSourceEntry.ColumnCount = 4;
            this.tableLayoutPanelSourceEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanelSourceEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSourceEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSourceEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelSourceEntry.Controls.Add(this.buttonShowSourceDialog, 2, 0);
            this.tableLayoutPanelSourceEntry.Controls.Add(this.buttonShowConfigDialog, 2, 1);
            this.tableLayoutPanelSourceEntry.Controls.Add(this.labelSource, 0, 0);
            this.tableLayoutPanelSourceEntry.Controls.Add(this.labelConfig, 0, 1);
            this.tableLayoutPanelSourceEntry.Controls.Add(this.textBoxSourcePath, 1, 0);
            this.tableLayoutPanelSourceEntry.Controls.Add(this.textBoxConfigPath, 1, 1);
            this.tableLayoutPanelSourceEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSourceEntry.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSourceEntry.Name = "tableLayoutPanelSourceEntry";
            this.tableLayoutPanelSourceEntry.RowCount = 2;
            this.tableLayoutPanelSourceEntry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSourceEntry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSourceEntry.Size = new System.Drawing.Size(593, 59);
            this.tableLayoutPanelSourceEntry.TabIndex = 1;
            // 
            // buttonShowSourceDialog
            // 
            this.buttonShowSourceDialog.Location = new System.Drawing.Point(539, 3);
            this.buttonShowSourceDialog.Name = "buttonShowSourceDialog";
            this.buttonShowSourceDialog.Size = new System.Drawing.Size(26, 22);
            this.buttonShowSourceDialog.TabIndex = 0;
            this.buttonShowSourceDialog.Text = "...";
            this.buttonShowSourceDialog.UseVisualStyleBackColor = true;
            this.buttonShowSourceDialog.Click += new System.EventHandler(this.buttonShowSourceDialog_Click);
            // 
            // buttonShowConfigDialog
            // 
            this.buttonShowConfigDialog.Location = new System.Drawing.Point(539, 32);
            this.buttonShowConfigDialog.Name = "buttonShowConfigDialog";
            this.buttonShowConfigDialog.Size = new System.Drawing.Size(26, 22);
            this.buttonShowConfigDialog.TabIndex = 1;
            this.buttonShowConfigDialog.Text = "...";
            this.buttonShowConfigDialog.UseVisualStyleBackColor = true;
            this.buttonShowConfigDialog.Click += new System.EventHandler(this.buttonShowConfigDialog_Click);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.Location = new System.Drawing.Point(3, 3);
            this.labelSource.Margin = new System.Windows.Forms.Padding(3);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(114, 23);
            this.labelSource.TabIndex = 2;
            this.labelSource.Text = "Source:";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelConfig
            // 
            this.labelConfig.AutoSize = true;
            this.labelConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelConfig.Location = new System.Drawing.Point(3, 32);
            this.labelConfig.Margin = new System.Windows.Forms.Padding(3);
            this.labelConfig.Name = "labelConfig";
            this.labelConfig.Size = new System.Drawing.Size(114, 24);
            this.labelConfig.TabIndex = 3;
            this.labelConfig.Text = "Config:";
            this.labelConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxSourcePath
            // 
            this.textBoxSourcePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSourcePath.Location = new System.Drawing.Point(123, 3);
            this.textBoxSourcePath.Name = "textBoxSourcePath";
            this.textBoxSourcePath.Size = new System.Drawing.Size(410, 20);
            this.textBoxSourcePath.TabIndex = 4;
            this.textBoxSourcePath.TextChanged += new System.EventHandler(this.textBoxSourcePath_TextChanged);
            // 
            // textBoxConfigPath
            // 
            this.textBoxConfigPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxConfigPath.Location = new System.Drawing.Point(123, 32);
            this.textBoxConfigPath.Name = "textBoxConfigPath";
            this.textBoxConfigPath.Size = new System.Drawing.Size(410, 20);
            this.textBoxConfigPath.TabIndex = 5;
            this.textBoxConfigPath.TextChanged += new System.EventHandler(this.textBoxConfigPath_TextChanged);
            // 
            // tableLayoutPanelUpdateSourceEntry
            // 
            this.tableLayoutPanelUpdateSourceEntry.ColumnCount = 3;
            this.tableLayoutPanelUpdateSourceEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUpdateSourceEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanelUpdateSourceEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanelUpdateSourceEntry.Controls.Add(this.buttonUpdateSourceEntry, 1, 0);
            this.tableLayoutPanelUpdateSourceEntry.Controls.Add(this.buttonClearSourceEntry, 2, 0);
            this.tableLayoutPanelUpdateSourceEntry.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelUpdateSourceEntry.Location = new System.Drawing.Point(0, 59);
            this.tableLayoutPanelUpdateSourceEntry.Name = "tableLayoutPanelUpdateSourceEntry";
            this.tableLayoutPanelUpdateSourceEntry.RowCount = 1;
            this.tableLayoutPanelUpdateSourceEntry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUpdateSourceEntry.Size = new System.Drawing.Size(593, 31);
            this.tableLayoutPanelUpdateSourceEntry.TabIndex = 2;
            // 
            // buttonUpdateSourceEntry
            // 
            this.buttonUpdateSourceEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateSourceEntry.Location = new System.Drawing.Point(434, 4);
            this.buttonUpdateSourceEntry.Name = "buttonUpdateSourceEntry";
            this.buttonUpdateSourceEntry.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdateSourceEntry.TabIndex = 0;
            this.buttonUpdateSourceEntry.Text = "Update";
            this.buttonUpdateSourceEntry.UseVisualStyleBackColor = true;
            this.buttonUpdateSourceEntry.Click += new System.EventHandler(this.buttonUpdateSourceEntry_Click);
            // 
            // buttonClearSourceEntry
            // 
            this.buttonClearSourceEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearSourceEntry.Location = new System.Drawing.Point(515, 4);
            this.buttonClearSourceEntry.Name = "buttonClearSourceEntry";
            this.buttonClearSourceEntry.Size = new System.Drawing.Size(75, 23);
            this.buttonClearSourceEntry.TabIndex = 1;
            this.buttonClearSourceEntry.Text = "Clear";
            this.buttonClearSourceEntry.UseVisualStyleBackColor = true;
            this.buttonClearSourceEntry.Click += new System.EventHandler(this.buttonClearSourceEntry_Click);
            // 
            // tableLayoutPanelSourcesHeader
            // 
            this.tableLayoutPanelSourcesHeader.ColumnCount = 3;
            this.tableLayoutPanelSourcesHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSourcesHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSourcesHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSourcesHeader.Controls.Add(this.buttonAddSourceEntry, 1, 0);
            this.tableLayoutPanelSourcesHeader.Controls.Add(this.buttonRemoveSourceEntry, 2, 0);
            this.tableLayoutPanelSourcesHeader.Controls.Add(this.labelSources, 0, 0);
            this.tableLayoutPanelSourcesHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSourcesHeader.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelSourcesHeader.Name = "tableLayoutPanelSourcesHeader";
            this.tableLayoutPanelSourcesHeader.RowCount = 1;
            this.tableLayoutPanelSourcesHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSourcesHeader.Size = new System.Drawing.Size(593, 29);
            this.tableLayoutPanelSourcesHeader.TabIndex = 0;
            // 
            // buttonAddSourceEntry
            // 
            this.buttonAddSourceEntry.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddSourceEntry.Image")));
            this.buttonAddSourceEntry.Location = new System.Drawing.Point(538, 3);
            this.buttonAddSourceEntry.Name = "buttonAddSourceEntry";
            this.buttonAddSourceEntry.Size = new System.Drawing.Size(23, 23);
            this.buttonAddSourceEntry.TabIndex = 0;
            this.buttonAddSourceEntry.UseVisualStyleBackColor = true;
            this.buttonAddSourceEntry.Click += new System.EventHandler(this.buttonAddSourceEntry_Click);
            // 
            // buttonRemoveSourceEntry
            // 
            this.buttonRemoveSourceEntry.Image = ((System.Drawing.Image)(resources.GetObject("buttonRemoveSourceEntry.Image")));
            this.buttonRemoveSourceEntry.Location = new System.Drawing.Point(567, 3);
            this.buttonRemoveSourceEntry.Name = "buttonRemoveSourceEntry";
            this.buttonRemoveSourceEntry.Size = new System.Drawing.Size(23, 23);
            this.buttonRemoveSourceEntry.TabIndex = 1;
            this.buttonRemoveSourceEntry.UseVisualStyleBackColor = true;
            this.buttonRemoveSourceEntry.Click += new System.EventHandler(this.buttonRemoveSourceEntry_Click);
            // 
            // labelSources
            // 
            this.labelSources.AutoSize = true;
            this.labelSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSources.Location = new System.Drawing.Point(3, 3);
            this.labelSources.Margin = new System.Windows.Forms.Padding(3);
            this.labelSources.Name = "labelSources";
            this.labelSources.Size = new System.Drawing.Size(529, 23);
            this.labelSources.TabIndex = 2;
            this.labelSources.Text = "Source solutions/projects:";
            this.labelSources.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // errorProviderAppPath
            // 
            this.errorProviderAppPath.ContainerControl = this;
            // 
            // errorProviderSourcePath
            // 
            this.errorProviderSourcePath.ContainerControl = this;
            // 
            // ConfigUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupConfigsLocations);
            this.Controls.Add(this.groupGeneralConfig);
            this.Name = "ConfigUserControl";
            this.Size = new System.Drawing.Size(599, 496);
            this.Load += new System.EventHandler(this.ConfigUserControl_Load);
            this.groupGeneralConfig.ResumeLayout(false);
            this.tableLayoutPanelUpdateGeneralConfig.ResumeLayout(false);
            this.tableLayoutOutputLevel.ResumeLayout(false);
            this.tableLayoutOutputLevel.PerformLayout();
            this.tableLayoutPanelAppLocation.ResumeLayout(false);
            this.tableLayoutPanelAppLocation.PerformLayout();
            this.groupConfigsLocations.ResumeLayout(false);
            this.panelSourcesList.ResumeLayout(false);
            this.panelSelectedSourceEntry.ResumeLayout(false);
            this.tableLayoutPanelSourceEntry.ResumeLayout(false);
            this.tableLayoutPanelSourceEntry.PerformLayout();
            this.tableLayoutPanelUpdateSourceEntry.ResumeLayout(false);
            this.tableLayoutPanelSourcesHeader.ResumeLayout(false);
            this.tableLayoutPanelSourcesHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderAppPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderSourcePath)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupGeneralConfig;
        private System.Windows.Forms.GroupBox groupConfigsLocations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAppLocation;
        private System.Windows.Forms.TextBox textBoxAppPath;
        private System.Windows.Forms.Label labelAppPath;
        private System.Windows.Forms.Button buttonShowAppDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSourcesHeader;
        private System.Windows.Forms.Button buttonAddSourceEntry;
        private System.Windows.Forms.Button buttonRemoveSourceEntry;
        private System.Windows.Forms.Label labelSources;
        private System.Windows.Forms.Panel panelSelectedSourceEntry;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSourceEntry;
        private System.Windows.Forms.Button buttonShowSourceDialog;
        private System.Windows.Forms.Button buttonShowConfigDialog;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelConfig;
        private System.Windows.Forms.TextBox textBoxSourcePath;
        private System.Windows.Forms.TextBox textBoxConfigPath;
        private System.Windows.Forms.Panel panelSourcesList;
        private System.Windows.Forms.ListBox listBoxSources;
        private System.Windows.Forms.ToolTip toolTipController;
        private System.Windows.Forms.ErrorProvider errorProviderAppPath;
        private System.Windows.Forms.ErrorProvider errorProviderSourcePath;
        private System.Windows.Forms.TableLayoutPanel tableLayoutOutputLevel;
        private System.Windows.Forms.Label labelOutputLevel;
        private System.Windows.Forms.ComboBox comboBoxOutputLevel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUpdateGeneralConfig;
        private System.Windows.Forms.Button buttonUpdateGeneralConfig;
        private System.Windows.Forms.Button buttonClearGeneralConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUpdateSourceEntry;
        private System.Windows.Forms.Button buttonUpdateSourceEntry;
        private System.Windows.Forms.Button buttonClearSourceEntry;
    }
}

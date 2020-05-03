namespace Escher
{
    partial class Editor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.designMaster = new FastColoredTextBoxNS.FastColoredTextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFindStampNumber = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuBeautify = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuValidate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.upgradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKeywordAssignment = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.panelDesign = new System.Windows.Forms.Panel();
            this.designSlave = new FastColoredTextBoxNS.FastColoredTextBox();
            this.splitter = new System.Windows.Forms.Splitter();
            this.menuSplit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFindAlbumNumber = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.designMaster)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.panelDesign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.designSlave)).BeginInit();
            this.SuspendLayout();
            // 
            // designMaster
            // 
            this.designMaster.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.designMaster.AutoScrollMinSize = new System.Drawing.Size(2, 17);
            this.designMaster.BackBrush = null;
            this.designMaster.CharHeight = 17;
            this.designMaster.CharWidth = 8;
            this.designMaster.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.designMaster.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.designMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.designMaster.Font = new System.Drawing.Font("Consolas", 9F);
            this.designMaster.IsReplaceMode = false;
            this.designMaster.Location = new System.Drawing.Point(0, 0);
            this.designMaster.Name = "designMaster";
            this.designMaster.Paddings = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.designMaster.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.designMaster.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("designMaster.ServiceColors")));
            this.designMaster.ShowLineNumbers = false;
            this.designMaster.Size = new System.Drawing.Size(821, 774);
            this.designMaster.TabIndex = 2;
            this.designMaster.Zoom = 100;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.White;
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuInsert,
            this.upgradeToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(823, 28);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFindStampNumber,
            this.menuFindAlbumNumber,
            this.toolStripMenuItem7,
            this.menuReplace,
            this.toolStripMenuItem1,
            this.menuPreview,
            this.toolStripMenuItem2,
            this.menuBeautify,
            this.toolStripMenuItem6,
            this.menuSplit,
            this.toolStripMenuItem3,
            this.menuValidate,
            this.toolStripMenuItem4,
            this.menuSave,
            this.toolStripMenuItem5,
            this.menuExit});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.ShortcutKeyDisplayString = "";
            this.menuEdit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.menuEdit.Size = new System.Drawing.Size(49, 24);
            this.menuEdit.Text = "Edit";
            // 
            // menuFindStampNumber
            // 
            this.menuFindStampNumber.Name = "menuFindStampNumber";
            this.menuFindStampNumber.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.menuFindStampNumber.Size = new System.Drawing.Size(274, 26);
            this.menuFindStampNumber.Text = "Find Stamp Number";
            // 
            // menuReplace
            // 
            this.menuReplace.Name = "menuReplace";
            this.menuReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
            this.menuReplace.Size = new System.Drawing.Size(274, 26);
            this.menuReplace.Text = "Replace";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(271, 6);
            // 
            // menuPreview
            // 
            this.menuPreview.Name = "menuPreview";
            this.menuPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Space)));
            this.menuPreview.Size = new System.Drawing.Size(274, 26);
            this.menuPreview.Text = "Preview";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(271, 6);
            // 
            // menuBeautify
            // 
            this.menuBeautify.Name = "menuBeautify";
            this.menuBeautify.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.B)));
            this.menuBeautify.Size = new System.Drawing.Size(274, 26);
            this.menuBeautify.Text = "Beautify";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(271, 6);
            // 
            // menuValidate
            // 
            this.menuValidate.Name = "menuValidate";
            this.menuValidate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.V)));
            this.menuValidate.Size = new System.Drawing.Size(274, 26);
            this.menuValidate.Text = "Validate";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(271, 6);
            // 
            // menuSave
            // 
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(274, 26);
            this.menuSave.Text = "Save";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(271, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.menuExit.Size = new System.Drawing.Size(274, 26);
            this.menuExit.Text = "Exit";
            // 
            // menuInsert
            // 
            this.menuInsert.Name = "menuInsert";
            this.menuInsert.Size = new System.Drawing.Size(59, 24);
            this.menuInsert.Text = "Insert";
            // 
            // upgradeToolStripMenuItem
            // 
            this.upgradeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuKeywordAssignment});
            this.upgradeToolStripMenuItem.Name = "upgradeToolStripMenuItem";
            this.upgradeToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.upgradeToolStripMenuItem.Text = "Upgrade";
            // 
            // menuKeywordAssignment
            // 
            this.menuKeywordAssignment.Name = "menuKeywordAssignment";
            this.menuKeywordAssignment.Size = new System.Drawing.Size(208, 26);
            this.menuKeywordAssignment.Text = "Replace \':=\' by \'=\'";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.BackColor = System.Drawing.Color.White;
            this.status.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.Location = new System.Drawing.Point(25, 4);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(47, 20);
            this.status.TabIndex = 4;
            this.status.Text = "status";
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.White;
            this.panelInfo.Controls.Add(this.status);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(0, 28);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(823, 35);
            this.panelInfo.TabIndex = 5;
            // 
            // panelDesign
            // 
            this.panelDesign.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDesign.Controls.Add(this.splitter);
            this.panelDesign.Controls.Add(this.designSlave);
            this.panelDesign.Controls.Add(this.designMaster);
            this.panelDesign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDesign.Location = new System.Drawing.Point(0, 63);
            this.panelDesign.Name = "panelDesign";
            this.panelDesign.Size = new System.Drawing.Size(823, 776);
            this.panelDesign.TabIndex = 6;
            // 
            // designSlave
            // 
            this.designSlave.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.designSlave.AutoScrollMinSize = new System.Drawing.Size(2, 17);
            this.designSlave.BackBrush = null;
            this.designSlave.BackColor = System.Drawing.Color.Snow;
            this.designSlave.CharHeight = 17;
            this.designSlave.CharWidth = 8;
            this.designSlave.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.designSlave.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.designSlave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.designSlave.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.designSlave.IsReplaceMode = false;
            this.designSlave.Location = new System.Drawing.Point(0, 582);
            this.designSlave.Name = "designSlave";
            this.designSlave.Paddings = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.designSlave.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.designSlave.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("designSlave.ServiceColors")));
            this.designSlave.ShowLineNumbers = false;
            this.designSlave.Size = new System.Drawing.Size(821, 192);
            this.designSlave.SourceTextBox = this.designMaster;
            this.designSlave.TabIndex = 3;
            this.designSlave.Visible = false;
            this.designSlave.Zoom = 100;
            // 
            // splitter
            // 
            this.splitter.BackColor = System.Drawing.Color.FloralWhite;
            this.splitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter.Location = new System.Drawing.Point(0, 575);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(821, 7);
            this.splitter.TabIndex = 4;
            this.splitter.TabStop = false;
            this.splitter.Visible = false;
            // 
            // menuSplit
            // 
            this.menuSplit.Name = "menuSplit";
            this.menuSplit.Size = new System.Drawing.Size(274, 26);
            this.menuSplit.Text = "Split";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(271, 6);
            // 
            // menuFindAlbumNumber
            // 
            this.menuFindAlbumNumber.Name = "menuFindAlbumNumber";
            this.menuFindAlbumNumber.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.menuFindAlbumNumber.Size = new System.Drawing.Size(274, 26);
            this.menuFindAlbumNumber.Text = "Find Album Number";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(271, 6);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 839);
            this.ControlBox = false;
            this.Controls.Add(this.panelDesign);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Editor";
            this.Text = "Editor";
            ((System.ComponentModel.ISupportInitialize)(this.designMaster)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelDesign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.designSlave)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FastColoredTextBoxNS.FastColoredTextBox designMaster;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuPreview;
        private System.Windows.Forms.ToolStripMenuItem menuValidate;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelDesign;
        private System.Windows.Forms.ToolStripMenuItem menuInsert;
        private System.Windows.Forms.ToolStripMenuItem menuBeautify;
        private System.Windows.Forms.ToolStripMenuItem menuFindStampNumber;
        private System.Windows.Forms.ToolStripMenuItem menuReplace;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem upgradeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuKeywordAssignment;
        private FastColoredTextBoxNS.FastColoredTextBox designSlave;
        private System.Windows.Forms.Splitter splitter;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem menuSplit;
        private System.Windows.Forms.ToolStripMenuItem menuFindAlbumNumber;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
    }
}
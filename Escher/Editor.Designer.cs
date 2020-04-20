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
            this.design = new FastColoredTextBoxNS.FastColoredTextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.menuValidate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.panelDesign = new System.Windows.Forms.Panel();
            this.menuInsert = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.design)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.panelDesign.SuspendLayout();
            this.SuspendLayout();
            // 
            // design
            // 
            this.design.AutoCompleteBracketsList = new char[] {
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
            this.design.AutoScrollMinSize = new System.Drawing.Size(2, 17);
            this.design.BackBrush = null;
            this.design.CharHeight = 17;
            this.design.CharWidth = 8;
            this.design.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.design.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.design.Dock = System.Windows.Forms.DockStyle.Fill;
            this.design.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.design.IsReplaceMode = false;
            this.design.Location = new System.Drawing.Point(0, 0);
            this.design.Name = "design";
            this.design.Paddings = new System.Windows.Forms.Padding(0);
            this.design.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.design.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("design.ServiceColors")));
            this.design.ShowLineNumbers = false;
            this.design.Size = new System.Drawing.Size(821, 774);
            this.design.TabIndex = 2;
            this.design.Zoom = 100;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.White;
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuInsert});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(823, 28);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuPreview,
            this.menuValidate,
            this.menuSave,
            this.menuExit});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(49, 24);
            this.menuEdit.Text = "Edit";
            // 
            // menuPreview
            // 
            this.menuPreview.Name = "menuPreview";
            this.menuPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.menuPreview.Size = new System.Drawing.Size(224, 26);
            this.menuPreview.Text = "Preview";
            // 
            // menuValidate
            // 
            this.menuValidate.Name = "menuValidate";
            this.menuValidate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.V)));
            this.menuValidate.Size = new System.Drawing.Size(224, 26);
            this.menuValidate.Text = "Validate";
            // 
            // menuSave
            // 
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(224, 26);
            this.menuSave.Text = "Save";
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.menuExit.Size = new System.Drawing.Size(224, 26);
            this.menuExit.Text = "Exit";
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
            this.panelDesign.Controls.Add(this.design);
            this.panelDesign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDesign.Location = new System.Drawing.Point(0, 63);
            this.panelDesign.Name = "panelDesign";
            this.panelDesign.Size = new System.Drawing.Size(823, 776);
            this.panelDesign.TabIndex = 6;
            // 
            // menuInsert
            // 
            this.menuInsert.Name = "menuInsert";
            this.menuInsert.Size = new System.Drawing.Size(59, 24);
            this.menuInsert.Text = "Insert";
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
            ((System.ComponentModel.ISupportInitialize)(this.design)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelDesign.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FastColoredTextBoxNS.FastColoredTextBox design;
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
    }
}
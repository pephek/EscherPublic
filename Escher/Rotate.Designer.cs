namespace Escher
{
    partial class Rotate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rotate));
            this.udAngle = new System.Windows.Forms.NumericUpDown();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.panelSelection = new System.Windows.Forms.Panel();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonReject = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.udAngle)).BeginInit();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.panelSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // udAngle
            // 
            this.udAngle.DecimalPlaces = 1;
            this.udAngle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udAngle.Location = new System.Drawing.Point(6, 8);
            this.udAngle.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.udAngle.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.udAngle.Name = "udAngle";
            this.udAngle.Size = new System.Drawing.Size(57, 22);
            this.udAngle.TabIndex = 1;
            this.udAngle.ValueChanged += new System.EventHandler(this.udAngle_ValueChanged);
            this.udAngle.Enter += new System.EventHandler(this.udAngle_ValueChanged);
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Black;
            this.panelButtons.Controls.Add(this.buttonSelect);
            this.panelButtons.Controls.Add(this.buttonSave);
            this.panelButtons.Controls.Add(this.buttonClose);
            this.panelButtons.Controls.Add(this.udAngle);
            this.panelButtons.Location = new System.Drawing.Point(341, 303);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(171, 39);
            this.panelButtons.TabIndex = 2;
            // 
            // buttonSave
            // 
            this.buttonSave.Image = global::Escher.Properties.Resources.Save_16x;
            this.buttonSave.Location = new System.Drawing.Point(102, 4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(32, 32);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Image = global::Escher.Properties.Resources.Close_red_16x;
            this.buttonClose.Location = new System.Drawing.Point(135, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(32, 32);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.TabStop = false;
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // pbImage
            // 
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Location = new System.Drawing.Point(248, 149);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(124, 91);
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Image = global::Escher.Properties.Resources.RectangularSelection_16x;
            this.buttonSelect.Location = new System.Drawing.Point(69, 3);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(32, 32);
            this.buttonSelect.TabIndex = 10;
            this.buttonSelect.UseVisualStyleBackColor = true;
            // 
            // panelSelection
            // 
            this.panelSelection.BackColor = System.Drawing.Color.Black;
            this.panelSelection.Controls.Add(this.buttonZoomOut);
            this.panelSelection.Controls.Add(this.buttonZoomIn);
            this.panelSelection.Controls.Add(this.buttonAccept);
            this.panelSelection.Controls.Add(this.buttonReject);
            this.panelSelection.Location = new System.Drawing.Point(371, 362);
            this.panelSelection.Name = "panelSelection";
            this.panelSelection.Size = new System.Drawing.Size(137, 39);
            this.panelSelection.TabIndex = 9;
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Enabled = false;
            this.buttonZoomOut.Image = global::Escher.Properties.Resources.ZoomOut_16x;
            this.buttonZoomOut.Location = new System.Drawing.Point(36, 3);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(32, 32);
            this.buttonZoomOut.TabIndex = 3;
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Enabled = false;
            this.buttonZoomIn.Image = global::Escher.Properties.Resources.ZoomIn_16x1;
            this.buttonZoomIn.Location = new System.Drawing.Point(3, 3);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(32, 32);
            this.buttonZoomIn.TabIndex = 2;
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            // 
            // buttonAccept
            // 
            this.buttonAccept.Image = global::Escher.Properties.Resources.Checkmark_16x;
            this.buttonAccept.Location = new System.Drawing.Point(102, 3);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(32, 32);
            this.buttonAccept.TabIndex = 1;
            this.buttonAccept.UseVisualStyleBackColor = true;
            // 
            // buttonReject
            // 
            this.buttonReject.Image = global::Escher.Properties.Resources.Close_red_16x;
            this.buttonReject.Location = new System.Drawing.Point(69, 3);
            this.buttonReject.Name = "buttonReject";
            this.buttonReject.Size = new System.Drawing.Size(32, 32);
            this.buttonReject.TabIndex = 0;
            this.buttonReject.UseVisualStyleBackColor = true;
            // 
            // Rotate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelSelection);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.pbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Rotate";
            this.Text = "Rotate";
            ((System.ComponentModel.ISupportInitialize)(this.udAngle)).EndInit();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.panelSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.NumericUpDown udAngle;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Panel panelSelection;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonReject;
    }
}
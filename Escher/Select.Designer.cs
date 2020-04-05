namespace Escher
{
    partial class Select
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Select));
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonToggle = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.panelSelection = new System.Windows.Forms.Panel();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonReject = new System.Windows.Forms.Button();
            this.pbPrint = new System.Windows.Forms.PictureBox();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.panelButtons.SuspendLayout();
            this.panelSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Black;
            this.panelButtons.Controls.Add(this.buttonToggle);
            this.panelButtons.Controls.Add(this.buttonSelect);
            this.panelButtons.Controls.Add(this.buttonClose);
            this.panelButtons.Controls.Add(this.buttonSave);
            this.panelButtons.Controls.Add(this.buttonPrev);
            this.panelButtons.Controls.Add(this.buttonNext);
            this.panelButtons.Location = new System.Drawing.Point(168, 261);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(204, 39);
            this.panelButtons.TabIndex = 4;
            // 
            // buttonToggle
            // 
            this.buttonToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonToggle.Image = global::Escher.Properties.Resources.Reverse_16x;
            this.buttonToggle.Location = new System.Drawing.Point(69, 3);
            this.buttonToggle.Name = "buttonToggle";
            this.buttonToggle.Size = new System.Drawing.Size(32, 32);
            this.buttonToggle.TabIndex = 9;
            this.buttonToggle.TabStop = false;
            this.buttonToggle.UseVisualStyleBackColor = true;
            this.buttonToggle.Click += new System.EventHandler(this.buttonToggle_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSelect.Image = global::Escher.Properties.Resources.RectangularSelection_16x;
            this.buttonSelect.Location = new System.Drawing.Point(102, 3);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(32, 32);
            this.buttonSelect.TabIndex = 8;
            this.buttonSelect.TabStop = false;
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Image = global::Escher.Properties.Resources.Close_red_16x;
            this.buttonClose.Location = new System.Drawing.Point(168, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(32, 32);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.TabStop = false;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Image = global::Escher.Properties.Resources.Save_16x;
            this.buttonSave.Location = new System.Drawing.Point(135, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(32, 32);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.TabStop = false;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonPrev
            // 
            this.buttonPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrev.Image = global::Escher.Properties.Resources.Previous_16x;
            this.buttonPrev.Location = new System.Drawing.Point(3, 3);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(32, 32);
            this.buttonPrev.TabIndex = 3;
            this.buttonPrev.TabStop = false;
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNext.Image = global::Escher.Properties.Resources.Next_16x;
            this.buttonNext.Location = new System.Drawing.Point(36, 3);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(32, 32);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.TabStop = false;
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // panelSelection
            // 
            this.panelSelection.BackColor = System.Drawing.Color.Black;
            this.panelSelection.Controls.Add(this.buttonZoomOut);
            this.panelSelection.Controls.Add(this.buttonZoomIn);
            this.panelSelection.Controls.Add(this.buttonAccept);
            this.panelSelection.Controls.Add(this.buttonReject);
            this.panelSelection.Location = new System.Drawing.Point(225, 327);
            this.panelSelection.Name = "panelSelection";
            this.panelSelection.Size = new System.Drawing.Size(137, 39);
            this.panelSelection.TabIndex = 8;
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
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonReject
            // 
            this.buttonReject.Image = global::Escher.Properties.Resources.Close_red_16x;
            this.buttonReject.Location = new System.Drawing.Point(69, 3);
            this.buttonReject.Name = "buttonReject";
            this.buttonReject.Size = new System.Drawing.Size(32, 32);
            this.buttonReject.TabIndex = 0;
            this.buttonReject.UseVisualStyleBackColor = true;
            this.buttonReject.Click += new System.EventHandler(this.buttonReject_Click);
            // 
            // pbPrint
            // 
            this.pbPrint.Location = new System.Drawing.Point(295, 199);
            this.pbPrint.Name = "pbPrint";
            this.pbPrint.Size = new System.Drawing.Size(124, 75);
            this.pbPrint.TabIndex = 7;
            this.pbPrint.TabStop = false;
            this.pbPrint.Click += new System.EventHandler(this.pbPrint_Click);
            // 
            // pbColor
            // 
            this.pbColor.Location = new System.Drawing.Point(228, 157);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new System.Drawing.Size(101, 81);
            this.pbColor.TabIndex = 6;
            this.pbColor.TabStop = false;
            this.pbColor.Click += new System.EventHandler(this.pbColor_Click);
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(154, 111);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(135, 112);
            this.pbImage.TabIndex = 5;
            this.pbImage.TabStop = false;
            this.pbImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pbImage_Paint);
            this.pbImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseDown);
            this.pbImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseMove);
            this.pbImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseUp);
            // 
            // Select
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelSelection);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.pbPrint);
            this.Controls.Add(this.pbColor);
            this.Controls.Add(this.pbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Select";
            this.Text = "Select";
            this.panelButtons.ResumeLayout(false);
            this.panelSelection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.PictureBox pbColor;
        private System.Windows.Forms.PictureBox pbPrint;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonToggle;
        private System.Windows.Forms.Panel panelSelection;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonReject;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Button buttonZoomIn;
    }
}
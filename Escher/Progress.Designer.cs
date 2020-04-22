namespace Escher
{
    partial class Progress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Progress));
            this.labelPrinting = new System.Windows.Forms.Label();
            this.labelWaiting = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelCreating = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPrinting
            // 
            this.labelPrinting.AutoSize = true;
            this.labelPrinting.Location = new System.Drawing.Point(12, 10);
            this.labelPrinting.Name = "labelPrinting";
            this.labelPrinting.Size = new System.Drawing.Size(152, 17);
            this.labelPrinting.TabIndex = 0;
            this.labelPrinting.Text = "1 · Printing page # of #";
            // 
            // labelWaiting
            // 
            this.labelWaiting.AutoSize = true;
            this.labelWaiting.ForeColor = System.Drawing.Color.Silver;
            this.labelWaiting.Location = new System.Drawing.Point(12, 60);
            this.labelWaiting.Name = "labelWaiting";
            this.labelWaiting.Size = new System.Drawing.Size(274, 17);
            this.labelWaiting.TabIndex = 3;
            this.labelWaiting.Text = "3 · Waiting for completion of the document";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(218, 95);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(68, 41);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelCreating
            // 
            this.labelCreating.AutoSize = true;
            this.labelCreating.Location = new System.Drawing.Point(12, 35);
            this.labelCreating.Name = "labelCreating";
            this.labelCreating.Size = new System.Drawing.Size(163, 17);
            this.labelCreating.TabIndex = 5;
            this.labelCreating.Text = "2 · Creating image # of #";
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(296, 148);
            this.ControlBox = false;
            this.Controls.Add(this.labelCreating);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelWaiting);
            this.Controls.Add(this.labelPrinting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Progress";
            this.Text = "Escher ̣̤̤· Print Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPrinting;
        private System.Windows.Forms.Label labelWaiting;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelCreating;
    }
}
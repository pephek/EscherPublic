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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.design = new System.Windows.Forms.RichTextBox();
            this.error = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // design
            // 
            this.design.Dock = System.Windows.Forms.DockStyle.Fill;
            this.design.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.design.Location = new System.Drawing.Point(0, 0);
            this.design.Name = "design";
            this.design.Size = new System.Drawing.Size(800, 450);
            this.design.TabIndex = 0;
            this.design.Text = "";
            // 
            // error
            // 
            this.error.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.error.Location = new System.Drawing.Point(0, 428);
            this.error.Name = "error";
            this.error.Size = new System.Drawing.Size(800, 22);
            this.error.TabIndex = 1;
            this.error.Text = "statusStrip1";
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.error);
            this.Controls.Add(this.design);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Editor";
            this.Text = "Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox design;
        private System.Windows.Forms.StatusStrip error;
    }
}
namespace Escher
{
    partial class Preview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preview));
            this.labelCaption = new System.Windows.Forms.Label();
            this.labelLegend = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Location = new System.Drawing.Point(75, 9);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(113, 17);
            this.labelCaption.TabIndex = 0;
            this.labelCaption.Text = "Escher · Preview";
            // 
            // labelLegend
            // 
            this.labelLegend.AutoSize = true;
            this.labelLegend.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLegend.ForeColor = System.Drawing.Color.Gray;
            this.labelLegend.Location = new System.Drawing.Point(75, 139);
            this.labelLegend.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLegend.Name = "labelLegend";
            this.labelLegend.Size = new System.Drawing.Size(104, 187);
            this.labelLegend.TabIndex = 1;
            this.labelLegend.Text = "c = ± color\r\nn = ± number\r\nv = ± value\r\nf = ± frame\r\nt = ± title\r\ns = ± font\r\n\r\np" +
    " = setup\r\n\r\n- = previous\r\n+ = next";
            // 
            // Preview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.labelLegend);
            this.Controls.Add(this.labelCaption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Preview";
            this.Text = "Escher · Preview";
            this.Load += new System.EventHandler(this.Preview_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Preview_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Preview_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Label labelLegend;
    }
}
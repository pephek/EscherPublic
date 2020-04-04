namespace Escher
{
    partial class Print
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Print));
            this.groupBoxFormat = new System.Windows.Forms.GroupBox();
            this.checkBoxIncludeMarginForPunchHoles = new System.Windows.Forms.CheckBox();
            this.comboBoxFormat = new System.Windows.Forms.ComboBox();
            this.groupBoxStampOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxIncludeValue = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeNumber = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeImage = new System.Windows.Forms.CheckBox();
            this.groupBoxFrameOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonThinDotted = new System.Windows.Forms.RadioButton();
            this.radioButtonThick = new System.Windows.Forms.RadioButton();
            this.radioButtonThinSolid = new System.Windows.Forms.RadioButton();
            this.groupBoxColorOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonColor = new System.Windows.Forms.RadioButton();
            this.radioButtonGreyscale = new System.Windows.Forms.RadioButton();
            this.groupBoxPageOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxIncludeTitle = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeBorder = new System.Windows.Forms.CheckBox();
            this.groupBoxCatalogOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxAppendCatalog = new System.Windows.Forms.CheckBox();
            this.comboBoxCatalog = new System.Windows.Forms.ComboBox();
            this.groupBoxFontOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonLarge = new System.Windows.Forms.RadioButton();
            this.radioButtonMedium = new System.Windows.Forms.RadioButton();
            this.radioButtonSmall = new System.Windows.Forms.RadioButton();
            this.groupBoxAlbumOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxIncludePdfBookmarks = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeHtmlScans = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeSamplePagesOnly = new System.Windows.Forms.CheckBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxFormat.SuspendLayout();
            this.groupBoxStampOptions.SuspendLayout();
            this.groupBoxFrameOptions.SuspendLayout();
            this.groupBoxColorOptions.SuspendLayout();
            this.groupBoxPageOptions.SuspendLayout();
            this.groupBoxCatalogOptions.SuspendLayout();
            this.groupBoxFontOptions.SuspendLayout();
            this.groupBoxAlbumOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFormat
            // 
            this.groupBoxFormat.Controls.Add(this.checkBoxIncludeMarginForPunchHoles);
            this.groupBoxFormat.Controls.Add(this.comboBoxFormat);
            this.groupBoxFormat.Location = new System.Drawing.Point(24, 174);
            this.groupBoxFormat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxFormat.Name = "groupBoxFormat";
            this.groupBoxFormat.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxFormat.Size = new System.Drawing.Size(396, 121);
            this.groupBoxFormat.TabIndex = 0;
            this.groupBoxFormat.TabStop = false;
            this.groupBoxFormat.Text = "Format";
            // 
            // checkBoxIncludeMarginForPunchHoles
            // 
            this.checkBoxIncludeMarginForPunchHoles.AutoSize = true;
            this.checkBoxIncludeMarginForPunchHoles.Location = new System.Drawing.Point(19, 70);
            this.checkBoxIncludeMarginForPunchHoles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludeMarginForPunchHoles.Name = "checkBoxIncludeMarginForPunchHoles";
            this.checkBoxIncludeMarginForPunchHoles.Size = new System.Drawing.Size(227, 21);
            this.checkBoxIncludeMarginForPunchHoles.TabIndex = 1;
            this.checkBoxIncludeMarginForPunchHoles.Text = "Include Margin for Punch Holes";
            this.checkBoxIncludeMarginForPunchHoles.UseVisualStyleBackColor = true;
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Location = new System.Drawing.Point(19, 30);
            this.comboBoxFormat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(363, 24);
            this.comboBoxFormat.TabIndex = 0;
            // 
            // groupBoxStampOptions
            // 
            this.groupBoxStampOptions.Controls.Add(this.checkBoxIncludeValue);
            this.groupBoxStampOptions.Controls.Add(this.checkBoxIncludeNumber);
            this.groupBoxStampOptions.Controls.Add(this.checkBoxIncludeImage);
            this.groupBoxStampOptions.Location = new System.Drawing.Point(24, 26);
            this.groupBoxStampOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxStampOptions.Name = "groupBoxStampOptions";
            this.groupBoxStampOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxStampOptions.Size = new System.Drawing.Size(157, 127);
            this.groupBoxStampOptions.TabIndex = 1;
            this.groupBoxStampOptions.TabStop = false;
            this.groupBoxStampOptions.Text = "Stamp Options";
            // 
            // checkBoxIncludeValue
            // 
            this.checkBoxIncludeValue.AutoSize = true;
            this.checkBoxIncludeValue.Location = new System.Drawing.Point(19, 89);
            this.checkBoxIncludeValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludeValue.Name = "checkBoxIncludeValue";
            this.checkBoxIncludeValue.Size = new System.Drawing.Size(115, 21);
            this.checkBoxIncludeValue.TabIndex = 2;
            this.checkBoxIncludeValue.Text = "Include Value";
            this.checkBoxIncludeValue.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeNumber
            // 
            this.checkBoxIncludeNumber.AutoSize = true;
            this.checkBoxIncludeNumber.Location = new System.Drawing.Point(19, 62);
            this.checkBoxIncludeNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludeNumber.Name = "checkBoxIncludeNumber";
            this.checkBoxIncludeNumber.Size = new System.Drawing.Size(129, 21);
            this.checkBoxIncludeNumber.TabIndex = 1;
            this.checkBoxIncludeNumber.Text = "Include Number";
            this.checkBoxIncludeNumber.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeImage
            // 
            this.checkBoxIncludeImage.AutoSize = true;
            this.checkBoxIncludeImage.Location = new System.Drawing.Point(19, 34);
            this.checkBoxIncludeImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludeImage.Name = "checkBoxIncludeImage";
            this.checkBoxIncludeImage.Size = new System.Drawing.Size(117, 21);
            this.checkBoxIncludeImage.TabIndex = 0;
            this.checkBoxIncludeImage.Text = "Include Image";
            this.checkBoxIncludeImage.UseVisualStyleBackColor = true;
            // 
            // groupBoxFrameOptions
            // 
            this.groupBoxFrameOptions.Controls.Add(this.radioButtonThinDotted);
            this.groupBoxFrameOptions.Controls.Add(this.radioButtonThick);
            this.groupBoxFrameOptions.Controls.Add(this.radioButtonThinSolid);
            this.groupBoxFrameOptions.Location = new System.Drawing.Point(197, 30);
            this.groupBoxFrameOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxFrameOptions.Name = "groupBoxFrameOptions";
            this.groupBoxFrameOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxFrameOptions.Size = new System.Drawing.Size(140, 123);
            this.groupBoxFrameOptions.TabIndex = 2;
            this.groupBoxFrameOptions.TabStop = false;
            this.groupBoxFrameOptions.Text = "Frame Options";
            // 
            // radioButtonThinDotted
            // 
            this.radioButtonThinDotted.AutoSize = true;
            this.radioButtonThinDotted.Location = new System.Drawing.Point(19, 86);
            this.radioButtonThinDotted.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonThinDotted.Name = "radioButtonThinDotted";
            this.radioButtonThinDotted.Size = new System.Drawing.Size(103, 21);
            this.radioButtonThinDotted.TabIndex = 2;
            this.radioButtonThinDotted.TabStop = true;
            this.radioButtonThinDotted.Text = "Thin Dotted";
            this.radioButtonThinDotted.UseVisualStyleBackColor = true;
            // 
            // radioButtonThick
            // 
            this.radioButtonThick.AutoSize = true;
            this.radioButtonThick.Location = new System.Drawing.Point(19, 59);
            this.radioButtonThick.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonThick.Name = "radioButtonThick";
            this.radioButtonThick.Size = new System.Drawing.Size(63, 21);
            this.radioButtonThick.TabIndex = 1;
            this.radioButtonThick.TabStop = true;
            this.radioButtonThick.Text = "Thick";
            this.radioButtonThick.UseVisualStyleBackColor = true;
            // 
            // radioButtonThinSolid
            // 
            this.radioButtonThinSolid.AutoSize = true;
            this.radioButtonThinSolid.Location = new System.Drawing.Point(19, 32);
            this.radioButtonThinSolid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonThinSolid.Name = "radioButtonThinSolid";
            this.radioButtonThinSolid.Size = new System.Drawing.Size(92, 21);
            this.radioButtonThinSolid.TabIndex = 0;
            this.radioButtonThinSolid.TabStop = true;
            this.radioButtonThinSolid.Text = "Thin Solid";
            this.radioButtonThinSolid.UseVisualStyleBackColor = true;
            // 
            // groupBoxColorOptions
            // 
            this.groupBoxColorOptions.Controls.Add(this.radioButtonColor);
            this.groupBoxColorOptions.Controls.Add(this.radioButtonGreyscale);
            this.groupBoxColorOptions.Location = new System.Drawing.Point(355, 31);
            this.groupBoxColorOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxColorOptions.Name = "groupBoxColorOptions";
            this.groupBoxColorOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxColorOptions.Size = new System.Drawing.Size(133, 123);
            this.groupBoxColorOptions.TabIndex = 3;
            this.groupBoxColorOptions.TabStop = false;
            this.groupBoxColorOptions.Text = "Color Options";
            // 
            // radioButtonColor
            // 
            this.radioButtonColor.AutoSize = true;
            this.radioButtonColor.Location = new System.Drawing.Point(20, 59);
            this.radioButtonColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonColor.Name = "radioButtonColor";
            this.radioButtonColor.Size = new System.Drawing.Size(62, 21);
            this.radioButtonColor.TabIndex = 1;
            this.radioButtonColor.TabStop = true;
            this.radioButtonColor.Text = "Color";
            this.radioButtonColor.UseVisualStyleBackColor = true;
            // 
            // radioButtonGreyscale
            // 
            this.radioButtonGreyscale.AutoSize = true;
            this.radioButtonGreyscale.Location = new System.Drawing.Point(20, 30);
            this.radioButtonGreyscale.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonGreyscale.Name = "radioButtonGreyscale";
            this.radioButtonGreyscale.Size = new System.Drawing.Size(93, 21);
            this.radioButtonGreyscale.TabIndex = 0;
            this.radioButtonGreyscale.TabStop = true;
            this.radioButtonGreyscale.Text = "Greyscale";
            this.radioButtonGreyscale.UseVisualStyleBackColor = true;
            // 
            // groupBoxPageOptions
            // 
            this.groupBoxPageOptions.Controls.Add(this.checkBoxIncludeTitle);
            this.groupBoxPageOptions.Controls.Add(this.checkBoxIncludeBorder);
            this.groupBoxPageOptions.Location = new System.Drawing.Point(669, 31);
            this.groupBoxPageOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxPageOptions.Name = "groupBoxPageOptions";
            this.groupBoxPageOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxPageOptions.Size = new System.Drawing.Size(164, 121);
            this.groupBoxPageOptions.TabIndex = 4;
            this.groupBoxPageOptions.TabStop = false;
            this.groupBoxPageOptions.Text = "Page Options";
            // 
            // checkBoxIncludeTitle
            // 
            this.checkBoxIncludeTitle.AutoSize = true;
            this.checkBoxIncludeTitle.Location = new System.Drawing.Point(13, 58);
            this.checkBoxIncludeTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludeTitle.Name = "checkBoxIncludeTitle";
            this.checkBoxIncludeTitle.Size = new System.Drawing.Size(106, 21);
            this.checkBoxIncludeTitle.TabIndex = 1;
            this.checkBoxIncludeTitle.Text = "Include Title";
            this.checkBoxIncludeTitle.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeBorder
            // 
            this.checkBoxIncludeBorder.AutoSize = true;
            this.checkBoxIncludeBorder.Location = new System.Drawing.Point(13, 31);
            this.checkBoxIncludeBorder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludeBorder.Name = "checkBoxIncludeBorder";
            this.checkBoxIncludeBorder.Size = new System.Drawing.Size(122, 21);
            this.checkBoxIncludeBorder.TabIndex = 0;
            this.checkBoxIncludeBorder.Text = "Include Border";
            this.checkBoxIncludeBorder.UseVisualStyleBackColor = true;
            // 
            // groupBoxCatalogOptions
            // 
            this.groupBoxCatalogOptions.Controls.Add(this.checkBoxAppendCatalog);
            this.groupBoxCatalogOptions.Controls.Add(this.comboBoxCatalog);
            this.groupBoxCatalogOptions.Location = new System.Drawing.Point(851, 32);
            this.groupBoxCatalogOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxCatalogOptions.Name = "groupBoxCatalogOptions";
            this.groupBoxCatalogOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxCatalogOptions.Size = new System.Drawing.Size(221, 121);
            this.groupBoxCatalogOptions.TabIndex = 5;
            this.groupBoxCatalogOptions.TabStop = false;
            this.groupBoxCatalogOptions.Text = "Catalog Options";
            // 
            // checkBoxAppendCatalog
            // 
            this.checkBoxAppendCatalog.AutoSize = true;
            this.checkBoxAppendCatalog.Location = new System.Drawing.Point(19, 70);
            this.checkBoxAppendCatalog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxAppendCatalog.Name = "checkBoxAppendCatalog";
            this.checkBoxAppendCatalog.Size = new System.Drawing.Size(131, 21);
            this.checkBoxAppendCatalog.TabIndex = 1;
            this.checkBoxAppendCatalog.Text = "Append Catalog";
            this.checkBoxAppendCatalog.UseVisualStyleBackColor = true;
            // 
            // comboBoxCatalog
            // 
            this.comboBoxCatalog.FormattingEnabled = true;
            this.comboBoxCatalog.Location = new System.Drawing.Point(19, 30);
            this.comboBoxCatalog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxCatalog.Name = "comboBoxCatalog";
            this.comboBoxCatalog.Size = new System.Drawing.Size(180, 24);
            this.comboBoxCatalog.TabIndex = 0;
            // 
            // groupBoxFontOptions
            // 
            this.groupBoxFontOptions.Controls.Add(this.radioButtonLarge);
            this.groupBoxFontOptions.Controls.Add(this.radioButtonMedium);
            this.groupBoxFontOptions.Controls.Add(this.radioButtonSmall);
            this.groupBoxFontOptions.Location = new System.Drawing.Point(513, 31);
            this.groupBoxFontOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxFontOptions.Name = "groupBoxFontOptions";
            this.groupBoxFontOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxFontOptions.Size = new System.Drawing.Size(137, 124);
            this.groupBoxFontOptions.TabIndex = 6;
            this.groupBoxFontOptions.TabStop = false;
            this.groupBoxFontOptions.Text = "Font Options";
            // 
            // radioButtonLarge
            // 
            this.radioButtonLarge.AutoSize = true;
            this.radioButtonLarge.Location = new System.Drawing.Point(21, 86);
            this.radioButtonLarge.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonLarge.Name = "radioButtonLarge";
            this.radioButtonLarge.Size = new System.Drawing.Size(88, 21);
            this.radioButtonLarge.TabIndex = 2;
            this.radioButtonLarge.TabStop = true;
            this.radioButtonLarge.Text = "Large (7)";
            this.radioButtonLarge.UseVisualStyleBackColor = true;
            // 
            // radioButtonMedium
            // 
            this.radioButtonMedium.AutoSize = true;
            this.radioButtonMedium.Location = new System.Drawing.Point(21, 59);
            this.radioButtonMedium.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonMedium.Name = "radioButtonMedium";
            this.radioButtonMedium.Size = new System.Drawing.Size(100, 21);
            this.radioButtonMedium.TabIndex = 1;
            this.radioButtonMedium.TabStop = true;
            this.radioButtonMedium.Text = "Medium (6)";
            this.radioButtonMedium.UseVisualStyleBackColor = true;
            // 
            // radioButtonSmall
            // 
            this.radioButtonSmall.AutoSize = true;
            this.radioButtonSmall.Location = new System.Drawing.Point(21, 32);
            this.radioButtonSmall.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonSmall.Name = "radioButtonSmall";
            this.radioButtonSmall.Size = new System.Drawing.Size(85, 21);
            this.radioButtonSmall.TabIndex = 0;
            this.radioButtonSmall.TabStop = true;
            this.radioButtonSmall.Text = "Small (5)";
            this.radioButtonSmall.UseVisualStyleBackColor = true;
            // 
            // groupBoxAlbumOptions
            // 
            this.groupBoxAlbumOptions.Controls.Add(this.checkBoxIncludePdfBookmarks);
            this.groupBoxAlbumOptions.Controls.Add(this.checkBoxIncludeHtmlScans);
            this.groupBoxAlbumOptions.Controls.Add(this.checkBoxIncludeSamplePagesOnly);
            this.groupBoxAlbumOptions.Location = new System.Drawing.Point(440, 174);
            this.groupBoxAlbumOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxAlbumOptions.Name = "groupBoxAlbumOptions";
            this.groupBoxAlbumOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxAlbumOptions.Size = new System.Drawing.Size(251, 121);
            this.groupBoxAlbumOptions.TabIndex = 7;
            this.groupBoxAlbumOptions.TabStop = false;
            this.groupBoxAlbumOptions.Text = "Album Options";
            // 
            // checkBoxIncludePdfBookmarks
            // 
            this.checkBoxIncludePdfBookmarks.AutoSize = true;
            this.checkBoxIncludePdfBookmarks.Location = new System.Drawing.Point(17, 85);
            this.checkBoxIncludePdfBookmarks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludePdfBookmarks.Name = "checkBoxIncludePdfBookmarks";
            this.checkBoxIncludePdfBookmarks.Size = new System.Drawing.Size(174, 21);
            this.checkBoxIncludePdfBookmarks.TabIndex = 2;
            this.checkBoxIncludePdfBookmarks.Text = "Include Pdf Bookmarks";
            this.checkBoxIncludePdfBookmarks.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeHtmlScans
            // 
            this.checkBoxIncludeHtmlScans.AutoSize = true;
            this.checkBoxIncludeHtmlScans.Location = new System.Drawing.Point(17, 58);
            this.checkBoxIncludeHtmlScans.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludeHtmlScans.Name = "checkBoxIncludeHtmlScans";
            this.checkBoxIncludeHtmlScans.Size = new System.Drawing.Size(150, 21);
            this.checkBoxIncludeHtmlScans.TabIndex = 1;
            this.checkBoxIncludeHtmlScans.Text = "Include Html Scans";
            this.checkBoxIncludeHtmlScans.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeSamplePagesOnly
            // 
            this.checkBoxIncludeSamplePagesOnly.AutoSize = true;
            this.checkBoxIncludeSamplePagesOnly.Location = new System.Drawing.Point(17, 30);
            this.checkBoxIncludeSamplePagesOnly.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxIncludeSamplePagesOnly.Name = "checkBoxIncludeSamplePagesOnly";
            this.checkBoxIncludeSamplePagesOnly.Size = new System.Drawing.Size(203, 21);
            this.checkBoxIncludeSamplePagesOnly.TabIndex = 0;
            this.checkBoxIncludeSamplePagesOnly.Text = "Include Sample Pages Only";
            this.checkBoxIncludeSamplePagesOnly.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(959, 242);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(113, 50);
            this.buttonOk.TabIndex = 8;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(840, 242);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(113, 50);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // Print
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(1093, 321);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxAlbumOptions);
            this.Controls.Add(this.groupBoxFontOptions);
            this.Controls.Add(this.groupBoxCatalogOptions);
            this.Controls.Add(this.groupBoxPageOptions);
            this.Controls.Add(this.groupBoxColorOptions);
            this.Controls.Add(this.groupBoxFrameOptions);
            this.Controls.Add(this.groupBoxStampOptions);
            this.Controls.Add(this.groupBoxFormat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Print";
            this.Text = "Escher - Preview";
            this.Load += new System.EventHandler(this.Print_Load);
            this.Shown += new System.EventHandler(this.Print_Shown);
            this.groupBoxFormat.ResumeLayout(false);
            this.groupBoxFormat.PerformLayout();
            this.groupBoxStampOptions.ResumeLayout(false);
            this.groupBoxStampOptions.PerformLayout();
            this.groupBoxFrameOptions.ResumeLayout(false);
            this.groupBoxFrameOptions.PerformLayout();
            this.groupBoxColorOptions.ResumeLayout(false);
            this.groupBoxColorOptions.PerformLayout();
            this.groupBoxPageOptions.ResumeLayout(false);
            this.groupBoxPageOptions.PerformLayout();
            this.groupBoxCatalogOptions.ResumeLayout(false);
            this.groupBoxCatalogOptions.PerformLayout();
            this.groupBoxFontOptions.ResumeLayout(false);
            this.groupBoxFontOptions.PerformLayout();
            this.groupBoxAlbumOptions.ResumeLayout(false);
            this.groupBoxAlbumOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFormat;
        private System.Windows.Forms.ComboBox comboBoxFormat;
        private System.Windows.Forms.CheckBox checkBoxIncludeMarginForPunchHoles;
        private System.Windows.Forms.GroupBox groupBoxStampOptions;
        private System.Windows.Forms.CheckBox checkBoxIncludeValue;
        private System.Windows.Forms.CheckBox checkBoxIncludeNumber;
        private System.Windows.Forms.CheckBox checkBoxIncludeImage;
        private System.Windows.Forms.GroupBox groupBoxFrameOptions;
        private System.Windows.Forms.RadioButton radioButtonThinDotted;
        private System.Windows.Forms.RadioButton radioButtonThick;
        private System.Windows.Forms.RadioButton radioButtonThinSolid;
        private System.Windows.Forms.GroupBox groupBoxColorOptions;
        private System.Windows.Forms.RadioButton radioButtonColor;
        private System.Windows.Forms.RadioButton radioButtonGreyscale;
        private System.Windows.Forms.GroupBox groupBoxPageOptions;
        private System.Windows.Forms.CheckBox checkBoxIncludeTitle;
        private System.Windows.Forms.CheckBox checkBoxIncludeBorder;
        private System.Windows.Forms.GroupBox groupBoxCatalogOptions;
        private System.Windows.Forms.CheckBox checkBoxAppendCatalog;
        private System.Windows.Forms.ComboBox comboBoxCatalog;
        private System.Windows.Forms.GroupBox groupBoxFontOptions;
        private System.Windows.Forms.RadioButton radioButtonLarge;
        private System.Windows.Forms.RadioButton radioButtonMedium;
        private System.Windows.Forms.RadioButton radioButtonSmall;
        private System.Windows.Forms.GroupBox groupBoxAlbumOptions;
        private System.Windows.Forms.CheckBox checkBoxIncludePdfBookmarks;
        private System.Windows.Forms.CheckBox checkBoxIncludeHtmlScans;
        private System.Windows.Forms.CheckBox checkBoxIncludeSamplePagesOnly;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}
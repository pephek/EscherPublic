﻿namespace Escher
{
    partial class Imaging
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Imaging));
            this.panelImaging = new System.Windows.Forms.Panel();
            this.brightness = new System.Windows.Forms.NumericUpDown();
            this.angle = new System.Windows.Forms.NumericUpDown();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.labelMode = new System.Windows.Forms.Label();
            this.toolTipRotate = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipCrop = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipBrighten = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipSelect = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipThumbnail = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipResize = new System.Windows.Forms.ToolTip(this.components);
            this.pTrial = new System.Windows.Forms.PictureBox();
            this.buttonResize = new System.Windows.Forms.Button();
            this.buttonThumbnail = new System.Windows.Forms.Button();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.buttonCrop = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonBrighten = new System.Windows.Forms.Button();
            this.buttonRotate = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonReject = new System.Windows.Forms.Button();
            this.pPrint = new System.Windows.Forms.PictureBox();
            this.pColor = new System.Windows.Forms.PictureBox();
            this.pThumb = new System.Windows.Forms.PictureBox();
            this.pImage = new System.Windows.Forms.PictureBox();
            this.resize = new System.Windows.Forms.NumericUpDown();
            this.panelImaging.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.angle)).BeginInit();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pTrial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pThumb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resize)).BeginInit();
            this.SuspendLayout();
            // 
            // panelImaging
            // 
            this.panelImaging.BackColor = System.Drawing.Color.Black;
            this.panelImaging.Controls.Add(this.resize);
            this.panelImaging.Controls.Add(this.brightness);
            this.panelImaging.Controls.Add(this.angle);
            this.panelImaging.Controls.Add(this.buttonAccept);
            this.panelImaging.Controls.Add(this.buttonReject);
            this.panelImaging.Location = new System.Drawing.Point(420, 287);
            this.panelImaging.Name = "panelImaging";
            this.panelImaging.Size = new System.Drawing.Size(151, 39);
            this.panelImaging.TabIndex = 10;
            // 
            // brightness
            // 
            this.brightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brightness.Location = new System.Drawing.Point(73, 4);
            this.brightness.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.brightness.Name = "brightness";
            this.brightness.Size = new System.Drawing.Size(74, 30);
            this.brightness.TabIndex = 3;
            // 
            // angle
            // 
            this.angle.DecimalPlaces = 1;
            this.angle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.angle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.angle.Location = new System.Drawing.Point(73, 4);
            this.angle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.angle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.angle.Name = "angle";
            this.angle.Size = new System.Drawing.Size(74, 30);
            this.angle.TabIndex = 2;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Black;
            this.panelButtons.Controls.Add(this.buttonResize);
            this.panelButtons.Controls.Add(this.buttonThumbnail);
            this.panelButtons.Controls.Add(this.buttonUndo);
            this.panelButtons.Controls.Add(this.buttonCrop);
            this.panelButtons.Controls.Add(this.buttonZoomOut);
            this.panelButtons.Controls.Add(this.buttonZoomIn);
            this.panelButtons.Controls.Add(this.buttonBrighten);
            this.panelButtons.Controls.Add(this.buttonRotate);
            this.panelButtons.Controls.Add(this.buttonSelect);
            this.panelButtons.Controls.Add(this.buttonClose);
            this.panelButtons.Controls.Add(this.buttonSave);
            this.panelButtons.Controls.Add(this.buttonPrevious);
            this.panelButtons.Controls.Add(this.buttonNext);
            this.panelButtons.Location = new System.Drawing.Point(240, 44);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(455, 39);
            this.panelButtons.TabIndex = 11;
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.BackColor = System.Drawing.Color.Black;
            this.labelMode.ForeColor = System.Drawing.Color.White;
            this.labelMode.Location = new System.Drawing.Point(3, 3);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(73, 17);
            this.labelMode.TabIndex = 12;
            this.labelMode.Text = "labelMode";
            // 
            // toolTipRotate
            // 
            this.toolTipRotate.IsBalloon = true;
            this.toolTipRotate.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipRotate.ToolTipTitle = "Rotate";
            // 
            // toolTipCrop
            // 
            this.toolTipCrop.IsBalloon = true;
            this.toolTipCrop.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipCrop.ToolTipTitle = "Crop";
            // 
            // toolTipBrighten
            // 
            this.toolTipBrighten.IsBalloon = true;
            this.toolTipBrighten.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipBrighten.ToolTipTitle = "Brighten";
            // 
            // toolTipSelect
            // 
            this.toolTipSelect.IsBalloon = true;
            this.toolTipSelect.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipSelect.ToolTipTitle = "Brighten";
            // 
            // toolTipThumbnail
            // 
            this.toolTipThumbnail.IsBalloon = true;
            this.toolTipThumbnail.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipThumbnail.ToolTipTitle = "Thumbnail";
            // 
            // toolTipResize
            // 
            this.toolTipResize.IsBalloon = true;
            this.toolTipResize.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipResize.ToolTipTitle = "Resize";
            // 
            // pTrial
            // 
            this.pTrial.BackColor = System.Drawing.Color.Black;
            this.pTrial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pTrial.Location = new System.Drawing.Point(214, 235);
            this.pTrial.Name = "pTrial";
            this.pTrial.Size = new System.Drawing.Size(135, 112);
            this.pTrial.TabIndex = 13;
            this.pTrial.TabStop = false;
            this.pTrial.Visible = false;
            // 
            // buttonResize
            // 
            this.buttonResize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonResize.Image = global::Escher.Properties.Resources.Resize_16x;
            this.buttonResize.Location = new System.Drawing.Point(248, 3);
            this.buttonResize.Name = "buttonResize";
            this.buttonResize.Size = new System.Drawing.Size(32, 32);
            this.buttonResize.TabIndex = 17;
            this.buttonResize.TabStop = false;
            this.toolTipResize.SetToolTip(this.buttonResize, "Resize the image between 50% and 100%");
            this.buttonResize.UseVisualStyleBackColor = true;
            // 
            // buttonThumbnail
            // 
            this.buttonThumbnail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonThumbnail.Image = global::Escher.Properties.Resources.IconFile_16x;
            this.buttonThumbnail.Location = new System.Drawing.Point(314, 3);
            this.buttonThumbnail.Name = "buttonThumbnail";
            this.buttonThumbnail.Size = new System.Drawing.Size(32, 32);
            this.buttonThumbnail.TabIndex = 16;
            this.buttonThumbnail.TabStop = false;
            this.toolTipThumbnail.SetToolTip(this.buttonThumbnail, "(Re)create the thumbnail of the image");
            this.buttonThumbnail.UseVisualStyleBackColor = true;
            // 
            // buttonUndo
            // 
            this.buttonUndo.Image = global::Escher.Properties.Resources.Undo_16x;
            this.buttonUndo.Location = new System.Drawing.Point(387, 3);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(32, 32);
            this.buttonUndo.TabIndex = 15;
            this.buttonUndo.TabStop = false;
            this.buttonUndo.UseVisualStyleBackColor = true;
            // 
            // buttonCrop
            // 
            this.buttonCrop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCrop.Image = global::Escher.Properties.Resources.ImageCrop_16x;
            this.buttonCrop.Location = new System.Drawing.Point(182, 3);
            this.buttonCrop.Name = "buttonCrop";
            this.buttonCrop.Size = new System.Drawing.Size(32, 32);
            this.buttonCrop.TabIndex = 14;
            this.buttonCrop.TabStop = false;
            this.toolTipCrop.SetToolTip(this.buttonCrop, "Crop the image to remove too much black space around the borders");
            this.buttonCrop.UseVisualStyleBackColor = true;
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Enabled = false;
            this.buttonZoomOut.Image = global::Escher.Properties.Resources.ZoomOut_16x;
            this.buttonZoomOut.Location = new System.Drawing.Point(109, 3);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(32, 32);
            this.buttonZoomOut.TabIndex = 13;
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Enabled = false;
            this.buttonZoomIn.Image = global::Escher.Properties.Resources.ZoomIn_16x1;
            this.buttonZoomIn.Location = new System.Drawing.Point(76, 3);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(32, 32);
            this.buttonZoomIn.TabIndex = 12;
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            // 
            // buttonBrighten
            // 
            this.buttonBrighten.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBrighten.Image = global::Escher.Properties.Resources.Brightness_16x;
            this.buttonBrighten.Location = new System.Drawing.Point(215, 3);
            this.buttonBrighten.Name = "buttonBrighten";
            this.buttonBrighten.Size = new System.Drawing.Size(32, 32);
            this.buttonBrighten.TabIndex = 11;
            this.buttonBrighten.TabStop = false;
            this.toolTipBrighten.SetToolTip(this.buttonBrighten, "Adjust the brightness between -100 and +100");
            this.buttonBrighten.UseVisualStyleBackColor = true;
            // 
            // buttonRotate
            // 
            this.buttonRotate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRotate.Image = global::Escher.Properties.Resources.AdRotator_16x;
            this.buttonRotate.Location = new System.Drawing.Point(149, 3);
            this.buttonRotate.Name = "buttonRotate";
            this.buttonRotate.Size = new System.Drawing.Size(32, 32);
            this.buttonRotate.TabIndex = 10;
            this.buttonRotate.TabStop = false;
            this.toolTipRotate.SetToolTip(this.buttonRotate, "Rotate the image between -180° and +180°");
            this.buttonRotate.UseVisualStyleBackColor = true;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSelect.Image = global::Escher.Properties.Resources.RectangularSelection_16x;
            this.buttonSelect.Location = new System.Drawing.Point(281, 3);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(32, 32);
            this.buttonSelect.TabIndex = 8;
            this.buttonSelect.TabStop = false;
            this.toolTipSelect.SetToolTip(this.buttonSelect, "Select the vignette (and frame) of the stamp");
            this.buttonSelect.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Image = global::Escher.Properties.Resources.Close_red_16x;
            this.buttonClose.Location = new System.Drawing.Point(420, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(32, 32);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.TabStop = false;
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Image = global::Escher.Properties.Resources.Save_16x;
            this.buttonSave.Location = new System.Drawing.Point(354, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(32, 32);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.TabStop = false;
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrevious.Image = global::Escher.Properties.Resources.Previous_16x;
            this.buttonPrevious.Location = new System.Drawing.Point(3, 3);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(32, 32);
            this.buttonPrevious.TabIndex = 3;
            this.buttonPrevious.TabStop = false;
            this.buttonPrevious.UseVisualStyleBackColor = true;
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
            // 
            // buttonAccept
            // 
            this.buttonAccept.Image = global::Escher.Properties.Resources.Checkmark_16x;
            this.buttonAccept.Location = new System.Drawing.Point(3, 3);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(32, 32);
            this.buttonAccept.TabIndex = 1;
            this.buttonAccept.UseVisualStyleBackColor = true;
            // 
            // buttonReject
            // 
            this.buttonReject.Image = global::Escher.Properties.Resources.Close_red_16x;
            this.buttonReject.Location = new System.Drawing.Point(36, 3);
            this.buttonReject.Name = "buttonReject";
            this.buttonReject.Size = new System.Drawing.Size(32, 32);
            this.buttonReject.TabIndex = 0;
            this.buttonReject.UseVisualStyleBackColor = true;
            // 
            // pPrint
            // 
            this.pPrint.BackColor = System.Drawing.Color.Black;
            this.pPrint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPrint.Location = new System.Drawing.Point(194, 207);
            this.pPrint.Name = "pPrint";
            this.pPrint.Size = new System.Drawing.Size(135, 112);
            this.pPrint.TabIndex = 9;
            this.pPrint.TabStop = false;
            // 
            // pColor
            // 
            this.pColor.BackColor = System.Drawing.Color.Black;
            this.pColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pColor.Location = new System.Drawing.Point(155, 186);
            this.pColor.Name = "pColor";
            this.pColor.Size = new System.Drawing.Size(135, 112);
            this.pColor.TabIndex = 8;
            this.pColor.TabStop = false;
            // 
            // pThumb
            // 
            this.pThumb.BackColor = System.Drawing.Color.Black;
            this.pThumb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pThumb.Location = new System.Drawing.Point(114, 150);
            this.pThumb.Name = "pThumb";
            this.pThumb.Size = new System.Drawing.Size(135, 112);
            this.pThumb.TabIndex = 7;
            this.pThumb.TabStop = false;
            // 
            // pImage
            // 
            this.pImage.BackColor = System.Drawing.Color.Black;
            this.pImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pImage.Location = new System.Drawing.Point(69, 113);
            this.pImage.Name = "pImage";
            this.pImage.Size = new System.Drawing.Size(135, 112);
            this.pImage.TabIndex = 6;
            this.pImage.TabStop = false;
            // 
            // resize
            // 
            this.resize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resize.Location = new System.Drawing.Point(73, 4);
            this.resize.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.resize.Name = "resize";
            this.resize.Size = new System.Drawing.Size(74, 30);
            this.resize.TabIndex = 4;
            this.resize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // Imaging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(926, 450);
            this.Controls.Add(this.pTrial);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelImaging);
            this.Controls.Add(this.pPrint);
            this.Controls.Add(this.pColor);
            this.Controls.Add(this.pThumb);
            this.Controls.Add(this.pImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Imaging";
            this.Text = "Imaging";
            this.panelImaging.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.brightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.angle)).EndInit();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pTrial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pThumb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pImage;
        private System.Windows.Forms.PictureBox pThumb;
        private System.Windows.Forms.PictureBox pColor;
        private System.Windows.Forms.PictureBox pPrint;
        private System.Windows.Forms.Panel panelImaging;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonReject;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonBrighten;
        private System.Windows.Forms.Button buttonRotate;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonCrop;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.NumericUpDown angle;
        private System.Windows.Forms.PictureBox pTrial;
        private System.Windows.Forms.NumericUpDown brightness;
        private System.Windows.Forms.Button buttonThumbnail;
        private System.Windows.Forms.ToolTip toolTipRotate;
        private System.Windows.Forms.ToolTip toolTipCrop;
        private System.Windows.Forms.ToolTip toolTipBrighten;
        private System.Windows.Forms.ToolTip toolTipSelect;
        private System.Windows.Forms.ToolTip toolTipThumbnail;
        private System.Windows.Forms.ToolTip toolTipResize;
        private System.Windows.Forms.Button buttonResize;
        private System.Windows.Forms.NumericUpDown resize;
    }
}
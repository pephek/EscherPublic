namespace Escher
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
            this.resize = new System.Windows.Forms.NumericUpDown();
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
            this.r = new ColorSlider.ColorSlider();
            this.g = new ColorSlider.ColorSlider();
            this.b = new ColorSlider.ColorSlider();
            this.toolTipRecolor = new System.Windows.Forms.ToolTip(this.components);
            this.panelRecolor = new System.Windows.Forms.Panel();
            this.toolTipRerunRecolor = new System.Windows.Forms.ToolTip(this.components);
            this.labelRecolor = new System.Windows.Forms.Label();
            this.toolTipBlacken = new System.Windows.Forms.ToolTip(this.components);
            this.blacken = new System.Windows.Forms.NumericUpDown();
            this.toolTipMeasure = new System.Windows.Forms.ToolTip(this.components);
            this.buttonRerunRecolor = new System.Windows.Forms.Button();
            this.pTrial = new System.Windows.Forms.PictureBox();
            this.buttonMeasure = new System.Windows.Forms.Button();
            this.buttonBlacken = new System.Windows.Forms.Button();
            this.buttonRecolor = new System.Windows.Forms.Button();
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
            this.measure = new System.Windows.Forms.NumericUpDown();
            this.panelImaging.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.angle)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelRecolor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blacken)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTrial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pThumb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measure)).BeginInit();
            this.SuspendLayout();
            // 
            // panelImaging
            // 
            this.panelImaging.BackColor = System.Drawing.Color.Black;
            this.panelImaging.Controls.Add(this.measure);
            this.panelImaging.Controls.Add(this.blacken);
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
            this.panelButtons.Controls.Add(this.buttonMeasure);
            this.panelButtons.Controls.Add(this.buttonBlacken);
            this.panelButtons.Controls.Add(this.buttonRecolor);
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
            this.panelButtons.Size = new System.Drawing.Size(555, 39);
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
            this.toolTipSelect.ToolTipTitle = "Select";
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
            // r
            // 
            this.r.BackColor = System.Drawing.Color.Black;
            this.r.BarPenColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(94)))), ((int)(((byte)(110)))));
            this.r.BarPenColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(60)))), ((int)(((byte)(74)))));
            this.r.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.r.ColorSchema = ColorSlider.ColorSlider.ColorSchemas.RedColors;
            this.r.ElapsedInnerColor = System.Drawing.Color.Black;
            this.r.ElapsedPenColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(94)))), ((int)(((byte)(110)))));
            this.r.ElapsedPenColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(60)))), ((int)(((byte)(74)))));
            this.r.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.r.ForeColor = System.Drawing.Color.White;
            this.r.LargeChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.r.Location = new System.Drawing.Point(4, 42);
            this.r.Margin = new System.Windows.Forms.Padding(4);
            this.r.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.r.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            -2147483648});
            this.r.Name = "r";
            this.r.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.r.ScaleSubDivisions = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.r.ShowDivisionsText = true;
            this.r.ShowSmallScale = true;
            this.r.Size = new System.Drawing.Size(760, 59);
            this.r.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.r.TabIndex = 15;
            this.r.Text = "Red";
            this.r.ThumbInnerColor = System.Drawing.Color.Red;
            this.r.ThumbPenColor = System.Drawing.Color.Red;
            this.r.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.r.ThumbSize = new System.Drawing.Size(16, 16);
            this.r.TickAdd = 0F;
            this.r.TickColor = System.Drawing.Color.White;
            this.r.TickDivide = 0F;
            this.r.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // g
            // 
            this.g.BackColor = System.Drawing.Color.Black;
            this.g.BarPenColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(94)))), ((int)(((byte)(110)))));
            this.g.BarPenColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(60)))), ((int)(((byte)(74)))));
            this.g.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.g.ColorSchema = ColorSlider.ColorSlider.ColorSchemas.RedColors;
            this.g.ElapsedInnerColor = System.Drawing.Color.Black;
            this.g.ElapsedPenColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(94)))), ((int)(((byte)(110)))));
            this.g.ElapsedPenColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(60)))), ((int)(((byte)(74)))));
            this.g.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.g.ForeColor = System.Drawing.Color.White;
            this.g.LargeChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.g.Location = new System.Drawing.Point(4, 88);
            this.g.Margin = new System.Windows.Forms.Padding(4);
            this.g.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.g.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            -2147483648});
            this.g.Name = "g";
            this.g.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.g.ScaleSubDivisions = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.g.ShowDivisionsText = true;
            this.g.ShowSmallScale = true;
            this.g.Size = new System.Drawing.Size(760, 59);
            this.g.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.g.TabIndex = 16;
            this.g.Text = "Red";
            this.g.ThumbInnerColor = System.Drawing.Color.Green;
            this.g.ThumbPenColor = System.Drawing.Color.Green;
            this.g.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.g.ThumbSize = new System.Drawing.Size(16, 16);
            this.g.TickAdd = 0F;
            this.g.TickColor = System.Drawing.Color.White;
            this.g.TickDivide = 0F;
            this.g.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // b
            // 
            this.b.BackColor = System.Drawing.Color.Black;
            this.b.BarPenColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(94)))), ((int)(((byte)(110)))));
            this.b.BarPenColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(60)))), ((int)(((byte)(74)))));
            this.b.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.b.ColorSchema = ColorSlider.ColorSlider.ColorSchemas.RedColors;
            this.b.ElapsedInnerColor = System.Drawing.Color.Black;
            this.b.ElapsedPenColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(94)))), ((int)(((byte)(110)))));
            this.b.ElapsedPenColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(60)))), ((int)(((byte)(74)))));
            this.b.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.b.ForeColor = System.Drawing.Color.White;
            this.b.LargeChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.b.Location = new System.Drawing.Point(4, 134);
            this.b.Margin = new System.Windows.Forms.Padding(4);
            this.b.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.b.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            -2147483648});
            this.b.Name = "b";
            this.b.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.b.ScaleSubDivisions = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.b.ShowDivisionsText = true;
            this.b.ShowSmallScale = true;
            this.b.Size = new System.Drawing.Size(760, 59);
            this.b.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.b.TabIndex = 17;
            this.b.Text = "Red";
            this.b.ThumbInnerColor = System.Drawing.Color.Blue;
            this.b.ThumbPenColor = System.Drawing.Color.Blue;
            this.b.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.b.ThumbSize = new System.Drawing.Size(16, 16);
            this.b.TickAdd = 0F;
            this.b.TickColor = System.Drawing.Color.White;
            this.b.TickDivide = 0F;
            this.b.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // toolTipRecolor
            // 
            this.toolTipRecolor.IsBalloon = true;
            this.toolTipRecolor.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipRecolor.ToolTipTitle = "Adjust";
            // 
            // panelRecolor
            // 
            this.panelRecolor.BackColor = System.Drawing.Color.Black;
            this.panelRecolor.Controls.Add(this.labelRecolor);
            this.panelRecolor.Controls.Add(this.buttonRerunRecolor);
            this.panelRecolor.Controls.Add(this.b);
            this.panelRecolor.Controls.Add(this.g);
            this.panelRecolor.Controls.Add(this.r);
            this.panelRecolor.Location = new System.Drawing.Point(135, 455);
            this.panelRecolor.Name = "panelRecolor";
            this.panelRecolor.Size = new System.Drawing.Size(786, 184);
            this.panelRecolor.TabIndex = 17;
            // 
            // toolTipRerunRecolor
            // 
            this.toolTipRerunRecolor.IsBalloon = true;
            this.toolTipRerunRecolor.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipRerunRecolor.ToolTipTitle = "Reset";
            // 
            // labelRecolor
            // 
            this.labelRecolor.AutoSize = true;
            this.labelRecolor.BackColor = System.Drawing.Color.Black;
            this.labelRecolor.ForeColor = System.Drawing.Color.White;
            this.labelRecolor.Location = new System.Drawing.Point(42, 10);
            this.labelRecolor.Name = "labelRecolor";
            this.labelRecolor.Size = new System.Drawing.Size(176, 17);
            this.labelRecolor.TabIndex = 20;
            this.labelRecolor.Text = "R = R% · G = G% · B = B%";
            // 
            // toolTipBlacken
            // 
            this.toolTipBlacken.IsBalloon = true;
            this.toolTipBlacken.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipBlacken.ToolTipTitle = "Blacken";
            // 
            // blacken
            // 
            this.blacken.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blacken.Location = new System.Drawing.Point(73, 4);
            this.blacken.Name = "blacken";
            this.blacken.Size = new System.Drawing.Size(74, 30);
            this.blacken.TabIndex = 5;
            // 
            // toolTipMeasure
            // 
            this.toolTipMeasure.IsBalloon = true;
            this.toolTipMeasure.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipMeasure.ToolTipTitle = "Measure";
            // 
            // buttonRerunRecolor
            // 
            this.buttonRerunRecolor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRerunRecolor.Image = global::Escher.Properties.Resources.Rerun_16x;
            this.buttonRerunRecolor.Location = new System.Drawing.Point(4, 3);
            this.buttonRerunRecolor.Name = "buttonRerunRecolor";
            this.buttonRerunRecolor.Size = new System.Drawing.Size(32, 32);
            this.buttonRerunRecolor.TabIndex = 19;
            this.buttonRerunRecolor.TabStop = false;
            this.toolTipRerunRecolor.SetToolTip(this.buttonRerunRecolor, "Reset the RGB color components to 0%");
            this.buttonRerunRecolor.UseVisualStyleBackColor = true;
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
            // buttonMeasure
            // 
            this.buttonMeasure.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMeasure.Image = global::Escher.Properties.Resources.RulerMeasure_16x;
            this.buttonMeasure.Location = new System.Drawing.Point(314, 3);
            this.buttonMeasure.Name = "buttonMeasure";
            this.buttonMeasure.Size = new System.Drawing.Size(32, 32);
            this.buttonMeasure.TabIndex = 20;
            this.buttonMeasure.TabStop = false;
            this.toolTipMeasure.SetToolTip(this.buttonMeasure, "Measure the size of the stamp");
            this.buttonMeasure.UseVisualStyleBackColor = true;
            // 
            // buttonBlacken
            // 
            this.buttonBlacken.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBlacken.Image = global::Escher.Properties.Resources.Airbrush_16x;
            this.buttonBlacken.Location = new System.Drawing.Point(281, 3);
            this.buttonBlacken.Name = "buttonBlacken";
            this.buttonBlacken.Size = new System.Drawing.Size(32, 32);
            this.buttonBlacken.TabIndex = 19;
            this.buttonBlacken.TabStop = false;
            this.toolTipBlacken.SetToolTip(this.buttonBlacken, "Blacken the background around the stamp");
            this.buttonBlacken.UseVisualStyleBackColor = true;
            // 
            // buttonRecolor
            // 
            this.buttonRecolor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRecolor.Image = global::Escher.Properties.Resources.ColorPalette_16x;
            this.buttonRecolor.Location = new System.Drawing.Point(215, 3);
            this.buttonRecolor.Name = "buttonRecolor";
            this.buttonRecolor.Size = new System.Drawing.Size(32, 32);
            this.buttonRecolor.TabIndex = 18;
            this.buttonRecolor.TabStop = false;
            this.toolTipRecolor.SetToolTip(this.buttonRecolor, "Adjust the RGB color components from -10% to +10%");
            this.buttonRecolor.UseVisualStyleBackColor = true;
            // 
            // buttonResize
            // 
            this.buttonResize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonResize.Image = global::Escher.Properties.Resources.Resize_16x;
            this.buttonResize.Location = new System.Drawing.Point(347, 3);
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
            this.buttonThumbnail.Location = new System.Drawing.Point(413, 3);
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
            this.buttonUndo.Location = new System.Drawing.Point(486, 3);
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
            this.toolTipCrop.SetToolTip(this.buttonCrop, "Crop the image to remove too much black background around the stamp");
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
            this.buttonBrighten.Location = new System.Drawing.Point(248, 3);
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
            this.buttonSelect.Location = new System.Drawing.Point(380, 3);
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
            this.buttonClose.Location = new System.Drawing.Point(519, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(32, 32);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.TabStop = false;
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Image = global::Escher.Properties.Resources.Save_16x;
            this.buttonSave.Location = new System.Drawing.Point(453, 3);
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
            // measure
            // 
            this.measure.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.measure.Location = new System.Drawing.Point(73, 4);
            this.measure.Name = "measure";
            this.measure.Size = new System.Drawing.Size(74, 30);
            this.measure.TabIndex = 6;
            // 
            // Imaging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1230, 910);
            this.Controls.Add(this.panelRecolor);
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
            ((System.ComponentModel.ISupportInitialize)(this.resize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.angle)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelRecolor.ResumeLayout(false);
            this.panelRecolor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blacken)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTrial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pThumb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measure)).EndInit();
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
        private ColorSlider.ColorSlider r;
        private ColorSlider.ColorSlider b;
        private ColorSlider.ColorSlider g;
        private System.Windows.Forms.Button buttonRecolor;
        private System.Windows.Forms.ToolTip toolTipRecolor;
        private System.Windows.Forms.Panel panelRecolor;
        private System.Windows.Forms.Button buttonRerunRecolor;
        private System.Windows.Forms.ToolTip toolTipRerunRecolor;
        private System.Windows.Forms.Label labelRecolor;
        private System.Windows.Forms.Button buttonBlacken;
        private System.Windows.Forms.ToolTip toolTipBlacken;
        private System.Windows.Forms.NumericUpDown blacken;
        private System.Windows.Forms.Button buttonMeasure;
        private System.Windows.Forms.ToolTip toolTipMeasure;
        private System.Windows.Forms.NumericUpDown measure;
    }
}
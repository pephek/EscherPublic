using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public partial class Imaging : Form
    {
        private const string cImage = "_image.jpg";
        private const string cThumb = "_thumb.jpg";
        private const string cColor = "_color.jpg";
        private const string cPrint = "_print.jpg";
        private const string cTrial = "_trial.jpg";

        private Design series;
        private DesignEntry stamp;
        private string folder;
        private string country;
        private string section;

        private string thumb;
        private string image;
        private string color;
        private string print;

        private ImagingMode mode;

        private bool isChanged = false;

        private bool isSelecting;
        private Point selectionStart;
        private Rectangle selection = new Rectangle();
        private Brush selectionBrush;

        private Size baseSizePortrait = new Size();
        private Size baseSizeLandscape = new Size();
        private Size baseSize;

        private float scale;

        public Imaging()
        {
            InitializeComponent();

            this.BackColor = Color.Black;

            buttonPrevious.Click += new EventHandler((sender, e) => Previous());
            buttonNext.Click += new EventHandler((sender, e) => Next());

            buttonRotate.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Rotate));
            buttonCrop.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Crop));
            buttonRecolor.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Recolor));
            buttonBrighten.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Brighten));
            buttonBlacken.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Blacken));
            buttonMeasure.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Measure));
            buttonResize.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Resize));
            buttonSelect.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Select));
            buttonThumbnail.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Thumbnail));

            buttonSave.Click += new EventHandler((sender, e) => Save());
            buttonUndo.Click += new EventHandler((sender, e) => Undo());
            buttonClose.Click += new EventHandler((sender, e) => Exit());

            buttonAccept.Click += new EventHandler((sender, e) => AcceptOrReject(accepted: true));
            buttonReject.Click += new EventHandler((sender, e) => AcceptOrReject(accepted: false));

            angle.ValueChanged += new EventHandler((sender, e) => Rotate((float)angle.Value));
            brightness.ValueChanged += new EventHandler((sender, e) => Brightness((float)brightness.Value));
            blacken.ValueChanged += new EventHandler((sender, e) => Blacken((byte)blacken.Value));
            measure.ValueChanged += new EventHandler((sender, e) => Measure((byte)measure.Value, this.stamp.Perforation));
            resize.ValueChanged += new EventHandler((sender, e) => Resice((float)resize.Value));

            r.ValueChanged += new EventHandler((sender, e) => Recolor((float)r.Value, (float)g.Value, (float)b.Value));
            g.ValueChanged += new EventHandler((sender, e) => Recolor((float)r.Value, (float)g.Value, (float)b.Value));
            b.ValueChanged += new EventHandler((sender, e) => Recolor((float)r.Value, (float)g.Value, (float)b.Value));

            buttonRerunRecolor.Click += new EventHandler((sender, e) => RerunRecolor());

            pTrial.MouseMove += new MouseEventHandler((sender, e) => MouseMoved(e.Button, e.Location));
            pTrial.MouseDown += new MouseEventHandler((sender, e) => MouseIsDown(e.Location));
            pTrial.MouseUp += new MouseEventHandler((sender, e) => MouseIsUp());
            pTrial.Paint += new PaintEventHandler((sender, e) => PaintSelection(e.Graphics));

            this.KeyPreview = true;

            this.scale = 1;
        }

        private void GetPortraitAndLandscapeBaseSizes(string rootPath)
        {
            this.baseSizePortrait = new Size();
            this.baseSizeLandscape = new Size();

            foreach (DesignEntry stamp in this.series)
            {
                string imagePath = string.Format("{0}\\image\\{1}.jpg", rootPath, stamp.Number);

                if (File.Exists(imagePath))
                {
                    using (Image image = Image.FromFile(imagePath))
                    {
                        if (image.Width < image.Height) // Portrait
                        {
                            if (image.Width > this.baseSizePortrait.Width)
                            {
                                this.baseSizePortrait.Width = image.Width;
                            }
                            if (image.Height > this.baseSizePortrait.Height)
                            {
                                this.baseSizePortrait.Height = image.Height;
                            }
                        }
                        else // Landscape
                        {
                            if (image.Width > this.baseSizeLandscape.Width)
                            {
                                this.baseSizeLandscape.Width = image.Width;
                            }
                            if (image.Height > this.baseSizeLandscape.Height)
                            {
                                this.baseSizeLandscape.Height = image.Height;
                            }
                        }
                    }
                }
            }

            if (this.baseSizePortrait.IsEmpty)
            {
                this.baseSizePortrait = Escher.Properties.Resources.ImageNotFound.Size;
            }
            if (this.baseSizeLandscape.IsEmpty)
            {
                this.baseSizeLandscape = Escher.Properties.Resources.ImageNotFound.Size;
            }

            this.baseSizePortrait.Width = (int)(1.1 * this.baseSizePortrait.Width);
            this.baseSizePortrait.Height = (int)(1.1 * this.baseSizePortrait.Height);
            this.baseSizeLandscape.Width = (int)(1.1 * this.baseSizeLandscape.Width);
            this.baseSizeLandscape.Height = (int)(1.1 * this.baseSizeLandscape.Height);
        }

        public void SetImage(Design series, string stampNumber, string folder, string country, string section)
        {
            this.series = series;
            this.folder = folder;
            this.country = country;
            this.section = section;

            this.stamp = series.GetStampFromSeries(stampNumber);

            string path = string.Format("{0}\\{1}\\{2}", folder, country, section);

            GetPortraitAndLandscapeBaseSizes(path);

            this.thumb = string.Format("{0}\\{1}.jpg", path, stamp.Number);
            this.image = string.Format("{0}\\image\\{1}.jpg", path, stamp.Number);
            this.color = string.Format("{0}\\xlcolor\\{1}.jpg", path, stamp.Number);
            this.print = string.Format("{0}\\xlprint\\{1}.jpg", path, stamp.Number);

            if (File.Exists(cThumb)) File.Delete(cThumb);
            if (File.Exists(cImage)) File.Delete(cImage);
            if (File.Exists(cColor)) File.Delete(cColor);
            if (File.Exists(cPrint)) File.Delete(cPrint);

            if (File.Exists(this.thumb)) File.Copy(this.thumb, cThumb, overwrite: true);
            if (File.Exists(this.image)) File.Copy(this.image, cImage, overwrite: true);
            if (File.Exists(this.color)) File.Copy(this.color, cColor, overwrite: true);
            if (File.Exists(this.print)) File.Copy(this.print, cPrint, overwrite: true);

            pImage.LoadImageAndUnlock(cImage);
            pThumb.LoadImageAndUnlock(cThumb);
            pColor.LoadImageAndUnlock(cColor);
            pPrint.LoadImageAndUnlock(cPrint);
            pTrial.LoadImageAndUnlock(cTrial);

            pImage.SizeMode = PictureBoxSizeMode.AutoSize;
            pThumb.SizeMode = PictureBoxSizeMode.AutoSize;
            pColor.SizeMode = PictureBoxSizeMode.AutoSize;
            pPrint.SizeMode = PictureBoxSizeMode.AutoSize;
            pTrial.SizeMode = PictureBoxSizeMode.AutoSize;

            this.baseSize = (pImage.Width < pImage.Height ? this.baseSizePortrait : this.baseSizeLandscape);

            buttonPrevious.Enabled = this.series.FindIndex(s => s.Number == this.stamp.Number) > 0;
            buttonNext.Enabled = this.series.FindIndex(s => s.Number == this.stamp.Number) < this.series.Count() - 1;

            buttonRotate.Enabled = File.Exists(cImage);
            buttonCrop.Enabled = buttonRotate.Enabled;
            buttonRecolor.Enabled = buttonRotate.Enabled;
            buttonBrighten.Enabled = buttonRotate.Enabled;
            buttonBlacken.Enabled = buttonRotate.Enabled;
            buttonMeasure.Enabled = !string.IsNullOrEmpty(this.stamp.Perforation);
            buttonResize.Enabled = buttonRotate.Enabled;
            buttonSelect.Enabled = buttonRotate.Enabled;
            buttonThumbnail.Enabled = buttonRotate.Enabled;

            buttonSave.Enabled = false;
            buttonUndo.Enabled = false;

            panelButtons.Visible = true;
            panelImaging.Visible = false;
            panelRecolor.Visible = false;

            this.CancelButton = buttonClose;
            this.AcceptButton = null;

            this.mode = ImagingMode.None;

            labelMode.Text = this.mode.Text(this.stamp.Number, pImage.Image, this.scale);

            Repaint();
        }

        private void Repaint()
        {
            int width = (int)(this.scale * 2 * this.baseSize.Width);
            int height = (int)(this.scale * 2 * this.baseSize.Height);

            if (panelButtons.Width > width)
            {
                width = panelButtons.Width;
            }

            if (panelButtons.Width > this.baseSize.Width)
            {
                panelButtons.Location = new Point(width / 2 - panelButtons.Width / 2, height);
            }
            else
            {
                panelButtons.Location = new Point(baseSize.Width / 2 - panelButtons.Width / 2, height);
            }

            panelImaging.Location = new Point(width / 2 - panelImaging.Width / 2, height);

            panelRecolor.Location = new Point(5, height - panelRecolor.Height);
            panelRecolor.Width = width - 2 * panelRecolor.Left;
            r.Width = panelRecolor.Width - r.Left;
            g.Width = r.Width;
            b.Width = r.Width;

            height += panelButtons.Height;

            int screenWidthInPixels = Screen.FromControl(this).WorkingArea.Width;
            int screenHeightInPixels = Screen.FromControl(this).WorkingArea.Height;

            if (width != this.Width || height != this.Height)
            {
                this.Size = new Size(width, height);
                this.Location = new Point((screenWidthInPixels - width) / 2, (screenHeightInPixels - height) / 2);
            }

            int w = baseSize.Width;
            int h = baseSize.Height;

            pThumb.Location = new Point(w / 2 - pThumb.Width / 2, h / 2 - pThumb.Height / 2);
            pImage.Location = new Point(w / 2 - pImage.Width / 2, h * 3 / 2 - pImage.Height / 2);
            pColor.Location = new Point(w * 3 / 2 - pColor.Width / 2, h / 2 - pColor.Height / 2);
            pPrint.Location = new Point(w * 3 / 2 - pPrint.Width / 2, h * 3 / 2 - pPrint.Height / 2);
            pTrial.Location = new Point(w - pTrial.Width / 2, h - pTrial.Height / 2);

            if (this.scale != 1)
            {

                pThumb.Location = new Point((int)(this.scale * pThumb.Left), (int)(this.scale * pThumb.Top));
                pImage.Location = new Point((int)(this.scale * pImage.Left), (int)(this.scale * pImage.Top));
                pColor.Location = new Point((int)(this.scale * pColor.Left), (int)(this.scale * pColor.Top));
                pPrint.Location = new Point((int)(this.scale * pPrint.Left), (int)(this.scale * pPrint.Top));
                pTrial.Location = new Point((int)(this.scale * pTrial.Left), (int)(this.scale * pTrial.Top));

                pThumb.Size = new Size((int)(this.scale * pThumb.Width), (int)(this.scale * pThumb.Height));
                pImage.Size = new Size((int)(this.scale * pImage.Width), (int)(this.scale * pImage.Height));
                pColor.Size = new Size((int)(this.scale * pColor.Width), (int)(this.scale * pColor.Height));
                pPrint.Size = new Size((int)(this.scale * pPrint.Width), (int)(this.scale * pPrint.Height));
                pTrial.Size = new Size((int)(this.scale * pTrial.Width), (int)(this.scale * pTrial.Height));

                pThumb.SizeMode = PictureBoxSizeMode.StretchImage;
                pImage.SizeMode = PictureBoxSizeMode.StretchImage;
                pColor.SizeMode = PictureBoxSizeMode.StretchImage;
                pPrint.SizeMode = PictureBoxSizeMode.StretchImage;
                pTrial.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            if (this.Width > screenWidthInPixels || this.Height > screenHeightInPixels)
            {
                if (this.scale == 1)
                {
                    this.scale /= 2;

                    labelMode.Text = this.mode.Text(this.stamp.Number, pImage.Image, this.scale);

                    Repaint();
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (buttonReject.Visible && buttonReject.Enabled)
                    {
                        AcceptOrReject(accepted: false);
                    }
                    break;
                case Keys.Enter:
                    if (buttonAccept.Visible && buttonAccept.Enabled)
                    {
                        AcceptOrReject(accepted: true);
                    }
                    break;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (buttonPrevious.Enabled)
                    {
                        Previous();
                    }
                    break;
                case Keys.Right:
                    if (buttonNext.Enabled)
                    {
                        Next();
                    }
                    break;
            }
        }

        private DialogResult DiscardChanges()
        {
            if (buttonSave.Enabled)
            {
                return MessageBox.Show("The image has unsaved changed, do you want to discard the changes?", App.GetName() + " · Discard changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            }
            else
            {
                return DialogResult.Yes;
            }
        }

        private void Previous()
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                SetImage(this.series, this.series[this.series.FindIndex(s => s.Number == this.stamp.Number) - 1].Number, this.folder, this.country, this.section);
            }
        }

        private void Next()
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                SetImage(this.series, this.series[this.series.FindIndex(s => s.Number == this.stamp.Number) + 1].Number, this.folder, this.country, this.section);
            }
        }

        private void Save()
        {
            if (MessageBox.Show("Are you sure you want to save the changes?", App.GetName() + " · Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (File.Exists(cThumb)) File.Copy(cThumb, this.thumb, overwrite: true);
                if (File.Exists(cImage)) File.Copy(cImage, this.image, overwrite: true);
                if (File.Exists(cColor)) File.Copy(cColor, this.color, overwrite: true);
                if (File.Exists(cPrint)) File.Copy(cPrint, this.print, overwrite: true);

                SetImage(this.series, this.stamp.Number, this.folder, this.country, this.section);
            }
        }

        private void Undo()
        {
            if (MessageBox.Show("Are you sure you want to undo the changes?", App.GetName() + " · Undo changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                SetImage(this.series, this.stamp.Number, this.folder, this.country, this.section);
            }
        }

        private void Exit()
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                this.DialogResult = isChanged ? DialogResult.OK : DialogResult.Cancel;

                this.Close();
            }
        }

        private void SetMode(ImagingMode mode)
        {
            this.mode = mode;

            labelMode.Text = this.mode.Text(scale: this.scale);

            panelButtons.Visible = false;
            panelImaging.Visible = true;
            panelRecolor.Visible = false;

            pTrial.Visible = true;
            pImage.Visible = false;
            pThumb.Visible = false;
            pColor.Visible = false;
            pPrint.Visible = false;

            pTrial.Image = new Bitmap(pImage.Image);
            pTrial.Cursor = Cursors.Default;

            angle.Visible = false;
            brightness.Visible = false;
            blacken.Visible = false;
            measure.Visible = false;
            resize.Visible = false;

            this.isSelecting = false;

            switch (this.mode)
            {
                case ImagingMode.Rotate:
                    angle.Value = 0.0M;
                    angle.Visible = true;
                    break;
                case ImagingMode.Brighten:
                    brightness.Value = 0;
                    brightness.Visible = true;
                    break;
                case ImagingMode.Blacken:
                    blacken.Value = 0;
                    blacken.Visible = true;
                    break;
                case ImagingMode.Measure:
                    measure.Tag = false;
                    measure.Value = 64;
                    measure.Tag = true;
                    measure.Visible = true;
                    Measure((byte)measure.Value, this.stamp.Perforation);
                    break;
                case ImagingMode.Resize:
                    resize.Value = 100;
                    resize.Visible = true;
                    break;
                case ImagingMode.Crop:
                case ImagingMode.Select:
                    pTrial.Cursor = Cursors.Cross;
                    this.isSelecting = true;
                    this.selection = new Rectangle();
                    if (this.mode == ImagingMode.Select)
                    {
                        this.selectionBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0));
                    }
                    else
                    {
                        this.selectionBrush = new SolidBrush(Color.FromArgb(192, 255, 255, 255));
                    }
                    break;
                case ImagingMode.Recolor:
                    panelRecolor.Visible = true;
                    RerunRecolor();
                    break;
            }

            buttonAccept.Enabled = false;

            Repaint();

            this.Focus();

            if (mode == ImagingMode.Thumbnail)
            {
                AcceptOrReject(accepted: true);
            }
        }

        private void AcceptOrReject(bool accepted)
        {
            if (accepted)
            {
                this.isChanged = true;

                if (this.mode != ImagingMode.Measure)
                {
                    buttonSave.Enabled = true;
                    buttonUndo.Enabled = true;
                }

                switch (this.mode)
                {
                    case ImagingMode.Rotate:

                        this.baseSize.Width = (int)((float)pTrial.Image.Width / pImage.Image.Width * this.baseSize.Width);
                        this.baseSize.Height = (int)((float)pTrial.Image.Height / pImage.Image.Height * this.baseSize.Height);

                        pImage.Image.Dispose();
                        pImage.Image = new Bitmap(pTrial.Image);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);
                        pThumb.LoadImageAndUnlock(cThumb);

                        break;

                    case ImagingMode.Crop:

                        pImage.Image.Dispose();
                        pImage.Image = pTrial.Image.GetSelection(this.selection, convertToGrayscale: false);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);
                        pThumb.LoadImageAndUnlock(cThumb);

                        break;

                    case ImagingMode.Recolor:

                        pImage.Image = pImage.Image.Recolor((float)r.Value / 100, (float)g.Value / 100, (float)b.Value / 100);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);
                        pThumb.LoadImageAndUnlock(cThumb);

                        break;

                    case ImagingMode.Brighten:

                        pImage.Image.Dispose();
                        pImage.Image = new Bitmap(pTrial.Image);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);
                        pThumb.LoadImageAndUnlock(cThumb);

                        if (File.Exists(this.color))
                        {
                            pColor.Image = pColor.Image.Brighten((float)brightness.Value / 100);
                            pColor.Image.SaveAsJpeg(cColor, 100);
                        }

                        if (File.Exists(this.print))
                        {
                            pPrint.Image = pPrint.Image.Brighten((float)brightness.Value / 100);
                            pPrint.Image.SaveAsJpeg(cPrint, 100);
                        }

                        break;

                    case ImagingMode.Blacken:

                        pImage.Image = pImage.Image.Blacken((byte)blacken.Value);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);
                        pThumb.LoadImageAndUnlock(cThumb);

                        break;

                    case ImagingMode.Measure:
                        break;

                    case ImagingMode.Resize:

                        pImage.Image.Dispose();
                        pImage.Image = new Bitmap(pTrial.Image);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        break;

                    case ImagingMode.Select:

                        pColor.Image.Dispose();
                        pColor.Image = pTrial.Image.GetSelection(this.selection, convertToGrayscale: false);
                        pColor.Image.SaveAsJpeg(cColor, 100);
                        pColor.LoadImageAndUnlock(cColor);

                        pPrint.Image.Dispose();
                        pPrint.Image = pTrial.Image.GetSelection(this.selection, convertToGrayscale: true);
                        pPrint.Image.SaveAsJpeg(cPrint, 100);
                        pPrint.LoadImageAndUnlock(cPrint);

                        break;

                    case ImagingMode.Thumbnail:

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);
                        pThumb.LoadImageAndUnlock(cThumb);

                        break;
                }
            }

            this.mode = ImagingMode.None;

            labelMode.Text = this.mode.Text(this.stamp.Number, pImage.Image, this.scale);

            panelButtons.Visible = true;
            panelImaging.Visible = false;
            panelRecolor.Visible = false;

            pTrial.Visible = false;
            pImage.Visible = true;
            pThumb.Visible = true;
            pColor.Visible = true;
            pPrint.Visible = true;

            Repaint();
        }

        private void Rotate(float value)
        {
            if (this.mode == ImagingMode.Rotate)
            {
                pTrial.Image.Dispose();

                pTrial.Image = pImage.Image.Rotate(value, true, false, Color.Black);

                buttonAccept.Enabled = (value != 0);

                Repaint();
            }
        }

        private void Brightness(float value)
        {
            if (mode == ImagingMode.Brighten)
            {
                pTrial.Image.Dispose();

                pTrial.Image = pImage.Image.Brighten(value / 100);

                buttonAccept.Enabled = (value != 0);
            }
        }

        private void Blacken(byte value)
        {
            if (this.mode == ImagingMode.Blacken)
            {
                pTrial.Image.Dispose();

                pTrial.Image = pImage.Image.Blacken(value);

                buttonAccept.Enabled = (value != 0);
            }
        }

        private void Measure(byte value, string perforation)
        {
            if (this.mode == ImagingMode.Measure && (bool)measure.Tag)
            {
                this.UseWaitCursor = true;

                pTrial.Image.Dispose();

                pTrial.Image = pImage.Image.Measure(value, perforation, out string size).Resize(2.0F); Repaint();

                buttonAccept.Enabled = true;

                this.UseWaitCursor = false;

                labelMode.Text = string.Format("{0} : {1}", this.mode.Text(scale: this.scale), size);
            }
        }

        private void Resice(float value)
        {
            if (this.mode == ImagingMode.Resize)
            {
                pTrial.Image.Dispose();

                pTrial.Image = pImage.Image.Resize(value / 100);

                buttonAccept.Enabled = (value != 100);
            }
        }

        private void Recolor(float r, float g, float b)
        {
            if (this.mode == ImagingMode.Recolor)
            {
                if ((bool)panelRecolor.Tag)
                {
                    labelRecolor.Text = string.Format("R = {0}% · G = {1}% · B = {2}%", r, g, b);

                    pTrial.Image.Dispose();

                    pTrial.Image = pImage.Image.Recolor(r / 100, g / 100, b / 100);

                    buttonAccept.Enabled = (r != 0 || g != 0 || b != 0);
                }
            }
        }

        private void RerunRecolor()
        {
            if (this.mode == ImagingMode.Recolor)
            {
                panelRecolor.Tag = false;
                r.Value = 0;
                g.Value = 0;
                b.Value = 0;
                panelRecolor.Tag = true;

                Recolor((float)r.Value, (float)g.Value, (float)b.Value);
            }
        }

        private void MouseMoved(MouseButtons button, Point location)
        {
            if (this.isSelecting)
            {
                if (button != MouseButtons.Left)
                    return;
                Point selectionEnd = location;
                selection.Location = new Point(
                    Math.Min(selectionStart.X, selectionEnd.X),
                    Math.Min(selectionStart.Y, selectionEnd.Y));
                selection.Size = new Size(
                    Math.Abs(selectionStart.X - selectionEnd.X),
                    Math.Abs(selectionStart.Y - selectionEnd.Y));
                pTrial.Invalidate();
            }
        }

        private void MouseIsDown(Point location)
        {
            if (this.isSelecting)
            {
                selectionStart = location;
                pTrial.Invalidate();
            }
        }

        private void MouseIsUp()
        {
            if (this.isSelecting)
            {
                buttonAccept.Enabled = (this.selection.Width >= 50 && this.selection.Height >= 50);
            }
        }

        private void PaintSelection(Graphics graphics)
        {
            if (this.isSelecting)
            {
                if (selection.Width > 0 && selection.Height > 0)
                {
                    graphics.FillRectangle(selectionBrush, selection);
                }
            }
        }
    }
}

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

        public Imaging()
        {
            InitializeComponent();

            this.BackColor = Color.Black;

            buttonPrevious.Click += new EventHandler((sender, e) => Previous());
            buttonNext.Click += new EventHandler((sender, e) => Next());

            buttonRotate.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Rotating));
            buttonCrop.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Cropping));
            buttonBrighten.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Brightening));
            buttonResize.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Resize));
            buttonSelect.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Selecting));
            buttonThumbnail.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Thumbnail));

            buttonSave.Click += new EventHandler((sender, e) => Save());
            buttonUndo.Click += new EventHandler((sender, e) => Undo());
            buttonClose.Click += new EventHandler((sender, e) => Exit());

            buttonAccept.Click += new EventHandler((sender, e) => AcceptOrReject(accepted: true));
            buttonReject.Click += new EventHandler((sender, e) => AcceptOrReject(accepted: false));

            angle.ValueChanged += new EventHandler((sender, e) => Rotate((float)angle.Value));
            brightness.ValueChanged += new EventHandler((sender, e) => Brightness((float)brightness.Value));
            resize.ValueChanged += new EventHandler((sender, e) => Resice((float)resize.Value));

            pTrial.MouseMove += new MouseEventHandler((sender, e) => MouseMoved(e.Button, e.Location));
            pTrial.MouseDown += new MouseEventHandler((sender, e) => MouseIsDown(e.Location));
            pTrial.MouseUp += new MouseEventHandler((sender, e) => MouseIsUp());
            pTrial.Paint += new PaintEventHandler((sender, e) => PaintSelection(e.Graphics));

            this.KeyPreview = true;
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
            buttonBrighten.Enabled = buttonRotate.Enabled;
            buttonResize.Enabled = buttonRotate.Enabled;
            buttonSelect.Enabled = buttonRotate.Enabled;
            buttonThumbnail.Enabled = buttonRotate.Enabled;

            buttonSave.Enabled = false;
            buttonUndo.Enabled = false;

            panelButtons.Visible = true;
            panelImaging.Visible = false;

            this.CancelButton = buttonClose;
            this.AcceptButton = null;

            this.mode = ImagingMode.None;

            labelMode.Text = this.mode.Text(this.stamp.Number);

            Repaint();
        }

        private void Repaint()
        {
            int width = 2 * this.baseSize.Width;
            int height = 2 * this.baseSize.Height;

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

            labelMode.Text = this.mode.Text();

            panelButtons.Visible = false;
            panelImaging.Visible = true;

            pTrial.Visible = true;
            pImage.Visible = false;
            pThumb.Visible = false;
            pColor.Visible = false;
            pPrint.Visible = false;

            pTrial.Image = new Bitmap(pImage.Image);
            pTrial.Cursor = Cursors.Default;

            angle.Visible = false;
            brightness.Visible = false;
            resize.Visible = false;

            this.isSelecting = false;

            switch (this.mode)
            {
                case ImagingMode.Rotating:
                    angle.Value = 0.0M;
                    angle.Visible = true;
                    break;
                case ImagingMode.Brightening:
                    brightness.Value = 0;
                    brightness.Visible = true;
                    break;
                case ImagingMode.Resize:
                    resize.Value = 100;
                    resize.Visible = true;
                    break;
                case ImagingMode.Cropping:
                case ImagingMode.Selecting:
                    pTrial.Cursor = Cursors.Cross;
                    this.isSelecting = true;
                    this.selection = new Rectangle();
                    if (this.mode == ImagingMode.Selecting)
                    {
                        this.selectionBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0));
                    }
                    else
                    {
                        this.selectionBrush = new SolidBrush(Color.FromArgb(192, 255, 255, 255));
                    }
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

                buttonSave.Enabled = true;
                buttonUndo.Enabled = true;

                switch (this.mode)
                {
                    case ImagingMode.Rotating:

                        this.baseSize.Width = (int)((float)pTrial.Image.Width / pImage.Image.Width * this.baseSize.Width);
                        this.baseSize.Height = (int)((float)pTrial.Image.Height / pImage.Image.Height * this.baseSize.Height);

                        pImage.Image.Dispose();
                        pImage.Image = new Bitmap(pTrial.Image);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);
                        pThumb.LoadImageAndUnlock(cThumb);

                        break;

                    case ImagingMode.Cropping:

                        pImage.Image.Dispose();
                        pImage.Image = pTrial.Image.GetSelection(this.selection, convertToGrayscale: false);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);
                        pThumb.LoadImageAndUnlock(cThumb);

                        break;

                    case ImagingMode.Brightening:

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

                    case ImagingMode.Resize:

                        pImage.Image.Dispose();
                        pImage.Image = new Bitmap(pTrial.Image);
                        pImage.Image.SaveAsJpeg(cImage, 100);

                        break;

                    case ImagingMode.Selecting:

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

            labelMode.Text = this.mode.Text(this.stamp.Number);

            panelButtons.Visible = true;
            panelImaging.Visible = false;

            pTrial.Visible = false;
            pImage.Visible = true;
            pThumb.Visible = true;
            pColor.Visible = true;
            pPrint.Visible = true;

            Repaint();
        }

        private void Rotate(float value)
        {
            pTrial.Image.Dispose();

            pTrial.Image = pImage.Image.Rotate(value, true, false, Color.Black);

            buttonAccept.Enabled = (value != 0);

            Repaint();
        }

        private void Brightness(float value)
        {
            pTrial.Image.Dispose();

            pTrial.Image = pImage.Image.Brighten(value / 100);

            buttonAccept.Enabled = (value != 0);

            Repaint();
        }

        private void Resice(float value)
        {
            pTrial.Image.Dispose();

            pTrial.Image = pImage.Image.Resize(value / 100);

            buttonAccept.Enabled = (value != 100);

            Repaint();
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

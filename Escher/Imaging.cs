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

        private Design stamps;
        private DesignEntry stamp;
        private string folder;
        private string country;
        private string section;

        private string thumb;
        private string image;
        private string color;
        private string print;

        private ImagingMode mode;

        private bool isChanged;

        private bool isSelecting;
        private Point selectionStart;
        private Rectangle selection = new Rectangle();
        private Brush selectionBrush;

        private int baseWidth;
        private int baseHeight;

        public Imaging()
        {
            InitializeComponent();

            this.BackColor = Color.Black;

            buttonPrevious.Click += new EventHandler((sender, e) => Previous());
            buttonNext.Click += new EventHandler((sender, e) => Next());

            buttonRotate.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Rotating));
            buttonCrop.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Cropping));
            buttonSelect.Click += new EventHandler((sender, e) => SetMode(ImagingMode.Selecting));

            buttonSave.Click += new EventHandler((sender, e) => Save());
            buttonUndo.Click += new EventHandler((sender, e) => Undo());
            buttonClose.Click += new EventHandler((sender, e) => Exit());

            buttonAccept.Click += new EventHandler((sender, e) => AcceptOrReject(accepted: true));
            buttonReject.Click += new EventHandler((sender, e) => AcceptOrReject(accepted: false));

            angle.ValueChanged += new EventHandler((sender, e) => Rotate((float)angle.Value));

            pTrial.MouseMove += new MouseEventHandler((sender, e) => MouseMoved(e.Button, e.Location));
            pTrial.MouseDown += new MouseEventHandler((sender, e) => MouseIsDown(e.Location));
            pTrial.MouseUp += new MouseEventHandler((sender, e) => MouseIsUp());
            pTrial.Paint += new PaintEventHandler((sender, e) => PaintSelection(e.Graphics));

            this.KeyPreview = true;
        }

        public DialogResult SetImage(Design stamps, DesignEntry stamp, string folder, string country, string section)
        {
            this.stamps = stamps;
            this.stamp = stamp;
            this.folder = folder;
            this.country = country;
            this.section = section;

            string path = string.Format("{0}\\{1}\\{2}", folder, country, section);

            this.thumb = string.Format("{0}\\{1}.jpg", path, stamp.Number);
            this.image = string.Format("{0}\\image\\{1}.jpg", path, stamp.Number);
            this.color = string.Format("{0}\\xlcolor\\{1}.jpg", path, stamp.Number);
            this.print = string.Format("{0}\\xlprint\\{1}.jpg", path, stamp.Number);

            if (!File.Exists(this.image))
            {
                MessageBox.Show(string.Format("Image '{0}' is not found!", this.image), App.GetName() + " · Image not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return DialogResult.Cancel;
            }

            if (File.Exists(cThumb)) File.Delete(cThumb);
            if (File.Exists(cImage)) File.Delete(cImage);
            if (File.Exists(cColor)) File.Delete(cColor);
            if (File.Exists(cPrint)) File.Delete(cPrint);

            if (File.Exists(this.thumb)) File.Copy(this.thumb, cThumb, overwrite: true);
            if (File.Exists(this.image)) File.Copy(this.image, cImage, overwrite: true);
            if (File.Exists(this.color)) File.Copy(this.color, cColor, overwrite: true);
            if (File.Exists(this.print)) File.Copy(this.print, cPrint, overwrite: true);

            ResetImage();

            buttonPrevious.Enabled = this.stamps.FindIndex(s => s.Number == this.stamp.Number) > 0;
            buttonNext.Enabled = this.stamps.FindIndex(s => s.Number == this.stamp.Number) < this.stamps.Count() - 1;

            buttonSave.Enabled = false;
            buttonUndo.Enabled = false;

            isChanged = false;

            return DialogResult.OK;
        }

        private void ResetImage()
        {
            this.mode = ImagingMode.None;

            labelMode.Text = this.mode.Text();

            LoadImage(pImage, cImage);
            LoadImage(pThumb, cThumb);
            LoadImage(pColor, cColor);
            LoadImage(pPrint, cPrint);
            LoadImage(pTrial, cTrial);

            this.baseWidth = pImage.Width;
            this.baseHeight = pImage.Height;

            int width;
            int height;

            if (this.baseWidth < this.baseHeight) // Portrait stamp
            {
                width = 3 * this.baseWidth;
                height = 2 * this.baseHeight;
            }
            else // Landscape stamp
            {
                width = 2 * this.baseWidth;
                height = 3 * this.baseHeight;
            }

            SetLocations(this.baseWidth, this.baseHeight);

            if (panelButtons.Width > width)
            {
                width = panelButtons.Width;
            }

            panelButtons.Location = new Point(0, height - panelButtons.Height);
            panelImaging.Location = new Point(0, height - panelButtons.Height);

            panelButtons.Visible = true;
            panelImaging.Visible = false;

            this.CancelButton = buttonClose;
            this.AcceptButton = null;

            int screenWidthInPixels = Screen.FromControl(this).WorkingArea.Width;
            int screenHeightInPixels = Screen.FromControl(this).WorkingArea.Height;

            if (width != this.Width || height != this.Height)
            {
                this.Size = new Size(width, height);
                this.Location = new Point((screenWidthInPixels - width) / 2, (screenHeightInPixels - height) / 2);
            }
        }

        private void SetLocations(int w, int h)
        {
            if (w < h) // Portrait stamp
            {
                pThumb.Location = new Point(w - pThumb.Width / 2, h - pThumb.Height / 2);
                pImage.Location = new Point(w - pImage.Width / 2, h - pImage.Height / 2);
                pTrial.Location = new Point(w - pTrial.Width / 2, h - pTrial.Height / 2);
                pColor.Location = new Point((int)(2.5 * w) - pColor.Width / 2, (int)(0.5 * h) - pColor.Height / 2);
                pPrint.Location = new Point((int)(2.5 * w) - pPrint.Width / 2, (int)(1.5 * h) - pPrint.Height / 2);
            }
            else // Landscape stamp
            {
            }
        }

        private void LoadImage(PictureBox pictureBox, string imagePath)
        {
            if (File.Exists(imagePath))
            {
                using (Image image = Image.FromFile(imagePath))
                {
                    Bitmap bitmap = new Bitmap(image);
                    pictureBox.Image = bitmap;
                }
            }
            else
            {
                pictureBox.Image = Escher.Properties.Resources.ImageNotFound;
            }

            pictureBox.Width = pictureBox.Image.Width;
            pictureBox.Height = pictureBox.Image.Height;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
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
                if (SetImage(this.stamps, this.stamps[this.stamps.FindIndex(s => s.Number == this.stamp.Number) - 1], this.folder, this.country, this.section) == DialogResult.Cancel)
                {
                    Close();
                }
            }
        }

        private void Next()
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                if (SetImage(this.stamps, this.stamps[this.stamps.FindIndex(s => s.Number == this.stamp.Number) + 1], this.folder, this.country, this.section) == DialogResult.Cancel)
                {
                    Close();
                }
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

                SetImage(this.stamps, this.stamp, this.folder, this.country, this.section);
            }
        }

        private void Undo()
        {
            if (MessageBox.Show("Are you sure you want to undo the changes?", App.GetName() + " · Undo changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                SetImage(this.stamps, this.stamp, this.folder, this.country, this.section);
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

            this.isSelecting = false;

            switch (this.mode)
            {
                case ImagingMode.Rotating:
                    angle.Visible = true;
                    angle.Value = 0.0M;
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

            SetLocations(this.baseWidth, this.baseHeight);

            this.Focus();
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
                    case ImagingMode.Cropping:

                        pImage.Image.Dispose();

                        if (this.mode == ImagingMode.Rotating)
                        {
                            pImage.Image = new Bitmap(pTrial.Image);
                        }
                        else
                        {
                            pImage.Image = pTrial.Image.GetSelection(this.selection, convertToGrayscale: false);
                        }

                        pImage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);

                        LoadImage(pThumb, cThumb);

                        break;

                    case ImagingMode.Selecting:

                        pColor.Image.Dispose();
                        pColor.Image = pTrial.Image.GetSelection(this.selection, convertToGrayscale: false);
                        pColor.Image.SaveAsJpeg(cColor, 100);

                        LoadImage(pColor, cColor);

                        pPrint.Image.Dispose();
                        pPrint.Image = pTrial.Image.GetSelection(this.selection, convertToGrayscale: true);
                        pPrint.Image.SaveAsJpeg(cPrint, 100);

                        LoadImage(pPrint, cPrint);

                        break;
                }
            }

            this.mode = ImagingMode.None;

            labelMode.Text = this.mode.Text();

            panelButtons.Visible = true;
            panelImaging.Visible = false;

            pTrial.Visible = false;
            pImage.Visible = true;
            pThumb.Visible = true;
            pColor.Visible = true;
            pPrint.Visible = true;

            SetLocations(this.baseWidth, this.baseHeight);
        }

        private void Rotate(float value)
        {
            pTrial.Image.Dispose();

            pTrial.Image = pImage.Image.Rotate(value, true, false, Color.Black);

            buttonAccept.Enabled = (value != 0);

            SetLocations(this.baseWidth, this.baseHeight);
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

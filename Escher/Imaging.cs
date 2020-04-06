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

            buttonUndo.Click += new EventHandler((sender, e) => Undo());
            buttonClose.Click += new EventHandler((sender, e) => Exit());

            buttonAccept.Click += new EventHandler((sender, e) => AcceptOrReject(sender, e, accepted: true));
            buttonReject.Click += new EventHandler((sender, e) => AcceptOrReject(sender, e, accepted: false));

            angle.ValueChanged += new EventHandler((sender, e) => Rotate((float)angle.Value));
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

            LoadImage(pimage, cImage);
            LoadImage(pthumb, cThumb);
            LoadImage(pcolor, cColor);
            LoadImage(pprint, cPrint);
            LoadImage(ptrial, cTrial);

            this.baseWidth = pimage.Width;
            this.baseHeight = pimage.Height;

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
                pthumb.Location = new Point(w - pthumb.Width / 2, h - pthumb.Height / 2);
                pimage.Location = new Point(w - pimage.Width / 2, h - pimage.Height / 2);
                ptrial.Location = new Point(w - ptrial.Width / 2, h - ptrial.Height / 2);
                pcolor.Location = new Point((int)(2.5 * w) - pcolor.Width / 2, (int)(0.5 * h) - pcolor.Height / 2);
                pprint.Location = new Point((int)(2.5 * w) - pprint.Width / 2, (int)(1.5 * h) - pprint.Height / 2);
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
                return MessageBox.Show("The image has unsaved changed, do you want to discard the changes?", App.GetName() + " · Discard changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private void Undo()
        {
            SetImage(this.stamps, this.stamp, this.folder, this.country, this.section);
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

            ptrial.Visible = true;
            pimage.Visible = false;
            pthumb.Visible = false;
            pcolor.Visible = false;
            pprint.Visible = false;

            ptrial.Image = new Bitmap(pimage.Image);
            ptrial.Cursor = Cursors.Default;

            angle.Visible = false;

            this.isSelecting = false;

            switch (this.mode)
            {
                case ImagingMode.Rotating:
                    angle.Visible = true;
                    angle.Value = 0.0M;
                    break;
                case ImagingMode.Cropping:
                    ptrial.Cursor = Cursors.Cross;
                    break;
            }

            buttonAccept.Enabled = false;

            SetLocations(this.baseWidth, this.baseHeight);
        }

        private void AcceptOrReject(object sender, EventArgs e, bool accepted)
        {
            if (accepted)
            {
                this.isChanged = true;

                buttonSave.Enabled = true;
                buttonUndo.Enabled = true;

                switch (this.mode)
                {
                    case ImagingMode.Rotating:

                        pimage.Image.Dispose();
                        pimage.Image = new Bitmap(ptrial.Image);

                        pimage.Image.SaveAsJpeg(cImage, 100);

                        ImageHelper.CreateThumbnail(cImage, cThumb, stamp.Width, stamp.Height);

                        LoadImage(pthumb, cThumb);

                        break;
                }
            }

            this.mode = ImagingMode.None;

            labelMode.Text = this.mode.Text();

            panelButtons.Visible = true;
            panelImaging.Visible = false;

            ptrial.Visible = false;
            pimage.Visible = true;
            pthumb.Visible = true;
            pcolor.Visible = true;
            pprint.Visible = true;

            SetLocations(this.baseWidth, this.baseHeight);
        }

        private void Rotate(float value)
        {
            ptrial.Image.Dispose();

            ptrial.Image = pimage.Image.Rotate(value, true, false, Color.Black);

            buttonAccept.Enabled = (value != 0);

            SetLocations(this.baseWidth, this.baseHeight);
        }

    }
}

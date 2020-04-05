using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public partial class Select : Form
    {
        private bool hasChanges = false;

        private Design stamps;
        private DesignEntry stamp;
        private string folder;
        private string country;
        private string section;

        private string image;
        private string color;
        private string print;

        private bool isSelecting;
        private Point selectionStart;
        private Rectangle selection = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0)); // Color.FromArgb(128, 72, 145, 220)

        private ColorStyle colorStyle;

        public Select()
        {
            InitializeComponent();
        }

        public DialogResult SetImage(Design stamps, DesignEntry stamp, string folder, string country, string section)
        {
            this.BackColor = Color.Black;

            buttonPrev.BackColor = Color.White;
            buttonNext.BackColor = Color.White;
            buttonRotate.BackColor = Color.White;
            buttonBrightness.BackColor = Color.White;
            buttonSelect.BackColor = Color.White;
            buttonSave.BackColor = Color.White;
            buttonClose.BackColor = Color.White;
            buttonToggle.BackColor = Color.White;

            buttonZoomIn.BackColor = Color.White;
            buttonZoomOut.BackColor = Color.White;
            buttonReject.BackColor = Color.White;
            buttonAccept.BackColor = Color.White;

            this.stamps = stamps;
            this.stamp = stamp;
            this.folder = folder;
            this.country = country;
            this.section = section;

            string path = string.Format("{0}\\{1}\\{2}", folder, country, section);

            this.image = string.Format("{0}\\image\\{1}.jpg", path, stamp.Number);
            this.color = string.Format("{0}\\xlcolor\\{1}.jpg", path, stamp.Number);
            this.print = string.Format("{0}\\xlprint\\{1}.jpg", path, stamp.Number);

            if (!File.Exists(this.image))
            {
                MessageBox.Show(string.Format("Image '{0}' does not exist!", this.image), App.GetName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return DialogResult.Cancel;
            }

            pbImage.Load(this.image);
            pbImage.SizeMode = PictureBoxSizeMode.AutoSize;

            int width;
            int height = pbImage.Height + panelButtons.Height;

            if (pbImage.Image.Width > panelButtons.Width)
            {
                width = pbImage.Width;
                pbImage.Location = new Point(0, 0);
                panelButtons.Location = new Point((width - panelButtons.Width) / 2, pbImage.Height);
            }
            else
            {
                width = panelButtons.Width;
                pbImage.Location = new Point((panelButtons.Width - pbImage.Width) / 2, 0);
                panelButtons.Location = new Point(0, pbImage.Height);
            }

            panelSelection.Top = panelButtons.Top;
            panelSelection.Left = panelButtons.Left + panelButtons.Width / 2 - panelSelection.Width / 2;

            panelSelection.Enabled = false;
            panelSelection.Visible = false;

            pbColor.Visible = false;
            pbColor.SizeMode = PictureBoxSizeMode.StretchImage;

            if (File.Exists(this.color))
            {
                pbColor.Load(this.color);
                UpdateColorImage();
            }
            else
            {
                pbColor.Image = null;
            }

            pbPrint.Visible = false;
            pbPrint.SizeMode = PictureBoxSizeMode.StretchImage;

            if (File.Exists(this.print))
            {
                pbPrint.Load(this.print);
                UpdatePrintImage();
            }
            else
            {
                pbPrint.Image = null;
            }

            this.colorStyle = ColorStyle.Greyscale;

            UpdateImage();

            buttonSave.Enabled = false;

            buttonPrev.Enabled = this.stamps.FindIndex(s => s.Number == this.stamp.Number) > 0;
            buttonNext.Enabled = this.stamps.FindIndex(s => s.Number == this.stamp.Number) < this.stamps.Count() - 1;

            this.CancelButton = buttonClose;
            this.AcceptButton = buttonSelect;

            buttonSelect.Focus();

            this.isSelecting = false;

            this.Size = new Size(width, height);

            int screenWidthInPixels = Screen.FromControl(this).WorkingArea.Width;
            int screenHeightInPixels = Screen.FromControl(this).WorkingArea.Height;

            this.Location = new Point((screenWidthInPixels - width) / 2, (screenHeightInPixels - height) / 2);

            return DialogResult.OK;
        }

        private void pbImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isSelecting)
            {
                if (e.Button != MouseButtons.Left)
                    return;
                Point selectionEnd = e.Location;
                selection.Location = new Point(
                    Math.Min(selectionStart.X, selectionEnd.X),
                    Math.Min(selectionStart.Y, selectionEnd.Y));
                selection.Size = new Size(
                    Math.Abs(selectionStart.X - selectionEnd.X),
                    Math.Abs(selectionStart.Y - selectionEnd.Y));
                pbImage.Invalidate();
            }
        }

        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.isSelecting)
            {
                // Determine the initial rectangle coordinates...
                selectionStart = e.Location;
                pbImage.Invalidate();
            }
        }

        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.isSelecting)
            {
                buttonAccept.Enabled = true;
            }
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            if (this.isSelecting)
            {
                if (pbImage.Image != null)
                {
                    if (selection != null && selection.Width > 0 && selection.Height > 0)
                    {
                        e.Graphics.FillRectangle(selectionBrush, selection);
                    }
                }
            }
        }

        private void pbPrint_Click(object sender, EventArgs e)
        {
            this.colorStyle = this.colorStyle.Toggle();

            UpdateImage();
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            this.colorStyle = this.colorStyle.Toggle();

            UpdateImage();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                if (SetImage(this.stamps, this.stamps[this.stamps.FindIndex(s => s.Number == this.stamp.Number) + 1], this.folder, this.country, this.section) == DialogResult.Cancel)
                {
                    buttonClose_Click(sender, e);
                }
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                if (SetImage(this.stamps, this.stamps[this.stamps.FindIndex(s => s.Number == this.stamp.Number) - 1], this.folder, this.country, this.section) == DialogResult.Cancel)
                {
                    buttonClose_Click(sender, e);
                }
            }
        }

        private void buttonRotate_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;

            Rotate rotate = new Rotate();

            dialogResult = rotate.SetImage(this.stamp, this.folder, this.country, this.section);

            if (dialogResult == DialogResult.OK)
            {
                dialogResult = rotate.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    this.hasChanges = true;
                    throw new Exception("todo");
                }
            }
        }

        private void buttonToggle_Click(object sender, EventArgs e)
        {
            this.colorStyle = this.colorStyle.Toggle();

            UpdateImage();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            pbImage.Invalidate();
            pbImage.Cursor = Cursors.Cross;
            pbImage.Focus();

            panelButtons.Enabled = false;
            panelButtons.Visible = false;

            panelSelection.Enabled = true;
            panelSelection.Visible = true;

            this.CancelButton = buttonReject;
            this.AcceptButton = buttonAccept;

            buttonAccept.Enabled = false;

            this.isSelecting = true;

            pbColor.Visible = false;
            pbPrint.Visible = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.hasChanges = true;

            pbColor.Image.SaveAsJpeg(this.color, 100);
            pbPrint.Image.SaveAsJpeg(this.print, 100);

            buttonSave.Enabled = false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                this.DialogResult = hasChanges ? DialogResult.OK : DialogResult.Cancel;

                this.Close();
            }
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            Update(accepted: false);
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            Update(accepted: true);
        }

        private DialogResult DiscardChanges()
        {
            if (buttonSave.Enabled)
            {
                return MessageBox.Show("The image has been changed, do you want to discard the changes?", App.GetName() + " · Discard changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                return DialogResult.Yes;
            }
        }

        private void Update(bool accepted)
        {
            pbImage.Invalidate();
            pbImage.Cursor = Cursors.Default;
            pbImage.Focus();

            panelSelection.Enabled = false;
            panelSelection.Visible = false;

            panelButtons.Enabled = true;
            panelButtons.Visible = true;

            this.CancelButton = buttonClose;
            this.AcceptButton = buttonSelect;

            buttonSave.Enabled = accepted;

            this.isSelecting = false;

            if (accepted)
            {
                pbColor.Image = pbImage.Image.GetSelection(this.selection, convertToGrayscale: false);
                UpdateColorImage();

                pbPrint.Image = pbImage.Image.GetSelection(this.selection, convertToGrayscale: true); ;
                UpdatePrintImage();
            }

            this.colorStyle = ColorStyle.Greyscale;

            UpdateImage();
        }

        private void UpdateColorImage()
        {
            pbColor.Width = pbColor.Image.Width * 2 / 3;
            pbColor.Height = pbColor.Image.Height * 2 / 3;
            pbColor.Left = pbImage.Left + pbImage.Width / 2 - pbColor.Width / 2;
            pbColor.Top = pbImage.Top + pbImage.Height / 2 - pbColor.Height / 2;
        }

        private void UpdatePrintImage()
        {
            pbPrint.Width = pbPrint.Image.Width * 2 / 3;
            pbPrint.Height = pbPrint.Image.Height * 2 / 3;
            pbPrint.Left = pbImage.Left + pbImage.Width / 2 - pbPrint.Width / 2;
            pbPrint.Top = pbImage.Top + pbImage.Height / 2 - pbPrint.Height / 2;
        }

        private void UpdateImage()
        {
            pbColor.Visible = false;
            pbPrint.Visible = false;

            if (pbColor.Image != null || pbPrint.Image != null)
            {
                switch (this.colorStyle)
                {
                    case ColorStyle.Color:
                        if (pbColor.Image != null)
                        {
                            pbColor.Visible = true;
                        }
                        else
                        {
                            pbPrint.Visible = true;
                            this.colorStyle = ColorStyle.Greyscale;
                        }
                        break;
                    case ColorStyle.Greyscale:
                        if (pbPrint.Image != null)
                        {
                            pbPrint.Visible = true;
                        }
                        else
                        {
                            pbColor.Visible = true;
                            this.colorStyle = ColorStyle.Color;
                        }
                        break;
                }
            }
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            //pbImage.Image = pbImage.Image.Rotate(5, true, true, Color.Black);
            //pbImage.Width = pbImage.Image.Width;
            //pbImage.Height = pbImage.Image.Height;
            //this.Width = pbImage.Image.Width;
            //Bitmap bitmap = new Bitmap(pbImage.Image);
            //Rectangle r = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            //int alpha = 128;
            //using (Graphics g = Graphics.FromImage(bitmap))
            //{
            //    using (Brush cloud_brush = new SolidBrush(Color.FromArgb(alpha, Color.White)))
            //    {
            //        g.FillRectangle(cloud_brush, r);
            //    }
            //}
            //pbImage.Image = bitmap;
        }
    }
}

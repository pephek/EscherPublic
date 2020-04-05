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
        private string folder;
        private string country;
        private string section;
        private string number;
        private string[] numbers;

        private string image;
        private string color;
        private string print;

        private ColorStyle colorStyle;
        private bool isSelecting;

        private Point selectionStart;
        private Rectangle selection = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0)); // Color.FromArgb(128, 72, 145, 220)

        public Select()
        {
            InitializeComponent();
        }

        public bool SetImage(string folder, string country, string section, string number, string[] numbers)
        {
            this.BackColor = Color.Black;

            buttonPrev.BackColor = Color.White;
            buttonNext.BackColor = Color.White;
            buttonSelect.BackColor = Color.White;
            buttonSave.BackColor = Color.White;
            buttonClose.BackColor = Color.White;
            buttonToggle.BackColor = Color.White;
            buttonZoomIn.BackColor = Color.White;
            buttonZoomOut.BackColor = Color.White;
            buttonReject.BackColor = Color.White;
            buttonAccept.BackColor = Color.White;

            this.folder = folder;
            this.country = country;
            this.section = section;
            this.number = number;
            this.numbers = numbers;

            string path = string.Format("{0}\\{1}\\{2}", folder, country, section);

            this.image = string.Format("{0}\\image\\{1}.jpg", path, number);
            this.color = string.Format("{0}\\xlcolor\\{1}.jpg", path, number);
            this.print = string.Format("{0}\\xlprint\\{1}.jpg", path, number);

            if (!File.Exists(this.image))
            {
                MessageBox.Show(string.Format("Image '{0}' does not exist!", this.image), App.GetName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.Close();

                return false;
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

            UpdateDisplayImage();

            buttonSave.Enabled = false;

            buttonPrev.Enabled = Array.IndexOf(numbers, number) > 0;
            buttonNext.Enabled = Array.IndexOf(numbers, number) < numbers.Length - 1;

            this.CancelButton = buttonClose;
            this.AcceptButton = buttonSelect;

            buttonSelect.Focus();

            this.isSelecting = false;

            this.Size = new Size(width, height);

            int screenWidthInPixels = Screen.FromControl(this).WorkingArea.Width;
            int screenHeightInPixels = Screen.FromControl(this).WorkingArea.Height;

            this.Location = new Point((screenWidthInPixels - width) / 2, (screenHeightInPixels - height) / 2);

            return true;
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

            UpdateDisplayImage();
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            this.colorStyle = this.colorStyle.Toggle();

            UpdateDisplayImage();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                if (!SetImage(this.folder, this.country, this.section, this.numbers[Array.IndexOf(this.numbers, this.number) + 1], this.numbers))
                {
                    this.Close();
                }
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
                if (!SetImage(this.folder, this.country, this.section, this.numbers[Array.IndexOf(this.numbers, this.number) - 1], this.numbers))
                {
                    this.Close();
                }
            }
        }

        private void buttonToggle_Click(object sender, EventArgs e)
        {
            this.colorStyle = this.colorStyle.Toggle();

            UpdateDisplayImage();
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
            ImageHelper.SaveImageAsJpeg(pbColor.Image, this.color);
            ImageHelper.SaveImageAsJpeg(pbPrint.Image, this.print);

            buttonSave.Enabled = false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (DiscardChanges() == DialogResult.Yes)
            {
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
                return MessageBox.Show("The images have been changed, do you want to discard the changes?", App.GetName() + " · Discard changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                pbColor.Image = ImageHelper.GetSelectionFromImage(pbImage.Image, this.selection, convertToGrayscale: false);
                UpdateColorImage();

                pbPrint.Image = ImageHelper.GetSelectionFromImage(pbImage.Image, this.selection, convertToGrayscale: true); ;
                UpdatePrintImage();
            }

            this.colorStyle = ColorStyle.Greyscale;

            UpdateDisplayImage();
        }

        private void UpdateColorImage()
        {
            pbColor.Size = new Size(pbColor.Image.Width * 2 / 3, pbColor.Image.Height * 2 / 3);
            pbColor.Location = new Point((pbImage.Width - pbColor.Width) / 2, (pbImage.Height - pbColor.Height) / 2);
            pbColor.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void UpdatePrintImage()
        {
            pbPrint.Size = new Size(pbPrint.Image.Width * 2 / 3, pbPrint.Image.Height * 2 / 3);
            pbPrint.Location = new Point((pbImage.Width - pbPrint.Width) / 2, (pbImage.Height - pbPrint.Height) / 2);
            pbPrint.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void UpdateDisplayImage()
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
    }
}

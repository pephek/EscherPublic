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
    public partial class Select : Form
    {
        private string image;
        private string color;
        private string print;

        private ColorStyle colorStyle;
        private bool isSelecting;

        public Select()
        {
            InitializeComponent();
        }

        public void SetImage (string folder, string country, string section, string number)
        {
            folder = string.Format("{0}\\{1}\\{2}", folder, country, section);

            this.image = string.Format("{0}\\image\\{1}.jpg", folder, number);
            this.color = string.Format("{0}\\xlcolor\\{1}.jpg", folder, number);
            this.print = string.Format("{0}\\xlprint\\{1}.jpg", folder, number);

            pbImage.Load(this.image);
            pbImage.SizeMode = PictureBoxSizeMode.AutoSize;

            int width;
            int height = pbImage.Height + panelButtons.Height;

            if (pbImage.Image.Width > panelButtons.Width)
            {
                width = pbImage.Width;
                pbImage.Location = new Point(0, 0);
                panelButtons.Location = new Point(width - panelButtons.Width, pbImage.Height);
            }
            else
            {
                width = panelButtons.Width;
                pbImage.Location = new Point((panelButtons.Width - pbImage.Width) / 2, 0);
                panelButtons.Location = new Point(0, pbImage.Height);
            }

            pbColor.Visible = false;
            if (File.Exists(this.color))
            {
                pbColor.Load(this.color);
                UpdateColorImage();
            }

            pbPrint.Visible = false;
            if (File.Exists(this.print))
            {
                pbPrint.Load(this.print);
                UpdatePrintImage();
            }

            this.colorStyle = ColorStyle.Color;

            UpdateDisplayImage();

            buttonSave.Enabled = false;

            this.Size = new Size(width, height);

            int screenWidthInPixels = Screen.FromControl(this).WorkingArea.Width;
            int screenHeightInPixels = Screen.FromControl(this).WorkingArea.Height;

            this.Location = new Point((screenWidthInPixels - width) / 2, (screenHeightInPixels - height) / 2);
        }

        private void pbImage_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            panelButtons.Enabled = false;

            pbColor.Visible = false;
            pbPrint.Visible = false;

            this.Height = pbImage.Height;

            this.isSelecting = true;
        }

        private void buttonToggle_Click(object sender, EventArgs e)
        {
            this.colorStyle = this.colorStyle.Toggle();

            UpdateDisplayImage();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateColorImage()
        {
            pbColor.Size = new Size(pbColor.Image.Width * 2 / 3, pbColor.Image.Height * 2 / 3);
            pbColor.Location = new Point((pbImage.Width - pbColor.Width) / 2, (pbImage.Width - pbColor.Width) / 2);
            pbColor.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void UpdatePrintImage()
        {
            pbPrint.Size = new Size(pbPrint.Image.Width * 2 / 3, pbPrint.Image.Height * 2 / 3);
            pbPrint.Location = new Point((pbImage.Width - pbPrint.Width) / 2, (pbImage.Width - pbPrint.Width) / 2);
            pbPrint.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void UpdateDisplayImage()
        {
            pbColor.Visible = false;
            pbPrint.Visible = false;

            if (pbColor.Image == null && pbPrint.Image == null)
            {
                buttonToggle.Enabled = false;
            }
            else
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

            buttonToggle.Enabled = (pbColor.Image != null && pbPrint.Image != null);
        }
    }
}

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
    public partial class Rotate : Form
    {
        private string folder;
        private string country;
        private string section;
        private string number;
        private float width;
        private float height;

        private string image;
        private string thumbnail;

        private bool isSelecting;
        private Point selectionStart;
        private Rectangle selection = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0)); // Color.FromArgb(128, 72, 145, 220)

        public Rotate()
        {
            InitializeComponent();
        }

        public DialogResult SetImage(string folder, string country, string section, string number, float xwidth, float xheight)
        {
            this.BackColor = Color.Black;

            buttonSelect.BackColor = Color.White;
            buttonClose.BackColor = Color.White;
            buttonSave.BackColor = Color.White;

            buttonZoomIn.BackColor = Color.White;
            buttonZoomOut.BackColor = Color.White;
            buttonReject.BackColor = Color.White;
            buttonAccept.BackColor = Color.White;

            this.folder = folder;
            this.country = country;
            this.section = section;
            this.number = number;
            this.width = xwidth;
            this.height = xheight;

            string path = string.Format("{0}\\{1}\\{2}", folder, country, section);

            this.image = string.Format("{0}\\image\\{1}.jpg", path, number);
            this.thumbnail = string.Format("{0}\\{1}.jpg", path, number);

            if (!File.Exists(this.image))
            {
                MessageBox.Show(string.Format("Image '{0}' does not exist!", this.image), App.GetName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return DialogResult.Cancel;
            }

            pbImage.Load(this.image);
            pbImage.SizeMode = PictureBoxSizeMode.AutoSize;

            int width;
            int height = (int)(1.5 * pbImage.Height) + panelButtons.Height;

            if ((int)(1.5 * pbImage.Image.Width) > panelButtons.Width)
            {
                width = (int)(1.5 * pbImage.Width);
                pbImage.Location = new Point(0, 0);
                panelButtons.Location = new Point((width - panelButtons.Width) / 2, (int)(1.5 * pbImage.Height));
            }
            else
            {
                width = panelButtons.Width;
                pbImage.Location = new Point((panelButtons.Width - (int)(1.5 * pbImage.Width)) / 2, 0);
                panelButtons.Location = new Point(0, (int)(1.5 * pbImage.Height));
            }

            panelSelection.Top = panelButtons.Top;
            panelSelection.Left = panelButtons.Left + panelButtons.Width / 2 - panelSelection.Width / 2;

            panelSelection.Enabled = false;
            panelSelection.Visible = false;

            buttonSave.Enabled = false;

            this.CancelButton = buttonClose;
            this.AcceptButton = buttonSelect;

            buttonSelect.Focus();

            this.isSelecting = false;

            this.Size = new Size(width, height);

            int screenWidthInPixels = Screen.FromControl(this).WorkingArea.Width;
            int screenHeightInPixels = Screen.FromControl(this).WorkingArea.Height;

            this.Location = new Point((screenWidthInPixels - width) / 2, (screenHeightInPixels - height) / 2);

            UpdateImage();

            return DialogResult.OK;
        }

        private void UpdateImage()
        {
            pbImage.Left = pbImage.Width / 4;
            pbImage.Top = pbImage.Height / 4;
        }
    }
}

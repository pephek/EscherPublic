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
        private const float cMultiplier = 2F;

        private bool hasChanges = false;

        private DesignEntry stamp;
        private string folder;
        private string country;
        private string section;

        private string image;
        private string thumb;

        private bool isSelecting;
        private Point selectionStart;
        private Rectangle selection = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0)); // Color.FromArgb(128, 72, 145, 220)

        public Rotate()
        {
            InitializeComponent();
        }

        public DialogResult SetImage(DesignEntry stamp, string folder, string country, string section)
        {
            this.BackColor = Color.Black;

            buttonSelect.BackColor = Color.White;
            buttonClose.BackColor = Color.White;
            buttonSave.BackColor = Color.White;

            buttonZoomIn.BackColor = Color.White;
            buttonZoomOut.BackColor = Color.White;
            buttonReject.BackColor = Color.White;
            buttonAccept.BackColor = Color.White;

            this.stamp = stamp;
            this.folder = folder;
            this.country = country;
            this.section = section;

            string path = string.Format("{0}\\{1}\\{2}", folder, country, section);

            this.image = string.Format("{0}\\image\\{1}.jpg", path, stamp.Number);
            this.thumb= string.Format("{0}\\{1}.jpg", path, stamp.Number);

            if (!File.Exists(this.image))
            {
                MessageBox.Show(string.Format("Image '{0}' does not exist!", this.image), App.GetName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return DialogResult.Cancel;
            }

            pbImage.Load(this.image);
            pbImage.SizeMode = PictureBoxSizeMode.AutoSize;

            int width;
            int height = (int)(cMultiplier * pbImage.Height) + panelButtons.Height;

            if ((int)(cMultiplier * pbImage.Image.Width) > panelButtons.Width)
            {
                width = (int)(cMultiplier * pbImage.Width);
                panelButtons.Location = new Point((width - panelButtons.Width) / 2, (int)(cMultiplier * pbImage.Height));
            }
            else
            {
                width = panelButtons.Width;
                panelButtons.Location = new Point(0, (int)(cMultiplier * pbImage.Height));
            }

            panelSelection.Top = panelButtons.Top;
            panelSelection.Left = panelButtons.Left + panelButtons.Width / 2 - panelSelection.Width / 2;

            panelSelection.Enabled = false;
            panelSelection.Visible = false;

            pbThumb.Visible = false;
            pbThumb.SizeMode = PictureBoxSizeMode.Normal;

            if (File.Exists(this.thumb))
            {
                pbThumb.Load(this.thumb);
            }
            else
            {
                pbThumb.Image = null;
            }

            buttonSave.Enabled = false;

            this.CancelButton = buttonClose;
            this.AcceptButton = buttonSelect;

            buttonSelect.Focus();

            this.isSelecting = false;

            this.Size = new Size(width, height);

            int screenWidthInPixels = Screen.FromControl(this).WorkingArea.Width;
            int screenHeightInPixels = Screen.FromControl(this).WorkingArea.Height;

            this.Location = new Point((screenWidthInPixels - width) / 2, (screenHeightInPixels - height) / 2);

            UpdateImages();

            return DialogResult.OK;
        }

        private void UpdateImages()
        {
            pbImage.Left = (this.Width - pbImage.Width) / 2;
            pbImage.Top = ((this.Height - panelButtons.Height) - pbImage.Height) / 2;

            if (pbThumb.Image != null)
            {
                pbThumb.Visible = true;
                pbThumb.Left = pbImage.Left + pbImage.Width / 2 - pbThumb.Width / 2;
                pbThumb.Top = pbImage.Top + pbImage.Height / 2 - pbThumb.Height / 2;
            }
        }

        private void udAngle_ValueChanged(object sender, EventArgs e)
        {
            pbImage.Image.Dispose();

            pbImage.Image = Image.FromFile(this.image).Rotate((float)udAngle.Value, true, false, Color.Black);

            UpdateImages();

            buttonSave.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.hasChanges = true;

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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public partial class Preview : Form
    {
        private Page page;
        private PrintMode mode;
        private int number;
        private float scale;
        private bool designing;
        private Artifacts artifacts;

        public Preview()
        {
            InitializeComponent();
        }

        private void Preview_Load(object sender, EventArgs e)
        {
            labelCaption.Left = 3;
            labelCaption.Top = 3;
            labelLegend.Left = labelCaption.Left;
            labelLegend.Top = labelCaption.Top + labelCaption.Height + 3;
        }

        private void Preview_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    this.Close();
                    break;
                default:
                    switch (e.KeyChar.ToString().ToLower())
                    {
                        case "p":
                            ShowPageSetup();
                            break;
                    }
                    break;
            }
        }

        private void Preview_Paint(object sender, PaintEventArgs e)
        {
            ShowPage(artifacts, scale, designing);
        }

        private void ShowPageSetup()
        {
            Print print = new Print();
            print.printMode = PrintMode.ToScreen;
            DialogResult result = print.ShowDialog();

            if (result == DialogResult.OK)
            {
                PageSetup.Load();

                ShowPage(page, mode, number, scale, designing);
            }
        }

        public void ShowPage(Artifacts artifacts, float scale, bool designing)
        {
            if (artifacts.Count() == 0)
            {
                return;
            }

            using (Graphics g = this.CreateGraphics())
            {
                float currentX = 0;
                float currentY = 0;

                foreach (Artifact artifact in artifacts)
                {
                    artifact.X *= scale;
                    artifact.Y *= scale;
                    artifact.Width *= scale;
                    artifact.Height *= scale;
                    artifact.FontSize *= scale;

                    switch (artifact.Type)
                    {
                        case ArtifactType.Cursor:
                            currentX = artifact.X;
                            currentY = artifact.Y;
                            break;

                        case ArtifactType.MoveSolid:
                            g.DrawLine(artifact.ForeColor, currentX, currentY, currentX + artifact.Width, currentY + artifact.Height);
                            currentX += artifact.Width;
                            currentY += artifact.Height;
                            break;
                    }
                }
            }
        }

        public void ShowPage(Page page, PrintMode mode, int number, float scale, bool designing)
        {
            this.page = page;
            this.mode = mode;
            this.number = number;
            this.scale = scale;
            this.designing = designing;

            PageSetup setup = PageSetup.Get();

            Pen red = new Pen(Color.Red);
            Pen gray = new Pen(Color.Gray);
            Pen black = new Pen(Color.Black);
            Pen white = new Pen(Color.White);

            this.FormBorderStyle = FormBorderStyle.None;

            int screenW = Screen.FromControl(this).WorkingArea.Width;
            int screenH = Screen.FromControl(this).WorkingArea.Height;

            if (setup.PageFormat.PageHeight > setup.PageFormat.PageWidth)
            {
                this.Height = screenH;
                this.Width = setup.PageFormat.PageWidth * screenH / setup.PageFormat.PageHeight;
            }
            else
            {
                this.Width = screenW;
                this.Height = setup.PageFormat.PageHeight * screenW / setup.PageFormat.PageWidth;
            }

            this.Left = 0;
            this.Top = 0;

            artifacts = new Artifacts();

            artifacts.AddRectangle(0, 0, this.Width - 1, this.Height - 1, red);
            artifacts.AddRectangle(10, 10, 300, 300, red);
        }
    }
}

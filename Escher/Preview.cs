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
        private float scaleX;
        private float scaleY;
        private Artifacts artifacts;

        public Preview()
        {
            InitializeComponent();
        }

        private void Preview_Load(object sender, EventArgs e)
        {
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
            PaintPage(this.artifacts, this.scaleX, this.scaleY, this.mode);
        }

        private void ShowPageSetup()
        {
            Print print = new Print();
            print.printMode = PrintMode.ToScreen;
            DialogResult result = print.ShowDialog();

            if (result == DialogResult.OK)
            {
                PageSetup.Load();

                PreparePage(page, mode, number);
            }
        }

        public void PaintPage(Artifacts artifacts, float scaleX, float scaleY, PrintMode mode)
        {
            if (artifacts.Count() == 0)
            {
                return;
            }

            using (Graphics g = this.CreateGraphics())
            {
                PaintPage(g, artifacts, scaleX, scaleY, mode);
            }
        }

        public void PaintPage(Graphics g, Artifacts artifacts, float scaleX, float scaleY, PrintMode mode)
        {
            float currentX = 0;
            float currentY = 0;

            foreach (Artifact artefact in artifacts.Get())
            {
                if (mode == PrintMode.ToDocument && artefact.screenOnly)
                {
                    continue;
                }

                Artifact artifact = artefact.GetScaledCopy(scaleX, scaleY);

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

                    case ArtifactType.Text:
                        g.DrawString(artifact.Text, artifact.Font, artifact.TextColor, artifact.X, artifact.Y);
                        break;
                }
            }
        }

        public void PreparePage(Page page, PrintMode mode, int number)
        {
            this.page = page;
            this.mode = mode;
            this.number = number;

            PageSetup setup = PageSetup.Get();
            PageFormat format = setup.PageFormat;

            Pen red = new Pen(Color.Red);
            Pen gray = new Pen(Color.Gray);
            Pen black = new Pen(Color.Black);
            Pen white = new Pen(Color.White);

            SolidBrush redText = new SolidBrush(Color.Red);
            SolidBrush grayText = new SolidBrush(Color.Gray);
            SolidBrush blackText = new SolidBrush(Color.Black);
            SolidBrush whiteText = new SolidBrush(Color.White);

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

            this.scaleX = (float)this.Width / setup.PageFormat.PageWidth;
            this.scaleY = (float)this.Height / setup.PageFormat.PageHeight;

            using (Graphics g = this.CreateGraphics())
            {
                g.PageUnit = GraphicsUnit.Millimeter;

                artifacts = new Artifacts(g, this.scaleX, this.scaleX);

                // Form border
                artifacts.AddRectangle(0, 0, (this.Width - 1) / scaleX, (this.Height - 1) / scaleY, gray, screenOnly: true);

                // Form caption
                artifacts.AddText(1, 1, "Escher · Preview", blackText, "Microsoft Sans Serif", 8, screenOnly: true);

                // Legenda
                artifacts.AddText(1, artifacts.Last().Bottom(2), "c = ± color", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "n = ± number", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "v = ± value", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "f = ± frame", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "t = ± title", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "s = ± font", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(2), "p = setup", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(2), "- = previous", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "+ = next", grayText, "Courier New", 8, screenOnly: true);

                // Page border
                artifacts.AddRectangle(format.Border.Left, format.Border.Top, format.Border.Width, format.Border.Height, gray, screenOnly: true);

                // Free space
                artifacts.AddRectangle(format.Free.Left, format.Free.Top, format.Free.Width, format.Free.Height, red, screenOnly: true);
            }
        }
    }
}

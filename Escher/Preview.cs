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
            PaintPage(this.artifacts, this.scale, this.mode);
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

        public void PaintPage(Artifacts artifacts, float scale, PrintMode mode)
        {
            if (artifacts.Count() == 0)
            {
                return;
            }

            using (Graphics g = this.CreateGraphics())
            {
                PaintPage(g, artifacts, scale, mode);
            }
        }

        public void PaintPage(Graphics g, Artifacts artifacts, float scale, PrintMode mode)
        {
            Pen pen;

            float currentX = 0;
            float currentY = 0;

            foreach (Artifact artefact in artifacts.Get())
            {
                if (mode == PrintMode.ToDocument && artefact.screenOnly)
                {
                    continue;
                }

                Artifact artifact = artefact.GetScaledCopy(scale);

                switch (artifact.Type)
                {
                    case ArtifactType.Cursor:
                        currentX = artifact.X;
                        currentY = artifact.Y;
                        break;

                    case ArtifactType.MoveSolid:
                        pen = new Pen(artifact.ForeColor);
                        g.DrawLine(pen, currentX, currentY, currentX + artifact.Width, currentY + artifact.Height);
                        currentX += artifact.Width;
                        currentY += artifact.Height;
                        break;

                    case ArtifactType.MoveDotted:
                        pen = new Pen(artifact.ForeColor);
                        pen.DashPattern = new float[] { 1, 2};
                        g.DrawLine(pen, currentX, currentY, currentX + artifact.Width, currentY + artifact.Height);
                        currentX += artifact.Width;
                        currentY += artifact.Height;
                        break;

                    case ArtifactType.Text:
                        g.DrawString(artifact.Text, artifact.Font, artifact.TextColor, artifact.X, artifact.Y);
                        break;

                    default:
                        throw new Exception(string.Format("Artifact type '{0}' not yet implemented", artifact.Type));
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

            if (setup.IncludeMarginForPunchHoles)
            {
                float additionalMarginLeft = 12.5f;

                format = new PageFormat(format.FormatName, format.TitleStyle, format.TitleFont, format.PageWidth, format.PageHeight, format.MarginLeft + additionalMarginLeft, format.MarginRight, format.MarginTop, format.MarginBottom, format.FreeLeft, format.FreeRight, format.FreeTop, format.FreeBottom, format.PrePrintedBorder, format.PrePrintedTitle);
            }

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
                this.Width = (int)(setup.PageFormat.PageWidth * screenH / setup.PageFormat.PageHeight);
            }
            else
            {
                this.Width = screenW;
                this.Height = (int)(setup.PageFormat.PageHeight * screenW / setup.PageFormat.PageWidth);
            }

            this.Left = 0;
            this.Top = 0;

            float scaleX = (float)this.Width / setup.PageFormat.PageWidth;
            float scaleY = (float)this.Height / setup.PageFormat.PageHeight;
            this.scale = (scaleX + scaleY) / 2;

            using (Graphics g = this.CreateGraphics())
            {
                g.PageUnit = GraphicsUnit.Millimeter;

                artifacts = new Artifacts(g, this.scale);

                // Form caption
                artifacts.AddText(1, 1, "Escher · Preview", grayText, "Microsoft Sans Serif", 8, screenOnly: true);

                // Legenda
                artifacts.AddText(1, artifacts.Last().Bottom(2), "c = ± color", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "n = ± number", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "v = ± value", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "f = ± frame", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "t = ± title", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "s = ± font", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(2), "p = page setup", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(2), "- = previous", grayText, "Courier New", 8, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), "+ = next", grayText, "Courier New", 8, screenOnly: true);

                // Form border
                artifacts.AddRectangle(0, 0, (this.Width - 1) / scale, (this.Height - 1) / scale, Color.Gray, screenOnly: true);

                // Page border
                artifacts.AddRectangle(format.Border.Left, format.Border.Top, format.Border.Width, format.Border.Height, Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);

                // Free space
                artifacts.AddRectangle(format.Free.Left, format.Free.Top, format.Free.Width, format.Free.Height, Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);

                // Mids of the free space
                artifacts.AddHorizontalLine(format.Free.Left, format.Free.Top + format.Free.Height / 2, format.Free.Width, Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);
                artifacts.AddVerticalLine(format.Free.Left + format.Free.Width / 2, format.Free.Top, format.Free.Height, Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);

                // Punch holes
                if (setup.IncludeMarginForPunchHoles)
                {
                    artifacts.AddRectangle(format.Border.Left / 2 - 2.5f, format.Border.Top, 5f, format.Border.Height, Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);
                }
            }
        }
    }
}

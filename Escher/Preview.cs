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
            PaintPage(this.artifacts, this.mode, this.scaleX, this.scaleY, this.designing);
        }

        private void ShowPageSetup()
        {
            Print print = new Print();
            print.printMode = PrintMode.ToScreen;
            DialogResult result = print.ShowDialog();

            if (result == DialogResult.OK)
            {
                PageSetup.Load();

                PreparePage(page, mode, number, designing);
            }
        }

        public void PaintPage(Artifacts artifacts, PrintMode mode, float scaleX, float scaleY, bool designing)
        {
            if (artifacts.Count() == 0)
            {
                return;
            }

            using (Graphics g = this.CreateGraphics())
            {
                PreviewOrPrintPage(g, artifacts, scaleX, scaleY);
            }
        }

        public void PreviewOrPrintPage(Graphics g, Artifacts artifacts, float scaleX, float scaleY)
        {
            float currentX = 0;
            float currentY = 0;

            foreach (Artifact artifact in artifacts)
            {
                artifact.X *= scaleX;
                artifact.Y *= scaleY;
                artifact.Width *= scaleX;
                artifact.Height *= scaleY;
                artifact.FontSize *= scaleX;

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

        public void PreparePage(Page page, PrintMode mode, int number, bool designing)
        {
            this.page = page;
            this.mode = mode;
            this.number = number;
            this.designing = designing;

            PageSetup setup = PageSetup.Get();

            Pen red = new Pen(Color.Red);
            Pen gray = new Pen(Color.Gray);
            Pen black = new Pen(Color.Black);
            Pen white = new Pen(Color.White);

            this.FormBorderStyle = FormBorderStyle.None;

            //using (Graphics g = this.CreateGraphics())
            //{
            //    var x = g.PageScale;
            //    var y = g.PageUnit;
            //    //g.PageUnit = GraphicsUnit.Millimeter;
            //    g.PageUnit = GraphicsUnit.Pixel;
            //    int w = Screen.FromControl(this).WorkingArea.Width;
            //    int h = Screen.FromControl(this).WorkingArea.Height;
            //    var mmW = (w * 25.4) / g.DpiX;
            //    var mmH = (h * 25.4) / g.DpiY;
            //}

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

            this.scaleX = (float)this.Width / setup.PageFormat.PageWidth;
            this.scaleY = (float)this.Height / setup.PageFormat.PageHeight;

            this.Left = 0;
            this.Top = 0;

            artifacts = new Artifacts();

            //artifacts.AddRectangle(0, 0, this.Width - 1, this.Height - 1, red);
            artifacts.AddRectangle(0, 0, (this.Width - 1) / scaleX, (this.Height - 1) / scaleY, red);
            artifacts.AddRectangle(10, 0, (int)(setup.PageFormat.PageWidth / 2), (int)(setup.PageFormat.PageHeight / 2), black);
        }
    }
}

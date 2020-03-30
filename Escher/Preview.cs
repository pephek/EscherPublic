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
                //DrawStringWithCharacterBounds(g, "!%Nederland", new Font("Darleston", 50), new Rectangle(500, 500, 500, 500));
                //HighlightACharacterRange1(g);
                //HighlightACharacterRange2(g);
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
                        g.DrawRectangle(new Pen(Color.Red), artifact.X, artifact.Y, artifact.Width, artifact.Height);
                        break;

                    default:
                        throw new Exception(string.Format("Artifact type '{0}' not yet implemented", artifact.Type));
                }
            }
        }

        private void DrawStringWithCharacterBounds(Graphics gr, string text, Font font, Rectangle rect)
        {
            using (StringFormat string_format = new StringFormat())
            {
                //string_format.Alignment = StringAlignment.Center;
                //string_format.LineAlignment = StringAlignment.Center;

                // Draw the string.
                //gr.DrawString(text, font, Brushes.Blue, 10, 50);
                gr.DrawRectangle(new Pen(Color.Green), rect);
                gr.DrawString(text, font, Brushes.Blue, rect, string_format);

                // Make a CharacterRange for the string's characters.
                List<CharacterRange> range_list = new List<CharacterRange>();
                for (int i = 0; i < text.Length; i++)
                {
                    range_list.Add(new CharacterRange(i, 1));
                }
                string_format.SetMeasurableCharacterRanges(range_list.ToArray());

                // Measure the string's character ranges.
                Region[] regions = gr.MeasureCharacterRanges(text, font, rect, string_format);

                // Draw the character bounds.
                for (int i = 0; i < text.Length; i++)
                {
                    Rectangle char_rect = Rectangle.Round(regions[i].GetBounds(gr));
                    gr.DrawRectangle(Pens.Red, char_rect);
                }
            }
        }

        private void HighlightACharacterRange1(Graphics g)
        {

            // Declare the string to draw.
            string message = "!%Nederland";

            // Declare the word to highlight.
            string searchWord = "This is the string to highlight a word in."; //"string";

            // Create a CharacterRange array with the searchWord 
            // location and length.
            //CharacterRange[] ranges = new CharacterRange[]{new CharacterRange (message.IndexOf(searchWord), searchWord.Length)};
            CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, message.Length) };

            // Construct a StringFormat object.
            StringFormat stringFormat = new StringFormat();

            // Set the ranges on the StringFormat object.
            stringFormat.SetMeasurableCharacterRanges(ranges);

            // Declare the font to write the message in.
            //Font largeFont = new Font("Courier New", 50F);//, GraphicsUnit.Pixel);
            Font font = new Font("Darleston", 50F);//, GraphicsUnit.Pixel);

            // Construct a new Rectangle.
            Rectangle displayRectangle = new Rectangle(100, 200, 500, 100);

            // Convert the Rectangle to a RectangleF.
            RectangleF displayRectangleF = (RectangleF)displayRectangle;

            // Get the Region to highlight by calling the 
            // MeasureCharacterRanges method.
            Region[] charRegion = g.MeasureCharacterRanges(message, font, displayRectangleF, stringFormat);

            // Draw the message string on the form.
            g.DrawString(message, font, Brushes.Blue, displayRectangleF);

            // Fill in the region using a semi-transparent color.
            g.FillRegion(new SolidBrush(Color.FromArgb(50, Color.Fuchsia)), charRegion[0]);

            RectangleF r = charRegion[0].GetBounds(g);
            g.DrawRectangle(new Pen(Color.Green), r.Left, r.Top, r.Width, r.Height);

            SizeF size = g.MeasureString(message, font, 2000, StringFormat.GenericTypographic);

            g.DrawRectangle(new Pen(Color.Red), 100, 200, size.Width, size.Height);

            font.Dispose();
        }

        private void HighlightACharacterRange2(Graphics g)
        {

            // Declare the string to draw.
            string message = "Escher · Preview";

            // Declare the word to highlight.
            string searchWord = "This is the string to highlight a word in."; //"string";

            // Create a CharacterRange array with the searchWord 
            // location and length.
            //CharacterRange[] ranges = new CharacterRange[]{new CharacterRange (message.IndexOf(searchWord), searchWord.Length)};
            CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, message.Length) };

            // Construct a StringFormat object.
            StringFormat stringFormat = new StringFormat();

            // Set the ranges on the StringFormat object.
            stringFormat.SetMeasurableCharacterRanges(ranges);

            // Declare the font to write the message in.
            //Font largeFont = new Font("Courier New", 50F);//, GraphicsUnit.Pixel);
            Font font = new Font("Microsoft Sans Serif", 8F);//, GraphicsUnit.Pixel);

            // Construct a new Rectangle.
            Rectangle displayRectangle = new Rectangle(100, 400, 500, 100);

            // Convert the Rectangle to a RectangleF.
            RectangleF displayRectangleF = (RectangleF)displayRectangle;

            // Get the Region to highlight by calling the 
            // MeasureCharacterRanges method.
            Region[] charRegion = g.MeasureCharacterRanges(message, font, displayRectangleF, stringFormat);

            // Draw the message string on the form.
            g.DrawString(message, font, Brushes.Blue, displayRectangleF);

            // Fill in the region using a semi-transparent color.
            g.FillRegion(new SolidBrush(Color.FromArgb(50, Color.Fuchsia)), charRegion[0]);

            RectangleF r = charRegion[0].GetBounds(g);
            g.DrawRectangle(new Pen(Color.Green), r.Left, r.Top, r.Width, r.Height);

            SizeF size = g.MeasureString(message, font, 2000, StringFormat.GenericTypographic);

            g.DrawRectangle(new Pen(Color.Red), 100, 200, size.Width, size.Height);

            font.Dispose();
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
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                artifacts = new Artifacts(g, this.scale);

                // Form caption
                artifacts.AddText(1, 1, 0, "Escher · Preview", "Microsoft Sans Serif", 8, foreColor: Color.Gray, screenOnly: true);

                // Legenda
                artifacts.AddText(1, artifacts.Last().Bottom(2), 0, "c = ± color", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), 0, "n = ± number", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), 0, "v = ± value", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), 0, "f = ± frame", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), 0, "t = ± title", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), 0, "s = ± font", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(2), 0, "p = page setup", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(2), 0, "- = previous", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), 0, "+ = next", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);

                // Form border
                artifacts.AddRectangle(0, 0, (this.Width - 1) / scale, (this.Height - 1) / scale, Color.Gray, screenOnly: true);

                // Page border
                artifacts.AddRectangle(format.Border.Left, format.Border.Top, format.Border.Width, format.Border.Height, Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);

                // Free space
                artifacts.AddRectangle(format.Free.Left, format.Free.Top, format.Free.Width, format.Free.Height, Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);

                // Mids of the free space
                artifacts.AddHorizontalLine(format.Free.Left, format.Free.Top + format.Free.Height / 2, format.Free.Width, foreColor: Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);
                artifacts.AddVerticalLine(format.Free.Left + format.Free.Width / 2, format.Free.Top, format.Free.Height, foreColor: Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);

                // Punch holes
                if (setup.IncludeMarginForPunchHoles)
                {
                    artifacts.AddRectangle(format.Border.Left / 2 - 2.5f, format.Border.Top, 5f, format.Border.Height, Color.Gray, frameStyle: FrameStyle.ThinDotted, screenOnly: true);
                }

                // Border
                if (setup.IncludeBorder && !string.IsNullOrEmpty(page.AlbumNumber))
                {
                    artifacts.AddHorizontalLine(format.Border.Left, format.Border.Top, format.Border.Width);
                    artifacts.AddHorizontalLine(format.Border.Left, format.Border.Bottom, format.Border.Width);
                }

                // Copyright statement
                if (!string.IsNullOrEmpty(page.Copyright))
                {
                    artifacts.AddText(format.Border.Left, format.Border.Bottom + 2, 0, page.Copyright, format.TitleFont, 7, foreColor: Color.Gray);
                }

                // Album number
                if (!string.IsNullOrEmpty(page.AlbumNumber))
                {
                    artifacts.AddText(format.Border.Left, format.Border.Bottom + 2, format.Border.Width, page.AlbumNumber, format.TitleFont, 7, alignment: Alignment.Right);
                }

                // Country
                if (!string.IsNullOrEmpty(page.Title) && string.IsNullOrEmpty(page.AlbumNumber)) // Must be the front page
                {
                    if (page.Title.Contains("<br>"))
                    {
                        artifacts.AddText(format.Free.Left, format.Free.Top, format.Free.Width, page.Title.Split("<br>")[0], "Darleston", 50, alignment: Alignment.Centered);
                        artifacts.AddText(format.Free.Left, artifacts.Last().Bottom(), format.Free.Width, page.Title.Split("<br>")[1], "Darleston", 25, alignment: Alignment.Centered);
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
        }
    }
}

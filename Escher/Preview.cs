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
                        g.DrawRectangle(new Pen(Color.Red), artifact.X, artifact.Y, artifact.Width, artifact.Height);
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

            float y = format.Free.Top;

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
                artifacts.AddText(1, artifacts.Last().Bottom(2), 0, char.ConvertFromUtf32(0x2191) + " = next", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);
                artifacts.AddText(1, artifacts.Last().Bottom(), 0, char.ConvertFromUtf32(0x2193) + " = previous", "Courier New", 8, foreColor: Color.Gray, screenOnly: true);

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
                        artifacts.AddText(format.Free.Left, format.Free.Top, format.Free.Width, page.Title, "Darleston", 50, alignment: Alignment.Centered);
                    }
                }
                else
                {
                    switch (format.TitleStyle)
                    {
                        case TitleStyle.Davo:
                            if (setup.IncludeTitle)
                            {
                                y += artifacts.AddText(format.Free.Left, y, format.Free.Width, page.Country, format.TitleFont, 18, fontBold: true, foreColor: Color.Black, alignment: Alignment.Centered);
                            }
                            else
                            {
                                y += artifacts.AddText(format.Free.Left, y, format.Free.Width, page.Country, format.TitleFont, 18, fontBold: true, foreColor: Color.Gray, alignment: Alignment.Centered, screenOnly: true);
                            }
                            break;
                        case TitleStyle.Importa:
                            if (setup.IncludeTitle)
                            {
                                if (!string.IsNullOrEmpty(page.Title))
                                {
                                    artifacts.AddText(format.Border.Left, format.Border.Top - artifacts.GetTextHeight("Darleston", 20) - 1F, format.Border.Width, page.Title.Split("<br>")[0].Replace("!%", "").Replace("%", ""), "Darleston", 20, alignment: Alignment.Right);
                                }
                                else
                                {
                                    artifacts.AddText(format.Border.Left, format.Border.Top - artifacts.GetTextHeight(format.TitleFont, 8) - 2.5F, format.Border.Width, page.Country, format.TitleFont, 8, alignment: Alignment.Right);
                                }
                            }
                            else
                            {
                                artifacts.AddText(format.Border.Left, format.Border.Top - artifacts.GetTextHeight(format.TitleFont, 8) - 2.5F, format.Border.Width, page.Country, format.TitleFont, 8, foreColor: Color.Gray, alignment: Alignment.Right, screenOnly: true);
                            }
                            break;
                    }

                }

                bool printPage = (mode == PrintMode.ToScreen) || (mode == PrintMode.ToDocument && !setup.IncludeSamplePagesOnly) || (mode == PrintMode.ToDocument && setup.IncludeSamplePagesOnly && page.IsSample);

                if (!printPage)
                {
                    artifacts.AddText(format.Free.Left, y, format.Free.Width, "DIT ALBUMBLAD IS NIET OPGENOMEN IN HET VOORBEELD DOCUMENT!%%THIS ALBUM PAGE IS NOT INCLUDED IN THE SAMPLES DOCUMENT!", format.TitleFont, 12, fontBold: true, foreColor: Color.Red, alignment: Alignment.Centered);
                }
                else
                {
                    // Eg. 1867-1869. Koning Willem III.
                    y += artifacts.AddText(format.Free.Left, y, format.Free.Width, string.IsNullOrEmpty(page.Series) ? "·" : page.Series, format.TitleFont, 7, foreColor: string.IsNullOrEmpty(page.Series) ? Color.White : Color.Black, alignment: Alignment.Centered);

                    // Eg. Type I.
                    y += artifacts.AddText(format.Free.Left, y, format.Free.Width, string.IsNullOrEmpty(page.MainType) ? "" : page.MainType, format.TitleFont, 7, alignment: Alignment.Centered);

                    float pageMargin = page.Margin;
                    float yCombine;

                    for (int v = 0; v < page.Varieties.Count(); v++)
                    {
                        Varieties varieties = page.Varieties[v];

                        // When spacing is not set at the varieties level then take over the page wide spacing
                        if (varieties.Margin == 0)
                        {
                            varieties.Margin = page.Margin;
                        }

                        // When varieties are combined then remember this y position
                        yCombine = y;

                        if (!string.IsNullOrEmpty(varieties.SubType))
                        {
                            y += artifacts.AddText(format.Free.Left, y + artifacts.GetTextHeight(format.TitleFont, 9), format.Free.Width, varieties.SubType, format.TitleFont, 7, alignment: Alignment.Centered);

                            yCombine = y;
                        }

                        int n = varieties.Rows.Count() - 1;
                        int a = 0;
                        int b = 0;

                        while (page.RowWidth(v, n, format.Free.Width, out a) > format.Free.Width && varieties.Rows[n].Count() > 1)
                        {
                            throw new Exception("TODO");
                        }

                        // Eg.A - Kamtanding 12¾ : 11¾.
                        float fontSize = varieties.FontOfDescription ? 5 : 7;

                        string text = varieties.PublicDescription;
                        Alignment alignment = varieties.Alignment;

                        if (setup.IncludeNumber && !string.IsNullOrEmpty(varieties.PrivateDescription))
                        {
                            text = varieties.PrivateDescription;
                            alignment = Alignment.Centered;
                        }

                        float textWidth = artifacts.GetTextWidth(text, format.TitleFont, fontSize);
                        float rowWidth = page.RowWidth(v, 0);
                        float rowLeft = page.RowLeft(v, 0, format.Free.Left, format.Free.Width);

                        if (textWidth <= rowWidth)
                        {
                            y += artifacts.AddText(rowLeft + varieties.Horizontal, y + varieties.Vertical, rowWidth, text, format.TitleFont, fontSize, alignment: alignment);
                        }
                        else
                        {
                            y += artifacts.AddText(rowLeft + varieties.Horizontal - textWidth, y + varieties.Vertical, rowWidth + 2 * textWidth, text, format.TitleFont, fontSize, alignment: alignment);
                        }

                        for (int r = 0; r < varieties.Rows.Count(); r++)
                        {
                            List<Variety> row = varieties.Rows[r];

                            for (int s = 0; s < row.Count(); s++)
                            {
                                Variety variety = row[s];

                                variety.FrameLeft = page.FrameLeft(v, r, s, format.Free.Left, format.Free.Width);
                                variety.FrameOffset = page.FrameOffset(v, r, s);
                                variety.FrameWidth = page.FrameWidth(v, r, s);
                            }

                            bool hasDescriptions = false;

                            for (int s = 0; s < row.Count(); s++)
                            {
                                Variety variety = row[s];

                                if (!string.IsNullOrEmpty(variety.Title))
                                {
                                    y -= 1; // As soon as we found a description then move the y-position 1 millimeter up

                                    // Eg. Zonder punt in linker onderhoek.
                                    y += artifacts.AddText(variety.FrameLeft, y + variety.Vertical, variety.FrameWidth, variety.Title, format.TitleFont, 5, alignment: variety.Alignment);

                                    // There is at least one stamp with a description
                                    hasDescriptions = true;
                                }
                            }

                            if (hasDescriptions) // Now move the y-position 1 millimeter down
                            {
                                y += 1;
                            }
                        }
                    }
                }
            }
        }
    }
}

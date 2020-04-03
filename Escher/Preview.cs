﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public partial class Preview : Form
    {
        private Design design;
        private Page page;
        private PrintMode printMode;
        private ScreenMode screenMode;
        private int pageNumber;
        private float pageScale;
        private float transformScale;
        private Artifacts artifacts;
        private float dpiX;
        private float dpiY;

        private bool mouseDown;
        private Point mouseLastLocation;

        public Preview()
        {
            InitializeComponent();
        }

        public void SetPreview(Design design, int pageNumber, PrintMode printMode, ScreenMode screenMode)
        {
            this.design = design;

            this.page = PageHelper.Get(design, pageNumber);

            this.pageNumber = pageNumber;
            this.printMode = printMode;
            this.screenMode = screenMode;

            using (Graphics graphics = this.CreateGraphics())
            {
                this.dpiX = graphics.DpiX;
                this.dpiY = graphics.DpiY;
            }

            ResizePreview(PageSetup.Get().PageFormat, this.printMode, this.screenMode, out this.pageScale, out this.transformScale);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Location = new Point(0, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RefreshPreview(resizePreview: false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Left:
                    if (this.pageNumber > 1)
                    {
                        this.pageNumber--;
                        this.page = PageHelper.Get(design, pageNumber);
                        RefreshPreview(resizePreview: false);
                    }
                    break;
                case Keys.Right:
                    if (this.pageNumber < this.design.NumberOfPages())
                    {
                        this.pageNumber++;
                        this.page = PageHelper.Get(design, pageNumber);
                        RefreshPreview(resizePreview: false);
                    }
                    break;
                case Keys.P:
                    ShowPageSetup();
                    break;
                case Keys.Oemplus:
                    switch (e.Modifiers)
                    {
                        case Keys.Shift:
                            this.screenMode = this.screenMode.Next();
                            this.Location = new Point(0, 0);
                            RefreshPreview(resizePreview: true);
                            break;
                        case Keys.None:
                            this.Location = new Point(0, 0);
                            break;
                    }
                    break;
                case Keys.OemMinus:
                    this.screenMode = this.screenMode.Prev();
                    this.Location = new Point(0, 0);
                    RefreshPreview(resizePreview: true);
                    break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.mouseDown = true;
            this.mouseLastLocation = e.Location;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - this.mouseLastLocation.X) + e.X, (this.Location.Y - this.mouseLastLocation.Y) + e.Y);

                this.Update();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.mouseDown = false;
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            this.Location = new Point(0, 0);
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            RefreshPreview(resizePreview: false);
        }

        private void ShowPageSetup()
        {
            string paper = PageSetup.Get().PageFormat.FormatName;

            Print print = new Print();
            print.printMode = PrintMode.ToScreen;
            DialogResult result = print.ShowDialog();

            if (result == DialogResult.OK)
            {
                PageSetup.Load();

                RefreshPreview(resizePreview: paper != PageSetup.Get().PageFormat.FormatName);
            }
        }

        private void RefreshPreview(bool resizePreview)
        {
            if (resizePreview)
            {
                if (this.screenMode == ScreenMode.MatchScreenWidth)
                {
                    vScrollBar.Value = 0;
                }
                ResizePreview(PageSetup.Get().PageFormat, this.printMode, this.screenMode, out this.pageScale, out this.transformScale);
            }

            using (Graphics g = this.CreateGraphics())
            {
                g.Clear(Color.White);

                GraphicsUnit pageUnit = g.PageUnit;

                g.PageUnit = GraphicsUnit.Millimeter;

                AssemblePreview(g, this.page, this.pageNumber, this.printMode, this.screenMode);

                g.PageUnit = pageUnit;

                PrintPreview(g, this.artifacts, this.pageScale, this.transformScale, this.printMode, this.screenMode);
            }

        }

        private void PrintTest(Graphics g)
        {
            //Font font = new Font("Darleston", 20);
            //SolidBrush brush = new SolidBrush(Color.Black);

            //string text = "Nederland";// s Nieuw Guinea";

            //g.DrawString(text, font, brush, (int)(5 * pageScale), 10);
            //g.DrawLine(new Pen(Color.Red), new Point((int)(5*pageScale), (int)(25 * pageScale)), new Point((int)(205 * pageScale), (int)(25 * pageScale)));
            //g.DrawLine(new Pen(Color.Green), new Point((int)(5 * pageScale / transformScale), (int)(100 * pageScale / transformScale)), new Point((int)(205 * pageScale / transformScale), (int)(100 * pageScale/transformScale)));

            SolidBrush brush = new SolidBrush(Color.Black);

            Font font;

            string text = "donker- tot lichtblauw";

            font = new Font("Times New Roman", 5);
            g.DrawString(text, font, brush, 5, 10);

            font = new Font("Times New Roman", 6);
            g.DrawString(text, font, brush, 5, 20);

            font = new Font("Times New Roman", 7);
            g.DrawString(text, font, brush, 5, 30);
        }

        private void PrintPreview(Graphics g, Artifacts artifacts, float pageScale, float transformScale, PrintMode printMode, ScreenMode screenMode)
        {
            Pen pen;

            if (printMode == PrintMode.ToScreen)
            {
                g.ScaleTransform(transformScale, transformScale);

                if (screenMode == ScreenMode.MatchScreenWidth)
                {
                    g.TranslateTransform(0, -vScrollBar.Value);
                }
            }

            // Determines smoothness for shapes such as lines, ellipses, and polygons
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Determines smoothness for text
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // Determines smoothness for images that are scaled
            g.InterpolationMode = InterpolationMode.High;

            //PrintTest(g);
            //return;

            float currentX = 0;
            float currentY = 0;

            foreach (Artifact artefact in artifacts.Get())
            {
                if (printMode == PrintMode.ToDocument && artefact.screenOnly)
                {
                    continue;
                }

                Artifact artifact = artefact.GetScaledCopy(pageScale);

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
                        //TextRenderer.DrawText(g, artifact.Text, artifact.Font, new Point((int)artifact.X, (int)artifact.Y), artifact.TextColor.Color);
                        g.DrawRectangle(new Pen(Color.Red), artifact.X, artifact.Y, artifact.Width, artifact.Height);
                        break;

                    case ArtifactType.Image:
                        if (printMode == PrintMode.ToDocument)
                        {
                            throw new Exception("todo");
                        }
                        if (artifact.Width > 0 && artifact.Height > 0)
                        {
                            g.DrawImage(artifact.Picture, artifact.X, artifact.Y, artifact.Width, artifact.Height);
                        }
                        break;

                    default:
                        throw new Exception(string.Format("Artifact type '{0}' not yet implemented", artifact.Type));
                }
            }
        }

        public void ResizePreview(PageFormat pageFormat, PrintMode printMode, ScreenMode screenMode, out float pageScale, out float transformScale)
        {
            if (printMode == PrintMode.ToDocument)
            {
                screenMode = ScreenMode.MatchPaper;
            }

            vScrollBar.Visible = screenMode == ScreenMode.MatchScreenWidth;

            int screenWidthInPixels = Screen.FromControl(this).WorkingArea.Width;
            int screenHeightInPixels = Screen.FromControl(this).WorkingArea.Height;

            int pageWidthInPixels = (int)((pageFormat.PageWidth / 25.4) * this.dpiX);
            int pageHeightInPixels = (int)((pageFormat.PageHeight / 25.4) * this.dpiY);

            int width = 0;
            int height = 0;

            switch (screenMode)
            {
                case ScreenMode.MatchPaper:
                    width = pageWidthInPixels;
                    height = pageHeightInPixels;
                    break;

                case ScreenMode.MatchScreenHeight:
                    height = screenHeightInPixels;
                    width = (int)(pageFormat.PageWidth * screenHeightInPixels / pageFormat.PageHeight);
                    break;

                case ScreenMode.MatchScreenWidth:
                    width = screenWidthInPixels;
                    height = (int)(pageFormat.PageHeight * screenWidthInPixels / pageFormat.PageWidth);
                    vScrollBar.Maximum = height - screenHeightInPixels;
                    vScrollBar.LargeChange = screenHeightInPixels;
                    vScrollBar.SmallChange = screenHeightInPixels / 10;
                    break;
            }

            switch (screenMode)
            {
                case ScreenMode.MatchPaper:
                    transformScale = 1;
                    break;
                case ScreenMode.MatchScreenHeight:
                case ScreenMode.MatchScreenWidth:
                    float transformScaleX = (float)width / pageWidthInPixels;
                    float transformScaleY = (float)height / pageHeightInPixels;
                    transformScale = (transformScaleX + transformScaleY) / 2;
                    break;
                default:
                    transformScale = 1;
                    break;
            }

            float pageScaleX = (float)width / pageFormat.PageWidth;
            float pageScaleY = (float)height / pageFormat.PageHeight;

            pageScale = (pageScaleX + pageScaleY) / 2 / transformScale;

            this.Size = new Size(width, height);
        }

        private void AssemblePreview(Graphics g, Page page, int pageNumber, PrintMode printMode, ScreenMode screenMode)
        {
            // Determines smoothness for text
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            PageSetup setup = PageSetup.Get();

            PageFormat format = setup.PageFormat;

            if (setup.IncludeMarginForPunchHoles)
            {
                float additionalMarginLeft = 12.5f;

                format = new PageFormat(format.FormatName, format.TitleStyle, format.TitleFont, format.PageWidth, format.PageHeight, format.MarginLeft + additionalMarginLeft, format.MarginRight, format.MarginTop, format.MarginBottom, format.FreeLeft, format.FreeRight, format.FreeTop, format.FreeBottom, format.PrePrintedBorder, format.PrePrintedTitle);
            }

            float y = format.Free.Top;

            artifacts = new Artifacts(g, this.pageScale);

            // Form caption
            artifacts.AddText(1, 1, 0, string.Format("Escher · Preview · Paper:<b>{0}</b> · Page Number:<b>{1}</b>", format.FormatName, pageNumber), "Microsoft Sans Serif", 7, foreColor: Color.Gray, screenOnly: true);

            // Legenda
            artifacts.AddText(1, artifacts.Last().Bottom(2), 0, "c: ± color · n: ± number · v: ± value · f: ± frame · t: ± title · s: ± font", "Microsoft Sans Serif", 7, foreColor: Color.Gray, screenOnly: true);
            artifacts.AddText(1, artifacts.Last().Bottom(2), 0, "+: next match · -: previous match · =: pin", "Microsoft Sans Serif", 7, foreColor: Color.Gray, screenOnly: true);
            artifacts.AddText(1, artifacts.Last().Bottom(2), 0, char.ConvertFromUtf32(0x2190) + ": previous page · " + char.ConvertFromUtf32(0x2192) + ": next page" + " · p: page setup", "Microsoft Sans Serif", 7, foreColor: Color.Gray, screenOnly: true);
            artifacts.AddText(1, artifacts.Last().Bottom(2), 0, "esc: close", "Microsoft Sans Serif", 7, foreColor: Color.Gray, screenOnly: true);

            // Form border
            artifacts.AddRectangle(0, 0, (this.Width - 1) / pageScale, (this.Height - 1) / pageScale, Color.Gray, screenOnly: true);

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
                            y += artifacts.AddText(format.Free.Left, y, format.Free.Width, page.Country, format.TitleFont, 18, fontBold: true, foreColor: Color.Black, alignment: Alignment.Centered).Height;
                        }
                        else
                        {
                            y += artifacts.AddText(format.Free.Left, y, format.Free.Width, page.Country, format.TitleFont, 18, fontBold: true, foreColor: Color.Gray, alignment: Alignment.Centered, screenOnly: true).Height;
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

            bool printPage = (printMode == PrintMode.ToScreen) || (printMode == PrintMode.ToDocument && !setup.IncludeSamplePagesOnly) || (printMode == PrintMode.ToDocument && setup.IncludeSamplePagesOnly && page.IsSample);

            if (!printPage)
            {
                artifacts.AddText(format.Free.Left, y, format.Free.Width, "DIT ALBUMBLAD IS NIET OPGENOMEN IN HET VOORBEELD DOCUMENT!%%THIS ALBUM PAGE IS NOT INCLUDED IN THE SAMPLES DOCUMENT!", format.TitleFont, 12, fontBold: true, foreColor: Color.Red, alignment: Alignment.Centered);
            }
            else
            {
                // Eg. 1867-1869. Koning Willem III.
                y += artifacts.AddText(format.Free.Left, y, format.Free.Width, string.IsNullOrEmpty(page.Series) ? "·" : page.Series, format.TitleFont, 7, foreColor: string.IsNullOrEmpty(page.Series) ? Color.White : Color.Black, alignment: Alignment.Centered).Height;

                // Eg. Type I.
                y += artifacts.AddText(format.Free.Left, y, format.Free.Width, string.IsNullOrEmpty(page.MainType) ? "" : page.MainType, format.TitleFont, 7, alignment: Alignment.Centered).Height;

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
                        y += artifacts.AddText(format.Free.Left, y + artifacts.GetTextHeight(format.TitleFont, 9), format.Free.Width, varieties.SubType, format.TitleFont, 7, alignment: Alignment.Centered).Height;

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
                        y += artifacts.AddText(rowLeft + varieties.Horizontal, y + varieties.Vertical, rowWidth, text, format.TitleFont, fontSize, alignment: alignment).Height;
                    }
                    else
                    {
                        y += artifacts.AddText(rowLeft + varieties.Horizontal - textWidth, y + varieties.Vertical, rowWidth + 2 * textWidth, text, format.TitleFont, fontSize, alignment: alignment).Height;
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
                                y += artifacts.AddText(variety.FrameLeft, y + variety.Vertical, variety.FrameWidth, variety.Title, format.TitleFont, 5, alignment: variety.Alignment).Height;

                                // There is at least one stamp with a description
                                hasDescriptions = true;
                            }
                        }

                        if (hasDescriptions)
                        {
                            // Now move the y-position 1 millimeter down
                            y += 1 - row[0].Vertical;
                        }

                        float rowHeight = page.RowHeight(v, r);

                        for (int s = 0; s < row.Count(); s++)
                        {
                            Variety variety = row[s];

                            float x1 = variety.FrameLeft;
                            float y1 = y + variety.FrameOffset + (rowHeight - variety.Height) / 2;

                            // Stamp/sheet

                            float currentX = x1;
                            float currentY = y1;

                            if (!variety.Skip)
                            {
                                if (string.IsNullOrEmpty(variety.Sheet))
                                {
                                    // A page without album number is a title page, so do show the coat of arms
                                    if (setup.IncludeImage || string.IsNullOrEmpty(page.AlbumNumber))
                                    {
                                        artifacts.AddImage(page.ImagesPath, variety.Number, currentX, currentY, variety.Width, variety.Height, variety.ExtraWidth, variety.ExtraHeight, variety.Shape, variety.Appearance, variety.Picture, setup.ColorStyle);
                                    }
                                }
                                else
                                {
                                    throw new Exception(("todo print sheet"));
                                }
                            }

                            // Frame

                            currentX = x1;
                            currentY = y1;

                            if (!variety.Skip && variety.FrameColor != Color.White)
                            {
                                switch (variety.Shape)
                                {
                                    case Shape.Rectangle:
                                        artifacts.AddRectangle(currentX, currentY, variety.Width, variety.Height, variety.FrameColor, frameStyle: setup.FrameStyle, appearance: variety.Appearance);
                                        break;

                                    default:
                                        throw new Exception("todo");
                                }
                            }

                            currentX = x1 + variety.Width;
                            currentY = y1 + variety.Height;

                        } // for (int s = 0; s < row.Count(); s++)

                        y += rowHeight + 1;

                        for (int s = 0; s < row.Count(); s++)
                        {
                            Variety variety = row[s];

                            if (!variety.Skip && setup.IncludeNumber)
                            {
                                // Eg. 7 IA
                                artifacts.AddText(variety.FrameLeft, y + variety.FrameOffset, variety.Width, variety.Number == "0" ? "" : variety.Number, format.TitleFont, (float) setup.FontSize, fontBold: true, alignment: Alignment.Centered); 
                            }
                        } // for (int s = 0; s < row.Count(); s++)

                        if (setup.IncludeNumber)
                        {
                            y += artifacts.GetTextHeight(format.TitleFont, (int) setup.FontSize, fontBold: true);
                        }

                        float yMax = 0;

                        if (!varieties.Combine)
                        {
                            yMax = y;
                        }

                        for (int s = 0; s < row.Count(); s++)
                        {
                            Variety variety = row[s];

                            float yNew = 0;

                            if (!variety.Skip && setup.IncludeValue)
                            {
                                // Eg. 5 ct. mat ultramarijn
                                yNew = artifacts.AddText(variety.FrameLeft - 1, y + variety.FrameOffset, variety.Width + 2, variety.Description, format.TitleFont, (int)setup.FontSize, alignment: Alignment.Centered).Height;
                            }

                            if (yNew > yMax)
                            {
                                yMax = yNew;
                            }
                        }

                        y += yMax + 2;

                    } // for (int r = 0; r < varieties.Rows.Count(); r++)

                    if (v < page.Varieties.Count() - 1)
                    {
                        // Are the next varieties combined with the current varieties?
                        if (page.Varieties[v + 1].Combine)
                        {
                            // Restore the y-position
                            y = yCombine;
                        }
                    }

                } // for (int v = 0; v < page.Varieties.Count(); v++)
            }
        }
    }
}

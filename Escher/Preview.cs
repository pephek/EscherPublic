using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

            this.Load += new EventHandler((sender, e) => this.Location = new Point(0, 0));
            this.Paint += new PaintEventHandler((sender, e) => RefreshPreview(resizePreview: false));
            this.KeyDown += new KeyEventHandler((sender, e) => HandleKeyDown(e.KeyCode, e.Modifiers));
            this.MouseDown += new MouseEventHandler((sender, e) => HandleMouseDown(e.Location));
            this.MouseMove += new MouseEventHandler((sender, e) => HandleMouseMove(e.X, e.Y));
            this.MouseUp += new MouseEventHandler((sender, e) => HandleMouseUp());
            this.MouseDoubleClick += new MouseEventHandler((sender, e) => this.Location = new Point(0, 0));

            vScrollBar.KeyDown += new KeyEventHandler((sender, e) => HandleKeyDown(e.KeyCode, e.Modifiers));
            vScrollBar.ValueChanged += new EventHandler((sender, e) => RefreshPreview(resizePreview: false));
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

        private void HandleKeyDown(Keys keyCode, Keys modifiers)
        {
            switch (keyCode)
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
                    switch (modifiers)
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

        private void HandleMouseDown(Point location)
        {
            foreach (Artifact artifact in artifacts.Get())
            {
                if (artifact.Type == ArtifactType.Image && artifact.Number != "")
                {
                    Artifact artefact = artifact.GetScaledCopy(this.pageScale * this.transformScale);

                    if (artefact.Overlaps(location))
                    {
                        Design series = design.GetStampsFromSeries(pageNumber: this.pageNumber, number: artifact.Number);

                        DesignEntry stamp = series.GetStampFromSeries(artifact.Number);

                        if (stamp != null)
                        {
                            Imaging imaging = new Imaging();

                            imaging.SetImage(
                                series: series,
                                stampNumber: stamp.Number,
                                folder: App.GetSetting("ImagesFolder"),
                                country: design.GetCountry(stamp.PageNumber).Text,
                                section: design.GetSection(stamp.PageNumber).Text
                            );

                            if (imaging.ShowDialog() == DialogResult.OK)
                            {
                                RefreshPreview(resizePreview: false);
                            }

                            return;
                        }
                    }
                }
            }

            this.mouseDown = true;
            this.mouseLastLocation = location;
        }

        private void HandleMouseMove(int x, int y)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - this.mouseLastLocation.X) + x, (this.Location.Y - this.mouseLastLocation.Y) + y);

                this.Update();
            }
        }

        private void HandleMouseUp()
        {
            this.mouseDown = false;
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
            SolidBrush brush;

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
                        pen.Dispose();
                        break;

                    case ArtifactType.MoveDotted:
                        pen = new Pen(artifact.ForeColor);
                        pen.DashPattern = new float[] { 1, 2};
                        g.DrawLine(pen, currentX, currentY, currentX + artifact.Width, currentY + artifact.Height);
                        currentX += artifact.Width;
                        currentY += artifact.Height;
                        pen.Dispose();
                        break;

                    case ArtifactType.Text:
                        g.DrawString(artifact.Text, artifact.Font, artifact.TextColor, artifact.X, artifact.Y);
                        g.DrawRectangle(new Pen(Color.Red), artifact.X, artifact.Y, artifact.Width+1, artifact.Height);
                        break;

                    case ArtifactType.Area:
                        brush = new SolidBrush(artifact.ForeColor);
                        g.FillRectangle(brush, new Rectangle((int)currentX, (int)currentY, (int)artifact.Width, (int)artifact.Height));
                        brush.Dispose();
                        break;

                    case ArtifactType.Image:
                        if (printMode == PrintMode.ToDocument)
                        {
                            throw new Exception("todo");
                        }
                        Bitmap bitmap = new Bitmap(artifact.Image);
                        bitmap.RotateFlip(artifact.RotateFlipType);
                        artifact.Image = bitmap;
                        g.DrawImage(artifact.Image, artifact.X, artifact.Y, artifact.Width, artifact.Height);
                        bitmap.Dispose();
                        artifact.Image.Dispose();
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
            if (setup.IncludeBorder && page.AlbumNumber != "")
            {
                artifacts.AddHorizontalLine(format.Border.Left, format.Border.Top, format.Border.Width);
                artifacts.AddHorizontalLine(format.Border.Left, format.Border.Bottom, format.Border.Width);
            }

            // Copyright statement
            if (page.Copyright != "")
            {
                artifacts.AddText(format.Border.Left, format.Border.Bottom + 2, 0, page.Copyright, format.TitleFont, 7, foreColor: Color.Gray);
            }

            // Album number
            if (page.AlbumNumber != "")
            {
                artifacts.AddText(format.Border.Left, format.Border.Bottom + 2, format.Border.Width, page.AlbumNumber, format.TitleFont, 7, alignment: Alignment.Right);
            }

            // Country
            if (page.Title != "" && page.AlbumNumber == "") // Must be the front page
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
                            if (page.Title != "")
                            {
                                artifacts.AddText(format.Border.Left, format.Border.Top - artifacts.GetTextHeight("Darleston", 20) - 1F, format.Border.Width, page.Title.Split("<br>")[0].Replace("!" + HtmlHelper.cBreak, "").Replace(HtmlHelper.cBreak, ""), "Darleston", 20, alignment: Alignment.Right);
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
                Debug.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                Debug.WriteLine(string.Format("y at start = {0}", Math.Round(y, 2)));

                // Eg. 1867-1869. Koning Willem III.
                y += artifacts.AddText(format.Free.Left, y, format.Free.Width, page.Series == "" ? "·" : page.Series, format.TitleFont, 7, foreColor: page.Series == "" ? Color.White : Color.Black, alignment: Alignment.Centered).Height;

                Debug.WriteLine(string.Format("y after series = {0}", Math.Round(y, 2)));

                // Eg. Type I.
                y += artifacts.AddText(format.Free.Left, y, format.Free.Width, page.MainType, format.TitleFont, 7, alignment: Alignment.Centered).Height;

                y += artifacts.AddText(format.Free.Left, y, format.Free.Width, "", format.TitleFont, 7, alignment: Alignment.Centered).Height;

                Debug.WriteLine(string.Format("y after main type = {0}", Math.Round(y, 2)));

                float pageMargin = page.Margin;
                float yCombine;

                Debug.WriteLine(string.Format("Iterating {0} varieties", page.Varieties.Count()));

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

                    if (varieties.SubType != "")
                    {
                        // Eg. Type II
                        //y += artifacts.GetTextHeight(format.TitleFont, 9 - 1); // -1 to compensate difference with VB6
                        y += 1.6F;

                        Debug.WriteLine(string.Format("y after adding free space = {0}", Math.Round(y, 2)));

                        y += artifacts.AddText(format.Free.Left, y, format.Free.Width, varieties.SubType, format.TitleFont, 7, alignment: Alignment.Centered).Height;

                        y += artifacts.GetTextHeight(format.TitleFont, 7);

                        Debug.WriteLine(string.Format("y after sub type = {0}", Math.Round(y, 2)));

                        yCombine = y; // There is a new y position when varieties are combined
                    }

                    int n = varieties.Rows.Count() - 1;
                    int a = 0;
                    int b = 0;

                    //while (page.RowWidth(v, n, format.Free.Width, out a) > format.Free.Width && varieties.Rows[n].Count() > 1)
                    //{
                    //    throw new Exception("TODO");
                    //}

                    // Eg.A - Kamtanding 12¾ : 11¾.
                    float fontSize = varieties.FontOfDescription ? 5 : 7;

                    string text;
                    Alignment alignment;

                    if (setup.IncludeNumber && varieties.PrivateDescription != "")
                    {
                        text = varieties.PrivateDescription;
                        alignment = Alignment.Centered;
                    }
                    else
                    {
                        text = varieties.PublicDescription;
                        alignment = varieties.Alignment;
                    }

                    if (text != "")
                    {
                        float textWidth = g.MeasureText(text, format.TitleFont, fontSize, fontBold: false, fontItalic: false).Width;
                        float rowWidth = page.RowWidth(v, 0);
                        float rowLeft = page.RowLeft(v, 0, format.Free.Left, format.Free.Width);

                        Debug.WriteLine(string.Format("Text width = {0}", Math.Round(textWidth, 2)));
                        Debug.WriteLine(string.Format("Row width = {0}", Math.Round(rowWidth, 2)));
                        Debug.WriteLine(string.Format("Row left = {0}", Math.Round(rowLeft, 2)));

                        if (textWidth <= rowWidth)
                        {
                            y += artifacts.AddText(rowLeft + varieties.Horizontal, y + varieties.Vertical, rowWidth, text, format.TitleFont, fontSize, alignment: alignment).Height;
                        }
                        else
                        {
                            y += artifacts.AddText(rowLeft + varieties.Horizontal - textWidth, y + varieties.Vertical, rowWidth + 2 * textWidth, text, format.TitleFont, fontSize, alignment: alignment).Height;
                        }

                        y += 1; // - varieties.Vertical;
                    }

                    Debug.WriteLine(string.Format("y after varieties = {0}", Math.Round(y, 2)));

                    Debug.WriteLine(string.Format("Iterating {0} rows", varieties.Rows.Count()));

                    for (int r = 0; r < varieties.Rows.Count(); r++)
                    {
                        List<Variety> row = varieties.Rows[r];

                        Debug.WriteLine(string.Format("Iterating {0} stamps", row.Count()));

                        for (int s = 0; s < row.Count(); s++)
                        {
                            Variety stamp = row[s];

                            stamp.FrameLeft = page.FrameLeft(v, r, s, format.Free.Left, format.Free.Width);
                            stamp.FrameOffset = page.FrameOffset(v, r, s);
                            stamp.FrameWidth = page.FrameWidth(v, r, s);

                            Debug.WriteLine(string.Format("Frame[{0}]: left {1}, offset {2}, width {3}", s, stamp.FrameLeft, stamp.FrameOffset, stamp.FrameWidth));
                        }

                        bool hasDescriptions = false; // No stamps have a description yet

                        Debug.WriteLine(string.Format("Iterating again {0} stamps", row.Count()));

                        float maxHeight = 0;

                        for (int s = 0; s < row.Count(); s++)
                        {
                            Variety stamp = row[s];

                            if (stamp.Title != "")
                            {
                                if (!hasDescriptions)
                                {
                                    // As soon as we found a description then move the y-position 1 millimeter up
                                    y -= 1;

                                    Debug.WriteLine(string.Format("y after moving 1 mm. up = {0}", Math.Round(y, 2)));
                                }

                                // Eg. Zonder punt in linker onderhoek.
                                float height = artifacts.AddText(stamp.FrameLeft, y + stamp.Vertical, stamp.FrameWidth, stamp.Title, format.TitleFont, 5, alignment: stamp.Alignment).Height;

                                maxHeight = Math.Max(maxHeight, height);

                                // There is at least one stamp with a description
                                hasDescriptions = true;
                            }
                        }

                        if (hasDescriptions)
                        {
                            // Now move the y-position 1 millimeter down
                            y += maxHeight + 1 - row[0].Vertical;
                        }

                        Debug.WriteLine(string.Format("y after descriptions = {0}", Math.Round(y, 2)));

                        Debug.WriteLine(string.Format("Iterating again {0} stamps", row.Count()));

                        float rowHeight = page.RowHeight(v, r);

                        Debug.WriteLine(string.Format("Row height = {0}", Math.Round(rowHeight, 2)));

                        for (int s = 0; s < row.Count(); s++)
                        {
                            Variety stamp = row[s];

                            float x1 = stamp.FrameLeft;
                            float y1 = y + stamp.FrameOffset + (rowHeight - stamp.Height) / 2 + stamp.Vertical;

                            Debug.Print(string.Format("Location[{0}]: x {1}, y {2}", s, Math.Round(x1, 2), Math.Round(y1, 2)));

                            // Stamp/sheet

                            if (!stamp.Skip)
                            {
                                if (stamp.Sheet == "")
                                {
                                    // A page without album number is a title page, so do show the coat of arms
                                    if (setup.IncludeImage || page.AlbumNumber == "")
                                    {
                                        artifacts.AddImage(page.ImagesPath, stamp.Number, x1, y1, stamp.Width, stamp.Height, stamp.Shape, stamp.Appearance, stamp.Picture, setup.ColorStyle, setup.FrameStyle);
                                    }
                                }
                                else
                                {
                                    throw new Exception(("todo print sheet"));
                                }
                            }

                            // Frame

                            if (!stamp.Skip && stamp.FrameColor != Color.White)
                            {
                                switch (stamp.Shape)
                                {
                                    case Shape.Rectangle:
                                        artifacts.AddRectangle(x1, y1, stamp.Width, stamp.Height, stamp.FrameColor, frameStyle: setup.FrameStyle, appearance: stamp.Appearance);
                                        break;

                                    default:
                                        throw new Exception("todo");
                                }
                            }

                        } // for (int s = 0; s < row.Count(); s++)

                        y += rowHeight + 1;

                        Debug.WriteLine(string.Format("y after frames & images = {0}", Math.Round(y, 2)));

                        // Number

                        if (setup.IncludeNumber)
                        {
                            Debug.WriteLine(string.Format("Iterating again {0} stamps for number", row.Count()));

                            maxHeight = 0;

                            for (int s = 0; s < row.Count(); s++)
                            {
                                Variety stamp = row[s];

                                if (!stamp.Skip)
                                {
                                    // Eg. 7 IA
                                    float height = artifacts.AddText(stamp.FrameLeft, y + stamp.FrameOffset + stamp.Vertical, stamp.Width, stamp.Number == "0" ? "" : stamp.Number, format.TitleFont, (float)setup.FontSize, fontBold: true, alignment: Alignment.Centered).Height;

                                    maxHeight = Math.Max(maxHeight, height);
                                }
                            } // for (int s = 0; s < row.Count(); s++)

                            y += maxHeight;
                        }

                        Debug.WriteLine(string.Format("y after numbers = {0}", Math.Round(y, 2)));

                        // Colors & Values

                        if (setup.IncludeValueAndColor)
                        {
                            Debug.WriteLine(string.Format("Iterating again {0} stamps for value & color", row.Count()));

                            maxHeight = 0;

                            for (int s = 0; s < row.Count(); s++)
                            {
                                Variety stamp = row[s];

                                if (!stamp.Skip)
                                {
                                    // Eg. 5 ct. mat ultramarijn
                                    float height = artifacts.AddText(stamp.FrameLeft - 1, y + stamp.FrameOffset + stamp.Vertical, stamp.Width + 2, stamp.Description, format.TitleFont, (int)setup.FontSize, alignment: Alignment.Centered).Height;

                                    maxHeight = Math.Max(maxHeight, height);
                                }
                            } // for (int s = 0; s < row.Count(); s++)

                            y += maxHeight;
                        }

                        // Add 2 mm. spacing
                        y += 2;

                        Debug.WriteLine(string.Format("y after values & colors = {0}", Math.Round(y, 2)));

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

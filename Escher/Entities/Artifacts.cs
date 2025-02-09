﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Escher
{
    public class Artifacts
    {
        private Graphics graphics;
        private float scale;
        private int pageNumber;
        private string albumNumber;

        private List<Artifact> artifacts = new List<Artifact>();

        private Dictionary<string, Image> images = new Dictionary<string, Image>();

        public void Clear(Graphics graphics, float scale, int pageNumber, string albumNumber)
        {
            this.graphics = graphics;
            this.scale = scale;

            this.artifacts.Clear();

            if (pageNumber != this.pageNumber || albumNumber != this.albumNumber)
            {
                this.images.Clear();
            }

            this.pageNumber = pageNumber;
            this.albumNumber = albumNumber;
        }

        public List<Artifact> Get()
        {
            return artifacts;
        }

        public int Count()
        {
            return artifacts.Count();
        }

        public Artifact Last()
        {
            return artifacts[artifacts.Count() - 1];
        }

        public void AddCursor(float x, float y)
        {
            Artifact artifact = new Artifact(ArtifactType.Cursor);

            artifact.X = x;
            artifact.Y = y;

            artifacts.Add(artifact);
        }

        public void AddMove(float width, float height, Color foreColor, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular, bool screenOnly = false)
        {
            ArtifactType type;

            if (frameStyle == FrameStyle.Thick)
            {
                type = ArtifactType.Rectangle;
            }
            else if (frameStyle == FrameStyle.ThinSolid)
            {
                type = ArtifactType.MoveSolid;
            }
            else
            {
                type = ArtifactType.MoveDotted;
            }

            Artifact artifact = new Artifact(type);

            artifact.Width = width;
            artifact.Height = height;
            artifact.ForeColor = foreColor;
            artifact.Appearance = appearance;
            artifact.screenOnly = screenOnly;

            artifacts.Add(artifact);
        }

        public void AddRectangle(float x, float y, float width, float height, Color foreColor, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular, bool screenOnly = false)
        {
            AddCursor(x, y);

            if (frameStyle == FrameStyle.ThinSolid || frameStyle == FrameStyle.ThinDotted)
            {
                AddMove(width, 0, foreColor, frameStyle, appearance, screenOnly);
                AddMove(0, height, foreColor, frameStyle, appearance, screenOnly);
                AddMove(-width, 0, foreColor, frameStyle, appearance, screenOnly);
                AddMove(0, -height, foreColor, frameStyle, appearance, screenOnly);
            }
            else // frameStyle == FrameStyle.ThickFrame
            {
                AddMove(width, height, foreColor, frameStyle, appearance, screenOnly);
            }
        }

        private void AddArea(float x, float y, float width, float height, Color foreColor)
        {
            AddCursor(x, y);

            Artifact artifact = new Artifact(ArtifactType.Area);
            artifact.Width = width;
            artifact.Height = height;
            artifact.ForeColor = foreColor;

            artifacts.Add(artifact);
        }

        public void AddHorizontalLine(float x, float y, float width, Color? foreColor = null, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular, bool screenOnly = false)
        {
            AddCursor(x, y);

            if (frameStyle == FrameStyle.ThinSolid || frameStyle == FrameStyle.ThinDotted)
            {
                AddMove(width, 0, foreColor == null ? Color.Black : (Color)foreColor, frameStyle, appearance, screenOnly);
            }
            else // frameStyle == FrameStyle.ThickFrame
            {
                throw new Exception(string.Format("Cannot add a line with style '{0}'", frameStyle));
            }
        }

        public void AddVerticalLine(float x, float y, float height, Color? foreColor = null, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular, bool screenOnly = false)
        {
            AddCursor(x, y);

            if (frameStyle == FrameStyle.ThinSolid || frameStyle == FrameStyle.ThinDotted)
            {
                AddMove(0, height, foreColor == null ? Color.Black : (Color)foreColor, frameStyle, appearance, screenOnly);
            }
            else // frameStyle == FrameStyle.ThickFrame
            {
                throw new Exception(string.Format("Cannot add a line with style '{0}'", frameStyle));
            }
        }

        private void AddImage(float x, float y, float width, float height, string number, string image, string overprint, RotateFlipType? rotateFlipType, Appearance appearance, bool roundedCorners)
        {
            Artifact artifact = new Artifact(ArtifactType.Image);

            artifact.X = x;
            artifact.Y = y;
            artifact.Width = width;
            artifact.Height = height;
            artifact.Number = number;
            if (!this.images.ContainsKey(image))
            {
                this.images.Add(image, ImageHelper.LoadImageAndUnlock(image));
            }
            artifact.Image = this.images[image];
            artifact.Overprint = overprint;
            artifact.RotateFlipType = rotateFlipType ?? RotateFlipType.RotateNoneFlipNone;
            artifact.Appearance = appearance;
            artifact.RoundedCorners = roundedCorners;

            artifacts.Add(artifact);
        }

        public void AddImage(string path, string number, string positions, float left, float top, float width, float height, Shape shape, Appearance appearance, string picture, string overprint, Color frameColor, ColorStyle colorStyle, FrameStyle frameStyle, bool roundedCorners)
        {
            number = !string.IsNullOrEmpty(picture) ? picture.Trim() : number.Trim();

            if (number.Contains('('))
            {
                number = number.Substring(0, number.IndexOf('(') - 1).Trim();
            }

            string image = null;

            string n = number;

            while (image == null && n != "")
            {
                image = ImageHelper.GetDisplayImagePath(path, n, colorStyle);

                if (image == null)
                {
                    if (!Char.IsDigit(n[0]))
                    {
                        n = n.Substring(1);
                    }
                    else if (!Char.IsDigit(n[n.Length - 1]) || n.Contains('+'))
                    {
                        n = n.Substring(0, n.Length - 1);
                    }
                    else
                    {
                        n = "";
                    }
                }
            }

            if (image == null)
            {
                return; // No need to stay here if the image is not found, return immediately
            }

            float x = left;
            float y = top;
            float w = width;
            float h = height;

            switch (shape)
            {
                case Shape.Triangle45:
                    AddImage(left + 3 * 5 / (float) Math.Sqrt(2), top + 2 * 5 / (float) Math.Sqrt(2), width - 2 * 3 * 5 / (float) Math.Sqrt(2), height - 5 - 2 * 5 / (float) Math.Sqrt(2), n, image, overprint, RotateFlipType.RotateNoneFlipNone, appearance, roundedCorners);
                    break;
                case Shape.Triangle60Inverted:
                    AddImage(left + 5 + 5 / (float)Math.Sqrt(3), top + 5, width - 2 * (5 + 5 / (float)Math.Sqrt(3)), height - 2 * (5 + 5 / (float)Math.Sqrt(3)), n, image, overprint, RotateFlipType.RotateNoneFlipNone, appearance, roundedCorners);
                    break;
                case Shape.Triangle60:
                    AddImage(left + 5 + 5 / (float)Math.Sqrt(3), top + 2 * 5, width - 2 * (5 + 5 / (float)Math.Sqrt(3)), height - 2 * (5 + 5 / (float)Math.Sqrt(3)), n, image, overprint, RotateFlipType.RotateNoneFlipNone, appearance, roundedCorners);
                    break;
                case Shape.RectangleRotated:
                    AddImage(left + 5 * (float)Math.Sqrt(2), top + 5 * (float)Math.Sqrt(2), width - 2 * 5 * (float)Math.Sqrt(2), height - 2 * 5 * (float)Math.Sqrt(2), n, image, overprint, RotateFlipType.RotateNoneFlipNone, appearance, roundedCorners);
                    break;

                default:

                    int numberOfStamps = appearance.NumberOfStamps();

                    for (int i = 0; i < numberOfStamps; i++)
                    {
                        RotateFlipType? rotateFlipType = RotateFlipType.RotateNoneFlipNone;

                        switch (appearance)
                        {
                            case Appearance.Singular:
                            case Appearance.Rotated:
                            case Appearance.WatermarkInverted:
                            case Appearance.WatermarkReversed:
                            case Appearance.WatermarkInvertedAndReversed:
                            case Appearance.WatermarkSideways:
                            case Appearance.WatermarkSidewaysInverted:
                                if (frameColor != Color.White)
                                {
                                    x = left + 4;
                                    y = top + 4;
                                    w = width - 8;
                                    h = height - 8;
                                    switch (appearance)
                                    {
                                        case Appearance.Rotated:
                                        case Appearance.WatermarkInverted:
                                            rotateFlipType = RotateFlipType.Rotate180FlipNone;
                                            break;
                                        case Appearance.WatermarkReversed:
                                            rotateFlipType = RotateFlipType.RotateNoneFlipX;
                                            break;
                                        case Appearance.WatermarkInvertedAndReversed:
                                            rotateFlipType = RotateFlipType.Rotate180FlipX;
                                            break;
                                        case Appearance.WatermarkSideways:
                                            rotateFlipType = RotateFlipType.Rotate270FlipNone;
                                            break;
                                        case Appearance.WatermarkSidewaysInverted:
                                            rotateFlipType = RotateFlipType.Rotate90FlipNone;
                                            break;
                                    }
                                }
                                break;
                            case Appearance.Proof:
                                if (frameStyle == FrameStyle.Thick)
                                {
                                    AddArea(left + 4, top + 4, width - 8, height - 8, Color.Black);
                                }
                                x = left + 8;
                                y = top + 8;
                                w = width - 16;
                                h = height - 16;
                                break;
                            case Appearance.BandePublicitaire:
                                h = (height - 4) / 1.4F - 4;
                                x = left + 4;
                                y = top + height - 4 - h;
                                w = width - 8;
                                break;
                            case Appearance.PaireCarnet:
                                // H = 2 * {(h - 4) + 0.3 * (h - 4)} + 4 = 2 * 1.3 * (h - 4) + 4 = 2.6 * h - 10.4 + 4 = 2.6 * h - 6.4
                                // h = (H + 6.4) / 2.6
                                h = (height + 6.4F) / 2.6F - 8;
                                x = left + 4;
                                y = top + (i == 0 ? height / 2 - h - 2 : height / 2 + 2);
                                w = width - 8;
                                break;
                            case Appearance.HorizontalInterpanneaux:
                                w = (width - 4) / 2.5F;
                                x = left + (i == 0 ? 4 : w * 1.5F + 4);
                                y = top + 4;
                                w = w - 4;
                                h = height - 8;
                                break;
                            case Appearance.PairHorizontal:
                            case Appearance.HorizontalStrip3:
                            case Appearance.HorizontalStrip4:
                            case Appearance.HorizontalStrip5:
                            case Appearance.HorizontalStrip6:
                            case Appearance.TeteBecheHorizontal:
                            case Appearance.TeteBecheHorizontalGutter:
                            case Appearance.HorizontalGutterPair:
                                w = (width + (numberOfStamps - 1) * 4) / numberOfStamps;
                                x = left + i * (w - 4) + 4;
                                y = top + 4;
                                w = w - 8;
                                h = height - 8;
                                if (appearance == Appearance.TeteBecheHorizontal)
                                {
                                    if (w < h)
                                    {
                                        rotateFlipType = (i == 0 ? RotateFlipType.RotateNoneFlipNone : RotateFlipType.Rotate180FlipNone);
                                    }
                                    else
                                    {
                                        rotateFlipType = (i == 0 ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone);
                                    }
                                }
                                if (appearance == Appearance.TeteBecheHorizontalGutter)
                                {
                                    if (w < h)
                                    {
                                        rotateFlipType = (i == 0 ? RotateFlipType.RotateNoneFlipNone : RotateFlipType.Rotate180FlipNone);
                                    }
                                    else
                                    {
                                        rotateFlipType = (i == 0 ? RotateFlipType.Rotate90FlipNone : RotateFlipType.Rotate270FlipNone);
                                    }
                                }
                                if (i == 1 && (appearance == Appearance.TeteBecheHorizontalGutter || appearance == Appearance.HorizontalGutterPair))
                                {
                                    rotateFlipType = null;
                                }
                                break;
                            case Appearance.PairVertical:
                            case Appearance.VerticalStrip3:
                            case Appearance.VerticalStrip4:
                            case Appearance.VerticalStrip5:
                            case Appearance.VerticalStrip6:
                            case Appearance.TeteBecheVertical:
                            case Appearance.TeteBecheVerticalGutter:
                            case Appearance.VerticalGutterPair:
                                h = (height + (numberOfStamps - 1) * 4) / numberOfStamps;
                                x = left + 4;
                                y = top + i * (h - 4) + 4;
                                w = width - 8;
                                h = h - 8;
                                if (appearance == Appearance.TeteBecheVertical || appearance == Appearance.TeteBecheVerticalGutter)
                                {
                                    if (w < h)
                                    {
                                        rotateFlipType = (i == 0 ? RotateFlipType.RotateNoneFlipNone : RotateFlipType.Rotate180FlipNone);
                                    }
                                    else
                                    {
                                        rotateFlipType = (i == 0 ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone);
                                    }
                                }
                                if (i == 1 && (appearance == Appearance.TeteBecheVerticalGutter || appearance == Appearance.VerticalGutterPair ))
                                {
                                    rotateFlipType = null;
                                }
                                break;

                            case Appearance.Block5x5:
                                w = (width - 4) / 5;
                                h = (height - 4) / 5;
                                x = left + 4 + (i % 5) * w;
                                y = top + 4 + (i / 5) * h;
                                w = w - 4;
                                h = h - 4;
                                break;

                            case Appearance.Block2x5:
                            case Appearance.Block2x5Rotated:
                                int rows = appearance.NumberOfHorizontalAndVerticalStamps().Width;
                                int cols = appearance.NumberOfHorizontalAndVerticalStamps().Height;
                                w = (width - 4) / rows;
                                h = (height - 4) / cols;
                                x = left + 4 + (i % rows) * w;
                                y = top + 4 + ((i / rows) % cols) * h;
                                w = w - 4;
                                h = h - 4;
                                if (appearance == Appearance.Block2x5Rotated)
                                {
                                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                                }
                                break;

                            case Appearance.Block:
                                w = (width + 4) / 2;
                                h = (height + 4) / 2;
                                x = left + (i == 1 || i == 3 ? w : 4);
                                y = top + (i == 2 || i == 3 ? h : 4);
                                w = w - 8;
                                h = h - 8;
                                break;
                            case Appearance.Sheet2x3:
                                w = (width + 8) / 3;
                                h = (height + 4) / 2;
                                x = left + (i % 3) * (w - 4) + 4;
                                y = top + (i / 3) * (h - 4) + 4;
                                w = w - 8;
                                h = h - 8;
                                break;
                            case Appearance.ImperfTop:
                                h = (height + 4) / 2;
                                x = left + 4;
                                y = top + h;
                                w = width - 8;
                                h = h - 8;
                                break;
                            case Appearance.ImperfBottom:
                                h = (height + 4) / 2;
                                x = left + 4;
                                y = top + 4;
                                w = width - 8;
                                h = h - 8;
                                break;
                            case Appearance.ImperfLeft:
                                w = (width + 4) / 2;
                                x = left + w;
                                y = top + 4;
                                w = w - 8;
                                h = height - 8;
                                break;
                            case Appearance.ImperfRight:
                                w = (width + 4) / 2;
                                x = left + 4;
                                y = top + 4;
                                w = w - 8;
                                h = height - 8;
                                break;
                            default:
                                throw new Exception("todo");
                        }

                        if (rotateFlipType != null)
                        {
                            switch (appearance)
                            {
                                case Appearance.ImperfTop:
                                case Appearance.ImperfBottom:
                                case Appearance.ImperfLeft:
                                case Appearance.ImperfRight:
                                case Appearance.Proof:
                                case Appearance.BandePublicitaire:
                                case Appearance.PaireCarnet:
                                    if (frameStyle == FrameStyle.Thick && i == 0)
                                    {
                                        AddArea(left + 4, top + 4, width - 8F + 0.25F, height - 8F + 0.25F, Color.Black);
                                    }
                                    break;
                            }

                            if (positions == "")
                            {
                                AddImage(x, y, w, h, n, image, overprint, rotateFlipType, appearance, roundedCorners);
                            }
                            else
                            {
                                SheetPosition sheetPosition = SheetHelper.GetSheetPosition(positions, i);

                                if (sheetPosition != null)
                                {
                                    if (sheetPosition.Type == "")
                                    {
                                        AddImage(x, y, w, h, n, image, overprint, rotateFlipType, appearance, roundedCorners: false);
                                    }
                                    else
                                    {
                                        AddImage(x, y, w, h, n, ImageHelper.GetDisplayImagePath(path, n + sheetPosition.Type, colorStyle), overprint, rotateFlipType, appearance, roundedCorners);
                                    }

                                    AddText(x + 0.5F, y + 0.5F, w - 1, sheetPosition.Position, "Times New Roman", 7, fontBold: true, alignment: Alignment.Left);
                                    AddText(x + 0.5F, y + 0.5F, w - 1, sheetPosition.Type, "Times New Roman", 7, fontBold: true, alignment: Alignment.Right);
                                    AddText(x + 0.5F, y + h - 2.5F, w - 1, sheetPosition.Description, "Times New Roman", 7, fontBold: true, alignment: Alignment.Centered);
                                }
                                else
                                {
                                    AddImage(x, y, w, h, n, image, overprint, rotateFlipType, appearance, roundedCorners);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public SizeF AddText(float x, float y, float width, string text, string fontName, float fontSize, bool fontBold = false, bool fontItalic = false, Color? foreColor = null, Alignment alignment = Alignment.Left, bool screenOnly = false)
        {
            Font font = new Font(fontName, fontSize, (fontBold ? FontStyle.Bold : 0) | (fontItalic ? FontStyle.Italic : 0));

            //text = text.Trim();

            if (text == "~")
            {
                text = "";
            }
            else if (text == "!")
            {
                text = " ";
            }

            if (text == "")
            {
                return new SizeF(0, graphics.MeasureString("X", font).Height);
            }

            if (text.Contains(HtmlHelper.cBreak))
            {
                string[] texts = text.Split(HtmlHelper.cBreak, joinAgainExceptFirstOne: true);

                SizeF _1st = AddText(x, y, width, texts[0], fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);
                SizeF _2nd = AddText(x, y + _1st.Height, width, texts[1], fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);

                SizeF size = new SizeF(Math.Max(_1st.Width, _2nd.Width), _1st.Height + _2nd.Height);

                return size;
            }
            else
            {
                RectangleF textSize = this.graphics.MeasureText(text, fontName, fontSize, fontBold, fontItalic);

                if (width != 0 && textSize.Width > width)
                {
                    text = text.Replace("  ", " ");

                    for (int i = text.Length - 1; i >= 0; i--)
                    {
                        if (text[i] == ' ' && this.graphics.MeasureText(text.Substring(0, i), fontName, fontSize, fontBold, fontItalic).Width <= width)
                        {
                            return AddText(x, y, width, text.Substring(0, i) + HtmlHelper.cBreak + text.Substring(i + 1), fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);
                        }
                    }

                    return AddText(x, y, 4096, text, fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);
                }
                else
                {
                    float xNew = x - textSize.Left;

                    switch (alignment)
                    {
                        case Alignment.Left:
                            break;
                        case Alignment.Centered:
                            xNew += (width - textSize.Width) / 2;
                            break;
                        case Alignment.Right:
                            xNew += width - textSize.Width;
                            break;
                    }

                    List<string> texts = GraphicsHelper.SplitText(text, fontName, out string newFontName, fontSize, out float newFontSize, fontBold, out bool newFontBold, fontItalic, out bool newFontItalic, out float verticalShift);

                    if (texts != null)
                    {
                        SizeF _1st = AddText(xNew, y, width, texts[0], fontName, fontSize, fontBold, fontItalic, foreColor, Alignment.Left, screenOnly);
                        SizeF _2nd = AddText(xNew + _1st.Width, y + verticalShift, width, texts[1], newFontName, newFontSize, newFontBold, newFontItalic, foreColor, Alignment.Left, screenOnly);

                        SizeF size = new SizeF(_1st.Width + _2nd.Width, _1st.Height);

                        return size;
                    }
                    else
                    {
                        Artifact artifact = new Artifact(ArtifactType.Text);

                        artifact.X = xNew;
                        artifact.Y = y;
                        artifact.Text = text;
                        artifact.TextColor = foreColor == null ? new SolidBrush(Color.Black) : new SolidBrush((Color)foreColor);
                        artifact.Font = font;
                        artifact.screenOnly = screenOnly;

                        artifact.Width = textSize.Width;
                        artifact.Height = textSize.Height;

                        artifacts.Add(artifact);

                        if (!screenOnly)
                        {
                            //Debug.WriteLine(string.Format("\t'{0}',x={1},y={2},[{3},{4},{5}] w={6} h={7}", text, Math.Round(artifact.X, 2), Math.Round(artifact.Y, 2), fontName, fontSize, fontBold, Math.Round(artifact.Width, 2), Math.Round(artifact.Height, 2)));
                        }

                        return new SizeF(artifact.Width, artifact.Height);
                    }
                }
            }
        }

        public float GetTextHeight(string fontName, float fontSize)
        {
            Font font = new Font(fontName, fontSize);

            SizeF size = this.graphics.MeasureString("X", font);

            return size.Height;
        }
    }
}

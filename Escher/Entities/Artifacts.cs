using System;
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

        private List<Artifact> artifacts;

        public Artifacts(Graphics graphics, float scale)
        {
            this.graphics = graphics;
            this.scale = scale;

            this.artifacts = new List<Artifact>();
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

        private void AddCursor(float x, float y)
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

        private void AddImage(float x, float y, float width, float height, string number, Image image)
        {
            Artifact artifact = new Artifact(ArtifactType.Image);

            artifact.X = x;
            artifact.Y = y;
            artifact.Width = width;
            artifact.Height = height;
            artifact.Number = number;
            artifact.Picture = image;

            artifacts.Add(artifact);
        }

        public void AddImage(string path, string number, float x, float y, float width, float height, float extraWidth, float extraHeight, Shape shape, Appearance appearance, string picture, ColorStyle colorStyle)
        {
            number = !string.IsNullOrEmpty(picture) ? picture.Trim() : number.Trim();

            if (number.Contains('('))
            {
                number = number.Substring(0, number.IndexOf('(') - 1).Trim();
            }

            Image image = null;

            string n = number;

            while (image == null && n != "")
            {
                image = ImageHelper.GetDisplayImage(path, n, colorStyle);

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

            switch (shape)
            {
                case Shape.Triangle45:
                    AddImage(x + 3 * 5 / (float) Math.Sqrt(2), y + 2 * 5 / (float) Math.Sqrt(2), width - 2 * 5 / (float) Math.Sqrt(2), height - 5 - 2 * 5 / (float) Math.Sqrt(2), n, image);
                    break;
                case Shape.Triangle60Inverted:
                    AddImage(x + 5 + 5 / (float)Math.Sqrt(3), y + 5, width - 2 * (5 + 5 / (float)Math.Sqrt(3)), height - 2 * (5 + 5 / (float)Math.Sqrt(3)), n, image);
                    break;
                case Shape.Triangle60:
                    AddImage(x + 5 + 5 / (float)Math.Sqrt(3), y + 2 * 5, width - 2 * (5 + 5 / (float)Math.Sqrt(3)), height - 2 * (5 + 5 / (float)Math.Sqrt(3)), n, image);
                    break;
                case Shape.RectangleRotated:
                    AddImage(x + 5 * (float)Math.Sqrt(2), y + 5 * (float)Math.Sqrt(2), width - 2 * 5 * (float)Math.Sqrt(2), height - 2 * 5 * (float)Math.Sqrt(2), n, image);
                    break;
                default:
                    for (int i = 0; i < appearance.NumberOfStamps(); i++)
                    {
                        float z = 1;
                        switch (appearance)
                        {
                            case Appearance.Singular:
                                x += 4;
                                y += 4;
                                width -= 8;
                                height -= 8;
                                break;
                            default:
                                throw new Exception("todo");
                        }
                        AddImage(x, y, z * width, z * height, n, image);
                    }
                    break;
            }
        }

        public SizeF AddText(float x, float y, float width, string text, string fontName, float fontSize, bool fontBold = false, bool fontItalic = false, Color? foreColor = null, Alignment alignment = Alignment.Left, bool screenOnly = false)
        {
            //!TODO
            //switch (fontSize)
            //{
            //    case 7: fontSize = 7.2F; break;
            //}
            Font font = new Font(fontName, fontSize, (fontBold ? FontStyle.Bold : 0) | (fontItalic ? FontStyle.Italic : 0));

            text = text.Trim();

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

            int linefeed = text.IndexOf('%');

            if (linefeed >= 0)
            {
                string[] texts = new string[] { text.Substring(0, linefeed), text.Substring(linefeed + 1)};

                SizeF first = AddText(x, y, width, texts[0], fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);

                SizeF second = AddText(x, y + first.Height, width, texts[1], fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);

                return new SizeF(Math.Max(first.Width, second.Width), first.Height + second.Height);
            }
            else if (width != 0 && GetTextDimension(graphics, text, font).Width > width)
            {
                text = text.Replace("  ", " ");

                for (int i = text.Length - 1; i >= 0; i--)
                {
                    if (text[i] == ' ' && GetTextDimension(graphics, text.Substring(0, i), font).Width <= width)
                    {
                        return AddText(x, y, width, text.Substring(0, i) + "%" + text.Substring(i + 1), fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);
                    }
                }

                return AddText(x, y, 4096, text, fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);
            }
            else if (text.Contains("<b>"))
            {
                string[] texts = text.Split("<b>", joinAgainExceptFirstOne: true);

                x += AddText(x, y, width, texts[0], fontName, fontSize, false, fontItalic, foreColor, alignment, screenOnly).Width;

                texts = texts[1].Split("</b>", joinAgainExceptFirstOne: true);

                x += AddText(x, y, width, texts[0], fontName, fontSize, true, fontItalic, foreColor, alignment, screenOnly).Width;

                return AddText(x, y, width, texts[1], fontName, fontSize, false, fontItalic, foreColor, alignment, screenOnly);
            }
            else if (text.Contains("<i>"))
            {
                string[] texts = text.Split("<i>", joinAgainExceptFirstOne: true);

                x += AddText(x, y, width, texts[0], fontName, fontSize, fontBold, false, foreColor, alignment, screenOnly).Width;

                texts = texts[1].Split("</i>", joinAgainExceptFirstOne: true);

                x += AddText(x, y, width, texts[0], fontName, fontSize, fontBold, true, foreColor, alignment, screenOnly).Width;

                return AddText(x, y, width, texts[1], fontName, fontSize, fontBold, false, foreColor, alignment, screenOnly);
            }
            else if (text.Contains("<s>"))
            {
                string[] texts = text.Split("<s>", joinAgainExceptFirstOne: true);

                x += AddText(x, y, width, texts[0], fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly).Width;

                texts = texts[1].Split("</s>", joinAgainExceptFirstOne: true);

                x += AddText(x, y - 0.5F, width, texts[0], fontName, fontSize - 1, fontBold, fontItalic, foreColor, alignment, screenOnly).Width;

                return AddText(x, y, width, texts[1], fontName, fontSize, fontBold, fontItalic, foreColor, alignment, screenOnly);
            }
            else
            {
                Artifact artifact = new Artifact(ArtifactType.Text);

                artifact.X = x;
                artifact.Y = y;
                artifact.Text = text;
                artifact.TextColor = foreColor == null ? new SolidBrush(Color.Black) : new SolidBrush((Color)foreColor);
                artifact.Font = font;
                artifact.screenOnly = screenOnly;

                RectangleF dimension = GetTextDimension(this.graphics, artifact.Text, artifact.Font);

                artifact.Width = dimension.Width;
                artifact.Height = this.graphics.MeasureString("X", font).Height; // fonts.GetTextHeight(graphics, fontName, fontSize); // font.GetHeight(graphics); // dimension.Height;

                artifact.X -= dimension.Left;
                //artifact.Y -= dimension.Top;

                switch (alignment)
                {
                    case Alignment.Left:
                        break;
                    case Alignment.Centered:
                        artifact.X += (width - artifact.Width) / 2;
                        break;
                    case Alignment.Right:
                        artifact.X += width - artifact.Width;
                        break;

                }

                if (!screenOnly)
                {
                    string debug = string.Format("\t'{0}',x={1},y={2},[{3},{4},{5}] w={6} h={7}", text, Math.Round(artifact.X, 2), Math.Round(artifact.Y, 2), fontName, fontSize, fontBold, Math.Round(artifact.Width, 2), Math.Round(artifact.Height, 2));
                    Debug.WriteLine(debug);
                    debug = debug;
                }

                artifacts.Add(artifact);

                return new SizeF(artifact.Width, artifact.Height);
            }
        }

        private RectangleF GetTextDimension(Graphics graphics, string text, Font font)
        {
            // http://csharphelper.com/blog/2015/02/measure-character-positions-when-drawing-long-strings-in-c/

            CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, text.Length) };

            StringFormat stringFormat = new StringFormat();
            stringFormat.Trimming = StringTrimming.None;
            stringFormat.FormatFlags = StringFormatFlags.NoClip;
            stringFormat.SetMeasurableCharacterRanges(ranges);

            //StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
            //StringFormat stringFormat = new StringFormat();
            //stringFormat.Alignment = StringAlignment.Near;
            //stringFormat.LineAlignment = StringAlignment.Near;
            //stringFormat.Trimming = StringTrimming.None;
            //stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            //stringFormat.SetMeasurableCharacterRanges(ranges);

            RectangleF displayRectangleF = new RectangleF(0, 0, 4096, 4096);
            Region[] regions = graphics.MeasureCharacterRanges(text, font, displayRectangleF, stringFormat);
            RectangleF bounds = regions[0].GetBounds(graphics);

            return new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        public float GetTextHeight(string fontName, float fontSize)
        {
            Font font = new Font(fontName, fontSize);

            return this.graphics.MeasureString("X", font).Height;
        }

        public float GetTextWidth(string text, string fontName, float fontSize, bool fontBold = false, bool fontItalic = false)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }
            else
            {
                Font font = new Font(fontName, fontSize, (fontBold ? FontStyle.Bold : 0) | (fontItalic ? FontStyle.Italic : 0));

                return GetTextDimension(this.graphics, text, font).Width;
            }
        }
    }
}

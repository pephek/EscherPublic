using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Escher
{
    public class Artifacts
    {
        private List<Artifact> artifacts;

        private Graphics graphics;
        private float scale;

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

        public float AddText(float x, float y, float width, string text, string fontName, float fontSize, bool fontBold = false, bool fontItalic = false, Color? foreColor = null, Alignment alignment = Alignment.Left, bool screenOnly = false)
        {
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
                return 0; // GetTextDimension(graphics, "", font).Height;
            }

            int linefeed = text.IndexOf('%');

            if (linefeed >= 0)
            {
                string[] texts = new string[] { text.Substring(0, linefeed), text.Substring(linefeed + 1)};

                float height = AddText(x, y, width, texts[0], fontName, fontSize, fontBold, fontBold, foreColor, alignment, screenOnly);

                return AddText(x, y + height, width, texts[1], fontName, fontSize, fontBold, fontBold, foreColor, alignment, screenOnly);
            }
            else if (width != 0 && GetTextDimension(graphics, text, font).Width > width)
            {
                text = text.Replace("  ", " ");

                for (int i = text.Length - 1; i >= 0; i--)
                {
                    if (text[i] == ' ' && GetTextDimension(graphics, text.Substring(0, i), font).Width <= width)
                    {
                        return AddText(x, y, width, text.Substring(0, i) + "%" + text.Substring(i + 1), fontName, fontSize, fontBold, fontBold, foreColor, alignment, screenOnly);
                    }
                }

                return AddText(x, y, 4096, text, fontName, fontSize, fontBold, fontBold, foreColor, alignment, screenOnly);
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

                GraphicsUnit pageUnit = graphics.PageUnit;

                graphics.PageUnit = GraphicsUnit.Display;

                RectangleF dimension = GetTextDimension(graphics, artifact.Text, artifact.Font);

                artifact.Width = dimension.Width / this.scale;
                artifact.Height = dimension.Height / this.scale;

                artifact.X -= dimension.Left / this.scale;

                graphics.PageUnit = pageUnit;

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
                artifacts.Add(artifact);

                return artifact.Height;
            }
        }

        private RectangleF GetTextDimension(Graphics graphics, string text, Font font)
        {
            CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, text.Length) };
            StringFormat stringFormat = new StringFormat();
            stringFormat.SetMeasurableCharacterRanges(ranges);
            RectangleF displayRectangleF = new RectangleF(0, 0, 4096, 4096);
            Region[] regions = graphics.MeasureCharacterRanges(text, font, displayRectangleF, stringFormat);
            RectangleF bounds = regions[0].GetBounds(graphics);

            return new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);
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

        public float GetTextHeight(string fontName, float fontSize, bool fontBold = false, bool fontItalic = false)
        {
            Font font = new Font(fontName, fontSize, (fontBold ? FontStyle.Bold : 0) | (fontItalic ? FontStyle.Italic : 0));

            return GetTextDimension(this.graphics, "X", font).Height;
        }
    }
}

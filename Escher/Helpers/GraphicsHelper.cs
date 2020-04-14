using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class GraphicsHelper
    {
        private static int Minimum(int[] array)
        {
            int minimum = -1;

            for (int a = 0; a < array.Length; a++)
            {
                if (array[a] >= 0)
                {
                    if (minimum == -1)
                    {
                        minimum = array[a];
                    }
                    else
                    {
                        minimum = Math.Min(minimum, array[a]);
                    }
                }
            }

            return minimum;
        }

        public static List<string> SplitText(string text, float fontSize, out float newFontSize, bool fontBold, out bool newFontBold, bool fontItalic, out bool newFontItalic)
        {
            List<string> texts = null;

            newFontSize = fontSize;
            newFontBold = fontBold;
            newFontItalic = fontItalic;

            int bold = text.IndexOf(HtmlHelper.cBold);
            int boldEnd = text.IndexOf(HtmlHelper.cBoldEnd);
            int italic = text.IndexOf(HtmlHelper.cItalic);
            int italicEnd = text.IndexOf(HtmlHelper.cItalicEnd);
            int superscript = text.IndexOf(HtmlHelper.cSuperscript);
            int superscriptEnd = text.IndexOf(HtmlHelper.cSuperscriptEnd);

            int minimum = Minimum(new int[] { bold, boldEnd, italic, italicEnd, superscript, superscriptEnd });

            if (minimum >= 0)
            {
                if (bold == minimum)
                {
                    texts = text.Split(HtmlHelper.cBold, joinAgainExceptFirstOne: true).ToList();
                    newFontBold = true;
                }
                else if (minimum == boldEnd)
                {
                    texts = text.Split(HtmlHelper.cBoldEnd, joinAgainExceptFirstOne: true).ToList();
                    newFontBold = false;
                }
                else if (minimum == italic)
                {
                    texts = text.Split(HtmlHelper.cItalic, joinAgainExceptFirstOne: true).ToList();
                    newFontItalic = true;
                }
                else if (minimum == italicEnd)
                {
                    texts = text.Split(HtmlHelper.cItalicEnd, joinAgainExceptFirstOne: true).ToList();
                    newFontItalic = false;
                }
                else if (minimum == superscript)
                {
                    texts = text.Split(HtmlHelper.cSuperscript, joinAgainExceptFirstOne: true).ToList();
                    newFontSize -= 1;
                }
                else if (minimum == superscriptEnd)
                {
                    texts = text.Split(HtmlHelper.cSuperscriptEnd, joinAgainExceptFirstOne: true).ToList();
                    newFontSize += 1;
                }
                else
                {
                    throw new Exception("Did not find a minimum for <b>,</b>,<i>,</i>,<s>,</s>");
                }
            }

            return texts;
        }
    }

    public static class GraphicsMethodExtensions
    {
        public static RectangleF MeasureText(this Graphics graphics, string text, string fontName, float fontSize, bool fontBold, bool fontItalic)
        {
            RectangleF _1st;
            RectangleF _2nd;

            RectangleF size;

            if (text.Contains(HtmlHelper.cBreak))
            {
                string[] texts = text.Split(HtmlHelper.cBreak, joinAgainExceptFirstOne: true);

                _1st = MeasureText(graphics, texts[0], fontName, fontSize, fontBold, fontItalic);
                _2nd = MeasureText(graphics, texts[1], fontName, fontSize, fontBold, fontItalic);

                size = new RectangleF(0, 0, Math.Max(_1st.Width, _2nd.Width), _1st.Height + _2nd.Height);
            }
            else
            {
                List<string> texts = GraphicsHelper.SplitText(text, fontSize, out float newFontSize, fontBold, out bool newFontBold, fontItalic, out bool newFontItalic);

                if (texts != null)
                {
                    _1st = MeasureText(graphics, texts[0], fontName, fontSize, fontBold, fontItalic);
                    _2nd = MeasureText(graphics, texts[1], fontName, newFontSize, newFontBold, newFontItalic);

                    size = new RectangleF(0, 0, _1st.Width + _2nd.Width, _1st.Height);
                }
                else if (text == "")
                {
                    return new RectangleF();
                }
                else
                {
                    Font font = new Font(fontName, fontSize, (fontBold ? FontStyle.Bold : 0) | (fontItalic ? FontStyle.Italic : 0));

                    CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, text.Length) };

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Trimming = StringTrimming.None;
                    stringFormat.FormatFlags = StringFormatFlags.NoClip;
                    stringFormat.SetMeasurableCharacterRanges(ranges);

                    RectangleF displayRectangleF = new RectangleF(0, 0, 4096, 4096);
                    Region[] regions = graphics.MeasureCharacterRanges(text, font, displayRectangleF, stringFormat);
                    RectangleF bounds = regions[0].GetBounds(graphics);

                    float height = graphics.MeasureString("X", font).Height;

                    size = new RectangleF(bounds.X, bounds.Y, bounds.Width, height);
                }
            }

            return size;
        }
    }
}

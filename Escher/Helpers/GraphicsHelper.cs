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

        public static List<string> SplitText(string text, string fontName, out string newFontName, float fontSize, out float newFontSize, bool fontBold, out bool newFontBold, bool fontItalic, out bool newFontItalic, out float verticalShift)
        {
            List<string> texts = null;

            newFontName = fontName;
            newFontSize = fontSize;
            newFontBold = fontBold;
            newFontItalic = fontItalic;

            verticalShift = 0;

            if (text.IndexOf('<') >= 0)
            {
                int bold = text.IndexOf(HtmlHelper.cBold, StringComparison.Ordinal);
                int boldEnd = text.IndexOf(HtmlHelper.cBoldEnd, StringComparison.Ordinal);
                int italic = text.IndexOf(HtmlHelper.cItalic, StringComparison.Ordinal);
                int italicEnd = text.IndexOf(HtmlHelper.cItalicEnd, StringComparison.Ordinal);
                int superscript = text.IndexOf(HtmlHelper.cSuperscript, StringComparison.Ordinal);
                int superscriptEnd = text.IndexOf(HtmlHelper.cSuperscriptEnd, StringComparison.Ordinal);
                int font = text.IndexOf(HtmlHelper.cFont, StringComparison.Ordinal);
                int fontEnd = text.IndexOf(HtmlHelper.cFontEnd, StringComparison.Ordinal);

                int minimum = Minimum(new int[] { bold, boldEnd, italic, italicEnd, superscript, superscriptEnd, font, fontEnd });

                if (minimum >= 0)
                {
                    if (minimum == bold)
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
                    else if (minimum == font)
                    {
                        texts = text.Split(HtmlHelper.cFont, joinAgainExceptFirstOne: true).ToList();
                        newFontName = "Times New Roman";
                        newFontSize -= 10;
                        verticalShift = 3;
                    }
                    else if (minimum == fontEnd)
                    {
                        texts = text.Split(HtmlHelper.cFontEnd, joinAgainExceptFirstOne: true).ToList();
                        newFontName = "Darleston";
                        newFontSize += 10;
                    }
                    else
                    {
                        throw new Exception("Did not find a minimum for <b>,</b>,<i>,</i>,<s>,</s>,<f>,</f>");
                    }
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
                List<string> texts = GraphicsHelper.SplitText(text, fontName, out string newFontName, fontSize, out float newFontSize, fontBold, out bool newFontBold, fontItalic, out bool newFontItalic, out float ignore);

                if (texts != null)
                {
                    _1st = MeasureText(graphics, texts[0], fontName, fontSize, fontBold, fontItalic);
                    _2nd = MeasureText(graphics, texts[1], newFontName, newFontSize, newFontBold, newFontItalic);

                    //RectangleF try1 = MeasureText(graphics, "X", "Darleston", 25, false, false);
                    //RectangleF try2 = MeasureText(graphics, "X", "Microsoft Himalaya", 25 - 7, false, true);

                    //try1 = MeasureText(graphics, "X", "Darleston", 20, false, false);
                    //try2 = MeasureText(graphics, "X", "Microsoft Himalaya", 20 - 7, false, true);

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

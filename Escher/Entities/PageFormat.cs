using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class PageFormat
    {
        public readonly string FormatName;
        public readonly TitleStyle TitleStyle;
        public readonly string TitleFont;
        public readonly int PageWidth;
        public readonly int PageHeight;
        public readonly int MarginLeft;
        public readonly int MarginRight;
        public readonly int MarginTop;
        public readonly int MarginBottom;
        public readonly int FreeLeft;
        public readonly int FreeRight;
        public readonly int FreeTop;
        public readonly int FreeBottom;
        public readonly bool PrePrintedBorder;
        public readonly bool PrePrintedTitle;

        public readonly RectangleF Border;
        public readonly RectangleF Free;

        public PageFormat(string FormatName, TitleStyle TitleStyle, string TitleFont, int PageWidth, int PageHeight, int MarginLeft, int MarginRight, int MarginTop, int MarginBottom, int FreeLeft, int FreeRight, int FreeTop, int FreeBottom, bool PrePrintedBorder, bool PrePrintedTitle)
        {
            this.FormatName = FormatName;
            this.TitleStyle = TitleStyle;
            this.TitleFont = TitleFont;
            this.PageWidth = PageWidth;
            this.PageHeight = PageHeight;
            this.MarginLeft = MarginLeft;
            this.MarginRight = MarginRight;
            this.MarginTop = MarginTop;
            this.MarginBottom = MarginBottom;
            this.FreeLeft = FreeLeft;
            this.FreeRight = FreeRight;
            this.FreeTop = FreeTop;
            this.FreeBottom = FreeBottom;
            this.PrePrintedBorder = PrePrintedBorder;
            this.PrePrintedTitle = PrePrintedTitle;

            this.Border = new RectangleF(
                this.MarginLeft,
                this.MarginTop,
                this.PageWidth - (this.MarginLeft + this.MarginRight),
                this.PageHeight - (this.MarginTop + this.MarginBottom)
            );
            this.Free = new RectangleF(
                this.MarginLeft + this.FreeLeft,
                this.MarginTop + this.FreeTop,
                this.PageWidth - (this.MarginLeft + this.FreeLeft + this.MarginRight + this.FreeRight),
                this.PageHeight - (this.MarginTop + this.FreeTop + this.MarginBottom + this.FreeBottom)
            );

            Debug.Assert((this.Free.Width < this.Border.Width) || (this.Free.Height < this.Border.Height));
        }
    }

    public class PageFormats
    {
        private static List<PageFormat> pageFormats = new List<PageFormat>();

        public static void Load(string PageFormatsFile)
        {
            if (!File.Exists(PageFormatsFile))
            {
                App.SetException((string.Format("Page formats file '{0}' does not exist.", PageFormatsFile)));
            }

            pageFormats = new List<PageFormat>();

            using (var streamReader = new StreamReader(PageFormatsFile))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    string titleStyleString = csvReader.GetField("Title.Style");

                    if (!Enum.TryParse(csvReader.GetField("Title.Style"), true, out TitleStyle titleStyle))
                    {
                        App.SetException((string.Format("Unknown title style '{0}'.", csvReader.GetField("Title.Style"))));
                    }

                    pageFormats.Add(new PageFormat(
                        csvReader.GetField("Format"),
                        titleStyle,
                        csvReader.GetField("Title.Font"),
                        csvReader.GetField<int>("Page.Width"),
                        csvReader.GetField<int>("Page.Height"),
                        csvReader.GetField<int>("Margin.Left"),
                        csvReader.GetField<int>("Margin.Right"),
                        csvReader.GetField<int>("Margin.Top"),
                        csvReader.GetField<int>("Margin.Bottom"),
                        csvReader.GetField<int>("Free.Left"),
                        csvReader.GetField<int>("Free.Right"),
                        csvReader.GetField<int>("Free.Top"),
                        csvReader.GetField<int>("Free.Bottom"),
                        csvReader.GetField<bool>("Page.PrePrintedBorder"),
                        csvReader.GetField<bool>("Page.PrePrintedTitle")
                    ));
                }
            }
        }

        public static List<PageFormat> Get()
        {
            return pageFormats;
        }

        public static PageFormat Get(string formatName)
        {
            PageFormat pageFormat = pageFormats.FirstOrDefault(format => format.FormatName == formatName);

            return pageFormat;
        }
    }
}

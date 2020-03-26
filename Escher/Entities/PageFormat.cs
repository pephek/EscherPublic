using CsvHelper;
using System;
using System.Collections.Generic;
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
        private readonly TitleStyle TitleStyle;
        private readonly string TitleFont;
        private readonly int PageWidth;
        private readonly int PageHeight;
        private readonly int MarginLeft;
        private readonly int MarginRight;
        private readonly int MarginTop;
        private readonly int MarginBottom;
        private readonly int FreeLeft;
        private readonly int FreeRight;
        private readonly int FreeTop;
        private readonly int FreeBottom;
        private readonly bool PrePrintedBorder;
        private readonly bool PrePrintedTitle;

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
        }

        private static List<PageFormat> _PageFormats = new List<PageFormat>();

        public static void LoadPageFormats(string PageFormatsFile)
        {
            if (!File.Exists(PageFormatsFile))
            {
                App.SetException((string.Format("Page formats file '{0}' does not exist.", PageFormatsFile)));
            }

            List<PageFormat> pageFormats = new List<PageFormat>();

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

                    _PageFormats.Add(new PageFormat(
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

        public static List<PageFormat> GetPageFormats()
        {
            return _PageFormats;
        }
    }
}

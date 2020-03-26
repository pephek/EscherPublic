using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class StampSheet
    {
        private const double cFrameMargin = 2;

        private static Dictionary<SheetNumber, Sheet> sheets;

        private static void Initialize()
        {
            sheets = new Dictionary<SheetNumber, Sheet>();

            Sheet sheet;

            int n = 0;

            // +++
            // Nederland 1852
            // ---

            sheet = new Sheet();

            sheet.HorizontalCount = 5;
            sheet.VerticalCount = 5;
            sheet.StampWidth = 20;
            sheet.StampHeight = 23;

            sheet.SetNumberfOfBlocksAndPictures(4, 1);

            for (int b = 0; b < sheet.Blocks.Length; b++)
            {
                SheetBlock block = sheet.Blocks[b];

                for (int p = 0; p < block.Picture.Length; p++)
                {
                    block.Picture[p] = App.GetSetting("ImagesFolder") + "\\Nederland\\Platen\\print\\n.Jpg";
                }

                switch (b + 1)
                {
                    case 1: n = 1;  break;
                    case 2: n = 26; break;
                    case 3: n = 51; break;
                    case 4: n = 76; break;
                }

                for (int v = 0; v < sheet.VerticalCount; v++)
                {
                    for (int h = 0; h < sheet.HorizontalCount; h++)
                    {
                        block.Type[v, h] = "1";
                        block.Position[v, h] = n.ToString();
                        n++;
                    }
                }
            }

            sheets[SheetNumber.Nederland1852] = sheet;

            // +++
            // Nederland 1864
            // ---

            sheet = new Sheet();

            sheet.HorizontalCount = 5;
            sheet.VerticalCount = 5;
            sheet.StampWidth = 20;
            sheet.StampHeight = 24;

            sheet.SetNumberfOfBlocksAndPictures(4, 1);

            for (int b = 0; b < sheet.Blocks.Length; b++)
            {
                SheetBlock block = sheet.Blocks[b];

                for (int p = 0; p < block.Picture.Length; p++)
                {
                    block.Picture[p] = App.GetSetting("ImagesFolder") + "\\Nederland\\Platen\\print\\n1864.Jpg";
                }

                switch (b + 1)
                {
                    case 1: n = 1; break;
                    case 2: n = 6; break;
                    case 3: n = 51; break;
                    case 4: n = 56; break;
                }

                for (int v = 0; v < sheet.VerticalCount; v++)
                {
                    for (int h = 0; h < sheet.HorizontalCount; h++)
                    {
                        block.Type[v, h] = "1";
                        block.Position[v, h] = n.ToString();
                        n++;
                    }
                    n += 5;
                }
            }

            sheets[SheetNumber.Nederland1864] = sheet;

            // Overall sizes

            foreach (var s in sheets)
            {
                s.Value.SheetWidth = cFrameMargin + s.Value.MarginLeft + s.Value.HorizontalCount * s.Value.StampWidth + cFrameMargin + s.Value.MarginRight + (s.Value.Gutter ? s.Value.MarginLeft : 0);
                s.Value.SheetHeight = cFrameMargin + s.Value.MarginTop + s.Value.VerticalCount * s.Value.StampHeight + cFrameMargin + s.Value.MarginBottom;

                if (s.Key == SheetNumber.Nederland1852)
                {
                    s.Value.SheetWidth += 20;
                    s.Value.SheetHeight += 20;
                }
            }

        }

        public static void GetSize(string sheetNumber, out double width, out double height)
        {
            SheetNumber sheet;
            int block;

            if (sheets == null)
            {
                Initialize();
            }

            GetSheetAndBlockNumber(sheetNumber, out sheet, out block);

            if (sheets.ContainsKey(sheet))
            {
                width = sheets[sheet].SheetWidth;
                height = sheets[sheet].SheetHeight;
            }
            else
            {
                throw new Exception(string.Format("Stamp sheet '{0}' not initialized", sheetNumber));
            }
        }

        private static void GetSheetAndBlockNumber(string sheetNumber, out SheetNumber sheet, out int block)
        {
            if (!sheetNumber.Contains("#"))
            {
                block = 0;
            }
            else
            {
                block = Convert.ToInt32(sheetNumber.Substring(sheetNumber.Length - 1));

                sheetNumber = sheetNumber.Substring(0, sheetNumber.Length - 2);
            }

            switch (sheetNumber.ToLower())
            {
                case "yverttellierreunion12":
                    sheet = SheetNumber.YvertTellierReunion12;
                    break;
                case "yverttellierreunion31":
                    sheet = SheetNumber.YvertTellierReunion31;
                    break;
                case "yverttellierreunion31x":
                    sheet = SheetNumber.YvertTellierReunion31x;
                    break;
                case "yverttellierreunion45":
                    sheet = SheetNumber.YvertTellierReunion45;
                    break;
                case "yverttellierreunion45x":
                    sheet = SheetNumber.YvertTellierReunion45x;
                    break;
                case "yverttellierreunion80":
                    sheet = SheetNumber.YvertTellierReunion80;
                    break;
                case "yverttellierreunion81":
                    sheet = SheetNumber.YvertTellierReunion81;
                    break;
                case "nederland1852":
                    sheet = SheetNumber.Nederland1852;
                    break;
                case "nederland1864":
                    sheet = SheetNumber.Nederland1864;
                    break;
                default:
                    throw new Exception(string.Format("Unknown sheet number '{0}'", sheetNumber));
            }
        }
    }
}

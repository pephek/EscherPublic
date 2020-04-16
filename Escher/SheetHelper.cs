using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public static class SheetHelper
    {
        private static Dictionary<SheetNumber, Sheet> sheets;

        static SheetHelper()
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
            sheet.Color = Color.Black;
            sheet.FontName = "Darleston";
            sheet.FontSize = 10;
            sheet.FontBold = true;

            sheet.SetNumberfOfBlocksAndPictures(4, 1);

            for (int b = 0; b < sheet.Blocks.Length; b++)
            {
                SheetBlock block = sheet.Blocks[b];

                for (int p = 0; p < block.Picture.Length; p++)
                {
                    block.Picture[p] = "n";
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
                        block.Type[v, h] = 1;
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
            sheet.Color = Color.Black;
            sheet.FontName = "Darleston";
            sheet.FontSize = 7;
            sheet.FontBold = true;

            sheet.SetNumberfOfBlocksAndPictures(4, 1);

            for (int b = 0; b < sheet.Blocks.Length; b++)
            {
                SheetBlock block = sheet.Blocks[b];

                for (int p = 0; p < block.Picture.Length; p++)
                {
                    block.Picture[p] = "n1864";
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
                        block.Type[v, h] = 1;
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
                s.Value.SheetWidth = s.Value.HorizontalCount * s.Value.StampWidth;
                s.Value.SheetHeight =s.Value.VerticalCount * s.Value.StampHeight;
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
                block = Convert.ToInt32(sheetNumber.Substring(sheetNumber.Length - 1)) - 1;

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

        public static void GetSize(string sheetNumber, out float width, out float height)
        {
            SheetNumber sheet;
            int block;

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

        public static Sheet GetSheet(string number, out SheetBlock block)
        {
            SheetNumber sheetNumber;
            int blockNumber;

            GetSheetAndBlockNumber(number, out sheetNumber, out blockNumber);

            if (!sheets.ContainsKey(sheetNumber))
            {
                throw new Exception(string.Format("Stamp sheet '{0}' not initialized", sheetNumber));
            }

            Sheet sheet = sheets[sheetNumber];

            block = sheet.Blocks[blockNumber];

            return sheet;
        }

        public static SheetPosition GetSheetPosition(string sheetPositions, int position)
        {
            SheetPosition sheetPosition = null;

            try
            {
                sheetPositions = sheetPositions.Substring(1, sheetPositions.Length - 2);

                string[] positions = sheetPositions.Split("),(");

                if (position < positions.Length)
                {
                    sheetPosition = new SheetPosition();

                    string[] attributes = positions[position].Split(":")[1].Split(",");

                    sheetPosition.Position = attributes[0];
                    sheetPosition.Type = attributes[1];
                    sheetPosition.Description = attributes[2];
                }
            }
            catch
            {

            }

            return sheetPosition;
        }
    }
}

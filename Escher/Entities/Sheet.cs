using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Sheet
    {
        public int HorizontalCount;
        public int VerticalCount;
        public float StampWidth;
        public float StampHeight;
        public float SheetWidth;
        public float SheetHeight;
        public SheetBlock[] Blocks;
        public bool Gutter;
        public Color? Color;
        public string FontName;
        public int FontSize;
        public bool FontBold;

        public Sheet()
        {
            Gutter = false;
            Color = null;
        }

        public void SetNumberfOfBlocksAndPictures(int blocks, int pictures)
        {
            Blocks = new SheetBlock[blocks];

            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = new SheetBlock(VerticalCount, HorizontalCount, pictures);
            }
        }
    }
}

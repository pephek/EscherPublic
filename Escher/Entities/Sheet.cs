using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Sheet
    {
        public int HorizontalCount;
        public int VerticalCount;
        public double StampWidth;
        public double StampHeight;
        public double SheetWidth;
        public double SheetHeight;
        public double MarginLeft;
        public double MarginRight;
        public double MarginTop;
        public double MarginBottom;
        public SheetBlock[] Blocks;
        public bool Gutter;

        public Sheet()
        {
            MarginLeft = 0;
            MarginRight = 0;
            MarginTop = 0;
            MarginBottom = 0;
            Gutter = false;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class SheetBlock
    {
        public string[,] Type;
        public string[,] Position;
        public string[,] Description;
        public string[,] Variety;
        public string[] Picture;

        public SheetBlock(int vertical, int horizontal, int pictures)
        {
            Type = new string[vertical, horizontal];
            Position = new string[vertical, horizontal];
            Description = new string[vertical, horizontal];
            Variety = new string[vertical, horizontal];
            Picture = new string[pictures];
        }
    }
}

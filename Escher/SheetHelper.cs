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

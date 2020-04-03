using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum ColorStyle
    {
        Greyscale, // 0
        Color // 1
    }

    public static class ColorStyleExtensions
    {
        public static ColorStyle Toggle(this ColorStyle colorStyle)
        {
            switch (colorStyle)
            {
                case ColorStyle.Greyscale:
                    return ColorStyle.Color;
                case ColorStyle.Color:
                    return ColorStyle.Greyscale;
                default:
                    throw new ArgumentOutOfRangeException("colorStyle");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum FontSize
    {
        Small = 5,
        Medium = 6,
        Large = 7
    }

    public static class FontSizeExtensions
    {
        public static FontSize Next(this FontSize fontSize)
        {
            switch (fontSize)
            {
                case FontSize.Small:
                    return FontSize.Medium;
                case FontSize.Medium:
                    return FontSize.Large;
                case FontSize.Large:
                    return FontSize.Small;
                default:
                    throw new ArgumentOutOfRangeException("fontSize");
            }
        }
    }
}

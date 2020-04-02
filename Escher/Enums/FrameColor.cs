using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum FrameColor
    {
        Black,
        White
    }

    public static class FrameColorExtensions
    {
        public static Color GetColor(this FrameColor frameColor)
        {
            switch (frameColor)
            {
                case FrameColor.Black:
                    return Color.Black;
                case FrameColor.White:
                    return Color.White;
                default:
                    throw new ArgumentOutOfRangeException("frameColor");
            }
        }
    }
}

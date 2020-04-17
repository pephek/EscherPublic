using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum FrameStyle
    {
        Thick,
        ThinSolid,
        ThinDotted
    }

    public static class FrameStyleExtensions
    {
        public static FrameStyle Next(this FrameStyle frameStyle)
        {
            switch (frameStyle)
            {
                case FrameStyle.Thick:
                    return FrameStyle.ThinSolid;
                case FrameStyle.ThinSolid:
                    return FrameStyle.ThinDotted;
                case FrameStyle.ThinDotted:
                    return FrameStyle.Thick;
                default:
                    throw new ArgumentOutOfRangeException("frameStyle");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum ScreenMode
    {
        MatchPaper,
        MatchScreenHeight,
        MatchScreenWidth
    }

    public static class ScreenModeExtensions
    {
        public static ScreenMode Next(this ScreenMode screenMode)
        {
            switch (screenMode)
            {
                case ScreenMode.MatchPaper:
                    return ScreenMode.MatchScreenHeight;
                case ScreenMode.MatchScreenHeight:
                    return ScreenMode.MatchScreenWidth;
                case ScreenMode.MatchScreenWidth:
                    return ScreenMode.MatchPaper;
                default:
                    throw new ArgumentOutOfRangeException("screenMode");
            }
        }

        public static ScreenMode Prev(this ScreenMode screenMode)
        {
            switch (screenMode)
            {
                case ScreenMode.MatchPaper:
                    return ScreenMode.MatchScreenWidth;
                case ScreenMode.MatchScreenHeight:
                    return ScreenMode.MatchPaper;
                case ScreenMode.MatchScreenWidth:
                    return ScreenMode.MatchScreenHeight;
                default:
                    throw new ArgumentOutOfRangeException("screenMode");
            }
        }
    }
}

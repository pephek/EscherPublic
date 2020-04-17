using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum ScreenMode
    {
        MatchPaper,
        MatchScreenHeight,
        MatchScreenWidth,
        MatchRealLife
    }

    public static class ScreenModeExtensions
    {
        public static string ToText(this ScreenMode screenMode)
        {
            switch (screenMode)
            {
                case ScreenMode.MatchPaper:
                    return "Match Paper";
                case ScreenMode.MatchScreenHeight:
                    return "Match Screen Height";
                case ScreenMode.MatchScreenWidth:
                    return "Match Screen Width";
                case ScreenMode.MatchRealLife:
                    return "Match Real Life";
                default:
                    throw new ArgumentOutOfRangeException("screenMode");
            }
        }

        public static ScreenMode Next(this ScreenMode screenMode)
        {
            switch (screenMode)
            {
                case ScreenMode.MatchPaper:
                    return ScreenMode.MatchScreenHeight;
                case ScreenMode.MatchScreenHeight:
                    return ScreenMode.MatchScreenWidth;
                case ScreenMode.MatchScreenWidth:
                    return ScreenMode.MatchRealLife;
                case ScreenMode.MatchRealLife:
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
                    return ScreenMode.MatchRealLife;
                case ScreenMode.MatchScreenHeight:
                    return ScreenMode.MatchPaper;
                case ScreenMode.MatchScreenWidth:
                    return ScreenMode.MatchScreenHeight;
                case ScreenMode.MatchRealLife:
                    return ScreenMode.MatchScreenWidth;
                default:
                    throw new ArgumentOutOfRangeException("screenMode");
            }
        }
    }
}

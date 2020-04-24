using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum TitleStyle
    {
        Davo,
        Importa
    }

    public static class TitleStyleExtensions
    {
        public static TitleStyle Toggle(this TitleStyle titleStyle)
        {
            switch (titleStyle)
            {
                case TitleStyle.Davo:
                    return TitleStyle.Importa;
                case TitleStyle.Importa:
                    return TitleStyle.Davo;
                default:
                    throw new ArgumentOutOfRangeException("titleStyle");
            }
        }
    }
}

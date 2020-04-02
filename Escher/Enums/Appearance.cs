using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum Appearance
    {
        Singular, //0
        PairHorizontal, // 1
        PairVertical,// 2
        Block, // 3
        TeteBecheHorizontal, // 4
        TeteBecheVertical, // 5
        Sheet2x3, // 6
        Proof, // 7
        HorizontalStrip3, // 8
        HorizontalStrip4, // 9
        HorizontalStrip5, // 10
        HorizontalStrip6, // 11
        Rotated, // 12
        InterpanneauxHorizontal, // 13
        TeteBecheHorizontalGutter, // 14
        HorizontalGutterPair, // 15
        PaireCarnet, // 16
        BandePublicitaire, // 17
        VerticalStrip3, // 18
        VerticalStrip4, // 19
        VerticalStrip5, // 20
        VerticalStrip6, // 21
        ImperfRight, // 22
        ImperfLeft, // 23
        ImperfTop, // 24
        ImperfBottom // 25    
    }

    public static class AppearanceExtensions
    {
        public static int NumberOfStamps(this Appearance appearance)
        {
            switch (appearance)
            {
                case Appearance.Singular:
                    return (1);
                case Appearance.PairHorizontal:
                    return (2);
                case Appearance.PairVertical:
                    return (2);
                case Appearance.Block:
                    return (4);
                case Appearance.TeteBecheHorizontal:
                    return (2);
                case Appearance.TeteBecheVertical:
                    return (2);
                case Appearance.Sheet2x3:
                    return (6);
                case Appearance.Proof:
                    return (1);
                case Appearance.HorizontalStrip3:
                    return (3);
                case Appearance.HorizontalStrip4:
                    return (4);
                case Appearance.HorizontalStrip5:
                    return (5);
                case Appearance.HorizontalStrip6:
                    return (6);
                case Appearance.Rotated:
                    return (1);
                case Appearance.InterpanneauxHorizontal:
                    return (2);
                case Appearance.TeteBecheHorizontalGutter:
                    return (3);
                case Appearance.HorizontalGutterPair:
                    return (3);
                case Appearance.PaireCarnet:
                    return (2);
                case Appearance.BandePublicitaire:
                    return (1);
                case Appearance.VerticalStrip3:
                    return (3);
                case Appearance.VerticalStrip4:
                    return (4);
                case Appearance.VerticalStrip5:
                    return (5);
                case Appearance.VerticalStrip6:
                    return (6);
                case Appearance.ImperfRight:
                    return (1);
                case Appearance.ImperfLeft:
                    return (1);
                case Appearance.ImperfTop:
                    return (1);
                case Appearance.ImperfBottom:
                    return (1);
                default:
                    throw new ArgumentOutOfRangeException("appearance");
            }
        }
    }
}

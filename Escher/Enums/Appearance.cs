using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum Appearance
    {
        Singular,
        PairHorizontal,
        PairVertical,
        Block,
        TeteBecheHorizontal,
        TeteBecheVertical,
        Sheet2x3,
        Proof,
        HorizontalStrip3,
        HorizontalStrip4,
        HorizontalStrip5,
        HorizontalStrip6,
        Rotated,
        TeteBecheHorizontalGutter,
        TeteBecheVerticalGutter,
        HorizontalGutterPair,
        VerticalGutterPair,
        PaireCarnet,
        BandePublicitaire,
        VerticalStrip3,
        VerticalStrip4,
        VerticalStrip5,
        VerticalStrip6,
        ImperfRight,
        ImperfLeft,
        ImperfTop,
        ImperfBottom   
    }

    public static class AppearanceExtensions
    {
        public static int NumberOfStamps(this Appearance appearance)
        {
            switch (appearance)
            {
                case Appearance.Singular:
                case Appearance.Proof:
                case Appearance.Rotated:
                    return 1;

                case Appearance.ImperfRight:
                case Appearance.ImperfLeft:
                case Appearance.ImperfTop:
                case Appearance.ImperfBottom:
                    return 1;

                case Appearance.PairHorizontal:
                case Appearance.PairVertical:
                case Appearance.TeteBecheHorizontal:
                case Appearance.TeteBecheVertical:
                    return 2;

                case Appearance.TeteBecheHorizontalGutter:
                case Appearance.TeteBecheVerticalGutter:
                case Appearance.HorizontalGutterPair:
                case Appearance.VerticalGutterPair:
                    return 3;

                case Appearance.Block:
                    return 4;

                case Appearance.Sheet2x3:
                    return 6;

                case Appearance.HorizontalStrip3:
                case Appearance.VerticalStrip3:
                    return 3;

                case Appearance.HorizontalStrip4:
                case Appearance.VerticalStrip4:
                    return 4;

                case Appearance.HorizontalStrip5:
                case Appearance.VerticalStrip5:
                    return 5;

                case Appearance.HorizontalStrip6:
                case Appearance.VerticalStrip6:
                    return 6;

                case Appearance.PaireCarnet:
                    return 2;
                case Appearance.BandePublicitaire:
                    return 1;

                default:
                    throw new ArgumentOutOfRangeException("appearance");
            }
        }
    }
}

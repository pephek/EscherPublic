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
        InterpanneauxHorizontal,
        TeteBecheHorizontalGutter,
        TeteBecheVerticalGutter,
        HorizontalGutterPair,
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
                case Appearance.TeteBecheVerticalGutter:
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

using System;
using System.Collections.Generic;
using System.Drawing;
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
        Block5x5,
        Block2x5,
        Block2x5Rotated,
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
        HorizontalInterpanneaux,
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

                case Appearance.Block5x5:
                    return 25;

                case Appearance.Block2x5:
                case Appearance.Block2x5Rotated:
                    return 10;

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
                case Appearance.HorizontalInterpanneaux:
                    return 2;

                default:
                    throw new ArgumentOutOfRangeException("appearance");
            }
        }

        public static Size NumberOfHorizontalAndVerticalStamps(this Appearance appearance)
        {
            switch (appearance)
            {
                case Appearance.Singular:
                case Appearance.Proof:
                case Appearance.Rotated:
                case Appearance.ImperfRight:
                case Appearance.ImperfLeft:
                case Appearance.ImperfTop:
                case Appearance.ImperfBottom:
                    return new Size(1, 1);

                case Appearance.PairHorizontal:
                case Appearance.TeteBecheHorizontal:
                    return new Size(2, 1);

                case Appearance.PairVertical:
                case Appearance.TeteBecheVertical:
                    return new Size(1, 2);

                case Appearance.TeteBecheHorizontalGutter:
                case Appearance.HorizontalGutterPair:
                    return new Size(3, 1);

                case Appearance.TeteBecheVerticalGutter:
                case Appearance.VerticalGutterPair:
                    return new Size(1, 3);

                case Appearance.Block:
                    return new Size(2, 2);

                case Appearance.Block5x5:
                    return new Size(5, 5);

                case Appearance.Block2x5:
                case Appearance.Block2x5Rotated:
                    return new Size(2, 5);

                case Appearance.Sheet2x3:
                    return new Size(3, 2);

                case Appearance.HorizontalStrip3:
                    return new Size(3, 1);

                case Appearance.VerticalStrip3:
                    return new Size(1, 3);

                case Appearance.HorizontalStrip4:
                    return new Size(4, 1);

                case Appearance.VerticalStrip4:
                    return new Size(1, 4);

                case Appearance.HorizontalStrip5:
                    return new Size(5, 1);

                case Appearance.VerticalStrip5:
                    return new Size(1, 5);

                case Appearance.HorizontalStrip6:
                    return new Size(6, 1);

                case Appearance.VerticalStrip6:
                    return new Size(1, 6);

                case Appearance.BandePublicitaire:
                    return new Size(1, 1);

                case Appearance.PaireCarnet:
                    return new Size(1, 2);

                case Appearance.HorizontalInterpanneaux:
                    return new Size(3, 1);

                default:
                    throw new ArgumentOutOfRangeException("appearance");
            }
        }
    }
}

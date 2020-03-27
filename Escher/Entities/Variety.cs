using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Variety
    {
        public readonly string Number;
        public readonly string Description;
        public readonly FrameColor FrameColor;
        public readonly double Width;
        public readonly double Height;
        public readonly double ExtraWidth;
        public readonly double ExtraHeight;
        public readonly double Horizontal;
        public readonly double Vertical;
        public readonly string Title;
        public readonly Shape Shape;
        public readonly bool Skip;
        public readonly string Part;
        public readonly Appearance Appearance;
        public readonly string Picture;
        public readonly string Overprint;
        public readonly Alignment Alignment;
        public readonly string Sheet;

        // Not static data, but computed in target's scale mode while drawing the album page

        public double FrameLeft;
        public double FrameOffset;
        public double FrameWidth;

        public Variety(
            string number,
            string description,
            FrameColor frameColor,
            double width,
            double height,
            double extraWidth,
            double extraHeight,
            double horizontal,
            double vertical,
            string title,
            Shape shape,
            bool skip,
            string part,
            Appearance appearance,
            string picture,
            string overprint,
            Alignment alignment,
            string sheet
        )
        {
            Number = number;
            Description = description;
            FrameColor = frameColor;
            Width = width;
            Height = height;
            ExtraWidth = extraWidth;
            ExtraHeight = extraHeight;
            Horizontal = horizontal;
            Vertical = vertical;
            Title = title;
            Shape = shape;
            Skip = skip;
            Part = part;
            Appearance = appearance;
            Picture = picture;
            Overprint = overprint;
            Alignment = alignment;
            Sheet = sheet;
        }

        public static string GetNumber(DesignEntry entry)
        {
            string number = "";

            PageSetup pageSetup = PageSetup.Get();

            if (pageSetup.Catalog == Catalog.None)
            {
                number = entry.Number;
            }
            else
            {
                switch (pageSetup.Catalog)
                {
                    case Catalog.Scott:
                        number = entry.Sc;
                        break;
                    case Catalog.Michel:
                        number = entry.Mi;
                        break;
                    case Catalog.Yvert:
                        number = entry.Yv;
                        break;
                    case Catalog.Gibbons:
                        number = entry.Sg;
                        break;
                    case Catalog.Chan:
                        number = entry.Ch;
                        break;
                    case Catalog.Afinsa:
                        number = entry.Af;
                        break;
                    case Catalog.Maury:
                        number = entry.Ma;
                        break;
                    case Catalog.Newfoundland:
                        number = entry.Nc;
                        break;
                    case Catalog.Afa:
                        number = entry.Afa;
                        break;
                    case Catalog.Facit:
                        number = entry.Fac;
                        break;
                }

                if (pageSetup.AppendCatalog && number != "")
                {
                    number = entry.Number + " </b>(" + number + ")";
                }
            }

            return number;
        }

        public static string GetValueAndColor(DesignEntry entry)
        {
            string valueAndColor;

            string value = entry.Value ?? "";
            string color = entry.Color ?? "";

            if (entry.Separate)
            {
                valueAndColor = (value != "" ? value : "!") + "%" + (color != "" ? color : "!");
            }
            else if (value != "" && color == "")
            {
                valueAndColor = value;
            }
            else if (color != "" && value == "")
            {
                valueAndColor = color;
            }
            else if (value != "" && color != "")
            {
                valueAndColor = value + " · " + color;
            }
            else
            {
                valueAndColor = "!";
            }

            return valueAndColor;
        }
    }
}

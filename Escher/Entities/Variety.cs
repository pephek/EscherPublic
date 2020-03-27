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
            string Number,
            string Description,
            FrameColor FrameColor,
            double Width,
            double Height,
            double ExtraWidth,
            double ExtraHeight,
            double Horizontal,
            double Vertical,
            string Title,
            Shape Shape,
            bool Skip,
            string Part,
            Appearance Appearance,
            string Picture,
            string Overprint,
            Alignment Alignment,
            string Sheet
        )
        {
            this.Number = Number;
            this.Description = Description;
            this.FrameColor = FrameColor;
            this.Width = Width;
            this.Height = Height;
            this.ExtraWidth = ExtraWidth;
            this.ExtraHeight = ExtraHeight;
            this.Horizontal = Horizontal;
            this.Vertical = Vertical;
            this.Title = Title;
            this.Shape = Shape;
            this.Skip = Skip;
            this.Part = Part;
            this.Appearance = Appearance;
            this.Picture = Picture;
            this.Overprint = Overprint;
            this.Alignment = Alignment;
            this.Sheet = Sheet;
        }
    }
}

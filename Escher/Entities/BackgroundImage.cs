using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class BackgroundImage
    {
        public readonly string Number;
        public readonly float Width;
        public readonly float Height;
        public readonly float Horizontal;
        public readonly float Vertical;
        public readonly bool RoundedCorners;

        public BackgroundImage(
            string number,
            float width,
            float height,
            float horizontal,
            float vertical,
            bool roundedCorners
        )
        {
            Number = number;
            Width = width;
            Height = height;
            Horizontal = horizontal;
            Vertical = vertical;
            RoundedCorners = roundedCorners;
        }
    }
}

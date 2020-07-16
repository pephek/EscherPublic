using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Varieties
    {
        public readonly string SubType;
        public readonly string PublicDescription;
        public readonly string PrivateDescription;
        public readonly bool Combine;
        public readonly float Horizontal;
        public readonly float Vertical;
        public readonly float VerticalMoveRelative;
        public readonly float VerticalMoveAbsolute;
        public readonly Alignment Alignment;
        public readonly bool FontOfType;
        public readonly bool FontOfDescription;
        public float Spacing;
        public readonly float WatermarkHeight;
        public readonly string WatermarkImage;
        public readonly Appearance Appearance;
        public float MaxWidth;

        public List<List<Variety>> Rows;

        public Varieties(
            string subType,
            string publicDescription,
            string privateDescription,
            bool combine,
            float horizontal,
            float vertical,
            float verticalMoveRelative,
            float verticalMoveAbsolute,
            Alignment alignment,
            bool fontOfType,
            bool fontOfDescription,
            float spacing,
            float watermarkHeight,
            string watermarkImage,
            Appearance appearance,
            float maxWidth
        )
        {
            SubType = subType;
            PublicDescription = publicDescription;
            PrivateDescription = privateDescription;
            Combine = combine;
            Horizontal = horizontal;
            Vertical = vertical;
            VerticalMoveRelative = verticalMoveRelative;
            VerticalMoveAbsolute = verticalMoveAbsolute;
            Alignment = alignment;
            FontOfType = fontOfType;
            FontOfDescription = fontOfDescription;
            Spacing = spacing;
            WatermarkHeight = watermarkHeight;
            WatermarkImage = watermarkImage;
            Appearance = appearance;
            MaxWidth = maxWidth;

            Rows = new List<List<Variety>>();
        }
    }
}

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
        public readonly double Horizontal;
        public readonly double Vertical;
        public readonly Alignment Alignment;
        public readonly bool FontOfType;
        public readonly bool FontOfDescription;
        public readonly double Margin;

        public List<Variety> Rows;

        public Varieties(
            string subType,
            string publicDescription,
            string privateDescription,
            bool combine,
            double horizontal,
            double vertical,
            Alignment alignment,
            bool fontOfType,
            bool fontOfDescription,
            double margin
        )
        {
            SubType = subType;
            PublicDescription = publicDescription;
            PrivateDescription = privateDescription;
            Combine = combine;
            Horizontal = horizontal;
            Vertical = vertical;
            Alignment = alignment;
            FontOfType = fontOfType;
            FontOfDescription = fontOfDescription;
            Margin = margin;

            Rows = new List<Variety>();
        }
    }
}

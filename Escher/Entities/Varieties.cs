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
        public readonly Alignment Alignment;
        public readonly bool FontOfType;
        public readonly bool FontOfDescription;
        public float Margin;

        public List<List<Variety>> Rows;

        public Varieties(
            string subType,
            string publicDescription,
            string privateDescription,
            bool combine,
            float horizontal,
            float vertical,
            Alignment alignment,
            bool fontOfType,
            bool fontOfDescription,
            float margin
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

            Rows = new List<List<Variety>>();
        }
    }
}

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
            string SubType,
            string PublicDescription,
            string PrivateDescription,
            bool Combine,
            double Horizontal,
            double Vertical,
            Alignment Alignment,
            bool FontOfType,
            bool FontOfDescription,
            double Margin
        )
        {
            this.SubType = SubType;
            this.PublicDescription = PublicDescription;
            this.PrivateDescription = PrivateDescription;
            this.Combine = Combine;
            this.Horizontal = Horizontal;
            this.Vertical = Vertical;
            this.Alignment = Alignment;
            this.FontOfType = FontOfType;
            this.FontOfDescription = FontOfDescription;
            this.Margin = Margin;

            this.Rows = new List<Variety>();
        }
    }
}

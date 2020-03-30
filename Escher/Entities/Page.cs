using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Page
    {
        public string Country;
        public string Copyright;
        public string Title;
        public string Series;
        public string MainType;
        public string Folder;
        public double OffsetVertical;
        public double Spacing;
        public string AlbumNumber;
        public bool IsSample;

        public List<Varieties> Varieties = new List<Varieties>();

        public void AddVarieties(string publicDescription, string privateDescription, string subType, double horizontal, double vertical, bool combine, Alignment alignment, bool fontOfType, bool fontOfDescritpion, double margin)
        {
            if (Varieties.Count() == 0) // The first varieties can never be combined
            {
                combine = false;
            }

            Varieties varieties = new Varieties(subType, publicDescription, privateDescription, combine, horizontal, vertical, alignment, fontOfType, fontOfDescritpion, margin);

            Varieties.Add(varieties);
        }

        public void AddVariety(string number, string description, string title, Color frameColor, double width, double height, double extraWidth, double extraHeight, double horizontal, double vertical, bool skip, Appearance appearance, string picture, string overprint, Shape shape, Alignment alignment, string sheet)
        {
            Variety variety = new Variety(number, description, frameColor, width, height, extraWidth, extraHeight, horizontal, vertical, title, shape, skip, "", appearance, picture, overprint, alignment, sheet);

            Varieties varieties = Varieties[Varieties.Count() - 1];

            varieties.Rows.Add(variety);
        }
    }
}

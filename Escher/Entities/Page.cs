using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Page
    {
        public string Country;
        public string Copyright;
        public string Series;
        public string MainType;
        public string Folder;
        public double OffsetVertical;
        public double Spacing;
        public string PageNumber;
        public bool IsSample;

        public void AddVarieties(string publicDescription, string privateDescription, string subType, double horizontal, double vertical, bool combine, Alignment alignment, bool fontOfType, bool fontOfDescritpion, double margin)
        {

        }
    }
}

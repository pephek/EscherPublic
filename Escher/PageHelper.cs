using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class PageHelper
    {
        public static Page Get(Design design, int pageNumber)
        {
            string subType = "";
            string subTypeToPass = "";

            Page page = new Page();

            // Look up the title
            // a) NEDERLAND
            // b) BADEN
            // c) REPUBLIK MALUKU SELATAN
            DesignEntry country = design.LastOrDefault(entry => entry.Class == Class.Country && entry.Page <= pageNumber);

            page.Country = country.Text;
            page.Copyright = country.Copyright;

            // Look up the sub title
            // a) 1867-1869. Koning Willem III.
            // b) ALTDEUTSCHLAND
            // c) 1950. U.P.U.
            DesignEntry series = design.LastOrDefault(entry => entry.Class == Class.Series && entry.Page <= pageNumber);

            page.Series = series.Text;

            // Look up the sub sub title
            // a) Type I.
            // b) Portomarken
            // c) Regular.
            DesignEntry mainType = design.FirstOrDefault(entry => entry.Class == Class.Type && entry.Page == pageNumber);

            if (mainType != null)
            {
                page.MainType = mainType.Text;
            }

            // Loop up the folder
            // a) Frankeerzegels
            // b) Briefmarken
            // c) Cinderellas
            DesignEntry part = design.LastOrDefault(entry => entry.Class == Class.Part && entry.Page <= pageNumber);

            page.Folder = part.Text;

            // Look up the first entry having this page number
            int first = design.FindIndex(entry => entry.Page == pageNumber);

            int i = first;
            while (design[i].Page == design[first].Page)
            {
                DesignEntry entry = design[i];

                switch (entry.Class)
                {
                    case Class.PageFeed:

                        page.OffsetVertical = entry.OffsetVertical;
                        page.Spacing = entry.Width;
                        page.PageNumber = entry.PageNumber;
                        page.IsSample = entry.Sample;

                        break;

                    case Class.Varieties:

                    case Class.LineFeed:

                        if (subType != page.MainType)
                        {
                            subTypeToPass = subType;
                        }
                        else
                        {
                            subTypeToPass = "";
                        }

                        page.AddVarieties(entry.Text, entry.Comment, subTypeToPass, entry.OffsetHorizontal, entry.OffsetVertical, entry.Combine, entry.Alignment, entry.FontOfType, entry.FontOfDescription, entry.Width);

                        subType = "";

                        break;
                }

                i++;
            }
            return page;
        }
    }
}

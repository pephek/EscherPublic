using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Design : List<DesignEntry>
    {
    }

    public static class DesignExtensionMethods
    {
        public static DesignEntry FindStampNumber(this Design design, string stampNumber)
        {
            return design.FirstOrDefault(entry => entry.Class == Class.Variety && entry.Number.ToLower() == stampNumber.ToLower());
        }

        public static DesignEntry FindPageNumber(this Design design, int pageNumber)
        {
            return design.FirstOrDefault(entry => entry.Class == Class.PageFeed && entry.PageNumber == pageNumber);
        }

        public static DesignEntry FindAlbumNumber(this Design design, string albumNumber)
        {
            return design.FirstOrDefault(entry => entry.Class == Class.PageFeed && entry.AlbumNumber.ToLower() == albumNumber.ToLower());
        }

        public static int NumberOfPages(this Design design)
        {
            return design[design.Count() - 2].PageNumber;
        }

        public static DesignEntry GetCountry(this Design design, int pageNumber)
        {
            return design.LastOrDefault(entry => entry.Class == Class.Country && entry.PageNumber <= pageNumber);
        }

        public static DesignEntry GetSection(this Design design, int pageNumber)
        {
            return design.LastOrDefault(entry => entry.Class == Class.Section && entry.PageNumber <= pageNumber);
        }

        public static DesignEntry GetSeries(this Design design, int pageNumber)
        {
            return design.LastOrDefault(entry => entry.Class == Class.Series && entry.PageNumber <= pageNumber);
        }

        public static Design GetStampsFromSeries(this Design design, int pageNumber, string number)
        {
            Design stamps = new Design();

            for (int i = 0; i < design.Count(); i++)
            {
                DesignEntry entry = design[i];

                if (entry.PageNumber == pageNumber && entry.Class == Class.Stamp && entry.Number == number)
                {
                    int j = i;

                    while (design[j].Class == Class.Stamp)
                    {
                        j--;
                    }

                    j++;

                    while (design[j].Class == Class.Stamp)
                    {
                        stamps.Add(design[j]);

                        j++;
                    }

                    return stamps;
                }
            }

            return stamps;
        }
    }
}

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

    public static class DesignExtensionMethohds
    {
        public static DesignEntry FindStampNumber(this Design design, string stampNumber)
        {
            return design.FirstOrDefault(entry => entry.Class == Class.Variety && entry.Number.ToLower() == stampNumber.ToLower());
        }

        public static DesignEntry FindPageNumber(this Design design, int pageNumber)
        {
            return design.FirstOrDefault(entry => entry.Class == Class.PageFeed && entry.Page == pageNumber);
        }

        public static DesignEntry FindAlbumNumber(this Design design, string albumNumber)
        {
            return design.FirstOrDefault(entry => entry.Class == Class.PageFeed && entry.PageNumber.ToLower() == albumNumber.ToLower());
        }
    }
}

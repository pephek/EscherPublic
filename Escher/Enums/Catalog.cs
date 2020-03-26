using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum Catalog
    {
        None,
        Scott,
        Michel,
        Yvert,
        Gibbons,
        Chan,
        Afinsa,
        Maury,
        Newfoundland,
        Afa,
        Facit
    }

    public class Catalogs
    {
        private static string[] catalogs = new string[] {
            "(None)",
            "Scott",
            "Michel",
            "Yvert & Tellier",
            "Stanley Gibbons",
            "Chan",
            "Afinsa",
            "Maury",
            "Newfoundland Specialized",
            "Afa",
            "Facit"
        };

        public static string[] Get()
        {
            return catalogs;
        }

        public static string Convert(Catalog catalog)
        {
            return catalogs[(int)catalog];
        }

        public static Catalog Convert(string catalog)
        {
            return (Catalog)Array.IndexOf(catalogs, catalog);
        }
    }
}

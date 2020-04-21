using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Bookmark
    {
        public string Text;
        public string AlbumNumber;
        public int PageNumber;
        public string PageText;
        public List<Bookmark> Bookmarks = new List<Bookmark>();
        public string ForeColor;
    }
}

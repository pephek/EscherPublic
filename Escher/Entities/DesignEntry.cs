using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class DesignEntry
    {
        public Class Class;
        public string Text;
        public string Value;
        public string Number;
        public string Color;
        public FrameColor FrameColor;
        public double Width;
        public double Height;
        public double ExtraWidth;
        public double ExtraHeight;
        public double OffsetHorizontal;
        public double OffsetVertical;
        public int Page;
        public bool Skip;
        public bool Separate;
        public bool Combine;
        public string Perforation;
        public bool Menu;
        public Appearance Appearance;
        public string Picture;
        public string Overprint;
        public string Comment;
        public Shape Shape;
        public bool Variant;
        public bool SkipVariant;
        public string Sc;
        public string Mi;
        public string Sg;
        public string Yv;
        public string Ch;
        public string Af;
        public string Ma;
        public string Nc;
        public string Afa;
        public string Fac;
        public Alignment Alignment;
        public string Sheet;
        public bool Landscape;
        public bool Unlisted;
        public string Html;
        public string Catalogue;
        public string Copyright;
        public string PageNumber;
        public bool Special;
        public string Pdf;
        public string Settings;
        public string Version;
        public bool FontOfType;
        public bool FontOfDescription;
        public string Issued;
        public string Printed;
        public string Perfs;
        public bool Sample;

        public DesignEntry(Class Class, int Page)
        {
            this.Class = Class;
            this.Page = Page;
        }

        public DesignEntry(int Page)
        {
            this.Page = Page;
        }

        public void SetClass(Class Class, string Text)
        {
            this.Class = Class;
            this.Text = Text;
        }
    }
}

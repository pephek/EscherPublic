using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class PageSetup
    {
        public PageFormat PageFormat;
        public bool IncludeMarginForPunchHoles;
        public bool IncludeImage;
        public bool IncludeNumber;
        public bool IncludeValueAndColor;
        public FrameStyle FrameStyle;
        public ColorStyle ColorStyle;
        public FontSize FontSize;
        public bool IncludeBorder;
        public bool IncludeTitle;
        public Catalog Catalog;
        public bool AppendCatalog;
        public bool IncludeSamplePagesOnly;
        public bool IncludeHtmlScans;
        public bool IncludePdfBookmarks;
        public decimal RealLifePageScale;

        private static PageSetup pageSetup = new PageSetup();

        public static void Load()
        {
            pageSetup.PageFormat = PageFormats.Get(Properties.Settings.Default.Paper);
            pageSetup.IncludeMarginForPunchHoles = Properties.Settings.Default.IncludeMarginForPunchHoles;
            pageSetup.IncludeImage = Properties.Settings.Default.IncludeImage;
            pageSetup.IncludeNumber = Properties.Settings.Default.IncludeNumber;
            pageSetup.IncludeValueAndColor = Properties.Settings.Default.IncludeValueAndColor;
            pageSetup.FrameStyle = Properties.Settings.Default.FrameStyle;
            pageSetup.ColorStyle = Properties.Settings.Default.ColorStyle;
            pageSetup.FontSize = Properties.Settings.Default.FontSize;
            pageSetup.IncludeBorder = Properties.Settings.Default.IncludeBorder;
            pageSetup.IncludeTitle = Properties.Settings.Default.IncludeTitle;
            pageSetup.Catalog = Properties.Settings.Default.Catalog;
            pageSetup.AppendCatalog = Properties.Settings.Default.AppendCatalog;
            pageSetup.IncludeSamplePagesOnly = Properties.Settings.Default.IncludeSamplePagesOnly;
            pageSetup.IncludeHtmlScans = Properties.Settings.Default.IncludeHtmlScans;
            pageSetup.IncludePdfBookmarks = Properties.Settings.Default.IncludePdfBookmarks;
            pageSetup.RealLifePageScale = Properties.Settings.Default.RealLifePageScale;
        }

        public static PageSetup Get()
        {
            return pageSetup;
        }
    }
}

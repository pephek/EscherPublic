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

        private static PageSetup pageSetup = new PageSetup();

        public static void Load()
        {
            pageSetup.PageFormat = PageFormats.Get(App.GetSetting("Print.Format", "A4"));
            pageSetup.IncludeMarginForPunchHoles = App.GetSetting("Print.IncludeMarginForPunchHoles", false);
            pageSetup.IncludeImage = App.GetSetting("Print.IncludeImage", true);
            pageSetup.IncludeNumber = App.GetSetting("Print.IncludeNumber", true);
            pageSetup.IncludeValueAndColor = App.GetSetting("Print.IncludeValueAndColor", true);
            pageSetup.FrameStyle = App.GetSetting("Print.FrameStyle", FrameStyle.Thick);
            pageSetup.ColorStyle = App.GetSetting("Print.ColorStyle", ColorStyle.Color);
            pageSetup.FontSize = App.GetSetting("Print.FontSize", FontSize.Small);
            pageSetup.IncludeBorder = App.GetSetting("Print.IncludeBorder", true);
            pageSetup.IncludeTitle = App.GetSetting("Print.IncludeTitle", true);
            pageSetup.Catalog = Catalogs.Convert(App.GetSetting("Print.Catalog", Catalogs.Get()[0]));
            pageSetup.AppendCatalog = App.GetSetting("Print.AppendCatalog", false);
            pageSetup.IncludeSamplePagesOnly = App.GetSetting("Print.IncludeSamplePagesOnly", false);
            pageSetup.IncludeHtmlScans = App.GetSetting("Print.IncludeHtmlScans", false);
            pageSetup.IncludePdfBookmarks = App.GetSetting("Print.IncludePdfBookmarks", true);
        }

        public static PageSetup Get()
        {
            return pageSetup;
        }
    }
}

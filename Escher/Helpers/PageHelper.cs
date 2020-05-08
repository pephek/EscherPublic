using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class PageHelper
    {
        public static Page Get(Design design, int pageNumber, FrameStyle frameStyle)
        {
            string subType = "";
            string subTypeToPass = "";
            string description = "";

            DesignEntry pagefeed = design.GetPagefeed(pageNumber);

            Page page = new Page(pagefeed.Html, pagefeed.Comment);

            // Look up the title
            // a) NEDERLAND
            // b) BADEN
            // c) REPUBLIK MALUKU SELATAN
            DesignEntry country = design.GetCountry(pageNumber);

            page.Country = country.Text;
            page.Copyright = country.Copyright;
            page.FrontPageTitle = country.FrontPageTitle;
            page.FrontPageSubTitle = country.FrontPageSubTitle;
            page.PageTitle = country.PageTitle;

            // Loop up the folder
            // a) Frankeerzegels
            // b) Briefmarken
            // c) Cinderellas
            DesignEntry section = design.GetSection(pageNumber);

            page.Section = section.Text;

            page.ImagesPath = string.Format("{0}\\{1}\\{2}\\", App.GetSetting("ImagesFolder"), page.Country, page.Section);

            // Look up the sub title
            // a) 1867-1869. Koning Willem III.
            // b) ALTDEUTSCHLAND
            // c) 1950. U.P.U.
            DesignEntry series = design.GetSeries(pageNumber);

            page.Series = series.Text;

            // Look up the sub sub title
            // a) Type I.
            // b) Portomarken
            // c) Regular.
            DesignEntry mainType = design.FirstOrDefault(entry => entry.Class == Class.Type && entry.PageNumber == pageNumber);

            if (mainType != null)
            {
                page.MainType = mainType.Text;
            }

            // Look up the first entry having this page number
            int first = design.FindIndex(entry => entry.PageNumber == pageNumber);

            int i = first;
            while (design[i].PageNumber == design[first].PageNumber)
            {
                DesignEntry entry = design[i];

                switch (entry.Class)
                {
                    case Class.PageFeed:

                        page.OffsetVertical = entry.OffsetVertical;
                        page.AlbumNumber = entry.AlbumNumber;
                        page.IsSample = entry.Sample;
                        if (entry.Width != 0)
                        {
                            page.Margin = entry.Width;
                        }

                        break;

                    case Class.Type:
                        subType = entry.Text;
                        break;

                    case Class.Description:
                        description = entry.Text;
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

                        page.AddVarieties(
                            entry.Text,
                            entry.Comment,
                            subTypeToPass,
                            entry.OffsetHorizontal,
                            entry.OffsetVertical,
                            entry.VerticalMove,
                            entry.Combine,
                            entry.Alignment,
                            entry.FontOfType,
                            entry.FontOfDescription,
                            entry.Width
                        );

                        subType = "";

                        break;

                    case Class.Variety:

                        if (entry.ApplyTo.ToUpper() != "VB" && PassFrameStyle(entry.ApplyToFrameStyle, frameStyle))
                        {
                            page.AddVariety(
                                Variety.GetNumber(entry),
                                Variety.GetValueAndColor(entry),
                                description,
                                entry.FrameColor,
                                entry.Width,
                                entry.Height,
                                entry.OffsetHorizontal,
                                entry.OffsetVertical,
                                entry.Skip,
                                entry.Appearance,
                                string.IsNullOrEmpty(entry.Picture) ? entry.Number : entry.Picture,
                                entry.Overprint,
                                entry.Shape,
                                entry.Alignment,
                                entry.Positions
                            );

                            description = "";
                        }

                        break;
                }

                i++;
            }

            return page;
        }

        private static bool PassFrameStyle(string applyToFrameStyle, FrameStyle frameStyle)
        {
            bool pass = true;

            if (applyToFrameStyle != "")
            {
                switch (frameStyle)
                {
                    case FrameStyle.ThinSolid:
                    case FrameStyle.ThinDotted:
                        pass = (applyToFrameStyle.ToLower() == "thin");
                        break;
                    case FrameStyle.Thick:
                        pass = (applyToFrameStyle.ToLower() == "thick");
                        break;
                }
            }

            return pass;
        }
    }
}

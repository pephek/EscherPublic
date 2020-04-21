using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class BookmarksHelper
    {
        public static string GetBookmarks(Design design, bool includeSamplePagesOnly)
        {
            List<Bookmark> bookmarks = new List<Bookmark>();

            Bookmark country = null;
            Bookmark section = null;
            Bookmark page = null;
            Bookmark main = null;

            for (int i = 0; i < design.Count(); i++)
            {
                DesignEntry entry = design[i];

                switch (entry.Class)
                {
                    case Class.Country:

                        country = new Bookmark()
                        {
                            Text = entry.Text, // Set the text for the country
                            ForeColor = "[1 0 1]" // Magenta
                        };

                        section = new Bookmark()
                        {
                            Text = design[i + 1].Comment, // Set the text for the section (the part follows the country)
                            ForeColor = "[0 0 1]" //Blue
                        };

                        country.Bookmarks.Add(section);

                        bookmarks.Add(country);

                        break;

                    case Class.PageFeed:

                        if (!entry.Combine)
                        {
                            if (design[i + 1].Class == Class.Section)
                            {
                                if (country.Bookmarks[country.Bookmarks.Count() - 1].Text != design[i + 1].Comment)
                                {
                                    section = new Bookmark()
                                    {
                                        Text = design[i + 1].Comment,
                                        ForeColor = "[0 0 1]" // Blue
                                    };

                                    country.Bookmarks.Add(section);
                                }
                            }

                            main = new Bookmark()
                            {
                                Text = entry.Html,
                                PageNumber = entry.PageNumber,
                                AlbumNumber = entry.AlbumNumber,
                                PageText = entry.Html,
                                ForeColor = (includeSamplePagesOnly && !entry.Sample ? "[1 0 0]" : "[0 0 0]")

                            };

                            if (entry.Comment != "&nbsp;")
                            {
                                main.Text += "   (" + entry.Comment + ")";
                                main.PageText += "   (" + entry.Comment + ")";
                            }

                            if (entry.AlbumNumber != "")
                            {
                                main.Text = entry.AlbumNumber + " : " + main.Text;
                            }

                            section.Bookmarks.Add(main);
                        }
                        else
                        {
                            page = new Bookmark()
                            {
                                Text = entry.AlbumNumber + " : " + entry.Comment,
                                PageNumber = entry.PageNumber,
                                AlbumNumber = entry.AlbumNumber,
                                PageText = entry.Comment,
                                ForeColor = (includeSamplePagesOnly && !entry.Sample ? "[1 0 0]" : "[0 0 0]")
                            };

                            main.Bookmarks.Add(page);
                        }

                        break;
                }
            }

            //Bookmark overview = new Bookmark()
            //{
            //    Text = "Overzicht Klemstroken · Overview Mounts", // Set the text for the mounts overview
            //    ForeColor = "[1 0 1]" // Magenta
            //};

            //bookmarks.Add(overview);

            int pageNumber = 1;

            StringBuilder xml = new StringBuilder();

            foreach (Bookmark c in bookmarks)
            {
                if (bookmarks.Count() > 1)
                {
                    xml.AppendLine(Format(pageNumber, c.Text, c.Bookmarks.Count, c.ForeColor, "2"));
                }
                foreach (Bookmark s in c.Bookmarks)
                {
                    if (c.Bookmarks.Count() > 1)
                    {
                        xml.AppendLine(Format(pageNumber, s.Text, s.Bookmarks.Count, s.ForeColor, "2"));
                    }
                    foreach (Bookmark m in s.Bookmarks)
                    {
                        xml.AppendLine(Format(pageNumber, m.Text, m.Bookmarks.Count, m.ForeColor, "2"));

                        pageNumber++;

                        foreach (Bookmark p in m.Bookmarks)
                        {
                            xml.AppendLine(Format(pageNumber, p.Text, p.Bookmarks.Count, p.ForeColor, "0"));

                            pageNumber++;
                        }
                    }
                }
            }


            xml.Replace("& ", "&amp; ").
                Replace("&nbsp;", " ").
                Replace("&#8470;", "Nº").
                Replace("<b>", "").
                Replace("</b>", "").
                Replace("<i>", "").
                Replace("</i>", "").
                Replace("<s>", "").
                Replace("</s>", "");

            StringBuilder doc = new StringBuilder();

            doc.AppendLine("<?xml version=\"1.0\" encoding=\"iso-8859-1\" ?>");
            doc.AppendLine("<Bookmarks>");
            doc.AppendLine(xml.ToString());
            doc.AppendLine("</Bookmarks>");

            return doc.ToString();
        }

        private static string Format(int pageNumber, string text, int children, string color, string style)
        {
            // C = "[1 0 0]" -> Red
            // C = "[0 1 0]" -> Green
            // C = "[0 0 1]" -> Blue
            // C = "[1 1 1]" -> White
            // C = "[0 0 0]" -> Black

            // F = "0" -> Normal
            // F = "1" -> Italic
            // F = "2" -> Bold
            // F = "3" -> Italic + Bold

            return string.Format("<Bookmark Page=\"{0:0000}\" Text=\"{1}\" Children=\"{2}\" C=\"{3}\" F=\"{4}\" />", pageNumber, text, children, color, style);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class BookmarksHelper
    {
        public static void GetBookmarks(Design design, string pdfName, bool includeSamplePagesOnly, out string bookmarksInXml, out string bookmarksInHtm)
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
                                main.Text += " (" + entry.Comment + ")";
                                main.PageText += " (" + entry.Comment + ")";
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

            Bookmark overview = new Bookmark()
            {
                Text = "Overzicht Klemstroken · Overview Mounts", // Set the text for the mounts overview
                ForeColor = "[1 0 1]" // Magenta
            };

            bookmarks.Add(overview);

            int pageNumber = 1;

            StringBuilder doc = new StringBuilder();

            doc.AppendLine("<?xml version=\"1.0\" encoding=\"iso-8859-1\" ?>");
            doc.AppendLine("<Bookmarks>");

            foreach (Bookmark c in bookmarks)
            {
                if (bookmarks.Count() > 1)
                {
                    doc.AppendLine(Format(pageNumber, c.Text, c.Bookmarks.Count, c.ForeColor, "2"));
                }
                foreach (Bookmark s in c.Bookmarks)
                {
                    if (c.Bookmarks.Count() > 1)
                    {
                        doc.AppendLine(Format(pageNumber, s.Text, s.Bookmarks.Count, s.ForeColor, "2"));
                    }
                    foreach (Bookmark m in s.Bookmarks)
                    {
                        doc.AppendLine(Format(pageNumber, m.Text, m.Bookmarks.Count, m.ForeColor, "2"));

                        pageNumber++;

                        foreach (Bookmark p in m.Bookmarks)
                        {
                            doc.AppendLine(Format(pageNumber, p.Text, p.Bookmarks.Count, p.ForeColor, "0"));

                            pageNumber++;
                        }
                    }
                }
            }

            doc.AppendLine("</Bookmarks>");

            doc.Replace("& ", "&amp; ").
                Replace("&nbsp;", " ").
                Replace("&#8470;", "Nº").
                Replace("<b>", "").
                Replace("</b>", "").
                Replace("<i>", "").
                Replace("</i>", "").
                Replace("<s>", "").
                Replace("</s>", "").
                Replace("<f>", "").
                Replace("</f>", "");

            bookmarksInXml = doc.ToString();

            doc = new StringBuilder();

            foreach (Bookmark c in bookmarks)
            {
                if (bookmarks.Count() > 1)
                {
                    doc.AppendLine(string.Format("<div ng-init='model.pages={0}'></div>", pageNumber - 1));
                    doc.AppendLine(string.Format("<h1 align='center' style='font-weight: bold; color: green;'>{0}</h1>", c.Text));
                }
                foreach (Bookmark s in c.Bookmarks)
                {
                    if (c.Bookmarks.Count() >= 2) // Only show section when there are two or more sections
                    {
                        doc.AppendLine(string.Format("<h2 align='center' style='font-weight: bold; color: blue;'>{0}</h2>", s.Text));
                    }
                    foreach (Bookmark m in s.Bookmarks)
                    {
                        doc.AppendLine(string.Format("<br><p align='center' style='font-weight: bold;'>{0}</p>", m.PageText));
                        doc.AppendLine(string.Format("<table align='center'><tr>"));

                        doc.AppendLine(string.Format("<td style='text-align: center; font-size: small;'>"));
                        doc.AppendLine(string.Format("<a style='cursor: pointer;' ng-click='model.zoomPage({0})' href='http://www.eperforationgauge.com/specializedstampalbumpages/pdfs/{1}_samples/{2}-large.jpg' target='_blank'>", m.PageNumber, pdfName, m.PageNumber));
                        doc.AppendLine(string.Format("<img border='1' src='http://www.eperforationgauge.com/specializedstampalbumpages/pdfs/{0}_samples/{1}-small.jpg'><br><strong>{2}</strong></a><br>&nbsp;", pdfName, m.PageNumber, m.AlbumNumber));
                        doc.AppendLine(string.Format("</td>"));

                        foreach (Bookmark p in m.Bookmarks)
                        {
                            doc.AppendLine(string.Format("<td style='text-align: center; font-size: small;'>"));
                            doc.AppendLine(string.Format("<a style='cursor: pointer;' ng-click='model.zoomPage({0})' href='http://www.eperforationgauge.com/specializedstampalbumpages/pdfs/{1}_samples/{2}-large.jpg' target='_blank'>", p.PageNumber, pdfName, p.PageNumber));
                            doc.AppendLine(string.Format("<img border='1' src='http://www.eperforationgauge.com/specializedstampalbumpages/pdfs/{0}_samples/{1}-small.jpg'><br><strong>{2}</strong></a><br>&nbsp;", pdfName, p.PageNumber, p.AlbumNumber));
                            doc.AppendLine(string.Format("</td>"));
                        }

                        doc.AppendLine(string.Format("</tr></table>"));
                    }
                }
            }

            bookmarksInHtm = doc.ToString();
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

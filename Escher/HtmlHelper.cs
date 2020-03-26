using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class HtmlHelper
    {
        private static string GetTableOfContents(List<CatalogEntry> catalog, int i)
        {
            StringBuilder html = new StringBuilder();

            string country = catalog[i].Text;

            i++;

            while (i < catalog.Count() && catalog[i].Class != Class.Country)
            {
                CatalogEntry entry = catalog[i];

                if (entry.Class == Class.Part)
                {
                    html.Append(string.Format("<li><a href=\"#part({0},{1})\"><small>{2}</small></a>", country, entry.Text, entry.Text));
                }

                i++;
            }

            return html.ToString();
        }

        private static string GetHeading(string heading, string text)
        {
            text = text.Replace("!%", "").Replace("!", "").Replace("~", "").Replace("%", " &#9166; ");

            if (text == "")
            {
                return "";
            }
            else
            {
                return string.Format("<h{0}>{1}</h{2}>", heading, text, heading);
            }
        }

        private static string GetText(string text)
        {
            text = text.Replace("!%", "").Replace("!", "").Replace("~", "");

            if (text == "")
            {
                return "";
            }
            else
            {
                return string.Format("{0}<br><br>", text);
            }
        }

        private static string GetStamp(CatalogEntry entry)
        {
            if (entry.Number == "0" || entry.Number == "!")
            {
                return "";
            }
            else
            {
                return string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", entry.Number, entry.Value.Replace("!", ""), entry.Color?.Replace("!", ""));
            }
        }

        private static string GetStampDescription(string text)
        {
            text = text.Replace("!", "").Replace("%", " &#9166; ");

            if (text == "")
            {
                return "";
            }
            else
            {
                return string.Format("<tr><td></td><td><small>{0}<small></td><td></td></tr>", text);
            }
        }

        public static string GetDesignInHtml(List<CatalogEntry> catalog)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<style>a, a.visited {color: blue;} body, td {font-family: Arial; font-size: 75%;}</style>");

            string country = "";
            string part = "";

            for (int i = 0; i < catalog.Count(); i++)
            {
                CatalogEntry entry = catalog[i];

                switch (entry.Class)
                {
                    case Class.PageFeed:
                        html.Append(string.Format("<center><h1 id=\"page({0},{1})\"><a href=\"page({2})\">{3} ({4})</a></h1></center>", entry.Page, entry.PageNumber, entry.Page, entry.PageNumber, entry.Page));
                        if (!entry.Combine)
                        {
                            html.Append(string.Format("<center>{0} ({1})</center>", entry.Html, entry.Comment));
                        }
                        else
                        {
                            html.Append(string.Format("<center>{0}</center>", entry.Comment));
                        }
                        break;

                    case Class.Country:
                        country = entry.Text;
                        html.Append(GetHeading("1", entry.Text));
                        html.Append(GetTableOfContents(catalog, i));
                        break;

                    case Class.Part:
                        part = entry.Text;
                        html.Append(string.Format("<a name=\"part({0},{1})\"></a>", country, entry.Text));
                        html.Append(GetHeading("2", entry.Text));
                        break;

                    case Class.Type:
                        html.Append(GetHeading("3", entry.Text));
                        break;

                    case Class.Series:
                        html.Append(GetHeading("4", entry.Text));
                        break;

                    case Class.Varieties:
                        html.Append(GetText(entry.Text));
                        break;

                    case Class.Variety:
                    case Class.Description:
                        html.Append("<table>");
                        while (entry.Class == Class.Variety || entry.Class == Class.Description)
                        {
                            if (entry.Class == Class.Variety)
                            {
                                html.Append(GetStamp(entry));
                            }
                            else if (entry.Class == Class.Description)
                            {
                                html.Append(GetStampDescription(entry.Text));
                            }
                            entry = catalog[++i];
                        }
                        html.Append("</table><br>");
                        break;

                    case Class.Stamp:
                        string s = string.Format("<a href=\"stamp({0},{1},{2})\"><img src=\"{3}\\{4}\\{5}\\{6}.jpg\"></a>&nbsp;", country, part, entry.Number, App.GetSetting("ImagesFolder"), country, part, entry.Number);
                        html.Append(s);
                        break;
                }
            }

            return html.ToString();
        }
    }
}

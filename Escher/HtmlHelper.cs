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
        private static string GetTableOfContents(Design design, int i)
        {
            StringBuilder html = new StringBuilder();

            string country = design[i].Text;

            i++;

            while (i < design.Count() && design[i].Class != Class.Country)
            {
                DesignEntry entry = design[i];

                if (entry.Class == Class.Section)
                {
                    html.Append(string.Format("<li><a href=\"#section({0},{1})\"><small>{2}</small></a>", country, entry.Text, entry.Text));
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

        private static string GetStamp(DesignEntry entry)
        {
            if (entry.Number == "0" || entry.Number == "!")
            {
                return "";
            }
            else
            {
                return string.Format("<tr><td><b>{0}</b></td><td>{1}</td><td>{2}</td></tr>", entry.Number, entry.Value.Replace("!", ""), entry.Color?.Replace("!", ""));
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

        public static string GetDesignInHtml(Design design)
        {
            StringBuilder html = new StringBuilder();

            html.Append(@"
                <style>
                    a, a.visited {
                        color: blue;
                    }
                    body, td {
                        font-family: Arial;
                        font-size: 75%;
                    }
                    img.display-image-exists {
                        border: 1px solid #ddd;
                        border-radius: 3px;
                        padding: 30px;
                    }
                    img.display-image-not-exists {
                        border: 2px solid rgba(255, 0, 0, 1);
                        border-radius: 3px;
                        padding: 3px;
                    }
                    img:hover {
                        box-shadow: 0 0 3px 1px rgba(0, 140, 186, 0.5);
                    }
                </style>
            ");

            string country = "";
            string section = "";

            for (int i = 0; i < design.Count(); i++)
            {
                DesignEntry entry = design[i];

                switch (entry.Class)
                {
                    case Class.PageFeed:
                        html.Append(string.Format("<center><h1 id=\"page({0},{1})\"><a href=\"page({2})\">{3} ({4})</a></h1></center>", entry.PageNumber, entry.AlbumNumber, entry.PageNumber, entry.AlbumNumber, entry.PageNumber));
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
                        html.Append(GetTableOfContents(design, i));
                        break;

                    case Class.Section:
                        section = entry.Text;
                        html.Append(string.Format("<a name=\"section({0},{1})\"></a>", country, entry.Text));
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
                            entry = design[++i];
                        }
                        html.Append("</table><br>");
                        break;

                    case Class.Stamp:
                        string thumbnail = ImageHelper.GetThumbnailImage(App.GetSetting("ImagesFolder"), country, section, entry.Number, entry.Width, entry.Height, out bool existsDisplayImage);
                        string s;
                        if (existsDisplayImage)
                        {
                            s = string.Format("<a href=\"stamp({0},{1},{2})\"><img src=\"{3}\" title=\"{4}\" style=\"border:3px solid white\"></a>", country, section, entry.Number, thumbnail, entry.Number);

                        }
                        else
                        {
                            s = string.Format("<a href=\"stamp({0},{1},{2})\"><img src=\"{3}\" title=\"{4}\" style=\"border:3px dotted red\">&nbsp;</a>", country, section, entry.Number, thumbnail, entry.Number);
                        }
                        html.Append(s);
                        break;
                }
            }

            return html.ToString();
        }
    }
}

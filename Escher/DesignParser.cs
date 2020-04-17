using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class DesignParser
    {
        private Action<int> progress;
        private int previousProgress = 0;

        public Design Parse(string designText, Action<int> setProgress, out string error)
        {
            Design design = new Design();

            error = "";

            progress = setProgress;

            string line;

            try
            {
                string[] lines = designText.Split('\n');

                int i = 0;
                int p = 0;

                DesignEntry lastEntry = null;

                bool end = false;

                while (i < lines.Length && string.IsNullOrEmpty(error) && !end)
                {
                    line = lines[i];

                    DesignEntry entry = Parse(design, line, p, i);

                    if (entry != null)
                    {
                        if (lastEntry != null)
                        {
                            if (entry.Class == Class.Stamp || entry.Class == Class.Variety || entry.Class == Class.Description || entry.Class == Class.LineFeed)
                            {
                                if (lastEntry.Class != Class.Stamp && lastEntry.Class != Class.Variety && lastEntry.Class != Class.Description && lastEntry.Class != Class.LineFeed)
                                {
                                    design.Add(new DesignEntry(Class.Images, p));
                                    design.Add(new DesignEntry(Class.ListBegin, p));
                                }
                            }

                            if (lastEntry.Class == Class.Stamp || lastEntry.Class == Class.Variety || lastEntry.Class == Class.Description || lastEntry.Class == Class.LineFeed)
                            {
                                if (entry.Class != Class.Stamp && entry.Class != Class.Variety && entry.Class != Class.Description && entry.Class != Class.LineFeed)
                                {
                                    design.Add(new DesignEntry(Class.ListEnd, p));
                                }
                            }
                        }

                        design.Add(entry);

                        lastEntry = entry;

                        p = entry.PageNumber;

                        end = (entry.Class == Class.None);
                    }

                    i++;

                    SetProgress(i, lines.Length);
                }

                if (!end)
                {
                    design.Add(Parse(design, "End", 0, 0));
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }

            return design;
        }

        private DesignEntry Parse(Design design, string line, int pageNumber, int lineNumber)
        {
            DesignEntry entry = new DesignEntry(pageNumber);

            line = line.Replace("\t", "").Trim();

            if (line == "" || line.Substring(0, 1) == "'")
            {
                return null;
            }

            string[] splitted = line.Split('|');

            for (int i = 0; i < splitted.Length; i++)
            {
                string s = splitted[i].Replace(" ", "");

                switch (s.ToLower())
                {
                    case "end":
                        entry.Class = Class.None;
                        entry.PageNumber = -1;
                        break;
                    case "pagefeed":
                        entry.Class = Class.PageFeed;
                        entry.PageNumber++;
                        entry.Text = entry.PageNumber.ToString();
                        break;
                    case "linefeed":
                        entry.Class = Class.LineFeed;
                        break;
                    default:
                        Parse(design, entry, splitted[i], lineNumber);
                        break;
                }
            }

            return entry;
        }

        private void Parse(Design design, DesignEntry entry, string keyValuePair, int line)
        {
            string key;
            string val;

            if (keyValuePair.Contains(":="))
            {
                key = keyValuePair.Split(new[] { ":=" }, StringSplitOptions.None)[0].Trim();
                val = keyValuePair.Split(new[] { ":=" }, StringSplitOptions.None)[1].Trim();
            }
            else
            {
                key = keyValuePair.Trim();
                val = "";
            }

            switch (key.ToLower())
            {
                case "country": // Eg. Country:=Nederland
                    entry.SetClass(Class.Country, val);
                    break;

                case "part": // Eg. Part:=Frankeerzegels
                    entry.SetClass(Class.Section, val);
                    break;

                case "type": // Eg. Type:=Type I
                    entry.SetClass(Class.Type, val);
                    if (key.Contains("Comment:"))
                    {
                        throw new Exception(string.Format("Line {0}: Comment: in Type:= is not yet implemented!", line));
                    }
                    break;

                case "series": // Eg. Series:=1867-1869. Koning Willem III.
                    entry.SetClass(Class.Series, val);
                    break;

                case "varieties": // Eg. Varieties:=1946-'47.</b> Frankeerzegels.
                    entry.SetClass(Class.Varieties, val);
                    if (key.Contains("Comment:"))
                    {
                        throw new Exception(string.Format("Line {0}: Comment: in Type:= is not yet implemented!", line));
                    }
                    entry.Alignment = Alignment.Centered;
                    break;

                case "description": // Eg. Description:=Getand.
                    entry.SetClass(Class.Description, val);
                    entry.Alignment = Alignment.Centered;
                    break;

                case "stamp": //Eg. Stamp:=19
                    entry.Class = Class.Stamp;
                    entry.Number = val;
                    break;

                case "variety": // Eg. Variety:=19A
                    entry.Class = Class.Variety;
                    entry.Number = val;
                    if (design[design.Count() - 1].Class == Class.Description)
                    {
                        entry.Alignment = design[design.Count() - 1].Alignment;
                    }
                    break;

                case "design": // Eg. Design:=A30
                    entry.SetClass(Class.Design, val);
                    break;

                case "value": // Eg. Vlue:=5 ct.
                    entry.Value = val;
                    break;

                case "color":
                    entry.Color = val;
                    break;

                case "width":
                    entry.Width = GetSize(entry.Width, val);
                    break;

                case "height":
                    entry.Height = GetSize(entry.Height, val);
                    break;

                case "horizontal":
                    entry.OffsetHorizontal = Convert.ToSingle(val);
                    break;

                case "vertical":
                    entry.OffsetVertical = Convert.ToSingle(val);
                    break;

                case "skip":
                    entry.Skip = Convert.ToBoolean(val);
                    break;

                case "menu":
                    entry.Menu = Convert.ToBoolean(val);
                    break;

                case "combine":
                    entry.Combine = Convert.ToBoolean(val);
                    break;

                case "separate":
                    entry.Separate = Convert.ToBoolean(val);
                    break;

                case "alignment":
                    switch (val.ToLower())
                    {
                        case "":
                        case "left":
                            entry.Alignment = Alignment.Left;
                            break;
                        case "centered":
                            entry.Alignment = Alignment.Centered;
                            break;
                        case "right":
                            entry.Alignment = Alignment.Right;
                            break;
                        default:
                            throw new Exception(string.Format("Unknown alignment '{0}'", val));
                    }
                    break;

                case "applyto":
                    entry.ApplyTo = val;
                    break;

                case "applytoframestyle":
                    entry.ApplyToFrameStyle = val;
                    break;

                case "framecolor":
                    switch (val.ToLower())
                    {
                        case "":
                        case "black":
                            entry.FrameColor = FrameColor.Black;
                            break;
                        case "white":
                            entry.FrameColor = FrameColor.White;
                            break;
                        default:
                            throw new Exception(string.Format("Unknown frame color '{0}'", val));
                    }
                    break;

                case "picture":
                    entry.Picture = val;
                    if (entry.Picture == "bw:")
                    {
                        entry.Picture = "..\\print\\" + entry.Number;
                        while (entry.Picture != "" && (entry.Picture[entry.Picture.Length - 1] < '0' || entry.Picture[entry.Picture.Length - 1] > '9'))
                        {
                            entry.Picture = entry.Picture.Substring(0, entry.Picture.Length - 1);
                        }
                    }
                    else if (entry.Picture.StartsWith("bw:"))
                    {
                        entry.Picture = entry.Picture.Replace("bw:", "..\\print\\");
                    }
                    break;

                case "overprint":
                    entry.Overprint = val;
                    switch (entry.Overprint)
                    {
                        case "Serikat":
                            entry.Overprint = "Rockwell|7|False|False|Republik%Indonesia Serikat%27 Des. 49|0|0|0";
                            break;
                        case "RIS":
                            entry.Overprint = "Aparajita|10|False|True|R I S|0|0|0";
                            break;
                        case "RISMerdeka":
                            entry.Overprint = "Aparajita|10|False|True|R I S%Merdeka|0|0|0";
                            break;
                        case "RISDjakarta":
                            entry.Overprint = "Aparajita|10|False|True|R I S%Djakarta|0|0|0";
                            break;
                        default:
                            if (entry.Overprint.StartsWith("#")) // For the numeral cancels of Iceland
                            {
                                entry.Overprint = "Baskerville Old Face|18|True|False|" + entry.Overprint.Substring(1) + "|100|100|100";
                            }
                            else
                            { 
                                entry.Overprint = "";
                            }
                            break;
                    }
                    break;

                case "comment":
                case "private":
                    entry.Comment = val;
                    break;

                case "shape":
                    switch (val.ToLower())
                    {
                        case "":
                        case "rectangle":
                            entry.Shape = Shape.Rectangle;
                            break;
                        case "rectanglerotated":
                            entry.Shape = Shape.RectangleRotated;
                            break;
                        case "triangle60":
                            entry.Shape = Shape.Triangle60;
                            break;
                        case "triangle60inverted":
                            entry.Shape = Shape.Triangle60Inverted;
                            break;
                        case "triangle45":
                            entry.Shape = Shape.Triangle45;
                            break;
                        case "triangle45inverted":
                            entry.Shape = Shape.Triangle45Inverted;
                            break;
                        case "hexagonvertical":
                            entry.Shape = Shape.HexagonVertical;
                            break;
                        default:
                            throw new Exception(string.Format("Unknown shape '{0}'", val));
                    }
                    break;

                case "variant":
                    entry.Variant = Convert.ToBoolean(val);
                    if (entry.Variant && entry.Class == Class.Variety)
                    {
                        throw new Exception("Variant attribute not supported on a Variety; check VB for functionality");
                    }
                    break;

                case "skipvariant":
                    entry.SkipVariant = Convert.ToBoolean(val);
                    break;

                case "special":
                    entry.Special = Convert.ToBoolean(val);
                    break;

                case "sc":
                    entry.Sc = val;
                    switch (entry.Sc)
                    {
                        case "~":
                            entry.Sc = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "mi":
                    entry.Mi = val;
                    switch (entry.Mi)
                    {
                        case "~":
                            entry.Mi = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "sg":
                    entry.Sg = val;
                    switch (entry.Sg)
                    {
                        case "~":
                            entry.Sg = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "yv":
                    entry.Yv = val;
                    switch (entry.Yv)
                    {
                        case "~":
                            entry.Yv = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "ch":
                    entry.Ch = val;
                    switch (entry.Ch)
                    {
                        case "~":
                            entry.Ch = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "af":
                    entry.Af = val;
                    switch (entry.Af)
                    {
                        case "~":
                            entry.Af = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "ma":
                    entry.Ma = val;
                    switch (entry.Ma)
                    {
                        case "~":
                            entry.Ma = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "nc":
                    entry.Nc = val;
                    switch (entry.Nc)
                    {
                        case "~":
                            entry.Nc = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "afa":
                    entry.Afa = val;
                    switch (entry.Afa)
                    {
                        case "~":
                            entry.Afa = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;
                case "fac":
                    entry.Fac = val;
                    switch (entry.Fac)
                    {
                        case "~":
                            entry.Fac = entry.Number;
                            break;
                        case "<":
                            throw new Exception(":=< not supported any more");
                    }
                    break;

                case "landscape":
                    entry.Landscape = Convert.ToBoolean(val);
                    break;

                case "unlisted":
                    entry.Unlisted = Convert.ToBoolean(val);
                    break;

                case "html":
                    entry.Html = val;
                    break;

                case "catalogue":
                    entry.Catalogue = val;
                    break;

                case "copyright":
                    entry.Copyright = val;
                    break;

                case "pagenumber":
                    entry.AlbumNumber = val;
                    break;

                case "pdf":
                    entry.Pdf = val;
                    break;

                case "settings":
                    entry.Settings = val;
                    break;

                case "version":
                    entry.Version = val;
                    break;

                case "fontoftype":
                    entry.FontOfType = Convert.ToBoolean(val);
                    break;

                case "fontofdescription":
                    entry.FontOfDescription = Convert.ToBoolean(val);
                    break;

                case "issued":
                    entry.Issued = val;
                    break;

                case "perfs":
                    entry.Perfs = val;
                    break;

                case "printed":
                    entry.Printed = val;
                    break;

                case "sample":
                    entry.Sample = Convert.ToBoolean(val);
                    break;

                case "perforation":
                    entry.Perforation = val;
                    break;

                case "sheet":
                    // Do nothing here, this is for VB
                    break;

                case "positions":
                    entry.Positions = val;
                    break;

                case "appearance":
                    switch (val.ToLower())
                    {
                        case "single":
                        case "singular":
                            entry.Appearance = Appearance.Singular;
                            break;
                        case "rotated":
                            entry.Appearance = Appearance.Rotated;
                            break;
                        case "pairhorizontal":
                        case "horizontalpair":
                            entry.Appearance = Appearance.PairHorizontal;
                            entry.Width = 2 * entry.Width - 4;
                            break;
                        case "pairvertical":
                        case "verticalpair":
                            entry.Appearance = Appearance.PairVertical;
                            entry.Height = 2 * entry.Height - 4;
                            break;
                        case "tetebechehorizontal":
                        case "horizontaltetebeche":
                            entry.Appearance = Appearance.TeteBecheHorizontal;
                            entry.Width = 2 * entry.Width - 4;
                            break;
                        case "tetebechevertical":
                        case "verticaltetebeche":
                            entry.Appearance = Appearance.TeteBecheVertical;
                            entry.Height = 2 * entry.Height - 4;
                            break;
                        case "tetebechehorizontalgutter":
                        case "horizontaltetebechegutter":
                            entry.Appearance = Appearance.TeteBecheHorizontalGutter;
                            entry.Width = 3 * entry.Width - 2 * 4;
                            break;
                        case "tetebecheverticalgutter":
                        case "verticaltetebechegutter":
                            entry.Appearance = Appearance.TeteBecheVerticalGutter;
                            entry.Height = 3 * entry.Height - 2 * 4;
                            break;
                        case "horizontalgutterpair":
                            entry.Appearance = Appearance.HorizontalGutterPair;
                            entry.Width = 3 * entry.Width - (3 - 1) * 4;
                            break;
                        case "verticalgutterpair":
                            entry.Appearance = Appearance.VerticalGutterPair;
                            entry.Height = 3 * entry.Height - (3 - 1) * 4;
                            break;
                        case "block":
                            entry.Appearance = Appearance.Block;
                            entry.Width = 2 * entry.Width - 4;
                            entry.Height = 2 * entry.Height - 4;
                            break;
                        case "block5x5":
                            entry.Appearance = Appearance.Block5x5;
                            entry.Width = 5 * entry.Width - (5 - 1) * 4;
                            entry.Height = 5 * entry.Height - (5 - 1) * 4;
                            break;
                        case "sheet2x3":
                            entry.Appearance = Appearance.Sheet2x3;
                            entry.Width = 3 * entry.Width - 2 * 4;
                            entry.Height = 2 * entry.Height - 1 * 4;
                            break;
                        case "proof":
                            entry.Appearance = Appearance.Proof;
                            entry.Width = entry.Width + 8;
                            entry.Height = entry.Height + 8;
                            break;
                        case "horizontalstrip3":
                            entry.Appearance = Appearance.HorizontalStrip3;
                            entry.Width = 3 * entry.Width - (3 - 1) * 4;
                            break;
                        case "horizontalstrip4":
                            entry.Appearance = Appearance.HorizontalStrip4;
                            entry.Width = 4 * entry.Width - (4 - 1) * 4;
                            break;
                        case "horizontalstrip5":
                            entry.Appearance = Appearance.HorizontalStrip5;
                            entry.Width = 5 * entry.Width - (5 - 1) * 4;
                            break;
                        case "horizontalstrip6":
                            entry.Appearance = Appearance.HorizontalStrip6;
                            entry.Width = 6 * entry.Width - (6 - 1) * 4;
                            break;
                        case "verticalstrip3":
                            entry.Appearance = Appearance.VerticalStrip3;
                            entry.Height = 3 * entry.Height - (3 - 1) * 4;
                            break;
                        case "verticalstrip4":
                            entry.Appearance = Appearance.VerticalStrip4;
                            entry.Height = 4 * entry.Height - (4 - 1) * 4;
                            break;
                        case "verticalstrip5":
                            entry.Appearance = Appearance.VerticalStrip5;
                            entry.Height = 5 * entry.Height - (5 - 1) * 4;
                            break;
                        case "verticalstrip6":
                            entry.Appearance = Appearance.VerticalStrip6;
                            entry.Height = 6 * entry.Height - (6 - 1) * 4;
                            break;
                        case "carnetpaire":
                        case "pairecarnet":
                            entry.Appearance = Appearance.PaireCarnet;
                            entry.Height = 2 * (entry.Height - 4 + 0.3F * (entry.Height - 4)) + 4;
                            break;
                        case "bandepublicitaire":
                        case "publicitairebande":
                            entry.Appearance = Appearance.BandePublicitaire;
                            entry.Height = (entry.Height - 4) + 0.4F * (entry.Height - 4) + 4;
                            break;
                        case "interpanneauxhorizontal":
                        case "horizontalinterpanneaux":
                            entry.Appearance = Appearance.HorizontalInterpanneaux;
                            entry.Width = (entry.Width - 4) * 2.5F + 4;
                            break;
                        case "imperfright":
                            entry.Appearance = Appearance.ImperfRight;
                            entry.Width = 2 * entry.Width - 4;
                            break;
                        case "imperfleft":
                            entry.Appearance = Appearance.ImperfLeft;
                            entry.Width = 2 * entry.Width - 4;
                            break;
                        case "imperftop":
                            entry.Appearance = Appearance.ImperfTop;
                            entry.Height = 2 * entry.Height - 4;
                            break;
                        case "imperfbottom":
                            entry.Appearance = Appearance.ImperfBottom;
                            entry.Height = 2 * entry.Height - 4;
                            break;
                    }
                    break;

                case "size":
                    switch (val.ToLower())
                    {
                        case "kfs":
                            entry.Width = 21;
                            entry.Height = 25;
                            break;
                        case "kfl":
                            entry.Width = 25;
                            entry.Height = 21;
                            break;

                        case "mfs":
                            entry.Width = 25;
                            entry.Height = 32;
                            break;
                        case "mfl":
                            entry.Width = 32;
                            entry.Height = 25;
                            break;

                        case "gfs":
                            entry.Width = 25;
                            entry.Height = 36;
                            break;
                        case "gfl":
                            entry.Width = 36;
                            entry.Height = 25;
                            break;

                        case "gb_kfs":
                            entry.Width = 20;
                            entry.Height = 24;
                            break;
                        case "gb_kfl":
                            entry.Width = 24;
                            entry.Height = 20;
                            break;

                        case "us_smallportrait":
                            entry.Width = 22;
                            entry.Height = 26;
                            break;
                        case "us_smalllandscape":
                            entry.Width = 26;
                            entry.Height = 22;
                            break;
                        case "us_largeportrait":
                            entry.Width = 26;
                            entry.Height = 41;
                            break;
                        case "us_largelandscape":
                            entry.Width = 41;
                            entry.Height = 26;
                            break;

                        case "petitportrait":
                            entry.Width = 20;
                            entry.Height = 24;
                            break;
                        case "petitpaysage":
                            entry.Width = 24;
                            entry.Height = 20;
                            break;
                        case "moyenportrait":
                            entry.Width = 24;
                            entry.Height = 40;
                            break;
                        case "moyenpaysage":
                            entry.Width = 40;
                            entry.Height = 24;
                            break;
                        case "grandportrait":
                            entry.Width = 26;
                            entry.Height = 40;
                            break;
                        case "grandpaysage":
                            entry.Width = 40;
                            entry.Height = 26;
                            break;

                        case "va":
                            entry.Width = 33; // 32.75
                            entry.Height = 28; // 27.75
                            break;
                        case "xa":
                            entry.Width = 33; // 32.75
                            entry.Height = 28; // 27.75
                            // entry.Perforation = "Comb Perforation 14 x 13¾";
                            break;
                        case "ma":
                            entry.Width = 65 - 4;
                            entry.Height = 56 - 4;
                            break;

                        case "vb":
                            entry.Width = 28; // 27.75
                            entry.Height = 33; // 32.75
                            break;
                        case "xb":
                            entry.Width = 28; // 27.75
                            entry.Height = 33; // 32.75
                            // entry.Perforation = "Comb Perforation 13¾ x 14";
                            break;
                        case "mb":
                            entry.Width = 56 - 4;
                            entry.Height = 65 - 4;
                            break;

                        case "vc":
                            entry.Width = 39; // 38.75;
                            entry.Height = 30; // 29.75;
                            break;
                        case "xc":
                            entry.Width = 39; // 38.75;
                            entry.Height = 30; // 29.75;
                            // entry.Perforation = "Comb Perforation 13½ x 14¼";
                            break;
   
                        case "vd":
                            entry.Width = 30; // 29.75;
                            entry.Height = 39; // 38.75;
                            break;
                        case "xd":
                            entry.Width = 30; // 29.75;
                            entry.Height = 39; // 38.75;
                            // entry.Perforation = "Comb Perforation 14¼ x 13½";
                            break;

                        case "ve":
                            entry.Width = 38; // 37.5;
                            entry.Height = 47; // 45.25 47;
                            break;
                        case "xe":
                            entry.Width = 38; // 37.5;
                            entry.Height = 47; // 45.25;
                            // entry.Perforation = "Line Perforation 12½";
                            break;

                        case "vf":
                            entry.Width = 42; // 41.75;
                                    entry.Height = 35;
                            break;
                        case "xfa":
                            entry.Width = 42; // 41.75;
                            entry.Height = 35;
                            // entry.Perforation = "Line Perforation 14½";
                            break;
                        case "xfb":
                            entry.Width = 42; // 41.75;
                            entry.Height = 35;
                            // entry.Perforation = "Line Perforation 13¾";
                            break;
                        case "mf":
                            entry.Width = 74 - 4;
                            entry.Height = 62 - 4;
                            break;

                        case "vg":
                            entry.Width = 35;
                            entry.Height = 42; // 41.75;
                            break;
                        case "xga":
                            entry.Width = 35;
                            entry.Height = 42; // 41.75;
                            // entry.Perforation = "Line Perforation 14½";
                            break;
                        case "xgb":
                            entry.Width = 35;
                            entry.Height = 42; // 41.75;
                            // entry.Perforation = "Line Perforation 13¾";
                            break;
                        case "mg":
                            entry.Width = 68 - 4;
                            entry.Height = 70 - 4;
                            break;

                        case "vh":
                            entry.Width = 38; // 38.25;
                            entry.Height = 30; // 29.75;
                            break;
                        case "xh":
                            entry.Width = 38; // 38.25;
                            entry.Height = 30; // 29.75;
                            // entry.Perforation = "Comb Perforation 13½ x 14½";
                            break;
                        case "mh":
                            entry.Width = 74 - 4;
                            entry.Height = 60 - 4;
                            break;

                        case "vi":
                            entry.Width = 41; // 41.25;
                            entry.Height = 32; // 31.5;
                            break;
                        case "xia":;
                            entry.Width = 41; // 41.25;
                            entry.Height = 32; // 31.5;
                            // entry.Perforation = "Line Perforation 14½";
                            break;
                        case "xib":
                            entry.Width = 41; // 41.25;
                            entry.Height = 32; // 31.5;
                            // entry.Perforation = "Line Perforation 13¾";
                            break;
                        case "mi":
                            entry.Width = 74 - 4;
                            entry.Height = 60 - 4;
                            break;

                        case "vj":
                            entry.Width = 28; // 27.5;
                            entry.Height = 33; // 32.75;
                            break;
                        case "xj":
                            entry.Width = 28; // 27.5;
                            entry.Height = 33; // 32.75;
                            // entry.Perforation = "Comb Perforation 13¾ x 14";
                            break;

                        case "vk":
                            entry.Width = 30; // 30.25;
                            entry.Height = 37; // 36.5;
                            break;
                        case "xka":
                            entry.Width = 30; // 30.25;
                            entry.Height = 37; // 36.5;
                            // entry.Perforation = "Line Perforation 14½";
                            break;
                        case "xkb":
                            entry.Width = 30; // 30.25;
                            entry.Height = 37; // 36.5;
                            // entry.Perforation = "Line Perforation 13¾";
                            break;

                        case "vl":
                            entry.Width = 61; // 61.25;
                            entry.Height = 30; // 29.5;
                            break;
                        case "xl":
                            entry.Width = 61; // 61.25;
                            entry.Height = 30; // 29.5;
                            // entry.Perforation = "Line Perforation 12½";
                            break;

                        case "vm":
                            entry.Width = 30; // 29.5;
                            entry.Height = 62; // 61.5;
                            break;
                        case "xm":
                            entry.Width = 30; // 29.5;
                            entry.Height = 62; // 61.5;
                            // entry.Perforation = "Line Perforation 12½";
                            break;

                        case "vn":
                            entry.Width = 62;
                            entry.Height = 31;
                            break;
                        case "xna":
                            entry.Width = 62;
                            entry.Height = 31;
                            // entry.Perforation = "Line Perforation 14½";
                            break;
                        case "xnb":
                            entry.Width = 62;
                            entry.Height = 31;
                            // entry.Perforation = "Line Perforation 13¾";
                            break;

                        case "vo":
                            entry.Width = 31;
                            entry.Height = 62;
                            break;
                        case "xoa":
                            entry.Width = 31;
                            entry.Height = 62;
                            // entry.Perforation = "Line Perforation 14½";
                            break;
                        case "xob":
                            entry.Width = 31;
                            entry.Height = 62;
                            // entry.Perforation = "Line Perforation 13¾";
                            break;

                        case "vp":
                            entry.Width = 30; // 29.75;
                            entry.Height = 39; // 38.75;
                            break;
                        case "xp":
                            entry.Width = 30; // 29.75;
                            entry.Height = 3; // 38.75;
                            // entry.Perforation = "Comb Perforation 14¼ x 13½";
                            break;

                        case "v1":
                        case "x1":
                            entry.Width = 148;
                            entry.Height = 88; // 87.5;
                            break;
                        case "v2":
                        case "x2":
                            entry.Width = 88.5F;
                            entry.Height = 116; // 115.5;
                            break;

                        case "fdc":
                            entry.Width = 164;
                            entry.Height = 92;
                            break;

                        default:
                            var designEntry = design.LastOrDefault(x => x.Class == Class.Design && x.Text == val);
                            if (designEntry == null)
                            {
                                throw new Exception(string.Format("Cannot find the design '{0}'", val));
                            }
                            entry.Width = designEntry.Width;
                            entry.Height = designEntry.Height;
                            break;
                    }

                    entry.Width += 4;
                    entry.Height += 4;

                    break;

                default:
                    throw new Exception(string.Format("Unknown key '{0}'", key));
            }
        }

        private float GetSize(float size, string value)
        {
            switch (value.Substring(0, 1))
            {
                case "+":
                    return (size - 4) + Convert.ToSingle(value.Substring(1)) + 4;
                case "-":
                    return (size - 4) - Convert.ToSingle(value.Substring(1)) + 4;
                case "*":
                    return (size - 4) * Convert.ToSingle(value.Substring(1)) + 4;
                case "/":
                    return (size - 4) / Convert.ToSingle(value.Substring(1)) + 4;
                default:
                    return Convert.ToSingle(value);
            }
        }

        private void SetProgress(int i, int n)
        {
            if (progress != null)
            {
                int currentProgress = 100 * i / n;

                if (currentProgress != previousProgress)
                {
                    progress(currentProgress);

                    previousProgress = currentProgress;
                }
            }
        }
    }
}

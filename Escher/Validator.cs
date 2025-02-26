﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Validator
    {
        public const char cKeywordSeparator = '|';
        public const char cKeywordAssignment = '=';

        private const char cEndOfFile = '\0';
        private const char cLowerCaseA = 'a';
        private const char cLowerCaseZ = 'z';
        private const char cUpperCaseA = 'A';
        private const char cUpperCaseZ = 'Z';
        private const char cBlankSpace = ' ';
        private const char cDoubleQuote = '"';
        private const char cSingleQuote = '\'';
        private const char cTabulate = '\t';
        private const char cCarriageReturn = '\r';
        private const char cLineFeed = '\n';

        private char[] sCode;
        private string eCode;
        private int pCode;
        private int qCode;
        private int nCode;
        private int lCode;
        private bool bCode;

        private Action<int> progress;
        private int previousProgress;

        public int SelectionStart()
        {
            return this.pCode;
        }

        public int SelectionLength()
        {
            return this.nCode;
        }

        public bool Parse(string design, Action<int> setProgress, out string error)
        {
            bool parsed;

            progress = setProgress;
            previousProgress = 0;
            if (progress != null)
            {
                setProgress(0);
            }

            sCode = (design + cEndOfFile).ToCharArray();
            pCode = 0;
            nCode = 0;
            eCode = "";
            bCode = false;
            lCode = sCode.Length;

            if (design.Trim().Length == 0)
            {
                eCode = "Design is empty";
                parsed = false;
            }
            else
            {
                if (!ParseAlbum())
                {
                    parsed = false;
                }
                else
                {
                    if (!ParseLists())
                    {
                        parsed = false;
                    }
                    else
                    {
                        if (NextChar() == cEndOfFile)
                        {
                            parsed = true;
                        }
                        else if (NextKeyWord() == "End")
                        {
                            GetKeyWord("End");
                            parsed = true;
                        }
                        else
                        {
                            nCode = NextKeyWord().Length;
                            eCode = string.Format("Invalid attribute '{0}'", NextKeyWord());
                            parsed = false;
                        }
                    }
                }
            }

            error = eCode;

            return parsed;
        }

        public bool Parse(string design, out string error)
        {
            bool parsed;

            sCode = (design + cEndOfFile).ToCharArray();
            pCode = 0;
            nCode = 0;
            eCode = "";
            bCode = false;
            lCode = sCode.Length;

            ReadBlanks();

            if (NextChar() == cEndOfFile || NextChar() == cSingleQuote)
            {
                parsed = true;
            }
            else
            {
                switch (NextKeyWord())
                {
                    case "Album":
                        parsed = ParseAlbum();
                        break;
                    case "Country":
                        parsed = ParseCountry();
                        break;
                    case "Design":
                        parsed = ParseDesign();
                        break;
                    case "Part":
                        parsed = ParsePart();
                        break;
                    case "Series":
                        parsed = ParseSeries();
                        break;
                    case "Type":
                        parsed = ParseType();
                        break;
                    case "PageFeed":
                        parsed = ParsePageFeed();
                        break;
                    case "Varieties":
                        parsed = ParseVarieties();
                        break;
                    case "Variety":
                        parsed = ParseVariety();
                        break;
                    case "Description":
                        parsed = ParseDescription();
                        break;
                    case "LineFeed":
                        parsed = ParseLineFeed();
                        break;
                    case "End":
                        parsed = ParseEnd();
                        break;
                    case "Stamp":
                        parsed = ParseStamp();
                        break;
                    case "Image":
                        parsed = ParseImage();
                        break;
                    default:
                        parsed = SetUnknownKeyword(NextKeyWord());
                        break;
                }
            }

            error = eCode;

            return parsed;
        }
        private void SetProgress()
        {
            if (progress != null)
            {
                int currentProgress = 100 * pCode / lCode;

                if (currentProgress != previousProgress)
                {
                    progress(currentProgress);

                    previousProgress = currentProgress;
                }
            }
        }

        private bool SetUnknownKeyword(string nextKeyWord)
        {
            nCode = nextKeyWord.Length;
            eCode = string.Format("Unknown keyword '{0}'", nextKeyWord);

            return false;
        }

        private bool SetInvalidValue(string value, string keyWord)
        {
            nCode = eCode.Length;
            pCode = pCode - nCode - 1;
            eCode = string.Format("Invalid value '{0}' for keyword '{1}'", eCode, keyWord);

            return false;
        }

        private bool ParseLists()
        {
            do
            {
                if (!ParseList()) return false;

            } while (NextKeyWord() == "Country");

            return true;
        }

        private bool ParseList()
        {
            if (!ParseCountry()) return false;

            if (NextKeyWord() == "Design" && !ParseDesignList()) return false;

            if (NextKeyWord() == "Stamp" && !ParseStamps()) return false;

            if (!ParsePart()) return false;

            if (!ParseSeries()) return false;

            if (!ParsePages()) return false;

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseAlbum()
        {
            bool pdfFound = false;
            bool versionFound = false;
            int albumFound = pCode;

            if (!GetKeyWord("Album")) return false;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Pdf":
                        if (!ParseString(nextKeyWord, true)) return false;
                        pdfFound = true;
                        break;
                    case "Version":
                        if (!ParseString(nextKeyWord, true)) return false;
                        versionFound = true;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            if (!pdfFound)
            {
                pCode = albumFound;
                eCode = "Keyword 'Pdf' not found";

                return false;
            }
            if (!versionFound)
            {
                pCode = albumFound;
                eCode = "Keyword 'Version' not found";

                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseCountry()
        {
            bool frontPageTitleFound = false;
            bool frontPageSubTitleFound = false;
            bool pageTitleFound = false;
            int countryFound = pCode;

            if (!ParseString("Country", true)) return false;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Picture":
                    case "Copyright":
                    case "Settings":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    case "Catalogue":
                        if (!ParseCatalogue(nextKeyWord)) return false;
                        break;
                    case "FrontPageTitle":
                        if (!ParseString(nextKeyWord, true)) return false;
                        frontPageTitleFound = true;
                        break;
                    case "FrontPageSubTitle":
                        if (!ParseString(nextKeyWord, false)) return false;
                        frontPageSubTitleFound = true;
                        break;
                    case "PageTitle":
                        if (!ParseString(nextKeyWord, true)) return false;
                        pageTitleFound = true;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            if (!frontPageTitleFound)
            {
                pCode = countryFound;
                eCode = "Keyword 'FrontPageTitle' not found";

                return false;
            }
            if (!frontPageSubTitleFound)
            {
                pCode = countryFound;
                eCode = "Keyword 'FrontPageSubTitle' not found";

                return false;
            }
            if (!pageTitleFound)
            {
                pCode = countryFound;
                eCode = "Keyword 'PageTitle' not found";

                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseCatalogue(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            string[] values = new string[] { "scott", "michel", "yvert", "gibbons", "chan", "afinsa", "maury", "newfoundland", "afa", "facit" };

            if (!values.Contains(eCode.ToLower()))
            {
                return SetInvalidValue(eCode, keyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseAlignment(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            string[] values = new string[] { "", "left", "centered", "right" };

            if (!values.Contains(eCode.ToLower()))
            {
                return SetInvalidValue(eCode, keyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseApplyTo(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            string[] values = new string[] { "vb", "c#" };

            if (!values.Contains(eCode.ToLower()))
            {
                return SetInvalidValue(eCode, keyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseApplyToFrameStyle(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            string[] values = new string[] { "thin", "thick" };

            if (!values.Contains(eCode.ToLower()))
            {
                return SetInvalidValue(eCode, keyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseAppearance(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            string[] values = new string[] {
                "single", "singular", "rotated", "watermarkinverted", "watermarkreversed", "watermarkinvertedandreversed", "watermarksideways", "watermarksidewaysinverted",
                "pairhorizontal", "horizontalpair",
                "pairvertical", "verticalpair",
                "tetebechehorizontal", "horizontaltetebeche",
                "tetebechevertical", "verticaltetebeche",
                "tetebechehorizontalgutter", "horizontaltetebechegutter",
                "tetebecheverticalgutter", "verticaltetebechegutter",
                "horizontalgutterpair", "verticalgutterpair",
                "block", "block5x5", "block2x5", "block2x5rotated", "sheet2x3",
                "proof",
                "horizontalstrip3", "horizontalstrip4", "horizontalstrip5", "horizontalstrip6",
                "verticalstrip3", "verticalstrip4", "verticalstrip5", "verticalstrip6",
                "pairecarnet", "carnetpaire",
                "bandepublicitaire", "publicitairebande",
                "interpanneauxhorizontal", "horizontalinterpanneaux",
                "imperfright", "imperfleft", "imperftop", "imperfbottom"
            };

            if (!values.Contains(eCode.ToLower()))
            {
                return SetInvalidValue(eCode, keyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseColor(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            string[] values = new string[] { "", "black", "white" };

            if (!values.Contains(eCode.ToLower()))
            {
                return SetInvalidValue(eCode, keyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseShape(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            string[] values = new string[] { "", "rectangle", "rectanglerotated", "triangle60", "triangle60inverted", "triangle45", "triangle45inverted", "hexagonvertical" };

            if (!values.Contains(eCode.ToLower()))
            {
                return SetInvalidValue(eCode, keyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseDesignList()
        {
            do
            {
                if (!ParseDesign()) return false;

            } while (NextKeyWord() == "Design");

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseDesign()
        {
            bool widthFound = false;
            bool heightFound = false;
            int designFound = pCode;

            if (!ParseString("Design", true)) return false;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Width":
                        widthFound = true;
                        break;
                    case "Height":
                        heightFound = true;
                        break;
                }

                switch (nextKeyWord)
                {
                    case "Width":
                    case "Height":
                        if (!ParseNumber(nextKeyWord, 1, false)) return false;
                        break;
                    case "Comment":
                        if (!ParseString(nextKeyWord, false)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            if (!widthFound)
            {
                pCode = designFound;
                eCode = "Keyword 'Width' not found";

                return false;
            }
            if (!heightFound)
            {
                pCode = designFound;
                eCode = "Keyword 'Height' not found";

                return false;
            }

            return true;
        }


        /// <summary>
        /// </summary>
        private bool ParseStamps()
        {
            do
            {
                if (!ParseStamp()) return false;

            } while (NextKeyWord() == "Stamp");

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseStamp()
        {

            bool valueFound;
            bool sizeFound;
            bool widthFound;
            bool heightFound;
            int stampFound;

            valueFound = false;
            sizeFound = false;
            widthFound = false;
            heightFound = false;

            if (!ParseString("Stamp", true)) return false;

            stampFound = pCode;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Value":
                        valueFound = true;
                        break;
                    case "Size":
                        sizeFound = true;
                        break;
                    case "Width":
                        widthFound = true;
                        break;
                    case "Height":
                        heightFound = true;
                        break;
                }
 
                switch (nextKeyWord)
                {
                    case "Height":
                    case "Width":
                        if (!ParseNumber(nextKeyWord, 1, true)) return false;
                        break;
                    case "Color":
                        if (!ParseString(nextKeyWord, false)) return false;
                        break;
                    case "Value":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    case "Size":
                        if (!ParseSize(NextKeyWord())) return false;
                        break;
                    case "Comment":
                    case "Issued":
                    case "Printed":
                    case "Perforation":
                        if (!ParseString(nextKeyWord, false)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            if (!valueFound) {
                pCode = stampFound;
                eCode = "Keyword 'Value' not found";
                return false;
            }

            if (!sizeFound && !widthFound && !heightFound)
            {
                pCode = stampFound;
                eCode = "Keyword 'Size' or keywords 'Width' & 'Height' not found";
                return false;
            }
            else if (!sizeFound && widthFound && !heightFound)
            {
                pCode = stampFound;
                eCode = "Keyword 'Height' not found";
                return false;
            }
            else if (!sizeFound && !widthFound && heightFound)
            {
                pCode = stampFound;
                eCode = "Keyword  'Width' not found";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParsePart()
        {
            if (!ParseString("Part", true)) return false;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Value":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    case "Menu":
                        if (!ParseBoolean(nextKeyWord)) return false;
                        break;
                    case "Comment":
                        if (!ParseString(nextKeyWord, false)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseSeries()
        {
            bool parseSeries = ParseString("Series", false);

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Comment":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            if (NextKeyWord() == "Design" && !ParseDesignList()) return false;

            if (NextKeyWord() == "Stamp" && !ParseStamps()) return false;

            return parseSeries;
        }

        /// <summary>
        /// </summary>
        private bool ParsePages()
        {
            do
            {
                if (!ParsePage()) return false;

            } while (NextKeyWord() == "PageFeed");

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParsePage()
        {
            if (!ParsePageFeed()) return false;

            if (NextKeyWord() == "Part" && !ParsePart()) return false;

            if (NextKeyWord() == "Image" && !ParseImageList()) return false;

            if (NextKeyWord() == "Series" && !ParseSeries()) return false;

            if (NextKeyWord() == "Type" && !ParseType()) return false;

            if (!ParseSections()) return false;

            return true;
         }

        /// <summary>
        /// </summary>
        private bool ParseImageList()
        {
            do
            {
                if (!ParseImage()) return false;

            } while (NextKeyWord() == "Image");

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseSections()
        {
            do
            {
                if (!ParseSection()) return false;

            } while (NextKeyWord() == "Part" || NextKeyWord() == "Type" || NextKeyWord() == "Varieties");

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseSection()
        {
            if (NextKeyWord() == "Part" && !ParsePart()) return false;

            if (NextKeyWord() == "Type" && !ParseType()) return false;

            if (!ParseVarieties()) return false;

            if (!ParseVarietyList()) return false;

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseVarietyList()
        {
            do
            {
                if (!ParseVariety()) return false;

            } while (NextKeyWord() == "Description" || NextKeyWord() == "Variety");

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseVarieties()
        {
            if (!ParseString("Varieties", false)) return false;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Vertical":
                    case "Horizontal":
                    case "VerticalMove":
                    case "VerticalMoveRelative":
                    case "VerticalMoveAbsolute":
                        if (!ParseNumber(nextKeyWord, Int32.MinValue, false)) return false;
                        break;
                    case "Combine":
                        if (!ParseBoolean(nextKeyWord)) return false;
                        break;
                    case "Alignment":
                        if (!ParseAlignment(nextKeyWord)) return false;
                        break;
                    case "FontOfType":
                    case "FontOfDescription":
                        if (!ParseBoolean(nextKeyWord)) return false;
                        break;
                    case "Comment":
                    case "Private":
                    case "Watermark":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    case "Width":
                    case "WatermarkHeight":
                        if (!ParseNumber(nextKeyWord, 0, true)) return false;
                        break;
                    case "MaxWidth":
                        if (!ParseNumber(nextKeyWord, 50, false)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            return true;
        }

        private bool ParseImage()
        {
            bool heightFound;
            int imageFound;

            heightFound = false;

            if (!ParseString("Image", true)) return false;

            imageFound = pCode;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Height":
                        heightFound = true;
                        break;
                }

                switch (nextKeyWord)
                {
                    case "Horizontal":
                    case "Vertical":
                        if (!ParseNumber(nextKeyWord, Int32.MinValue, false)) return false;
                        break;
                    case "Height":
                    case "Width":
                        if (!ParseNumber(nextKeyWord, 1, true)) return false;
                        break;
                    case "RoundedCorners":
                        if (!ParseBoolean(nextKeyWord)) return false;
                        break;
                }
            }

            if (!heightFound)
            {
                pCode = imageFound;
                eCode = "Keyword 'Height' not found";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseVariety()
        {
            bool sizeFound;
            bool widthFound;
            bool heightFound;
            int varietyFound;

            sizeFound = false;
            widthFound = false;
            heightFound = false;

            if (NextKeyWord() == "Description" && !ParseDescription()) return false;

            if (!ParseString("Variety", true)) return false;

            varietyFound = pCode;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Size":
                        sizeFound = true;
                        break;
                    case "Width":
                        widthFound = true;
                        break;
                    case "Height":
                        heightFound = true;
                        break;
                }

                switch (nextKeyWord)
                {
                    case "Appearance":
                        if (!ParseAppearance(nextKeyWord)) return false;
                        break;
                    case "Block":
                    case "Separate":
                    case "Skip":
                    case "Variant":
                    case "Unlisted":
                        if (!ParseBoolean(nextKeyWord)) return false;
                        break;
                    case "FrameColor":
                        if (!ParseColor(nextKeyWord)) return false;
                        break;
                    case "Horizontal":
                    case "Vertical":
                        if (!ParseNumber(nextKeyWord, Int32.MinValue, false)) return false;
                        break;
                    case "Height":
                    case "Width":
                        if (!ParseNumber(nextKeyWord, 1, true)) return false;
                        break;
                    case "Picture":
                    case "Color":
                    case "Overprint":
                        if (!ParseString(nextKeyWord, false)) return false;
                        break;
                    case "Value":
                        if (!ParseString(nextKeyWord, false)) return false;
                        break;
                    case "Sheet":
                    case "Positions":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    case "Size":
                        if (!ParseSize(nextKeyWord)) return false;
                        break;
                    case "Shape":
                        if (!ParseShape(nextKeyWord)) return false;
                        break;
                    case "Sc":
                    case "Mi":
                    case "Sg":
                    case "Yv":
                    case "Ch":
                    case "Af":
                    case "Ma":
                    case "Nc":
                    case "Afa":
                    case "Fac":
                        if (!ParseString(nextKeyWord, false)) return false;
                        break;
                    case "Alignment":
                        if (!ParseAlignment(nextKeyWord)) return false;
                        break;
                    case "ApplyTo":
                        if (!ParseApplyTo(nextKeyWord)) return false;
                        break;
                    case "ApplyToFrameStyle":
                        if (!ParseApplyToFrameStyle(nextKeyWord)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            if (NextKeyWord() == "LineFeed" && !ParseLineFeed()) return false;

            if (!sizeFound && !widthFound && !heightFound)
            {
                pCode = varietyFound;
                eCode = "Keyword 'Size' or keywords 'Width' & 'Height' not found";
                return false;
            }
            else if (!sizeFound && widthFound && !heightFound)
            {
                pCode = varietyFound;
                eCode = "Keyword 'Height' not found";
                return false;
            }
            else if (!sizeFound && !widthFound && heightFound)
            {
                pCode = varietyFound;
                eCode = "Keyword 'Width' not found";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseDescription()
        {
            if (!ParseString("Description", true)) return false;

            while (NextSeparator() == cKeywordSeparator)
            { 
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Alignment":
                        if (!ParseAlignment(nextKeyWord)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }       

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParsePageFeed()
        {
            if (!GetKeyWord("PageFeed"))
            {
                if (bCode)
                {
                    bCode = false;
                }
                else
                {
                    return false;
                }
            }

            while (NextSeparator() == cKeywordSeparator)
            { 
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Comment":
                    case "Html":
                    case "PageNumber":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    case "Vertical":
                        if (!ParseNumber(nextKeyWord, Int32.MinValue, false)) return false;
                        break;
                    case "Width": // Margin between frames
                        if (!ParseNumber(nextKeyWord, -25, false)) return false;
                        break;
                    case "Value":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    case "Landscape":
                        if (!ParseBoolean(nextKeyWord)) return false;
                        break;
                    case "Combine":
                        if (!ParseBoolean(nextKeyWord)) return false;
                        break;
                    case "Variant":
                    case "Special":
                    case "SkipVariant":
                    case "Sample":
                        if (!ParseBoolean(nextKeyWord)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseLineFeed()
        {
            if (!GetKeyWord("LineFeed")) return false;

            if (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                return SetUnknownKeyword(nextKeyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseEnd()
        {
            if (!GetKeyWord("End")) return false;

            if (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                return SetUnknownKeyword(nextKeyWord);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseType()
        {
            if (!ParseString("Type", false)) return false;

            while (NextSeparator() == cKeywordSeparator)
            {
                GetSeparator(cKeywordSeparator);

                string nextKeyWord = NextKeyWord();

                switch (nextKeyWord)
                {
                    case "Comment":
                        if (!ParseString(nextKeyWord, true)) return false;
                        break;
                    default:
                        return SetUnknownKeyword(nextKeyWord);
                }
            }

            if (NextKeyWord() == "Design" && !ParseDesignList()) return false;

            if (NextKeyWord() == "Stamp" && !ParseStamps()) return false;


            return true;
	    }

        /// <summary>
        /// </summary>
        private bool ParseString(string keyWord, bool required)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, required)) return false;

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseNumber(string keyWord, int minimum, bool allowOperators)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            bool isNumeric;
            double number;

            if (allowOperators && "+-*/".Contains(eCode.Substring(0, 1)))
            {
                isNumeric = Double.TryParse(eCode.Substring(1), out number);

                if (!isNumeric)
                {
                    return SetInvalidValue(eCode, keyWord);
                }
                if (!(number > 0))
                {
                    return SetInvalidValue(eCode, keyWord);
                }
            }
            else
            {
                isNumeric = Double.TryParse(eCode.Substring(0), out number);

                if (!isNumeric)
                {
                    return SetInvalidValue(eCode, keyWord);
                }
                if (!(number >= minimum))
                {
                    return SetInvalidValue(eCode, keyWord);
                }
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseSize(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            return true;
        }

        /// <summary>
        /// </summary>
        private bool ParseBoolean(string keyWord)
        {
            if (!GetKeyWord(keyWord)) return false;

            if (!GetSeparator(cKeywordAssignment)) return false;

            if (!ReadKeyWordValue(keyWord, true)) return false;

            if (eCode.ToLower() != "true" && eCode.ToLower() != "false")
            {
                return SetInvalidValue(eCode, keyWord);
            }

            return true;
        }

        /// <summary>
        /// Returns whether or not a keyword has a value depending if this is required.
        /// The value is stored in the global variable eCode and can be used by the caller after this.
        /// </summary>
        private bool ReadKeyWordValue(string keyWord, bool required)
        {
            bool readKeyWordValue;

            char nextChar;

            eCode = ""; // Start with an empty value

            nextChar = NextChar();

            // The value ends when the next character is a vertical bar, double quote, new line, tab or end of file
            while (nextChar != cKeywordSeparator && nextChar != cDoubleQuote && nextChar != cCarriageReturn && nextChar != cLineFeed && nextChar != cTabulate && nextChar != cEndOfFile)
            {
                eCode = eCode + ReadChar(); // Read one character and append it to the value
                nextChar = NextChar(); // Read ahead one character
            }

            eCode = eCode.Trim();

            if (!required) // If the value is not mandatory then return success
            {
                readKeyWordValue = true;
            }
            else if (eCode.Trim() != "") // If the value is not empty then return success
            {
                readKeyWordValue = true;
            }
            else
            {
                nCode = 0;
                eCode = string.Format("Expected value for '{0}'", keyWord);
                readKeyWordValue = false;
            }

            return readKeyWordValue;
        }

        /// <summary>
        /// Returns the next character from the current pointer in the buffer without forwarding the pointer.
        /// </summary>
        private char NextChar()
        {
            //char c = char.Parse(sCode.Substring(pCode, 1));
            char c = sCode[pCode];

            return c;
        }

        /// <summary>
        /// Returns the next character from the current pointer in the buffer with forwarding the pointer after thiss.
        /// </summary>
        private char ReadChar()
        {
            char readChar = NextChar();

            pCode++;

            return readChar;
        }

        /// <summary>
        /// The keyword is now case sensitive!
        /// </summary>
        private string NextKeyWord()
        {
            string nextKeyWord = "";

            ReadBlanks();

            qCode = pCode;

            while (IsLetter(NextChar()))
            {
                nextKeyWord += ReadChar();
            }

            pCode = qCode;

            return nextKeyWord;
        }

        /// <summary>
        /// </summary>
        private bool GetKeyWord(string keyWord)
        {
            bool getKeyWord;

            ReadBlanks();

            string nextKeyWord = NextKeyWord();

            if (nextKeyWord == keyWord)
            {
                pCode += keyWord.Length;

                getKeyWord = true;
            }
            else
            {
                nCode = nextKeyWord.Length;

                eCode = string.Format("Expected '{0}', but found '{1}'", keyWord, nextKeyWord);

                getKeyWord = false;
            }

            SetProgress();

            return getKeyWord;
        }

        /// <summary>
        /// </summary>
        private char NextSeparator()
        {
            ReadBlanks();

            qCode = pCode;

            char nextSeparator = '\0';

            if (IsSeparator(NextChar()))
            {
                nextSeparator = ReadChar();
            }

            pCode = qCode;

            return nextSeparator;
        }

        /// <summary>
        /// </summary>
        private bool GetSeparator(char separator)
        {
            bool getSeparator;

            ReadBlanks();

            if (NextSeparator() == separator)
            {
                pCode += 1; //  NextSeparator().Length;

                getSeparator = true;
            }
            else
            {
                nCode = 1; // NextSeparator().Length;

                eCode = string.Format("Expected '{0}'", separator);

                getSeparator = false;
            }

            return getSeparator;
        }

        /// <summary>
        /// Reads all the blanks from the input buffer; these have no meaning and are ignored completely.
        /// </summary>
        private void ReadBlanks()
        {
            char nextChar;
            char readChar;

            while (IsBlank(NextChar()))
            {
                readChar = ReadChar();
            }

            // Comment on a line starts with a single quote
            if (NextChar() == cSingleQuote)
            {
                nextChar = NextChar();

                while (nextChar != cCarriageReturn && nextChar != cLineFeed && nextChar != cEndOfFile)
                {
                    readChar = ReadChar();
                    nextChar = NextChar();
                }

                ReadBlanks();
            }
        }

        /// <summary>
        /// Returns whether a character is a letter, i.e. lower case a to z or upper case A to Z.
        /// </summary>
        private bool IsLetter(char c)
        {
            return ((c >= cLowerCaseA && c <= cLowerCaseZ) || (c >= cUpperCaseA && c <= cUpperCaseZ));
        }

        /// <summary>
        /// Returns whether a character is a blank, i.e. space, double quote, right-pointing double-angle quotation mark, tab, carriage return or linefeed.
        /// </summary>
        private bool IsBlank(char c)
        {
            return (c == cBlankSpace || c == cTabulate || c == cCarriageReturn || c == cLineFeed);
        }

        /// <summary>
        /// </summary>
        private bool IsSeparator(char c)
        {
            return (c == cKeywordAssignment || c == cKeywordSeparator);
        }
    }
}

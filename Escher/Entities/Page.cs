﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Page
    {
        public string Html;
        public string Comment;
        public string ImagesPath = "";
        public string Country = "";
        public string Section = "";
        public string Copyright = "";
        public string FrontPageTitle = "";
        public string FrontPageSubTitle = "";
        public string PageTitle = "";
        public string Series = "";
        public string MainType = "";
        public float OffsetVertical;
        public string AlbumNumber = "";
        public bool IsSample;
        public float Spacing;

        public List<Varieties> Varieties = new List<Varieties>();

        public List<BackgroundImage> BackgroundImages = new List<BackgroundImage>();

        public Page(string html, string comment)
        {
            this.Html = html;
            this.Comment = comment;

            this.Spacing = 2.5F;
        }

        public string GetPageTitle()
        {
            string pageTitle;

            if (this.Html != "")
            {
                if (this.Comment == "&nbsp;")
                {
                    pageTitle = this.Html;
                }
                else
                {
                    pageTitle = this.Html + " · (" + this.Comment + ")";
                }
            }
            else
            {
                pageTitle = this.Comment;
            }

            return pageTitle.Replace("&#8470;", char.ConvertFromUtf32(0x2116));
        }

        public void AddImage(string number, float width, float height, float horizontal, float vertical, bool roundedCorners)
        {
            BackgroundImage backgroundImage = new BackgroundImage(number, width, height, horizontal, vertical, roundedCorners);

            BackgroundImages.Add(backgroundImage);
        }

        public void AddVarieties(string publicDescription, string privateDescription, string subType, float horizontal, float vertical, float verticalMoveRelative, float verticalMoveAbsolute, bool combine, Alignment alignment, bool fontOfType, bool fontOfDescritpion, float spacing, float watermarkHeight, string watermarkImage, Appearance appearance, float maxWidth)
        {
            if (Varieties.Count() == 0) // The first varieties can never be combined
            {
                combine = false;
            }

            Varieties varieties = new Varieties(subType, publicDescription, privateDescription, combine, horizontal, vertical, verticalMoveRelative, verticalMoveAbsolute, alignment, fontOfType, fontOfDescritpion, spacing, watermarkHeight, watermarkImage, appearance, maxWidth);

            Varieties.Add(varieties);
        }

        public void AddVariety(string number, string description, string title, FrameColor frameColor, float width, float height, float horizontal, float vertical, bool skip, Appearance appearance, string picture, string overprint, Shape shape, Alignment alignment, string positions)
        {
            Variety variety = new Variety(number, description, frameColor.GetColor(), width, height, horizontal, vertical, title, shape, skip, "", appearance, picture, overprint, alignment, positions);

            Varieties varieties = Varieties[Varieties.Count() - 1];

            if (varieties.Rows.Count() == 0)
            {
                varieties.Rows.Add(new List<Variety>());
            }

            varieties.Rows[varieties.Rows.Count() - 1].Add(variety);
        }

        public float RowWidth(int v, int r, float maxWidth, out int lastVarietyThatFits)
        {
            lastVarietyThatFits = 0;

            float width = 0;

            List<Variety> row = Varieties[v].Rows[r];

            for (int i = 0; i < row.Count(); i++)
            {
                width += row[i].Width;

                if (width <= maxWidth)
                {
                    lastVarietyThatFits = i;
                }

                if (i < row.Count() - 1)
                {
                    width += Spacing;
                }
            }

            return width;
        }

        public float RowWidth(int v, int r)
        {
            return RowWidth(v, r, 0, out int ignore);
        }

        public float RowHeight(int v, int r)
        {
            float rowHeight = 0;

            List<Variety> row = Varieties[v].Rows[r];

            for (int s = 0; s < row.Count(); s++)
            {
                if (row[s].Height > rowHeight)
                {
                    rowHeight = row[s].Height;
                }
            }

            return rowHeight;
        }

        public float RowLeft(int v, int r, float freeLeft, float freeWidth)
        {
            float rowLeft = freeLeft + Center(v, r, freeWidth) - RowWidth(v, r) / 2;

            if (rowLeft < freeLeft)
            {
                rowLeft = freeLeft;
            }
            else if (rowLeft + RowWidth(v, r) > freeLeft + freeWidth)
            {
                rowLeft = freeLeft + freeWidth - RowWidth(v, r);
            }

            return rowLeft;
        }

        public float Center(int v, int r, float freeWidth)
        {
            float center;

            float spacing = Varieties[v].Spacing;

            float width = 0;

            int n = 0;
            int i = 0;

            if (v < Varieties.Count() - 1 && Varieties[v + 1].Combine)
            {
                width = RowWidth(v, r);

                n = 1;
                i = 1;

                for (int w = v + 1; w < Varieties.Count(); w++)
                {
                    if (!Varieties[w].Combine)
                    {
                        break;
                    }

                    width += spacing + RowWidth(w, 0);
                    n++;
                }
            }

            if (Varieties[v].Combine)
            {
                width = RowWidth(v, 0);

                n = 1;
                i = 1;

                for (int w = v + 1; w < Varieties.Count(); w++)
                {
                    if (!Varieties[w].Combine)
                    {
                        break;
                    }

                    width += spacing + RowWidth(w, 0);
                    n++;
                }

                for (int w = v - 1; w >= 0; w--)
                {
                    i++;

                    if (!Varieties[w].Combine)
                    {
                        break;
                    }

                    width += spacing + RowWidth(w, 0);
                    n++;
                }
                n++;
            }

            if (n == 0)
            {
                center = freeWidth / 2;
            }
            else
            {
                center = (i - 0.5F) * freeWidth / n;

                if (center - RowWidth(v, r) / 2 < 0)
                {
                    if (!Varieties[v].Combine)
                    {
                        center = (freeWidth - width) / 2 + RowWidth(v, r) / 2;
                    }
                }
                else if (Varieties[v].Combine)
                {
                    if (center < Center(v - 1, 0, freeWidth) + RowWidth(v - 1, 0) / 2 + spacing + RowWidth(v, r) / 2)
                    {
                        center = Center(v - 1, 0, freeWidth) + RowWidth(v - 1, 0) / 2 + spacing + RowWidth(v, r) / 2;
                    }
                }
            }

            return center;
        }

        public float FrameLeft(int v, int r, int s, float freeLeft, float freeWidth)
        {
            float frameLeft = RowLeft(v, r, freeLeft, freeWidth);

            for (int i = 0; i < s; i++)
            {
                frameLeft += Varieties[v].Rows[r][i].Width;
                frameLeft += Varieties[v].Spacing;
            }

            frameLeft += Varieties[v].Rows[r][s].Horizontal;

            return frameLeft;
        }

        public float FrameOffset(int v, int r, int s)
        {
            float frameOffset = Varieties[v].Rows[r][s].Vertical;

            return frameOffset;
        }

        public float FrameWidth(int v, int r, int s)
        {
            float frameWidth = 0;

            for (int i = s; i < Varieties[v].Rows[r].Count(); i++)
            {
                if (i != s & !string.IsNullOrEmpty(Varieties[v].Rows[r][s].Title))
                {
                    break;
                }

                frameWidth += Varieties[v].Rows[r][s].Width + Varieties[v].Spacing;
            }

            frameWidth -= Varieties[v].Spacing;

            return frameWidth;
        }
    }
}

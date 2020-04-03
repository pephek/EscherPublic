using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class ImageHelper
    {
        private static string NormalizeNumber(string number)
        {
            if (number.Contains('('))
            {
                number = number.Substring(0, number.IndexOf('(') - 1).Trim();
            }

            // Remove any spaces in the number
            number = number.Replace(" ", "");

            return number;
        }

        public static Image GetDisplayImage(string path, string number, ColorStyle colorStyle)
        {
            Image image = null;

            number = NormalizeNumber(number);

            path = string.Format("{0}\\{1}\\{2}.jpg", path, colorStyle == ColorStyle.Color ? "xlcolor" : "xlprint", number);

            if (File.Exists(path))
            {
                image = Image.FromFile(path);
            }

            return image;
        }

        public static string GetThumbnailImage(string imagesFolder, string country, string section, string number, float width, float height, out bool existsDisplayImage)
        {
            number = NormalizeNumber(number);

            string thumbnail = string.Format("{0}\\{1}\\{2}\\{3}.jpg", imagesFolder, country, section, number);

            if (!File.Exists(thumbnail))
            {
                string folder;

                country = country.CapitalizeFirstLetters();

                folder = string.Format("{0}\\{1}", imagesFolder, country.CapitalizeFirstLetters());
                Directory.CreateDirectory(folder);

                folder = string.Format("{0}\\{1}", folder, section.CapitalizeFirstLetters());
                Directory.CreateDirectory(folder);

                Directory.CreateDirectory(string.Format("{0}\\image", folder));
                Directory.CreateDirectory(string.Format("{0}\\color", folder));
                Directory.CreateDirectory(string.Format("{0}\\print", folder));
                Directory.CreateDirectory(string.Format("{0}\\xlcolor", folder));
                Directory.CreateDirectory(string.Format("{0}\\xlprint", folder));

                string source = string.Format("{0}\\image\\{1}.jpg", folder, number);

                if (File.Exists(source))
                {
                    CreateThumbnail(source, thumbnail, width, height);
                }

            }

            existsDisplayImage = File.Exists(string.Format("{0}\\{1}\\{2}\\xlcolor\\{3}.jpg", imagesFolder, country, section, number));

            return thumbnail;
        }

        private static void CreateThumbnail(string large, string small, float width, float height)
        {
            int thumbnailWidth;
            int thumbnailHeight;

            using (Image image = Image.FromFile(large))
            {
                float imageWidth = (float)image.Width / image.HorizontalResolution * 25.4F;
                float imageHeight = (float)image.Height / image.VerticalResolution * 25.4F;

                width -= 10;
                height -= 10;

                float ratio = width / imageWidth < height / imageHeight ? width / imageWidth : height / imageHeight;

                thumbnailWidth = (int)(ratio * image.Width);
                thumbnailHeight = (int)(ratio * image.Height);
            }

            CreateThumbnail(large, small, thumbnailWidth, thumbnailHeight, 100);
        }

        private static void CreateThumbnail(string large, string small, int width, int height, int quality)
        {
            Bitmap smallBitmap;
            Graphics graphics;

            Bitmap largeBitmap = new Bitmap(large);

            smallBitmap = new Bitmap(width, height);

            graphics = Graphics.FromImage(smallBitmap);

            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            graphics.DrawImage(largeBitmap, 0, 0, width, height);

            graphics.Dispose();

            EncoderParameters encoderParams = new EncoderParameters();
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            ImageCodecInfo[] imageCodesInfo = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < imageCodesInfo.Length; i++)
            {
                if (imageCodesInfo[i].FormatDescription.Equals("JPEG"))
                {
                    smallBitmap.Save(small, imageCodesInfo[i], encoderParams);
                }
            }

            smallBitmap.Dispose();
            largeBitmap.Dispose();
        }
    }
}

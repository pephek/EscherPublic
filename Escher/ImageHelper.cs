using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public class ImageHelper
    {
        public static Image LoadImageAndUnlock(string imagePath)
        {
            Bitmap bitmap = null;

            if (File.Exists(imagePath))
            {
                using (Image image = Image.FromFile(imagePath))
                {
                    bitmap = new Bitmap(image);
                }
            }

            return bitmap;
        }

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
            number = NormalizeNumber(number);

            path = string.Format("{0}\\{1}\\{2}.jpg", path, colorStyle == ColorStyle.Color ? "xlcolor" : "xlprint", number);

            return LoadImageAndUnlock(path);
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

        public static void CreateThumbnail(string large, string small, float width, float height)
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

                ratio *= 120 / image.HorizontalResolution; // !!!

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

    public static class PictureBoxExtensionMethods
    {
        public static void LoadImageAndUnlock(this PictureBox pictureBox, string imagePath)
        {
            Image image = ImageHelper.LoadImageAndUnlock(imagePath) ?? Escher.Properties.Resources.ImageNotFound;

            pictureBox.Image = image;
        }
    }

    public static class ImageExtensionMethods
    {
        public static Image GetSelection(this Image image, Rectangle selection, bool convertToGrayscale)
        {
            LocklessBitmap bitmap = new LocklessBitmap(selection.Width, selection.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap.Bitmap))
            {
                //graphics.DrawImage(new Bitmap(image), new Rectangle(0, 0, selection.Width, selection.Height), selection, GraphicsUnit.Pixel);
                graphics.DrawImage(image, new Rectangle(0, 0, selection.Width, selection.Height), selection, GraphicsUnit.Pixel);
            }

            if (convertToGrayscale)
            {
                bitmap.SetToGrayscale();
            }

            return bitmap.Bitmap;
        }

        public static void SaveAsJpeg(this Image image, string jpeg, long quality)
        {
            ImageCodecInfo imageCodecInfo;
            System.Drawing.Imaging.Encoder encoder;
            EncoderParameter encoderParameter;
            EncoderParameters encoderParameters;

            imageCodecInfo = GetEncoderInfo("image/jpeg");
            encoder = System.Drawing.Imaging.Encoder.Quality;
            encoderParameters = new EncoderParameters(1);
            encoderParameter = new EncoderParameter(encoder, 100L);
            encoderParameters.Param[0] = encoderParameter;

            image.Save(jpeg, imageCodecInfo, encoderParameters);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static Bitmap Brighten(this Image image, float brightness)
        {
            float contrast = 1.0f; // no change in contrast
            float gamma = 1.0f; // no change in gamma

            //float adjustedBrightness = brightness - 1.0f;

            // create matrix that will brighten and contrast the image
            float[][] ptsArray = {
                new float[] {contrast, 0, 0, 0, 0}, // scale red
                new float[] {0, contrast, 0, 0, 0}, // scale green
                new float[] {0, 0, contrast, 0, 0}, // scale blue
                new float[] {0, 0, 0, 1.0f, 0},     // don't scale alpha
                new float[] { brightness, brightness, brightness, 0, 1}};

            ImageAttributes imageAttributes = new ImageAttributes();

            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);

            Bitmap newBitmap = new Bitmap(image.Width, image.Height);

            Graphics g = Graphics.FromImage(newBitmap);

            g.DrawImage(image, new Rectangle(0, 0, newBitmap.Width, newBitmap.Height), 0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel, imageAttributes);

            return newBitmap;
        }

        /// <summary>
        /// Method to rotate an Image object. The result can be one of three cases:
        /// - upsizeOk = true: output image will be larger than the input, and no clipping occurs 
        /// - upsizeOk = false & clipOk = true: output same size as input, clipping occurs
        /// - upsizeOk = false & clipOk = false: output same size as input, image reduced, no clipping
        /// 
        /// A background color must be specified, and this color will fill the edges that are not 
        /// occupied by the rotated image. If color = transparent the output image will be 32-bit, 
        /// otherwise the output image will be 24-bit.
        /// 
        /// Note that this method always returns a new Bitmap object, even if rotation is zero - in 
        /// which case the returned object is a clone of the input object. 
        /// </summary>
        /// <param name="inputImage">input Image object, is not modified</param>
        /// <param name="angleDegrees">angle of rotation, in degrees</param>
        /// <param name="upsizeOk">see comments above</param>
        /// <param name="clipOk">see comments above, not used if upsizeOk = true</param>
        /// <param name="backgroundColor">color to fill exposed parts of the background</param>
        /// <returns>new Bitmap object, may be larger than input image</returns>
        public static Bitmap Rotate(this Image inputImage, float angleDegrees, bool upsizeOk, bool clipOk, Color backgroundColor)
        {
            // Test for zero rotation and return a clone of the input image
            if (angleDegrees == 0f)
                return (Bitmap)inputImage.Clone();

            // Set up old and new image dimensions, assuming upsizing not wanted and clipping OK
            int oldWidth = inputImage.Width;
            int oldHeight = inputImage.Height;
            int newWidth = oldWidth;
            int newHeight = oldHeight;
            float scaleFactor = 1f;

            // If upsizing wanted or clipping not OK calculate the size of the resulting bitmap
            if (upsizeOk || !clipOk)
            {
                double angleRadians = angleDegrees * Math.PI / 180d;

                double cos = Math.Abs(Math.Cos(angleRadians));
                double sin = Math.Abs(Math.Sin(angleRadians));
                newWidth = (int)Math.Round(oldWidth * cos + oldHeight * sin);
                newHeight = (int)Math.Round(oldWidth * sin + oldHeight * cos);
            }

            // If upsizing not wanted and clipping not OK need a scaling factor
            if (!upsizeOk && !clipOk)
            {
                scaleFactor = Math.Min((float)oldWidth / newWidth, (float)oldHeight / newHeight);
                newWidth = oldWidth;
                newHeight = oldHeight;
            }

            // Create the new bitmap object. If background color is transparent it must be 32-bit, 
            //  otherwise 24-bit is good enough.
            Bitmap newBitmap = new Bitmap(newWidth, newHeight, backgroundColor == Color.Transparent ?
                                             PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
            newBitmap.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);

            // Create the Graphics object that does the work
            using (Graphics graphicsObject = Graphics.FromImage(newBitmap))
            {
                graphicsObject.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsObject.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsObject.SmoothingMode = SmoothingMode.HighQuality;

                // Fill in the specified background color if necessary
                if (backgroundColor != Color.Transparent)
                    graphicsObject.Clear(backgroundColor);

                // Set up the built-in transformation matrix to do the rotation and maybe scaling
                graphicsObject.TranslateTransform(newWidth / 2f, newHeight / 2f);

                if (scaleFactor != 1f)
                    graphicsObject.ScaleTransform(scaleFactor, scaleFactor);

                graphicsObject.RotateTransform(angleDegrees);
                graphicsObject.TranslateTransform(-oldWidth / 2f, -oldHeight / 2f);

                // Draw the result 
                graphicsObject.DrawImage(inputImage, 0, 0);
            }

            return newBitmap;
        }
    }
}

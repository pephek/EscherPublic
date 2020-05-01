using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public static class ImageHelper
    {
        public static Bitmap FrameTopLeft;
        public static Bitmap FrameTopRight;
        public static Bitmap FrameBottomLeft;
        public static Bitmap FrameBottomRight;

        public static Bitmap FrameTop;
        public static Bitmap FrameBottom;
        public static Bitmap FrameLeft;
        public static Bitmap FrameRight;

        public static Bitmap FrameCenterTop;
        public static Bitmap FrameCenterBottom;
        public static Bitmap FrameCenterVertical;

        public static Bitmap FrameCenterLeft;
        public static Bitmap FrameCenterRight;
        public static Bitmap FrameCenterHorizontal;

        public static Bitmap FrameCenter;

        static ImageHelper()
        {
            const int cFrameSize = 196; // Pixels
            const int cBorderSize = 28; // Pixels

            Bitmap bitmap = Escher.Properties.Resources.Frame;

            Rectangle topLeft = new Rectangle(0, 0, cBorderSize, cBorderSize);
            FrameTopLeft = bitmap.Clone(topLeft, bitmap.PixelFormat);

            Rectangle topRight = new Rectangle(cFrameSize - cBorderSize, 0, cBorderSize, cBorderSize);
            FrameTopRight = bitmap.Clone(topRight, bitmap.PixelFormat);

            Rectangle bottomLeft = new Rectangle(0, cFrameSize - cBorderSize, cBorderSize, cBorderSize);
            FrameBottomLeft = bitmap.Clone(bottomLeft, bitmap.PixelFormat);

            Rectangle bottomRight = new Rectangle(cFrameSize - cBorderSize, cFrameSize - cBorderSize, cBorderSize, cBorderSize);
            FrameBottomRight = bitmap.Clone(bottomRight, bitmap.PixelFormat);

            Rectangle top = new Rectangle(cBorderSize, 0, cBorderSize, cBorderSize);
            FrameTop = bitmap.Clone(top, bitmap.PixelFormat);

            Rectangle bottom = new Rectangle(cBorderSize, cFrameSize - cBorderSize, cBorderSize, cBorderSize);
            FrameBottom = bitmap.Clone(bottom, bitmap.PixelFormat);

            Rectangle left = new Rectangle(0, cBorderSize, cBorderSize, cBorderSize);
            FrameLeft = bitmap.Clone(left, bitmap.PixelFormat);

            Rectangle right = new Rectangle(cFrameSize - cBorderSize, cBorderSize, cBorderSize, cBorderSize);
            FrameRight = bitmap.Clone(right, bitmap.PixelFormat);

            Rectangle centerTop = new Rectangle(cFrameSize / 2 - 1 - cBorderSize / 2, 0, cBorderSize, 2 * cBorderSize);
            FrameCenterTop = bitmap.Clone(centerTop, bitmap.PixelFormat);

            Rectangle centerBottom = new Rectangle(cFrameSize / 2 - 1 - cBorderSize / 2, cFrameSize - 2 * cBorderSize, cBorderSize, 2 * cBorderSize);
            FrameCenterBottom = bitmap.Clone(centerBottom, bitmap.PixelFormat);

            Rectangle centerVertical = new Rectangle(cFrameSize / 2 - 1 - cBorderSize / 2, cBorderSize, cBorderSize, cBorderSize);
            FrameCenterVertical = bitmap.Clone(centerVertical, bitmap.PixelFormat);

            Rectangle centerLeft = new Rectangle(0, cFrameSize / 2 - 1 - cBorderSize / 2, 2 * cBorderSize, cBorderSize);
            FrameCenterLeft = bitmap.Clone(centerLeft, bitmap.PixelFormat);

            Rectangle centerRight = new Rectangle(cFrameSize - 2 * cBorderSize, cFrameSize / 2 - 1 - cBorderSize / 2, 2 * cBorderSize, cBorderSize);
            FrameCenterRight = bitmap.Clone(centerRight, bitmap.PixelFormat);

            Rectangle centerHorizontal = new Rectangle(cBorderSize, cFrameSize / 2 - 1 - cBorderSize / 2, cBorderSize, cBorderSize);
            FrameCenterHorizontal = bitmap.Clone(centerHorizontal, bitmap.PixelFormat);

            Rectangle center = new Rectangle(cFrameSize / 2 - 1 - cBorderSize / 2, cFrameSize / 2 - 1 - cBorderSize / 2, cBorderSize, cBorderSize);
            FrameCenter = bitmap.Clone(center, bitmap.PixelFormat);
        }

        public static Image LoadImageAndUnlock(string imagePath)
        {
            Bitmap bitmap = null;

            if (imagePath != null && File.Exists(imagePath))
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

        public static string GetDisplayImagePath(string path, string number, ColorStyle colorStyle)
        {
            number = NormalizeNumber(number);

            path = string.Format("{0}\\{1}\\{2}.jpg", path, colorStyle == ColorStyle.Color ? "xlcolor" : "xlprint", number);

            return (File.Exists(path) ? path : null);
        }

        public static Image GetDisplayImage(string path, string number, ColorStyle colorStyle)
        {
            number = NormalizeNumber(number);

            path = string.Format("{0}\\{1}\\{2}.jpg", path, colorStyle == ColorStyle.Color ? "xlcolor" : "xlprint", number);

            return ImageHelper.LoadImageAndUnlock(path);
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
            Bitmap largeBitmap = new Bitmap(large);

            Bitmap thumbnailBitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(thumbnailBitmap))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                g.DrawImage(largeBitmap, 0, 0, width, height);
            }

            ImageCodecInfo imageCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(e => e.MimeType == "image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters();
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            thumbnailBitmap.Save(small, imageCodecInfo, encoderParams);

            thumbnailBitmap.Dispose();
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

        public static void SaveAsJpeg(this Bitmap bitmap, string jpeg, long quality)
        {
            ((Image)bitmap).SaveAsJpeg(jpeg, quality);
        }

        public static void SaveAsJpeg(this Image image, string jpeg, long quality)
        {
            ImageCodecInfo imageCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(e => e.MimeType == "image/jpeg");
            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(encoder, 100L);
            encoderParameters.Param[0] = encoderParameter;

            image.Save(jpeg, imageCodecInfo, encoderParameters);
        }

        public static Bitmap Recolor(this Image image, float r, float g, float b)
        {
            LocklessBitmap recoloredBitmap = new LocklessBitmap(image.Width, image.Height);

            using (Graphics graphics = Graphics.FromImage(recoloredBitmap.Bitmap))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
            }

            recoloredBitmap.Recolor(r, g, b);

            return recoloredBitmap.Bitmap;
        }

        public static Bitmap Blacken(this Image image, byte threshold)
        {
            LocklessBitmap blackendBitmap = new LocklessBitmap(image.Width, image.Height);

            using (Graphics graphics = Graphics.FromImage(blackendBitmap.Bitmap))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
            }

            blackendBitmap.Blacken(threshold);

            return blackendBitmap.Bitmap;
        }

        public static Bitmap Measure(this Image image, byte threshold, string perforation, out string size)
        {
            LocklessBitmap bitmap = new LocklessBitmap(image.Width, image.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap.Bitmap))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
            }

            size = SizeHelper.Measure(bitmap, SizeHelper.GetPerforations(perforation), threshold);

            return bitmap.Bitmap;
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

            Bitmap brightenedBitmap = new Bitmap(image.Width, image.Height);

            using (Graphics g = Graphics.FromImage(brightenedBitmap))
            {
                g.DrawImage(image, new Rectangle(0, 0, brightenedBitmap.Width, brightenedBitmap.Height), 0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel, imageAttributes);
            }


            return brightenedBitmap;
        }

        public static Bitmap Resize(this Bitmap bitmap, float ratio)
        {
            return ((Image)bitmap).Resize(ratio);
        }

        public static Bitmap Resize(this Image image, float ratio)
        {
            Bitmap resizedBitmap;

            int width = (int)(ratio * image.Width);
            int height = (int)(ratio * image.Height);

            resizedBitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                g.DrawImage(image, 0, 0, width, height);
            }

            return resizedBitmap;
        }

        /// <summary>
        /// Sharpens the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="strength">The strength between 0.0 and 1.0.</param>
        /// <returns></returns>
        public static Bitmap Sharpen(this Image image, double strength)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap sharpenedImage = new Bitmap(image);

            // Create sharpening filter.
            const int filterSize = 5;

            var filter = new double[,]
                {
            {-1, -1, -1, -1, -1},
            {-1,  2,  2,  2, -1},
            {-1,  2, 16,  2, -1},
            {-1,  2,  2,  2, -1},
            {-1, -1, -1, -1, -1}
                };

            double bias = 1.0 - strength;
            double factor = strength / 16.0;

            const int s = filterSize / 2;

            var result = new Color[image.Width, image.Height];

            // Lock image bits for read/write.
            if (sharpenedImage != null)
            {
                BitmapData pbits = sharpenedImage.LockBits(new Rectangle(0, 0, width, height),
                                                            ImageLockMode.ReadWrite,
                                                            PixelFormat.Format24bppRgb);

                // Declare an array to hold the bytes of the bitmap.
                int bytes = pbits.Stride * height;
                var rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

                int rgb;
                // Fill the color array with the new sharpened color values.
                for (int x = s; x < width - s; x++)
                {
                    for (int y = s; y < height - s; y++)
                    {
                        double red = 0.0, green = 0.0, blue = 0.0;

                        for (int filterX = 0; filterX < filterSize; filterX++)
                        {
                            for (int filterY = 0; filterY < filterSize; filterY++)
                            {
                                int imageX = (x - s + filterX + width) % width;
                                int imageY = (y - s + filterY + height) % height;

                                rgb = imageY * pbits.Stride + 3 * imageX;

                                red += rgbValues[rgb + 2] * filter[filterX, filterY];
                                green += rgbValues[rgb + 1] * filter[filterX, filterY];
                                blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                            }

                            rgb = y * pbits.Stride + 3 * x;

                            int r = Math.Min(Math.Max((int)(factor * red + (bias * rgbValues[rgb + 2])), 0), 255);
                            int g = Math.Min(Math.Max((int)(factor * green + (bias * rgbValues[rgb + 1])), 0), 255);
                            int b = Math.Min(Math.Max((int)(factor * blue + (bias * rgbValues[rgb + 0])), 0), 255);

                            result[x, y] = Color.FromArgb(r, g, b);
                        }
                    }
                }

                // Update the image with the sharpened pixels.
                for (int x = s; x < width - s; x++)
                {
                    for (int y = s; y < height - s; y++)
                    {
                        rgb = y * pbits.Stride + 3 * x;

                        rgbValues[rgb + 2] = result[x, y].R;
                        rgbValues[rgb + 1] = result[x, y].G;
                        rgbValues[rgb + 0] = result[x, y].B;
                    }
                }

                // Copy the RGB values back to the bitmap.
                Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
                // Release image bits.
                sharpenedImage.UnlockBits(pbits);
            }

            return sharpenedImage;
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

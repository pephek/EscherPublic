using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class LocklessBitmap
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public LocklessBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }

    public static class LocklessBitmapExtensionMethods
    {
        public static void SetToGrayscale(this LocklessBitmap bitmap)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    // Convert to grayscale as used in television
                    int grayscale = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);

                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, grayscale, grayscale, grayscale));
                }
            }
        }

        public static void Recolor(this LocklessBitmap bitmap, float r, float g, float b)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    byte rPixel = pixel.R;
                    byte gPixel = pixel.G;
                    byte bPixel = pixel.B;

                    if (r != 0)
                        rPixel = (byte)Math.Min((1 - r) * pixel.R, 255);
                    if (g != 0)
                        gPixel = (byte)Math.Min((1 - g) * pixel.G, 255);
                    if (b != 0)
                        bPixel = (byte)Math.Min((1 - b) * pixel.B, 255);

                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, rPixel, gPixel, bPixel));
                }
            }
        }
    }
}

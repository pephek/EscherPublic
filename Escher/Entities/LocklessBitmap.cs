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

        public int GetGrayscale(int x, int y)
        {
            Color pixel = GetPixel(x, y);

            // Convert to grayscale as used in television
            return (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
        }

        public bool IsColor(int x, int y, Color color)
        {
            return Bits[x + (y * Width)] == color.ToArgb();
        }

        public bool IsColor(int x1, int y1, int x2, int y2, Color color, float percentage)
        {
            int count = 0;
            int match = 0;

            for (int y = y1; y <= y2; y++)
            {
                for (int x = x1; x <= x2; x++)
                {
                    count++;
                    if (IsColor(x, y, color))
                    {
                        match++;
                    }
                }
            }

            return (float)match / count >= percentage;
        }

        public void SetPixels(int x1, int y1, int x2, int y2, Color color)
        {
            for (int y = y1; y <= y2; y++)
            {
                for (int x = x1; x <= x2; x++)
                {
                    SetPixel(x, y, color);
                }
            }
        }

        public LocklessBitmap Copy(Rectangle area)
        {
            LocklessBitmap copy = new LocklessBitmap(area.Width, area.Height);

            for (int y = 0; y < area.Height; y++)
            {
                for (int x = 0; x < area.Width; x++)
                {
                    copy.SetPixel(x, y, GetPixel(x + area.X, y + area.Y));
                }
            }

            return copy;
        }

        public void Copy(LocklessBitmap bitmap, Rectangle area)
        {
            for (int y = 0; y < area.Height; y++)
            {
                for (int x = 0; x < area.Width; x++)
                {
                    SetPixel(x + area.X, y + area.Y, bitmap.GetPixel(x, y));
                }
            }
        }

        public LocklessBitmap Rotate(int angle)
        {
            LocklessBitmap rotated = null;

            switch (angle)
            {
                case 0:
                    rotated = new LocklessBitmap(Width, Height);
                    for (int x = 0; x < rotated.Width; x++)
                    {
                        for (int y = 0; y < rotated.Height; y++)
                        {
                            rotated.SetPixel(x, y, GetPixel(x, y));
                        }
                    }
                    break;
                case 180:
                    rotated = new LocklessBitmap(Width, Height);
                    for (int x = 0; x < rotated.Width; x++)
                    {
                        for (int y = 0; y < rotated.Height; y++)
                        {
                            rotated.SetPixel(x, y, GetPixel((Width - 1) - x, (Height - 1) - y));
                        }
                    }
                    break;
                case 90:
                    rotated = new LocklessBitmap(Height, Width);
                    for (int y = 0; y < rotated.Height; y++)
                    {
                        for (int x = 0; x < rotated.Width; x++)
                        {
                            rotated.SetPixel(x, y, GetPixel(y, (Height - 1) - x));
                        }
                    }
                    break;
                case -90:
                    rotated = new LocklessBitmap(Height, Width);
                    for (int y = 0; y < rotated.Height; y++)
                    {
                        for (int x = 0; x < rotated.Width; x++)
                        {
                            rotated.SetPixel(x, y, GetPixel((Width - 1) - y, x));
                        }
                    }
                    break;
            }

            return rotated;
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
                    int grayscale = bitmap.GetGrayscale(x, y);

                    bitmap.SetPixel(x, y, Color.FromArgb(bitmap.GetPixel(x, y).A, grayscale, grayscale, grayscale));
                }
            }
        }

        public static void Blacken(this LocklessBitmap bitmap, byte threshold)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                int x = 0;

                while (x <= bitmap.Width - 1 && bitmap.GetGrayscale(x, y) <= threshold)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(bitmap.GetPixel(x, y).A, 0, 0, 0));
                    x++;
                }

                x = bitmap.Width - 1;

                while (x >= 0 && bitmap.GetGrayscale(x, y) <= threshold)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(bitmap.GetPixel(x, y).A, 0, 0, 0));
                    x--;
                }
            }

            for (int x = 0; x < bitmap.Width; x++)
            {
                int y = 0;

                while (y <= bitmap.Height - 1 && bitmap.GetGrayscale(x, y) <= threshold)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(bitmap.GetPixel(x, y).A, 0, 0, 0));
                    y++;
                }

                y = bitmap.Height - 1;

                while (y >= 0 && bitmap.GetGrayscale(x, y) <= threshold)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(bitmap.GetPixel(x, y).A, 0, 0, 0));
                    y--;
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

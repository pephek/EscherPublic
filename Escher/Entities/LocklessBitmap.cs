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

        public bool HasColor(int x1, int y1, int x2, int y2, Color color, float percentage)
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

        public static bool Measure(this LocklessBitmap bitmap, byte threshold)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            int xMin = width / 8;
            int xMax = width - xMin;
            int yMin = height / 8;
            int yMax = height - yMin;

            Rectangle[] areas = new Rectangle[]
            { 
                new Rectangle(xMin, 0, xMax - xMin, yMin),
                new Rectangle(xMax, yMin, xMin, yMax - yMin),
                new Rectangle(xMin, yMax, xMax - xMin, yMin),
                new Rectangle(0, yMin, xMin, yMax - yMin)
            };

            int[] rotation = new int[] { 0, -90, 180, +90 };

            int[] antiRotation = new int[] { 0, +90, 180, -90 };

            LocklessBitmap[] bitmaps = new LocklessBitmap[]
            {
                bitmap.Copy(areas[0]).Rotate(rotation[0]),
                bitmap.Copy(areas[1]).Rotate(rotation[1]),
                bitmap.Copy(areas[2]).Rotate(rotation[2]),
                bitmap.Copy(areas[3]).Rotate(rotation[3])
            };

            int[,] bounds = new int[4,2];

            float[][] perforations = new float[4][];

            Dictionary<int, float>[] teeth = new Dictionary<int, float>[4];
            Dictionary<int, float>[] holes = new Dictionary<int, float>[4];

            for (int i = 0; i < bitmaps.Length; i++)
            {
                LocklessBitmap b = bitmaps[i];

                teeth[i] = new Dictionary<int, float>();
                holes[i] = new Dictionary<int, float>();

                bounds[i, 0] = 0;
                bounds[i, 1] = 0;

                perforations[i] = new float[0];

                for (int y = 0; y < b.Height; y++)
                {
                    for (int x = 0; x < b.Width; x++)
                    {
                        b.SetPixel(x, y, b.GetGrayscale(x, y) <= threshold ? Color.Black : Color.White);
                    }
                }

                bool removed = true;

                while (removed)
                {
                    removed = false;

                    for (int y = 1; y < b.Height - 1; y++)
                    {
                        for (int x = 1; x < b.Width - 1; x++)
                        {
                            Color otherColor = b.IsColor(x, y, Color.Black) ? Color.White : Color.Black;

                            int otherCount = 0;

                            for (int dx = -1; dx <= +1; dx++)
                            {
                                for (int dy = -1; dy <= +1; dy++)
                                {
                                    if (!(dx == 0 && dy == 0))
                                    {
                                        if (b.IsColor(x + dx, y + dy, otherColor))
                                        {
                                            otherCount++;
                                        }
                                    }
                                }
                            }

                            if (otherCount >= 6)
                            {
                                b.SetPixel(x, y, otherColor);

                                removed = true;
                            }
                        }
                    }
                }

                int y1, y2;

                y1 = 0;
                while (y1 < b.Height && b.HasColor(0, y1, b.Width - 1, y1, Color.Black, 0.90F))
                {
                    y1++;
                }

                if (!(y1 > 0 && y1 < b.Height))
                {
                    continue; // Not found the upper bound, so continue with the next side
                }

                y2 = y1;
                while (y2 < b.Height && !b.HasColor(0, y2, b.Width - 1, y2, Color.White, 1))
                {
                    y2++;
                }

                if (!(y2 > y1 && y2 < b.Height))
                {
                    continue; // Not found the lower bound, so continue with the next side
                }

                for (int x = 1; x < b.Width - 1; x++) // Iterate from +1 to Width-2 because of the pixel removal !!!
                {
                    for (int y = y2; y > y1; y--)
                    {
                        if (!b.IsColor(x, y, Color.White))
                        {
                            for (int yy = y; yy > y1; yy--)
                            {
                                b.SetPixel(x, yy, Color.Black);
                            }

                            break;
                        }
                    }
                }

                perforations[i] = new float[b.Width - 2];

                float[] perfs = perforations[i];

                for (int x = 1; x < b.Width - 1; x++) // Iterate from +1 to Width-2 because of the pixel removal which not done at the side !!!
                {
                    for (int y = y1; y <= y2; y++)
                    {
                        if (b.IsColor(x, y, Color.White))
                        {
                            perfs[x - 1] = y;

                            break;
                        }
                        else
                        {
                            b.SetPixel(x, y, Color.LightGray);
                        }
                    }
                }

                for (int m = 1; m <= 25; m++)
                {
                    if (m % 2 == 0)
                    {
                        for (int p = 1; p < perfs.Length - 1; p++)
                        {
                            perfs[p] = (perfs[p - 1] + perfs[p] + perfs[p + 1]) / 3;
                        }
                    }
                    else
                    {
                        for (int p = perfs.Length - 2; p >= 1; p--)
                        {
                            perfs[p] = (perfs[p - 1] + perfs[p] + perfs[p + 1]) / 3;
                        }

                    }
                }

                teeth[i] = new Dictionary<int, float>();
                holes[i] = new Dictionary<int, float>();

                for (int p = 1; p < perfs.Length - 1; p++)
                {
                    if (perfs[p] < perfs[p - 1] && perfs[p] < perfs[p + 1])
                    {
                        teeth[i].Add(p, perfs[p]);
                    }
                    if (perfs[p] > perfs[p - 1] && perfs[p] > perfs[p + 1])
                    {
                        holes[i].Add(p, perfs[p]);
                    }
                }

                for (int x = 1; x < b.Width - 1; x++)
                {
                    int y = (int)perfs[x - 1];
                    b.SetPixels(x, y - 1, x, y + 1, Color.Blue);
                }

                bounds[i,0] = y1;
                bounds[i,1] = y2;
                b.SetPixels(0, y1, b.Width - 1, y1, Color.Magenta);
                b.SetPixels(0, y2, b.Width - 1, y2, Color.Magenta);
            }

            for (int i = 0; i < bitmaps.Length; i++)
            {
                bitmaps[i] = bitmap.Copy(areas[i]).Rotate(rotation[i]);

                LocklessBitmap b = bitmaps[i];

                b.SetToGrayscale();

                if (bounds[i,0] != 0)
                {
                    b.SetPixels(0, bounds[i, 0] - 1, b.Width - 1, bounds[i, 0] + 1, Color.Red);
                }
                if (bounds[i, 1] != 0)
                {
                    b.SetPixels(0, bounds[i, 1] - 1, b.Width - 1, bounds[i, 1] + 1, Color.Black);
                }
                if (teeth[i].Count() != 0)
                {
                    foreach (KeyValuePair<int, float> t in teeth[i])
                    {
                        int y = (int)Math.Round((double)t.Value);

                        b.SetPixels(t.Key - 1, 0, t.Key + 1, y, Color.Yellow);
                    }
                }
                if (holes[i].Count() != 0)
                {
                    foreach (KeyValuePair<int, float> h in holes[i])
                    {
                        int y = (int)Math.Round((double)h.Value);

                        b.SetPixels(h.Key - 1, y, h.Key + 1, b.Height - 1, Color.Blue);
                    }
                }
                if (perforations[i].Length != 0)
                {
                    for (int x = 1; x < b.Width - 1; x++)
                    {
                        int y = (int)Math.Round((double)perforations[i][x - 1]);

                        b.SetPixels(x, y - 1, x, y + 1, Color.Magenta);
                    }
                }

                bitmap.Copy(bitmaps[i].Rotate(antiRotation[i]), areas[i]);
            }

            //int ri;
            //int to;

            //ri = 100;
            //to = 100;

            //for (int i = 0; i < bitmaps.Length; i++)
            //{
            //    LocklessBitmap b = bitmaps[i];

            //    for (int y = 0; y < b.Height; y++)
            //    {
            //        for (int x = 0; x < b.Width; x++)
            //        {
            //            bitmap.SetPixel(ri + x, to + y, b.GetPixel(x, y));
            //        }
            //    }

            //    to += bitmaps[i].Height;
            //}

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Escher
{
    public enum Side
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3
    }

    public class SizeHelper
    {
        private static float GetPerforation(string perforation)
        {
            if (perforation.EndsWith("¼"))
            {
                perforation = perforation.Replace("¼", ".25");
            }
            else if (perforation.EndsWith("½"))
            {
                perforation = perforation.Replace("½", ".50");
            }
            else if (perforation.EndsWith("¾"))
            {
                perforation = perforation.Replace("¾", ".75");
            }

            return float.Parse(perforation, CultureInfo.InvariantCulture.NumberFormat);
        }

        public static float[] GetPerforations(string perforation)
        {
            float[] perforations = new float[4];

            string[] perfs = perforation.ToLower().Replace(":", "x").Split('x');

            switch (perfs.Length)
            {
                case 4:
                    perforations[(int)Side.Top] = GetPerforation(perfs[0]);
                    perforations[(int)Side.Right] = GetPerforation(perfs[1]);
                    perforations[(int)Side.Bottom] = GetPerforation(perfs[2]);
                    perforations[(int)Side.Left] = GetPerforation(perfs[3]);
                    break;
                case 2:
                    perforations[(int)Side.Top] = GetPerforation(perfs[0]);
                    perforations[(int)Side.Right] = GetPerforation(perfs[1]);
                    perforations[(int)Side.Bottom] = perforations[(int)Side.Top];
                    perforations[(int)Side.Left] = perforations[(int)Side.Right];
                    break;
                case 1:
                    perforations[(int)Side.Top] = GetPerforation(perfs[0]);
                    perforations[(int)Side.Right] = perforations[(int)Side.Top];
                    perforations[(int)Side.Bottom] = perforations[(int)Side.Top];
                    perforations[(int)Side.Left] = perforations[(int)Side.Top];
                    break;
            }

            return perforations;
        }

        public static string Measure(LocklessBitmap bitmap, float[] perf, byte threshold)
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

            int[] edges = new int[4] { 0, 0, 0, 0 };

            float[][] perforations = new float[4][];

            Dictionary<int, float>[] teeth = new Dictionary<int, float>[4];
            Dictionary<int, float>[] holes = new Dictionary<int, float>[4];

            for (int i = 0; i < bitmaps.Length; i++)
            {
                LocklessBitmap b = bitmaps[i];

                teeth[i] = new Dictionary<int, float>();
                holes[i] = new Dictionary<int, float>();

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
                while (y1 < b.Height && b.IsColor(0, y1, b.Width - 1, y1, Color.Black, 0.90F))
                {
                    y1++;
                }

                if (!(y1 > 0 && y1 < b.Height))
                {
                    continue; // Not found the upper bound, so continue with the next side
                }

                y2 = y1;
                while (y2 < b.Height && !b.IsColor(0, y2, b.Width - 1, y2, Color.White, 1))
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

                edges[i] = y1;
            }

            int widthInPixels = 0;
            int heightInPixels = 0;

            int top = edges[0];
            int right = edges[1] != 0 ? bitmap.Width - edges[1] - 1 : 0;
            int bottom = edges[2] != 0 ? bitmap.Height - edges[2] - 1 : 0;
            int left = edges[3];

            if (left != 0 && right != 0)
            {
                widthInPixels = right - left;
            }

            if (top != 0 && bottom != 0)
            {
                heightInPixels = bottom - top;
            }

            for (int i = 0; i < bitmaps.Length; i++)
            {
                bitmaps[i] = bitmap.Copy(areas[i]).Rotate(rotation[i]);

                LocklessBitmap b = bitmaps[i];

                b.SetToGrayscale();

                int thickness = 0;

                if (edges[i] != 0)
                {
                    b.SetPixels(0, edges[i] - thickness, b.Width - 1, edges[i] + thickness, Color.Red);
                }

                if (teeth[i].Count() != 0)
                {
                    foreach (KeyValuePair<int, float> t in teeth[i])
                    {
                        int y = (int)Math.Round((double)t.Value);

                        b.SetPixels(t.Key - thickness, 0, t.Key + thickness, y, Color.Yellow);
                    }
                }
                if (holes[i].Count() != 0)
                {
                    foreach (KeyValuePair<int, float> h in holes[i])
                    {
                        int y = (int)Math.Round((double)h.Value);

                        b.SetPixels(h.Key - thickness, y, h.Key + thickness, b.Height - 1, Color.Blue);
                    }
                }
                if (perforations[i].Length != 0)
                {
                    for (int x = 1; x < b.Width - 1; x++)
                    {
                        int y = (int)Math.Round((double)perforations[i][x - 1]);

                        b.SetPixels(x, y - thickness, x, y + thickness, Color.Magenta);
                    }
                }

                bitmap.Copy(bitmaps[i].Rotate(antiRotation[i]), areas[i]);
            }

            float[] sizes = new float[4] { 0, 0, 0, 0 };

            for (int i = 0; i < bitmaps.Length; i++)
            {
                if (teeth[i].Count >= 5 && holes[i].Count >= 5)
                {
                    int teethFirst = teeth[i].First().Key;
                    int teethLast = teeth[i].Last().Key;

                    int teethPixels = teethLast - teethFirst;
                    float teethSize = 20F / perf[i] * (teeth[i].Count() - 1);

                    int holesFirst = holes[i].First().Key;
                    int holesLast = holes[i].Last().Key;

                    int holesPixels = holesLast - holesFirst;
                    float holesSize = 20F / perf[i] * (holes[i].Count() - 1);

                    float areaSizeInMillimetersBasedOnTeeth = (float)areas[i].Width / teethPixels * teethSize;
                    float areaSizeInMillimetersBasedOnHoles = (float)areas[i].Width / holesPixels * holesSize;

                    int sizeInPixels = (i == 0 || i == 2 ? widthInPixels : heightInPixels);

                    float stampSizeTeeth = (float)sizeInPixels / areas[i].Width * areaSizeInMillimetersBasedOnTeeth;
                    float stampSizeHoles = (float)sizeInPixels / areas[i].Width * areaSizeInMillimetersBasedOnHoles;

                    sizes[i] = (stampSizeTeeth + stampSizeHoles) / 2;
                }
            }

            float horizontal = sizes[0] != 0 && sizes[2] != 0 ? (sizes[0] + sizes[2]) / 2 : (sizes[0] != 0 ? sizes[0] : sizes[2]);
            float vertical = sizes[1] != 0 && sizes[3] != 0 ? (sizes[1] + sizes[3]) / 2 : (sizes[1] != 0 ? sizes[1] : sizes[3]);

            string size = string.Format(
                "{0} × {1} {2} {3} · {4} × {5} · {6}",
                Math.Round(horizontal, 1), Math.Round(vertical, 1), char.ConvertFromUtf32(0x2190),
                Math.Round(sizes[0], 1), Math.Round(sizes[2], 1),
                Math.Round(sizes[1], 1), Math.Round(sizes[3], 1)
            );

            return size;
        }
    }
}

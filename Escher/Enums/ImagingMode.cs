using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public enum ImagingMode
    {
        None,
        Rotate,
        Crop,
        Recolor,
        Brighten,
        Blacken,
        Measure,
        Resize,
        Select,
        Thumbnail
    }

    public static class ImagingModeExtensions
    {
        public static string Text(this ImagingMode imagingMode, string number = "", float scale = 1)
        {
            switch (imagingMode)
            {
                case ImagingMode.None:
                    return string.Format("Stamp Number = {0} · Scale = {1}%", number, (int)(100 * scale));
                case ImagingMode.Rotate:
                    return "Rotate Image";
                case ImagingMode.Crop:
                    return "Crop Image";
                case ImagingMode.Recolor:
                    return "Adjust Image Color";
                case ImagingMode.Brighten:
                    return "Brighten Image";
                case ImagingMode.Blacken:
                    return "Blackening Background";
                case ImagingMode.Measure:
                    return "Measuring Stamp";
                case ImagingMode.Resize:
                    return "Resize Image";
                case ImagingMode.Select:
                    return "Select Vignette";
                case ImagingMode.Thumbnail:
                    return "Create Thumbnail Image";
                default:
                    throw new ArgumentOutOfRangeException("imagingMode");
            }
        }
    }
}

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
        Rotating,
        Cropping,
        Recolor,
        Brightening,
        Blackening,
        Resize,
        Selecting,
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
                case ImagingMode.Rotating:
                    return "Rotate Image";
                case ImagingMode.Cropping:
                    return "Crop Image";
                case ImagingMode.Recolor:
                    return "Adjust Image Color";
                case ImagingMode.Brightening:
                    return "Brighten Image";
                case ImagingMode.Blackening:
                    return "Blackening Background";
                case ImagingMode.Resize:
                    return "Resize Image";
                case ImagingMode.Selecting:
                    return "Select Vignette";
                case ImagingMode.Thumbnail:
                    return "Create Thumbnail Image";
                default:
                    throw new ArgumentOutOfRangeException("imagingMode");
            }
        }
    }
}

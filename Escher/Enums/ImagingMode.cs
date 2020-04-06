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
        Brightening,
        Selecting
    }

    public static class ImagingModeExtensions
    {
        public static string Text(this ImagingMode imagingMode, float scale = 1)
        {
            switch (imagingMode)
            {
                case ImagingMode.None:
                    return string.Format("Scale = {0}%", (int)(100 * scale));
                case ImagingMode.Rotating:
                    return "Rotate Image";
                case ImagingMode.Cropping:
                    return "Crop Image";
                case ImagingMode.Brightening:
                    return "Brighten Image";
                case ImagingMode.Selecting:
                    return "Select Vignette";
                default:
                    throw new ArgumentOutOfRangeException("imagingMode");
            }
        }
    }
}

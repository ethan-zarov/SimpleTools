using UnityEngine;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Provides HSV adjustment for a given color.
        /// </summary>
        public static Color ChangeHSV(this Color c, Vector3 valueChange)
        {
            Vector3 hsv;
            Color.RGBToHSV(c, out hsv.x, out hsv.y, out hsv.z);
            hsv += valueChange;
            return Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        }

        /// <summary>
        /// Provides hue adjustment for a given color.
        /// </summary>
        public static Color ChangeHue(this Color c, float valueChange)
        {
            Vector3 hsv;
            Color.RGBToHSV(c, out hsv.x, out hsv.y, out hsv.z);
            hsv.x += valueChange;
            return Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        }

        /// <summary>
        /// Provides saturation adjustment for a given color.
        /// </summary>
        public static Color ChangeSaturation(this Color c, float valueChange)
        {
            Vector3 hsv;
            Color.RGBToHSV(c, out hsv.x, out hsv.y, out hsv.z);
            hsv.y += valueChange;
            return Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        }

        /// <summary>
        /// Provides value adjustment for a given color.
        /// </summary>
        public static Color ChangeValue(this Color c, float valueChange)
        {
            Vector3 hsv;
            Color.RGBToHSV(c, out hsv.x, out hsv.y, out hsv.z);
            hsv.z += valueChange;
            return Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        }

        /// <summary>
        /// Sets alpha of a color to a certain value.
        /// </summary>
        public static Color SetAlpha(this Color value, float alpha)
        {
            value.a = alpha;
            return value;
        }
    }

}
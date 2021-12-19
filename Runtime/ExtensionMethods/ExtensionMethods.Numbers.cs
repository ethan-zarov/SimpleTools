using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {
        
        /// <summary>
        /// Remaps a float value within one given range to another.
        /// </summary>
        /// <param name="value">Value to remap.</param>
        /// <param name="from1">Lower end of the initial range.</param>
        /// <param name="to1">Upper end of the initial range.</param>
        /// <param name="from2">Lower end of the target range.</param>
        /// <param name="to2">Upper end of the target range.</param>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        
        /// <summary>
        /// Converts a value given a range to it's percent (0-1) along that range.
        /// </summary>
        /// <param name="value">Value to remap.</param>
        /// <param name="from">Lower end of initial range.</param>
        /// <param name="to">Upper end of initial range.</param>
        /// <returns>Returns from 0 to 1 the position "value" is along the range.</returns>
        public static float RemapTo01(this float value, float from, float to)
        {
            return (value - from) / (to - from);
        }
        
        /// <summary>
        /// Converts a value from 0 to 1 to it's relative value along a given range.
        /// </summary>
        /// <param name="value">Value to remap, typically between 0 and 1.</param>
        /// <param name="targetFrom">Lower end of target range.</param>
        /// <param name="targetTo">Upper end of target range.</param>
        public static float RemapFrom01(this float value, float targetFrom, float targetTo)
        {
            return value * (targetTo - targetFrom) + targetFrom;
        }
        

        /// <returns>String with XXX,XXX.X format.</returns>
        public static float RoundToTenths(this float value)
        {
            int dec = Mathf.RoundToInt((value * 10) % 10);
            if (dec > 9) dec = 9;
            return Mathf.FloorToInt(value) + ((float)dec / 10f);
        }


        
        /// <returns>Returns average value of a list of integers.</returns>
        public static float GetAverage(this List<int> value)
        {
            return Mathf.RoundToInt((float)value.Sum() / value.Count);
        }


        /// <returns>Returns average value of a list of floats.</returns>
        public static float GetAverage(this List<float> value)
        {
            return value.Sum() / value.Count;
        }

        
        
        #region Delta-Time Driven Lerp
        public static float Lerp_DeltaTime(this float value, float target, float t)
        {
            return Mathf.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.deltaTime));
        }
        
        public static float Lerp_UnscaledDeltaTime(this float value, float target, float t)
        {
            return Mathf.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.unscaledDeltaTime));
        }
        
        public static float Lerp_FixedDeltaTime(this float value, float target, float t)
        {
            return Mathf.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.fixedDeltaTime));
        }
        
        public static float Lerp_FixedUnscaledDeltaTime(this float value, float target, float t)
        {
            return Mathf.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.fixedUnscaledDeltaTime));
        }
        #endregion
    }

}
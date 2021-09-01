using UnityEngine;
using System.Collections.Generic;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {

        #region Int/Float Functions
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



        /// <returns>String with XXX,XXX.X format.</returns>
        public static float RoundToTenths(this float value)
        {
            int dec = Mathf.RoundToInt((value * 10) % 10);
            if (dec > 9) dec = 9;
            return Mathf.FloorToInt(value) + ((float)dec / 10f);
        }



        /// <returns>Returns average value of a list of integers.</returns>
        public static int GetAverage(this List<int> value)
        {
            int totalValue = 0;
            foreach (var singleInt in value)
            {
                totalValue += singleInt;
            }
            return Mathf.RoundToInt((float)totalValue / value.Count);
        }


        /// <returns>Returns average value of a list of floats.</returns>
        public static float GetAverage(this List<float> value)
        {
            float totalValue = 0;
            foreach (var singleInt in value)
            {
                totalValue += singleInt;
            }
            return totalValue / value.Count;
        }
        #endregion

        #region Numerical Strings
        /// <summary>
        /// Creates a time-value string in XX:XX format.
        /// </summary>
        /// <param name="value">Float value (in seconds) to convert.</param>
        /// <param name="showDecimal">If true, displays time as X.XX when below 10 seconds.</param>
        public static string TimerCountdownDisplay(this float value, bool showDecimal)
        {
            if (value < 0.02f)
            {

                if (showDecimal)
                {
                    return "0.00";
                }
                else
                {
                    return "0:00";
                }
            }
            else if (value < 10 && showDecimal)
            {
                int totalVal = Mathf.RoundToInt(value * 100);
                string ones = Mathf.FloorToInt((float)totalVal / 100f).ToString();
                int decInt = (totalVal % 100);

                string dec = (totalVal % 100).ToString();
                if (decInt < 10)
                {
                    dec = "0" + decInt.ToString();
                }

                return ones + "." + dec;
            }
            else
            {
                int hours = Mathf.FloorToInt(value / 3600);
                int minutes = Mathf.FloorToInt(value / 60) % 60;
                int seconds = Mathf.CeilToInt(value % 60);
                if (seconds == 60)
                {
                    minutes++;
                    seconds = 00;
                }
                string secondsString = seconds.ToString();
                if (seconds < 10)
                {
                    secondsString = "0" + seconds.ToString();
                }
                string hoursString = hours > 0 ? hours.ToString() + ":" : "";

                return hoursString + minutes.ToString() + ":" + secondsString;
            }

        }
        /// <summary>
        /// Creates a time-value string in XX:XX or X.XX sec. format when below 10 seconds.
        /// </summary>
        public static string TimerCountdownDisplayDecimal(this float value)
        {
            if (value < 10)
            {
                return TimerCountdownDisplay(value, true) + " sec.";
            }
            else return TimerCountdownDisplay(value, false);
        }

        /// <summary>
        /// Converts a number to a string with decimal rounded to the tenths place.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>String with XXX,XXX.X format.</returns>
        public static string GetRoundedDecimal(this float value)
        {
            int dec = Mathf.RoundToInt((value * 10) % 10);
            if (dec > 9) dec = 9;
            return WrittenNumber(Mathf.FloorToInt(value)) + "." + dec.ToString();
        }

        /// <summary>
        /// Converts a number to a string with XXX,XXX.X or X.XX formats.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>String with XXX,XXX.X format or X.XX format.</returns>
        public static string GetRoundedDecimalExtended(this float value)
        {
            int dec;
            if (value >= 10)
            {
                dec = Mathf.RoundToInt((value * 10) % 10);
                if (dec > 9) dec = 9;
            }
            else
            {
                dec = Mathf.RoundToInt((value * 100) % 100);
                if (dec > 99) dec = 99;
            }
            return WrittenNumber(Mathf.FloorToInt(value)) + "." + dec.ToString();
        }


        /// <summary>
        /// Converts an int to a string with XXX,XXX,XXX format.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>String in XXX,XXX,XXX format.</returns>
        public static string WrittenNumber(this int value)
        {
            string output = "";

            int preComma = value - (value % 1000);
            int postComma = value % 1000;

            preComma /= 1000;
            preComma = preComma % 1000;

            int millions = Mathf.FloorToInt((float)value / 1000000);
            if (millions > 0)
            {
                output += millions.ToString() + ",";
                if (preComma < 100)
                {
                    if (preComma < 10)
                    {
                        output += "00";
                    }
                    else
                    {
                        output += "0";
                    }
                }
                output += preComma.ToString() + ",";
                if (postComma < 100)
                {
                    if (postComma < 10)
                    {
                        output += "00";
                    }
                    else
                    {
                        output += "0";
                    }
                }
                output += postComma.ToString();

            }
            else if (preComma > 0)
            {
                output += preComma.ToString() + ",";

                if (postComma < 100)
                {
                    if (postComma < 10)
                    {
                        output += "00";
                    }
                    else
                    {
                        output += "0";
                    }
                }
                output += postComma.ToString();
            }
            else
            {
                output += value.ToString();
            }

            return output;
        }


        public static string ToRoman(int number)
        {
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            return "?";
        }

        /// <summary>
        /// Generates a "+X THING(S)" text.
        /// </summary>
        /// <param name="pluralized">When there are multiple THINGs, does it have an S?</param>
        /// <returns></returns>
        public static string GenerateBonusText(this int amount, string type, bool pluralized)
        {
            if (amount > 0)
            {
                return $"+{amount} {type}{(amount > 1 && pluralized ? 's' : ' ')}\n";
            }
            else return "";
        }

        /// <summary>
        /// Generates a "+X THING(IES)" text.
        /// </summary>
        /// <returns></returns>
        public static string GenerateBonusText(this int amount, string type, string pluralType)
        {
            if (amount > 0)
            {
                return $"+{amount} {(amount > 1 ? pluralType : type)}\n";
            }
            else return "";
        }
        #endregion
    }

}
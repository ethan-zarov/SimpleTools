using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {

        /// <summary>
        /// Alphabetize the characters in the string.
        /// </summary>
        /// 
        public static string Alphabetize(this string s)
        {
            var a = s.ToCharArray();
            Array.Sort(a);
            return new string(a);
        }
        
        
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
                return showDecimal ? "0.00" : "0:00";
            }

            if (value < 10 && showDecimal)
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

        public static string ToTuple(int number)
        {
            if (number < 10)
            {
                switch (number)
                {
                    case 0: return "Nulltuple";
                    case 1: return "Single";
                    case 2: return "Double";
                    case 3: return "Triple";
                    case 4: return "Quadruple";
                    case 5: return "Quintuple";
                    case 6: return "Hextuple";
                    case 7: return "Septuple";
                    case 8: return "Octuple";
                    case 9: return "Nonuple";
                    default: return "Nulltuple";
                }
            }
            else
            {
                int ones = number % 10;
                int tens = (number / 10) % 10;
                int hundreds = number / 100;

                string output = GetOnesPrefix(ones) + GetTensPrefix(tens);
                
                if (hundreds > 0)
                {
                    if (hundreds > 1) output = output + GetTensPrefix(hundreds);
                    output += "cen";
                }

                if (output[output.Length - 1].Equals('c')) return output + "uple";
                else return output + "tuple";
            }
        }

        public static string GetOnesPrefix(int oneVal)
        {
            switch (oneVal)
            {
                case 0: return "";
                case 1: return "un";
                case 2: return "duo";
                case 3: return "tre";
                case 4: return "quattuor";
                case 5: return "quin";
                case 6: return "hex";
                case 7: return "sept";
                case 8: return "octo";
                case 9: return "novem";
                default: return "";
            }
        }

        public static string GetTensPrefix(int tenVal)
        {
            switch (tenVal)
            {
                case 0: return "";
                case 1: return "dec";
                case 2: return "vigin";
                case 3: return "trigin";
                case 4: return "quadragin";
                case 5: return "quinquagin";
                case 6: return "hexagin";
                case 7: return "septagin";
                case 8: return "octogin";
                case 9: return "nongen";
                default: return "";
            }
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
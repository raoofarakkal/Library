using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library2.Cs
{
    public class Common
    {

        public static long MyCLng(string pStr)
        {
            long mRet = 0;
            bool success = long.TryParse(pStr, out mRet);
            return mRet;
        }

        public static int MyCInt(string pStr)
        {
            int mRet = 0;
            bool success = int.TryParse(pStr, out mRet);
            return mRet;
        }

        public static bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        public static bool IsDate(string pDate)
        {
            DateTime temp;
            return DateTime.TryParse(pDate, out temp);
        }

        public static DateTime MyCDate(string pDate)
        {
            if (IsDate(pDate))
            {
                return Convert.ToDateTime(pDate);
            }
            else
            {
                return DateTime.MinValue ;
            }

        }

        #region String Functions

        public static string Left(string text, int length)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (length <= 0 || text.Length == 0)
                    return string.Empty;
                else if (text.Length <= length)
                    return text;
                else
                    return text.Substring(0, length);
            }
            else
                return string.Empty;
        }

        public static string Right(string text, int length)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (length <= 0 || text.Length == 0)
                    return string.Empty;
                else if (text.Length <= length)
                    return text;
                else
                    return text.Substring(text.Length - length);
            }
            else
                return string.Empty;
        }

        public static string Mid(string text, int startIndex, int length)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (length <= 0 || text.Length == 0)
                    return string.Empty;
                else if (startIndex > text.Length || startIndex < 0)
                    return text;
                else if (startIndex + length > text.Length)
                    length = text.Length - startIndex;

                return text.Substring(startIndex, length);
            }
            else
                return string.Empty;
        }

        public static string Mid(string text, int startIndex)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                return Mid(text, startIndex, text.Length);
            }
            else
                return string.Empty;
        }

        public static string[] SplitByString(string pStr2Split, string pStrSplitBy)
        {
            return pStr2Split.Split(new string[] { pStrSplitBy }, StringSplitOptions.None);
        }

        #endregion

        public static int generateRandomNumber(int pMinVal, int pMaxVal)
        {
            return (new Random()).Next(pMinVal, pMaxVal);
        }

    }
}

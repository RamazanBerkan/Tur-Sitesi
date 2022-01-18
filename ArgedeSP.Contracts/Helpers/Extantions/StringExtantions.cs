using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ArgedeSP.Contracts.Helpers.Extantions
{
    public static class StringExtantions
    {
        public static string Kisalt(this string icerik, int karakterSayisi)
        {
            if (icerik.Length > karakterSayisi)
            {
                return icerik.Substring(0, karakterSayisi) + "...";
            }
            else
            {
                return icerik;
            }
        }

        public static string ClearTags(this string metin)
        {
            metin = WebUtility.HtmlDecode(metin);
            if (metin == null || metin == string.Empty)
                return string.Empty;
            metin = metin.Replace("\"", "-").Replace("'", "-");
            return System.Text.RegularExpressions.Regex.Replace(metin, "<[^>]*>", string.Empty);
        }

        public static int ToInteger(this string s, int iMin, int iMax)
        {
            int a = iMin;
            if (int.TryParse(s, out a))
            {
                if (a > iMax) a = iMax;
            }
            return a;
        }

        public static string UppercaseFirst(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}

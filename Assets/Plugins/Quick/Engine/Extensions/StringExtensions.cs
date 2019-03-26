// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace QuickEngine.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes the whitespaces characters. No Regex!
        /// </summary>
        public static string RemoveWhitespaces(this string str)
        {
            return str.Replace(" ", "");
        }

        /// <summary>
        /// Removes all types of whitespace characters (space, tabs, line breaks...). Uses Regex!
        /// </summary>
        public static string RemoveAllTypesOfWhitespaces(this string str)
        {
            return Regex.Replace(str, @"\s", "");
        }


        /// <summary>
        /// Returns true if the entire string is numeric, false otherwise.
        /// </summary>
        public static bool IsNumeric(this string str)
        {
            return (!string.IsNullOrEmpty(str)) && (new Regex(@"^-?[0-9]*\.?[0-9]+$").IsMatch(str.Trim()));
        }
        /// <summary>
        /// Returns true if the string contains a numeric sequence, false otherwise.
        /// </summary>
        public static bool ContainsNumeric(this string str)
        {
            return (!string.IsNullOrEmpty(str)) && (new Regex(@"[0-9]+").IsMatch(str));
        }
        /// <summary>
        /// Extension for string.IsNullOrEmpty(). Returns true if the string is null or empty, false otherwise.
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

#if !(UNITY_5_6 && UNITY_WINRT)
        /// <summary>
        /// Converts the specified string to title case (except for words that are entirely in uppercase, which are considered to be acronyms).
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string str)
        {
            if (str.IsNullOrEmpty()) { return str; }

            // get globalization info
            CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            // convert to title case
            return textInfo.ToTitleCase(str);
        }
#endif

        /// <summary>
        ///  A simple extension method based on Binary Worrier's code which will handle acronyms properly, and is repeatable (won't mangle already spaced words). 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UnPascalCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                var currentUpper = char.IsUpper(text[i]);
                var prevUpper = char.IsUpper(text[i - 1]);
                var nextUpper = (text.Length > i + 1) ? char.IsUpper(text[i + 1]) || char.IsWhiteSpace(text[i + 1]) : prevUpper;
                var spaceExists = char.IsWhiteSpace(text[i - 1]);
                if (currentUpper && !spaceExists && (!nextUpper || !prevUpper))
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static string RemoveDiacritics(this string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]))
                {
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.EnclosingMark:
                        break;
                    default:
                        sb.Append(stFormD[ich]);
                        break;
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static char GetAccent(this string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc == UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            if (sb.Length > 0)
            {
                return sb.ToString().Normalize(NormalizationForm.FormC)[0];
            }
            else
            {
                return default(char);
            }
        }

        public static bool IsDiacriticsed(this string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc == UnicodeCategory.NonSpacingMark)
                {
                    return true;
                }
            }

            return false;
        }

        public static string FixNewLine(this string s)
        {
            return s.Replace("\r", "\n").Replace(((char)3).ToString(), "\n");
        }

        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(this string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        public static string StripTagsCharArray(this string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string[] Split(this string s, string separator, StringSplitOptions splitOptions = StringSplitOptions.None)
        {
            return s.Split(new string[] { separator }, splitOptions);
        }

        public static int OccurenceCount(this string str, string val)
        {
            int occurrences = 0;
            int startingIndex = 0;

            while ((startingIndex = str.IndexOf(val, startingIndex)) >= 0)
            {
                ++occurrences;
                ++startingIndex;
            }

            return occurrences;
        }

        public static int NthIndexOf(this string target, string value, int n)
        {

            string[] result = target.Split(value);
            n--;
            if (n >= 0 && n < result.Length)
            {
                int index = 0;
                for (int i = 0; i <= n; i++)
                {
                    index += result[i].Length + value.Length;
                }
                return index - value.Length;
            }
            else
            {
                return -1;
            }
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp = StringComparison.Ordinal)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool EndsWith(this string a, string b)
        {
            int ap = a.Length - 1;
            int bp = b.Length - 1;

            while (ap >= 0 && bp >= 0 && a[ap] == b[bp])
            {
                ap--;
                bp--;
            }
            return (bp < 0 && a.Length >= b.Length) ||

                    (ap < 0 && b.Length >= a.Length);
        }

        public static bool StartsWith(this string a, string b)
        {
            int aLen = a.Length;
            int bLen = b.Length;
            int ap = 0; int bp = 0;

            while (ap < aLen && bp < bLen && a[ap] == b[bp])
            {
                ap++;
                bp++;
            }

            return (bp == bLen && aLen >= bLen) ||

                    (ap == aLen && bLen >= aLen);
        }

        public static bool RegexMatch(this string a, string b)
        {
            var myRegExp = new Regex(a);
            return myRegExp.Match(b).Success;
        }
    }
}
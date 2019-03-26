// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Text.RegularExpressions;

namespace QuickEngine.Utils
{
    public static class QIPValidator
    {
        /// <summary>
        /// Returns true if the string parameter is a valid IPv4 addrees, false otherwise.
        /// </summary>
        /// <param name="str">IP string to be checked.</param>
        /// <returns></returns>
        public static bool IsValidIPAddress(string str)
        {
            return Regex.IsMatch(str, @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
        }
    }
}
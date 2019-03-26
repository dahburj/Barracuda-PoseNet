// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QuickEngine.Utils
{
    public static class QEmailValidator
    {
        /// <summary>
        /// True if the address is invalid
        /// </summary>
        private static bool isInvalid;

        /// <summary>
        /// True if email is valid, false otherwise.
        /// </summary>
        /// <param name="emailString">Email address.</param>
        /// <returns></returns>
        public static bool IsValidEmail(string emailString)
        {
            isInvalid = false;
            if(string.IsNullOrEmpty(emailString)) { return false; }

            // Use IdnMapping class to convert Unicode domain names. 
            emailString = Regex.Replace(emailString, @"(@)(.+)$", DomainMapper, RegexOptions.None);

            if(isInvalid) { return false; }

            // Return true if strIn is in valid e-mail format. 
            return Regex.IsMatch(emailString,
                                 @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                 @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                                 RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// convert Unicode domain names
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch(Exception)
            {
                isInvalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

    }
}
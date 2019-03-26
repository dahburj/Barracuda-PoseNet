// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;

namespace QuickEngine.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns true if a date is in a period between two others.
        /// </summary>
        /// <param name="date">Date checked.</param>
        /// <param name="from">Start of period.</param>
        /// <param name="to">End of period.</param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime date, DateTime from, DateTime to) { return ((from <= date) && (to >= date)); }
        /// <summary>
        /// Returns the date as midnight
        /// </summary>
        /// <param name="date">Date.</param>
        public static DateTime Midnight(this DateTime date) { return new DateTime(date.Year, date.Month, date.Day); }
        /// <summary>
        /// Returns the first day of the month.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns></returns>
        public static DateTime FirstOfMonth(this DateTime date) { return new DateTime(date.Year, date.Month, 1); }
        /// <summary>
        /// Returns the last day of the month
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime date) { return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1); }
        /// <summary>
        /// Returns yesterday's date (keeps the time)
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns></returns>
        public static DateTime Yesterday(this DateTime date) { return date.AddDays(-1); }
        /// <summary>
        /// Returns yesterday's date at midnight
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns></returns>
        public static DateTime YesterdayMidnight(this DateTime date) { return date.Yesterday().Midnight(); }
        /// <summary>
        /// Returns Tomorrow's date (keeps the time)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime Tomorrow(this DateTime date) { return date.AddDays(1); }
        /// <summary>
        /// Returns Tomorrow's date at midnight
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime TomorrowMidnight(this DateTime date) { return date.Tomorrow().Midnight(); }
        /// <summary>
        /// Returns true if both dates are on the same day.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="compareDate">Date to be compared.</param>
        /// <returns></returns>
        public static bool IsSameDay(this DateTime date, DateTime compareDate) { return date.Midnight().Equals(compareDate.Midnight()); }
        /// <summary>
        /// Returns true if the date is greater than <paramref name="compareDate"/>
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="compareDate">Date to compare.</param>
        /// <returns></returns>
        public static bool IsLaterDate(this DateTime date, DateTime compareDate) { return date > compareDate; }
        /// <summary>
        /// Returns true if the date is less than <paramref name="compareDate"/>
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="compareDate">Date to compare.</param>
        /// <returns></returns>
        public static bool IsOlderDate(this DateTime date, DateTime compareDate) { return date < compareDate; }
        /// <summary>
        /// Checks whether the given day is Today.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>True if the given day is Today, false otherwise.</returns>
        public static bool IsToday(this DateTime date) { return date.Date == DateTime.Now.Date; }
        /// <summary>
        /// Checks whether the given day is Tomorrow.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>True if the given day is Tomorrow, false otherwise.</returns>
        public static bool IsTomorrow(this DateTime date) { return date.Date == DateTime.Now.Date.AddDays(1); }
        /// <summary>
        /// Checks whether the given day is Yesterday.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>True if the given day is yesterday, false otherwise.</returns>
        public static bool IsYesterday(this DateTime date) { return date.Date == DateTime.Now.Date.AddDays(-1); }

        #region Date To String
        #region DD/MM/YY methods
        /// <summary>
        /// Formats the given DateTime to "dd/MM/yy".
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>The string representation according to the format.</returns>
        public static string ToDdMmYySlash(this DateTime date) { return date.ToString("dd/MM/yy"); }
        /// <summary>
        /// Formats the given DateTime to "dd.MM.yy".
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>The string representation according to the format.</returns>
        public static string ToDdMmYyDot(this DateTime date) { return date.ToString("dd.MM.yy"); }
        /// <summary>
        /// Formats the given DateTime to "dd-MM-yy".
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>The string representation according to the format.</returns>
        public static string ToDdMmYyHyphen(this DateTime date) { return date.ToString("dd-MM-yy"); }
        /// <summary>
        /// Formats the given DateTime to "dd*MM*yy" by applying the given separator.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="separator">The given separator.</param>
        /// <returns>The string representation according to the format.</returns>
        public static string ToDdMmYyWithSep(this DateTime date, string separator) { return date.ToString(string.Format("dd{0}MM{0}yy", separator)); }
        #endregion

        #region DD/MM/YYYY methods
        /// <summary>
        /// Formats the given DateTime to "dd/MM/yyyy".
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>The string representation according to the format.</returns>
        public static string ToDdMmYyyySlash(this DateTime date) { return date.ToString("dd/MM/yyyy"); }
        /// <summary>
        /// Formats the given DateTime to "dd.MM.yyyy".
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>The string representation according to the format.</returns>
        public static string ToDdMmYyyyDot(this DateTime date) { return date.ToString("dd.MM.yyyy"); }
        /// <summary>
        /// Formats the given DateTime to "dd-MM-yyyy".
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>The string representation according to the format.</returns>
        public static string ToDdMmYyyyHyphen(this DateTime date) { return date.ToString("dd-MM-yyyy"); }
        /// <summary>
        /// Formats the given DateTime to "dd*MM*yyyy" by applying the given separator.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="separator">The prefered separator.</param>
        /// <returns>The string representation according to the format.</returns>
        public static string ToDdMmYyyyWithSep(this DateTime date, string separator) { return date.ToString(string.Format("dd{0}MM{0}yyyy", separator)); }
        #endregion
        #endregion
    }
}
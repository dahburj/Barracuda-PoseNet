// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace QuickEngine.Extensions
{
    public static class IListExtensions
    {
        /// <summary>
        /// Returns true if this list is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IList<T> items)
        {
            return items == null || !items.Any();
        }

        /// <summary>
        /// Returns true if this list is NOT null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool NotNullOrEmpty<T>(this IList<T> items)
        {
            return items != null && items.Any();
        }

#if !(UNITY_5_6 && UNITY_WINRT)
        /// <summary>
        /// Shuffle the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do
                {
                    provider.GetBytes(box);
                }
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
#endif

    }
}

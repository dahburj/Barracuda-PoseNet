// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEngine.Extensions
{
    public static class ArrayAndListExtensions
    {
        /// <summary>
        /// Returns true if the array is null or empty.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return ((array == null) || (array.Length == 0));
        }
        /// <summary>
        /// Returns true if the list is null or empty.
        /// </summary>
        /// <typeparam name="T">List Type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return ((list == null) || (list.Count == 0));
        }
        /// <summary>
        /// Returns true if the dictionary is null or empty.
        /// </summary>
        /// <typeparam name="TKey">Key Type.</typeparam>
        /// <typeparam name="TValue">Value Type.</typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return ((dict == null) || (dict.Count == 0));
        }
        /// <summary>
        /// Returns a random element of the array. Does NOT check if the array is empty or null!
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
        /// <summary>
        /// Returns a random element of the list. Does NOT check if the list is empty or null!
        /// </summary>
        /// <typeparam name="T">List Type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Radom element from the list</returns>
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        public static void ShuffleArray<T>(this T[] array)
        {
            T temp;
            for(int i = array.Length - 1; i > 0; i--)
            {
                // Get a random position lower than i
                int randomPosition = Random.Range(0,i);
                // Swap values
                temp = array[i];
                array[i] = array[randomPosition];
                array[randomPosition] = temp;
            }
        }
        /// <summary>
        /// Shuffle the list.
        /// </summary>
        /// <typeparam name="T">List type.</typeparam>
        /// <param name="list">The list.</param>
        public static void ShuffleList<T>(this List<T> list)
        {
            T temp;
            for(int i = list.Count - 1; i > 0; i--)
            {
                // Get a random position lower than i
                int randomPosition = Random.Range(0,i);
                // Swap values
                temp = list[i];
                list[i] = list[randomPosition];
                list[randomPosition] = temp;
            }
        }
        /// <summary>
        /// Joins all the elements of the array into a string separated by the given separator string.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="separator">String separator.</param>
        /// <returns></returns>
        public static string ToString<T>(this T[] array, string separator)
        {
            if(array.IsNullOrEmpty())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < array.Length - 1; i++)
            {
                builder.Append(array[i].ToString());
                builder.Append(separator);
            }
            builder.Append(array[array.Length - 1].ToString());
            return builder.ToString();
        }
        /// <summary>
        /// Joins all the elements of the list into a string separated by the given separator string.
        /// </summary>
        /// <typeparam name="T">List Type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="separator">String separator.</param>
        /// <returns></returns>
        public static string ToString<T>(this List<T> list, string separator)
        {
            if(list.IsNullOrEmpty())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < list.Count - 1; i++)
            {
                builder.Append(list[i].ToString());
                builder.Append(separator);
            }
            builder.Append(list[list.Count - 1].ToString());
            return builder.ToString();
        }
		
		/// <summary>
        /// Joins all the elements of the array into a string separated by the given separator character.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="separator">String separator.</param>
        /// <returns></returns>
        public static string ToString<T>(this T[] array, char separator)
        {
            if(array.IsNullOrEmpty())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < array.Length - 1; i++)
            {
                builder.Append(array[i].ToString());
                builder.Append(separator);
            }
            builder.Append(array[array.Length - 1].ToString());
            return builder.ToString();
        }

        /// <summary>
        /// Joins all the elements of the list into a string separated by the given separator character.
        /// </summary>
        /// <typeparam name="T">List Type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="separator">String separator.</param>
        /// <returns></returns>
        public static string ToString<T>(this List<T> list, char separator)
        {
            if(list.IsNullOrEmpty())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < list.Count - 1; i++)
            {
                builder.Append(list[i].ToString());
                builder.Append(separator);
            }
            builder.Append(list[list.Count - 1].ToString());
            return builder.ToString();
        }
    }
}
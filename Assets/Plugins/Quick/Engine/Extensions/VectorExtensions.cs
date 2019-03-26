// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using UnityEngine;

namespace QuickEngine.Extensions
{
    public static class VectorExtensions
    {
        #region Vector2
        /// <summary>
        /// Sets a new X value to the vector and returns it.
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <param name="x">New X value.</param>
        /// <returns></returns>
        public static Vector2 SetX(this Vector2 v2, float x) { return new Vector2(x, v2.y); }
        /// <summary>
        /// Sets a new Y value to the vector and returns it.
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <param name="y">New Y value.</param>
        /// <returns></returns>
        public static Vector2 SetY(this Vector2 v2, float y) { return new Vector2(v2.x, y); }
        /// <summary>
        /// Adds X to vector.x, adds Y value to vector.y and returns the new Vector2.
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <param name="x">Value to be added to X.</param>
        /// <param name="y">Value to be added to Y.</param>
        /// <returns></returns>
        public static Vector2 AddXY(this Vector2 v2, float x, float y) { return new Vector2(v2.x + x, v2.y + y); }
        /// <summary>
        /// Returns the projection of this vector onto the given base.
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <param name="base">Base Vector2.</param>
        /// <returns></returns>
        public static Vector2 Proj(this Vector2 v2, Vector2 @base)
        {
            var direction = @base.normalized;
            var magnitude = Vector2.Dot(v2, direction);
            return direction * magnitude;
        }
        /// <summary>
        /// Returns the rejection of this vector onto the given base. The sum of a vector's projection and rejection on a base is equal to the original vector.
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <param name="base">Base Vector2.</param>
        /// <returns></returns>
        public static Vector2 Rej(this Vector2 v2, Vector2 @base)
        {
            return v2 - v2.Proj(@base);
        }
        /// <summary>
        /// Returns a Vector2 with rounded values to the set number of decimals. 
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <param name="decimals">Number of decimals.</param>
        /// <returns></returns>
        public static Vector2 Round(this Vector2 v2, int decimals = 1) { return new Vector2((float)Math.Round(v2.x, decimals), (float)Math.Round(v2.y, decimals)); }
        /// <summary>
        /// Returns a Vector3 to a string in X, Y format, rounded up to the set number of decimals.
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <param name="decimals">Number of decimals.</param>
        /// <returns></returns>
        public static string ToString(this Vector2 v2, int decimals = 0) { return "(" + Math.Round(v2.x, decimals) + ", " + Math.Round(v2.y, decimals) + ")"; }
        /// <summary>
        /// Returns a Vector3 to a string in X, Y format.
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <returns></returns>
        public static string ToString(this Vector2 v2) { return "(" + v2.x + ", " + v2.y + ")"; }
        #endregion

        #region Vector3
        /// <summary>
        /// Sets a new X value to the vector and returns it.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <param name="x">New X value.</param>
        /// <returns></returns>
        public static Vector3 SetX(this Vector3 v3, float x) { return new Vector3(x, v3.y, v3.z); }
        /// <summary>
        /// Sets a new Y value to the vector and returns it.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <param name="y">New Y value.</param>
        /// <returns></returns>
        public static Vector3 SetY(this Vector3 v3, float y) { return new Vector3(v3.x, y, v3.z); }
        /// <summary>
        /// Sets a new Z value to the vector and returns it.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <param name="z">New Z value.</param>
        /// <returns></returns>
        public static Vector3 SetZ(this Vector3 v3, float z) { return new Vector3(v3.x, v3.y, z); }
        /// <summary>
        /// Adds X to vector.x, adds Y value to vector.y, adds Z to vector.z and returns the new Vector3.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <param name="x">Value to be added to X.</param>
        /// <param name="y">Value to be added to Y.</param>
        /// <param name="z">Value to be added to Z.</param>
        /// <returns></returns>
        public static Vector3 AddXYZ(this Vector3 v3, float x, float y, float z) { return new Vector3(v3.x + x, v3.y + y, v3.z + z); }
        /// <summary>
        /// Returns the projection of this vector onto the given base.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <param name="base">Base Vector3.</param>
        /// <returns></returns>
        public static Vector3 Proj(this Vector3 v3, Vector3 @base)
        {
            var direction = @base.normalized;
            var magnitude = Vector3.Dot(v3, direction);
            return direction * magnitude;
        }
        /// <summary>
        /// Returns the rejection of this vector onto the given base. The sum of a vector's projection and rejection on a base is equal to the original vector.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <param name="base">Base Vector3.</param>
        /// <returns></returns>
        public static Vector3 Rej(this Vector3 v3, Vector3 @base)
        {
            return v3 - v3.Proj(@base);
        }
        /// <summary>
        /// Returns a Vector3 with rounded values to the set number of decimals. 
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <param name="decimals">Number of decimals.</param>
        /// <returns></returns>
        public static Vector3 Round(this Vector3 v3, int decimals = 1) { return new Vector3((float)Math.Round(v3.x, decimals), (float)Math.Round(v3.y, decimals), (float)Math.Round(v3.z, decimals)); }
        /// <summary>
        /// Returns a Vector3 to a string in X, Y format, rounded up to the set number of decimals.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <param name="decimals">Number of decimals.</param>
        /// <returns></returns>
        public static string ToString(this Vector3 v3, int decimals = 0) { return "(" + Math.Round(v3.x, decimals) + ", " + Math.Round(v3.y, decimals) + ", " + Math.Round(v3.z, decimals) + ")"; }
        /// <summary>
        /// Returns a Vector3 to a string in X, Y format.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <returns></returns>
        public static string ToStringXY(this Vector3 v3) { return "(" + v3.x + ", " + v3.y + ")"; }
        #endregion

        #region Rect
        public static Vector2 TopLeft(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMin);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale;
            result.xMax *= scale;
            result.yMin *= scale;
            result.yMax *= scale;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }

        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale.x;
            result.xMax *= scale.x;
            result.yMin *= scale.y;
            result.yMax *= scale.y;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }
        #endregion
    }
}

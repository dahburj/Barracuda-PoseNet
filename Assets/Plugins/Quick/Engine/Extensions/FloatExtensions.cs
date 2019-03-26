// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using UnityEngine;

namespace QuickEngine.Extensions
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Returns a float rounded up to the set number of decimals.
        /// </summary>
        public static float Round(this float f, int decimals = 1) { return (float)Math.Round(f, decimals); }
        /// <summary>
        /// Returns the float rounded to the nearest integer.
        /// </summary>
        public static float Round(this float f) { return Mathf.Round(f); }
    }
}

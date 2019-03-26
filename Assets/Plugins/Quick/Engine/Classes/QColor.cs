// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Extensions;
using System;
using UnityEngine;

namespace QuickEngine
{
    [Serializable]
    public class QColor
    {
        private Color color;
        private Color colorDark;
        private Color colorLight;

        public Color Color { get { return color; } }
        public Color ColorDark { get { return colorDark; } }
        public Color ColorLight { get { return colorLight; } }

        public QColor(Color color)
        {
            SetColor(color);
        }

        public QColor(Color color, float alpha)
        {
            SetColor(new Color(color.r, color.g, color.b, alpha));
        }

        public QColor(float r, float g, float b, bool from256 = true)
        {
            SetColor(from256 ? ColorExtensions.ColorFrom256(r, g, b) : new Color(r, g, b));
        }

        public QColor(float r, float g, float b, float a, bool from256 = true)
        {
            SetColor(from256 ? ColorExtensions.ColorFrom256(r, g, b, a) : new Color(r, g, b, a));
        }

        /// <summary>
        /// Updates the color.
        /// </summary>
        public void SetColor(Color color)
        {
            this.color = color;
            colorLight = color.Lighter();
            colorDark = color.Darker();
        }

        /// <summary>
        /// Returns the brightness of the color, defined as the average off the three color channels.
        /// </summary>
        public float ColorBrightness { get { return Color.Brightness(); } }
        /// <summary>
        /// Returns the brightness of the color, defined as the average off the three color channels.
        /// </summary>
        public float ColorDarkBrightness { get { return ColorDark.Brightness(); } }
        /// <summary>
        /// Returns the brightness of the color, defined as the average off the three color channels.
        /// </summary>
        public float ColorLightBrightness { get { return ColorLight.Brightness(); } }

        /// <summary>
		/// Returns an opaque version of the set color.
		/// </summary>
        public Color ColorOpaque { get { return Color.Opaque(); } }
        /// <summary>
        /// Returns an opaque version of the set color.
        /// </summary>
        public Color ColorDarkOpaque { get { return ColorDark.Opaque(); } }
        /// <summary>
        /// Returns an opaque version of the set color.
        /// </summary>
        public Color ColorLightOpaque { get { return ColorLight.Opaque(); } }

        /// <summary>
        /// Returns a new color that is this color inverted.
        /// </summary>
        public Color ColorInvert { get { return Color.Invert(); } }
        /// <summary>
        /// Returns a new color that is this color inverted.
        /// </summary>
        public Color ColorDarkInvert { get { return ColorDark.Invert(); } }
        /// <summary>
        /// Returns a new color that is this color inverted.
        /// </summary>
        public Color ColorLightInvert { get { return ColorLight.Invert(); } }

        /// <summary>
        /// Returns a new color with the RGB values scaled so that the color has the given brightness.
        /// <para>If the color is too dark, a grey is returned with the right brighness. The alpha is left uncanged.</para> 
        /// </summary>
        public Color ColorWithBrightness(float brightness) { return Color.WithBrightness(brightness); }
        /// <summary>
        /// Returns a new color with the RGB values scaled so that the color has the given brightness.
        /// <para>If the color is too dark, a grey is returned with the right brighness. The alpha is left uncanged.</para> 
        /// </summary>
        public Color ColorDarkWithBrightness(float brightness) { return ColorDark.WithBrightness(brightness); }
        /// <summary>
        /// Returns a new color with the RGB values scaled so that the color has the given brightness.
        /// <para>If the color is too dark, a grey is returned with the right brighness. The alpha is left uncanged.</para> 
        /// </summary>
        public Color ColorLightWithBrightness(float brightness) { return ColorLight.WithBrightness(brightness); }

        /// <summary>
        /// Returns a new color with the given alpha.
        /// </summary>
        public Color ColorWithAlpha(float alpha) { return Color.WithAlpha(alpha); }
        /// <summary>
        /// Returns a new color with the given alpha.
        /// </summary>
        public Color ColorDarkWithAlpha(float alpha) { return ColorDark.WithAlpha(alpha); }
        /// <summary>
        /// Returns a new color with the given alpha.
        /// </summary>
        public Color ColorLightWithAlpha(float alpha) { return ColorLight.WithAlpha(alpha); }
    }
}

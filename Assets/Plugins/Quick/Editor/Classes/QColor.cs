// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Extensions;
using System;
using UnityEditor;
using UnityEngine;

namespace QuickEditor
{
    [Serializable]
    public class QColor
    {
        private Color _dark;
        private Color _light;

        public Color Dark { get { return _dark; } set { _dark = value; } }
        public Color Light { get { return _light; } set { _light = value; } }

        public Color Color { get { return EditorGUIUtility.isProSkin ? Dark : Light; } }

        public QColor(Color color)
        {
            _dark = color;
            _light = color;
        }

        public QColor(Color color, float alpha)
        {
            _dark = new Color(color.r, color.g, color.b, alpha);
            _light = new Color(color.r, color.g, color.b, alpha);
        }

        public QColor(QColor qColor)
        {
            _dark = qColor.Dark;
            _light = qColor.Light;
        }

        public QColor(QColor qColor, float alpha, bool from256 = true)
        {
            _dark = new Color(qColor.Dark.r, qColor.Dark.g, qColor.Dark.b, from256 ? alpha / 256f : alpha);
            _light = new Color(qColor.Light.r, qColor.Light.g, qColor.Light.b, from256 ? alpha / 256f : alpha);
        }

        public QColor(Color dark, Color light)
        {
            _dark = dark;
            _light = light;
        }

        public QColor(float r, float g, float b, bool from256 = true)
        {
            _dark = from256 ? ColorExtensions.ColorFrom256(r, g, b) : new Color(r, g, b);
            _light = from256 ? ColorExtensions.ColorFrom256(r, g, b) : new Color(r, g, b);
        }

        public QColor(float r, float g, float b, float a, bool from256 = true)
        {
            _dark = from256 ? ColorExtensions.ColorFrom256(r, g, b, a) : new Color(r, g, b, a);
            _light = from256 ? ColorExtensions.ColorFrom256(r, g, b, a) : new Color(r, g, b, a);
        }

        public QColor(float rDark, float gDark, float bDark, float rLight, float gLight, float bLight, bool from256 = true)
        {
            _dark = from256 ? ColorExtensions.ColorFrom256(rDark, gDark, bDark) : new Color(rDark, gDark, bDark);
            _light = from256 ? ColorExtensions.ColorFrom256(rLight, gLight, bLight) : new Color(rLight, gLight, bLight);
        }

        public QColor(float rDark, float gDark, float bDark, float aDark, float rLight, float gLight, float bLight, float aLight, bool from256 = true)
        {
            _dark = from256 ? ColorExtensions.ColorFrom256(rDark, gDark, bDark, aDark) : new Color(rDark, gDark, bDark, aDark);
            _light = from256 ? ColorExtensions.ColorFrom256(rLight, gLight, bLight, aLight) : new Color(rLight, gLight, bLight, aLight);
        }
    }
}

// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using UnityEngine;

namespace QuickEditor
{
    public enum BorderType
    {
        /// <summary>
        /// No border.
        /// </summary>
        None,
        /// <summary>
        /// Normal border, with the same color all around.
        /// </summary>
        Normal,
        /// <summary>
        /// Two tone border. Lighter on top left and darker on bottom right. Creates a 3D effect.
        /// </summary>
        Shaded
    }

    [Serializable]
    public class QGeneratedTexture
    {
        private QColor _contentColor;
        private QColor _borderMainColor;
        private QColor _borderSecondaryColor;
        private int _borderWidth;
        private Texture2D _texture2D;

        public QColor ContentColor { get { return _contentColor; } set { _contentColor = value; } }
        public QColor BorderMainColor { get { return _borderMainColor; } set { _borderMainColor = value; } }
        public QColor BorderSecondaryColor { get { return _borderSecondaryColor; } set { _borderSecondaryColor = value; } }
        public int BorderWidth { get { return _borderWidth; } set { _borderWidth = value; } }
        public BorderType BorderType = BorderType.None;

        public Texture2D Texture2D { get { if (_texture2D == null) { _texture2D = GenerateTexture(ContentColor, BorderMainColor, BorderSecondaryColor, BorderType, BorderWidth); } return _texture2D; } }
        public Texture Texture { get { return Texture2D; } }

        public QGeneratedTexture(QColor content)
        {
            ContentColor = content;
            BorderMainColor = content;
            BorderSecondaryColor = content;
            BorderType = BorderType.None;
            BorderWidth = 1;
        }

        public QGeneratedTexture(QColor content, QColor border, BorderType borderType = BorderType.Normal, int borderWidth = 1)
        {
            ContentColor = content;
            BorderMainColor = border;
            BorderSecondaryColor = border;
            BorderType = borderType;
            BorderWidth = borderWidth;
        }

        public QGeneratedTexture(QColor content, QColor lightBorder, QColor darkBorder, BorderType borderType = BorderType.Shaded, int borderWidth = 2)
        {
            ContentColor = content;
            BorderMainColor = lightBorder;
            BorderSecondaryColor = darkBorder;
            BorderType = borderType;
            BorderWidth = borderWidth;
        }

        public Texture2D GetTexture2D(int width = 8, int height = 8, bool saveTexture = true)
        {
            if (!saveTexture) { return GenerateTexture(ContentColor, BorderMainColor, BorderSecondaryColor, BorderType, BorderWidth, width, height); }
            _texture2D = GenerateTexture(ContentColor, BorderMainColor, BorderSecondaryColor, BorderType, BorderWidth, width, height);
            return Texture2D;
        }

        public Texture GetTexture(int width = 8, int height = 8, bool saveTexture = true)
        {
            return GetTexture2D(width, height, saveTexture);
        }

        /// <summary>
        /// Generates a Texture2D with the given settings.
        /// </summary>
        /// <param name="content">Colors for the content(center color).</param>
        /// <param name="borderMain">Colors for the main border. </param>
        /// <param name="borderSecondary">Colors for the secondary border (used with BorderType.Shaded)</param>
        /// <param name="borderType">Border Type.</param>
        /// <param name="borderWidth">Border thickness.</param>
        /// <param name="width">Texture's width.</param>
        /// <param name="height">Texture's height.</param>
        /// <returns></returns>
        public static Texture2D GenerateTexture(QColor content, QColor borderMain, QColor borderSecondary, BorderType borderType, int borderWidth = 1, int width = 8, int height = 8)
        {
            if (borderWidth < 0) { borderWidth = 0; } else if (borderWidth > width / 2 || borderWidth > height / 2) { borderWidth = ((width > height ? height : width) / 2) - 1; }
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            if (borderType == BorderType.None)
            {
                for (int y = 0; y < width; y++) { for (int x = 0; x < height; x++) { texture.SetPixel(x, y, content.Color); } }
            }
            else if (borderType == BorderType.Normal)
            {
                for (int y = 0; y < width; y++)
                {
                    for (int x = 0; x < height; x++)
                    {
                        if (x <= (borderWidth - 1) || x >= (width - borderWidth))
                        {
                            texture.SetPixel(x, y, borderMain.Color);
                        }
                        else if (y <= (borderWidth - 1) || y >= (height - borderWidth))
                        {
                            texture.SetPixel(x, y, borderMain.Color);
                        }
                        else
                        {
                            texture.SetPixel(x, y, content.Color);
                        }
                    }
                }
            }
            else if (borderType == BorderType.Shaded)
            {
                //TODO: make the shaded texture
            }
            texture.Apply();
            return texture;
        }
    }
}

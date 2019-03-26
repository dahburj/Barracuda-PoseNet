// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuickEditor
{
    public partial class Style
    {
        public enum BackgroundType
        {
            High,
            Low
        }


        public enum InfoMessage
        {
            Help,
            Info,
            Warning,
            Error,
            Success
        }

        public enum Default
        {
            Toggle
        }

        public enum QuickButton
        {
            Minus,
            Plus,
            Cancel,
            Ok,
            Lock,
            LockSelected,
            Save,
            SaveSelected,
            Reset,
            Graph,
            GraphSelected,
            Data,
            Play,
            Stop
        }

        public enum LinkButton
        {
            Link,
            Unity,
            YouTube,
            Manual
        }

        public enum SlicedButton
        {
            Gray,
            GraySelected,
            Green,
            GreenSelected,
            Blue,
            BlueSelected,
            Orange,
            OrangeSelected,
            Red,
            RedSelected,
            Purple,
            PurpleSelected
        }

        public enum GhostButton
        {
            Gray,
            GraySelected,
            Green,
            GreenSelected,
            Blue,
            BlueSelected,
            Orange,
            OrangeSelected,
            Red,
            RedSelected,
            Purple,
            PurpleSelected
        }

        public enum SlicedBar
        {
            Gray,
            GraySelected,
            Green,
            GreenSelected,
            Blue,
            BlueSelected,
            Orange,
            OrangeSelected,
            Red,
            RedSelected,
            Purple,
            PurpleSelected
        }

        public enum GhostBar
        {
            Gray,
            GraySelected,
            Green,
            GreenSelected,
            Blue,
            BlueSelected,
            Orange,
            OrangeSelected,
            Red,
            RedSelected,
            Purple,
            PurpleSelected
        }

        public enum GhostTitle
        {
            Gray,
            Green,
            Blue,
            Orange,
            Red,
            Purple
        }

        public enum Align
        {
            Left,
            Center,
            Right
        }

        public enum FontStyle
        {
            Normal = 0,
            Bold = 1,
            Italic = 2,
            BoldAndItalic = 3
        }

        public enum Text
        {
            Title,
            Subtitle,
            Normal,
            Small,
            Tiny,
            Comment,
            Help,
            Button,
            Bar,
            InfoMessageTitle,
            InfoMessageMessage
        }

    }

    public class QStyles
    {
        public const string RESOURCES_PATH_SKINS = "Quick/Skins/";
        public static string RELATIVE_PATH_SKINS { get { return Q.QUICK_EDITOR_PATH + "Resources/" + RESOURCES_PATH_SKINS; } }

        public const string DARK_SKIN_FILENAME = "QSkinDark";
        public const string LIGHT_SKING_FILENAME = "QSkinLight";



        private const int FONT_SIZE_Title = 15;
        private const int FONT_SIZE_Subtitle = 13;
        private const int FONT_SIZE_Normal = 12;
        private const int FONT_SIZE_Small = 10;
        private const int FONT_SIZE_Tiny = 8;
        private const int FONT_SIZE_Comment = 11;
        private const int FONT_SIZE_Help = 11;

        private const int FONT_SIZE_Button = 10;
        private const int FONT_SIZE_Bar = 10;

        private const int FONT_SIZE_InfoMessageTitle = 12;
        private const int FONT_SIZE_InfoMessageMessage = 11;


        /// <summary>
        /// Returns the stylename in the following format: enum name + enum value.
        /// Example enum 'Button' and enum value 'Button.Minus', it returns the string 'ButtonMinus'.
        /// </summary>
        public static string GetStyleName<T>(T styleName) { return (typeof(T) + styleName.ToString()).Split('+')[1]; }

        /// <summary>
        /// Returns a style that has been added to the skin.
        /// </summary>
        public static GUIStyle GetStyle(string styleName) { return Skin.GetStyle(styleName); }
        /// <summary>
        /// Returns a style that has been added to the skin.
        /// This method is to be used paired with the enums in the Style class.
        /// </summary>
        public static GUIStyle GetStyle<T>(T styleName) { return Skin.GetStyle(GetStyleName(styleName)); }

        public static GUIStyle GetTextStyle(Style.Text style) { return GetStyle(GetStyleName(style)); }

        private static GUISkin skin;
        public static GUISkin Skin
        {
            get
            {
                if(skin == null)
                {
                    skin = Q.GetResource<GUISkin>(RESOURCES_PATH_SKINS, QUI.IsProSkin ? DARK_SKIN_FILENAME : LIGHT_SKING_FILENAME);
                    if(skin == null)
                    {
                        skin = GenerateSkin();
                        QUI.SetDirty(skin);
                        AssetDatabase.SaveAssets();
                    }
                }
                return skin;
            }
        }

        private static GUISkin GenerateSkin()
        {
            skin = Q.CreateAsset<GUISkin>(RELATIVE_PATH_SKINS, QUI.IsProSkin ? DARK_SKIN_FILENAME : LIGHT_SKING_FILENAME);

            List<GUIStyle> styles = new List<GUIStyle>();

            styles.AddRange(OldDefaultStyles());

            styles.AddRange(DefaultStyles());
            styles.AddRange(BackgroundStyles());
            styles.AddRange(QuickButtonStyles());
            styles.AddRange(LinkButtonStyles());
            styles.AddRange(SlicedButtonStyles());
            styles.AddRange(GhostButtonStyles());
            styles.AddRange(SlicedBarStyles());
            styles.AddRange(GhostBarStyles());
            styles.AddRange(GhostTitleStyles());
            styles.AddRange(TextStyles());
            styles.AddRange(InfoMessageHeaderStyles());

            skin.customStyles = styles.ToArray();

            skin.label = GUI.skin.label;

            QUI.SetDirty(skin);
            AssetDatabase.SaveAssets();
            return skin;
        }

        private static void UpdateSkin()
        {
            skin = null;
            skin = GenerateSkin();
        }

        public static void AddStyle(GUIStyle style)
        {
            if(style == null) { return; }
            List<GUIStyle> customStyles = new List<GUIStyle>();
            customStyles.AddRange(Skin.customStyles);
            if(customStyles.Contains(style)) { return; }
            customStyles.Add(style);
            Skin.customStyles = customStyles.ToArray();
        }
        public static void RemoveStyle(GUIStyle style)
        {
            if(style == null) { return; }
            List<GUIStyle> customStyles = new List<GUIStyle>();
            customStyles.AddRange(Skin.customStyles);
            if(!customStyles.Contains(style)) { return; }
            customStyles.Remove(style);
            Skin.customStyles = customStyles.ToArray();
        }

        public static Vector2 CalcSize(GUIContent content, Style.Text style)
        {
            return GetStyle(GetStyleName(style)).CalcSize(content);
        }

        private static List<GUIStyle> OldDefaultStyles()
        {
            List<GUIStyle> styles = new List<GUIStyle>
            {
                OldTextStyleWithBackground("Help", QResources.WhiteBackground.Texture2D, QColors.Help, 12, FontStyle.Normal, TextAnchor.MiddleLeft, true, true, QuickEngine.QResources.FontAwesome),
                OldTextStyleWithBackground("Info", QResources.WhiteBackground.Texture2D, QColors.Info, 12, FontStyle.Normal, TextAnchor.MiddleLeft, true, true, QuickEngine.QResources.FontAwesome),
                OldTextStyleWithBackground("Warning", QResources.WhiteBackground.Texture2D, QColors.Warning, 12, FontStyle.Normal, TextAnchor.MiddleLeft, true, true, QuickEngine.QResources.FontAwesome),
                OldTextStyleWithBackground("Error", QResources.WhiteBackground.Texture2D, QColors.Error, 12, FontStyle.Normal, TextAnchor.MiddleLeft, true, true, QuickEngine.QResources.FontAwesome)
            };
            return styles;
        }
        private static GUIStyle OldTextStyleWithBackground(string name, Texture2D background, QColor textColor, int fontSize, FontStyle fontStyle, TextAnchor alignment, bool richText = true, bool wordWrap = true, Font font = null)
        {
            return new GUIStyle()
            {
                name = name,
                normal =
                {
                    background = background,
                    textColor = QUI.IsProSkin ? textColor.Dark : textColor.Light
                },
                fontSize = fontSize,
                fontStyle = fontStyle,
                alignment = alignment,
                richText = richText,
                wordWrap = wordWrap,
                padding = new RectOffset(8, 8, 8, 8),
                border = new RectOffset(2, 2, 2, 2),
                font = font
            };
        }

        private static List<GUIStyle> DefaultStyles()
        {
            return new List<GUIStyle>()
            {
                new GUIStyle(GUI.skin.toggle) { name = GetStyleName(Style.Default.Toggle),
                                                fontSize = GetTextFontSize(Style.Text.Normal),
                                                font = GetTextFont(Style.Text.Normal),
                                                padding = new RectOffset(GUI.skin.toggle.padding.left + 2, GUI.skin.toggle.padding.right, GUI.skin.toggle.padding.top + 1, GUI.skin.toggle.padding.bottom),
                                                margin = new RectOffset(0,0,0,0)
                                              }
            };
        }

        private static List<GUIStyle> BackgroundStyles()
        {
            return new List<GUIStyle>()
            {
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.High, QColors.Color.Gray), QResources.backgroundHighGray),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.High, QColors.Color.Green), QResources.backgroundHighGreen),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.High, QColors.Color.Blue), QResources.backgroundHighBlue),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.High, QColors.Color.Orange), QResources.backgroundHighOrange),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.High, QColors.Color.Red), QResources.backgroundHighRed),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.High, QColors.Color.Purple), QResources.backgroundHighPurple),

                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.Low, QColors.Color.Gray), QResources.backgroundLowGray),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.Low, QColors.Color.Green), QResources.backgroundLowGreen),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.Low, QColors.Color.Blue), QResources.backgroundLowBlue),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.Low, QColors.Color.Orange), QResources.backgroundLowOrange),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.Low, QColors.Color.Red), QResources.backgroundLowRed),
                BackgroundStyle(GetBackgroundStyleName(Style.BackgroundType.Low, QColors.Color.Purple), QResources.backgroundLowPurple),
            };
        }
        private static GUIStyle BackgroundStyle(string styleName, QTexture qTexture)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.normal2D },
                border = new RectOffset(8, 8, 8, 8)
            };
        }
        private static string GetBackgroundStyleName(Style.BackgroundType type, QColors.Color color)
        {
            return GetStyleName(type) + color.ToString();
        }
        public static GUIStyle GetBackgroundStyle(Style.BackgroundType type, QColors.Color color)
        {
            return GetStyle(GetBackgroundStyleName(type, color));
        }

        private static List<GUIStyle> QuickButtonStyles()
        {
            return new List<GUIStyle>()
            {
                QuickButtonStyle(GetStyleName(Style.QuickButton.Minus), QResources.btnMinus),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Plus), QResources.btnPlus),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Cancel), QResources.btnCancel),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Ok), QResources.btnOk),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Lock), QResources.btnLock),
                QuickButtonStyle(GetStyleName(Style.QuickButton.LockSelected), QResources.btnLockSelected),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Save), QResources.btnSave),
                QuickButtonStyle(GetStyleName(Style.QuickButton.SaveSelected), QResources.btnSaveSelected),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Reset), QResources.btnReset),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Graph), QResources.btnGraph),
                QuickButtonStyle(GetStyleName(Style.QuickButton.GraphSelected), QResources.btnGraphSelected),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Data), QResources.btnData),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Play), QResources.btnPlay),
                QuickButtonStyle(GetStyleName(Style.QuickButton.Stop), QResources.btnStop)
            };
        }
        private static GUIStyle QuickButtonStyle(string styleName, QTexture qTexture)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.normal2D },
                onNormal = { background = qTexture.normal2D },
                hover = { background = qTexture.hover2D },
                onHover = { background = qTexture.hover2D },
                active = { background = qTexture.active2D },
                onActive = { background = qTexture.active2D },
                margin = new RectOffset(0, 0, 2, 0)
            };
        }

        private static List<GUIStyle> LinkButtonStyles()
        {
            RectOffset border = new RectOffset(70, 8, 8, 8);
            RectOffset padding = new RectOffset(70, 8, 8, 8);
            int fontSize = GetTextFontSize(Style.Text.Button);
            TextAnchor alignment = TextAnchor.MiddleLeft;
            Font font = GetTextFont(Style.Text.Button);
            return new List<GUIStyle>()
            {
                ButtonStyle(GetStyleName(Style.LinkButton.Link), QResources.linkButtonLink, new RectOffset(30, 8, 8, 8),  (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,  (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,  (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,  new RectOffset(30, 8, 8, 8), fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.LinkButton.Unity), QResources.linkButtonUnity, border,  (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,  (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,  (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,  padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.LinkButton.YouTube), QResources.linkButtonYouTube, border,  (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,  (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,  (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,  padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.LinkButton.Manual), QResources.linkButtonManual, border,  (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,  (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,  (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,  padding, fontSize, alignment, font),
            };
        }

        private static List<GUIStyle> SlicedButtonStyles()
        {
            RectOffset border = new RectOffset(8, 8, 8, 8);
            RectOffset padding = GetFontPadding(Style.Text.Button);
            int fontSize = GetTextFontSize(Style.Text.Button);
            TextAnchor alignment = TextAnchor.MiddleCenter;
            Font font = GetTextFont(Style.Text.Button);
            return new List<GUIStyle>()
            {
                ButtonStyle(GetStyleName(Style.SlicedButton.Gray), QResources.btnSlicedGray,        border, (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,     padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedButton.Green), QResources.btnSlicedGreen,      border, (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,         (QUI.IsProSkin ? QColors.GreenLight : QColors.GreenDark).Color,    (QUI.IsProSkin ? QColors.Green : QColors.GreenLight).Color,     padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedButton.Blue), QResources.btnSlicedBlue,        border, (QUI.IsProSkin ? QColors.Blue : QColors.BlueDark).Color,           (QUI.IsProSkin ? QColors.BlueLight : QColors.BlueDark).Color,      (QUI.IsProSkin ? QColors.Blue : QColors.BlueLight).Color,       padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedButton.Orange), QResources.btnSlicedOrange,    border, (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,       (QUI.IsProSkin ? QColors.OrangeLight : QColors.OrangeDark).Color,  (QUI.IsProSkin ? QColors.Orange : QColors.OrangeLight).Color,   padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedButton.Red), QResources.btnSlicedRed,          border, (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,             (QUI.IsProSkin ? QColors.RedLight : QColors.RedDark).Color,        (QUI.IsProSkin ? QColors.Red : QColors.RedLight).Color,         padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedButton.Purple), QResources.btnSlicedPurple,    border, (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,       (QUI.IsProSkin ? QColors.PurpleLight : QColors.PurpleDark).Color,  (QUI.IsProSkin ? QColors.Purple : QColors.PurpleLight).Color,   padding, fontSize, alignment, font),

                SelectedButtonStyle(GetStyleName(Style.SlicedButton.GraySelected), QResources.btnSlicedGray,        border, (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,     padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedButton.GreenSelected), QResources.btnSlicedGreen,      border, (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,         (QUI.IsProSkin ? QColors.GreenLight : QColors.GreenDark).Color,    (QUI.IsProSkin ? QColors.Green : QColors.GreenLight).Color,     padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedButton.BlueSelected), QResources.btnSlicedBlue,        border, (QUI.IsProSkin ? QColors.Blue : QColors.BlueDark).Color,           (QUI.IsProSkin ? QColors.BlueLight : QColors.BlueDark).Color,      (QUI.IsProSkin ? QColors.Blue : QColors.BlueLight).Color,       padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedButton.OrangeSelected), QResources.btnSlicedOrange,    border, (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,       (QUI.IsProSkin ? QColors.OrangeLight : QColors.OrangeDark).Color,  (QUI.IsProSkin ? QColors.Orange : QColors.OrangeLight).Color,   padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedButton.RedSelected), QResources.btnSlicedRed,          border, (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,             (QUI.IsProSkin ? QColors.RedLight : QColors.RedDark).Color,        (QUI.IsProSkin ? QColors.Red : QColors.RedLight).Color,         padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedButton.PurpleSelected), QResources.btnSlicedPurple,    border, (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,       (QUI.IsProSkin ? QColors.PurpleLight : QColors.PurpleDark).Color,  (QUI.IsProSkin ? QColors.Purple : QColors.PurpleLight).Color,   padding, fontSize, alignment, font)
            };
        }
        private static List<GUIStyle> GhostButtonStyles()
        {
            RectOffset border = new RectOffset(8, 8, 8, 8);
            RectOffset padding = GetFontPadding(Style.Text.Button);
            int fontSize = GetTextFontSize(Style.Text.Button);
            TextAnchor alignment = TextAnchor.MiddleCenter;
            Font font = GetTextFont(Style.Text.Button);
            return new List<GUIStyle>()
            {
                ButtonStyle(GetStyleName(Style.GhostButton.Gray),   QResources.btnGhostGray,    border, (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,     padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostButton.Green),  QResources.btnGhostGreen,   border, (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,         (QUI.IsProSkin ? QColors.GreenLight : QColors.GreenDark).Color,    (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,    padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostButton.Blue),   QResources.btnGhostBlue,    border, (QUI.IsProSkin ? QColors.Blue :  QColors.BlueDark).Color,          (QUI.IsProSkin ? QColors.BlueLight : QColors.BlueDark).Color,      (QUI.IsProSkin ? QColors.Blue : QColors.BlueDark).Color,      padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostButton.Orange), QResources.btnGhostOrange,  border, (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,       (QUI.IsProSkin ? QColors.OrangeLight : QColors.OrangeDark).Color,  (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,  padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostButton.Red),    QResources.btnGhostRed,     border, (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,             (QUI.IsProSkin ? QColors.RedLight : QColors.RedDark).Color,        (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,        padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostButton.Purple), QResources.btnGhostPurple,  border, (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,       (QUI.IsProSkin ? QColors.PurpleLight : QColors.PurpleDark).Color,  (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,  padding, fontSize, alignment, font),

                SelectedButtonStyle(GetStyleName(Style.GhostButton.GraySelected),   QResources.btnGhostGray,    border, (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,     padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostButton.GreenSelected),  QResources.btnGhostGreen,   border, (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,         (QUI.IsProSkin ? QColors.GreenLight : QColors.GreenDark).Color,    (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,    padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostButton.BlueSelected),   QResources.btnGhostBlue,    border, (QUI.IsProSkin ? QColors.Blue :  QColors.BlueDark).Color,          (QUI.IsProSkin ? QColors.BlueLight : QColors.BlueDark).Color,      (QUI.IsProSkin ? QColors.Blue : QColors.BlueDark).Color,      padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostButton.OrangeSelected), QResources.btnGhostOrange,  border, (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,       (QUI.IsProSkin ? QColors.OrangeLight : QColors.OrangeDark).Color,  (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,  padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostButton.RedSelected),    QResources.btnGhostRed,     border, (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,             (QUI.IsProSkin ? QColors.RedLight : QColors.RedDark).Color,        (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,        padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostButton.PurpleSelected), QResources.btnGhostPurple,  border, (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,       (QUI.IsProSkin ? QColors.PurpleLight : QColors.PurpleDark).Color,  (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,  padding, fontSize, alignment, font)
            };
        }
        private static List<GUIStyle> SlicedBarStyles()
        {
            RectOffset border = new RectOffset(8, 8, 8, 8);
            RectOffset padding = GetFontPadding(Style.Text.Bar);
            int fontSize = GetTextFontSize(Style.Text.Bar);
            TextAnchor alignment = TextAnchor.MiddleLeft;
            Font font = GetTextFont(Style.Text.Bar);
            return new List<GUIStyle>()
            {
                ButtonStyle(GetStyleName(Style.SlicedBar.Gray), QResources.btnSlicedGray,        border, (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,     padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedBar.Green), QResources.btnSlicedGreen,      border, (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,         (QUI.IsProSkin ? QColors.GreenLight : QColors.GreenDark).Color,    (QUI.IsProSkin ? QColors.Green : QColors.GreenLight).Color,     padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedBar.Blue), QResources.btnSlicedBlue,        border, (QUI.IsProSkin ? QColors.Blue : QColors.BlueDark).Color,           (QUI.IsProSkin ? QColors.BlueLight : QColors.BlueDark).Color,      (QUI.IsProSkin ? QColors.Blue : QColors.BlueLight).Color,       padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedBar.Orange), QResources.btnSlicedOrange,    border, (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,       (QUI.IsProSkin ? QColors.OrangeLight : QColors.OrangeDark).Color,  (QUI.IsProSkin ? QColors.Orange : QColors.OrangeLight).Color,   padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedBar.Red), QResources.btnSlicedRed,          border, (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,             (QUI.IsProSkin ? QColors.RedLight : QColors.RedDark).Color,        (QUI.IsProSkin ? QColors.Red : QColors.RedLight).Color,         padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.SlicedBar.Purple), QResources.btnSlicedPurple,    border, (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,       (QUI.IsProSkin ? QColors.PurpleLight : QColors.PurpleDark).Color,  (QUI.IsProSkin ? QColors.Purple : QColors.PurpleLight).Color,   padding, fontSize, alignment, font),

                SelectedButtonStyle(GetStyleName(Style.SlicedBar.GraySelected), QResources.btnSlicedGray,        border, (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,     padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedBar.GreenSelected), QResources.btnSlicedGreen,      border, (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,         (QUI.IsProSkin ? QColors.GreenLight : QColors.GreenDark).Color,    (QUI.IsProSkin ? QColors.Green : QColors.GreenLight).Color,     padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedBar.BlueSelected), QResources.btnSlicedBlue,        border, (QUI.IsProSkin ? QColors.Blue : QColors.BlueDark).Color,           (QUI.IsProSkin ? QColors.BlueLight : QColors.BlueDark).Color,      (QUI.IsProSkin ? QColors.Blue : QColors.BlueLight).Color,       padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedBar.OrangeSelected), QResources.btnSlicedOrange,    border, (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,       (QUI.IsProSkin ? QColors.OrangeLight : QColors.OrangeDark).Color,  (QUI.IsProSkin ? QColors.Orange : QColors.OrangeLight).Color,   padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedBar.RedSelected), QResources.btnSlicedRed,          border, (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,             (QUI.IsProSkin ? QColors.RedLight : QColors.RedDark).Color,        (QUI.IsProSkin ? QColors.Red : QColors.RedLight).Color,         padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.SlicedBar.PurpleSelected), QResources.btnSlicedPurple,    border, (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,       (QUI.IsProSkin ? QColors.PurpleLight : QColors.PurpleDark).Color,  (QUI.IsProSkin ? QColors.Purple : QColors.PurpleLight).Color,   padding, fontSize, alignment, font)
            };
        }
        private static List<GUIStyle> GhostBarStyles()
        {
            RectOffset border = new RectOffset(8, 8, 8, 8);
            RectOffset padding = GetFontPadding(Style.Text.Bar);
            int fontSize = GetTextFontSize(Style.Text.Bar);
            TextAnchor alignment = TextAnchor.MiddleLeft;
            Font font = GetTextFont(Style.Text.Bar);
            return new List<GUIStyle>()
            {
                ButtonStyle(GetStyleName(Style.GhostBar.Gray),   QResources.btnGhostGray,    border, (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityMild : QColors.UnityDark).Color,     padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostBar.Green),  QResources.btnGhostGreen,   border, (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,         (QUI.IsProSkin ? QColors.GreenLight : QColors.GreenDark).Color,    (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,    padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostBar.Blue),   QResources.btnGhostBlue,    border, (QUI.IsProSkin ? QColors.Blue :  QColors.BlueDark).Color,          (QUI.IsProSkin ? QColors.BlueLight : QColors.BlueDark).Color,      (QUI.IsProSkin ? QColors.Blue : QColors.BlueDark).Color,      padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostBar.Orange), QResources.btnGhostOrange,  border, (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,       (QUI.IsProSkin ? QColors.OrangeLight : QColors.OrangeDark).Color,  (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,  padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostBar.Red),    QResources.btnGhostRed,     border, (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,             (QUI.IsProSkin ? QColors.RedLight : QColors.RedDark).Color,        (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,        padding, fontSize, alignment, font),
                ButtonStyle(GetStyleName(Style.GhostBar.Purple), QResources.btnGhostPurple,  border, (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,       (QUI.IsProSkin ? QColors.PurpleLight : QColors.PurpleDark).Color,  (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,  padding, fontSize, alignment, font),

                SelectedButtonStyle(GetStyleName(Style.GhostBar.GraySelected),   QResources.btnGhostGray,    border, (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,    (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color,     padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostBar.GreenSelected),  QResources.btnGhostGreen,   border, (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,         (QUI.IsProSkin ? QColors.GreenLight : QColors.GreenDark).Color,    (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,    padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostBar.BlueSelected),   QResources.btnGhostBlue,    border, (QUI.IsProSkin ? QColors.Blue :  QColors.BlueDark).Color,          (QUI.IsProSkin ? QColors.BlueLight : QColors.BlueDark).Color,      (QUI.IsProSkin ? QColors.Blue : QColors.BlueDark).Color,      padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostBar.OrangeSelected), QResources.btnGhostOrange,  border, (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,       (QUI.IsProSkin ? QColors.OrangeLight : QColors.OrangeDark).Color,  (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,  padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostBar.RedSelected),    QResources.btnGhostRed,     border, (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,             (QUI.IsProSkin ? QColors.RedLight : QColors.RedDark).Color,        (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,        padding, fontSize, alignment, font),
                SelectedButtonStyle(GetStyleName(Style.GhostBar.PurpleSelected), QResources.btnGhostPurple,  border, (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,       (QUI.IsProSkin ? QColors.PurpleLight : QColors.PurpleDark).Color,  (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,  padding, fontSize, alignment, font)

            };
        }
        private static GUIStyle ButtonStyle(string styleName, QTexture qTexture, RectOffset border)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.normal2D },
                onNormal = { background = qTexture.normal2D },
                hover = { background = qTexture.hover2D },
                onHover = { background = qTexture.hover2D },
                active = { background = qTexture.active2D },
                onActive = { background = qTexture.active2D },
                border = border
            };
        }
        private static GUIStyle ButtonStyle(string styleName, QTexture qTexture, RectOffset border, Color normalTextColor, Color hoverTextColor, Color activeTextColor, RectOffset padding, int fontSize, TextAnchor alignment, Font font)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.normal2D, textColor = normalTextColor },
                onNormal = { background = qTexture.normal2D, textColor = normalTextColor },
                hover = { background = qTexture.hover2D, textColor = hoverTextColor },
                onHover = { background = qTexture.hover2D, textColor = hoverTextColor },
                active = { background = qTexture.active2D, textColor = activeTextColor },
                onActive = { background = qTexture.active2D, textColor = activeTextColor },
                border = border,
                padding = padding,
                fontSize = fontSize,
                alignment = alignment,
                font = font
            };
        }
        private static GUIStyle SelectedButtonStyle(string styleName, QTexture qTexture)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.active2D },
                onNormal = { background = qTexture.active2D },
                hover = { background = qTexture.hover2D },
                onHover = { background = qTexture.hover2D },
                active = { background = qTexture.normal2D },
                onActive = { background = qTexture.normal2D }
            };
        }
        private static GUIStyle SelectedButtonStyle(string styleName, QTexture qTexture, RectOffset border)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.active2D },
                onNormal = { background = qTexture.active2D },
                hover = { background = qTexture.hover2D },
                onHover = { background = qTexture.hover2D },
                active = { background = qTexture.normal2D },
                onActive = { background = qTexture.normal2D },
                border = border
            };
        }
        private static GUIStyle SelectedButtonStyle(string styleName, QTexture qTexture, RectOffset border, Color normalTextColor, Color hoverTextColor, Color activeTextColor, RectOffset padding, int fontSize, TextAnchor alignment, Font font)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.active2D, textColor = activeTextColor },
                onNormal = { background = qTexture.active2D, textColor = activeTextColor },
                hover = { background = qTexture.hover2D, textColor = hoverTextColor },
                onHover = { background = qTexture.hover2D, textColor = hoverTextColor },
                active = { background = qTexture.normal2D, textColor = normalTextColor },
                onActive = { background = qTexture.normal2D, textColor = normalTextColor },
                border = border,
                padding = padding,
                fontSize = fontSize,
                alignment = alignment,
                font = font
            };
        }

        private static List<GUIStyle> GhostTitleStyles()
        {
            RectOffset border = new RectOffset(8, 8, 8, 8);
            RectOffset padding = GetFontPadding(Style.Text.Title);
            int fontSize = GetTextFontSize(Style.Text.Title);
            TextAnchor alignment = TextAnchor.MiddleCenter;
            Font font = GetTextFont(Style.Text.Title);

            return new List<GUIStyle>()
            {
                TitleStyle(GetStyleName(Style.GhostTitle.Gray),     QResources.titleGhostGray, border,      (QUI.IsProSkin ? QColors.UnityLight : QColors.UnityDark).Color, padding, fontSize, alignment, font),
                TitleStyle(GetStyleName(Style.GhostTitle.Green),    QResources.titleGhostGreen, border,     (QUI.IsProSkin ? QColors.Green : QColors.GreenDark).Color,      padding, fontSize, alignment, font),
                TitleStyle(GetStyleName(Style.GhostTitle.Blue),     QResources.titleGhostBlue, border,      (QUI.IsProSkin ? QColors.Blue :  QColors.BlueDark).Color,       padding, fontSize, alignment, font),
                TitleStyle(GetStyleName(Style.GhostTitle.Orange),   QResources.titleGhostOrange, border,    (QUI.IsProSkin ? QColors.Orange : QColors.OrangeDark).Color,    padding, fontSize, alignment, font),
                TitleStyle(GetStyleName(Style.GhostTitle.Red),      QResources.titleGhostRed, border,       (QUI.IsProSkin ? QColors.Red : QColors.RedDark).Color,          padding, fontSize, alignment, font),
                TitleStyle(GetStyleName(Style.GhostTitle.Purple),   QResources.titleGhostPurple, border,    (QUI.IsProSkin ? QColors.Purple : QColors.PurpleDark).Color,    padding, fontSize, alignment, font)
            };
        }
        private static List<GUIStyle> TextStyles()
        {
            bool wordWrap = false;
            bool richText = true;
            List<GUIStyle> tempList = new List<GUIStyle>
            {
                TextStyle(GetStyleName(Style.Text.Title), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Title), wordWrap, richText, GetFontPadding(Style.Text.Title), GetTextFont(Style.Text.Title)),
                TextStyle(GetStyleName(Style.Text.Subtitle), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Subtitle), wordWrap, richText, GetFontPadding(Style.Text.Subtitle), GetTextFont(Style.Text.Subtitle)),
                TextStyle(GetStyleName(Style.Text.Normal), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Normal), wordWrap, richText, GetFontPadding(Style.Text.Normal), GetTextFont(Style.Text.Normal)),
                TextStyle(GetStyleName(Style.Text.Small), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Small), wordWrap, richText, GetFontPadding(Style.Text.Small), GetTextFont(Style.Text.Small)),
                TextStyle(GetStyleName(Style.Text.Tiny), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Tiny), wordWrap, richText, GetFontPadding(Style.Text.Tiny), GetTextFont(Style.Text.Tiny)),
                TextStyle(GetStyleName(Style.Text.Comment), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Comment), wordWrap, richText, GetFontPadding(Style.Text.Comment), GetTextFont(Style.Text.Comment)),
                TextStyle(GetStyleName(Style.Text.Help), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Help), wordWrap, richText, GetFontPadding(Style.Text.Help), GetTextFont(Style.Text.Help)),
                TextStyle(GetStyleName(Style.Text.Button), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Button), wordWrap, richText, GetFontPadding(Style.Text.Normal), GetTextFont(Style.Text.Button)),
                TextStyle(GetStyleName(Style.Text.Bar), TextAnchor.MiddleLeft, FontStyle.Normal, GetTextFontSize(Style.Text.Bar), wordWrap, richText, GetFontPadding(Style.Text.Normal), GetTextFont(Style.Text.Bar)),
            };
            return tempList;
        }
        private static GUIStyle TitleStyle(string styleName, QTexture qTexture, RectOffset border, Color normalTextColor, RectOffset padding, int fontSize, TextAnchor alignment, Font font)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.normal2D, textColor = normalTextColor },
                onNormal = { background = qTexture.normal2D, textColor = normalTextColor },
                border = border,
                padding = padding,
                fontSize = fontSize,
                alignment = alignment,
                font = font
            };
        }
        private static GUIStyle TextStyle(string styleName, TextAnchor alignment, FontStyle fontStyle, int fontSize, bool wordWrap, bool richText, RectOffset padding, Font font = null)
        {
            return new GUIStyle(GUI.skin.label)
            {
                name = styleName,
                alignment = alignment,
                fontSize = fontSize,
                fontStyle = fontStyle,
                richText = richText,
                wordWrap = wordWrap,
                padding = padding,
                margin = new RectOffset(0, 0, 0, 0),
                font = font
            };
        }

        private static List<GUIStyle> InfoMessageHeaderStyles()
        {
            return new List<GUIStyle>()
            {
                InfoMessageTitleStyle(GetStyleName(Style.InfoMessage.Success),      QResources.infoMessageTitleSuccess,     QUI.IsProSkin? QColors.GreenLight.Color : QColors.GreenDark.Color),
                InfoMessageTitleStyle(GetStyleName(Style.InfoMessage.Warning),      QResources.infoMessageTitleWarning,     QUI.IsProSkin? QColors.OrangeLight.Color : QColors.OrangeDark.Color),
                InfoMessageTitleStyle(GetStyleName(Style.InfoMessage.Error),        QResources.infoMessageTitleError,       QUI.IsProSkin? QColors.RedLight.Color : QColors.RedDark.Color),
                InfoMessageTitleStyle(GetStyleName(Style.InfoMessage.Info),         QResources.infoMessageTitleInfo,        QUI.IsProSkin? QColors.BlueLight.Color : QColors.BlueDark.Color),
                InfoMessageTitleStyle(GetStyleName(Style.InfoMessage.Help),         QResources.infoMessageTitleHelp,        QUI.IsProSkin? QColors.UnityLight.Color : QColors.UnityDark.Color),

                InfoMessageMessageStyle(GetStyleName(Style.InfoMessage.Success),    QResources.infoMessageMessageSuccess,   QUI.IsProSkin? QColors.GreenLight.Color : QColors.GreenDark.Color),
                InfoMessageMessageStyle(GetStyleName(Style.InfoMessage.Warning),    QResources.infoMessageMessageWarning,   QUI.IsProSkin? QColors.OrangeLight.Color : QColors.OrangeDark.Color),
                InfoMessageMessageStyle(GetStyleName(Style.InfoMessage.Error),      QResources.infoMessageMessageError,     QUI.IsProSkin? QColors.RedLight.Color : QColors.RedDark.Color),
                InfoMessageMessageStyle(GetStyleName(Style.InfoMessage.Info),       QResources.infoMessageMessageInfo,      QUI.IsProSkin? QColors.BlueLight.Color : QColors.BlueDark.Color),
                InfoMessageMessageStyle(GetStyleName(Style.InfoMessage.Help),       QResources.infoMessageMessageHelp,      QUI.IsProSkin? QColors.UnityLight.Color : QColors.UnityDark.Color)
            };
        }
        private static GUIStyle InfoMessageTitleStyle(string styleName, QTexture qTexture, Color normalTextColor)
        {
            return new GUIStyle()
            {
                name = styleName + "Title",
                normal = { background = qTexture.normal2D, textColor = normalTextColor },
                border = new RectOffset(24, 8, 8, 8),
                alignment = TextAnchor.MiddleLeft,
                fontSize = GetTextFontSize(Style.Text.InfoMessageTitle),
                padding = GetFontPadding(Style.Text.InfoMessageTitle),
                font = GetTextFont(Style.Text.InfoMessageTitle),
                richText = true,
                wordWrap = true,
                clipping = TextClipping.Clip,
                stretchHeight = false
            };
        }
        public static GUIStyle GetInfoMessageTitleStyle(Style.InfoMessage infoMessageType)
        {
            return GetStyle(GetStyleName(infoMessageType) + "Title");
        }
        private static GUIStyle InfoMessageMessageStyle(string styleName, QTexture qTexture, Color normalTextColor)
        {
            return new GUIStyle()
            {
                name = styleName + "Message",
                normal = { background = qTexture.normal2D, textColor = normalTextColor },
                border = new RectOffset(24, 8, 10, 8),
                contentOffset = new Vector2(0, 10),
                overflow = new RectOffset(0, 0, 0, 12),
                alignment = TextAnchor.UpperLeft,
                fontSize = GetTextFontSize(Style.Text.InfoMessageMessage),
                padding = GetFontPadding(Style.Text.InfoMessageMessage),
                font = GetTextFont(Style.Text.InfoMessageMessage),
                richText = true,
                wordWrap = true,
                clipping = TextClipping.Clip,
                stretchHeight = false
            };
        }
        public static GUIStyle GetInfoMessageMessageStyle(Style.InfoMessage infoMessageType)
        {
            return GetStyle(GetStyleName(infoMessageType) + "Message");
        }

        public static TextAnchor GetAlignment(Style.Align alignment)
        {
            switch(alignment)
            {
                case Style.Align.Left: return TextAnchor.MiddleLeft;
                case Style.Align.Center: return TextAnchor.MiddleCenter;
                case Style.Align.Right: return TextAnchor.MiddleRight;
                default: return TextAnchor.MiddleLeft;
            }
        }
        /// <summary>
        /// Converts the Style.FontStyle to FontStyle. By default it returns FontStyle.Normal.
        /// </summary>
        public static FontStyle GetFontStyle(Style.FontStyle fontStyle)
        {
            switch(fontStyle)
            {
                case Style.FontStyle.Normal: return FontStyle.Normal;
                case Style.FontStyle.Bold: return FontStyle.Bold;
                case Style.FontStyle.Italic: return FontStyle.Italic;
                case Style.FontStyle.BoldAndItalic: return FontStyle.BoldAndItalic;
                default: return FontStyle.Normal;
            }
        }
        public static RectOffset GetFontPadding(Style.Text textStyle)
        {
            switch(textStyle)
            {
                case Style.Text.Title: return new RectOffset(0, 0, 0, 0); //new RectOffset(2, 2, 1, 2);
                case Style.Text.Subtitle: return new RectOffset(0, 0, 0, 0); //new RectOffset(2, 2, -4, 2);
                case Style.Text.Normal: return new RectOffset(0, 0, 0, 0); //new RectOffset(2, 2, 1, 2);
                case Style.Text.Small: return new RectOffset(0, 0, 0, 0); //new RectOffset(2, 2, 1, 2);
                case Style.Text.Tiny: return new RectOffset(0, 0, 0, 0); //new RectOffset(2, 2, 1, 2);
                case Style.Text.Comment: return new RectOffset(0, 0, 0, 0); //new RectOffset(2, 2, 1, 2);
                case Style.Text.Help: return new RectOffset(0, 0, 0, 0); //new RectOffset(2, 2, 1, 2);
                case Style.Text.Button: return new RectOffset(4, 4, 0, 1);
                case Style.Text.Bar: return new RectOffset(24, 4, 1, 2);
                case Style.Text.InfoMessageTitle: return new RectOffset(26, 4, 0, 1);
                case Style.Text.InfoMessageMessage: return new RectOffset(26, 4, 4, 4);
                default: return new RectOffset(0, 0, 0, 0); //new RectOffset(2, 2, 1, 2);
            }
        }
        public static int GetTextFontSize(Style.Text textStyle)
        {
            switch(textStyle)
            {
                case Style.Text.Title: return FONT_SIZE_Title;
                case Style.Text.Subtitle: return FONT_SIZE_Subtitle;
                case Style.Text.Normal: return FONT_SIZE_Normal;
                case Style.Text.Small: return FONT_SIZE_Small;
                case Style.Text.Tiny: return FONT_SIZE_Tiny;
                case Style.Text.Comment: return FONT_SIZE_Comment;
                case Style.Text.Help: return FONT_SIZE_Help;
                case Style.Text.Button: return FONT_SIZE_Button;
                case Style.Text.Bar: return FONT_SIZE_Bar;
                case Style.Text.InfoMessageTitle: return FONT_SIZE_InfoMessageTitle;
                case Style.Text.InfoMessageMessage: return FONT_SIZE_InfoMessageMessage;
                default: return FONT_SIZE_Normal;
            }
        }
        public static Font GetTextFont(Style.Text textStyle)
        {
            switch(textStyle)
            {
                case Style.Text.Title: return QResources.GetFont(FontName.Ubuntu.Bold);
                case Style.Text.Subtitle: return QResources.GetFont(FontName.Ubuntu.Medium);
                case Style.Text.Normal: return QResources.GetFont(FontName.Ubuntu.Regular);
                case Style.Text.Small: return QResources.GetFont(FontName.Ubuntu.Regular);
                case Style.Text.Tiny: return QResources.GetFont(FontName.Ubuntu.Regular);
                case Style.Text.Comment: return QResources.GetFont(FontName.Ubuntu.RegularItalic);
                case Style.Text.Help: return QResources.GetFont(FontName.Ubuntu.LightItalic);
                case Style.Text.Button: return QResources.GetFont(FontName.Ubuntu.Medium);
                case Style.Text.Bar: return QResources.GetFont(FontName.Ubuntu.Medium);
                case Style.Text.InfoMessageTitle: return QResources.GetFont(FontName.Ubuntu.Medium);
                case Style.Text.InfoMessageMessage: return QResources.GetFont(FontName.Ubuntu.Regular);
                default: return QResources.GetFont(FontName.Ubuntu.Regular);
            }
        }
    }
}

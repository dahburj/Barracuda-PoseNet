// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Core;
using UnityEditor;
using UnityEngine;

namespace QuickEditor
{
    public class FontName
    {
        public enum Ubuntu
        {
            Light,
            LightItalic,
            Regular,
            RegularItalic,
            Medium,
            MediumItalic,
            Bold,
            BoldItalic
        }

        public enum Sansation
        {
            Light,
            LightItalic,
            Regular,
            RegularItalic,
            Bold,
            BoldItalic
        }
    }

    public partial class QResources
    {
        //https://fonts.google.com/specimen/Ubuntu?selection.family=Ubuntu
        private static Font
                            _ubuntuLight,
                            _ubuntuLightItalic,
                            _ubuntuRegular,
                            _ubuntuRegularItalic,
                            _ubuntuMedium,
                            _ubuntuMediumItalic,
                            _ubuntuBold,
                            _ubuntuBoldItalic;

        public static Font GetFont(FontName.Ubuntu ubuntu)
        {
            switch(ubuntu)
            {
                case FontName.Ubuntu.Light: if(_ubuntuLight == null) { _ubuntuLight = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-Light") as Font; } return _ubuntuLight;
                case FontName.Ubuntu.LightItalic: if(_ubuntuLightItalic == null) { _ubuntuLightItalic = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-LightItalic") as Font; } return _ubuntuLightItalic;
                case FontName.Ubuntu.Regular: if(_ubuntuRegular == null) { _ubuntuRegular = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-Regular") as Font; } return _ubuntuRegular;
                case FontName.Ubuntu.RegularItalic: if(_ubuntuRegularItalic == null) { _ubuntuRegularItalic = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-Italic") as Font; } return _ubuntuRegularItalic;
                case FontName.Ubuntu.Medium: if(_ubuntuMedium == null) { _ubuntuMedium = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-Medium") as Font; } return _ubuntuMedium;
                case FontName.Ubuntu.MediumItalic: if(_ubuntuMediumItalic == null) { _ubuntuMediumItalic = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-MediumItalic") as Font; } return _ubuntuMediumItalic;
                case FontName.Ubuntu.Bold: if(_ubuntuBold == null) { _ubuntuBold = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-Bold") as Font; } return _ubuntuBold;
                case FontName.Ubuntu.BoldItalic: if(_ubuntuBoldItalic == null) { _ubuntuBoldItalic = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-BoldItalic") as Font; } return _ubuntuBoldItalic;
                default: if(_ubuntuRegular == null) { _ubuntuRegular = Resources.Load("Quick/Fonts/Ubuntu/Ubuntu-Regular") as Font; } return _ubuntuRegular;
            }
        }

        private static Font
                            _sansationLight,
                            _sansationLightItalic,
                            _sansationRegular,
                            _sansationRegularItalic,
                            _sansationBold,
                            _sansationBoldItalic;

        public static Font GetFont(FontName.Sansation sansation)
        {
            switch(sansation)
            {
                case FontName.Sansation.Light: if(_sansationLight == null) { _sansationLight = Resources.Load("Quick/Fonts/Sansation/Sansation-Light") as Font; } return _sansationLight;
                case FontName.Sansation.LightItalic: if(_sansationLightItalic == null) { _sansationLightItalic = Resources.Load("Quick/Fonts/Sansation/Sansation-LightItalic") as Font; } return _sansationLightItalic;
                case FontName.Sansation.Regular: if(_sansationRegular == null) { _sansationRegular = Resources.Load("Quick/Fonts/Sansation/Sansation-Regular") as Font; } return _sansationRegular;
                case FontName.Sansation.RegularItalic: if(_sansationRegularItalic == null) { _sansationRegularItalic = Resources.Load("Quick/Fonts/Sansation/Sansation-RegularItalic") as Font; } return _sansationRegularItalic;
                case FontName.Sansation.Bold: if(_sansationBold == null) { _sansationBold = Resources.Load("Quick/Fonts/Sansation/Sansation-Bold") as Font; } return _sansationBold;
                case FontName.Sansation.BoldItalic: if(_sansationBoldItalic == null) { _sansationBoldItalic = Resources.Load("Quick/Fonts/Sansation/Sansation-BoldItalic") as Font; } return _sansationBoldItalic;
                default: if(_sansationRegular == null) { _sansationRegular = Resources.Load("Quick/Fonts/Sansation/Sansation-Regular") as Font; } return _sansationRegular;
            }
        }

        private static string _IMAGES;
        public static string IMAGES { get { if(string.IsNullOrEmpty(_IMAGES)) { _IMAGES = Q.PATH + "/Images/"; } return _IMAGES; } }

        private static string _BACKGROUNDS;
        public static string BACKGROUNDS { get { if(string.IsNullOrEmpty(_BACKGROUNDS)) { _BACKGROUNDS = IMAGES + "Backgrounds/"; } return _BACKGROUNDS; } }

        private static string _BUTTONS;
        public static string BUTTONS { get { if(string.IsNullOrEmpty(_BUTTONS)) { _BUTTONS = IMAGES + "Buttons/"; } return _BUTTONS; } }

        private static string _BARS;
        public static string BARS { get { if(string.IsNullOrEmpty(_BARS)) { _BARS = IMAGES + "Bars/"; } return _BARS; } }

        private static string _TITLES;
        public static string TITLES { get { if(string.IsNullOrEmpty(_TITLES)) { _TITLES = IMAGES + "Titles/"; } return _TITLES; } }

        private static string _LINES;
        public static string LINES { get { if(string.IsNullOrEmpty(_LINES)) { _LINES = IMAGES + "Lines/"; } return _LINES; } }

        private static string _INFOMESSAGES;
        public static string INFOMESSAGES { get { if(string.IsNullOrEmpty(_INFOMESSAGES)) { _INFOMESSAGES = IMAGES + "InfoMessages/"; } return _INFOMESSAGES; } }

        private static string _ICONS;
        public static string ICONS { get { if(string.IsNullOrEmpty(_ICONS)) { _ICONS = IMAGES + "Icons/"; } return _ICONS; } }

        public static string IsProSkinTag = EditorGUIUtility.isProSkin ? "Dark" : "";

        public static QTexture backgroundSidebar = new QTexture(BACKGROUNDS, "backgroundSidebar" + IsProSkinTag);
        public static QTexture backgroundContentShadowLeft = new QTexture(BACKGROUNDS, "backgroundContentShadowLeft" + IsProSkinTag);
        public static QTexture backgroundContent = new QTexture(BACKGROUNDS, "backgroundContent" + IsProSkinTag);

        public static QTexture backgroundHighGray = new QTexture(BACKGROUNDS, "backgroundHighGray" + IsProSkinTag);
        public static QTexture backgroundHighGreen = new QTexture(BACKGROUNDS, "backgroundHighGreen" + IsProSkinTag);
        public static QTexture backgroundHighBlue = new QTexture(BACKGROUNDS, "backgroundHighBlue" + IsProSkinTag);
        public static QTexture backgroundHighOrange = new QTexture(BACKGROUNDS, "backgroundHighOrange" + IsProSkinTag);
        public static QTexture backgroundHighRed = new QTexture(BACKGROUNDS, "backgroundHighRed" + IsProSkinTag);
        public static QTexture backgroundHighPurple = new QTexture(BACKGROUNDS, "backgroundHighPurple" + IsProSkinTag);

        public static QTexture backgroundLowGray = new QTexture(BACKGROUNDS, "backgroundLowGray" + IsProSkinTag);
        public static QTexture backgroundLowGreen = new QTexture(BACKGROUNDS, "backgroundLowGreen" + IsProSkinTag);
        public static QTexture backgroundLowBlue = new QTexture(BACKGROUNDS, "backgroundLowBlue" + IsProSkinTag);
        public static QTexture backgroundLowOrange = new QTexture(BACKGROUNDS, "backgroundLowOrange" + IsProSkinTag);
        public static QTexture backgroundLowRed = new QTexture(BACKGROUNDS, "backgroundLowRed" + IsProSkinTag);
        public static QTexture backgroundLowPurple = new QTexture(BACKGROUNDS, "backgroundLowPurple" + IsProSkinTag);


        public static QTexture btnMinus = new QTexture(BUTTONS, "btnMinus" + IsProSkinTag);
        public static QTexture btnPlus = new QTexture(BUTTONS, "btnPlus" + IsProSkinTag);
        public static QTexture btnCancel = new QTexture(BUTTONS, "btnCancel" + IsProSkinTag);
        public static QTexture btnOk = new QTexture(BUTTONS, "btnOk" + IsProSkinTag);
        public static QTexture btnLock = new QTexture(BUTTONS, "btnLock" + IsProSkinTag);
        public static QTexture btnLockSelected = new QTexture(BUTTONS, "btnLockSelected" + IsProSkinTag);
        public static QTexture btnSave = new QTexture(BUTTONS, "btnSave" + IsProSkinTag);
        public static QTexture btnSaveSelected = new QTexture(BUTTONS, "btnSaveSelected" + IsProSkinTag);
        public static QTexture btnReset = new QTexture(BUTTONS, "btnReset" + IsProSkinTag);
        public static QTexture btnGraph = new QTexture(BUTTONS, "btnGraph" + IsProSkinTag);
        public static QTexture btnGraphSelected = new QTexture(BUTTONS, "btnGraphSelected" + IsProSkinTag);
        public static QTexture btnData = new QTexture(BUTTONS, "btnData" + IsProSkinTag);
        public static QTexture btnPlay = new QTexture(BUTTONS, "btnPlay" + IsProSkinTag);
        public static QTexture btnStop = new QTexture(BUTTONS, "btnStop" + IsProSkinTag);

        public static QTexture linkButtonLink = new QTexture(BUTTONS, "linkButtonLink" + IsProSkinTag);
        public static QTexture linkButtonUnity = new QTexture(BUTTONS, "linkButtonUnity" + IsProSkinTag);
        public static QTexture linkButtonYouTube = new QTexture(BUTTONS, "linkButtonYouTube" + IsProSkinTag);
        public static QTexture linkButtonManual = new QTexture(BUTTONS, "linkButtonManual" + IsProSkinTag);

        public static QTexture btnSlicedGray = new QTexture(BUTTONS, "btnSlicedGray" + IsProSkinTag);
        public static QTexture btnSlicedGreen = new QTexture(BUTTONS, "btnSlicedGreen" + IsProSkinTag);
        public static QTexture btnSlicedBlue = new QTexture(BUTTONS, "btnSlicedBlue" + IsProSkinTag);
        public static QTexture btnSlicedOrange = new QTexture(BUTTONS, "btnSlicedOrange" + IsProSkinTag);
        public static QTexture btnSlicedRed = new QTexture(BUTTONS, "btnSlicedRed" + IsProSkinTag);
        public static QTexture btnSlicedPurple = new QTexture(BUTTONS, "btnSlicedPurple" + IsProSkinTag);

        public static QTexture btnGhostGray = new QTexture(BUTTONS, "btnGhostGray" + IsProSkinTag);
        public static QTexture btnGhostGreen = new QTexture(BUTTONS, "btnGhostGreen" + IsProSkinTag);
        public static QTexture btnGhostBlue = new QTexture(BUTTONS, "btnGhostBlue" + IsProSkinTag);
        public static QTexture btnGhostOrange = new QTexture(BUTTONS, "btnGhostOrange" + IsProSkinTag);
        public static QTexture btnGhostRed = new QTexture(BUTTONS, "btnGhostRed" + IsProSkinTag);
        public static QTexture btnGhostPurple = new QTexture(BUTTONS, "btnGhostPurple" + IsProSkinTag);

        public static QTexture caretGray0 = new QTexture(BARS, "caretGray0" + IsProSkinTag);
        public static QTexture caretGray1 = new QTexture(BARS, "caretGray1" + IsProSkinTag);
        public static QTexture caretGray2 = new QTexture(BARS, "caretGray2" + IsProSkinTag);
        public static QTexture caretGray3 = new QTexture(BARS, "caretGray3" + IsProSkinTag);
        public static QTexture caretGray4 = new QTexture(BARS, "caretGray4" + IsProSkinTag);
        public static QTexture caretGray5 = new QTexture(BARS, "caretGray5" + IsProSkinTag);
        public static QTexture caretGray6 = new QTexture(BARS, "caretGray6" + IsProSkinTag);
        public static QTexture caretGray7 = new QTexture(BARS, "caretGray7" + IsProSkinTag);
        public static QTexture caretGray8 = new QTexture(BARS, "caretGray8" + IsProSkinTag);
        public static QTexture caretGray9 = new QTexture(BARS, "caretGray9" + IsProSkinTag);
        public static QTexture caretGray10 = new QTexture(BARS, "caretGray10" + IsProSkinTag);

        public static QTexture titleGhostGray = new QTexture(TITLES, "titleGhostGray" + IsProSkinTag);
        public static QTexture titleGhostGreen = new QTexture(TITLES, "titleGhostGreen" + IsProSkinTag);
        public static QTexture titleGhostBlue = new QTexture(TITLES, "titleGhostBlue" + IsProSkinTag);
        public static QTexture titleGhostOrange = new QTexture(TITLES, "titleGhostOrange" + IsProSkinTag);
        public static QTexture titleGhostRed = new QTexture(TITLES, "titleGhostRed" + IsProSkinTag);
        public static QTexture titleGhostPurple = new QTexture(TITLES, "titleGhostPurple" + IsProSkinTag);

        public static QTexture lineGray = new QTexture(LINES, "lineGray" + IsProSkinTag);
        public static QTexture lineGreen = new QTexture(LINES, "lineGreen" + IsProSkinTag);
        public static QTexture lineBlue = new QTexture(LINES, "lineBlue" + IsProSkinTag);
        public static QTexture lineOrange = new QTexture(LINES, "lineOrange" + IsProSkinTag);
        public static QTexture lineRed = new QTexture(LINES, "lineRed" + IsProSkinTag);
        public static QTexture linePurple = new QTexture(LINES, "linePurple" + IsProSkinTag);

        public static QTexture infoMessageTitleSuccess = new QTexture(INFOMESSAGES, "infoMessageTitleSuccess" + IsProSkinTag);
        public static QTexture infoMessageTitleWarning = new QTexture(INFOMESSAGES, "infoMessageTitleWarning" + IsProSkinTag);
        public static QTexture infoMessageTitleError = new QTexture(INFOMESSAGES, "infoMessageTitleError" + IsProSkinTag);
        public static QTexture infoMessageTitleInfo = new QTexture(INFOMESSAGES, "infoMessageTitleInfo" + IsProSkinTag);
        public static QTexture infoMessageTitleHelp = new QTexture(INFOMESSAGES, "infoMessageTitleHelp" + IsProSkinTag);

        public static QTexture infoMessageMessageSuccess = new QTexture(INFOMESSAGES, "infoMessageMessageSuccess" + IsProSkinTag);
        public static QTexture infoMessageMessageWarning = new QTexture(INFOMESSAGES, "infoMessageMessageWarning" + IsProSkinTag);
        public static QTexture infoMessageMessageError = new QTexture(INFOMESSAGES, "infoMessageMessageError" + IsProSkinTag);
        public static QTexture infoMessageMessageInfo = new QTexture(INFOMESSAGES, "infoMessageMessageInfo" + IsProSkinTag);
        public static QTexture infoMessageMessageHelp = new QTexture(INFOMESSAGES, "infoMessageMessageHelp" + IsProSkinTag);

        public static QTexture iconOk = new QTexture(ICONS, "iconOk" + IsProSkinTag);
        public static QTexture iconWarning = new QTexture(ICONS, "iconWarning" + IsProSkinTag);
        public static QTexture iconError = new QTexture(ICONS, "iconError" + IsProSkinTag);
        public static QTexture iconInfo = new QTexture(ICONS, "iconInfo" + IsProSkinTag);

        public static QTexture iconNumberGray = new QTexture(ICONS, "iconNumberGray" + IsProSkinTag);
        public static QTexture iconNumberGreen = new QTexture(ICONS, "iconNumberGreen" + IsProSkinTag);
        public static QTexture iconNumberBlue = new QTexture(ICONS, "iconNumberBlue" + IsProSkinTag);
        public static QTexture iconArrowUpGray = new QTexture(ICONS, "iconArrowUpGray" + IsProSkinTag);
        public static QTexture iconArrowUpGreen = new QTexture(ICONS, "iconArrowUpGreen" + IsProSkinTag);
        public static QTexture iconArrowUpBlue = new QTexture(ICONS, "iconArrowUpBlue" + IsProSkinTag);
        public static QTexture iconMaxGray = new QTexture(ICONS, "iconMaxGray" + IsProSkinTag);
        public static QTexture iconMaxGreen = new QTexture(ICONS, "iconMaxGreen" + IsProSkinTag);
        public static QTexture iconMaxBlue = new QTexture(ICONS, "iconMaxBlue" + IsProSkinTag);
        public static QTexture iconRecycleGray = new QTexture(ICONS, "iconRecycleGray" + IsProSkinTag);
        public static QTexture iconRecycleGreen = new QTexture(ICONS, "iconRecycleGreen" + IsProSkinTag);
        public static QTexture iconRecycleBlue = new QTexture(ICONS, "iconRecycleBlue" + IsProSkinTag);
        public static QTexture iconTimeGray = new QTexture(ICONS, "iconTimeGray" + IsProSkinTag);
        public static QTexture iconTimeGreen = new QTexture(ICONS, "iconTimeGreen" + IsProSkinTag);
        public static QTexture iconTimeBlue = new QTexture(ICONS, "iconTimeBlue" + IsProSkinTag);
        public static QTexture iconDebugGray = new QTexture(ICONS, "iconDebugGray" + IsProSkinTag);
        public static QTexture iconDebugGreen = new QTexture(ICONS, "iconDebugGreen" + IsProSkinTag);
        public static QTexture iconDebugBlue = new QTexture(ICONS, "iconDebugBlue" + IsProSkinTag);


        public static QGeneratedTexture TransparentBackground = new QGeneratedTexture(new QColor(0, 0, 0, 0));

        public static QGeneratedTexture WhiteBackground = new QGeneratedTexture(new QColor(QuickEngine.QColors.WhiteDark.Color, QuickEngine.QColors.WhiteLight.Color));

        public static QGeneratedTexture HelpBackground = new QGeneratedTexture(new QColor(QuickEngine.QColors.WhiteDark.Color, QuickEngine.QColors.WhiteLight.Color), new QColor(QColors.Help.Dark, QColors.Help.Light));
        public static QGeneratedTexture InfoBackground = new QGeneratedTexture(new QColor(QuickEngine.QColors.WhiteDark.Color, QuickEngine.QColors.WhiteLight.Color), new QColor(QColors.Info.Dark, QColors.Info.Light));
        public static QGeneratedTexture WarningBackground = new QGeneratedTexture(new QColor(QuickEngine.QColors.WhiteDark.Color, QuickEngine.QColors.WhiteLight.Color), new QColor(QColors.Warning.Dark, QColors.Warning.Light));
        public static QGeneratedTexture ErrorBackground = new QGeneratedTexture(new QColor(QuickEngine.QColors.WhiteDark.Color, QuickEngine.QColors.WhiteLight.Color), new QColor(QColors.Error.Dark, QColors.Error.Light));




    }
}

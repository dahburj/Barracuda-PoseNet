// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace QuickEditor
{
    public class QWindow : EditorWindow
    {
        /*
https://docs.unity3d.com/ScriptReference/MenuItem.html
MenuItem
class in UnityEditorOther Versions Leave feedback
Description
The MenuItem attribute allows you to add menu items to the main menu and inspector context menus.

The MenuItem attribute turns any static function into a menu command. Only static functions can use the MenuItem attribute.

To create a hotkey you can use the following special characters: % (ctrl on Windows, cmd on macOS), # (shift), & (alt). If no special modifier key combinations are required the key can be given after an underscore. For example to create a menu with hotkey shift-alt-g use "MyMenu/Do Something #&g". To create a menu with hotkey g and no key modifiers pressed use "MyMenu/Do Something _g".

Some special keyboard keys are supported as hotkeys, for example "#LEFT" would map to shift-left. The keys supported like this are: LEFT, RIGHT, UP, DOWN, F1 .. F12, HOME, END, PGUP, PGDN.

A hotkey text must be preceded with a space character ("MyMenu/Do_g" won't be interpreted as hotkey, while "MyMenu/Do _g" will).

         */

        #region Colors
        private Color tempColor = Color.white;
        private Color tempContentColor = Color.white;
        private Color tempBackgroundColor = Color.white;

        public void SaveColors(bool resetColors = false)
        {
            tempColor = GUI.color;
            tempContentColor = GUI.contentColor;
            tempBackgroundColor = GUI.backgroundColor;
            if (resetColors) { QUI.ResetColors(); }
        }

        public void RestoreColors()
        {
            GUI.color = tempColor;
            GUI.contentColor = tempContentColor;
            GUI.backgroundColor = tempBackgroundColor;
        }
        #endregion

        #region Dimensions
        public const float WIDTH_512 = 512f;
        public const float WIDTH_256 = 256f;
        public const float WIDTH_128 = 128f;
        public const float WIDTH_64 = 64f;


        public const float HEIGHT_8 = 8f;
        /// <summary>
        /// This is the EditorGUIUtility.singleLineHeight value
        /// </summary> 
        public const float HEIGHT_16 = 16f;
        public const float HEIGHT_24 = 24f;
        public const float HEIGHT_36 = 36f;

        public const float INDENT_24 = 24f;

        /// <summary>
        /// This is the EditorGUIUtility.standardVerticalSpacing value
        /// </summary>
        public const float SPACE_2 = 2;
        public const float SPACE_4 = 4;
        public const float SPACE_8 = 8;
        /// <summary>
        /// This is the EditorGUIUtility.singleLineHeight value
        /// </summary> 
        public const float SPACE_16 = 16;
        #endregion

        #region QLabel
        private QLabel _qLabel;
        public QLabel QLabel { get { if(_qLabel == null) { _qLabel = new QLabel(); } return _qLabel; } }

        public Dictionary<string, QLabel> qLabels;
        public QLabel GetQLabel(string text, Style.Text style = Style.Text.Normal)
        {
            if(qLabels == null) { qLabels = new Dictionary<string, QLabel>(); }
            if(!qLabels.ContainsKey(text)) { qLabels.Add(text, new QLabel(text, style)); }
            return qLabels[text];
        }
        #endregion

        public Dictionary<string, InfoMessage> infoMessage;
        protected void AddInfoMessage(string key, string title, string message, InfoMessageType type)
        {
            if (infoMessage == null) { infoMessage = new Dictionary<string, InfoMessage>(); }
            infoMessage.Add(key, new InfoMessage()
            {
                title = title,
                message = message,
                show = new AnimBool(false, Repaint),
                type = type
            });
        }
        protected void DrawInfoMessage(string key)
        {
            DrawInfoMessage(key, -1);
        }
        protected void DrawInfoMessage(string key, float width)
        {
            if (infoMessage == null) { Debug.Log("The infoMessage database is null."); return; }
            if (infoMessage.Count == 0) { Debug.Log("The infoMessage database is empty. Add the key to the database before you try to darw its message."); return; }
            if (!infoMessage.ContainsKey(key)) { Debug.Log("The infoMessage database does not contain any key named '" + key + "'. Check if it was added to the database or if you spelled the key wrong."); return; }
            QUI.DrawInfoMessage(infoMessage[key], width);
        }

        #region RequiresConstantRepaint
        public bool requiresContantRepaint = false;
        private void OnInspectorUpdate()
        {
            if (requiresContantRepaint) { Repaint(); }
        }
        #endregion

        public void CenterWindow()
        {
            var pos = position;
            pos.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
            position = pos;
        }
    }
}

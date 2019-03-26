// Copyri// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuickEditor
{
    public class QEditor : Editor
    {
        private bool lockEditor = false;
        public bool IsEditorLocked { get { return lockEditor; } }

        public bool UseFixedUpdate = false;
        public float UpdateInterval = 2f;
        double startTime;
        double elapsedInterval = 0;

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

        #region Colors
        private Color tempColor = Color.white;
        private Color tempContentColor = Color.white;
        private Color tempBackgroundColor = Color.white;

        public void SaveColors(bool resetColors = false)
        {
            tempColor = GUI.color;
            tempContentColor = GUI.contentColor;
            tempBackgroundColor = GUI.backgroundColor;
            if(resetColors) { QUI.ResetColors(); }
        }

        public void RestoreColors()
        {
            GUI.color = tempColor;
            GUI.contentColor = tempContentColor;
            GUI.backgroundColor = tempBackgroundColor;
        }
        #endregion

        #region Dimensions
        public const float WIDTH_420 = 420f;
        public const float WIDTH_210 = 210f;
        public const float WIDTH_140 = 140f;
        public const float WIDTH_105 = 105f;

        public const float HEIGHT_8 = 8f;
        /// <summary>
        /// This is the EditorGUIUtility.singleLineHeight value
        /// </summary> 
        public const float HEIGHT_16 = 16f;
        public const float HEIGHT_24 = 24f;
        public const float HEIGHT_36 = 36f;
        public const float HEIGHT_42 = 42f;

        public const float INDENT_24 = 24f;

        /// <summary>
        /// This is the EditorGUIUtility.standardVerticalSpacing value
        /// </summary>
        public const int SPACE_2 = 2;
        public const int SPACE_4 = 4;
        public const int SPACE_8 = 8;
        /// <summary>
        /// This is the EditorGUIUtility.singleLineHeight value
        /// </summary> 
        public const int SPACE_16 = 16;
        #endregion

        #region RequiresConstantRepaint
        public bool requiresContantRepaint = false;
        public override bool RequiresConstantRepaint()
        {
            return requiresContantRepaint;
        }
        #endregion

        protected virtual void InitAnimBools() { }
        protected virtual void GenerateInfoMessages() { infoMessage = new Dictionary<string, InfoMessage>(); }
        protected virtual void SerializedObjectFindProperties() { }
        protected virtual void InitializeVariables() { }

        protected virtual void OnEnable()
        {
            startTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += Update;

            SerializedObjectFindProperties();
            InitAnimBools();
            InitializeVariables();
            GenerateInfoMessages();
        }

        protected virtual void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        private void OnDestroy()
        {
            EditorApplication.update -= Update;
        }

        protected virtual void Update()
        {
            if(!UseFixedUpdate) { return; }
            if(EditorApplication.timeSinceStartup > (startTime + elapsedInterval + UpdateInterval))
            {
                elapsedInterval += UpdateInterval;
                FixedUpdate();
            }
        }

        protected virtual void FixedUpdate() { }

        /// <summary>
        /// Locks the editor and then Applies Modified Properties to the serializedObject.
        /// </summary>
        public void LockEditor()
        {
            lockEditor = true;
            serializedObject.ApplyModifiedProperties();
        }
        /// <summary>
        /// Applies Modified Properties to the serializedObject and then unlocks the editor.
        /// </summary>
        public void UnlockEditor()
        {
            serializedObject.ApplyModifiedProperties();
            lockEditor = false;
        }

        /// <summary>
        /// Displays a modal dialog.
        /// </summary>
        /// <param name="title"> The title of the message box.</param>
        /// <param name="message">The text of the message.</param>
        /// <param name="ok">Label displayed on the OK dialog button.</param>
        protected bool DisplayDialog(string title, string message, string ok, bool enableEditorLock = true)
        {
            if(enableEditorLock)
            {
                LockEditor();
            }
            return QUI.DisplayDialog(title, message, ok);
        }

        /// <summary>
        /// Displays a modal dialog.
        /// </summary>
        /// <param name="title"> The title of the message box.</param>
        /// <param name="message">The text of the message.</param>
        /// <param name="ok">Label displayed on the OK dialog button.</param>
        /// <param name="cancel">Label displayed on the Cancel dialog button.</param>
        protected bool DisplayDialog(string title, string message, string ok, string cancel, bool enableEditorLock = true)
        {
            if(enableEditorLock)
            {
                LockEditor();
            }
            return EditorUtility.DisplayDialog(title, message, ok, cancel);
        }

        protected void DrawHeader(Texture texture, float width = WIDTH_420, float height = HEIGHT_36)
        {
            QUI.Space(SPACE_4);
            SaveColors(true);
            QUI.DrawTexture(texture, width, height);
            RestoreColors();
            QUI.Space(SPACE_4);
        }
        protected void DrawInfoMessage(string key, float width)
        {
            if(infoMessage == null) { Debug.Log("The infoMessage database is null."); return; }
            if(infoMessage.Count == 0) { Debug.Log("The infoMessage database is empty. Add the key to the database before you try to darw its message."); return; }
            if(!infoMessage.ContainsKey(key)) { Debug.Log("The infoMessage database does not contain any key named '" + key + "'. Check if it was added to the database or if you spelled the key wrong."); return; }
            QUI.DrawInfoMessage(infoMessage[key], width);
        }
    }
}

// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Extensions;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickEditor
{
    public enum IconPosition { Left, Right }

    #region InfoMessage
    /// <summary>
    /// Enum used by the DrawInfoMessage in order to show an icon and set the proper style for each InfoMessage type.
    /// </summary>
    public enum InfoMessageType
    {
        Help,
        Info,
        Warning,
        Error,
        Success
    }
    /// <summary>
    /// Stores an AnimBool (used for show/hide animation), a title (optional), and a message.
    /// </summary>
    public class InfoMessage
    {
        /// <summary>
        /// Used to toggle show/hide of the message via an editor animation.
        /// </summary>
        public AnimBool show = new AnimBool(false);
        /// <summary>
        /// (optional) The title will appear as a bold text when displyed through DrawInfoMessage.
        /// </summary>
        public string title = string.Empty;
        /// <summary>
        /// The main text of the InfoMessage. This will get displayed with or without a title.
        /// </summary>
        public string message = string.Empty;
        /// <summary>
        /// Selects the proper style for each InfoMessage type.
        /// </summary>
        public InfoMessageType type = InfoMessageType.Info;
    }
    #endregion

    public class QLabel
    {
        private string _text;
        private Style.Text _style;
        private Vector2 _size;
        private GUIContent _content;

        public string text { get { return _text; } set { UpdateText(value); } }
        public Style.Text style { get { return _style; } set { UpdateStyle(value); } }
        public Vector2 size { get { return _size; } }
        public float x { get { return _size.x; } }
        public float y { get { return _size.y; } }
        public GUIContent content { get { return _content; } }

        public QLabel(string text, Style.Text style = Style.Text.Normal)
        {
            _content = new GUIContent(text);
            _size = QStyles.CalcSize(_content, style);
            _text = text;
        }

        public QLabel()
        {
            _text = string.Empty;
            _style = Style.Text.Normal;
            _content = new GUIContent(_text);
            _size = QStyles.CalcSize(_content, _style);
        }

        private void UpdateText(string text)
        {
            _text = text;
            _content.text = text;
            _size = QStyles.CalcSize(_content, _style);
        }

        private void UpdateStyle(Style.Text style)
        {
            _style = style;
            _size = QStyles.CalcSize(_content, style);
        }
    }

    [System.Serializable]
    public class LinkButtonData
    {
        public string text;
        public string url;
        public Style.LinkButton linkButton;
    }

    public static class QUI
    {
        public static Color AccentColorBlue { get { return QUI.IsProSkin ? QColors.Blue.Color : QColors.BlueLight.Color; } }
        public static Color AccentColorGreen { get { return QUI.IsProSkin ? QColors.Green.Color : QColors.GreenLight.Color; } }
        public static Color AccentColorOrange { get { return QUI.IsProSkin ? QColors.Orange.Color : QColors.OrangeLight.Color; } }
        public static Color AccentColorRed { get { return QUI.IsProSkin ? QColors.Red.Color : QColors.RedLight.Color; } }
        public static Color AccentColorPurple { get { return QUI.IsProSkin ? QColors.Purple.Color : QColors.PurpleLight.Color; } }
        public static Color AccentColorGray { get { return QUI.IsProSkin ? QColors.UnityLight.Color : QColors.UnityLight.Color; } }

        #region QLabel
        private static QLabel _qLabel;
        public static QLabel QLabel { get { if(_qLabel == null) { _qLabel = new QLabel(); } return _qLabel; } }

        public static Dictionary<string, QLabel> qLabels;
        public static QLabel GetQLabel(string text, Style.Text style = Style.Text.Normal)
        {
            if(qLabels == null) { qLabels = new Dictionary<string, QLabel>(); }
            if(!qLabels.ContainsKey(text)) { qLabels.Add(text, new QLabel(text, style)); }
            return qLabels[text];
        }
        #endregion

        private static bool result;
        private static string tempString;
        private static float tempFloat;

        public static bool IsProSkin { get { return EditorGUIUtility.isProSkin; } }

        public static float SingleLineHeight { get { return EditorGUIUtility.singleLineHeight; } }

        public static void SetNextControlName(string name) { GUI.SetNextControlName(name); }
        public static string GetNameOfFocusedControl() { return GUI.GetNameOfFocusedControl(); }

        public static void FocusControl(string name) { GUI.FocusControl(name); }
        public static void FocusTextInControl(string name) { EditorGUI.FocusTextInControl(name); }

        #region DetectKey

        public static bool DetectKey(Event @event, KeyCode keyCode, EventType eventType)
        {
            result = false;
            result = @event.isKey
                     && @event.keyCode == keyCode
                     && @event.type == eventType;
            if(result) { @event.Use(); }
            return result;
        }

        public static bool DetectKey(Event @event, KeyCode keyCode, EventType eventType, string focusedControlName) { return GUI.GetNameOfFocusedControl().Equals(focusedControlName) && DetectKey(@event, keyCode, eventType); }

        public static bool DetectKeyDown(Event @event, KeyCode keyCode) { return DetectKey(@event, keyCode, EventType.KeyDown); }
        public static bool DetectKeyDown(Event @event, KeyCode keyCode, string focusedControlName) { return GUI.GetNameOfFocusedControl().Equals(focusedControlName) && DetectKeyDown(@event, keyCode); }

        public static bool DetectKeyUp(Event @event, KeyCode keyCode) { return DetectKey(@event, keyCode, EventType.KeyUp); }
        public static bool DetectKeyUp(Event @event, KeyCode keyCode, string focusedControlName) { return GUI.GetNameOfFocusedControl().Equals(focusedControlName) && DetectKeyUp(@event, keyCode); }

        public static bool DetectKeyDownCombo(Event @event, EventModifiers modifier, KeyCode key) { return @event.modifiers == modifier && DetectKeyDown(@event, key); }
        public static bool DetectKeyUpCombo(Event @event, EventModifiers modifier, KeyCode key) { return @event.modifiers == modifier && DetectKeyUp(@event, key); }

        #endregion

        #region EditorDialog
        /// <summary>
        /// Displays a modal dialog.
        /// </summary>
        /// <param name="title"> The title of the message box.</param>
        /// <param name="message">The text of the message.</param>
        /// <param name="ok">Label displayed on the OK dialog button.</param>
        public static bool DisplayDialog(string title, string message, string ok)
        {
            return EditorUtility.DisplayDialog(title, message, ok);
        }

        /// <summary>
        /// Displays a modal dialog.
        /// </summary>
        /// <param name="title"> The title of the message box.</param>
        /// <param name="message">The text of the message.</param>
        /// <param name="ok">Label displayed on the OK dialog button.</param>
        /// <param name="cancel">Label displayed on the Cancel dialog button.</param>
        public static bool DisplayDialog(string title, string message, string ok, string cancel)
        {
            return EditorUtility.DisplayDialog(title, message, ok, cancel);
        }
        #endregion

        #region MarkSceneDirty
        /// <summary>
        /// Marks the current active scene as dirty, prompting a save. To mark all the currently opened scenes as dirty, just pass markAllScenesDirty as true.
        /// </summary>
        public static void MarkSceneDirty(bool markAllScenesDirty = false)
        {
            if(EditorApplication.isPlaying) { return; }
            if(markAllScenesDirty) { EditorSceneManager.MarkAllScenesDirty(); }
            else { EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene()); }
        }
        #endregion

        #region ResetKeyboardFocus
        /// <summary>
        /// Sets the controlID that has keybard control to 0
        /// </summary>
        public static void ResetKeyboardFocus()
        {
            GUIUtility.keyboardControl = 0;
        }
        #endregion

        #region SetDirty
        /// <summary>
        /// Marks target object as dirty. (Only suitable for non-scene objects).
        /// </summary>
        /// <param name="target">The object to mark as dirty.</param>
        public static void SetDirty(Object target)
        {
            EditorUtility.SetDirty(target);
        }
        #endregion

        #region RepaintAllViews / RepaintAnimationWindow / RepaintHierarchyWindow / RepaintProjectWindow
        /// <summary>
        /// Repaints ALL the views. Helps with updating the SceneViews and GameViews from a custom EditorWindow. This is done without the need to have these windows in focus.
        /// </summary>
        public static void RepaintAllViews()
        {
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        /// <summary>
        ///  Can be used to ensure repaint of the AnimationWindow.
        /// </summary>
        public static void RepaintAnimationWindow()
        {
            EditorApplication.RepaintAnimationWindow();
        }

        /// <summary>
        ///  Can be used to ensure repaint of the HierarchyWindow.
        /// </summary>
        public static void RepaintHierarchyWindow()
        {
            EditorApplication.RepaintHierarchyWindow();
        }

        /// <summary>
        ///  Can be used to ensure repaint of the ProjectWindow.
        /// </summary>
        public static void RepaintProjectWindow()
        {
            EditorApplication.RepaintProjectWindow();
        }
        #endregion

        #region ExitGUI
        public static void ExitGUI()
        {
            GUIUtility.ExitGUI();
        }
        #endregion;

        #region IsPersistent
        /// <summary>
        /// Determines if an object is stored on disk.
        /// Typically assets like prefabs, textures, audio clips, animation clips, materials
        /// are stored on disk.Returns false if the object lives in the scene. Typically
        /// this is a game object or component but it could also be a material that was created
        /// from code and not stored in an asset but instead stored in the scene.
        /// </summary>
        /// <param name="target">The object you want to test.</param>
        /// <returns>Returns false if it's a scene object.</returns>
        public static bool IsPersistent(Object target)
        {
            if(target == null) { return false; }
            return EditorUtility.IsPersistent(target);
        }
        #endregion

        #region GetLastRect
        /// <summary>
        ///  Get the rectangle last used by GUILayout for a control.
        /// </summary>
        /// <returns>The last used rectangle.</returns>
        public static Rect GetLastRect()
        {
            return GUILayoutUtility.GetLastRect();
        }
        #endregion

        #region BeginChangeCheck / EndChangeCheck
        /// <summary>
        /// Check if any control was changed inside a block of code.
        /// When needing to check if GUI.changed is set to true inside a block of code, wrap
        /// the code inside BeginChangeCheck () and EndChangeCheck () like this:
        /// EndChangeCheck will only return true if GUI.changed was set to true inside the
        /// block, but GUI.changed will be true afterwards both if it was set to true inside
        /// and if it was already true to begin with.
        /// </summary>
        public static void BeginChangeCheck()
        {
            EditorGUI.BeginChangeCheck();
        }

        /// <summary>
        /// Ends a change check started with BeginChangeCheck ().
        /// </summary>
        /// <returns>True if GUI.changed was set to true, otherwise false.</returns>
        public static bool EndChangeCheck()
        {
            return EditorGUI.EndChangeCheck();
        }
        #endregion

        #region ResetColors
        /// <summary>
        /// Resets all the GUI colors to their default values
        /// </summary>
        public static void ResetColors() { GUI.color = Color.white; GUI.contentColor = Color.white; GUI.backgroundColor = Color.white; }
        #endregion

        #region GUI.color
        /// <summary>
        /// Set the GUI.color value
        /// </summary>
        public static void SetGUIColor(Color color) { GUI.color = color; }
        /// <summary>
        /// Set the GUI.color value, taking into account if the Editor skin is set to Dark or Light.
        /// </summary>
        /// <param name="colorDark">Dark skin color</param>
        /// <param name="colorLight">Light skin color</param>
        public static void SetGUIColor(Color colorDark, Color colorLight) { GUI.color = IsProSkin ? colorDark : colorLight; }
        /// <summary>
        /// Returns the current value of GUI.color
        /// </summary>
        public static Color GetGUIColor { get { return GUI.color; } }
        #endregion
        #region GUI.contentColor
        /// <summary>
        /// Set the GUI.contentColor value
        /// </summary>
        public static void SetGUIContentColor(Color color) { GUI.contentColor = color; }
        /// <summary>
        /// Set the GUI.contentColor value, taking into account if the Editor skin is set to Dark or Light.
        /// </summary>
        /// <param name="colorDark">Dark skin color</param>
        /// <param name="colorLight">Light skin color</param>
        public static void SetGUIContentColor(Color colorDark, Color colorLight) { GUI.contentColor = IsProSkin ? colorDark : colorLight; }
        /// <summary>
        /// Returns the current value of GUI.contentColor
        /// </summary>
        public static Color GetGUIContentColor { get { return GUI.contentColor; } }
        #endregion
        #region GUI.backgroundColor
        /// <summary>
        /// Set the GUI.backgroundColor value
        /// </summary>
        public static void SetGUIBackgroundColor(Color color) { GUI.backgroundColor = color; }
        /// <summary>
        /// Set the GUI.backgroundColor value, taking into account if the Editor skin is set to Dark or Light.
        /// </summary>
        /// <param name="colorDark">Dark skin color</param>
        /// <param name="colorLight">Light skin color</param>
        public static void SetGUIBackgroundColor(Color colorDark, Color colorLight) { GUI.backgroundColor = IsProSkin ? colorDark : colorLight; }
        /// <summary>
        /// Returns the current value of GUI.backgroundColor
        /// </summary>
        public static Color GetGUIBackgroundColor { get { return GUI.backgroundColor; } }
        #endregion

        #region Space / FlexibleSpace
        /// <summary>
        ///Insert a space in the current layout group.
        /// The direction of the space is dependent on the layout group you're currently
        /// in when issuing the command. If in a vertical group, the space will be vertical:
        /// Note: This will override the GUILayout.ExpandWidth and GUILayout.ExpandHeightSpace
        /// of 20px between two buttons.
        /// In horizontal groups, the pixels are measured horizontally:
        /// </summary>
        public static void Space(float pixels)
        {
            try
            {
                GUILayout.Space(pixels);
            }
            catch
            {
                ExitGUI();
            }
        }

        /// <summary>
        /// Insert a flexible space element.
        /// Flexible spaces use up any leftover space in a layout. Note: This will override
        /// the GUILayout.ExpandWidth and GUILayout.ExpandHeightFlexible Space in a GUILayout
        /// Area.
        /// </summary>
        public static void FlexibleSpace()
        {
            //try catch addded to fix OSX issues
            try
            {
                GUILayout.FlexibleSpace();
            }
            catch
            {
                ExitGUI();
            }
        }
        #endregion

        #region GetTexture
        /// <summary>
        /// Returns the Texture found at the given path, with the given fileName and the given fileExtension.
        /// </summary>
        /// <param name="fileName">Texture fileName (without the extenstion - eg. '.png')</param>
        /// <param name="path">The path to the texture file</param>
        /// <param name="fileExtension">File extension (default: '.png')</param>
        /// <returns></returns>
        public static Texture GetTexture(string fileName, string path, string fileExtension = ".png") { return AssetDatabase.LoadAssetAtPath<Texture>(path + fileName + fileExtension); }
        #endregion

        #region DrawTexture
        /// <summary>
        /// Draws a Texture at it's default values (width and height).
        /// </summary>
        /// <param name="texture">Target texture.</param>
        public static void DrawTexture(Texture texture)
        {
            if(texture == null) { Debug.Log("[QUI] Texture is null!"); return; }
            Rect rect = GUILayoutUtility.GetRect(0f, 0f);
            rect.width = texture.width;
            rect.height = texture.height;
            GUILayout.Space(rect.height);
            GUI.DrawTexture(rect, texture);
        }

        /// <summary>
        /// Draws a Texture at it's default values (width and height), taking into account if the Editor is set to Dark or Light
        /// </summary>
        /// <param name="textureDark">Target texture for dark skin.</param>
        /// <param name="textureLight">Target texture for light skin.</param>
        public static void DrawTexture(Texture textureDark, Texture textureLight)
        {
            Texture texture = IsProSkin ? textureDark : textureLight;
            if(texture == null) { Debug.Log("[QUI] Texture is null!"); return; }
            Rect rect = GUILayoutUtility.GetRect(0f, 0f);
            rect.width = texture.width;
            rect.height = texture.height;
            GUILayout.Space(rect.height);
            GUI.DrawTexture(rect, texture);
        }

        /// <summary>
        /// Draws a Texture at specified with and height values
        /// </summary>
        /// <param name="texture">Target texture.</param>
        /// <param name="width">Set texture width.</param>
        /// <param name="height">Set texture height.</param>
        public static void DrawTexture(Texture texture, float width, float height)
        {
            if(texture == null) { Debug.Log("[QUI] Texture is null!"); return; }
            Rect rect = GUILayoutUtility.GetRect(0f, 0f);
            rect.width = width;
            rect.height = height;
            GUILayout.Space(rect.height);
            GUI.DrawTexture(rect, texture);
        }

        /// <summary>
        /// Draws a Texture at the specified rect position and size.
        /// <para>This is used mainly for reorderable lists.</para>
        /// </summary>
        /// <param name="rect">Rect's x and y are used for positioning and the width and height are used for sizing the texture.</param>
        /// <param name="texture">Target texture.</param>
        public static void DrawTexture(Rect rect, Texture texture)
        {
            if(texture == null) { Debug.Log("[QUI] Texture is null!"); return; }
            GUI.DrawTexture(rect, texture);
        }

        /// <summary>
        /// Draws a Texture at specified with and height values, taking into account if the Editor is set to Dark or Light
        /// </summary>
        /// <param name="textureDark">Target texture for dark skin.</param>
        /// <param name="textureLight">Target texture for light skin.</param>
        /// <param name="width">Set texture width.</param>
        /// <param name="height">Set texture height.</param>
        public static void DrawTexture(Texture textureDark, Texture textureLight, float width, float height)
        {
            Texture texture = IsProSkin ? textureDark : textureLight;
            if(texture == null) { Debug.Log("[QUI] Texture is null!"); return; }
            Rect rect = GUILayoutUtility.GetRect(0f, 0f);
            rect.width = width;
            rect.height = height;
            GUILayout.Space(rect.height);
            GUI.DrawTexture(rect, texture);
        }
        #endregion

        #region DrawLine
        public static void DrawLine(QColors.Color color, float width)
        {
            switch(color)
            {
                case QColors.Color.Gray: DrawTexture(QResources.lineGray.texture, width, 1); break;
                case QColors.Color.Green: DrawTexture(QResources.lineGreen.texture, width, 1); break;
                case QColors.Color.Blue: DrawTexture(QResources.lineBlue.texture, width, 1); break;
                case QColors.Color.Orange: DrawTexture(QResources.lineOrange.texture, width, 1); break;
                case QColors.Color.Red: DrawTexture(QResources.lineRed.texture, width, 1); break;
                case QColors.Color.Purple: DrawTexture(QResources.linePurple.texture, width, 1); break;
                default: DrawTexture(QResources.lineGray.texture, width, 1); break;
            }
            QUI.Space(1);
        }

        public static void DrawLine(Rect rect, QColors.Color color)
        {
            Rect r = new Rect(rect.x, rect.y, rect.width, 1);
            switch(color)
            {
                case QColors.Color.Gray: DrawTexture(r, QResources.lineGray.texture); break;
                case QColors.Color.Green: DrawTexture(r, QResources.lineGreen.texture); break;
                case QColors.Color.Blue: DrawTexture(r, QResources.lineBlue.texture); break;
                case QColors.Color.Orange: DrawTexture(r, QResources.lineOrange.texture); break;
                case QColors.Color.Red: DrawTexture(r, QResources.lineRed.texture); break;
                case QColors.Color.Purple: DrawTexture(r, QResources.linePurple.texture); break;
                default: DrawTexture(r, QResources.lineGray.texture); break;
            }
        }
        #endregion

        #region BeginArea / EndArea
        /// <summary>
        /// Begin a GUILayout block of GUI controls in a fixed screen area.
        /// </summary>
        public static void BeginArea(Rect screenRect)
        {
            GUILayout.BeginArea(screenRect);
        }
        /// <summary>
        /// Begin a GUILayout block of GUI controls in a fixed screen area.
        /// </summary>
        /// <param name="text">Optional text to display in the area.</param>
        public static void BeginArea(Rect screenRect, string text)
        {
            GUILayout.BeginArea(screenRect, text);
        }
        /// <summary>
        /// Begin a GUILayout block of GUI controls in a fixed screen area.
        /// </summary>
        public static void BeginArea(Rect screenRect, Texture image)
        {
            GUILayout.BeginArea(screenRect, image);
        }
        /// <summary>
        /// Begin a GUILayout block of GUI controls in a fixed screen area.
        /// </summary>
        /// <param name="content">Optional text, image and tooltip top display for this area.</param>
        public static void BeginArea(Rect screenRect, GUIContent content)
        {
            GUILayout.BeginArea(screenRect, content);
        }
        /// <summary>
        /// Begin a GUILayout block of GUI controls in a fixed screen area.
        /// </summary>
        /// <param name="style">The style to use. If left out, the empty GUIStyle (GUIStyle.none) is used, 
        /// giving a transparent background.</param>
        public static void BeginArea(Rect screenRect, GUIStyle style)
        {
            GUILayout.BeginArea(screenRect, style);
        }
        /// <summary>
        /// Begin a GUILayout block of GUI controls in a fixed screen area.
        /// </summary>
        /// <param name="text">Optional text to display in the area.</param>
        /// <param name="style">The style to use. If left out, the empty GUIStyle (GUIStyle.none) is used, 
        /// giving a transparent background.</param>
        public static void BeginArea(Rect screenRect, string text, GUIStyle style)
        {
            GUILayout.BeginArea(screenRect, text, style);
        }
        /// <summary>
        /// Begin a GUILayout block of GUI controls in a fixed screen area.
        /// </summary>
        /// <param name="image">Optional texture to display in the area.</param>
        /// <param name="style">The style to use. If left out, the empty GUIStyle (GUIStyle.none) is used, 
        /// giving a transparent background.</param>
        public static void BeginArea(Rect screenRect, Texture image, GUIStyle style)
        {
            GUILayout.BeginArea(screenRect, image, style);
        }
        /// <summary>
        /// Begin a GUILayout block of GUI controls in a fixed screen area.
        /// </summary>
        /// <param name="content">Optional text, image and tooltip top display for this area.</param>
        /// <param name="style">The style to use. If left out, the empty GUIStyle (GUIStyle.none) is used, 
        /// giving a transparent background.</param>
        public static void BeginArea(Rect screenRect, GUIContent content, GUIStyle style)
        {
            GUILayout.BeginArea(screenRect, content, style);
        }
        /// <summary>
        /// Close a GUILayout block started with BeginArea.
        /// </summary>
        public static void EndArea()
        {
            GUILayout.EndArea();
        }
        #endregion

        #region BeginHorizontal / EndHorizontal
        /// <summary>
        /// Begin a horizontal group and get its rect back.
        /// This is an extension to UnityEngine.GUILayout.BeginHorizontal. It can be used
        /// for making compound controlsHorizontal Compound group.
        /// </summary>
        public static Rect BeginHorizontal()
        {
            return EditorGUILayout.BeginHorizontal();
        }

        /// <summary>
        /// Begin a horizontal group and get its rect back.
        /// This is an extension to UnityEngine.GUILayout.BeginHorizontal. It can be used
        /// for making compound controlsHorizontal Compound group.
        /// </summary>
        /// <param name="width">The Horizontal Compound group's width.</param>
        public static Rect BeginHorizontal(float width)
        {
            return EditorGUILayout.BeginHorizontal(GUILayout.Width(width));
        }

        /// <summary>
        /// Begin a horizontal group and get its rect back.
        /// This is an extension to UnityEngine.GUILayout.BeginHorizontal. It can be used
        /// for making compound controlsHorizontal Compound group.
        /// </summary>
        /// <param name="width">The Horizontal Compound group's width.</param>
        /// <param name="height">The Horizontal Compound group's height.</param>
        public static Rect BeginHorizontal(float width, float height)
        {
            return EditorGUILayout.BeginHorizontal(GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Close a group started with BeginHorizontal.
        /// </summary>
        public static void EndHorizontal()
        {
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region BeginVertical / EndVertical
        /// <summary>
        /// Begin a vertical group and get its rect back.
        /// This is an extension to UnityEngine.GUILayout.BeginVertical. It can be used
        /// for making compound controlsVertical Compound group.
        /// </summary>
        public static Rect BeginVertical()
        {
            return EditorGUILayout.BeginVertical();
        }
        /// <summary>
        /// Begin a vertical group and get its rect back.
        /// This is an extension to UnityEngine.GUILayout.BeginVertical. It can be used
        /// for making compound controlsVertical Compound group.
        /// </summary>
        /// <param name="width">The Vertical Compound group's width.</param>
        public static Rect BeginVertical(float width)
        {
            return EditorGUILayout.BeginVertical(GUILayout.Width(width));
        }
        /// <summary>
        /// Begin a vertical group and get its rect back.
        /// This is an extension to UnityEngine.GUILayout.BeginVertical. It can be used
        /// for making compound controlsVertical Compound group.
        /// </summary>
        /// <param name="width">The Vertical Compound group's width.</param>
        /// <param name="height">The Vertical Compound group's height.</param>
        public static Rect BeginVertical(float width, float height)
        {
            return EditorGUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Close a group started with BeginVertical.
        /// </summary>
        public static void EndVertical()
        {
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region BeginScrollView / EndScrollView
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition);
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, float width)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width));
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <param name="height">The ScrollView's height.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, float width, float height)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));
        }


        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="style">Custom GUIStyle.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, style);
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="style">Custom GUIStyle.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, float width)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, style, GUILayout.Width(width));
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="style">Custom GUIStyle.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <param name="height">The ScrollView's height.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, float width, float height)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, style, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="alwaysShowHorizontal">Should the horizontal bar be always visible.</param>
        /// <param name="alwaysShowVertical">Should the vertical bar be always visible.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical);
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="alwaysShowHorizontal">Should the horizontal bar be always visible.</param>
        /// <param name="alwaysShowVertical">Should the vertical bar be always visible.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, float width)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, GUILayout.Width(width));
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="alwaysShowHorizontal">Should the horizontal bar be always visible.</param>
        /// <param name="alwaysShowVertical">Should the vertical bar be always visible.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <param name="height">The ScrollView's height.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, float width, float height)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="horizontalScrollbar">Optional UnityEngine.GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="verticalScrollbar"> Optional UnityEngine.GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar);
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="horizontalScrollbar">Optional UnityEngine.GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="verticalScrollbar"> Optional UnityEngine.GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, float width)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, GUILayout.Width(width));
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="horizontalScrollbar">Optional UnityEngine.GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="verticalScrollbar"> Optional UnityEngine.GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <param name="height">The ScrollView's height.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, float width, float height)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="alwaysShowHorizontal">Should the horizontal bar be always visible.</param>
        /// <param name="alwaysShowVertical">Should the vertical bar be always visible.</param>
        /// <param name="horizontalScrollbar">Optional UnityEngine.GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="verticalScrollbar"> Optional UnityEngine.GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="background">Custom GUIStyle background.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background);
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="alwaysShowHorizontal">Should the horizontal bar be always visible.</param>
        /// <param name="alwaysShowVertical">Should the vertical bar be always visible.</param>
        /// <param name="horizontalScrollbar">Optional UnityEngine.GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="verticalScrollbar"> Optional UnityEngine.GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="background">Custom GUIStyle background.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, float width)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, GUILayout.Width(width));
        }
        /// <summary>
        /// Begin an automatically layouted scrollview.
        /// Returns: The modified scrollPosition. Feed this back into the variable you pass in.
        /// </summary>
        /// <param name="scrollPosition">The position to use display.</param>
        /// <param name="alwaysShowHorizontal">Should the horizontal bar be always visible.</param>
        /// <param name="alwaysShowVertical">Should the vertical bar be always visible.</param>
        /// <param name="horizontalScrollbar">Optional UnityEngine.GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="verticalScrollbar"> Optional UnityEngine.GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current UnityEngine.GUISkin is used.</param>
        /// <param name="background">Custom GUIStyle background.</param>
        /// <param name="width">The ScrollView's width.</param>
        /// <param name="height">The ScrollView's height.</param>
        /// <returns>The modified scrollPosition. Feed this back into the variable you pass in.</returns>
        public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, float width, float height)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Close a group started with BeginScrollView.
        /// </summary>
        public static void EndScrollView()
        {
            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region BeginFadeGroup / EndFadeGroup
        /// <summary>
        /// Begins a group that can be be hidden/shown and the transition will be animated.
        /// </summary>
        /// <param name="value">A value between 0 and 1, 0 being hidden, and 1 being fully visible.</param>
        /// <returns>If the group is visible or not.</returns>
        public static bool BeginFadeGroup(float value)
        {
            return EditorGUILayout.BeginFadeGroup(value);
        }

        /// <summary>
        /// Closes a group started with BeginFadeGroup.
        /// </summary>
        public static void EndFadeGroup()
        {
            EditorGUILayout.EndFadeGroup();
        }
        #endregion

        #region Button
        /// <summary>
        /// Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="text">Text to display on the button.</param>
        /// <returns>Returns true when the users clicks the button.</returns>
        public static bool Button(string text) { if(GUILayout.Button(text)) { ResetKeyboardFocus(); return true; } return false; }
        /// <summary>
        ///  Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="text">Text to display on the button.</param>
        /// <param name="width">Set the button's width.</param>
        /// <returns></returns>
        public static bool Button(string text, float width) { if(GUILayout.Button(text, GUILayout.Width(width))) { ResetKeyboardFocus(); return true; } return false; }
        /// <summary>
        ///  Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="text">Text to display on the button.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        /// <returns></returns>
        public static bool Button(string text, float width, float height) { if(GUILayout.Button(text, GUILayout.Width(width), GUILayout.Height(height))) { ResetKeyboardFocus(); return true; } return false; }
        /// <summary>
        /// Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="text">Text to display on the button.</param>
        /// <param name="style">The style to use.</param>
        /// <returns>Returns true when the users clicks the button.</returns>
        public static bool Button(string text, GUIStyle style) { if(GUILayout.Button(text, style)) { ResetKeyboardFocus(); return true; } return false; }
        /// <summary>
        ///  Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="text">Text to display on the button.</param>
        /// <param name="style">The style to use.</param>
        /// <param name="width">Set the button's width.</param>
        /// <returns></returns>
        public static bool Button(string text, GUIStyle style, float width) { if(GUILayout.Button(text, style, GUILayout.Width(width))) { ResetKeyboardFocus(); return true; } return false; }
        /// <summary>
        ///  Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="text">Text to display on the button.</param>
        /// <param name="style">The style to use.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        /// <returns></returns>
        public static bool Button(string text, GUIStyle style, float width, float height) { if(GUILayout.Button(text, style, GUILayout.Width(width), GUILayout.Height(height))) { ResetKeyboardFocus(); return true; } return false; }
        /// <summary>
        /// Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>Returns true when the users clicks the button.</returns>
        public static bool Button(GUIStyle style) { if(GUILayout.Button(GUIContent.none, style)) { ResetKeyboardFocus(); return true; } return false; }
        /// <summary>
        /// Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        /// <returns>Returns true when the users clicks the button.</returns>
        public static bool Button(GUIStyle style, float width, float height) { if(GUILayout.Button(GUIContent.none, style, GUILayout.Width(width), GUILayout.Height(height))) { ResetKeyboardFocus(); return true; } return false; }

        public static bool Button(Rect rect, GUIStyle style)
        {
            return GUI.Button(rect, GUIContent.none, style);
        }
        public static bool Button(Rect rect, string text, GUIStyle style)
        {
            return GUI.Button(rect, new GUIContent(text), style);
        }

        #endregion

        #region Quick Buttons
        public static bool ButtonMinus() { return Button(QStyles.GetStyle(Style.QuickButton.Minus), 16, 16); }
        public static bool ButtonMinus(Rect rect) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(Style.QuickButton.Minus)); }

        public static bool ButtonPlus() { return Button(QStyles.GetStyle(Style.QuickButton.Plus), 16, 16); }
        public static bool ButtonPlus(Rect rect) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(Style.QuickButton.Plus)); }

        public static bool ButtonCancel() { return Button(QStyles.GetStyle(Style.QuickButton.Cancel), 16, 16); }
        public static bool ButtonCancel(Rect rect) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(Style.QuickButton.Cancel)); }

        public static bool ButtonOk() { return Button(QStyles.GetStyle(Style.QuickButton.Ok), 16, 16); }
        public static bool ButtonOk(Rect rect) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(Style.QuickButton.Ok)); }

        public static bool ButtonLock(bool isSelected = false) { return Button(QStyles.GetStyle(isSelected ? Style.QuickButton.LockSelected : Style.QuickButton.Lock), 16, 16); }
        public static bool ButtonLock(Rect rect, bool isSelected = false) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(isSelected ? Style.QuickButton.LockSelected : Style.QuickButton.Lock)); }

        public static bool ButtonSave(bool isSelected = false) { return Button(QStyles.GetStyle(isSelected ? Style.QuickButton.SaveSelected : Style.QuickButton.Save), 16, 16); }
        public static bool ButtonSave(Rect rect, bool isSelected = false) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(isSelected ? Style.QuickButton.SaveSelected : Style.QuickButton.Save)); }

        public static bool ButtonReset() { return Button(QStyles.GetStyle(Style.QuickButton.Reset), 16, 16); }
        public static bool ButtonReset(Rect rect) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(Style.QuickButton.Reset)); }

        public static bool ButtonGraph(bool isSelected = false) { return Button(QStyles.GetStyle(isSelected ? Style.QuickButton.GraphSelected : Style.QuickButton.Graph), 16, 16); }
        public static bool ButtonGraph(Rect rect, bool isSelected = false) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(isSelected ? Style.QuickButton.GraphSelected : Style.QuickButton.Graph)); }

        public static bool ButtonData() { return Button(QStyles.GetStyle(Style.QuickButton.Data), 16, 16); }
        public static bool ButtonData(Rect rect) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(Style.QuickButton.Data)); }

        public static bool ButtonPlay() { return Button(QStyles.GetStyle(Style.QuickButton.Play), 16, 16); }
        public static bool ButtonPlay(Rect rect) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(Style.QuickButton.Play)); }

        public static bool ButtonStop() { return Button(QStyles.GetStyle(Style.QuickButton.Stop), 16, 16); }
        public static bool ButtonStop(Rect rect) { return Button(new Rect(rect.x, rect.y, 16, 16), QStyles.GetStyle(Style.QuickButton.Stop)); }
        #endregion

        #region Link Buttons
        public static bool LinkButton(string text, string url, Style.LinkButton linkButton, bool expandWidth = false)
        {
            if(expandWidth)
            {
                if(GUILayout.Button(text, QStyles.GetStyle(linkButton), GUILayout.Height(20)))
                {
                    Application.OpenURL(url);
                    ResetKeyboardFocus();
                    return true;
                }
            }
            else
            {
                QLabel.text = text;
                QLabel.style = Style.Text.Button;
                if(GUILayout.Button(text, QStyles.GetStyle(linkButton), GUILayout.Width(70 + QLabel.x + 6 - (linkButton == Style.LinkButton.Link ? 40 : 0)), GUILayout.Height(20)))
                {
                    Application.OpenURL(url);
                    ResetKeyboardFocus();
                    return true;
                }
            }
            return false;
        }

        public static bool LinkButton(LinkButtonData data, bool expandWidth = false)
        {
            return LinkButton(data.text, data.url, data.linkButton, expandWidth);
        }

        public static void DrawLinkButtonsList(List<LinkButtonData> list)
        {
            if(list == null || list.Count == 0)
            {
                return;
            }

            for(int i = 0; i < list.Count; i++)
            {
                QUI.BeginHorizontal();
                {
                    QUI.Space(16);
                    QUI.LinkButton(list[i].text, list[i].url, list[i].linkButton);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();

                if(i != list.Count - 1)
                {
                    QUI.Space(8);
                }
            }
        }

        public static void DrawLinkButtonsList(List<LinkButtonData> list, float indent, float width)
        {
            if(list == null || list.Count == 0)
            {
                return;
            }

            for(int i = 0; i < list.Count; i++)
            {
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(indent);
                    QUI.LinkButton(list[i].text, list[i].url, list[i].linkButton);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();

                if(i != list.Count - 1)
                {
                    QUI.Space(8);
                }
            }
        }

        #endregion

        #region Sliced Buttons
        public static bool SlicedButton(string text, QColors.Color color, bool isSelected = false)
        {
            if(GUILayout.Button(text, GetSlicedButtonStyle(color, isSelected), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                ResetKeyboardFocus();
                return true;
            }
            return false;
        }
        public static bool SlicedButton(string text, QColors.Color color, float width, bool isSelected = false)
        {
            if(GUILayout.Button(text, GetSlicedButtonStyle(color, isSelected), GUILayout.Width(width), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                ResetKeyboardFocus();
                return true;
            }
            return false;
        }
        public static bool SlicedButton(string text, QColors.Color color, float width, float height, bool isSelected = false)
        {
            GetSlicedButtonStyle(color, isSelected).fontSize = Mathf.RoundToInt(height * 0.55f);
            result = Button(text, GetSlicedButtonStyle(color, isSelected), width, height);
            GetSlicedButtonStyle(color, isSelected).fontSize = QStyles.GetTextFontSize(Style.Text.Button);
            return result;
        }
        public static bool SlicedButton(string text, QColors.Color color, Rect rect, bool isSelected = false) { return Button(rect, text, GetSlicedButtonStyle(color, isSelected)); }

        public static bool SlicedButton(Rect rect, string text, QColors.Color color, bool isSelected = false)
        {
            GetSlicedButtonStyle(color, isSelected).fontSize = Mathf.RoundToInt(rect.height * 0.6f);
            result = Button(rect, text, GetSlicedButtonStyle(color, isSelected));
            GetSlicedButtonStyle(color, isSelected).fontSize = QStyles.GetTextFontSize(Style.Text.Button);
            return result;
        }

        public static GUIStyle GetSlicedButtonStyle(QColors.Color color, bool isSelected = false)
        {
            switch(color)
            {
                case QColors.Color.Gray: return QStyles.GetStyle(!isSelected ? Style.SlicedButton.Gray : Style.SlicedButton.GraySelected);
                case QColors.Color.Green: return QStyles.GetStyle(!isSelected ? Style.SlicedButton.Green : Style.SlicedButton.GreenSelected);
                case QColors.Color.Blue: return QStyles.GetStyle(!isSelected ? Style.SlicedButton.Blue : Style.SlicedButton.BlueSelected);
                case QColors.Color.Orange: return QStyles.GetStyle(!isSelected ? Style.SlicedButton.Orange : Style.SlicedButton.OrangeSelected);
                case QColors.Color.Red: return QStyles.GetStyle(!isSelected ? Style.SlicedButton.Red : Style.SlicedButton.RedSelected);
                case QColors.Color.Purple: return QStyles.GetStyle(!isSelected ? Style.SlicedButton.Purple : Style.SlicedButton.PurpleSelected);
                default: return QStyles.GetStyle(!isSelected ? Style.SlicedButton.Gray : Style.SlicedButton.GraySelected);
            }
        }
        #endregion

        #region Ghost Buttons
        public static bool GhostButton(string text, QColors.Color color, bool isSelected = false)
        {
            if(GUILayout.Button(text, GetGhostButtonStyle(color, isSelected), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                ResetKeyboardFocus();
                return true;
            }
            return false;
        }
        public static bool GhostButton(string text, QColors.Color color, float width, bool isSelected = false)
        {
            if(GUILayout.Button(text, GetGhostButtonStyle(color, isSelected), GUILayout.Width(width), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                ResetKeyboardFocus();
                return true;
            }
            return false;
        }
        public static bool GhostButton(string text, QColors.Color color, float width, float height, bool isSelected = false)
        {
            GetGhostButtonStyle(color, isSelected).fontSize = Mathf.RoundToInt(height * 0.55f);
            result = Button(text, GetGhostButtonStyle(color, isSelected), width, height);
            GetGhostButtonStyle(color, isSelected).fontSize = QStyles.GetTextFontSize(Style.Text.Button);
            return result;
        }
        public static bool GhostButton(string text, QColors.Color color, float width, float height, int fontSize, bool isSelected = false)
        {
            GetGhostButtonStyle(color, isSelected).fontSize = fontSize;
            result = Button(text, GetGhostButtonStyle(color, isSelected), width, height);
            GetGhostButtonStyle(color, isSelected).fontSize = QStyles.GetTextFontSize(Style.Text.Button);
            return result;
        }
        public static bool GhostButton(string text, QColors.Color color, Rect rect, bool isSelected = false) { return Button(rect, text, GetGhostButtonStyle(color, isSelected)); }

        public static bool GhostButton(Rect rect, string text, QColors.Color color, bool isSelected = false)
        {
            GetGhostButtonStyle(color, isSelected).fontSize = Mathf.RoundToInt(rect.height * 0.6f);
            result = Button(rect, text, GetGhostButtonStyle(color, isSelected));
            GetGhostButtonStyle(color, isSelected).fontSize = QStyles.GetTextFontSize(Style.Text.Button);
            return result;
        }

        public static GUIStyle GetGhostButtonStyle(QColors.Color color, bool isSelected = false)
        {
            switch(color)
            {
                case QColors.Color.Gray: return QStyles.GetStyle(!isSelected ? Style.GhostButton.Gray : Style.GhostButton.GraySelected);
                case QColors.Color.Green: return QStyles.GetStyle(!isSelected ? Style.GhostButton.Green : Style.GhostButton.GreenSelected);
                case QColors.Color.Blue: return QStyles.GetStyle(!isSelected ? Style.GhostButton.Blue : Style.GhostButton.BlueSelected);
                case QColors.Color.Orange: return QStyles.GetStyle(!isSelected ? Style.GhostButton.Orange : Style.GhostButton.OrangeSelected);
                case QColors.Color.Red: return QStyles.GetStyle(!isSelected ? Style.GhostButton.Red : Style.GhostButton.RedSelected);
                case QColors.Color.Purple: return QStyles.GetStyle(!isSelected ? Style.GhostButton.Purple : Style.GhostButton.PurpleSelected);
                default: return QStyles.GetStyle(!isSelected ? Style.GhostButton.Gray : Style.GhostButton.GraySelected);
            }
        }
        #endregion

        #region Icon Bar
        /// <summary>
        /// Draws a bar with the set color and icon (miniIcon) texture.
        /// </summary>
        public static void DrawIconBar(string text, QTexture qTexture, QColors.Color color, IconPosition iconPosition, float width, float height = 16, Style.Text textStyle = Style.Text.Small)
        {
            QUI.BeginVertical(width);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, color), width, height);
                QUI.Space(-height);

                if(qTexture != null && qTexture.texture != null)
                {
                    QUI.BeginVertical(width, height);
                    {
                        QUI.Space(2);
                        QUI.BeginHorizontal(width, height - 2);
                        {
                            QUI.Space(4);
                            if(iconPosition == IconPosition.Right) { QUI.FlexibleSpace(); }
                            QUI.DrawTexture(qTexture.texture, height - 4, height - 4);
                            if(iconPosition == IconPosition.Left) { QUI.FlexibleSpace(); }
                            QUI.Space(4);
                        }
                        QUI.EndHorizontal();
                    }
                    QUI.EndVertical();
                    QUI.Space(-height);
                }

                switch(color)
                {
                    case QColors.Color.Gray: QUI.SetGUIColor(AccentColorGray); break;
                    case QColors.Color.Blue: QUI.SetGUIColor(AccentColorBlue); break;
                    case QColors.Color.Green: QUI.SetGUIColor(AccentColorGreen); break;
                    case QColors.Color.Orange: QUI.SetGUIColor(AccentColorOrange); break;
                    case QColors.Color.Red: QUI.SetGUIColor(AccentColorRed); break;
                    case QColors.Color.Purple: QUI.SetGUIColor(AccentColorPurple); break;
                }

                QUI.Space(2);

                QLabel.text = text;
                QLabel.style = textStyle;
                QUI.BeginHorizontal(width, height);
                {
                    QUI.Space(4);
                    if(iconPosition == IconPosition.Left) { QUI.Space(height - 2); }
                    QUI.BeginVertical(QLabel.x, height);
                    {
                        QUI.Space(-2);
                        QUI.Space((height - QUI.SingleLineHeight) / 2);
                        QUI.Label(QLabel);
                        QUI.Space((height - QUI.SingleLineHeight) / 2);
                    }
                    QUI.EndVertical();
                }
                QUI.EndHorizontal();

                QUI.ResetColors();
            }
            QUI.EndVertical();
        }
        #endregion

        #region Ghost Titles
        public static void GhostTitle(string text, QColors.Color color) { EditorGUILayout.LabelField(text, GetGhostTitleStyle(color), GUILayout.Height(36)); }
        public static void GhostTitle(string text, QColors.Color color, float width) { EditorGUILayout.LabelField(text, GetGhostTitleStyle(color), GUILayout.Width(width), GUILayout.Height(36)); }
        public static void GhostTitle(string text, QColors.Color color, float width, float height) { EditorGUILayout.LabelField(text, GetGhostTitleStyle(color), GUILayout.Width(width), GUILayout.Height(height)); }

        public static GUIStyle GetGhostTitleStyle(QColors.Color color)
        {
            switch(color)
            {
                case QColors.Color.Gray: return QStyles.GetStyle(Style.GhostTitle.Gray);
                case QColors.Color.Green: return QStyles.GetStyle(Style.GhostTitle.Green);
                case QColors.Color.Blue: return QStyles.GetStyle(Style.GhostTitle.Blue);
                case QColors.Color.Orange: return QStyles.GetStyle(Style.GhostTitle.Orange);
                case QColors.Color.Red: return QStyles.GetStyle(Style.GhostTitle.Red);
                case QColors.Color.Purple: return QStyles.GetStyle(Style.GhostTitle.Purple);
                default: return QStyles.GetStyle(Style.GhostTitle.Gray);
            }
        }
        #endregion

        #region Bars
        public static bool SlicedBar(string text, QColors.Color color, AnimBool aBool, float width, float height = 16)
        {
            result = false;
            if(height > 16)
            {
                GetSlicedBarStyle(color, aBool.value).fontSize = Mathf.RoundToInt(height * 0.6f);
            }
            QUI.BeginVertical(width, height);
            {
                if(QUI.Button(text, GetSlicedBarStyle(color, aBool.value), width, height))
                {
                    result = true;
                }
                QUI.Space(-height);
                QUI.FlexibleSpace();
                DrawCaret(aBool);
                QUI.FlexibleSpace();
            }
            QUI.EndVertical();
            if(height > 16)
            {
                GetSlicedBarStyle(color, aBool.value).fontSize = QStyles.GetTextFontSize(Style.Text.Bar);
            }
            return result;
        }
        public static bool GhostBar(string text, QColors.Color color, AnimBool aBool, float width, float height = 16)
        {
            result = false;
            if(height > 16)
            {
                GetGhostBarStyle(color, aBool.value).fontSize = Mathf.RoundToInt(height * 0.6f);
            }
            QUI.BeginVertical(width, height);
            {
                if(QUI.Button(text, GetGhostBarStyle(color, aBool.value), width, height))
                {
                    result = true;
                }
                QUI.Space(-height);
                QUI.FlexibleSpace();
                DrawCaret(aBool);
                QUI.FlexibleSpace();
            }
            QUI.EndVertical();
            if(height > 16)
            {
                GetGhostBarStyle(color, aBool.value).fontSize = QStyles.GetTextFontSize(Style.Text.Bar);
            }
            return result;
        }
        public static bool GhostBar(Rect rect, string text, QColors.Color color, AnimBool aBool)
        {
            result = false;
            if(rect.height > 16)
            {
                GetGhostBarStyle(color, aBool.value).fontSize = Mathf.RoundToInt(rect.height * 0.6f);
            }
            if(QUI.Button(rect, text, GetGhostBarStyle(color, aBool.value)))
            {
                result = true;
            }
            DrawCaret(new Rect(rect.x + 4, rect.y + (rect.height - 16) / 2, 16, 16), aBool);
            if(rect.height > 16)
            {
                GetGhostBarStyle(color, aBool.value).fontSize = QStyles.GetTextFontSize(Style.Text.Bar);
            }
            return result;
        }

        public static void DrawCaret(AnimBool aBool)
        {
            QUI.BeginHorizontal(16, 16);
            {
                QUI.Space(4);
                if(aBool.faded == 0) { DrawTexture(QResources.caretGray10.texture, 16, 16); }
                else if(aBool.faded <= 0.1f) { DrawTexture(QResources.caretGray9.texture, 16, 16); }
                else if(aBool.faded <= 0.2f) { DrawTexture(QResources.caretGray8.texture, 16, 16); }
                else if(aBool.faded <= 0.3f) { DrawTexture(QResources.caretGray7.texture, 16, 16); }
                else if(aBool.faded <= 0.4f) { DrawTexture(QResources.caretGray6.texture, 16, 16); }
                else if(aBool.faded <= 0.5f) { DrawTexture(QResources.caretGray5.texture, 16, 16); }
                else if(aBool.faded <= 0.6f) { DrawTexture(QResources.caretGray4.texture, 16, 16); }
                else if(aBool.faded <= 0.7f) { DrawTexture(QResources.caretGray3.texture, 16, 16); }
                else if(aBool.faded <= 0.8f) { DrawTexture(QResources.caretGray2.texture, 16, 16); }
                else if(aBool.faded <= 0.9f) { DrawTexture(QResources.caretGray1.texture, 16, 16); }
                else { DrawTexture(QResources.caretGray0.texture, 16, 16); }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        public static void DrawCaret(Rect rect, AnimBool aBool)
        {
            if(aBool.faded == 0) { DrawTexture(rect, QResources.caretGray10.texture); }
            else if(aBool.faded <= 0.1f) { DrawTexture(rect, QResources.caretGray9.texture); }
            else if(aBool.faded <= 0.2f) { DrawTexture(rect, QResources.caretGray8.texture); }
            else if(aBool.faded <= 0.3f) { DrawTexture(rect, QResources.caretGray7.texture); }
            else if(aBool.faded <= 0.4f) { DrawTexture(rect, QResources.caretGray6.texture); }
            else if(aBool.faded <= 0.5f) { DrawTexture(rect, QResources.caretGray5.texture); }
            else if(aBool.faded <= 0.6f) { DrawTexture(rect, QResources.caretGray4.texture); }
            else if(aBool.faded <= 0.7f) { DrawTexture(rect, QResources.caretGray3.texture); }
            else if(aBool.faded <= 0.8f) { DrawTexture(rect, QResources.caretGray2.texture); }
            else if(aBool.faded <= 0.9f) { DrawTexture(rect, QResources.caretGray1.texture); }
            else { DrawTexture(rect, QResources.caretGray0.texture); }
        }

        public static GUIStyle GetSlicedBarStyle(QColors.Color color, bool isSelected = false)
        {
            switch(color)
            {
                case QColors.Color.Gray: return QStyles.GetStyle(isSelected ? Style.SlicedBar.GraySelected : Style.SlicedBar.Gray);
                case QColors.Color.Green: return QStyles.GetStyle(isSelected ? Style.SlicedBar.GreenSelected : Style.SlicedBar.Green);
                case QColors.Color.Blue: return QStyles.GetStyle(isSelected ? Style.SlicedBar.BlueSelected : Style.SlicedBar.Blue);
                case QColors.Color.Orange: return QStyles.GetStyle(isSelected ? Style.SlicedBar.OrangeSelected : Style.SlicedBar.Orange);
                case QColors.Color.Red: return QStyles.GetStyle(isSelected ? Style.SlicedBar.RedSelected : Style.SlicedBar.Red);
                case QColors.Color.Purple: return QStyles.GetStyle(isSelected ? Style.SlicedBar.PurpleSelected : Style.SlicedBar.Purple);
                default: return QStyles.GetStyle(isSelected ? Style.SlicedBar.GraySelected : Style.SlicedBar.Gray);
            }
        }
        public static GUIStyle GetGhostBarStyle(QColors.Color color, bool isSelected = false)
        {
            switch(color)
            {
                case QColors.Color.Gray: return QStyles.GetStyle(isSelected ? Style.GhostBar.GraySelected : Style.GhostBar.Gray);
                case QColors.Color.Green: return QStyles.GetStyle(isSelected ? Style.GhostBar.GreenSelected : Style.GhostBar.Green);
                case QColors.Color.Blue: return QStyles.GetStyle(isSelected ? Style.GhostBar.BlueSelected : Style.GhostBar.Blue);
                case QColors.Color.Orange: return QStyles.GetStyle(isSelected ? Style.GhostBar.OrangeSelected : Style.GhostBar.Orange);
                case QColors.Color.Red: return QStyles.GetStyle(isSelected ? Style.GhostBar.RedSelected : Style.GhostBar.Red);
                case QColors.Color.Purple: return QStyles.GetStyle(isSelected ? Style.GhostBar.PurpleSelected : Style.GhostBar.Purple);
                default: return QStyles.GetStyle(isSelected ? Style.GhostBar.GraySelected : Style.GhostBar.Gray);
            }
        }
        #endregion

        #region Toggle
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="trueText">The text label to use when true.</param>
        /// <param name="trueStyle">The style to use when true.</param>
        /// <param name="falseText">The text label to use when false.</param>
        /// <param name="falseStyle">The style to use when false.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, string trueText, GUIStyle trueStyle, string falseText, GUIStyle falseStyle, float width, float height)
        {
            return value ? GUILayout.Toggle(value, trueText, trueStyle, GUILayout.Width(width), GUILayout.Height(height)) : GUILayout.Toggle(value, falseText, falseStyle, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="trueText">The text label to use when true.</param>
        /// <param name="trueStyle">The style to use when true.</param>
        /// <param name="falseText">The text label to use when false.</param>
        /// <param name="falseStyle">The style to use when false.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, string trueText, GUIStyle trueStyle, string falseText, GUIStyle falseStyle)
        {
            return value ? GUILayout.Toggle(value, trueText, trueStyle) : GUILayout.Toggle(value, falseText, falseStyle);
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="trueStyle">The style to use when true.</param>
        /// <param name="falseStyle">The style to use when false.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, GUIStyle trueStyle, GUIStyle falseStyle, float width, float height)
        {
            return value ? GUILayout.Toggle(value, GUIContent.none, trueStyle, GUILayout.Width(width), GUILayout.Height(height)) : GUILayout.Toggle(value, GUIContent.none, falseStyle, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="trueStyle">The style to use when true.</param>
        /// <param name="falseStyle">The style to use when false.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, GUIStyle trueStyle, GUIStyle falseStyle)
        {
            return value ? GUILayout.Toggle(value, GUIContent.none, trueStyle) : GUILayout.Toggle(value, GUIContent.none, falseStyle);
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="text">The text label to use.</param>
        /// <param name="style">The style to use.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, string text, GUIStyle style, float width, float height)
        {
            return GUILayout.Toggle(value, text, style, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="text">The text label to use.</param>
        /// <param name="style">The style to use.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, string text, GUIStyle style)
        {
            return GUILayout.Toggle(value, text, style);
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="style">The style to use.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, GUIStyle style, float width, float height)
        {
            return GUILayout.Toggle(value, GUIContent.none, style, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="style">The style to use.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, GUIStyle style)
        {
            return GUILayout.Toggle(value, GUIContent.none, style, GUILayout.Width(12));
        }
        /// <summary>
        /// Make an on/off toggle button. This is a shhorthand method for the native GUILayout.Toggle method.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <param name="text">The text label to use.</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value, string text)
        {
            return GUILayout.Toggle(value, text, QStyles.GetStyle(QStyles.GetStyleName(Style.Default.Toggle)), GUILayout.Width(12)); ;
        }
        /// <summary>
        /// Make an on/off toggle button. This is a shhorthand method for the native GUILayout.Toggle method.
        /// </summary>
        /// <param name="value">Is the button on or off?</param>
        /// <returns>Returns the new value of the button.</returns>
        public static bool Toggle(bool value)
        {
            return GUILayout.Toggle(value, GUIContent.none, QStyles.GetStyle(QStyles.GetStyleName(Style.Default.Toggle)), GUILayout.Width(12)); ;
        }

        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="trueText">The text label to use when true.</param>
        /// <param name="trueStyle">The style to use when true.</param>
        /// <param name="falseText">The text label to use when false.</param>
        /// <param name="falseStyle">The style to use when false.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        public static void Toggle(SerializedProperty serializedProperty, string trueText, GUIStyle trueStyle, string falseText, GUIStyle falseStyle, float width, float height)
        {
            serializedProperty.boolValue = serializedProperty.boolValue ? GUILayout.Toggle(serializedProperty.boolValue, trueText, trueStyle, GUILayout.Width(width), GUILayout.Height(height)) : GUILayout.Toggle(serializedProperty.boolValue, falseText, falseStyle, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="trueText">The text label to use when true.</param>
        /// <param name="trueStyle">The style to use when true.</param>
        /// <param name="falseText">The text label to use when false.</param>
        /// <param name="falseStyle">The style to use when false.</param>
        public static void Toggle(SerializedProperty serializedProperty, string trueText, GUIStyle trueStyle, string falseText, GUIStyle falseStyle)
        {
            serializedProperty.boolValue = serializedProperty.boolValue ? GUILayout.Toggle(serializedProperty.boolValue, trueText, trueStyle) : GUILayout.Toggle(serializedProperty.boolValue, falseText, falseStyle);
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="trueStyle">The style to use when true.</param>
        /// <param name="falseStyle">The style to use when false.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        public static void Toggle(SerializedProperty serializedProperty, GUIStyle trueStyle, GUIStyle falseStyle, float width, float height)
        {
            serializedProperty.boolValue = serializedProperty.boolValue ? GUILayout.Toggle(serializedProperty.boolValue, GUIContent.none, trueStyle, GUILayout.Width(width), GUILayout.Height(height)) : GUILayout.Toggle(serializedProperty.boolValue, GUIContent.none, falseStyle, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="trueStyle">The style to use when true.</param>
        /// <param name="falseStyle">The style to use when false.</param>
        public static void Toggle(SerializedProperty serializedProperty, GUIStyle trueStyle, GUIStyle falseStyle)
        {
            serializedProperty.boolValue = serializedProperty.boolValue ? GUILayout.Toggle(serializedProperty.boolValue, GUIContent.none, trueStyle) : GUILayout.Toggle(serializedProperty.boolValue, GUIContent.none, falseStyle);
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="text">The text label to use.</param>
        /// <param name="style">The style to use.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        public static void Toggle(SerializedProperty serializedProperty, string text, GUIStyle style, float width, float height)
        {
            serializedProperty.boolValue = GUILayout.Toggle(serializedProperty.boolValue, text, style, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="text">The text label to use.</param>
        /// <param name="style">The style to use.</param>
        public static void Toggle(SerializedProperty serializedProperty, string text, GUIStyle style)
        {
            serializedProperty.boolValue = GUILayout.Toggle(serializedProperty.boolValue, text, style);
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="style">The style to use.</param>
        /// <param name="width">Set the button's width.</param>
        /// <param name="height">Set the button's height.</param>
        public static void Toggle(SerializedProperty serializedProperty, GUIStyle style, float width, float height)
        {
            serializedProperty.boolValue = GUILayout.Toggle(serializedProperty.boolValue, GUIContent.none, style, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="style">The style to use.</param>
        public static void Toggle(SerializedProperty serializedProperty, GUIStyle style)
        {
            serializedProperty.boolValue = GUILayout.Toggle(serializedProperty.boolValue, GUIContent.none, style, GUILayout.Width(12));
        }
        /// <summary>
        /// Make an on/off toggle button. This is a shhorthand method for the native GUILayout.Toggle method.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        /// <param name="text">The text label to use.</param>
        public static void Toggle(SerializedProperty serializedProperty, string text)
        {
            serializedProperty.boolValue = GUILayout.Toggle(serializedProperty.boolValue, text, QStyles.GetStyle(QStyles.GetStyleName(Style.Default.Toggle)));
        }
        /// <summary>
        /// Make an on/off toggle button. This is a shhorthand method for the native GUILayout.Toggle method.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a toggle for.</param>
        public static void Toggle(SerializedProperty serializedProperty)
        {
            QUI.BeginHorizontal();
            {
                QUI.Space(-3);
                EditorGUILayout.PropertyField(serializedProperty, GUIContent.none, GUILayout.Width(12));
            }
            QUI.EndHorizontal();
            //serializedProperty.boolValue = GUILayout.Toggle(serializedProperty.boolValue, GUIContent.none, QStyles.GetStyle(QStyles.GetStyleName(Style.Default.Toggle)), GUILayout.Width(12));
        }
        #endregion

        #region QToggle
        /// <summary>
        /// Draws a Toggle with a background that is blue when ON and gray when OFF.
        /// </summary>
        /// <param name="text">Describes what this toggle does.</param>
        /// <param name="serializedProperty">Serialized propery that handles this toggle.</param>
        /// <param name="height">Toggle height.</param>
        /// <param name="textStyle">Text style.</param>
        public static void QToggle(string text, SerializedProperty serializedProperty, float height, Style.Text textStyle)
        {
            QLabel.text = text;
            QLabel.style = textStyle;

            QUI.BeginHorizontal(QLabel.x + 30, height);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, serializedProperty.boolValue ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 30, height);
                QUI.Space(-QLabel.x - 24);

                QUI.BeginVertical(12, height);
                {
                    QUI.Space(-1);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                    QUI.Toggle(serializedProperty);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                }
                QUI.EndVertical();

                QUI.Space(2);

                QUI.BeginVertical(QLabel.x, height);
                {
                    QUI.Space(-1);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                }
                QUI.EndVertical();

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        /// <summary>
        /// Draws a Toggle with a background that is blue when ON and gray when OFF.
        /// </summary>
        /// <param name="text">Describes what this toggle does.</param>
        /// <param name="boolValue">target bool that handles this toggle.</param>
        /// <param name="height">Toggle height.</param>
        /// <param name="textStyle">Text style.</param>
        public static bool QToggle(string text, bool boolValue, float height, Style.Text textStyle)
        {
            result = false;

            QLabel.text = text;
            QLabel.style = textStyle;

            QUI.BeginHorizontal(QLabel.x + 30, height);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, boolValue ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 30, height);
                QUI.Space(-QLabel.x - 24);

                QUI.BeginVertical(12, height);
                {
                    QUI.Space(-1);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                    result = QUI.Toggle(boolValue);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                }
                QUI.EndVertical();

                QUI.Space(2);

                QUI.BeginVertical(QLabel.x, height);
                {
                    QUI.Space(-1);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                }
                QUI.EndVertical();

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            return result;
        }

        /// <summary>
        /// Draws a Toggle with a background that is blue when ON and gray when off.
        /// </summary>
        /// <param name="text">Describes what this toggle does.</param>
        /// <param name="serializedProperty">Serialized propery that handles this toggle.</param>
        /// <param name="height">Toggle height.</param>
        public static void QToggle(string text, SerializedProperty serializedProperty, float height)
        {
            QToggle(text, serializedProperty, height, Style.Text.Normal);
        }
        /// <summary>
        /// Draws a Toggle with a background that is blue when ON and gray when off.
        /// </summary>
        /// <param name="text">Describes what this toggle does.</param>
        /// <param name="boolValue">target bool that handles this toggle.</param>
        /// <param name="height">Toggle height.</param>
        public static bool QToggle(string text, bool boolValue, float height)
        {
            return QToggle(text, boolValue, height, Style.Text.Normal);
        }

        /// <summary>
        /// Draws a Toggle with a background that is blue when ON and gray when off.
        /// </summary>
        /// <param name="text">Describes what this toggle does.</param>
        /// <param name="serializedProperty">Serialized propery that handles this toggle.</param>
        /// <param name="textStyle">Text style.</param>
        public static void QToggle(string text, SerializedProperty serializedProperty, Style.Text textStyle)
        {
            QToggle(text, serializedProperty, 18, textStyle);
        }
        /// <summary>
        /// Draws a Toggle with a background that is blue when ON and gray when off.
        /// </summary>
        /// <param name="text">Describes what this toggle does.</param>
        /// <param name="boolValue">target bool that handles this toggle.</param>
        /// <param name="textStyle">Text style.</param>
        public static bool QToggle(string text, bool boolValue, Style.Text textStyle)
        {
            return QToggle(text, boolValue, 18, textStyle);
        }

        /// <summary>
        /// Draws a Toggle with a background that is blue when ON and gray when off.
        /// </summary>
        /// <param name="text">Describes what this toggle does.</param>
        /// <param name="serializedProperty">Serialized propery that handles this toggle.</param>
        public static void QToggle(string text, SerializedProperty serializedProperty)
        {
            QToggle(text, serializedProperty, 18, Style.Text.Normal);
        }
        /// <summary>
        /// Draws a Toggle with a background that is blue when ON and gray when off.
        /// </summary>
        /// <param name="text">Describes what this toggle does.</param>
        /// <param name="boolValue">target bool that handles this toggle.</param>
        public static bool QToggle(string text, bool boolValue)
        {
            return QToggle(text, boolValue, 18, Style.Text.Normal);
        }
        #endregion

        #region Popup
        /// <summary>
        /// Make a generic popup selection field.
        /// Takes the currently selected index as a parameter and returns the index selected
        /// by the user.Create a primitive depending on the option selected.
        /// </summary>
        /// <param name="serializedProperty">SerializedProperty of an enum variable that you want to show and change.</param>
        public static void Popup(SerializedProperty serializedProperty)
        {
            serializedProperty.enumValueIndex = EditorGUILayout.Popup(serializedProperty.enumValueIndex, serializedProperty.enumDisplayNames);
        }

        /// <summary>
        /// Make a generic popup selection field.
        /// Takes the currently selected index as a parameter and returns the index selected
        /// by the user.Create a primitive depending on the option selected.
        /// </summary>
        /// <param name="serializedProperty">SerializedProperty of an enum variable that you want to show and change.</param>
        /// <param name="width">Set the popup's width.</param>
        public static void Popup(SerializedProperty serializedProperty, float width)
        {
            serializedProperty.enumValueIndex = EditorGUILayout.Popup(serializedProperty.enumValueIndex, serializedProperty.enumDisplayNames, GUILayout.Width(width));
        }
        #endregion

        #region Label
        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        public static void Label(GUIContent label)
        {
            EditorGUILayout.LabelField(label);
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="width">The label's width.</param>
        public static void Label(GUIContent label, float width)
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The label's height.</param>
        public static void Label(GUIContent label, float width, float height)
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a custom style for the label.</param>
        public static void Label(GUIContent label, GUIStyle style)
        {
            EditorGUILayout.LabelField(label, style);
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a custom style for the label.</param>
        /// <param name="width">The label's width.</param>
        public static void Label(GUIContent label, GUIStyle style, float width)
        {
            EditorGUILayout.LabelField(label, style, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a custom style for the label.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The label's height.</param>
        public static void Label(GUIContent label, GUIStyle style, float width, float height)
        {
            EditorGUILayout.LabelField(label, style, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        public static void Label(string label)
        {
            EditorGUILayout.LabelField(label);
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="width">The label's width.</param>
        public static void Label(string label, float width)
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The label's height.</param>
        public static void Label(string label, float width, float height)
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="tooltip">The tooltip associtated with this label.</param>
        public static void Label(string label, string tooltip)
        {
            EditorGUILayout.LabelField(new GUIContent { text = label, tooltip = tooltip });
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="tooltip">The tooltip associtated with this label.</param>
        /// <param name="width">The label's width.</param>
        public static void Label(string label, string tooltip, float width)
        {
            EditorGUILayout.LabelField(new GUIContent { text = label, tooltip = tooltip }, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="tooltip">The tooltip associtated with this label.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The label's height.</param>
        public static void Label(string label, string tooltip, float width, float height)
        {
            EditorGUILayout.LabelField(new GUIContent { text = label, tooltip = tooltip }, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a custom style for the label.</param>        
        public static void Label(string label, GUIStyle style)
        {
            EditorGUILayout.LabelField(label, style);
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a custom style for the label.</param>
        /// <param name="width">The label's width.</param>
        public static void Label(string label, GUIStyle style, float width)
        {
            EditorGUILayout.LabelField(label, style, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a custom style for the label.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The lable's height.</param>
        public static void Label(string label, GUIStyle style, float width, float height)
        {
            EditorGUILayout.LabelField(label, style, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="tooltip">The tooltip associtated with this label.</param>
        /// <param name="style">Set a custom style for the label.</param>
        public static void Label(string label, string tooltip, GUIStyle style)
        {
            EditorGUILayout.LabelField(new GUIContent { text = label, tooltip = tooltip }, style);
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="tooltip">The tooltip associtated with this label.</param>
        /// <param name="style">Set a custom style for the label.</param>
        /// <param name="width">The label's width.</param>
        public static void Label(string label, string tooltip, GUIStyle style, float width)
        {
            EditorGUILayout.LabelField(new GUIContent { text = label, tooltip = tooltip }, style, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="tooltip">The tooltip associtated with this label.</param>
        /// <param name="style">Set a custom style for the label.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The label's height.</param>
        public static void Label(string label, string tooltip, GUIStyle style, float width, float height)
        {
            EditorGUILayout.LabelField(new GUIContent { text = label, tooltip = tooltip }, style, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="texture">The icon image contained.</param>
        public static void Label(Texture texture)
        {
            EditorGUILayout.LabelField(new GUIContent { image = texture });
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="texture">The icon image contained.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The label's height.</param>
        public static void Label(Texture texture, float width, float height)
        {
            EditorGUILayout.LabelField(new GUIContent { image = texture }, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="texture">The icon image contained.</param>
        /// <param name="tooltip">The tooltip associtated with this label.</param>
        public static void Label(Texture texture, string tooltip)
        {
            EditorGUILayout.LabelField(new GUIContent { image = texture, tooltip = tooltip });
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="texture">The icon image contained.</param>
        /// <param name="tooltip">The tooltip associtated with this label.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The label's height.</param>
        public static void Label(Texture texture, string tooltip, float width, float height)
        {
            EditorGUILayout.LabelField(new GUIContent { image = texture, tooltip = tooltip }, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a predefined custom style for the label.</param>
        public static void Label(string label, Style.Text style = Style.Text.Normal)
        {
            Label(label, QStyles.GetStyle(QStyles.GetStyleName(style)));
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a predefined custom style for the label.</param>
        /// <param name="width">The label's width.</param>
        public static void Label(string label, Style.Text style, float width)
        {
            Label(label, QStyles.GetStyle(QStyles.GetStyleName(style)), width);
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a predefined custom style for the label.</param>
        /// <param name="width">The label's width.</param>
        /// <param name="height">The label's height.</param>
        public static void Label(string label, Style.Text style, float width, float height)
        {
            Label(label, QStyles.GetStyle(QStyles.GetStyleName(style)), width, height);
        }

        public static void Label(QLabel qLabel)
        {
            Label(qLabel.text, qLabel.style, qLabel.x);
        }

        public static void Label(QLabel qLabel, float height)
        {
            Label(qLabel.text, qLabel.style, qLabel.x, height);
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="rect">Label position and size rect.</param>
        /// <param name="label">Label in front of the label field.</param>
        /// <param name="style">Set a predefined custom style for the label.</param>
        public static void Label(Rect rect, string label, Style.Text style = Style.Text.Normal)
        {
            EditorGUI.LabelField(rect, label, QStyles.GetStyle(QStyles.GetStyleName(style)));
        }

        public static void LabelWithBackground(string text, Style.Text textStyle = Style.Text.Small, float height = 20, float extraWidth = 0, QColors.Color color = QColors.Color.Gray)
        {
            QLabel.text = text;
            QLabel.style = textStyle;
            QUI.BeginHorizontal(QLabel.x + 16 + extraWidth, height);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, color), QLabel.x + 16, height);
                QUI.Space(-QLabel.x - 12 - extraWidth);

                QUI.BeginVertical(QLabel.x, height);
                {
                    QUI.Space(-1);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                }
                QUI.EndVertical();
            }
            QUI.EndHorizontal();
        }

        public static void LabelWithBackground(QLabel qLabel, float height = 20, float extraWidth = 0, QColors.Color color = QColors.Color.Gray)
        {
            QUI.BeginHorizontal(qLabel.x + 16 + extraWidth, height);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, color), qLabel.x + 16, height);
                QUI.Space(-qLabel.x - 12 - extraWidth);

                QUI.BeginVertical(qLabel.x, height);
                {
                    QUI.Space(-1);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                    QUI.Label(qLabel);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                }
                QUI.EndVertical();
            }
            QUI.EndHorizontal();
        }
        #endregion

        #region TextField
        public static string TextField(string text)
        {
            return EditorGUILayout.TextField(text);
        }

        public static string TextField(string text, Color backgroundColor)
        {
            QUI.SetGUIBackgroundColor(backgroundColor);
            tempString = EditorGUILayout.TextField(text);
            QUI.ResetColors();
            return tempString;
        }

        public static string TextField(string text, float width)
        {
            return EditorGUILayout.TextField(text, GUILayout.Width(width));
        }

        public static string TextField(string text, Color backgroundColor, float width)
        {
            QUI.SetGUIBackgroundColor(backgroundColor);
            tempString = EditorGUILayout.TextField(text, GUILayout.Width(width));
            QUI.ResetColors();
            return tempString;
        }

        public static string TextField(string text, float width, float height)
        {
            return EditorGUILayout.TextField(text, GUILayout.Width(width), GUILayout.Height(height));
        }

        public static string TextField(string text, Color backgroundColor, float width, float height)
        {
            QUI.SetGUIBackgroundColor(backgroundColor);
            tempString = EditorGUILayout.TextField(text, GUILayout.Width(width), GUILayout.Height(height));
            QUI.ResetColors();
            return tempString;
        }
        #endregion

        #region PropertyField
        /// <summary>
        /// Make a field for UnityEditor.SerializedProperty.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a field for.</param>
        public static void PropertyField(SerializedProperty serializedProperty)
        {
            EditorGUILayout.PropertyField(serializedProperty, GUIContent.none);
        }

        /// <summary>
        /// Make a field for UnityEditor.SerializedProperty.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a field for.</param>
        public static void PropertyField(SerializedProperty serializedProperty, GUIContent content)
        {
            EditorGUILayout.PropertyField(serializedProperty, content);
        }

        /// <summary>
        /// Make a field for UnityEditor.SerializedProperty.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a field for.</param>
        /// <param name="width">The serializedProperty's width</param>
        public static void PropertyField(SerializedProperty serializedProperty, float width)
        {
            EditorGUILayout.PropertyField(serializedProperty, GUIContent.none, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a field for UnityEditor.SerializedProperty.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a field for.</param>
        /// <param name="width">The serializedProperty's width</param>
        public static void PropertyField(SerializedProperty serializedProperty, GUIContent content, float width)
        {
            EditorGUILayout.PropertyField(serializedProperty, content, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a field for UnityEditor.SerializedProperty.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a field for.</param>
        /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
        public static void PropertyField(SerializedProperty serializedProperty, bool includeChildren)
        {
            EditorGUILayout.PropertyField(serializedProperty, GUIContent.none, includeChildren);
        }

        /// <summary>
        /// Make a field for UnityEditor.SerializedProperty.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a field for.</param>
        /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
        public static void PropertyField(SerializedProperty serializedProperty, bool includeChildren, GUIContent content)
        {
            EditorGUILayout.PropertyField(serializedProperty, content, includeChildren);
        }

        /// <summary>
        /// Make a field for UnityEditor.SerializedProperty.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a field for.</param>
        /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
        /// <param name="width">The serializedProperty's width</param>
        public static void PropertyField(SerializedProperty serializedProperty, bool includeChildren, float width)
        {
            EditorGUILayout.PropertyField(serializedProperty, GUIContent.none, includeChildren, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a field for UnityEditor.SerializedProperty.
        /// </summary>
        /// <param name="serializedProperty">The SerializedProperty to make a field for.</param>
        /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
        /// <param name="width">The serializedProperty's width</param>
        public static void PropertyField(SerializedProperty serializedProperty, bool includeChildren, GUIContent content, float width)
        {
            EditorGUILayout.PropertyField(serializedProperty, content, includeChildren, GUILayout.Width(width));
        }
        #endregion

        #region QObjectPropertyField
        public static void QObjectPropertyField(string text, SerializedProperty serializedProperty, float width, float height = 20, bool checkForNull = true)
        {
            QLabel.text = text;
            QLabel.style = Style.Text.Normal;

            QUI.BeginHorizontal(width, height);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, checkForNull && serializedProperty.objectReferenceValue != null ? QColors.Color.Blue : QColors.Color.Gray), width, height);
                QUI.Space(-width + 4);

                QUI.BeginVertical(QLabel.x, height);
                {
                    QUI.Space(-1);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                }
                QUI.EndVertical();

                QUI.BeginVertical(width - QLabel.x - 12, height);
                {
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                    QUI.PropertyField(serializedProperty);
                    QUI.Space((height - QUI.SingleLineHeight) / 2);
                }
                QUI.EndVertical();

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        #endregion

        #region ObjectField
        public static Object ObjectField(Object obj, System.Type objType, bool allowSceneObjects)
        {
            return EditorGUILayout.ObjectField(obj, objType, allowSceneObjects);
        }

        public static Object ObjectField(Object obj, System.Type objType, bool allowSceneObjects, float width)
        {
            return EditorGUILayout.ObjectField(obj, objType, allowSceneObjects, GUILayout.Width(width));
        }
        #endregion

        #region Vector3
        /// <summary>
        /// Make an X, Y & Z field for entering a UnityEngine.Vector3.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <returns>The value entered by the user.</returns>
        public static Vector3 Vector3(Vector3 value)
        {
            return EditorGUILayout.Vector3Field(GUIContent.none, value);
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a UnityEngine.Vector3.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="width">The field's width.</param>
        /// <returns>The value entered by the user.</returns>
        public static Vector3 Vector3(Vector3 value, float width)
        {
            return EditorGUILayout.Vector3Field(GUIContent.none, value, GUILayout.Width(width));
        }
        #endregion

        #region ColorField
        /// <summary>
        /// Make a field for selecting a UnityEngine.Color.
        /// </summary>
        /// <param name="value">The color to edit.</param>
        /// <returns>The color selected by the user.</returns>
        public static Color ColorField(Color value)
        {
            return EditorGUILayout.ColorField(value);
        }

        /// <summary>
        /// Make a field for selecting a UnityEngine.Color.
        /// </summary>
        /// <param name="value">The color to edit.</param>
        /// <param name="width">The field's width.</param>
        /// <returns>The color selected by the user.</returns>
        public static Color ColorField(Color value, float width)
        {
            return EditorGUILayout.ColorField(value, GUILayout.Width(width));
        }

        /// <summary>
        /// Make a field for selecting a UnityEngine.Color.
        /// </summary>
        /// <param name="value">The color to edit.</param>
        /// <param name="width">The field's width.</param>
        /// <param name="height">The field's height.</param>
        /// <returns>The color selected by the user.</returns>
        public static Color ColorField(Color value, float width, float height)
        {
            return EditorGUILayout.ColorField(value, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Make a field for selecting a UnityEngine.Color.
        /// </summary>
        /// <param name="value">The color to edit.</param>
        /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
        /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
        /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
        /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
        /// <returns>The color selected by the user.</returns>
        public static Color ColorField(Color value, bool showEyedropper, bool showAlpha, bool hdr)
        {
#if UNITY_2018_1_OR_NEWER
            return EditorGUILayout.ColorField(GUIContent.none, value, showEyedropper, showAlpha, hdr);
#else
            return EditorGUILayout.ColorField(GUIContent.none, value, showEyedropper, showAlpha, hdr, new ColorPickerHDRConfig(0, 1, 0, 1));
#endif
        }

        /// <summary>
        /// Make a field for selecting a UnityEngine.Color.
        /// </summary>
        /// <param name="value">The color to edit.</param>
        /// <param name="showEyedropper"></param>
        /// <param name="showAlpha"></param>
        /// <param name="hdr"></param>
        /// <param name="hdrConfig"></param>
        /// <param name="width">The field's width.</param>
        /// <returns>The color selected by the user.</returns>
        public static Color ColorField(Color value, bool showEyedropper, bool showAlpha, bool hdr, float width)
        {

#if UNITY_2018_1_OR_NEWER
            return EditorGUILayout.ColorField(GUIContent.none, value, showEyedropper, showAlpha, hdr, GUILayout.Width(width));
#else
            return EditorGUILayout.ColorField(GUIContent.none, value, showEyedropper, showAlpha, hdr, new ColorPickerHDRConfig(0, 1, 0, 1), GUILayout.Width(width));
#endif

        }

        /// <summary>
        /// Make a field for selecting a UnityEngine.Color.
        /// </summary>
        /// <param name="value">The color to edit.</param>
        /// <param name="showEyedropper"></param>
        /// <param name="showAlpha"></param>
        /// <param name="hdr"></param>
        /// <param name="hdrConfig"></param>
        /// <param name="width">The field's width.</param>
        /// <param name="height">The field's height.</param>
        /// <returns>The color selected by the user.</returns>
        public static Color ColorField(Color value, bool showEyedropper, bool showAlpha, bool hdr, float width, float height)
        {
#if UNITY_2018_1_OR_NEWER
            return EditorGUILayout.ColorField(GUIContent.none, value, showEyedropper, showAlpha, hdr, GUILayout.Width(width), GUILayout.Height(height));
#else
            return EditorGUILayout.ColorField(GUIContent.none, value, showEyedropper, showAlpha, hdr, new ColorPickerHDRConfig(0, 1, 0, 1), GUILayout.Width(width), GUILayout.Height(height));
#endif
        }
        #endregion

        #region Box
        /// <summary>
        /// Make an auto-layout box with its width expanded.
        /// </summary>
        /// <param name="content">Text, image and tooltip for this box.</param>
        public static void BoxExpandedWidth(GUIContent content, float height) { GUILayout.Box(content, GUILayout.ExpandWidth(true), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box with its width expanded.
        /// </summary>
        /// <param name="label">Text to display on the box.</param>
        public static void BoxExpandedWidth(string label, float height) { GUILayout.Box(label, GUILayout.ExpandWidth(true), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box with its width expanded.
        /// </summary>
        /// <param name="texture">Texture to display on the box.</param>
        public static void BoxExpandedWidth(Texture texture, float height) { GUILayout.Box(texture, GUILayout.ExpandWidth(true), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box with its width expanded.
        /// </summary>
        /// <param name="content">Text, image and tooltip for this box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpandedWidth(GUIContent content, GUIStyle style, float height) { GUILayout.Box(content, style, GUILayout.ExpandWidth(true), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box with its width expanded.
        /// </summary>
        /// <param name="label">Text to display on the box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpandedWidth(string label, GUIStyle style, float height) { GUILayout.Box(label, style, GUILayout.ExpandWidth(true), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box with its width expanded.
        /// </summary>
        /// <param name="texture">Texture to display on the box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpandedWidth(Texture texture, GUIStyle style, float height) { GUILayout.Box(texture, style, GUILayout.ExpandWidth(true), GUILayout.Height(height)); }

        /// <summary>
        /// Make an auto-layout box with its height expanded.
        /// </summary>
        /// <param name="content">Text, image and tooltip for this box.</param>
        public static void BoxExpandedHeight(GUIContent content, float width) { GUILayout.Box(content, GUILayout.Width(width), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its height expanded.
        /// </summary>
        /// <param name="label">Text to display on the box.</param>
        public static void BoxExpandedHeight(string label, float width) { GUILayout.Box(label, GUILayout.Width(width), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its height expanded.
        /// </summary>
        /// <param name="texture">Texture to display on the box.</param>
        public static void BoxExpandedHeight(Texture texture, float width) { GUILayout.Box(texture, GUILayout.Width(width), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its height expanded.
        /// </summary>
        /// <param name="content">Text, image and tooltip for this box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpandedHeight(GUIContent content, GUIStyle style, float width) { GUILayout.Box(content, style, GUILayout.Width(width), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its height expanded.
        /// </summary>
        /// <param name="label">Text to display on the box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpandedHeight(string label, GUIStyle style, float width) { GUILayout.Box(label, style, GUILayout.Width(width), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its height expanded.
        /// </summary>
        /// <param name="texture">Texture to display on the box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpandedHeight(Texture texture, GUIStyle style, float width) { GUILayout.Box(texture, style, GUILayout.Width(width), GUILayout.ExpandHeight(true)); }

        /// <summary>
        /// Make an auto-layout box with its width and height expanded.
        /// </summary>
        /// <param name="content">Text, image and tooltip for this box.</param>
        public static void BoxExpanded(GUIContent content) { GUILayout.Box(content, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its width and height expanded.
        /// </summary>
        /// <param name="label">Text to display on the box.</param>
        public static void BoxExpanded(string label) { GUILayout.Box(label, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its width and height expanded.
        /// </summary>
        /// <param name="texture">Texture to display on the box.</param>
        public static void BoxExpanded(Texture texture) { GUILayout.Box(texture, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its width and height expanded.
        /// </summary>
        /// <param name="content">Text, image and tooltip for this box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpanded(GUIContent content, GUIStyle style) { GUILayout.Box(content, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its width and height expanded.
        /// </summary>
        /// <param name="label">Text to display on the box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpanded(string label, GUIStyle style) { GUILayout.Box(label, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); }
        /// <summary>
        /// Make an auto-layout box with its width and height expanded.
        /// </summary>
        /// <param name="texture">Texture to display on the box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void BoxExpanded(Texture texture, GUIStyle style) { GUILayout.Box(texture, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); }

        /// <summary>
        /// Make an auto-layout box.
        /// </summary>
        /// <param name="content">Text, image and tooltip for this box.</param>
        public static void Box(GUIContent content, float width, float height) { GUILayout.Box(content, GUILayout.Width(width), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box.
        /// </summary>
        /// <param name="label">Text to display on the box.</param>
        public static void Box(string label, float width, float height) { GUILayout.Box(label, GUILayout.Width(width), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box.
        /// </summary>
        /// <param name="texture">Texture to display on the box.</param>
        public static void Box(Texture texture, float width, float height) { GUILayout.Box(texture, GUILayout.Width(width), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box.
        /// </summary>
        /// <param name="content">Text, image and tooltip for this box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void Box(GUIContent content, GUIStyle style, float width, float height) { GUILayout.Box(content, style, GUILayout.Width(width), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box.
        /// </summary>
        /// <param name="label">Text to display on the box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void Box(string label, GUIStyle style, float width, float height) { GUILayout.Box(label, style, GUILayout.Width(width), GUILayout.Height(height)); }
        /// <summary>
        /// Make an auto-layout box.
        /// </summary>
        /// <param name="texture">Texture to display on the box.</param>
        /// <param name="style">The style to use. If left out, the box style from the current GUISkin is used.</param>
        public static void Box(Texture texture, GUIStyle style, float width, float height)
        {

            GUILayout.Box(texture, style, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an auto-layout box.
        /// </summary>
        public static void Box(GUIStyle style, float width, float height)
        {
            GUILayout.Box(GUIContent.none, style, GUILayout.Width(width), GUILayout.Height(height));
        }
        /// <summary>
        /// Make an auto-layout box.
        /// </summary>
        public static void Box(Rect rect, GUIStyle style)
        {
            GUI.Box(rect, GUIContent.none, style);
        }
        #endregion

        #region InfoMessage / DrawInfoMessage
        /// <summary>
        /// Draws an InfoMessage box with the specified InfoMessage settings, with the set width and of the set InfoMessageType.
        /// </summary>
        /// <param name="im">Contains an AnimBool that manages the show/hide animation, a title (optional), and a message.</param>
        /// <param name="width">The width of this box. The height is determined automatically by the amount of text contained in the InfoMessage.</param>
        /// <param name="type">Depending on the type,it will draw a HelpBox, an InfoBox, a WarningBox or an ErrorBox. Each box has it's own style and icon.</param>
        public static void DrawInfoMessage(InfoMessage im, float width)
        {
            if(!im.show.value) { return; }
            if(QUI.BeginFadeGroup(im.show.faded))
            {
                QUI.BeginVertical();
                {
                    QUI.Space(2 * im.show.faded);
                    QUI.Label(im.title.IsNullOrEmpty() ? im.type.ToString() : im.title, QStyles.GetInfoMessageTitleStyle(GetStyleInfoMessage(im.type)), width * im.show.faded, 20);
                    if(!im.message.IsNullOrEmpty())
                    {
                        QUI.Space(-8);
                        QUI.Label(im.message, QStyles.GetInfoMessageMessageStyle(GetStyleInfoMessage(im.type)), width * im.show.faded);
                        QUI.Space(10 * im.show.faded);
                    }
                    QUI.Space(2 * im.show.faded);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }

        /// <summary>
        /// Converts InfoMessageType into Style.InfoMessage. This method is used to get the appropriate style for the selected info message.
        /// </summary>
        private static Style.InfoMessage GetStyleInfoMessage(InfoMessageType type)
        {
            switch(type)
            {
                case InfoMessageType.Help: return Style.InfoMessage.Help;
                case InfoMessageType.Info: return Style.InfoMessage.Info;
                case InfoMessageType.Warning: return Style.InfoMessage.Warning;
                case InfoMessageType.Error: return Style.InfoMessage.Error;
                case InfoMessageType.Success: return Style.InfoMessage.Success;
                default: return Style.InfoMessage.Info;
            }
        }
        #endregion

        #region DrawList

        public static void DrawList(SerializedProperty list, float width, string emptyMessage = "List is empty...")
        {
            if(list.arraySize == 0)
            {
                QUI.BeginHorizontal(width);
                {
                    QLabel.text = emptyMessage;
                    QLabel.style = Style.Text.Help;
                    QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                    {
                        QUI.Label(QLabel);
                        QUI.Space(2);
                    }
                    QUI.EndVertical();

                    QUI.FlexibleSpace();

                    QUI.BeginVertical(16, QUI.SingleLineHeight);
                    {
                        if(QUI.ButtonPlus())
                        {
                            list.InsertArrayElementAtIndex(0);
                        }
                        QUI.Space(1);
                    }
                    QUI.EndVertical();

                    QUI.Space(4);
                }
                QUI.EndHorizontal();
                return;
            }

            QUI.BeginVertical(width);
            {
                QLabel.style = Style.Text.Help;
                for(int i = 0; i < list.arraySize; i++)
                {
                    QUI.BeginHorizontal(width, QUI.SingleLineHeight);
                    {
                        QLabel.text = i.ToString();
                        QUI.Label(QLabel);

                        QUI.Space(2);

                        QUI.PropertyField(list.GetArrayElementAtIndex(i), true, width - QLabel.x - 2 - 16 - 12);

                        if(QUI.ButtonMinus())
                        {
                            list.DeleteArrayElementAtIndex(i);
                        }

                        QUI.Space(8);
                    }
                    QUI.EndHorizontal();
                }

                QUI.BeginHorizontal(width);
                {
                    QUI.FlexibleSpace();

                    QUI.BeginVertical(16, QUI.SingleLineHeight);
                    {
                        if(QUI.ButtonPlus())
                        {
                            list.InsertArrayElementAtIndex(0);
                        }
                        QUI.Space(1);
                    }
                    QUI.EndVertical();

                    QUI.Space(4);
                }
                QUI.EndHorizontal();
            }
            QUI.EndVertical();
        }
        public static void DrawCollapsableList(string barTitle, AnimBool show, QColors.Color color, SerializedProperty list, float width, float barHeight, string emptyMessage = "List is empty...")
        {
            tempFloat = (20 + 2 + 18 * (list.arraySize + 1) + 2) * show.faded; //background height
            if(show.faded > 0.1f)
            {
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(4 * show.faded);
                    QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, color), width - 4, tempFloat);
                }
                QUI.EndHorizontal();
                QUI.Space(-tempFloat);
            }

            if(QUI.GhostBar(barTitle, color, show, width, barHeight))
            {
                show.target = !show.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(8 * show.faded);
                if(QUI.BeginFadeGroup(show.faded))
                {
                    QUI.BeginVertical(width - 8);
                    {

                        QUI.Space(2);
                        QUI.DrawList(list, (width - 8) * show.faded, emptyMessage);
                        QUI.Space(2);

                        QUI.Space(4 * show.faded);
                    }
                    QUI.EndVertical();

                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();
        }

        public static void DrawStringList(List<string> list, Object targetObject, float width, string emptyMessage = "List is empty...")
        {
            if(list.Count == 0)
            {
                QUI.BeginHorizontal(width);
                {
                    QLabel.text = emptyMessage;
                    QLabel.style = Style.Text.Help;
                    QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                    {
                        QUI.Label(QLabel);
                        QUI.Space(2);
                    }
                    QUI.EndVertical();

                    QUI.FlexibleSpace();

                    QUI.BeginVertical(16, QUI.SingleLineHeight);
                    {
                        if(QUI.ButtonPlus())
                        {
                            Undo.RecordObject(targetObject, "AddItem");
                            list.Add("");
                        }
                        QUI.Space(1);
                    }
                    QUI.EndVertical();

                    QUI.Space(4);
                }
                QUI.EndHorizontal();
                return;
            }

            QUI.BeginVertical(width);
            {
                QLabel.style = Style.Text.Help;
                for(int i = 0; i < list.Count; i++)
                {
                    QUI.BeginHorizontal(width, QUI.SingleLineHeight);
                    {
                        QLabel.text = i.ToString();
                        QUI.Label(QLabel);

                        QUI.Space(2);

                        list[i] = QUI.TextField(list[i], width - QLabel.x - 2 - 16 - 12);

                        //QUI.PropertyField(list.GetArrayElementAtIndex(i), true, width - QLabel.x - 2 - 16 - 12);

                        if(QUI.ButtonMinus())
                        {
                            Undo.RecordObject(targetObject, "RemoveItem");
                            list.RemoveAt(i);
                            QUI.ExitGUI();
                        }

                        QUI.Space(8);
                    }
                    QUI.EndHorizontal();
                }

                QUI.BeginHorizontal(width);
                {
                    QUI.FlexibleSpace();

                    QUI.BeginVertical(16, QUI.SingleLineHeight);
                    {
                        if(QUI.ButtonPlus())
                        {
                            Undo.RecordObject(targetObject, "AddItem");
                            list.Add("");
                        }
                        QUI.Space(1);
                    }
                    QUI.EndVertical();

                    QUI.Space(4);
                }
                QUI.EndHorizontal();
            }
            QUI.EndVertical();
        }
        public static void DrawCollapsableStringList(string barTitle, AnimBool show, QColors.Color color, List<string> list, Object targetObject, float width, float barHeight, string emptyMessage = "List is empty...")
        {
            tempFloat = (20 + 2 + 18 * (list.Count + 1) + 2) * show.faded; //background height
            if(show.faded > 0.1f)
            {
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(4 * show.faded);
                    QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, color), width - 4, tempFloat);
                }
                QUI.EndHorizontal();
                QUI.Space(-tempFloat);
            }

            if(QUI.GhostBar(barTitle, color, show, width, barHeight))
            {
                show.target = !show.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(8 * show.faded);
                if(QUI.BeginFadeGroup(show.faded))
                {
                    QUI.BeginVertical(width - 8);
                    {

                        QUI.Space(2);
                        QUI.DrawStringList(list, targetObject, (width - 8) * show.faded, emptyMessage);
                        QUI.Space(2);

                        QUI.Space(4 * show.faded);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();
        }

        #endregion

        #region QObjectList
        public static void QObjectList(string barText, SerializedProperty serializedProperty, string emptyListText, AnimBool show, float width, float barHeight)
        {
            tempFloat = (20 + 2 + 18 * (serializedProperty.arraySize + 1) + 2) * show.faded; //background height
            if(show.faded > 0.2f) //draw the background
            {
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(4 * show.faded);
                    QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, serializedProperty.arraySize > 0 ? QColors.Color.Blue : QColors.Color.Gray), width - 4, tempFloat);
                }
                QUI.EndHorizontal();
                QUI.Space(-tempFloat);
            }

            if(QUI.SlicedBar(barText, serializedProperty.arraySize > 0 ? QColors.Color.Blue : QColors.Color.Gray, show, width, barHeight))
            {
                show.target = !show.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(8 * show.faded);
                if(QUI.BeginFadeGroup(show.faded))
                {
                    QUI.BeginVertical(width - 8);
                    {

                        QUI.Space(2);

                        if(serializedProperty.arraySize == 0)
                        {
                            QUI.BeginHorizontal(width - 8);
                            {
                                QLabel.text = emptyListText + " Click [+] to start...";
                                QLabel.style = Style.Text.Help;
                                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                                {
                                    QUI.Label(QLabel);
                                    QUI.Space(2);
                                }
                                QUI.EndVertical();

                                QUI.FlexibleSpace();

                                QUI.BeginVertical(16, QUI.SingleLineHeight);
                                {
                                    if(QUI.ButtonPlus())
                                    {
                                        serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
                                    }
                                    QUI.Space(1);
                                }
                                QUI.EndVertical();

                                QUI.Space(4);
                            }
                            QUI.EndHorizontal();
                        }
                        else
                        {
                            QUI.BeginVertical(width - 8);
                            {
                                QLabel.style = Style.Text.Help;
                                for(int i = 0; i < serializedProperty.arraySize; i++)
                                {
                                    QUI.BeginHorizontal(width - 8, QUI.SingleLineHeight);
                                    {
                                        QLabel.text = i.ToString();
                                        QUI.Label(QLabel);

                                        QUI.Space(2);

                                        QUI.PropertyField(serializedProperty.GetArrayElementAtIndex(i), width - QLabel.x - 2 - 16 - 12 - 8);

                                        if(QUI.ButtonMinus())
                                        {
                                            serializedProperty.DeleteArrayElementAtIndex(i);
                                        }

                                        QUI.Space(8);
                                    }
                                    QUI.EndHorizontal();
                                }

                                QUI.BeginHorizontal(width - 8);
                                {
                                    QUI.FlexibleSpace();

                                    QUI.BeginVertical(16, QUI.SingleLineHeight);
                                    {
                                        if(QUI.ButtonPlus())
                                        {
                                            serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
                                        }
                                        QUI.Space(1);
                                    }
                                    QUI.EndVertical();

                                    QUI.Space(4);
                                }
                                QUI.EndHorizontal();
                            }
                            QUI.EndVertical();

                            QUI.Space(16 * show.faded);
                        }
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();
        }
        #endregion
    }
}
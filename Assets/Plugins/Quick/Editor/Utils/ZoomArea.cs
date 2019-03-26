// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using QuickEngine.Extensions;

namespace QuickEditor.Utils
{
    public class ZoomArea : MonoBehaviour
    {
        private const float EditorWindowTabHeight = 21f;
        private static Matrix4x4 _previousGuiMatrix;

        /// <summary>
        /// When ZoomArea.Begin is called you must not be in between a GUI.Begin/EndGroup or GUILayout.Begin/EndArea call
        /// or else our little trick of calling GUI.EndGroup to end the implicit group that every Unity editor window has won't work.
        /// </summary>
        /// <param name="zoomScale"></param>
        /// <param name="screenCoordsArea"></param>
        public static Rect Begin(float zoomScale, Rect screenCoordsArea)
        {
            GUI.EndGroup();  //End the group Unity begins automatically for an EditorWindow to clip out the window tab. This allows us to draw outside of the size of the EditorWindow.

            Rect clippedArea = screenCoordsArea.ScaleSizeBy(1.0f / zoomScale, screenCoordsArea.TopLeft());
            clippedArea.y += EditorWindowTabHeight;
            GUI.BeginGroup(clippedArea);

            _previousGuiMatrix = GUI.matrix;
            Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
            GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

            return clippedArea;
        }

        public static void End()
        {
            GUI.matrix = _previousGuiMatrix;
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, EditorWindowTabHeight, Screen.width, Screen.height));
        }
    }
}

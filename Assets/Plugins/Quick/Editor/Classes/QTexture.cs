// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using UnityEditor;
using UnityEngine;

namespace QuickEditor
{
    [Serializable]
    public class QTexture
    {
        private string _filePath = "";

        private string _normalFileName = "";
        private Texture _normal;
        public Texture normal { get { if (_normal == null) { _normal = GetTexture(_normalFileName, _filePath); } return _normal; } }
        public Texture2D normal2D { get { return (Texture2D)normal; } }

        public Texture texture { get { return normal; } }
        public Texture2D texture2D { get { return normal2D; } }

        private string _activeFileName = "";
        private Texture _active;
        public Texture active { get { if (_active == null) { _active = GetTexture(_activeFileName, _filePath); } return _active; } }
        public Texture2D active2D { get { return (Texture2D)active; } }

        private string _hoverFileName = "";
        private Texture _hover;
        public Texture hover { get { if (_hover == null) { _hover = GetTexture(_hoverFileName, _filePath); } return _hover; } }
        public Texture2D hover2D { get { return (Texture2D)hover; } }

        public QTexture(string filePath, string normalFileName, string activeFileName = "", string hoverFileName = "")
        {
            FilePath = filePath;
            NormalFileName = normalFileName;
            ActiveFileName = activeFileName;
            if (string.IsNullOrEmpty(ActiveFileName) && !string.IsNullOrEmpty(NormalFileName)) { ActiveFileName = NormalFileName + "Active"; }
            HoverFileName = hoverFileName;
            if (string.IsNullOrEmpty(HoverFileName) && !string.IsNullOrEmpty(NormalFileName)) { HoverFileName = NormalFileName + "Hover"; }
        }

        public static Texture GetTexture(string fileName, string path) { return AssetDatabase.LoadAssetAtPath<Texture>(path + fileName + ".png"); }

        public string FilePath { get { return _filePath; } set { _filePath = value; } }
        public string NormalFileName { get { return _normalFileName; } set { _normalFileName = value; } }
        public string ActiveFileName { get { return _activeFileName; } set { _activeFileName = value; } }
        public string HoverFileName { get { return _hoverFileName; } set { _hoverFileName = value; } }

        public bool HasFilePath { get { return !string.IsNullOrEmpty(FilePath); } }
        public bool HasNormal { get { return !string.IsNullOrEmpty(NormalFileName) && normal != null; } }
        public bool HasTexture { get { return HasNormal; } }
        public bool HasActive { get { return !string.IsNullOrEmpty(ActiveFileName) && active != null; } }
        public bool HasHover { get { return !string.IsNullOrEmpty(HoverFileName) && hover != null; } }
    }
}

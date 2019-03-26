// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;
using QuickEngine.IO;

namespace QuickEditor.Utils
{
    public static class QAssets
    {
        /// <summary>
        /// Creates a new scriptable object asset at the specified path, with the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scriptableObject">This must be a class that is derived from ScriptableObject.</param>
        /// <param name="relativeFilePath">Usage: File.GetRelativeDirectoryPath("Quick")+"/Engine/Data/MyAssetFile.asset"</param>
        public static void SaveScriptableObject<T>(T scriptableObject, string relativeFilePath) where T : ScriptableObject
        {
            File.CreateDirectory(relativeFilePath);
#pragma warning disable 0219
            T asset = ScriptableObject.CreateInstance<T>();
#pragma warning restore 0219
            AssetDatabase.CreateAsset(scriptableObject, relativeFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Returns all the references to all the ScriptableObjects found at the relativeFolderPath.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativeFolderPath">Usage: File.GetRelativeDirectoryPath("Quick")+"/Engine/Data/"</param>
        public static Object[] GetScriptableObjects<T>(string relativeFolderPath)
        {
            return AssetDatabase.LoadAllAssetsAtPath(relativeFolderPath);
        }

        /// <summary>
        /// Returns the reference to a ScriptableObject found at the relativeFilePath.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativeFilePath">Usage: File.GetRelativeDirectoryPath("Quick")+"/Engine/Data/MyData.asset"</param>
        public static Object GetScriptableObject<T>(string relativeFilePath)
        {
            return AssetDatabase.LoadAssetAtPath(relativeFilePath, typeof(T));
        }


    }
}

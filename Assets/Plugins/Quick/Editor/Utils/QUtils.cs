// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace QuickEditor
{
    public class QUtils
    {
        /// <summary>
        /// Plays an audioClip in the editor. This is useful for previewing a sound in the inspector.
        /// </summary>
        /// <param name="clip">The AudioClip you want to play</param>
        public static void PlayAudioClip(AudioClip clip)
        {
            if(clip == null) { return; }
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod
                (
                    "PlayClip",
                    BindingFlags.Static | BindingFlags.Public,
                    null,
                    new System.Type[] { typeof(AudioClip) },
                    null
                );
            method.Invoke(null, new object[] { clip });
        }

        /// <summary>
        /// Stops all the audio clips that are currently playing in the editor.
        /// </summary>
        public static void StopAllClips()
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod
                (
                    "StopAllClips",
                    BindingFlags.Static | BindingFlags.Public,
                    null,
                    new System.Type[] { },
                    null
                );
            method.Invoke(null, new object[] { });
        }

        /// <summary>
        /// Created an unlinked copy of a prefab in the current scene, with the specified gameObjectName.
        /// </summary>
        /// <param name="prefabPath">Path to the prefab.</param>
        /// <param name="prefabName">Prefab name (without the '.prefab' extension).</param>
        /// <param name="gameObjectName">The name of the new gameObject.</param>
        public static void CreateGameObjectFromPrefab(string prefabPath, string prefabName, string gameObjectName)
        {
            var prefab = AssetDatabase.LoadAssetAtPath(prefabPath + prefabName + ".prefab", typeof(GameObject));
            if(prefab == null) { Debug.LogError("Could not find the " + prefabName + " prefab. It should be at " + prefabPath); return; }
            var go = UnityEngine.Object.Instantiate(prefab) as GameObject;
            go.name = gameObjectName;
            Undo.RegisterCreatedObjectUndo(go, "Created the '" + go.name + "' gameObject from the '" + prefabName + "' prefab");
            Selection.activeObject = go;
        }

        /// <summary>
        /// Adds a symbol to the currently active build target group.
        /// </summary>
        public static void AddScriptingDefineSymbol(string symbol)
        {
            AddScriptingDefineSymbol(symbol, GetActiveBuildTargetGroup());
        }

        /// <summary>
        /// Adds a symbol to the target buildTargetGroup.
        /// </summary>
        public static void AddScriptingDefineSymbol(string symbol, BuildTargetGroup buildTargetGroup)
        {
            List<string> list = GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if(list.Contains(symbol)) { return; }
            list.Add(symbol);
            SetScriptingDefineSymbolsForGroup(buildTargetGroup, list);
        }

        /// <summary>
        /// Adds a symbols array to the target buildTargetGroup.
        /// </summary>
        public static void AddScriptingDefineSymbols(string[] symbols)
        {
            AddScriptingDefineSymbols(symbols, GetActiveBuildTargetGroup());
        }

        /// <summary>
        /// Adds a symbols array to the target buildTargetGroup.
        /// </summary>
        public static void AddScriptingDefineSymbols(string[] symbols, BuildTargetGroup buildTargetGroup)
        {
            if(symbols == null || symbols.Length == 0) { return; }
            List<string> list = GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            for(int i = 0; i < symbols.Length; i++)
            {
                if(list.Contains(symbols[i])) { continue; }
                list.Add(symbols[i]);
            }
            SetScriptingDefineSymbolsForGroup(buildTargetGroup, list);
        }

        /// <summary>
        /// Removes a symbol to the currently active build target group.
        /// </summary>
        public static void RemoveScriptingDefineSymbol(string symbol)
        {
            RemoveScriptingDefineSymbol(symbol, GetActiveBuildTargetGroup());
        }

        /// <summary>
        /// Removes a symbol to the target build target group.
        /// </summary>
        public static void RemoveScriptingDefineSymbol(string symbol, BuildTargetGroup buildTargetGroup)
        {
            List<string> list = GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if(!list.Contains(symbol)) { return; }
            list.Remove(symbol);
            SetScriptingDefineSymbolsForGroup(buildTargetGroup, list);
        }

        /// <summary>
        /// Removes a symbols array to the currently active build target group.
        /// </summary>
        public static void RemoveScriptingDefineSymbols(string[] symbols)
        {
            RemoveScriptingDefineSymbols(symbols, EditorUserBuildSettings.selectedBuildTargetGroup);
        }

        /// <summary>
        /// Removes a symbols array to the target build target group.
        /// </summary>
        public static void RemoveScriptingDefineSymbols(string[] symbols, BuildTargetGroup buildTargetGroup)
        {
            if(symbols == null || symbols.Length == 0) { return; }
            List<string> list = GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            for(int i = 0; i < symbols.Length; i++)
            {
                if(list.Contains(symbols[i])) { list.Remove(symbols[i]); }
            }
            SetScriptingDefineSymbolsForGroup(buildTargetGroup, list);
        }

        /// <summary>
        /// Returns true if the symbols has already been added to the active build target group.
        /// </summary>
        public static bool DoesSymbolExist(string symbol)
        {
            string currentDefinedSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(GetActiveBuildTargetGroup());
            return currentDefinedSymbols.Contains(symbol);
        }

        /// <summary>
        /// Returns true if the symbols has already been added to target buildTargetGroup.
        /// </summary>
        public static bool DoesSymbolExist(string symbol, BuildTargetGroup buildTargetGroup)
        {
            string currentDefinedSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            return currentDefinedSymbols.Contains(symbol);
        }

        /// <summary>
        /// Returns the currently active BuildTargetGroup
        /// </summary>
        public static BuildTargetGroup GetActiveBuildTargetGroup()
        {
            switch(EditorUserBuildSettings.activeBuildTarget)
            {
#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX: return BuildTargetGroup.Standalone;  //Build a macOS standalone(Intel 64 - bit).
#else
                case BuildTarget.StandaloneOSXUniversal: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneOSXIntel: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneOSXIntel64: return BuildTargetGroup.Standalone;
                case BuildTarget.Tizen: return BuildTargetGroup.Tizen; //Build a Tizen player. --- is obsolete (Tzien has been removed in 2017.3)
#endif

                case BuildTarget.StandaloneWindows: return BuildTargetGroup.Standalone; //Build a Windows standalone.
                case BuildTarget.iOS: return BuildTargetGroup.iOS; //Build an iOS player.
                case BuildTarget.Android: return BuildTargetGroup.Android; //Build an Android .apk standalone app.
                case BuildTarget.StandaloneLinux: return BuildTargetGroup.Standalone; //Build a Linux standalone.
                case BuildTarget.StandaloneWindows64: return BuildTargetGroup.Standalone; //Build a Windows 64 - bit standalone.
                case BuildTarget.WebGL: return BuildTargetGroup.WebGL; //WebGL.
                case BuildTarget.WSAPlayer: return BuildTargetGroup.WSA; //Build an Windows Store Apps player.
                case BuildTarget.StandaloneLinux64: return BuildTargetGroup.Standalone; //Build a Linux 64 - bit standalone.
                case BuildTarget.StandaloneLinuxUniversal: return BuildTargetGroup.Standalone; //Build a Linux universal standalone.
                case BuildTarget.PSP2: return BuildTargetGroup.PSP2; //Build a PS Vita Standalone.
                case BuildTarget.PS4: return BuildTargetGroup.PS4; //Build a PS4 Standalone.
                case BuildTarget.XboxOne: return BuildTargetGroup.XboxOne; //Build a Xbox One Standalone.
                case BuildTarget.N3DS: return BuildTargetGroup.N3DS; //Build to Nintendo 3DS platform.

#if !UNITY_2018_1_OR_NEWER
                case BuildTarget.WiiU: return BuildTargetGroup.WiiU; //Build a Wii U standalone.
#endif

                case BuildTarget.tvOS: return BuildTargetGroup.tvOS; //Build to Apple's tvOS platform.
                case BuildTarget.Switch: return BuildTargetGroup.Switch; //Build a Nintendo Switch player.

#if UNITY_5_6 || UNITY_5_6_3
                case BuildTarget.SamsungTV: return BuildTargetGroup.SamsungTV;
#endif

                case BuildTarget.NoTarget: return BuildTargetGroup.Unknown;
                default: return BuildTargetGroup.Unknown;
            }
        }

        public static List<string> GetScriptingDefineSymbolsForGroup(BuildTargetGroup buildTargetGroup)
        {
            List<string> symbols = new List<string>();
            try
            {
                string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
                string[] array = defineSymbols.Split(';');
                if(array != null && array.Length > 0) symbols.AddRange(array);
            }
            catch(System.Exception)
            {
            }
            return symbols;
        }

        public static void SetScriptingDefineSymbolsForGroup(BuildTargetGroup buildTargetGroup, List<string> symbols)
        {
            symbols = CleanList(symbols);
            string symbolsString = string.Empty;
            if(symbols != null && symbols.Count > 0)
            {
                for(int i = 0; i < symbols.Count; i++)
                {
                    if(!string.IsNullOrEmpty(symbols[i]))
                        symbolsString += symbols[i] + ";";
                }
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbolsString);
        }

        /// <summary>
        /// Cleans the given list by removing any whitespaces, any duplicates and any empty entries
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> CleanList(List<string> list)
        {
            for(int i = 0; i < list.Count; i++) list[i] = Regex.Replace(list[i], @"\s+", ""); //remove whitespaces
            list = list.Distinct().ToList(); //remove duplicates
            list.RemoveAll(s => string.IsNullOrEmpty(s.Trim())); //remove empty entries
            return list;
        }

        /// <summary>
        /// Sends an email to bugreport@doozyentertainment.com
        /// </summary>
        public static void SendEmail(string from, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient smtpServer = new SmtpClient("smtpout.europe.secureserver.net", 80);
            smtpServer.Credentials = new System.Net.NetworkCredential("bugreport@doozyentertainment.com", "d00zyP1ay") as ICredentialsByHost;
            smtpServer.EnableSsl = false;
            ServicePointManager.ServerCertificateValidationCallback
                = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
            smtpServer.Send(mail);
            Debug.Log("[QuickEditor] Mail sent!");
        }
    }
}

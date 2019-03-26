// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if !(UNITY_5_6 && UNITY_WINRT)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#endif

namespace QuickEngine.Utils
{
    public static class QReflection
    {
#if !(UNITY_5_6 && UNITY_WINRT)
        public static List<string> AssemblyNames { get; private set; }
        public static Dictionary<string, Type> TypeCache { get; private set; }
        public static Dictionary<Assembly, List<string>> NameSpaceCache { get; private set; }
        public static Assembly[] Assemblies { get; private set; }

        static QReflection()
        {
            AssemblyNames = new List<string>();
            TypeCache = new Dictionary<string, Type>();
            NameSpaceCache = new Dictionary<Assembly, List<string>>();
            Assemblies = GetAllAssemblies();
        }

        public static void PrintManifestResources()
        {
            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach(var name in resourceNames)
            {
                Debug.Log(name);
            }
        }

        public static Type GetTypeByQualifiedName(string name)
        {
            try
            {
                Type type;
                TypeCache.TryGetValue(name, out type);
                if(ReferenceEquals(type, null))
                {
                    if(Assemblies == null || Assemblies.Any()) { Assemblies = GetAllAssemblies(); }
                    foreach(var asm in Assemblies.Where(assembly => !AssemblyNames.Contains(assembly.FullName)))
                    {
                        AssemblyNames.Add(asm.FullName);
                    }
                    foreach(var assemblyName in AssemblyNames)
                    {
                        type = Type.GetType(name + "," + assemblyName);
                        if(!ReferenceEquals(type, null)) { break; }
                    }
                    if(ReferenceEquals(type, null))
                    {
                        foreach(var assemblyName in AssemblyNames)
                        {
                            string temp = assemblyName.Substring(0, assemblyName.IndexOf(",", StringComparison.Ordinal));
                            type = Type.GetType(temp + "." + name + "," + assemblyName);
                            if(!ReferenceEquals(type, null)) { break; }
                        }
                    }
                    if(ReferenceEquals(type, null))
                    {
                        return null;
                    }
                    TypeCache.Add(name, type);
                }
                return type;
            }
            catch(Exception e)
            {
                Debug.LogError(string.Format("QReflection - Get Type by Qualified Name : Can't find the type - {0} - with the exception." + e, name));
                return null;
            }
        }

        public static string GetQualifiedName(string name, string @namespace = "")
        {
            string qualifiedName = "";
            if(Assemblies == null || !Assemblies.Any()) { Assemblies = GetAllAssemblies(); }
            List<Type> types = new List<Type>();
            foreach(var asm in Assemblies) { types.AddRange(asm.GetTypes()); }
            foreach(var type in types)
            {
                if(!string.IsNullOrEmpty(type.AssemblyQualifiedName) &&
                   type.AssemblyQualifiedName.Contains(name) &&
                   type.AssemblyQualifiedName.Contains(@namespace))
                {
                    qualifiedName = type.AssemblyQualifiedName;
                    break;
                }
            }
            return qualifiedName;
        }

        public static Type GetType(string name, string @namespace = "")
        {
            if(Assemblies == null || !Assemblies.Any()) { Assemblies = GetAllAssemblies(); }
            List<Type> types = new List<Type>();
            foreach(var asm in Assemblies) { types.AddRange(asm.GetTypes()); }
            var typeFound = types.FirstOrDefault(type => type.AssemblyQualifiedName != null &&
                                                         type.AssemblyQualifiedName.Contains(name) &&
                                                         type.AssemblyQualifiedName.Contains(@namespace));
            if(typeFound != null && !string.IsNullOrEmpty(typeFound.AssemblyQualifiedName))
            {
                if(!TypeCache.ContainsKey(typeFound.AssemblyQualifiedName))
                {
                    TypeCache.Add(typeFound.AssemblyQualifiedName, typeFound);
                }
            }
            return typeFound;
        }

        public static List<string> GetNameSpaces(Assembly assembly)
        {
            List<string> names;
            if(NameSpaceCache.TryGetValue(assembly, out names)) { return names; }
            names = assembly.GetTypes().Select(t => t.Namespace).Distinct().ToList();
            if(!NameSpaceCache.ContainsKey(assembly)) { NameSpaceCache.Add(assembly, names); }
            return names;
        }

        public static Assembly[] GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public static object GetSingletonInstance(Type type, string singletonName, bool singletonIsProperty, BindingFlags flags = BindingFlags.Public | BindingFlags.Static)
        {
            if(type == null) { return null; }
            object instance;
            if(singletonIsProperty)
            {
                var instanceProperty = type.GetProperty(singletonName, flags);
                if(instanceProperty == null) { return null; }
                instance = instanceProperty.GetValue(null, null);
            }
            else
            {
                var instanceField = type.GetField(singletonName, flags);
                if(instanceField == null) { return null; }
                instance = instanceField.GetValue(null);
            }
            return instance;
        }

        public static object GetSingletonProperty(Type type, string singletonName, string propertyName, bool singletonIsProperty = true, BindingFlags singletonFlags = BindingFlags.Public | BindingFlags.Static)
        {
            object instance = GetSingletonInstance(type, singletonName, singletonIsProperty, singletonFlags);
            if(instance == null) { return null; }
            var propertyToSet = type.GetProperty(propertyName);
            if(propertyToSet == null) { return null; }
            return propertyToSet.GetValue(instance, null);
        }

        public static object GetSingletonField(Type type, string singletonName, string fieldName, bool singletonIsProperty = true, BindingFlags singletonFlags = BindingFlags.Public | BindingFlags.Static)
        {
            object instance = GetSingletonInstance(type, singletonName, singletonIsProperty, singletonFlags);
            if(instance == null) { return null; }
            var fieldToSet = type.GetField(fieldName);
            if(fieldToSet == null) { return null; }
            return fieldToSet.GetValue(instance);
        }

        public static bool SetSingletonProperty(Type type, string singletonName, string propertyName, object value, bool singletonIsProperty = true, BindingFlags singletonFlags = BindingFlags.Public | BindingFlags.Static)
        {
            object instance = GetSingletonInstance(type, singletonName, singletonIsProperty, singletonFlags);
            if(instance == null) { return false; }
            var propertyToSet = type.GetProperty(propertyName);
            if(propertyToSet == null) { return false; }
            propertyToSet.SetValue(instance, value, null);
            return true;
        }

        public static bool SetSingletonField(Type type, string singletonName, string fieldName, object value, bool singletonIsProperty = true, BindingFlags singletonFlags = BindingFlags.Public | BindingFlags.Static)
        {
            object instance = GetSingletonInstance(type, singletonName, singletonIsProperty, singletonFlags);
            if(instance == null) { return false; }
            var fieldToSet = type.GetField(fieldName);
            if(fieldToSet == null) { return false; }
            fieldToSet.SetValue(instance, value);
            return true;
        }
#endif
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UdonSharp;
using VRC.Udon;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using VRC.SDKBase.Editor.BuildPipeline;
#endif


namespace Thry.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class SingletonAttribute : PropertyAttribute
    {
        public bool ShowInSimpleUI = true;

        public SingletonAttribute()
        {
        }

        public SingletonAttribute(bool showInSimpleUI)
        {
            ShowInSimpleUI = showInSimpleUI;
        }
        public int callbackOrder => -50;

#if UNITY_EDITOR
        class SingletonBuildCallback : IVRCSDKBuildRequestedCallback
        {
            public int callbackOrder => -40;

            public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
            {
                return CheckForDuplicates();
            }
        }

        static Type[] s_singletonTypes;
        static Type[] s_singletonTypesForUI;

        public static Type[] SingletonTypesForSimpleUI { get { return s_singletonTypesForUI; } }


        [InitializeOnLoadMethod]
        static void InitAssertion()
        {
            IEnumerable<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Distinct();
            s_singletonTypes = allTypes.Where(t => t.GetCustomAttribute<SingletonAttribute>() != null).ToArray();
            s_singletonTypesForUI = s_singletonTypes.Where(t => t.GetCustomAttribute<SingletonAttribute>().ShowInSimpleUI).ToArray();

            EditorApplication.playModeStateChanged += OnPlaymodeChanged;
        }

        [PostProcessSceneAttribute(-40)]
        public static void AutoReference()
        {
            Dictionary<Type, UdonSharpBehaviour> singletonsInScene = s_singletonTypes.Select(t => (UdonSharpBehaviour)GameObject.FindObjectOfType(t, true))
                .Where(b => b != null).ToDictionary(o => o.GetType());

            UdonSharpBehaviour[] behvaviours = GameObject.FindObjectsByType<UdonSharpBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            IEnumerable<Type> types = behvaviours.Select(b => b.GetType()).Distinct();
            Dictionary<Type, string[]> typeToFields = new Dictionary<Type, string[]>();
            foreach (Type type in types)
            {
                // find any fields singleton type 
                IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).
                    Where(f => singletonsInScene.ContainsKey(f.FieldType));
                
                foreach(UdonSharpBehaviour b in behvaviours.Where(b => b.GetType() == type))
                {
                    foreach(FieldInfo f in fields)
                    {
                        f.SetValue(b, singletonsInScene[f.FieldType]);
                    }
                }
            }
        }

        static void OnPlaymodeChanged(PlayModeStateChange playModeState)
        {
            if (playModeState == PlayModeStateChange.ExitingEditMode)
                CheckForDuplicates();
        }

        static bool CheckForDuplicates()
        {
            foreach (Type singleton in s_singletonTypes)
            {
                SingletonAttribute att = singleton.GetCustomAttribute<SingletonAttribute>();
                // find all types of this singleton in the scene
                IEnumerable<GameObject> objs = GameObject.FindObjectsByType(singleton, FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .Where(g => g is MonoBehaviour).Select(g => (g as MonoBehaviour).gameObject);
                GameObject first_obj = objs.FirstOrDefault();
                // Check if more than one in scene
                if (objs.Count() > 1)
                {
                    Debug.LogError($"Scene must only contain one instance of '{singleton}'!");
                    // popup
                    bool removeAuto = EditorUtility.DisplayDialog($"{singleton} Error", $"Scene must only contain one instance of '{singleton}'!", "Auto fix.", "Ok");
                    if (removeAuto)
                    {
                        foreach (GameObject go in objs)
                        {
                            if (go != first_obj)
                            {
                                GameObject prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
                                if (prefab)
                                    GameObject.DestroyImmediate(prefab);
                                else
                                    GameObject.DestroyImmediate(go);
                            }
                        }
                    }
                    return false;
                }
            }
            return true;
        }
#endif
    }
}
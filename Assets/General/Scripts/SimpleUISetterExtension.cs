using UdonSharp;
using UnityEngine;
using VRC.Udon;
using UnityEngine.UIElements;


#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

namespace Thry.General
{
    public class SimpleUISetterExtension : UdonSharpBehaviour
    {
        [SerializeField, HideInInspector] private SimpleUIInput[] _simpleUIReferences;

        protected void UpdateUIForVarialbe(string varialbeName)
        {
            if (_simpleUIReferences == null) return;
            object val = GetProgramVariable(varialbeName);
            foreach (SimpleUIInput script in _simpleUIReferences)
            {
                if (script.VariableName == varialbeName)
                {
                    if(val.GetType() == typeof(bool))
                    {
                        script.SetBool((bool)val);
                    }
                    else if(val.GetType() == typeof(float))
                    {
                        script.SetFloat((float)val);
                    }
                }
            }
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        // register scene saving callback on compile
        [InitializeOnLoadMethod]
        private static void RegisterSceneSavingCallback()
        {
            EditorSceneManager.sceneSaving += OnBeforeSave;
        }

        static void OnBeforeSave(Scene scene, string path)
        {
            SimpleUIInput[] scipts = GameObject.FindObjectsOfType<SimpleUIInput>(true);
            SimpleUISetterExtension[] extensions = GameObject.FindObjectsOfType<SimpleUISetterExtension>(true);
            foreach(SimpleUISetterExtension extension in extensions)
            {
                extension._simpleUIReferences = scipts.Where(s => s.GameObjectName == extension.name)
                    .OrderBy(s => s.name).ToArray();
            }
        }
#endif
    }
}
using UdonSharp;
using UnityEngine;
using VRC.Udon;
using System;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
using UnityEditor.Callbacks;
#endif

namespace Thry.Udon.UI
{
    public abstract class SimpleUISetterExtension : ThryBehaviour
    {
        [SerializeField, HideInInspector] private SimpleUIInput[] _simpleUIReferences;
        [SerializeField, HideInInspector] private SimpleUISetterExtension[] _otherExtensions;
        
        [NonSerialized] public bool SimpleUI_BlockVariableChanged = false;

        protected void VariableChangedFromBehaviour(string varialbeName)
        {
            if (SimpleUI_BlockVariableChanged) return;
            object val = GetProgramVariable(varialbeName);
            
            foreach (SimpleUIInput script in _simpleUIReferences)
            {
                if (script.VariableName == varialbeName)
                    script.SetValue(val);
            }
            
            foreach (SimpleUISetterExtension extension in _otherExtensions)
            {
                extension.SimpleUI_BlockVariableChanged = true;
                extension.SetProgramVariable(varialbeName, val);
                extension.SimpleUI_BlockVariableChanged = false;
            }
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        [PostProcessSceneAttribute(-39)]
        static void OnPostprocessScene()
        {
            SimpleUIInput[] scipts = GameObject.FindObjectsOfType<SimpleUIInput>(true);
            SimpleUISetterExtension[] extensions = GameObject.FindObjectsOfType<SimpleUISetterExtension>(true);
            foreach(SimpleUISetterExtension extension in extensions)
            {
                UdonBehaviour myBehaviour = UdonSharpEditor.UdonSharpEditorUtility.GetBackingUdonBehaviour(extension);
                extension._simpleUIReferences = scipts.Where(s => 
                        s.UdonBehaviours.Any(s => s == myBehaviour)
                    ).OrderBy(s => s.name).ToArray();

                extension._otherExtensions = extension._simpleUIReferences.SelectMany(s => s.UdonBehaviours)
                    .Select(s => UdonSharpEditor.UdonSharpEditorUtility.GetProxyBehaviour(s)).Distinct()
                    .Where(s => s != null && s.GetType() == extension.GetType()).Cast<SimpleUISetterExtension>().ToArray();
            }
        }
#endif
    }
}

using UdonSharp;
#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.General
{
    public class UI_Toggle_Adapter : UdonSharpBehaviour
    {
        public bool _isThryAdapter = true;
        public Toggle _uiToggle;

        [HideInInspector]
        public float local_float;
        [HideInInspector]
        public bool local_bool;

        public void GetLocalBool()
        {
            local_bool = _uiToggle.isOn;
        }

        public void GetLocalFloat()
        {
            
        }

        public void SetLocalBool()
        {
            _uiToggle.isOn = local_bool;
        }

        public void SetLocalFloat()
        {
        }
    }

#if !COMPILER_UDONSHARP && UNITY_EDITOR

    [CustomEditor(typeof(UI_Toggle_Adapter))]
    public class UI_Slider_Toggle_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("<size=20><color=#f542da>Toggle Adapter</color></size>", new GUIStyle(EditorStyles.label) { richText = true, alignment = TextAnchor.MiddleCenter }, GUILayout.Height(50));

            serializedObject.Update();
            UI_Toggle_Adapter action = (UI_Toggle_Adapter)target;

            action._uiToggle = (Toggle)EditorGUILayout.ObjectField(new GUIContent("Toggle"), action._uiToggle, typeof(Toggle), true);
        }
    }
#endif
}
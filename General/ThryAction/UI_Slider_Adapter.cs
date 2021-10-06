
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
    public class UI_Slider_Adapter : UdonSharpBehaviour
    {
        public bool _isThryAdapter = true;
        public Slider _uiSlider;
        [Header("Optional Handle Text")]
        public Text _uiSliderHandleText;
        public string _uiSliderHandlePrefix;
        public string _uiSliderHandlePostfix;

        [HideInInspector]
        public float local_float;
        [HideInInspector]
        public bool local_bool;

        public void GetLocalBool()
        {

        }

        public void GetLocalFloat()
        {
            local_float = _uiSlider.value;
            local_bool = _uiSlider.value == 1;
        }

        public void SetLocalBool()
        {

        }

        public void SetLocalFloat()
        {
            _uiSlider.value = local_float;
            if (_uiSliderHandleText != null) _uiSliderHandleText.text = _uiSliderHandlePrefix + local_float + _uiSliderHandlePostfix;
        }
    }

#if !COMPILER_UDONSHARP && UNITY_EDITOR

    [CustomEditor(typeof(UI_Slider_Adapter))]
    public class UI_Slider_Adapter_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("<size=20><color=#f542da>Slider Adapter</color></size>", new GUIStyle(EditorStyles.label) { richText = true, alignment = TextAnchor.MiddleCenter }, GUILayout.Height(50));

            serializedObject.Update();
            UI_Slider_Adapter action = (UI_Slider_Adapter)target;

            action._uiSlider = (Slider)EditorGUILayout.ObjectField(new GUIContent("Slider"), action._uiSlider, typeof(Slider), true);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Handle:");
            action._uiSliderHandlePrefix = EditorGUILayout.TextField(action._uiSliderHandlePrefix);
            action._uiSliderHandleText = (Text)EditorGUILayout.ObjectField(action._uiSliderHandleText, typeof(Text), true);
            action._uiSliderHandlePostfix = EditorGUILayout.TextField(action._uiSliderHandlePostfix);
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
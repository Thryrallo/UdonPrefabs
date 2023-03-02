
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
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class TA_Label : UdonSharpBehaviour
    {
        public ThryAction Action;
        public UnityEngine.UI.Text Label;
        public string Formatting = "";
        public string Prefix;
        public string Postfix;

        private void Start()
        {
            Action.RegsiterAdapter(this);
        }

        [HideInInspector]
        public float local_float;
        [HideInInspector]
        public bool local_bool;

        public void SetAdapterBool()
        {
            Label.text = Prefix + (local_bool ? "True" : "False") + Postfix;
        }

        public void SetAdapterFloat()
        {
            Label.text = Prefix + local_float.ToString(Formatting) + Postfix;
        }
    }

#if !COMPILER_UDONSHARP && UNITY_EDITOR

    [CustomEditor(typeof(TA_Label))]
    public class TA_Label_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("<size=20><color=#f542da>Label</color></size>", new GUIStyle(EditorStyles.label) { richText = true, alignment = TextAnchor.MiddleCenter }, GUILayout.Height(50));

            serializedObject.Update();
            TA_Label action = (TA_Label)target;
            if (!ThryActionEditor.MakeSureItsAnUdonBehaviour(action)) return;

            action.Label = (UnityEngine.UI.Text)EditorGUILayout.ObjectField(new GUIContent("Label"), action.Label, typeof(UnityEngine.UI.Text), true);
            action.Action = (ThryAction)EditorGUILayout.ObjectField(new GUIContent("Thry Action"), action.Action, typeof(ThryAction), true);

            EditorGUILayout.Space();
            action.Formatting = EditorGUILayout.TextField(new GUIContent("Formatting"), action.Formatting);
            action.Prefix = EditorGUILayout.TextField(new GUIContent("Prefix"), action.Prefix);
            action.Postfix = EditorGUILayout.TextField(new GUIContent("Postfix"), action.Postfix);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
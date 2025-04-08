#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
using UnityEditorInternal;
#endif

using System.Collections.Generic;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;

namespace Thry.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ToggleManager : SimpleUISetterExtension
    {
        protected override string LogPrefix => "Thry.ToggleManager";

        [SerializeField] private ToggleSection _toggleSectionPrefab;
        [SerializeField] private string[] _sectionNames;
        [SerializeField] private string[][] _sectionToggleNames;
        [SerializeField] private bool[][] _sectionDefaultValues;
        [SerializeField] private Object[][][] _sectionTargets;
        void Start()
        {
            if (_sectionNames.Length != _sectionTargets.Length)
            {
                Log(LogLevel.Warning, $"Section names and targets length mismatch. Please check the configuration.");
                return;
            }

            for (int i = 0; i < _sectionNames.Length; i++)
            {
                ToggleSection section = _toggleSectionPrefab.SetupNewSection(_sectionNames[i]);
                for (int j = 0; j < _sectionToggleNames[i].Length; j++)
                {
                    section.AddToggle(_sectionToggleNames[i][j], _sectionDefaultValues[i][j], _sectionTargets[i][j]);
                }
            }
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        class SectionData
        {
            public string Name = "New Section";
            public List<ToggleData> Toggles = new List<ToggleData>();
            public float GetHeight() => EditorGUIUtility.singleLineHeight * 5 + 10 + Toggles.Sum(t => t.GetHeight());
        }

        class ToggleData
        {
            public string Name = "New Toggle";
            public bool DefaultValue = false;
            public List<Object> Targets = new List<Object>();

            public float GetHeight() => EditorGUIUtility.singleLineHeight * 6 + 10 + Mathf.Max(0, Targets.Count-1) * (EditorGUIUtility.singleLineHeight + 2);
        }

        [CustomEditor(typeof(ToggleManager))]
        public class ToggleManagerEditor : Editor
        {
            bool _showBaseUI = false;
            private List<SectionData> _sections;

            private void LoadSectionData(ToggleManager toggleManager)
            {
                _sections = new List<SectionData>();
                for (int i = 0; i < toggleManager._sectionNames.Length; i++)
                {
                    SectionData section = new SectionData(){ Name = toggleManager._sectionNames[i] };
                    for (int j = 0; j < toggleManager._sectionToggleNames[i].Length; j++)
                    {
                        ToggleData toggle = new ToggleData()
                        {
                            Name = toggleManager._sectionToggleNames[i][j],
                            DefaultValue = toggleManager._sectionDefaultValues[i][j],
                            Targets = new List<Object>(toggleManager._sectionTargets[i][j])
                        };
                        section.Toggles.Add(toggle);
                    }
                    _sections.Add(section);
                }
            }

            private void SaveSectionData(ToggleManager toggleManager)
            {
                toggleManager._sectionNames = new string[_sections.Count];
                toggleManager._sectionToggleNames = new string[_sections.Count][];
                toggleManager._sectionDefaultValues = new bool[_sections.Count][];
                toggleManager._sectionTargets = new Object[_sections.Count][][];
                for (int i = 0; i < _sections.Count; i++)
                {
                    SectionData section = _sections[i];
                    toggleManager._sectionNames[i] = section.Name;
                    toggleManager._sectionToggleNames[i] = new string[section.Toggles.Count];
                    toggleManager._sectionDefaultValues[i] = new bool[section.Toggles.Count];
                    toggleManager._sectionTargets[i] = new Object[section.Toggles.Count][];
                    for (int j = 0; j < section.Toggles.Count; j++)
                    {
                        ToggleData toggle = section.Toggles[j];
                        toggleManager._sectionToggleNames[i][j] = toggle.Name;
                        toggleManager._sectionDefaultValues[i][j] = toggle.DefaultValue;
                        toggleManager._sectionTargets[i][j] = new Object[toggle.Targets.Count];
                        for (int k = 0; k < toggle.Targets.Count; k++)
                        {
                            toggleManager._sectionTargets[i][j][k] = toggle.Targets[k];
                        }
                    }
                }
            }

            private System.Type[] GetTypesOnGameObject(GameObject go)
            {
                System.Type[] types = go.GetComponents<Component>().
                    Select(c => c.GetType()).
                    Where(t => !t.IsSubclassOf(typeof(UdonSharpBehaviour))).
                    Append(typeof(GameObject)).
                    OrderBy(t => t.Name).ToArray();
                return types;
            }

            private GameObject GetGameObject(Object target)
            {
                return (target is GameObject) ? (GameObject)target : ((Component)target).gameObject;
            }

            private Object ObjectTypeDropdown(Rect rect, Object target)
            {
                if (target == null) return null;
                GameObject go = GetGameObject(target);
                System.Type[] types = GetTypesOnGameObject(go);
                int selectedTypeIndex = System.Array.IndexOf(types, target.GetType());
                
                selectedTypeIndex = EditorGUI.Popup(rect, selectedTypeIndex, types.Select(t => t.Name).ToArray());
                if(selectedTypeIndex < 0 || selectedTypeIndex >= types.Length)
                {
                    Debug.LogWarning($"Invalid type index selected. Returning original target.");
                    return target;
                }
                if(types[selectedTypeIndex] == typeof(GameObject))
                {
                    return go;
                }
                return go.GetComponent(types[selectedTypeIndex]);
            }

            public override void OnInspectorGUI()
            {
                _showBaseUI = EditorGUILayout.Foldout(_showBaseUI, "Internal", true);
                if (_showBaseUI)
                {
                    base.OnInspectorGUI();
                }
                EditorGUILayout.Space();

                ToggleManager toggleManager = (ToggleManager)target;
                if(_sections == null) LoadSectionData(toggleManager);

                ReorderableList sectionList = new ReorderableList(_sections, typeof(SectionData), true, true, true, true);
                sectionList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Sections"); };
                sectionList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 2;
                    rect.height = EditorGUIUtility.singleLineHeight;
                    SectionData section = _sections[index];
                    section.Name = EditorGUI.TextField(rect, "Section Name", section.Name);
                    rect.y += EditorGUIUtility.singleLineHeight + 2;
                    ReorderableList toggleList = new ReorderableList(section.Toggles, typeof(ToggleData), true, true, true, true);
                    toggleList.drawHeaderCallback = (Rect toggleRect) => { EditorGUI.LabelField(toggleRect, "Toggles"); };
                    toggleList.drawElementCallback = (Rect toggleRect, int toggleIndex, bool isActiveToggle, bool isFocusedToggle) =>
                    {
                        ToggleData toggle = section.Toggles[toggleIndex];
                        toggleRect.height = EditorGUIUtility.singleLineHeight;
                        toggle.Name = EditorGUI.TextField(toggleRect, "Toggle Name", toggle.Name);
                        toggleRect.y += EditorGUIUtility.singleLineHeight + 2;
                        toggle.DefaultValue = EditorGUI.Toggle(toggleRect, "Default Value", toggle.DefaultValue);
                        toggleRect.y += EditorGUIUtility.singleLineHeight + 2;

                        ReorderableList targetsList = new ReorderableList(toggle.Targets, typeof(Object), true, true, true, true);
                        targetsList.drawHeaderCallback = (Rect targetsRect) => { EditorGUI.LabelField(targetsRect, "Targets"); };
                        targetsList.drawElementCallback = (Rect targetsRect, int targetIndex, bool isActiveTarget, bool isFocusedTarget) =>
                        {
                            targetsRect.width -= 150;
                            toggle.Targets[targetIndex] = EditorGUI.ObjectField(targetsRect, toggle.Targets[targetIndex], typeof(Object), true);
                            targetsRect.x += targetsRect.width + 5;
                            targetsRect.width = 145;
                            toggle.Targets[targetIndex] = ObjectTypeDropdown(targetsRect, toggle.Targets[targetIndex]);
                        };
                        targetsList.elementHeightCallback = (int targetIndex) => EditorGUIUtility.singleLineHeight + 2;
                        targetsList.DoList(toggleRect);
                    };
                    toggleList.elementHeightCallback = (int toggleIndex) => _sections[index].Toggles[toggleIndex].GetHeight();
                    toggleList.DoList(rect);
                };
                sectionList.elementHeightCallback = (int index) => _sections[index].GetHeight();
                sectionList.DoLayoutList();

                if (GUILayout.Button("Apply Changes"))
                {
                    SaveSectionData(toggleManager);
                    EditorUtility.SetDirty(toggleManager);
                }
            }
        }
#endif
    }
}

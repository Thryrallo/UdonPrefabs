
using UdonSharp;
using UnityEngine;
using VRC.Udon;
using VRC.SDKBase;
using Thry.CustomAttributes;
using UnityEngine.UI;
using Thry.UI;
using VRC.Udon.Common;


#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
using System.Reflection;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine.Events;
using UdonSharpEditor;
#endif

namespace Thry.Udon.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class SimpleUIInput : UdonSharpBehaviour
    {
        // user configured
        public UdonBehaviour[] UdonBehaviours;
        public string ScriptTypeFullName;
        public string VariableName;
        public bool NetworkVariable;
        public string EventName;
        
        // Set by postprocessor
        [HideInInspector] public SimpleUIInput[] LinkedInputs;
        [HideInInspector, SerializeField] ClickSound _clickSound;
        [HideInInspector, SerializeField] bool _isUdonTargetAvailable = false;
        [HideInInspector, SerializeField] bool _isVariableTargetAvailable = false;
        [HideInInspector, SerializeField] bool _isEventTargetAvailable = false;

        // Set on start
        Animator _optionalAnimator;
        private bool _isInit = false;
        protected bool _vibrationFeedback = true;

        // Set by other scripts
        protected bool _supressExecutes = false;

        protected abstract System.Type VariableType { get; }
        protected abstract void Init();
        protected abstract object UIValue { get; set; }

        private void InternalInit(bool updateOptionals)
        {
            if (_isInit) return;
            _isInit = true;

            _optionalAnimator = gameObject.GetComponent<Animator>();
            
            Init();

            FillVariableReference();

            // Is in Start. Could be in OnEnable, but not needed. Should never desync
            bool changed = false;
            if (_isVariableTargetAvailable)
            {
                changed = UIValue != UdonBehaviours[0].GetProgramVariable(VariableName);
                UIValue = UdonBehaviours[0].GetProgramVariable(VariableName);
            }
            
            if(updateOptionals)
                UpdateOptionals(false, changed);
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        private void FillAfterAutoReference()
        {
            UdonBehaviours = UdonBehaviours.Where(u => u != null).ToArray();
            _isUdonTargetAvailable = UdonBehaviours.Length > 0;
            _isVariableTargetAvailable = false;
            _isEventTargetAvailable = false;
            if (_isUdonTargetAvailable)
            {
                _isEventTargetAvailable = _isUdonTargetAvailable && !string.IsNullOrEmpty(EventName);
            }
        }
#endif
        private void FillVariableReference()
        {
            if (!_isUdonTargetAvailable) return;
            if (string.IsNullOrEmpty(VariableName)) return;
            // Need to get type this way because after SetProgramVariable the GetProgramVariableType function only returns object 
            object varValue = UdonBehaviours[0].GetProgramVariable(VariableName);
            if (varValue != null)
            {
                System.Type varType = varValue.GetType();
                //Debug.Log($"{this.name} : {varType} == {VariableType} : {varType == VariableType} {varType != null && varType.Equals(VariableType)}");
                _isVariableTargetAvailable = _isUdonTargetAvailable && varType == VariableType;
            }
        }

        void OnEnable()
        {
            if(_isInit) UpdateOptionals(false, false); 
            else InternalInit(true);
            SendCustomEvent("OnAfterEnable");
        }

        public virtual void SetValue(object value)
        {
            InternalInit(false);
            UIValue = value;
            UpdateOptionals(false, true);
        }

        private void SyncValueToOtherUIS()
        {
            object value = UIValue;
            foreach (SimpleUIInput input in LinkedInputs)
            {
                input.SetValue(value);
            }
        }

        public virtual void Execute()
        {
            if (_supressExecutes) return;
            InternalInit(false);

            if (_isVariableTargetAvailable)
            {
                //Debug.Log($"Execute {VariableName}: {UIValue} != {UdonBehaviour.GetProgramVariable(VariableName)}");
                //Debug.Log($"{UdonBehaviour.GetProgramVariable(VariableName).GetType()} == {UIValue.GetType()}");
                //Debug.Log(UdonBehaviour.GetProgramVariable(VariableName) == UIValue);
                //Debug.Log(UdonBehaviour.GetProgramVariable(VariableName).Equals(UIValue));

                if (UdonBehaviours[0].GetProgramVariable(VariableName).Equals(UIValue))
                    return;

                SyncValueToOtherUIS();

                foreach (UdonBehaviour b in UdonBehaviours)
                {
                    if(NetworkVariable) Networking.SetOwner(Networking.LocalPlayer, b.gameObject);
                    b.SetProgramVariable(nameof(SimpleUISetterExtension.SimpleUI_BlockVariableChanged), true);
                    b.SetProgramVariable(VariableName, UIValue);
                    b.SetProgramVariable(nameof(SimpleUISetterExtension.SimpleUI_BlockVariableChanged), false);
                    if (NetworkVariable) b.RequestSerialization();
                }
            }

            UpdateOptionals(true, true);
            PlayClick();
            
            if (_isEventTargetAvailable)
                foreach (UdonBehaviour b in UdonBehaviours)
                    b.SendCustomEvent(EventName);
        }

        protected virtual void UpdateOptionals(bool wasUserInput, bool valueChanged)
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            if (_optionalAnimator)
            {
                if (VariableType == typeof(bool) && gameObject.activeInHierarchy)
                    _optionalAnimator.SetBool("isOn", (bool)UIValue);
            }
        }

        protected void PlayClick()
        {
            if (_clickSound)
            {
                _clickSound.Play(transform.position, _vibrationFeedback);
            }
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP 

        const string CLICK_SOUND_PREFAB_GUID = "0f44d240ab12c5d4899ea4cf2bfbc60d";

        [CustomEditor(typeof(SimpleUIInput), true, isFallback = true)]
        public class SimpleUIInputEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                UdonSharpEditor.UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target);
                SimpleUIInput script = (SimpleUIInput)target;

                EditorGUI.BeginChangeCheck();

                bool showUdonBehvaiour = string.IsNullOrWhiteSpace(script.ScriptTypeFullName)
                    || EditorApplication.isPlaying;
                System.Type targetType = null;
                if (showUdonBehvaiour)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(script.UdonBehaviours)));
                    if (script.UdonBehaviours != null && script.UdonBehaviours.Length > 0 && script.UdonBehaviours[0] != null)
                    {
                        UdonSharpBehaviour proxy = UdonSharpEditor.UdonSharpEditorUtility.GetProxyBehaviour(script.UdonBehaviours[0]);
                        if (proxy != null)
                            targetType = proxy.GetType();
                    }
                }
                System.Type singletonType = DrawSingletonPopup(script);
                if (singletonType != null)
                    targetType = singletonType;
                if (targetType != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    DrawVariablePopup(script, targetType);
                    script.NetworkVariable = GUILayout.Toggle(script.NetworkVariable, "Networked", GUILayout.Width(85));
                    EditorGUILayout.EndHorizontal();

                    DrawEventPopup(script, targetType);
                }


                UnityEditor.EditorGUILayout.Space(20);
                // draw serialized property fields, except for the fields we're hiding
                DrawPropertiesExcluding(serializedObject, new string[] { "UdonBehaviours", "ScriptTypeFullName", "VariableName", "NetworkVariable", "EventName", "m_Script" });

                serializedObject.ApplyModifiedProperties();

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(target);
                }

                // If on the same gamoobject as slider, button or toggle, fill it automatically
                AutoAddUICall(script);
            }

            static void AutoAddUICall(SimpleUIInput script)
            {
                MonoBehaviour uiObjToAddCall = null;
                UdonBehaviour behaviourToCall = UdonSharpEditorUtility.GetBackingUdonBehaviour(script);
                uiObjToAddCall = script.GetComponent<Slider>();
                if (uiObjToAddCall == null)
                    uiObjToAddCall = script.GetComponent<Toggle>();
                if (uiObjToAddCall == null)
                    uiObjToAddCall = script.GetComponent<Button>();


                if (uiObjToAddCall != null && behaviourToCall != null)
                {
                    SerializedObject serialUIObj = new SerializedObject(uiObjToAddCall);

                    bool hasCall = false;

                    SerializedProperty serialPropCalls = null;
                    if (uiObjToAddCall.GetType() == typeof(Slider)) serialPropCalls = serialUIObj.FindProperty("m_OnValueChanged.m_PersistentCalls.m_Calls");
                    else if (uiObjToAddCall.GetType() == typeof(Toggle)) serialPropCalls = serialUIObj.FindProperty("onValueChanged.m_PersistentCalls.m_Calls");
                    else if (uiObjToAddCall.GetType() == typeof(Button)) serialPropCalls = serialUIObj.FindProperty("m_OnClick.m_PersistentCalls.m_Calls");

                    if (serialPropCalls != null)
                    {
                        for (int i = 0; i < serialPropCalls.arraySize; i++)
                        {
                            SerializedProperty item = serialPropCalls.GetArrayElementAtIndex(i);
                            if (item.FindPropertyRelative("m_Target").objectReferenceValue == behaviourToCall
                                && item.FindPropertyRelative("m_MethodName").stringValue == nameof(UdonBehaviour.SendCustomEvent)
                                && item.FindPropertyRelative("m_Arguments") != null
                                && item.FindPropertyRelative("m_Arguments.m_StringArgument").stringValue == nameof(script.Execute))
                                hasCall = true;
                            if (hasCall)
                            {
                                // Set callstate to runtime and editor
                                SerializedProperty callstate = item.FindPropertyRelative("m_CallState");
                                if (callstate.intValue != 1)
                                {
                                    callstate.intValue = 1;
                                    serialUIObj.ApplyModifiedProperties();
                                }
                            }
                        }
                    }
                    if (!hasCall)
                    {
                        UnityAction<string> methodDelegate = UnityAction.CreateDelegate(typeof(UnityAction<string>), behaviourToCall, typeof(UdonBehaviour).GetMethod(nameof(UdonBehaviour.SendCustomEvent))) as UnityAction<string>;
                        if (uiObjToAddCall.GetType() == typeof(Slider)) UnityEventTools.AddStringPersistentListener(((Slider)uiObjToAddCall).onValueChanged, methodDelegate, nameof(script.Execute));
                        else if (uiObjToAddCall.GetType() == typeof(Toggle)) UnityEventTools.AddStringPersistentListener(((Toggle)uiObjToAddCall).onValueChanged, methodDelegate, nameof(script.Execute));
                        else if (uiObjToAddCall.GetType() == typeof(Button)) UnityEventTools.AddStringPersistentListener(((Button)uiObjToAddCall).onClick, methodDelegate, nameof(script.Execute));
                    }
                }
            }

            const string NONE = "- None -";
            const string CUSTOM = "- Custom -";
            bool _customSingleton;
            protected System.Type DrawSingletonPopup(SimpleUIInput script)
            {
                string[] options = SingletonAttribute.SingletonTypesForSimpleUI.Select(t => t.FullName).ToArray();
                string value = SingletonPopup("Singleton", script.ScriptTypeFullName, options);

                if (_customSingleton)
                {
                    value = EditorGUILayout.TextField(script.ScriptTypeFullName);
                }
                script.ScriptTypeFullName = value;

                int index = System.Array.IndexOf(options, script.ScriptTypeFullName);
                if (index != -1)
                    return SingletonAttribute.SingletonTypesForSimpleUI[index];

                if (_customSingleton)
                {
                    System.Type type = System.Type.GetType(script.ScriptTypeFullName);
                    if(type != null)
                    {
                        return type;
                    }
                }
                return null;
            }

            static void DrawVariablePopup(SimpleUIInput script, System.Type targetType)
            {
                IEnumerable<FieldInfo> fields = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                string[] options = fields.Where(f =>
                    (f.FieldType == script.VariableType) ||
                    (f.FieldType.IsEnum && script.VariableType == typeof(int))
                    ).Select(f => f.Name).ToArray();
                script.VariableName = Popup("Variable Name", script.VariableName, options);
            }

            static void DrawEventPopup(SimpleUIInput script, System.Type targetType)
            {
                IEnumerable<MethodInfo> methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                IEnumerable<PropertyInfo> properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                IEnumerable<MethodInfo> removeMethods = typeof(UdonSharpBehaviour).GetMethods(BindingFlags.Public | BindingFlags.Instance);
                removeMethods = removeMethods.Concat(properties.Select(p => p.GetMethod)).Concat(properties.Select(p => p.SetMethod)).Where(m => m != null);
                methods = methods.Where(m => !removeMethods.Any(um => um.Name == m.Name));
                string[] options = methods.Select(m => m.Name).ToArray();
                script.EventName = Popup("Event Name", script.EventName, options);
            }

            string SingletonPopup(string displayname, string value, string[] options)
            {
                options = options.Prepend(NONE).Prepend(CUSTOM).ToArray();
                int index = 1;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    index = System.Array.IndexOf(options, value);
                    if (index == -1)
                    {
                        _customSingleton = true;
                    }
                }
                if (_customSingleton)
                    index = 0;

                EditorGUI.BeginChangeCheck();
                int newIndex = EditorGUILayout.Popup(displayname, index, options);
                if (EditorGUI.EndChangeCheck())
                {
                    _customSingleton = newIndex == 0;
                    if (options[newIndex] == NONE)
                        return string.Empty;
                    return options[newIndex];
                }
                return value;
            }

            static string Popup(string displayname, string value, string[] options)
            {
                options = options.Prepend(NONE).ToArray();
                int index = 0;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    index = System.Array.IndexOf(options, value);
                    if (index == -1)
                    {
                        options = options.Prepend(value).ToArray();
                        index = 0;
                    }
                }

                EditorGUI.BeginChangeCheck();
                int newIndex = EditorGUILayout.Popup(displayname, index, options);
                if (EditorGUI.EndChangeCheck())
                {
                    if (options[newIndex] == NONE)
                        return string.Empty;
                    return options[newIndex];
                }
                return value;
            }
        }

        [PostProcessSceneAttribute(-40)]
        public static void OnPostprocessScene()
        {
            SimpleUIInput[] scripts = GameObject.FindObjectsOfType<SimpleUIInput>(true);

            // Autofill Udonbehaviour references
            IEnumerable<string> targetTypes = scripts.Select(s => s.ScriptTypeFullName).Distinct().Where(s => !string.IsNullOrWhiteSpace(s));
            Dictionary<string, UdonBehaviour[]> singletons = new Dictionary<string, UdonBehaviour[]>();
            foreach (string targetType in targetTypes)
            {
                System.Type type = System.Type.GetType(targetType);
                if (type != null)
                {
                    IEnumerable<UdonSharpBehaviour> proxies = GameObject.FindObjectsByType(type, FindObjectsInactive.Include, FindObjectsSortMode.None).
                        Select(b => b as UdonSharpBehaviour);
                    UdonBehaviour[] targets = proxies.Select(p => UdonSharpEditorUtility.GetBackingUdonBehaviour(p)).ToArray();
                    singletons.Add(targetType, targets);
                }
            }

            // Replace singletons with udon behaviour for performance gains
            int successfullUdonBehaviourReferences = 0;
            int linkedInputs = 0;
            foreach (SimpleUIInput script in scripts)
            {
                if (singletons.TryGetValue(script.ScriptTypeFullName, out UdonBehaviour[] targets))
                {
                    script.UdonBehaviours = targets;
                    successfullUdonBehaviourReferences++;
                }
                script.FillAfterAutoReference();
            }

            // Autoreference linked inputs
            foreach (SimpleUIInput script in scripts)
            {
                if (script.UdonBehaviours.Length > 0)
                    script.LinkedInputs = scripts.Where(s => 
                        s != script && 
                        s.VariableName == script.VariableName && 
                        s.UdonBehaviours.Length > 0 && 
                        s.UdonBehaviours[0] == script.UdonBehaviours[0]
                        ).ToArray();
                else
                    script.LinkedInputs = new SimpleUIInput[0];
                
                if (script.LinkedInputs.Length > 0)
                    linkedInputs++;
            }

            // Make sure CLICK_SOUND_PREFAB_GUID prefab is in scene
            ClickSound clickSound = GameObject.FindObjectOfType<ClickSound>();
            if (!clickSound && string.IsNullOrWhiteSpace(AssetDatabase.GUIDToAssetPath(CLICK_SOUND_PREFAB_GUID)))
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(CLICK_SOUND_PREFAB_GUID));
                if(prefab && PrefabUtility.FindAllInstancesOfPrefab(prefab).Length == 0)
                {
                    GameObject newGO = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    newGO.transform.position = Vector3.zero;
                    newGO.transform.rotation = Quaternion.identity;
                    clickSound = newGO.GetComponent<ClickSound>();
                }
            }
            if(clickSound)
            {
                foreach (SimpleUIInput script in scripts)
                {
                    script._clickSound = clickSound;
                }
            }

            Debug.Log($"[SimpleUIInput] AutoReferencing Results: {successfullUdonBehaviourReferences} behvaiours, {linkedInputs} input linkings");
        }

#endif
    }
}
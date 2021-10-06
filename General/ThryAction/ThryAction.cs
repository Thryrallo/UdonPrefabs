
using UdonSharp;
#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UdonSharpEditor;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;
#endif
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Thry.General
{
    public class ThryAction : UdonSharpBehaviour
    {
        const int ACTION_TYPE_NONE = 0;
        const int ACTION_TYPE_CLAPPER = 1;
        const int ACTION_TYPE_TOGGLE = 2;
        const int ACTION_TYPE_SLIDER = 3;

        public int actionType;

        //Normal or Mirror Manager
        const int SPECIAL_ACTION_TYPE_NORMAL = 0;
        const int SPECIAL_ACTION_TYPE_MIRROR_MANAGER = 1;

        public int specialActionType;

        //Special Sync type
        const int HIVE_NONE = 0;
        const int HIVE_MASTER = 1;
        const int HIVE_REMOTE = 2;

        public int hiveType;
        public ThryAction _master;
        public ThryAction[] _remotes;

        //Syncing
        public bool is_synced;

        [UdonSynced]
        public bool synced_bool;

        [UdonSynced]
        public float synced_float;

        public bool prev_local_bool;
        public bool local_bool;
        public float local_float;


        //Clapper
        public int requiredClaps;
        public string desktopKey;

        //Toggle
        public Toggle _uiToggle;

        //Slider
        public Slider _uiSlider;
        public Text _uiSliderHandleText;
        public string _uiSliderHandlePrefix;
        public string _uiSliderHandlePostfix;

        //Action Togggles
        public GameObject[] toggleObjects;
        public GameObject[] toggleObjectsInverted;
        public VRC_Pickup[] togglePickups;
        public Collider[] toggleColliders;
        //Action Teleport
        public Transform teleportTarget;
        //Action Script Calls
        public GameObject[] udonBehaviours;
        public string[] udonEventNames;
        public string[] udonValueNames;
        //Action Animator driver
        public Animator[] animators;
        public int[] animatorParameterTypes;
        public string[] animatorParameterNames;

        //Requirement Colliders
        public Collider[] hasToBeInsideAnyCollider;
        //Requirement Master
        public bool hasToBeMaster;
        //Requirement Instance Owner
        public bool hasToBeInstanceOwner;
        //Requirement Object Owner
        public bool hasToBeOwner;
        public GameObject hasToBeOwnerGameobject;
        //Requirment PlayerList
        public string[] autherizedPlayerDisplayNames;
        private bool isAutherizedPlayer;

        //Mirror Manager
        public Transform[] mirrors;
        public float maximumOpenDistance = 5;

        private Transform selectedMirror;

        bool hasStartNotRun = true;
        bool doBlockOnInteract = false;

        private void Start()
        {
            //Do null check for toggle and slider
            if (actionType == ACTION_TYPE_TOGGLE)
            {
                if (_uiToggle == null)
                {

                    Debug.LogError($"[ThryAction][{name}] _uiToggle is null");
                    actionType = 0;
                }
                else
                {
                    local_bool = _uiToggle.isOn;
                    ExecuteToggles();
                    ExecuteAnimatorsBool();
                    if (Networking.IsOwner(gameObject))
                    {
                        synced_bool = local_bool;
                        RequestSerialization();
                    }
                }
            }
            if (actionType == ACTION_TYPE_SLIDER)
            {
                if (_uiSlider == null)
                {
                    Debug.LogError($"[ThryAction][{name}] _uiSlider is null");
                    actionType = 0;
                }
                else
                {
                    local_float = _uiSlider.value;
                    ExecuteAnimatorsFloat();
                    if (Networking.IsOwner(gameObject))
                    {
                        synced_float = local_float;
                        RequestSerialization();
                    }
                }
            }

            //Check Autherized name list
            string localName = Networking.LocalPlayer.displayName;
            foreach (string n in autherizedPlayerDisplayNames) if (localName == n) isAutherizedPlayer = true;
            if (autherizedPlayerDisplayNames.Length == 0) isAutherizedPlayer = true;

            //Register remote client
            if (hiveType == HIVE_REMOTE)
            {
                _master.RegisterRemote(this);
                SyncValuesFromMaster();
                //If has bool or float, make sure it is set everywhere
                if (actionType == ACTION_TYPE_SLIDER || actionType == ACTION_TYPE_TOGGLE)
                {
                    _ExecuteNormalActions();
                }
            }

            hasStartNotRun = false;
        }

        private void OnEnable()
        {
            if (hasStartNotRun) return;
            //Animators forget their params after they been disabled. This restores them after reenabling.
            ExecuteAnimatorsFloat();
            ExecuteAnimatorsBool();
        }

        public void RegisterRemote(ThryAction r)
        {
            if (_remotes == null)
            {
                _remotes = new ThryAction[1];
            }
            else
            {
                ThryAction[] old = _remotes;
                _remotes = new ThryAction[old.Length + 1];
                Array.Copy(old, _remotes, old.Length);
            }
            _remotes[_remotes.Length - 1] = r;
        }

        //========Exposed Call==========

        public override void Interact()
        {
            OnInteraction();
        }

        public void OnInteraction()
        {
            if (doBlockOnInteract) return;
            if (specialActionType == 0)
            {
                if (IsNormalRequirementMet()) _UpdateValuesAndExecuteNormals();
                else _ResetUI();
            }
            if (specialActionType == 1 && IsMirrorRequirementMet()) _ExecuteMirror();
        }

        //=========Mirror manager=========

        private bool IsMirrorRequirementMet()
        {
            float closestTransform = float.MaxValue;
            Transform selected = null;
            VRCPlayerApi.TrackingData trackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

            Vector3 lookDirection = (trackingData.rotation * Vector3.forward).normalized;
            Ray lookRay = new Ray(trackingData.position, lookDirection);
            foreach (Transform t in mirrors)
            {
                Plane mirrorPlane = new Plane(t.rotation * Vector3.back, t.position);
                float intersectionDistance;
                if (mirrorPlane.Raycast(lookRay, out intersectionDistance) == false)
                    continue;

                Vector3 intersection = trackingData.position + lookDirection * intersectionDistance;

                Vector3 toIntersectionVector = intersection - t.position;
                toIntersectionVector = Quaternion.Inverse(t.rotation) * toIntersectionVector;

                if (Mathf.Abs(toIntersectionVector.x) > t.lossyScale.x / 2 || Mathf.Abs(toIntersectionVector.y) > t.lossyScale.y / 2)
                    continue;

                if (intersectionDistance < closestTransform)
                {
                    closestTransform = intersectionDistance;
                    selected = t;
                }
            }

            if (selected != null && closestTransform < maximumOpenDistance)
            {
                //Open new mirror
                //Turn old mirror off
                if (selectedMirror != selected)
                {
                    if (selectedMirror != null) selectedMirror.gameObject.SetActive(false);
                    selectedMirror = selected;
                }
                return true;
            }
            return selectedMirror != null && selectedMirror.gameObject.activeSelf;
        }

        private void _ExecuteMirror()
        {
            selectedMirror.gameObject.SetActive(!selectedMirror.gameObject.activeSelf);
        }

        //=======Requirements========

        private bool IsNormalRequirementMet()
        {
            return IsInsideColliderMet() && isAutherizedPlayer
                && (!hasToBeMaster || Networking.IsMaster) //Master requirement 
                && (!hasToBeInstanceOwner || Networking.IsInstanceOwner) //IsInstanceOwner requirement 
                && (!hasToBeOwner || Networking.IsOwner(hasToBeOwnerGameobject)); //Owner requirement
        }

        private bool IsInsideColliderMet()
        {
            if (hasToBeInsideAnyCollider.Length == 0) return true;
            Vector3 position = Networking.LocalPlayer.GetPosition();
            foreach (Collider c in hasToBeInsideAnyCollider)
            {
                if (c.ClosestPoint(position) == position)
                {
                    return true;
                }
            }
            return false;
        }

        //========Actions==========

        //Syncing
        public override void OnDeserialization()
        {
            if (!is_synced) return;
            doBlockOnInteract = true;
            prev_local_bool = local_bool;
            if (actionType == ACTION_TYPE_SLIDER)
            {
                local_float = synced_float;
                _uiSlider.value = local_float;
                _ExecuteFloatOnlyActions();
            }
            else if (actionType == ACTION_TYPE_TOGGLE)
            {
                local_bool = synced_bool;
                _uiToggle.isOn = local_bool;
                _ExecuteBoolOnlyActions();
            }
            else
            {
                local_bool = synced_bool;
            }
            _ExecuteAlwaysActions();
            SyncRemotesIfHiveAndMaster();
            doBlockOnInteract = false;
        }

        public void _UpdateValuesAndExecuteNormals()
        {
            //Let Master handle it
            if (hiveType == HIVE_REMOTE)
            {
                if (actionType == ACTION_TYPE_SLIDER) _master._uiSlider.value = _uiSlider.value;
                else if (actionType == ACTION_TYPE_TOGGLE) _master._uiToggle.isOn = _uiToggle.isOn;
                else _master._UpdateValuesAndExecuteNormals();
                return;
            }
            if (_UpdateValues())
            {
                _ExecuteNormalActions();
                SyncRemotesIfHiveAndMaster();
            }
        }

        public void _ResetUI()
        {
            if (actionType == ACTION_TYPE_SLIDER) _uiSlider.value = local_float;
            else if (actionType == ACTION_TYPE_TOGGLE) _uiToggle.isOn = local_bool;
        }

        private void SyncRemotesIfHiveAndMaster()
        {
            //Sync to remote actions
            if (hiveType == HIVE_MASTER)
            {
                foreach (ThryAction a in _remotes)
                {
                    a.SyncValuesFromMaster();
                    a._ExecuteNormalActions();
                }
            }
        }

        public void SyncValuesFromMaster()
        {
            prev_local_bool = _master.prev_local_bool;
            local_float = _master.local_float;
            local_bool = _master.local_bool;
            _SyncRemoteUIElelemt();
        }

        private bool _UpdateValues()
        {
            prev_local_bool = local_bool;
            if (is_synced)
            {
                if (actionType == ACTION_TYPE_SLIDER)
                {
                    if (_uiSlider.value == local_float) return false; //prevent loops
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);
                    synced_float = _uiSlider.value;
                    local_float = synced_float;
                    local_bool = synced_float == 1;
                }
                else if (actionType == ACTION_TYPE_TOGGLE)
                {
                    if (_uiToggle.isOn == local_bool) return false; //prevent loops
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);
                    synced_bool = _uiToggle.isOn;
                    local_bool = synced_bool;
                }
                else
                {
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);
                    synced_bool = !synced_bool;
                    local_bool = synced_bool;
                }
                RequestSerialization();
            }
            else
            {
                if (actionType == ACTION_TYPE_SLIDER)
                {
                    if (_uiSlider.value == local_float) return false; //prevent loops
                    local_float = _uiSlider.value;
                    local_bool = synced_float == 1;
                }
                else if (actionType == ACTION_TYPE_TOGGLE)
                {
                    if (_uiToggle.isOn == local_bool) return false; //prevent loops
                    local_bool = _uiToggle.isOn;
                }
                else
                {
                    local_bool = !local_bool;
                }
            }
            return true;
        }

        public void _SyncRemoteUIElelemt()
        {
            if (actionType == ACTION_TYPE_SLIDER) _uiSlider.value = local_float;
            else if (actionType == ACTION_TYPE_TOGGLE) _uiToggle.isOn = local_bool;
        }

        public void _ExecuteNormalActions()
        {
            if (actionType == ACTION_TYPE_SLIDER)
            {
                _ExecuteFloatOnlyActions();
            }
            else if (actionType == ACTION_TYPE_TOGGLE)
            {
                _ExecuteBoolOnlyActions();
            }
            _ExecuteAlwaysActions();
        }

        //For actions that can be synced using float
        private void _ExecuteFloatOnlyActions()
        {
            ExecuteAnimatorsFloat();
            if (_uiSliderHandleText != null) _uiSliderHandleText.text = _uiSliderHandlePrefix + local_float + _uiSliderHandlePostfix;
        }

        //For actions that can be synced using bool
        private void _ExecuteBoolOnlyActions()
        {
        }

        //For Actions that are just one trigger
        private void _ExecuteAlwaysActions()
        {
            ExecuteUdonBehaviours();
            ExecuteAnimatorsTrigger();

            ExecuteToggles();
            ExecuteAnimatorsBool();
            if (teleportTarget != null) Networking.LocalPlayer.TeleportTo(teleportTarget.position, teleportTarget.rotation);
        }

        private void ExecuteUdonBehaviours()
        {
            for (int i = 0; i < udonBehaviours.Length; i++)
            {
                if (Utilities.IsValid(udonBehaviours[i]))
                {
                    UdonBehaviour u = (UdonBehaviour)udonBehaviours[i].GetComponent(typeof(UdonBehaviour));
                    if (Utilities.IsValid(u))
                    {
                        //Set Value
                        if (i < udonValueNames.Length)
                        {
                            if (actionType == ACTION_TYPE_TOGGLE) u.SetProgramVariable(udonValueNames[i], _uiToggle.isOn);
                            else if (actionType == ACTION_TYPE_SLIDER) u.SetProgramVariable(udonValueNames[i], _uiSlider.value);
                        }
                        if (i < udonEventNames.Length) u.SendCustomEvent(udonEventNames[i]);
                    }
                }
            }
        }

        private void ExecuteToggles()
        {
            if (actionType == ACTION_TYPE_TOGGLE)
            {
                foreach (GameObject o in toggleObjects) o.SetActive(local_bool);
                foreach (GameObject o in toggleObjectsInverted) o.SetActive(!local_bool);
                foreach (Collider c in toggleColliders) c.enabled = local_bool;
                foreach (VRC_Pickup p in togglePickups) p.pickupable = local_bool;
            }
            //Used to make sure the toggles are synced
            else if (prev_local_bool != local_bool)
            {
                foreach (GameObject o in toggleObjects) o.SetActive(!o.activeSelf);
                foreach (Collider c in toggleColliders) c.enabled = !c.enabled;
                foreach (VRC_Pickup p in togglePickups) p.pickupable = !p.pickupable;
            }
        }

        private void ExecuteAnimatorsTrigger()
        {
            for (int i = 0; i < animators.Length; i++)
            {
                if (animators[i] != null && animatorParameterNames[i].Length > 0)
                {
                    if (animatorParameterTypes[i] == (int)UnityEngine.AnimatorControllerParameterType.Trigger) animators[i].SetTrigger(animatorParameterNames[i]);
                }
            }
        }

        private void ExecuteAnimatorsFloat()
        {
            for (int i = 0; i < animators.Length; i++)
            {
                if (animators[i] != null && animatorParameterNames[i].Length > 0)
                {
                    if (animatorParameterTypes[i] == (int)UnityEngine.AnimatorControllerParameterType.Float) animators[i].SetFloat(animatorParameterNames[i], local_float);
                    else if (animatorParameterTypes[i] == (int)UnityEngine.AnimatorControllerParameterType.Int) animators[i].SetInteger(animatorParameterNames[i], (int)local_float);
                }
            }
        }
        private void ExecuteAnimatorsBool()
        {
            for (int i = 0; i < animators.Length; i++)
            {
                if (animators[i] != null && animatorParameterNames[i].Length > 0)
                {
                    if (animatorParameterTypes[i] == (int)UnityEngine.AnimatorControllerParameterType.Bool) animators[i].SetBool(animatorParameterNames[i], local_bool);
                }
            }
        }
    }

#if !COMPILER_UDONSHARP && UNITY_EDITOR

    [CustomEditor(typeof(ThryAction))]
    public class ThryActionEditor : Editor
    {
        enum ActionType { Button, Clapper, Toggle, Slider }
        enum SpecialBehaviourType { Normal, MirrorManager }
        enum HiveType { None, Master, Remote }


        bool headerClapper;
        bool headerAct;
        bool headerReq;

        ThryAction action;

        bool isNotInit = true;
        GUIStyle headerStyle;

        void Init()
        {
            headerStyle = new GUIStyle("ShurikenModuleTitle")
            {
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = 22,
                contentOffset = new Vector2(20f, -2f)
            };

            action = (ThryAction)target;

            AutoAddComponents();

            isNotInit = false;
        }

        void AutoAddComponents()
        {
            MonoBehaviour uiObjToAddCall = null;
            SerializedObject serialUIObj = null;
            if (action.GetComponent<Slider>() != null)
            {
                action._uiSlider = action.GetComponent<Slider>();
                action.actionType = 3;

                uiObjToAddCall = action._uiSlider;
                serialUIObj = new SerializedObject(action._uiSlider);
            }
            else if (action.GetComponent<Toggle>() != null)
            {
                action._uiToggle = action.GetComponent<Toggle>();
                action.actionType = 2;

                uiObjToAddCall = action._uiToggle;
                serialUIObj = new SerializedObject(action._uiToggle);
            }else if(action.GetComponent<Button>() != null)
            {
                action.actionType = 0;

                uiObjToAddCall = action.GetComponent<Button>();
                serialUIObj = new SerializedObject(uiObjToAddCall);
            }

            UdonBehaviour u = action.GetComponent<UdonBehaviour>();
            if(u != null)
            {
                u.SyncMethod = Networking.SyncType.Manual;
            }

            EditorUtility.SetDirty(target);

            //Add call
            if (serialUIObj != null)
            {
                bool hasCall = false;

                SerializedProperty serialPropCalls = null;
                if      (uiObjToAddCall.GetType() == typeof(Slider)) serialPropCalls = serialUIObj.FindProperty("m_OnValueChanged.m_PersistentCalls.m_Calls");
                else if (uiObjToAddCall.GetType() == typeof(Toggle)) serialPropCalls = serialUIObj.FindProperty("onValueChanged.m_PersistentCalls.m_Calls");
                else if (uiObjToAddCall.GetType() == typeof(Button)) serialPropCalls = serialUIObj.FindProperty("m_OnClick.m_PersistentCalls.m_Calls");

                if (serialPropCalls != null)
                {
                    for (int i = 0; i < serialPropCalls.arraySize; i++)
                    {
                        SerializedProperty item = serialPropCalls.GetArrayElementAtIndex(i);
                        if (item.FindPropertyRelative("m_Target").objectReferenceValue == UdonSharpEditorUtility.GetBackingUdonBehaviour(action)
                            && item.FindPropertyRelative("m_MethodName").stringValue == nameof(UdonBehaviour.SendCustomEvent)
                            && item.FindPropertyRelative("m_Arguments") != null
                            && item.FindPropertyRelative("m_Arguments.m_StringArgument").stringValue == nameof(action.OnInteraction))
                            hasCall = true;
                    }
                }
                if (!hasCall)
                {
                    UnityAction<string> methodDelegate = UnityAction.CreateDelegate(typeof(UnityAction<string>), UdonSharpEditorUtility.GetBackingUdonBehaviour(action), typeof(UdonBehaviour).GetMethod(nameof(UdonBehaviour.SendCustomEvent))) as UnityAction<string>;
                    if      (uiObjToAddCall.GetType() == typeof(Slider)) UnityEventTools.AddStringPersistentListener(((Slider)uiObjToAddCall).onValueChanged, methodDelegate, nameof(action.OnInteraction));
                    else if (uiObjToAddCall.GetType() == typeof(Toggle)) UnityEventTools.AddStringPersistentListener(((Toggle)uiObjToAddCall).onValueChanged, methodDelegate, nameof(action.OnInteraction));
                    else if (uiObjToAddCall.GetType() == typeof(Button)) UnityEventTools.AddStringPersistentListener(((Button)uiObjToAddCall).onClick, methodDelegate, nameof(action.OnInteraction));
                }
            }
        }

        bool synced;
        SpecialBehaviourType behaviourType;
        ActionType actionType;
        HiveType hiveType;

        public override void OnInspectorGUI()
        {
            // Draws the default convert to UdonBehaviour button, program asset field, sync settings, etc.
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;

            if(isNotInit) Init();

            serializedObject.Update();

            action = (ThryAction)target;

            EditorGUILayout.LabelField("<size=30><color=#f542da>Thry's Action Script</color></size>", new GUIStyle(EditorStyles.label) { richText = true, alignment = TextAnchor.MiddleCenter }, GUILayout.Height(50));

            //____________Behaviour__________
            EditorGUILayout.LabelField("Behaviour", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();

            actionType = (ActionType)EditorGUILayout.EnumPopup("Type", (ActionType)action.actionType);
            behaviourType = (SpecialBehaviourType)EditorGUILayout.EnumPopup("Special Behaviour", (SpecialBehaviourType)action.specialActionType);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(action, "Modify Type");
                action.actionType = (int)actionType;
                action.specialActionType = (int)behaviourType;
            }

            if(actionType == ActionType.Clapper)
            {
                ClapperGUI();
            }else if(actionType == ActionType.Toggle)
            {
                action._uiToggle = (Toggle)EditorGUILayout.ObjectField(new GUIContent("Toggle"), action._uiToggle, typeof(Toggle), true);
            }else if(actionType == ActionType.Slider)
            {
                action._uiSlider = (Slider)EditorGUILayout.ObjectField(new GUIContent("Slider"), action._uiSlider, typeof(Slider), true);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Handle:");
                action._uiSliderHandlePrefix = EditorGUILayout.TextField(action._uiSliderHandlePrefix);
                action._uiSliderHandleText = (Text)EditorGUILayout.ObjectField(action._uiSliderHandleText, typeof(Text), true);
                action._uiSliderHandlePostfix = EditorGUILayout.TextField(action._uiSliderHandlePostfix);
                EditorGUILayout.EndHorizontal();
            }

            GUISyncing();
            GUIHive();

            //__________Other UI____________
            if (behaviourType == SpecialBehaviourType.MirrorManager)
            {
                MirrorMangerGUI();
            }
            else
            {
                NormalGUI();
            }

            serializedObject.ApplyModifiedProperties();

        }

        bool headerSync;
        private void GUISyncing()
        {
            if (hiveType != HiveType.Remote && behaviourType != SpecialBehaviourType.MirrorManager)
            {
                headerSync = EditorGUILayout.BeginFoldoutHeaderGroup(headerSync, "Syncing", headerStyle);
                if (headerSync)
                {
                    EditorGUI.BeginChangeCheck();

                    synced = EditorGUILayout.Toggle("Is Synced", action.is_synced);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(action, "Modify Syncing");
                        action.is_synced = synced;
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            else
            {
                action.is_synced = false;
            }
        }

        bool headerHive;
        private void GUIHive()
        {
            if (actionType != ActionType.Clapper)
            {
                headerHive = EditorGUILayout.BeginFoldoutHeaderGroup(headerHive, "Hive Control", headerStyle);
                if (headerHive)
                {
                    EditorGUILayout.LabelField("Hive", EditorStyles.boldLabel);
                    EditorGUI.BeginChangeCheck();

                    hiveType = (HiveType)EditorGUILayout.EnumPopup("Hive Type", (HiveType)action.hiveType);
                    if (hiveType == HiveType.Remote) action._master = (ThryAction)EditorGUILayout.ObjectField("Master Action", action._master, typeof(ThryAction), true);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(action, "Modify Hive");
                        action.hiveType = (int)hiveType;
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            else
            {
                action.hiveType = 0;
            }
        }

        private void ClapperGUI()
        {
            headerClapper = EditorGUILayout.BeginFoldoutHeaderGroup(headerClapper, "Clapper Settings", headerStyle);
            if (headerClapper)
            {
                EditorGUI.indentLevel += 1;
                action.requiredClaps = EditorGUILayout.IntField("Required Claps", action.requiredClaps);
                action.desktopKey = EditorGUILayout.TextField("Desktop Key", action.desktopKey);
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void ArrayGUI(string name, string text, string tooltip = "")
        {
            var property = serializedObject.FindProperty(name);
            EditorGUILayout.PropertyField(property, new GUIContent(text, tooltip));
        }

        struct ArrayData
        {
            public string title;
            public string tooltip;
            public UnityEngine.Object[] unityA;
            public int[] intA;
            public float[] floatA;
            public string[] stringA;
            public int type;
            public Type enumType;

            public ArrayData(string title, string tooltip, UnityEngine.Object[] a, Type t)
            {
                this.title = title;
                this.tooltip = tooltip;
                this.type = 0;
                this.unityA = a == null ? (UnityEngine.Object[])Array.CreateInstance(t, 0) : a;
                this.intA = null;
                this.floatA = null;
                this.stringA = null;
                this.enumType = null;
            }

            public ArrayData(string title, string tooltip, int[] a)
            {
                this.title = title;
                this.tooltip = tooltip;
                this.type = 1;
                this.unityA = null;
                this.intA = a == null ? new int[0] : a;
                this.floatA = null;
                this.stringA = null;
                this.enumType = null;
            }

            public ArrayData(string title, string tooltip, int[] a, Type enumType)
            {
                this.title = title;
                this.tooltip = tooltip;
                this.type = 1;
                this.unityA = null;
                this.intA = a == null ? new int[0] : a;
                this.floatA = null;
                this.stringA = null;
                this.enumType = enumType;
            }

            public ArrayData(string title, string tooltip, float[] a)
            {
                this.title = title;
                this.tooltip = tooltip;
                this.type = 2;
                this.unityA = null;
                this.intA = null;
                this.floatA = a == null ? new float[0] : a;
                this.stringA = null;
                this.enumType = null;
            }

            public ArrayData(string title, string tooltip, string[] a)
            {
                this.title = title;
                this.tooltip = tooltip;
                this.type = 3;
                this.unityA = null;
                this.intA = null;
                this.floatA = null;
                this.stringA = a==null?new string[0]:a;
                this.enumType = null;
            }

            public int Length()
            {
                if (type == 0) return unityA.Length;
                if (type == 1) return intA.Length;
                if (type == 2) return floatA.Length;
                if (type == 3) return stringA.Length;
                return 0;
            }

            public void NewLength(int l)
            {
                if(type == 0)
                {
                    UnityEngine.Object[] old = unityA;
                    unityA = (UnityEngine.Object[])Array.CreateInstance(unityA.GetType().GetElementType(), l);
                    Array.Copy(old, unityA, Math.Min(l, old.Length));
                }else if (type == 1)
                {
                    int[] old = intA;
                    intA = new int[l];
                    Array.Copy(old, intA, Math.Min(l, old.Length));
                }else if (type == 2)
                {
                    float[] old = floatA;
                    floatA = new float[l];
                    Array.Copy(old, floatA, Math.Min(l, old.Length));
                }else if (type == 3)
                {
                    string[] old = stringA;
                    stringA = new string[l];
                    Array.Copy(old, stringA, Math.Min(l, old.Length));
                }
            }

            public Enum GetEnumValue(int index)
            {
                int i = intA[index];
                if(Enum.IsDefined(enumType, i) == false)
                {
                    i = 0;
                }
                return (Enum)Enum.ToObject(enumType, i);
            }
        }

        private ArrayData[] ArraysGUI(params ArrayData[] arrays)
        {
            EditorGUI.BeginChangeCheck();
            int length = EditorGUILayout.IntField(arrays[0].Length());

            if(EditorGUI.EndChangeCheck() || arrays.Any(a => a.Length() < length)){
                for (int i = 0; i < arrays.Length; i++)
                {
                    arrays[i].NewLength(length);
                }
            }
            Rect headerR = EditorGUILayout.GetControlRect(true);
            headerR.width = headerR.width / arrays.Length;
            for (int a = 0; a < arrays.Length; a++)
            {
                EditorGUI.LabelField(headerR, new GUIContent(arrays[a].title, arrays[a].tooltip), EditorStyles.boldLabel);
                headerR.x += headerR.width;
            }
            for (int i = 0; i < length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int a = 0; a < arrays.Length; a++)
                {
                    if (arrays[a].type == 0) arrays[a].unityA[i] = EditorGUILayout.ObjectField(arrays[a].unityA[i], arrays[a].unityA.GetType().GetElementType(), true);
                    else if (arrays[a].type == 1 && arrays[a].enumType != null) arrays[a].intA[i] = Convert.ToInt32(EditorGUILayout.EnumPopup(arrays[a].GetEnumValue(i)));
                    else if (arrays[a].type == 1) arrays[a].intA[i] = EditorGUILayout.IntField(arrays[a].intA[i]);
                    else if (arrays[a].type == 2) arrays[a].floatA[i] = EditorGUILayout.FloatField(arrays[a].floatA[i]);
                    else if (arrays[a].type == 3) arrays[a].stringA[i] = EditorGUILayout.TextField(arrays[a].stringA[i]);
                    else EditorGUILayout.LabelField("Missing Type");
                }
                EditorGUILayout.EndHorizontal();
            }
            return arrays;
        }

        private void NormalGUI()
        {
            headerAct = EditorGUILayout.BeginFoldoutHeaderGroup(headerAct, "Actions", headerStyle);
            if (headerAct)
            {
                EditorGUI.indentLevel += 1;
                ArrayGUI(nameof(action.toggleObjects), "Toggle GameObjects");
                if(actionType == ActionType.Toggle) ArrayGUI(nameof(action.toggleObjectsInverted), "Toggle GameObjects Inverted");
                ArrayGUI(nameof(action.toggleColliders), "Toggle Colliders");
                ArrayGUI(nameof(action.togglePickups), "Toggle Pickups");
                action.teleportTarget = (Transform)EditorGUILayout.ObjectField(new GUIContent("Teleport to"), action.teleportTarget, typeof(Transform), true);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Udon Calls", EditorStyles.boldLabel);
                if (actionType == ActionType.Slider || actionType == ActionType.Toggle)
                {
                    ArrayData[] arrays = ArraysGUI(
                        new ArrayData("Udon Behaviour", "", action.udonBehaviours, typeof(GameObject)),
                        new ArrayData("Event Name", "Event to be called", action.udonEventNames),
                        new ArrayData("Value Name", "This variable will be set to the value of the ui element", action.udonValueNames));
                    action.udonBehaviours = (GameObject[])arrays[0].unityA;
                    action.udonEventNames = arrays[1].stringA;
                    action.udonValueNames = arrays[2].stringA;
                }
                else
                {
                    ArrayData[] arrays = ArraysGUI(
                        new ArrayData("Udon Behaviour", "", action.udonBehaviours, typeof(GameObject)),
                        new ArrayData("Event Name", "Event to be called", action.udonEventNames));
                    action.udonBehaviours = (GameObject[])arrays[0].unityA;
                    action.udonEventNames = arrays[1].stringA;
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Animators", EditorStyles.boldLabel);
                ArrayData[] arraysAnimator = ArraysGUI(
                    new ArrayData("Animator", "", action.animators, typeof(Animator)),
                    new ArrayData("Parameter Type", "", action.animatorParameterTypes, typeof(UnityEngine.AnimatorControllerParameterType)),
                    new ArrayData("Parameter Name", "Event to be called", action.animatorParameterNames));
                action.animators = (Animator[])arraysAnimator[0].unityA;
                action.animatorParameterTypes = arraysAnimator[1].intA;
                action.animatorParameterNames = arraysAnimator[2].stringA;
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            headerReq = EditorGUILayout.BeginFoldoutHeaderGroup(headerReq, "Requirements", headerStyle);
            if (headerReq)
            {
                EditorGUI.indentLevel += 1;
                ArrayGUI(nameof(action.hasToBeInsideAnyCollider), "Be Inside Collider", "Player has to be in one of these colliders for action to be executed.");
                EditorGUILayout.Space();
                action.hasToBeInstanceOwner = EditorGUILayout.Toggle(new GUIContent("Has to be Instance Owner"), action.hasToBeInstanceOwner);
                action.hasToBeOwner = EditorGUILayout.Toggle(new GUIContent("Has to be GameObject Owner"), action.hasToBeOwner);
                action.hasToBeMaster = EditorGUILayout.Toggle(new GUIContent("Has to be Master"), action.hasToBeMaster);
                if (action.hasToBeOwner) action.hasToBeOwnerGameobject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("GameObject to own"), action.hasToBeOwnerGameobject, typeof(GameObject), true);
                EditorGUILayout.Space();
                ArrayGUI(nameof(action.autherizedPlayerDisplayNames), "Autherized Players", "Player display name has to match one of this list.");
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        bool headerMirror;
        private void MirrorMangerGUI()
        {
            headerMirror = EditorGUILayout.BeginFoldoutHeaderGroup(headerMirror, "Mirror Manager", headerStyle);
            if (headerMirror)
            {
                EditorGUI.indentLevel += 1;
                ArrayGUI(nameof(action.mirrors), "Mirrors", "Nees to be the actual mirror plane.");
                action.maximumOpenDistance = EditorGUILayout.FloatField(new GUIContent("Maximum Distance", "Maximum distance the player can stand from the mirror and open it."), action.maximumOpenDistance);
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

#endif
}
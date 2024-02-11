
using UdonSharp;
using UnityEngine;
using VRC.Udon;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

namespace Thry.General
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SimpleUIInput : UdonSharpBehaviour
    {
        public string GameObjectName;
        public string EventName;
        public string VariableName;

        [HideInInspector] public SimpleUIInput[] LinkedInputs;

        UnityEngine.UI.Toggle valueToggle;
        UnityEngine.UI.Slider valueSlider;
        Animator optionalAnimator;

        private bool _isUdonTargetAvailable = false;
        private bool _isBoolTargetAvailable = false;
        private bool _isFloatTargetAvailable = false;
        private bool _isEventTargetAvailable = false;
        private UdonBehaviour _udonBehaviour;
        private void Start()
        {
            valueToggle = gameObject.GetComponent<UnityEngine.UI.Toggle>();
            valueSlider = gameObject.GetComponent<UnityEngine.UI.Slider>();
            optionalAnimator = gameObject.GetComponent<Animator>();

            if(!string.IsNullOrWhiteSpace(GameObjectName))
            {
                GameObject go = GameObject.Find(GameObjectName);
                if (go == null)
                {
                    Debug.Log("[Thry][SingletonEventCall] GameObject not found: " + GameObjectName);
                    return;
                }
                _udonBehaviour = go.GetComponent<UdonBehaviour>();
                if (_udonBehaviour == null)
                {
                    Debug.Log("[Thry][SingletonEventCall] UdonBehaviour not found: " + GameObjectName);
                    return;
                }
                _isUdonTargetAvailable = _udonBehaviour != null;
                _isBoolTargetAvailable = _isUdonTargetAvailable && valueToggle && !string.IsNullOrEmpty(VariableName);
                _isFloatTargetAvailable = _isUdonTargetAvailable && valueSlider && !string.IsNullOrEmpty(VariableName);
                _isEventTargetAvailable = _isUdonTargetAvailable && !string.IsNullOrEmpty(EventName);
            }

            // Is in Start. Could be in OnEnable, but not needed. Should never desync
            if(_isBoolTargetAvailable)
                SetBool((bool)_udonBehaviour.GetProgramVariable(VariableName));
            if(_isFloatTargetAvailable)
                SetFloat((float)_udonBehaviour.GetProgramVariable(VariableName));
        }

        void OnEnable()
        {
            if (optionalAnimator)
            {
                optionalAnimator.SetBool("isOn", valueToggle.isOn);
            }
        }

        private void SyncBool()
        {
            foreach (SimpleUIInput input in LinkedInputs)
            {
                input.SetBool(valueToggle.isOn);
            }
        }

        public void SetBool(bool value)
        {
            if(_isBoolTargetAvailable)
            {
                valueToggle.SetIsOnWithoutNotify(value);
                UpdateAnimator();
            }
        }

        private void SyncFloat()
        {
            foreach (SimpleUIInput input in LinkedInputs)
            {
                input.SetFloat(valueSlider.value);
            }
        }

        public void SetFloat(float value)
        {
            if(_isFloatTargetAvailable)
            {
                valueSlider.SetValueWithoutNotify(value);
                UpdateAnimator();
            }
        }

        public void Execute()
        {
            UpdateAnimator();

            if (!_isUdonTargetAvailable) return;

            if (_isFloatTargetAvailable)
            {
                _udonBehaviour.SetProgramVariable(VariableName, valueSlider.value);
                SyncFloat();
            }
            if (_isBoolTargetAvailable)
            {
                _udonBehaviour.SetProgramVariable(VariableName, valueToggle.isOn);
                SyncBool();
            }
            if(_isEventTargetAvailable)
                _udonBehaviour.SendCustomEvent(EventName);
        }

        private void UpdateAnimator()
        {
            if (optionalAnimator)
            {
                optionalAnimator.SetBool("isOn", valueToggle.isOn);
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
        foreach (SimpleUIInput script in scipts)
        {
            script.LinkedInputs = scipts.Where(s => s != script && s.GameObjectName == script.GameObjectName && s.VariableName == script.VariableName).ToArray();
        }
    }


#endif
    }
}
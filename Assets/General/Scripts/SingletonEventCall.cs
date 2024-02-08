
using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace Thry.General
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SingletonEventCall : UdonSharpBehaviour
    {
        public string GameObjectName;
        public string EventName;

        [Space(20)]
        public UnityEngine.UI.Toggle ValueToggle;
        public UnityEngine.UI.Slider ValueSlider;
        public string VariableName;

        private bool _isAvailable = false;
        private UdonBehaviour _udonBehaviour;
        private void Start()
        {
            if (string.IsNullOrWhiteSpace(GameObjectName) || string.IsNullOrWhiteSpace(EventName))
            {
                Debug.LogWarning("[Thry][SingletonEventCall] GameObjectName or EventName is empty");
                return;
            }
            GameObject go = GameObject.Find(GameObjectName);
            if (go == null)
            {
                Debug.LogWarning("[Thry][SingletonEventCall] GameObject not found: " + GameObjectName);
                return;
            }
            _udonBehaviour = go.GetComponent<UdonBehaviour>();
            if (_udonBehaviour == null)
            {
                Debug.LogWarning("[Thry][SingletonEventCall] UdonBehaviour not found: " + GameObjectName);
                return;
            }
            _isAvailable = true;
        }

        public void Execute()
        {
            if (!_isAvailable) return;
            if (ValueSlider)
                _udonBehaviour.SetProgramVariable(VariableName, ValueSlider.value);
            if (ValueToggle)
                _udonBehaviour.SetProgramVariable(VariableName, ValueToggle.isOn);
            _udonBehaviour.SendCustomEvent(EventName);
        }
    }
}
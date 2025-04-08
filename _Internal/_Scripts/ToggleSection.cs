using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon;

namespace Thry.Udon.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ToggleSection : UdonSharpBehaviour
    {
        [SerializeField] private Text _headerText;
        [SerializeField] private Text _buttonText;

        [SerializeField] private GameObject _togglePrefab;
        [SerializeField] private Text _togglePrefabText;
        
        private Toggle[] _toggles = new Toggle[0];
        private Object[][] _targets = new Object[0][];
        private bool[] _lastValues = new bool[0];

        public ToggleSection SetupNewSection(string sectionName)
        {
            GameObject newSection = Instantiate(this.gameObject, transform.parent);
            ToggleSection toggleSection = newSection.GetComponent<ToggleSection>();
            newSection.name = sectionName;
            toggleSection._headerText.text = sectionName;
            toggleSection._buttonText.text = sectionName;
            toggleSection._togglePrefab.SetActive(false);
            newSection.gameObject.SetActive(true);
            return toggleSection;
        }

        public void AddToggle(string name, bool defaultValue, Object[] targets)
        {
            GameObject toggle = Instantiate(_togglePrefab, _togglePrefab.transform.parent);
            toggle.name = name;
            toggle.GetComponentInChildren<Text>(true).text = name;
            toggle.gameObject.SetActive(true);
            Toggle toggleComponent = toggle.GetComponent<Toggle>();
            toggleComponent.isOn = defaultValue;
            Collections.Add(ref _toggles, toggleComponent);
            Collections.Add(ref _lastValues, defaultValue);
            Collections.Add(ref _targets, targets);
            OnToggleChanged(_toggles.Length - 1);
        }

        public void OnToggleChanged()
        {
            for (int i = 0; i < _toggles.Length; i++)
            {
                if(_toggles[i].isOn == _lastValues[i]) 
                    continue;
                OnToggleChanged(i);
                _lastValues[i] = _toggles[i].isOn;
            }
        }

        private void OnToggleChanged(int i)
        {
            bool isOn = _toggles[i].isOn;
            foreach (Object target in _targets[i])
            {
                System.Type targetType = target.GetType();
                if(targetType == typeof(GameObject))
                    ((GameObject)target).SetActive(isOn);
                else if(targetType == typeof(UdonSharpBehaviour))
                    ((UdonSharpBehaviour)target).enabled = isOn;
                else if(targetType == typeof(UdonBehaviour))
                    ((UdonBehaviour)target).enabled = isOn;
                else if(targetType == typeof(Collider))
                    ((Collider)target).enabled = isOn;
                else if(targetType == typeof(Renderer))
                    ((Renderer)target).enabled = isOn;
                else if(targetType == typeof(Canvas))
                    ((Canvas)target).enabled = isOn;
                else if(targetType == typeof(CanvasGroup))
                    ((CanvasGroup)target).interactable = isOn;
                else if(targetType == typeof(Graphic))
                    ((Graphic)target).raycastTarget = isOn;
                else
                    Debug.LogWarning($"[Thry] ToggleSection: Unknown target type {target.GetType()}");
            }
        }
    }
}
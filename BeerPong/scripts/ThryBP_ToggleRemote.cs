
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.BeerPong {
    public class ThryBP_ToggleRemote : UdonSharpBehaviour
    {
        private UnityEngine.UI.Toggle _ui;

        public ThryBP_Toggle _main;

        private void Start()
        {
            _ui = GetComponent<UnityEngine.UI.Toggle>();
            _ui.isOn = _main.value;
            _main.RegisterRemote(this);
        }

        public void Sync()
        {
            _ui.isOn = _main.value;
        }

        public override void Interact()
        {
            _main.SetValue(!_main.value);
        }

        public void OnValueChanged()
        {
            _main.SetValue(_ui.isOn);
        }
    }
}
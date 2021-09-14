
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.BeerPong {
    public class ThryBP_Toggle : UdonSharpBehaviour
    {
        private UnityEngine.UI.Toggle _ui;

        public ThryBP_Main _main;
        public int type;

        public Animator animator;

        [UdonSynced]
        public bool value;

        private void Start()
        {
            _ui = GetComponent<UnityEngine.UI.Toggle>();
            if(_ui != null)
            {
                _ui.isOn = value;
            }
            if (animator != null)
            {
                animator.SetBool("on", value);
            }
        }

        //=======Remotes Code========
        private GameObject[] _remotes = new GameObject[0];
        public void RegisterRemote(ThryBP_ToggleRemote remote)
        {
            GameObject[] newArray = new GameObject[_remotes.Length + 1];
            for (int i = 0; i < _remotes.Length; i++)
            {
                newArray[i] = _remotes[i];
            }
            newArray[_remotes.Length] = remote.gameObject;
            _remotes = newArray;
        }

        public override void OnDeserialization()
        {
            for (int i = 0; i < _remotes.Length; i++) ((UdonBehaviour)_remotes[i].GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Sync");
            if (_ui != null)
            {
                _ui.isOn = value;
            }
            if(_main != null)
            {
                if (type == 0)
                {
                    _main.ChangeTeams();
                }else if(type == 1)
                {
                    _main.ChangeBarrier();
                }
            }
            if(animator != null)
            {
                animator.SetBool("on", value);
            }
        }

        public override void Interact()
        {
            SetValue(!value);
        }

        public void OnValueChanged()
        {
            SetValue(_ui.isOn);
        }

        public void SetValue(bool b)
        {
            if (Networking.IsOwner(gameObject) == false) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            value = b;
            OnDeserialization();
            RequestSerialization();
        }
    }
}
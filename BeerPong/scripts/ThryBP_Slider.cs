
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Thry.BeerPong
{
    public class ThryBP_Slider : UdonSharpBehaviour
    {
        public ThryBP_CupsSpawn _cups;
        public ThryBP_Main _bpmain;

        Slider _slider;

        [UdonSynced]
        public int value;
        int _localValue;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            value = (int)_slider.value;
            _localValue = value;
        }

        public void OnValueChanged()
        {
            if((int)_slider.value != value) SetValue((int)_slider.value);
        }

        public void SetValue(int i)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            value = i;
            RequestSerialization();
            OnDeserialization();
            if (_cups != null) _cups.ResetGlassesIfNoneHit();
            if (_bpmain != null) _bpmain.ChangeAmountOfPlayers();
        }

        //=======Remotes Code========
        private GameObject[] _remotes = new GameObject[0];
        public void RegisterRemote(ThryBP_SliderRemote remote)
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
            if (_localValue == value) return;
            for (int i = 0; i < _remotes.Length; i++) ((UdonBehaviour)_remotes[i].GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Sync");
            _slider.value = value;
            if (_cups != null) _cups.ResetGlassesIfNoneHit();
            if (_bpmain != null) _bpmain.ChangeAmountOfPlayers();
            _localValue = value;
        }

    }
}
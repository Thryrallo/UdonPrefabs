
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Thry.BeerPong
{
    public class ThryBP_OptionsSelector : UdonSharpBehaviour
    {
        public string[] options;
        public int startOption;

        public ThryBP_CupsSpawn _cups;
        public ThryBP_Main _main;

        [Header("References")]
        public Text selectedText;

        [UdonSynced]
        [HideInInspector]
        public int selectedIndex;
        int localSelectedIndex;

        void Start()
        {
            selectedText.text = options[startOption];
        }

        //=======Remotes Code========
        private GameObject[] _remotes = new GameObject[0];
        public void RegisterRemote(ThryBP_OptionsSelectorRemote remote)
        {
            GameObject[] newArray = new GameObject[_remotes.Length + 1];
            for(int i = 0; i < _remotes.Length; i++)
            {
                newArray[i] = _remotes[i];
            }
            newArray[_remotes.Length] = remote.gameObject;
            _remotes = newArray;
        }

        public override void OnDeserialization()
        {
            if (localSelectedIndex == selectedIndex) return;
            for (int i = 0; i < _remotes.Length; i++) ((UdonBehaviour)_remotes[i].GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Sync");
            selectedText.text = options[selectedIndex];
            if (_cups != null) _cups.ResetGlassesIfNoneHit();
            if (_main != null) _main.ChangeGameMode();
            localSelectedIndex = selectedIndex;
        }

        public void Next()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            selectedIndex = (selectedIndex + 1) % options.Length;
            OnDeserialization();
            RequestSerialization();
        }

        public void Prev()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
            OnDeserialization();
            RequestSerialization();
        }
    }
}
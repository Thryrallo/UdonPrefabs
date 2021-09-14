
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Thry.BeerPong
{
    public class ThryBP_OptionsSelectorRemote : UdonSharpBehaviour
    {
        [Header("References")]
        public Text selectedText;
        public ThryBP_OptionsSelector _main;

        void Start()
        {
            selectedText.text = _main.options[_main.startOption];
            _main.RegisterRemote(this);
        }

        public void Sync()
        {
            selectedText.text = _main.options[_main.selectedIndex];
        }

        public void Next()
        {
            _main.Next();
        }

        public void Prev()
        {
            _main.Prev();
        }
    }
}
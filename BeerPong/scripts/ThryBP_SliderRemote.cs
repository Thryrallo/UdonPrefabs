
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Thry.BeerPong
{
    public class ThryBP_SliderRemote : UdonSharpBehaviour
    {
        Slider _slider;
        public ThryBP_Slider _main;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.value = _main.value;
            _main.RegisterRemote(this);
        }

        public void Sync()
        {
            _slider.value = _main.value;
        }

        public void OnValueChanged()
        {
            _main.SetValue((int)_slider.value);
        }
    }
}
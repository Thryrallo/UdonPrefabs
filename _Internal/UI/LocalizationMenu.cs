
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.UI
{
    public class LocalizationMenu : SimpleUIToggleGroup
    {
        public Image ActiveIcon;
        public Toggle OpeningToggle;

        protected override void UpdateOptionals(bool wasUserInput, bool valueChanged)
        {
            //Debug.Log($"Update Optionals {wasUserInput} {valueChanged} {UIValue} {OpeningToggle.isOn}");
            base.UpdateOptionals(wasUserInput, valueChanged);
            if(valueChanged && ActiveIcon)
                ActiveIcon.sprite = Toggles[(int)UIValue].GetComponent<Image>().sprite;
            if (wasUserInput && OpeningToggle)
                OpeningToggle.isOn = false;
        }
    }
}
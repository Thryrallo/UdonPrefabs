using UdonSharp;
using UnityEngine;

namespace Thry.Udon.AvatarTheme
{
    public abstract class AvatarThemeColorListener : UdonSharpBehaviour
    {
        [SerializeField, HideInInspector] protected AvatarThemeColor _avatarThemeColor;
        protected virtual void Start()
        {
            if(_avatarThemeColor)
                _avatarThemeColor.RegisterListener(this);
        }

        public abstract void OnColorChange(Color[] oldColors, Color[] newColors);
    }
}
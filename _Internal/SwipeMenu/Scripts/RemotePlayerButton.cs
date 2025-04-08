
using Thry.Udon.AvatarTheme;
using Thry.Udon.SwipeMenu;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Thry.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RemotePlayerButton : SimpleUIButton
    {
        [Space(20)]
        public Playerlist PlayerListManager;
        public int PlayerId;
        public UnityEngine.UI.Text PlayerNameText;

        [SerializeField, HideInInspector] private AvatarThemeColor _themeColor;

        public void OnAfterEnable()
        {
            if(_themeColor)
            {
                if( _themeColor.GetColor(VRCPlayerApi.GetPlayerById(PlayerId), out Color theme))
                {
                    ColorBlock colorBlock = GetComponent<Button>().colors;
                    colorBlock.normalColor = theme;
                    colorBlock.highlightedColor = theme;
                    colorBlock.pressedColor = theme;
                    colorBlock.selectedColor = theme;
                    colorBlock.disabledColor = theme;
                    GetComponent<Button>().colors = colorBlock;
                }
            }
        }

        public override void Execute()
        {   
            PlayClick();
            PlayerListManager.OpenRemotePlayerDetails(PlayerId);
        }
    }
}
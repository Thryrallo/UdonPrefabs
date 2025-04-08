
using BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp;
using Thry.Udon.AvatarTheme;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class HomePageManager : AvatarThemeColorListener
    {
        
        [SerializeField] Camera _localPlayerCamera;

        [SerializeField] UnityEngine.UI.Text _playerHeaderText;
        [SerializeField] UnityEngine.UI.Text _playerDetailsText;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColor1;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColor2;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColor3;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColor4;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColor5;

        [SerializeField] UnityEngine.UI.Text _worldPlayerCountText;
        [SerializeField] UnityEngine.UI.Text _worldUptimeText;
        [SerializeField] UnityEngine.UI.Text _worldInstanceMasterText;

        private Color[] _themes = new Color[5];
        private string _colorName = "unknown";

        [UdonSynced] private long _worldLoadTimeInUnixSeconds = 0;

        protected override void Start()
        {
            base.Start();
            if(Networking.IsMaster && _worldLoadTimeInUnixSeconds == 0)
            {
                _worldLoadTimeInUnixSeconds = System.DateTimeOffset.Now.ToUnixTimeSeconds();
                RequestSerialization();
            }
        }

        public override void OnColorChange(Color[] oldColors, Color[] newColors)
        {
            _themes = newColors;
            _colorName = _avatarThemeColor.GetColorName();
            if(_playerDetailsText.gameObject.activeInHierarchy)
                UpdatePlayerDetails();
        }

        public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float prevEyeHeightAsMeters)
        {
            if(player.isLocal && _playerDetailsText.gameObject.activeInHierarchy)
                UpdatePlayerDetails();
        }

        public override void OnMasterTransferred(VRCPlayerApi newMaster)
        {
            UpdateWorldDetails();
        }

        public void RequestUpdate()
        {
            UpdatePlayerDetails();
            UpdateWorldDetails();
            SendCustomEventDelayedSeconds(nameof(UpdateWorldDetails), 10f);
        }

        private void UpdatePlayerDetails()
        {
            int id = Networking.LocalPlayer.playerId;
            float height = Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();
            string hardware = GetDeviceName();

            _playerHeaderText.text = Networking.LocalPlayer.displayName;
            _playerDetailsColor1.color = _themes[0];
            _playerDetailsColor2.color = _themes[1];
            _playerDetailsColor3.color = _themes[2];
            _playerDetailsColor4.color = _themes[3];
            _playerDetailsColor5.color = _themes[4];
            _playerDetailsText.text = $"@{id}\n{height:0.00}m\n{_colorName}\n{hardware}";

            API.Avatar.TakeHeadshotPicture(Networking.LocalPlayer, _localPlayerCamera);
        }

        public void UpdateWorldDetails()
        {
            int uptime = (int)(System.DateTimeOffset.Now.ToUnixTimeSeconds() - _worldLoadTimeInUnixSeconds);
            int uptimeHours = (int)uptime / 3600;
            int uptimeMinutes = (int)(uptime % 3600) / 60;

            _worldPlayerCountText.text = $"{VRCPlayerApi.GetPlayerCount()}";
            _worldUptimeText.text = (uptimeHours > 0 ? $"{uptimeHours}h\n" : "") + $"{uptimeMinutes}min";
            _worldInstanceMasterText.text = Networking.Master.displayName;

            if(_worldPlayerCountText.gameObject.activeInHierarchy)
                SendCustomEventDelayedSeconds(nameof(UpdateWorldDetails), 10f); // update every 10 seconds while active
        }

        private string GetDeviceName()
        {
            //        Keyboard = 0,
            //Mouse = 1,
            //Controller = 2,
            //Gaze = 3,
            //Vive = 5,
            //Oculus = 6,
            //Index = 10,
            //HPMotionController = 11,
            //Osc = 12,
            //QuestHands = 13,
            //Generic = 14,
            //Touch = 15,
            //OpenXRGeneric = 16,
            //Count = 18
            VRCInputMethod method = InputManager.GetLastUsedInputMethod();
            switch(method)
            {
                case VRCInputMethod.Keyboard:
                case VRCInputMethod.Mouse: return "Keyboard & Mouse";
                case VRCInputMethod.Controller: return "Controller";
                case VRCInputMethod.Gaze: return "Gaze";
                case VRCInputMethod.Vive: return "Vive";
                case VRCInputMethod.Oculus: return "Oculus";
                case VRCInputMethod.ViveXr: return "Vive XR";
                case VRCInputMethod.Index: return "Valve Index";
                case VRCInputMethod.HPMotionController: return "HP Motion Controller";
                case VRCInputMethod.Osc: return "Osc";
                case VRCInputMethod.QuestHands: return "Meta Handtracking";
                case VRCInputMethod.Generic: return "Generic";
                case VRCInputMethod.Touch: return "Touchscreen";
                case VRCInputMethod.OpenXRGeneric: return "OpenXR Generic";
                case VRCInputMethod.Pico: return "Pico";
                case VRCInputMethod.SteamVR2: return "SteamVR";
                default: return $"Unknown ({(int)method})";
            }
        }
    }
}
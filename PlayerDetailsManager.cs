
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;
using System;
using Thry.General;
using UdonSharp;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using VRC.SDK3.Rendering;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using System.ComponentModel.Design;

namespace Thry.SAO
{
    public class PlayerDetailsManager : UdonSharpBehaviour
    {
        public UnityEngine.UI.Text PlayerDetailsText;
        public UnityEngine.UI.Image PlayerDetailsColor1;
        public UnityEngine.UI.Image PlayerDetailsColor2;
        public UnityEngine.UI.Image PlayerDetailsColor3;
        public UnityEngine.UI.Image PlayerDetailsColor4;
        public UnityEngine.UI.Image PlayerDetailsColor5;
        
        AvatarThemeColor _themeColor;

        private void Start()
        {
            _themeColor = AvatarThemeColor.Get();
        }

        public void RequestUpdate()
        {
            if (_themeColor != null)
                _themeColor.RequestUpdate(this, nameof(UpdatePlayerDetails));
            else
                UpdatePlayerDetails();
        }

        public void UpdatePlayerDetails()
        {
            int id = Networking.LocalPlayer.playerId;
            float height = Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();
            string hardware = GetDeviceName();
            string colorName = "unknown";
            string theme = "unknown";

            Color transprent = new Color(1, 1, 1, 0);
            PlayerDetailsColor1.color = transprent;
            PlayerDetailsColor2.color = transprent;
            PlayerDetailsColor3.color = transprent;
            PlayerDetailsColor4.color = transprent;
            PlayerDetailsColor5.color = transprent;
            if (_themeColor != null)
            {
                colorName = _themeColor.GetColorName();

                Color[] themes = _themeColor.GetColors();
                PlayerDetailsColor1.color = themes[0];
                PlayerDetailsColor2.color = themes[1];
                PlayerDetailsColor3.color = themes[2];
                PlayerDetailsColor4.color = themes[3];
                PlayerDetailsColor5.color = themes[4];
            }
            PlayerDetailsText.text = $"@{id}\n{height:0.00}m\n{colorName}\n{hardware}";
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
            int method = (int)InputManager.GetLastUsedInputMethod();
            if (method < 5) return "Desktop";
            if (method < 6) return "VR (Vive)";
            if (method < 10) return "VR (Meta)";
            if (method < 11) return "VR (Index)";
            if (method == 15) return "Mobile";
            return "Unknown";
        }
    }
}
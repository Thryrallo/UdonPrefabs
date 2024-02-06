
using UdonSharp;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using VRC.SDK3.Rendering;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Thry
{
    public class AvatarThemeColor : UdonSharpBehaviour
    {
        public Camera LocalPlayerBodyCamera;
        public float LocalPlayerCameraSize = 0.25f;
        public Light DirectionLight;

        const int BINS_PER_CHANNEL = 4;
        Color[] BIN_COLORS;
        float[] BIN_WEIGHTS;
        string[] BIN_NAMES;

        bool _isDirty = true;
        int _playerIndex;
        int[] _playerIndicies;

        Color[] _playerColors;
        string[] _playerNames;

        UdonSharpBehaviour _callbackBehaviour;
        string _callbackEvent;

        private void Start()
        {
            BIN_COLORS = new Color[BINS_PER_CHANNEL * BINS_PER_CHANNEL * BINS_PER_CHANNEL];
            BIN_WEIGHTS = new float[BINS_PER_CHANNEL * BINS_PER_CHANNEL * BINS_PER_CHANNEL];
            BIN_NAMES = new string[BINS_PER_CHANNEL * BINS_PER_CHANNEL * BINS_PER_CHANNEL];
            for (int r = 0; r < BINS_PER_CHANNEL; r++)
                for (int g = 0; g < BINS_PER_CHANNEL; g++)
                    for (int b = 0; b < BINS_PER_CHANNEL; b++)
                    {
                        int index = r * BINS_PER_CHANNEL * BINS_PER_CHANNEL + g * BINS_PER_CHANNEL + b;
                        BIN_COLORS[index] = new Color(
                            (r + 1f) / BINS_PER_CHANNEL,
                            (g + 1f) / BINS_PER_CHANNEL,
                            (b + 1f) / BINS_PER_CHANNEL
                        );
                        float h, s, v;
                        Color.RGBToHSV(BIN_COLORS[index], out h, out s, out v);
                        BIN_WEIGHTS[index] = (v * s) * 0.75f + 0.25f;
                        BIN_NAMES[index] = ColorToName(BIN_COLORS[index]);
                    }
        }

        public static AvatarThemeColor Get()
        {
            GameObject go = GameObject.Find("[Thry]AvatarThemeColor");
            if (go != null)
                return go.GetComponent<AvatarThemeColor>();
            return null;
        }

        public void RequestUpdate(UdonSharpBehaviour udonBehaviour, string eventName)
        {
            _callbackBehaviour = udonBehaviour;
            _callbackEvent = eventName;

            if (_isDirty)
            {
                _isDirty = false;
                
                Vector3 bodyPos = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Chest);
                Vector3 cPos = bodyPos + Networking.LocalPlayer.GetRotation() * Vector3.forward * 2;
                LocalPlayerBodyCamera.transform.SetPositionAndRotation(cPos, Quaternion.LookRotation(bodyPos - cPos));
                LocalPlayerBodyCamera.orthographicSize = LocalPlayerCameraSize * Networking.LocalPlayer.GetAvatarEyeHeightAsMeters() * 2;
                DirectionLight.enabled = true;
                LocalPlayerBodyCamera.Render();
                DirectionLight.enabled = false;

                VRCAsyncGPUReadback.Request(LocalPlayerBodyCamera.targetTexture, 0, (IUdonEventReceiver)this);
            }else
            {
                ExecuteCallback();
            }
        }

        public override void OnAvatarChanged(VRCPlayerApi player)
        {
            _isDirty = true;
        }

        public Color GetColor()
        {
            return BIN_COLORS[_playerIndex];
        }

        public Color[] GetColors()
        {
            Color[] colors = new Color[_playerIndicies.Length];
            for (int i = 0; i < _playerIndicies.Length; i++)
            {
                colors[i] = BIN_COLORS[_playerIndicies[i]];
            }
            return colors;
        }

        public string GetColorName()
        {
            return BIN_NAMES[_playerIndex];
        }

        private void ExecuteCallback()
        {
            _callbackBehaviour.SendCustomEvent(_callbackEvent);
        }

        private string ColorToName(Color c)
        {
            float hue, sat, val;
            Color.RGBToHSV(c, out hue, out sat, out val);
            string prefix = "";
            if (val < 0.5) prefix = "Dark ";

            if (sat < 0.3)
            {
                if (val < 0.15) return "Black";
                if (val < 0.3) return "Dark Gray";
                if (val < 0.6) return "Gray";
                if (val < 0.8) return "Light Gray";
                return "White";
            }
            if (hue < 0.05)
                return prefix + "Red";
            if (hue < 0.13)
                return prefix + "Orange";
            if (hue < 0.2)
            {
                if(val < 0.5)
                    return "Brown";
                return "Yellow";
            }
            if (hue < 0.44)
                return prefix + "Green";
            if (hue < 0.54)
                return prefix + "Cyan";
            if (hue < 0.75)
                return prefix + "Blue";
            if (hue < 0.83)
            {
                if (sat < 0.5)
                    return "Light Pink";
                if (sat < 0.8)
                    return prefix + "Pink";
                return prefix + "Purple";
            }
            if (hue < 0.92)
            {
                if (sat < 0.5)
                    return "Light Pink";
                if (sat < 0.8)
                    return prefix + "Pink";
                return prefix + "Purple";
            }
            return prefix + "Red";
        }

        public void OnAsyncGpuReadbackComplete(VRCAsyncGPUReadbackRequest request)
        {
            var px = new Color32[LocalPlayerBodyCamera.targetTexture.width * LocalPlayerBodyCamera.targetTexture.height];
            if (request.hasError || !request.TryGetData(px))
            {
                _playerIndex = 0;
            }
            else
            {
                _playerIndex = EvaluatePlayerColor(px);
            }

            ExecuteCallback();
        }

        private int EvaluatePlayerColor(Color32[] pixels)
        {
            int binSize = 256 / BINS_PER_CHANNEL; // Calculate the size of each bin
            int[] bins = new int[BINS_PER_CHANNEL * BINS_PER_CHANNEL * BINS_PER_CHANNEL];
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a < 128) continue;
                byte r = (byte)(pixels[i].r / binSize);
                byte g = (byte)(pixels[i].g / binSize);
                byte b = (byte)(pixels[i].b / binSize);

                int index = r * BINS_PER_CHANNEL * BINS_PER_CHANNEL + g * BINS_PER_CHANNEL + b;
                bins[index]++;
            }

            // find bin color with most bin weight
            float maxWheight = -1;
            int maxIndex = -1;
            _playerIndicies = new int[5]; // 5 colors with most bins
            for (int i = 0; i < bins.Length; i++)
            {
                if (bins[i] == 0)
                    continue;
                
                Debug.Log($"Bin {BIN_COLORS[i]} has {bins[i]} pixels with wheight {BIN_WEIGHTS[i]}");
                float wheight = BIN_WEIGHTS[i] * bins[i];
                if (wheight > maxWheight)
                {
                    maxWheight = wheight;
                    maxIndex = i;
                    Debug.Log($"New max bin: {i} with weight {maxWheight}");
                }
                
                for(int j=0; j < _playerIndicies.Length; j++)
                {
                    if (bins[i] > bins[_playerIndicies[j]])
                    {
                        for(int jj = _playerIndicies.Length - 1; jj > j; jj--)
                        {
                            _playerIndicies[jj] = _playerIndicies[jj - 1];
                        }
                        _playerIndicies[j] = i;
                        break;
                    }
                }
            }
            if (maxIndex == -1) return 0;
            return maxIndex;
        }
    }
}
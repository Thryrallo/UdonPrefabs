
using System.Globalization;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Rendering;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;




#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
#endif

namespace Thry
{
    public class AvatarThemeColor : UdonSharpBehaviour
    {
        public Camera LocalPlayerBodyCamera;
        public float LocalPlayerCameraSize = 0.25f;
        public Light[] Lights;
        public TextAsset ColorList;
        
        public Light[] OtherLights;
        public ReflectionProbe[] ReflectionProbes;

        const int BINS_H = 10;
        const int BINS_S = 5;
        const int BINS_V = 4;

        const int C_COLORS = 5;
        
        bool _isSyncing = false;
        bool _isLoaded = false;

        private string[] _colorSearchNames;
        private Vector3[] _colorSearchColors;

        Color[] _playerColors = new Color[C_COLORS];
        string[] _playerNames = new string[C_COLORS];

        [UdonSynced]
        Color[] _syncedColors = new Color[0];

        void Start()
        {
            _colorSearchNames = ColorList.text.Split('\n');
            _colorSearchColors = new Vector3[_colorSearchNames.Length];
            for (int i = 0; i < _colorSearchNames.Length; i++)
            {
                string[] parts = _colorSearchNames[i].Split(';');
                _colorSearchNames[i] = parts[0];
                _colorSearchColors[i] = new Vector3(
                    float.Parse(parts[1], CultureInfo.InvariantCulture), 
                    float.Parse(parts[2], CultureInfo.InvariantCulture), 
                    float.Parse(parts[3], CultureInfo.InvariantCulture));
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
            // Move Camera into position
            Vector3 bodyPos = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Chest);
            Vector3 headPos = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head);
            Vector3 lookAtPos = (bodyPos + headPos) / 2;
            if (lookAtPos == Vector3.zero)
                lookAtPos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            Vector3 cPos = lookAtPos + Networking.LocalPlayer.GetRotation() * Vector3.forward * 2;
            LocalPlayerBodyCamera.transform.SetPositionAndRotation(cPos, Quaternion.LookRotation(lookAtPos - cPos));
            LocalPlayerBodyCamera.orthographicSize = LocalPlayerCameraSize * Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();

            // clear as much lighting as possible
            UnityEngine.Rendering.AmbientMode amb_mode = UnityEngine.RenderSettings.ambientMode;
            Color amb_color = UnityEngine.RenderSettings.ambientLight;
            UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            UnityEngine.RenderSettings.ambientLight = Color.white;

            foreach (Light l in OtherLights)
                l.enabled = false;
            DisableRelfectionProbes(lookAtPos);

            // render
            foreach (Light l in Lights)
                l.enabled = true;
            LocalPlayerBodyCamera.Render();
            foreach (Light l in Lights)
                l.enabled = false;

            // restore lighting
            EnableReflectionProbes();
            foreach (Light l in OtherLights)
                l.enabled = true;

            UnityEngine.RenderSettings.ambientMode = amb_mode;
            UnityEngine.RenderSettings.ambientLight = amb_color;

            VRCAsyncGPUReadback.Request(LocalPlayerBodyCamera.targetTexture, 0, (IUdonEventReceiver)this);
        }

        bool[] _reflectionProbeNeedsEnabling;
        void DisableRelfectionProbes(Vector3 pos)
        {
            _reflectionProbeNeedsEnabling = new bool[ReflectionProbes.Length];
            for (int i = 0; i < ReflectionProbes.Length; i++)
            {
                // check if near
                Bounds bounds = ReflectionProbes[i].bounds;
                bounds.Expand(Networking.LocalPlayer.GetAvatarEyeHeightAsMeters() * 2);
                _reflectionProbeNeedsEnabling[i] = ReflectionProbes[i].enabled && bounds.Contains(pos);
                if (_reflectionProbeNeedsEnabling[i])
                    ReflectionProbes[i].enabled = false;
            }
        }

        void EnableReflectionProbes()
        {
            for (int i = 0; i < ReflectionProbes.Length; i++)
            {
                if (_reflectionProbeNeedsEnabling[i])
                {
                    ReflectionProbes[i].enabled = true;
                    if(ReflectionProbes[i].refreshMode == UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake)
                        ReflectionProbes[i].RenderProbe();
                }
            }
        }

        public override void OnAvatarChanged(VRCPlayerApi player)
        {
            // Delay by 1 second because of spawn in animation stuff
            SendCustomEventDelayedSeconds(nameof(FirstUpdate), 1);
        }
        public void FirstUpdate()
        {
            RequestUpdate(null, null);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if(!_isLoaded)
                RequestUpdate(null, null);
            _isLoaded = true;
#endif
            if (Time.frameCount % 100 != 0 || _isSyncing || !_isLoaded) return;
            CheckSynced();
        }

        public void CheckSynced()
        {
            int myId = Networking.LocalPlayer.playerId;
            bool isValid = myId * C_COLORS < _syncedColors.Length;
            if (isValid)
            {
                for (int i = 0; i < _playerColors.Length; i++)
                {
                    if (_syncedColors[myId * C_COLORS + i] != _playerColors[i])
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            if (isValid)
            {
                _isSyncing = false;
            }
            else
            {
                _isSyncing = true;
                Sync();
            }
        }

        void Sync()
        {
            int myId = Networking.LocalPlayer.playerId;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            if (!(myId * C_COLORS < _syncedColors.Length))
            {
                Color[] prev = _syncedColors;
                _syncedColors = new Color[myId * C_COLORS + C_COLORS];
                System.Array.Copy(prev, _syncedColors, prev.Length);
            }
            for (int i = 0; i < C_COLORS; i++)
            {
                _syncedColors[myId * C_COLORS + i] = _playerColors[i];
            }
            RequestSerialization();

            this.SendCustomEventDelayedSeconds(nameof(CheckSynced), Random.value * 15 + 3);
        }

        public Color GetColor(VRCPlayerApi player, Color fallback)
        {
            int id = player.playerId;
            if (id * C_COLORS < _syncedColors.Length)
            {
                return _syncedColors[id * C_COLORS];
            }
            return fallback;
        }

        public Color GetColor()
        {
            return _playerColors[0];
        }

        public Color[] GetColors()
        {
            return _playerColors;
        }

        public string GetColorName()
        {
            return _playerNames[0];
        }

        private string ColorToName(Color c)
        {
            // iterate known colors, find index with smallest rgb distance
            float smallestDistance = 10;
            int smallestIndex = 0;
            Vector3 colorAsVec = new Vector3(c.r, c.g, c.b);
            for (int i = 0; i < _colorSearchColors.Length; i++)
            {
                float distance = Vector3.Distance(_colorSearchColors[i], colorAsVec);
                // Debug.Log($"[Thry][AviTheme] {i}: {distance} {colorAsVec}-{_colorSearchColors[i]} {_colorSearchNames[i]}");
                    
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestIndex = i;
                }
            }
            return _colorSearchNames[smallestIndex];

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
            if (hue < 0.028)
                return prefix + "Red";
            if (hue < 0.13)
                return prefix + "Orange";
            if (hue < 0.2)
            {
                if (val < 0.5)
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
            if (hue < 0.94)
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
                for (int i = 0; i < _playerColors.Length; i++)
                {
                    _playerColors[i] = Color.black;
                    _playerNames[i] = "unknown";
                }
            }
            else
            {
                EvaluatePlayerColor(px);
                for (int i = 0; i < 5; i++)
                    _playerNames[i] = ColorToName(_playerColors[i]);
            }
            _isLoaded = true;
        }

        private void EvaluatePlayerColor(Color32[] pixels)
        {
            Color[] allBins = new Color[BINS_H * BINS_S * BINS_V];
            int fullBins = 0;
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a < 128) continue;

                float h, s, v;
                Color.RGBToHSV(pixels[i], out h, out s, out v);

                h = (h + 0.5f / BINS_H) % 1; // shift by half bin for red wrap around
                byte hB = (byte)(h * (BINS_H - 0.001));
                byte sB = (byte)(s * (BINS_S - 0.001));
                byte vB = (byte)(v * (BINS_V - 0.001));

                int index = hB * BINS_S * BINS_V + sB * BINS_V + vB;

                if (allBins[index].a == 0)
                    fullBins++;

                pixels[i].a = 255;
                allBins[index] += (Color)pixels[i];
            }

            // remove empty bins
            Color[] bins = new Color[fullBins];
            int newBinIndex = 0;
            for (int i = 0; i < allBins.Length; i++)
            {
                if (allBins[i].a > 0)
                {
                    bins[newBinIndex] = allBins[i] / allBins[i].a;
                    bins[newBinIndex++].a = allBins[i].a;
                }
            }

            // combine bins with close rgb values ( < 0.05 )
            for (int i = 0; i < bins.Length; i++)
            {
                for (int j = 0; j < bins.Length; j++)
                {
                    if (i == j) continue;
                    Color diff = bins[i] - bins[j];
                    if (Mathf.Abs(diff.r) < 0.05 && Mathf.Abs(diff.g) < 0.05 && Mathf.Abs(diff.b) < 0.05)
                    {
                        float totalA = bins[i].a + bins[j].a;
                        bins[i] = (bins[i] * bins[i].a + bins[j] * bins[j].a) / totalA;
                        bins[i].a = totalA;
                        bins[j] = Color.clear;
                    }
                }
            }

            // find bin color with most bin weight
            for (int i = 0; i < _playerColors.Length; i++)
                _playerColors[i] = Color.clear;

            for (int i = 0; i < bins.Length; i++)
            {
                if (bins[i].a == 0)
                    continue;

                float h, s, v;
                Color.RGBToHSV(bins[i], out h, out s, out v);
                float wheight = bins[i].a * (s * 0.5f + 0.5f) * (v * 0.5f + 0.5f);
                //Debug.Log($"Bin {bins[i]} with wheight {wheight}");

                for (int j = 0; j < _playerColors.Length; j++)
                {
                    if (wheight > _playerColors[j].a)
                    {
                        for (int jj = _playerColors.Length - 1; jj > j; jj--)
                        {
                            _playerColors[jj] = _playerColors[jj - 1];
                        }
                        _playerColors[j] = bins[i];
                        _playerColors[j].a = wheight;
                        break;
                    }
                }
            }

            string debug = "[Thry][AviTheme] BinSort:\n";
            for (int i = 0; i < _playerColors.Length; i++)
            {
                float h, s, v;
                Color.RGBToHSV(_playerColors[i], out h, out s, out v);
                h = BeigeFilter(_playerColors[i]);
                debug += $"{i}: {_playerColors[i].ToString("F2")} ({s.ToString("F2")},{v.ToString("F2")},{h.ToString("F2")})\n";
                _playerColors[i].a = s * v * h;
            }
            Debug.Log(debug);

            // Sort _playerColors by saturation * value
            for (int i = 0; i < _playerColors.Length; i++)
            {
                for (int j = i + 1; j < _playerColors.Length; j++)
                {
                    if (_playerColors[j].a > _playerColors[i].a)
                    {
                        Color temp = _playerColors[i];
                        _playerColors[i] = _playerColors[j];
                        _playerColors[j] = temp;
                    }
                }
            }

            debug = "[Thry][AviTheme] VSB Sort:\n";
            for (int i = 0; i < _playerColors.Length; i++)
            {
                debug += $"{i}: {ColorInfo(_playerColors[i])}\n";
                _playerColors[i].a = 1;
            }
            Debug.Log(debug);
        }

        public float BeigeFilter(Color c)
        {
            float h, s, v;
            Color.RGBToHSV(c, out h, out s, out v);
            // if (h > 0.05 && h < 0.12 && s > 0.25 && s < 0.7 && v > 0.3 && v < 0.9)
            bool hue = h < 0.12 || h > 0.94;
            bool sat = s > 0.15 && s < 0.55;
            bool val = v > 0.1 && v <= 1;
            if (hue && sat && val)
                return 0.05f;
            return 10;
        }

        string ColorInfo(Color c)
        {
            float h, s, v;
            Color.RGBToHSV(c, out h, out s, out v);
            return $"{c.ToString("F2")} {ColorToHex(c)} ({h.ToString("F2")},{s.ToString("F2")},{v.ToString("F2")})";
        }

        string ColorToHex(Color c)
        {
            return "#" + ((int)(c.r * 255)).ToString("X2") + ((int)(c.g * 255)).ToString("X2") + ((int)(c.b * 255)).ToString("X2");
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    // register scene saving callback on compile
        [InitializeOnLoadMethod]
        private static void RegisterSceneSavingCallback()
        {
            EditorSceneManager.sceneSaving += OnBeforeSave;
        }

        static void OnBeforeSave(Scene scene, string path)
        {
            // Find all objects that can affect the lighting of the avatar
            Light[] lights = GameObject.FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            ReflectionProbe[] reflectionProbes = GameObject.FindObjectsByType<ReflectionProbe>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            LightProbeGroup[] lightProbeGroups = GameObject.FindObjectsByType<LightProbeGroup>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            AvatarThemeColor[] scripts = GameObject.FindObjectsByType<AvatarThemeColor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (AvatarThemeColor s in scripts)
            {
                s.OtherLights = lights.Where(l => s.Lights.Contains(l) == false).ToArray();
                s.ReflectionProbes = reflectionProbes;
            }
        }
#endif
    }
}
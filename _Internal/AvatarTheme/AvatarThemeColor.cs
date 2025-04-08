
using System.Globalization;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Rendering;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using JetBrains.Annotations;
using VRC.Udon;
using Thry;
using UnityEngine.UI;
using TMPro;
using Thry.CustomAttributes;
using System.Collections;
using System.Collections.Generic;
using VRC.SDK3.Data;
using Thry.Udon.UI;
using System;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
#endif

namespace Thry.Udon.AvatarTheme
{
    [Flags]
    public enum ColorBlockApplyFlag
    {
        // Powers of 2
        Normal = 0b0001,
        Highlighted = 0b0010,
        Pressed = 0b0100,
        Selected = 0b01000,
        Disabled = 0b10000,
        All = Normal | Highlighted | Pressed | Selected | Disabled
    }

    public static class ColorBlockApplyFlagExtensions
    {
        public static ColorBlockApplyFlag Invert(this ColorBlockApplyFlag flag)
        {
            return (ColorBlockApplyFlag)~(int)flag;
        }

        public static bool IsSet(this ColorBlockApplyFlag flag, ColorBlockApplyFlag checkFlag)
        {
            return ((int)flag & (int)checkFlag) == (int)checkFlag;
        }
    }

    [Singleton(false)]
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class AvatarThemeColor : ThryBehaviour
    {
        protected override string LogPrefix => "Thry.AvatarTheme.AvatarThemeColor";

        public Color InEditorTestColor = Color.magenta;
        public Camera LocalPlayerBodyCamera;
        public float LocalPlayerCameraSize = 0.25f;
        public Light[] Lights;
        public TextAsset ColorList;
        
        [Space(50)]
        [Header("Automatically populated")]
        [SerializeField] Light[] _disableLights;
        [SerializeField] ReflectionProbe[] _reflectionProbes;
        [SerializeField] Graphic[] _colorGraphics;
        [SerializeField] Renderer[] _colorRenderers;
        [SerializeField] Selectable[] _colorSelectables;
        ColorBlock[] _selectedableOriginalHSV = new ColorBlock[0];

        const int BINS_H = 10;
        const int BINS_S = 5;
        const int BINS_V = 4;

        const int C_COLORS = 5;
        
        bool _isSyncing = false;
        bool _isLoaded = false;

        private string[] _colorSearchNames;
        private Vector3[] _colorSearchColors;

        Color[] _prevPlayerColors = new Color[C_COLORS];
        Color[] _playerColors = new Color[C_COLORS];
        string[] _playerNames = new string[C_COLORS];

        [UdonSynced]
        Color[] _syncedColors = new Color[0];

        DataDictionary _appliedComponentsOriginalColorData = new DataDictionary();

        private UdonBehaviour[] _listenersGeneric = new UdonBehaviour[0];
        private AvatarThemeColorListener[] _listeners = new AvatarThemeColorListener[0];

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
            _selectedableOriginalHSV = new ColorBlock[_colorSelectables.Length];
            for (int i = 0; i < _colorSelectables.Length; i++)
            {
                _selectedableOriginalHSV[i] = ColorBlockToHSV(_colorSelectables[i].colors);
            }

        }

        [PublicAPI]
        /// <summary>Add a component</summary>
        /// <param name="component">Component to add</param>
        public Component RegisterComponent(Component c)
        {
            if(c.GetType() == typeof(Graphic))
            {
                Collections.Add(ref _colorGraphics, (Graphic)c);
                Log(LogLevel.Vervose, $"Added Graphic {c.name}");
            }
                
            if(c.GetType() == typeof(Renderer))
            {
                Collections.Add(ref _colorRenderers, (Renderer)c);
                Log(LogLevel.Vervose, $"Added Renderer {c.name}");
            }
            if(c.GetType() == typeof(Selectable) || c.GetType().IsSubclassOf(typeof(Selectable)))
            {
                Collections.Add(ref _colorSelectables, (Selectable)c);
                Collections.Add(ref _selectedableOriginalHSV, ColorBlockToHSV(((Selectable)c).colors));
                Log(LogLevel.Vervose, $"Added Selectable {c.name}");
            }
            return c;
        }

        [PublicAPI]
        /// <summary>Registers a listener for local player color changes</summary>
        /// <param name="listener">UdonBehaviour to receive the event</param>
        public UdonBehaviour RegisterListener(UdonBehaviour listener)
        {
            if(!Collections.Contains(_listenersGeneric, listener))
            {
                Collections.Add(ref _listenersGeneric, listener);
            }
            return listener;
        }

        [PublicAPI]
        /// <summary>Registers a listener for local player color changes</summary>
        /// <param name="listener">AvatarThemeColorListener to receive the event</param>
        public AvatarThemeColorListener RegisterListener(AvatarThemeColorListener listener)
        {
            if(!Collections.Contains(_listeners, listener))
            {
                Collections.Add(ref _listeners, listener);
            }
            return listener;
        }

        [PublicAPI]
        /// <summary>Unregisters a listener for local player color changes</summary>
        /// /// <param name="listener">UdonBehaviour to remove from the listeners</param>
        public UdonBehaviour UnregisterListener(UdonBehaviour listener)
        {
            Collections.Remove(_listenersGeneric, listener);
            return listener;
        }

        [PublicAPI]
        /// <summary>Unregisters a listener for local player color changes</summary>
        /// /// <param name="listener">AvatarThemeColorListener to remove from the listeners</param>
        public AvatarThemeColorListener UnregisterListener(AvatarThemeColorListener listener)
        {
            Collections.Remove(_listeners, listener);
            return listener;
        }

        [PublicAPI]
        /// <summary>Get the main color of a player</summary>
        /// <param name="player">Player to get the color from</param>
        /// <param name="outColor">Color to output</param>
        /// <returns>>Boolean for success or failure</returns>
        public bool GetColor(VRCPlayerApi player, out Color color)
        {
            int id = player.playerId;
            if (id * C_COLORS < _syncedColors.Length)
            {
                color = _syncedColors[id * C_COLORS];
                return true;
            }
            color = Color.black;
            return false;
        }

        [PublicAPI]
        /// <summary>Get the colors of a player</summary>
        /// <param name="player">Player to get the colors from</param>
        /// <param name="colors">Colors to output</param>
        /// <returns>>Boolean for success or failure</returns>
        public bool GetColors(VRCPlayerApi player, out Color[] colors)
        {
            int id = player.playerId;
            if (id * C_COLORS < _syncedColors.Length)
            {
                colors = new Color[C_COLORS];
                for (int i = 0; i < C_COLORS; i++)
                {
                    colors[i] = _syncedColors[id * C_COLORS + i];
                }
                return true;
            }
            colors = new Color[0];
            return false;
        }

        [PublicAPI]
        /// <summary>Get the main color name of a player</summary>
        /// <param name="player">Player to get the color name from</param>
        /// <param name="colorName">Color name to output</param>
        /// <returns>>Boolean for success or failure</returns>
        public bool GetColorName(VRCPlayerApi player, out string colorName)
        {
            int id = player.playerId;
            if (id * C_COLORS < _syncedColors.Length)
            {
                colorName = ColorToName(_syncedColors[id * C_COLORS]);
                return true;
            }
            colorName = "unknown";
            return false;
        }

        [PublicAPI]
        /// <summary>Get the color names of a player</summary>
        /// <param name="player">Player to get the color names from</param>
        /// <param name="colorNames">Color names to output</param>
        /// <returns>>Boolean for success or failure</returns>
        public bool GetColorNames(VRCPlayerApi player, out string[] colorNames)
        {
            int id = player.playerId;
            if (id * C_COLORS < _syncedColors.Length)
            {
                colorNames = new string[C_COLORS];
                for (int i = 0; i < C_COLORS; i++)
                {
                    colorNames[i] = ColorToName(_syncedColors[id * C_COLORS + i]);
                }
                return true;
            }
            colorNames = new string[0];
            return false;
        }

        [PublicAPI]
        /// <summary>Get the main color of the local player</summary>
        /// <returns>Main Color of the local player</returns>
        public Color GetColor()
        {
            return _playerColors[0];
        }

        [PublicAPI]
        /// <summary>Get the colors of the local player</summary>
        /// <returns>Colors of the local player</returns>
        public Color[] GetColors()
        {
            return _playerColors;
        }

        [PublicAPI]
        /// <summary>Get the main color name of the local player</summary>
        /// <returns>Main Color name of the local player</returns>
        public string GetColorName()
        {
            return _playerNames[0];
        }

        [PublicAPI]
        /// <summary>Get the color names of the local player</summary>
        /// <returns>Color names of the local player</returns>
        public string[] GetColorNames()
        {
            return _playerNames;
        }

        [PublicAPI]
        /// <summary>Apply the theme color to a ColorBlock</summary>
        /// <param name="block">ColorBlock to apply the theme color to</param>
        /// <returns>ColorBlock with the theme color applied</returns>
        public ColorBlock MixThemeColorIntoColorBlock(ColorBlock block, Vector3 hsvMixing, Vector3 hsvMultiply, ColorBlockApplyFlag blockFlags)
        {
            Vector4 hsvMixing4 = new Vector4(hsvMixing.x, hsvMixing.y, hsvMixing.z, 1);
            Vector4 hsvMultiply4 = new Vector4(hsvMultiply.x, hsvMultiply.y, hsvMultiply.z, 1);
            return HSVColorBlockMixReturnRGB(ColorBlockToHSV(block), ColorToHSV(_playerColors[0]), hsvMixing4, hsvMultiply4, blockFlags);
        }

        [PublicAPI]
        /// <summary>Apply the theme color to a Graphic, Renderer or Selectable</summary>
        /// <param name="c">Component to apply the theme color to</param>
        /// <returns>Component with the theme color applied</returns>
        public Component ApplyPlayerColor(Component c, VRCPlayerApi player, ColorBlockApplyFlag blockFlags)
        {
            if(!IsUseableType(c)) return c;
            if(GetColor(player, out Color theme))
            {
                ApplyColorToComponent(c, theme, blockFlags);
            }
            return c;
        }

        [PublicAPI]
        /// <summary>Apply the theme color to a Graphic, Renderer or Selectable</summary>
        /// <param name="c">Component to apply the theme color to</param>
        /// <returns>Component with the theme color applied</returns>
        public Component ApplyPlayerColor(Component c, VRCPlayerApi player)
        {
            if(!IsUseableType(c)) return c;
            if(GetColor(player, out Color theme))
            {
                ApplyColorToComponent(c, theme, ColorBlockApplyFlag.All);
            }
            return c;
        }

        private bool IsUseableType(Component c)
        {
            return c.GetType() == typeof(Graphic) || c.GetType() == typeof(Renderer) || c.GetType() == typeof(Selectable) || c.GetType().IsSubclassOf(typeof(Selectable));
        }

        private ColorBlock GetCachedOriginalColorData(Component c)
        {
            if(!_appliedComponentsOriginalColorData.ContainsKey(c))
                _appliedComponentsOriginalColorData.Add(c, new DataToken(GetColorBlockForComponent(c)));
            return (ColorBlock)_appliedComponentsOriginalColorData[c].Reference;
        }

        private void ApplyColorToComponent(Component c, Color theme, ColorBlockApplyFlag blockFlags)
        {
            if(c.GetType() == typeof(Graphic))
                ((Graphic)c).color = theme;
            if(c.GetType() == typeof(Renderer))
                ((Renderer)c).material.color = theme;
            if(c.GetType() == typeof(Selectable) || c.GetType().IsSubclassOf(typeof(Selectable)))
            {
                Selectable s = (Selectable)c;
                ColorBlock originalColors = GetCachedOriginalColorData(c);
                s.colors = HSVColorBlockMixReturnRGB(originalColors, ColorToHSV(theme), new Vector3(1, 0, 0), new Vector3(0, 1, 1), blockFlags);
            }
        }

        private void NotifyListeners()
        {
            foreach(AvatarThemeColorListener listener in _listeners)
            {
                if(listener == null) continue;
                listener.OnColorChange(_prevPlayerColors, _playerColors);
            }
            foreach(UdonBehaviour listener in _listenersGeneric)
            {
                if(listener == null) continue;
                listener.SendCustomEvent(nameof(AvatarThemeColorListener.OnColorChange));
            }
        }

        private void UpdateObjectsToColor(Color c)
        {
            Color.RGBToHSV(c, out float h, out float s, out float v);
            Color hsv = new Color(h, s, v);

            foreach (Graphic g in _colorGraphics)
            {
                if (g != null)
                    g.color = c;
            }
            foreach (Renderer r in _colorRenderers)
            {
                if (r != null)
                    r.material.color = c;
            }
            for(int i = 0; i < _colorSelectables.Length; i++)
            {
                if(_colorSelectables[i] != null)
                    _colorSelectables[i].colors = HSVColorBlockOverwriteHueMixSVReturnRGB(_selectedableOriginalHSV[i], hsv);
            }
        }

#region Color Calculation
        public void RequestUpdate()
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

            foreach (Light l in _disableLights)
                l.enabled = false;
            DisableReflectionProbes(lookAtPos);

            // render
            foreach (Light l in Lights)
                l.enabled = true;
            LocalPlayerBodyCamera.Render();
            foreach (Light l in Lights)
                l.enabled = false;

            // restore lighting
            EnableReflectionProbes();
            foreach (Light l in _disableLights)
                l.enabled = true;

            UnityEngine.RenderSettings.ambientMode = amb_mode;
            UnityEngine.RenderSettings.ambientLight = amb_color;

            VRCAsyncGPUReadback.Request(LocalPlayerBodyCamera.targetTexture, 0, (IUdonEventReceiver)this);
        }

        bool[] _reflectionProbeNeedsEnabling;
        void DisableReflectionProbes(Vector3 pos)
        {
            _reflectionProbeNeedsEnabling = new bool[_reflectionProbes.Length];
            for (int i = 0; i < _reflectionProbes.Length; i++)
            {
                // check if near
                Bounds bounds = _reflectionProbes[i].bounds;
                bounds.Expand(Networking.LocalPlayer.GetAvatarEyeHeightAsMeters() * 2);
                _reflectionProbeNeedsEnabling[i] = _reflectionProbes[i].enabled && bounds.Contains(pos);
                if (_reflectionProbeNeedsEnabling[i])
                    _reflectionProbes[i].enabled = false;
            }
        }

        void EnableReflectionProbes()
        {
            for (int i = 0; i < _reflectionProbes.Length; i++)
            {
                if (_reflectionProbeNeedsEnabling[i])
                {
                    _reflectionProbes[i].enabled = true;
                    if(_reflectionProbes[i].refreshMode == UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake)
                        _reflectionProbes[i].RenderProbe();
                }
            }
        }

        public override void OnAvatarChanged(VRCPlayerApi player)
        {
            // Delay by 1 second because of spawn in animation stuff
            if(player.isLocal)
                SendCustomEventDelayedSeconds(nameof(RequestUpdate), 1);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if(!_isLoaded)
                RequestUpdate();
            _isLoaded = true;
#endif
            if (Networking.LocalPlayer == null) return;
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

            this.SendCustomEventDelayedSeconds(nameof(CheckSynced), UnityEngine.Random.value * 15 + 3);
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
                    
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestIndex = i;
                }
            }
            return _colorSearchNames[smallestIndex];
        }

        public new void OnAsyncGpuReadbackComplete(VRCAsyncGPUReadbackRequest request)
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
                _prevPlayerColors = _playerColors;
                _playerColors = new Color[C_COLORS];
                EvaluatePlayerColor(px);
                for (int i = 0; i < 5; i++)
                    _playerNames[i] = ColorToName(_playerColors[i]);
                    
                NotifyListeners();
                UpdateObjectsToColor(_playerColors[0]);
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
            Log(LogLevel.Vervose, debug);

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

#if UNITY_EDITOR
            // Overwrite with pink for in editor testing
            for (int i = 0; i < _playerColors.Length; i++)
            {
                _playerColors[i] = InEditorTestColor;
            }
#endif

            debug = "[Thry][AviTheme] VSB Sort:\n";
            for (int i = 0; i < _playerColors.Length; i++)
            {
                debug += $"{i}: {ColorInfo(_playerColors[i])}\n";
                _playerColors[i].a = 1;
            }
            Log(LogLevel.Vervose, debug);
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
#endregion

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

        private Color ColorToHSV(Color rgba)
        {
            float h, s, v;
            Color.RGBToHSV(rgba, out h, out s, out v);
            return new Color(h, s, v, rgba.a);
        }

        private ColorBlock ColorBlockToHSV(ColorBlock b)
        {
            ColorBlock newBlock = new ColorBlock();
            newBlock.normalColor = ColorToHSV(b.normalColor);
            newBlock.highlightedColor = ColorToHSV(b.highlightedColor);
            newBlock.pressedColor = ColorToHSV(b.pressedColor);
            newBlock.selectedColor = ColorToHSV(b.selectedColor);
            newBlock.disabledColor = ColorToHSV(b.disabledColor);
            newBlock.colorMultiplier = b.colorMultiplier;
            newBlock.fadeDuration = b.fadeDuration;
            return newBlock;
        }

        private Color HSVOverwriteHueMixSVReturnRGB(Color orignHSV, Color applyHSV)
        {
            Color hsv = Color.HSVToRGB(applyHSV.r, orignHSV.g * applyHSV.g, orignHSV.b * applyHSV.b);
            hsv.a = orignHSV.a;
            return hsv;
        }

        private ColorBlock HSVColorBlockOverwriteHueMixSVReturnRGB(ColorBlock bHSV, Color applyHSV)
        {
            ColorBlock newBlock = new ColorBlock();
            newBlock.normalColor =      HSVOverwriteHueMixSVReturnRGB(bHSV.normalColor, applyHSV);
            newBlock.highlightedColor = HSVOverwriteHueMixSVReturnRGB(bHSV.highlightedColor, applyHSV);
            newBlock.pressedColor =     HSVOverwriteHueMixSVReturnRGB(bHSV.pressedColor, applyHSV);
            newBlock.selectedColor =    HSVOverwriteHueMixSVReturnRGB(bHSV.selectedColor, applyHSV);
            newBlock.disabledColor =    HSVOverwriteHueMixSVReturnRGB(bHSV.disabledColor, applyHSV);
            newBlock.colorMultiplier = bHSV.colorMultiplier;
            newBlock.fadeDuration = bHSV.fadeDuration;
            return newBlock;
        }

        private Color HSVMixReturnRGB(Color hsv0, Color hsv1, Vector4 mix, Vector4 multiply)
        {
            Color rgb = Color.HSVToRGB(
                mix.x * hsv1.r + (1 - mix.x) * hsv0.r * Mathf.Lerp(1, hsv1.r, multiply.x),
                mix.y * hsv1.g + (1 - mix.y) * hsv0.g * Mathf.Lerp(1, hsv1.g, multiply.y),
                mix.z * hsv1.b + (1 - mix.z) * hsv0.b * Mathf.Lerp(1, hsv1.b, multiply.z));
            rgb.a = mix.w * hsv1.a + (1 - mix.w) * hsv0.a * Mathf.Lerp(1, hsv1.a, multiply.w);
            return rgb;
        }

        private ColorBlock HSVColorBlockMixReturnRGB(ColorBlock bHSV, Color applyHSV, Vector4 mix, Vector4 multiply, ColorBlockApplyFlag blockFlags)
        {
            ColorBlock newBlock = new ColorBlock();
            if(blockFlags.IsSet(ColorBlockApplyFlag.Normal))      newBlock.normalColor =      HSVMixReturnRGB(bHSV.normalColor, applyHSV, mix, multiply);
            else                                                  newBlock.normalColor = Color.HSVToRGB(bHSV.normalColor.r, bHSV.normalColor.g, bHSV.normalColor.b);
            if(blockFlags.IsSet(ColorBlockApplyFlag.Highlighted)) newBlock.highlightedColor = HSVMixReturnRGB(bHSV.highlightedColor, applyHSV, mix, multiply);
            else                                                  newBlock.highlightedColor = Color.HSVToRGB(bHSV.highlightedColor.r, bHSV.highlightedColor.g, bHSV.highlightedColor.b);
            if(blockFlags.IsSet(ColorBlockApplyFlag.Pressed))     newBlock.pressedColor =     HSVMixReturnRGB(bHSV.pressedColor, applyHSV, mix, multiply);
            else                                                  newBlock.pressedColor = Color.HSVToRGB(bHSV.pressedColor.r, bHSV.pressedColor.g, bHSV.pressedColor.b);
            if(blockFlags.IsSet(ColorBlockApplyFlag.Selected))    newBlock.selectedColor =    HSVMixReturnRGB(bHSV.selectedColor, applyHSV, mix, multiply);
            else                                                  newBlock.selectedColor = Color.HSVToRGB(bHSV.selectedColor.r, bHSV.selectedColor.g, bHSV.selectedColor.b);
            if(blockFlags.IsSet(ColorBlockApplyFlag.Disabled))    newBlock.disabledColor =    HSVMixReturnRGB(bHSV.disabledColor, applyHSV, mix, multiply);
            else                                                  newBlock.disabledColor = Color.HSVToRGB(bHSV.disabledColor.r, bHSV.disabledColor.g, bHSV.disabledColor.b);
            newBlock.colorMultiplier = bHSV.colorMultiplier;
            newBlock.fadeDuration = bHSV.fadeDuration;
            return newBlock;
        }

        private ColorBlock GetColorBlockForComponent(Component c)
        {
            if(c.GetType() == typeof(Selectable) || c.GetType().IsSubclassOf(typeof(Selectable)))
            {
                return ColorBlockToHSV(((Selectable)c).colors);
            }
            ColorBlock cb = new ColorBlock();
            if(c.GetType() == typeof(Graphic))
            {
                cb.normalColor = ((Graphic)c).color;
            }
            if(c.GetType() == typeof(Renderer))
            {
                cb.normalColor = ((Renderer)c).material.color;
            }
            return cb;
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP

        const string TAG_COLOR_OBJECTS = "AvatarThemeColor";

        // register scene saving callback on compile
        [InitializeOnLoadMethod]
        private static void RegisterSceneSavingCallback()
        {
            EditorSceneManager.sceneSaving += OnBeforeSave;
            EditorHelper.AddTag(TAG_COLOR_OBJECTS);
        }

        static T[] FindAllFindObjectsByType<T>()
        {
            return FindObjectsByType(typeof(T), FindObjectsInactive.Include, FindObjectsSortMode.None) as T[];
        }

        static bool IEnumerableContainsSameElements<T>(IEnumerable<T> e1, IEnumerable<T> e2)
        {
            if (e1 == null || e2 == null)
                return e1 == e2;
            if(e1.Count() != e2.Count())
                return false;
                
            foreach (T t in e1)
            {
                if (!e2.Contains(t))
                    return false;
                    
            }
            return true;
        }

        static void OnBeforeSave(Scene scene, string path)
        {
            // Find all objects that should be colored by tag
            SimpleUIInput[] colorButtons = FindAllFindObjectsByType<SimpleUIInput>().Where(b => b.tag == TAG_COLOR_OBJECTS).ToArray();
            Selectable[] colorSelectables = FindAllFindObjectsByType<Selectable>().Where(s => s.tag == TAG_COLOR_OBJECTS && s.GetComponent<SimpleUIInput>() == null).ToArray();
            Graphic[] colorGraphics = FindAllFindObjectsByType<Graphic>().Where(g => g.tag == TAG_COLOR_OBJECTS && g.GetComponent<Selectable>() == null && g.GetComponent<SimpleUIInput>() == null).ToArray();
            Renderer[] colorRenderers = FindAllFindObjectsByType<Renderer>().Where(r => r.tag == TAG_COLOR_OBJECTS).ToArray();
            
            // Find all objects that can affect the lighting of the avatar
            Light[] lights = FindAllFindObjectsByType<Light>();
            ReflectionProbe[] reflectionProbes = FindAllFindObjectsByType<ReflectionProbe>();
            LightProbeGroup[] lightProbeGroups = FindAllFindObjectsByType<LightProbeGroup>();

            AvatarThemeColor[] scripts = FindAllFindObjectsByType<AvatarThemeColor>();
            foreach (AvatarThemeColor s in scripts)
            {
                Light[] scriptLights = lights.Where(l => s.Lights.Contains(l) == false).ToArray();

                bool hasNotChanged = 
                    IEnumerableContainsSameElements(s._colorGraphics, colorGraphics) &&
                    IEnumerableContainsSameElements(s._colorRenderers, colorRenderers) &&
                    IEnumerableContainsSameElements(s._colorSelectables, colorSelectables) &&
                    IEnumerableContainsSameElements(s._disableLights, scriptLights) &&
                    IEnumerableContainsSameElements(s._reflectionProbes, reflectionProbes);

                s._colorGraphics = colorGraphics;
                s._colorRenderers = colorRenderers;
                s._colorSelectables = colorSelectables;

                s._disableLights = scriptLights;
                s._reflectionProbes = reflectionProbes;

                if (!hasNotChanged)
                {
                    EditorUtility.SetDirty(s);
                    Logger.Log("Thry.AvatarTheme.AvatarThemeColor", "Updated AvatarThemeColor references");
                }
            }
        }
#endif
    }
}
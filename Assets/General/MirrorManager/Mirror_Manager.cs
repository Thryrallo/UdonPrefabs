
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Mirror_Manager : UdonSharpBehaviour
    {
        public int RequiredClaps = 2;
        public string ClapperKey = "m";
        public float MaximumOpeningDistance = 5;
        // Layer 0, 4, 9, 11, 13, 14, 17, 18
        public LayerMask FullMask = 1 << 0 | 1 << 4 | 1 << 9 | 1 << 11 | 1 << 13 | 1 << 14 | 1 << 17 | 1 << 18;
        // Layer 9, 18
        public LayerMask AllPlayerMask = 1 << 9 | 1 << 18;
        // Layer 18
        public LayerMask LocalPlayerMask = 1 << 18;

        public Material NormalMirrorMaterial;
        public Material TransprentMirrorMaterial;

        [NonSerialized] public float Transparency = 1;
        private VRC_MirrorReflection _activeMirrorRefelctionMain;
        private VRC_MirrorReflection _activeMirrorRefelctionCutout;
        private Renderer _activeMirrorRendererMain;
        private Renderer _activeMirrorRendererCutout;
        private Renderer _activeMirrorIcon;
        private int _lastMask;
        [NonSerialized] public bool DoCutout;
        private bool _hasCutout;

        private TextMeshProUGUI _mirrorInformation;

        public static Mirror_Manager Get()
        {
            GameObject go = GameObject.Find("[Thry]Mirror_Manager");
            if (go != null)
                return go.GetComponent<Mirror_Manager>();
            return null;
        }

        private void Start()
        {
            Clapper clapper = Clapper.Get();
            SAO.Menu sao = SAO.Menu.Get();
            if(clapper)
            {
                clapper.RegisterClapperAction(this, nameof(OnClap), RequiredClaps, ClapperKey);
            }
            GameObject go = GameObject.Find("[Thry]Mirror_Information_Text");
            if(go != null)
            {
                _mirrorInformation = go.GetComponent<TextMeshProUGUI>();
                string text = _mirrorInformation.text;
                if(clapper)
                {
                    text += $"\n    > Clap {RequiredClaps} times";
                    text += $"\n    > Press the '{ClapperKey.ToUpper()}' key";
                }
                if(sao)
                {
                    text += $"\n    > Use the SAO Menu";
                    text += $"\n        - Point Finger & Swipe down";
                    text += $"\n        - Press the '{sao.GestureRecognizer.MenuKeyCode}'";
                }
                _mirrorInformation.text = text;
            }

            _lastMask = FullMask;
        }

        public void OnClap()
        {
            ToggleMirror(_lastMask);
        }

        public void FullMirror()
        {
            ToggleMirror(FullMask);
            ApplyTransparency(Transparency);
        }

        public void PlayerMirror()
        {
            ToggleMirror(AllPlayerMask);
            ApplyTransparency(Transparency);
        }

        public void LocalPlayerMirror()
        {
            ToggleMirror(LocalPlayerMask);
            ApplyTransparency(Transparency);
        }

        public void UpdateTransparency()
        {
            ApplyTransparency(Transparency);
        }

        public void UpdateCutout()
        {
            SetActiveMirrorOn(_lastMask);
            ApplyTransparency(Transparency);
        }

        private void ApplyTransparency(float value)
        {
            if (_activeMirrorRefelctionMain)
            {
                value = Mathf.Clamp01(value);
                if (value == 1)
                {
                    _activeMirrorRendererMain.sharedMaterial = NormalMirrorMaterial;
                }
                else
                {
                    TransprentMirrorMaterial.SetFloat("_Transparency", value);
                    _activeMirrorRendererMain.sharedMaterial = TransprentMirrorMaterial;
                }
                if(_hasCutout)
                    _activeMirrorRendererCutout.sharedMaterial.SetFloat("_Transparency", value);
            }
        }

        private void ToggleMirror(int mask)
        {
            VRCPlayerApi.TrackingData trackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

            Vector3 lookDirection = (trackingData.rotation * Vector3.forward).normalized;
            Ray lookRay = new Ray(trackingData.position, lookDirection);

            RaycastHit hit;
            if (Physics.Raycast(lookRay, out hit, MaximumOpeningDistance, 16, QueryTriggerInteraction.Collide))
            {
                GameObject obj = hit.collider.gameObject;
                VRC_MirrorReflection mirror = (VRC_MirrorReflection)obj.GetComponentInChildren(typeof(VRC_MirrorReflection));
                Renderer renderer = obj.GetComponent<Renderer>();
                Renderer icon = GetIcon(obj.transform);
                if (mirror && renderer)
                {
                    // _activeMirrorRefelction.m_ReflectLayers = LocalPlayerMask;
                    if(_activeMirrorRefelctionMain == mirror)
                    {
                        if(_activeMirrorRefelctionMain.m_ReflectLayers != mask)
                        {
                            SetActiveMirrorOn(mask);
                        }else
                        {
                            SetActiveMirrorOff();
                        }
                    }
                    else
                    {
                        SetActiveMirrorOff();
                        _activeMirrorRefelctionMain = mirror;
                        _activeMirrorRendererMain = renderer;
                        _activeMirrorIcon = icon;
                        _activeMirrorRefelctionCutout = null;
                        _activeMirrorRendererCutout = null;
                        _hasCutout = false;
                        if (mirror.transform.childCount > 0)
                        {
                            Transform child = mirror.transform.GetChild(0);
                            _activeMirrorRefelctionCutout = (VRC_MirrorReflection)child.GetComponent(typeof(VRC_MirrorReflection));
                            _activeMirrorRendererCutout = child.GetComponentInChildren<Renderer>();
                            _hasCutout = _activeMirrorRefelctionCutout != null;
                        }
                        SetActiveMirrorOn(mask);
                    }
                    return;
                }
            }
            SetActiveMirrorOff();
        }

        private void SetActiveMirrorOff()
        {
            if (_activeMirrorRefelctionMain)
            {
                _activeMirrorRefelctionMain.enabled = false;
                _activeMirrorRendererMain.enabled = false;
                _activeMirrorRefelctionCutout.enabled = false;
                _activeMirrorRendererCutout.enabled = false;
                if (_activeMirrorIcon)
                    _activeMirrorIcon.enabled = true;
                _activeMirrorRefelctionMain = null;
                _activeMirrorRendererMain = null;
                _activeMirrorIcon = null;
            }
        }
        
        private void SetActiveMirrorOn(int mask)
        {
            if (_activeMirrorRefelctionMain)
            {
                _lastMask = mask;

                if (_hasCutout)
                {
                    if(DoCutout)
                    {
                        _activeMirrorRefelctionMain.enabled = false;
                        _activeMirrorRendererMain.enabled = false;
                        _activeMirrorRefelctionCutout.enabled = true;
                        _activeMirrorRendererCutout.enabled = true;
                    }
                    else
                    {
                        _activeMirrorRefelctionCutout.enabled = false;
                        _activeMirrorRendererCutout.enabled = false;
                        _activeMirrorRefelctionMain.enabled = true;
                        _activeMirrorRendererMain.enabled = true;
                    }
                    _activeMirrorRefelctionMain.m_ReflectLayers = mask;
                    _activeMirrorRefelctionCutout.m_ReflectLayers = mask;
                }
                else
                {
                    _activeMirrorRefelctionMain.enabled = true;
                    _activeMirrorRendererMain.enabled = true;
                    _activeMirrorRefelctionMain.m_ReflectLayers = mask;
                }

                if (_activeMirrorIcon)
                    _activeMirrorIcon.enabled = false;
            }
        }

        private Renderer GetIcon(Transform mirrorTransform)
        {
            int siblingIndex = 0;
            if (mirrorTransform.parent != null && (siblingIndex = mirrorTransform.GetSiblingIndex()) > 0)
            {
                Transform t = mirrorTransform.parent.GetChild(siblingIndex - 1);
                if (t.name == "MirrorIcon")
                {
                    return t.GetComponent<Renderer>();
                }
            }
            return null;
        }
    }
}
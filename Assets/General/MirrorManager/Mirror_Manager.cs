
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
        public const byte TYPE_FULL = 0;
        public const byte TYPE_PLAYER = 1;
        public const byte TYPE_LOCAL_PLAYER = 2;

        public int RequiredClaps = 2;
        public string ClapperKey = "m";
        public float MaximumOpeningDistance = 5;
        // Layer 0, 4, 9, 11, 13, 14, 17, 18
        public LayerMask FullMask = 1 << 0 | 1 << 4 | 1 << 9 | 1 << 11 | 1 << 13 | 1 << 14 | 1 << 17 | 1 << 18;
        // Layer 9, 18
        public LayerMask AllPlayerMask = 1 << 9 | 1 << 18;
        // Layer 18
        public LayerMask LocalPlayerMask = 1 << 18;

        [NonSerialized] public float Transparency = 1;
        private ThryMirror _activeMirror;
        private int _lastMask;
        [NonSerialized] public bool DoCutout;

        private TextMeshProUGUI _mirrorInformation;
        private ThryMirrorMenu[] _allMenus = new ThryMirrorMenu[2];
        private int _allMenusLength = 0;

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

        public void RegisterMirrorMenu(ThryMirrorMenu menu)
        {
            if(_allMenusLength >= _allMenus.Length)
            {
                ThryMirrorMenu[] newMenus = new ThryMirrorMenu[_allMenus.Length * 2];
                Array.Copy(_allMenus, newMenus, _allMenus.Length);
                _allMenus = newMenus;
            }
            _allMenus[_allMenusLength++] = menu;
        }

        public void OnClap()
        {
            FindMirror(_lastMask);
        }

        public void FullMirror()
        {
            FindMirror(FullMask);
        }

        public void PlayerMirror()
        {
            FindMirror(AllPlayerMask);
        }

        public void LocalPlayerMirror()
        {
            FindMirror(LocalPlayerMask);
        }

        public void MirrorSettingsChanged()
        {
            if (_activeMirror)
            {
                _activeMirror.Set(_lastMask, DoCutout, Transparency);
            }
            for(int i=0;i<_allMenusLength;i++)
            {
                _allMenus[i].UpdateUI();
            }
        }

        private void FindMirror(int mask)
        {
            VRCPlayerApi.TrackingData trackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

            Vector3 lookDirection = (trackingData.rotation * Vector3.forward).normalized;
            Ray lookRay = new Ray(trackingData.position, lookDirection);

            RaycastHit hit;
            if (Physics.Raycast(lookRay, out hit, MaximumOpeningDistance, 16, QueryTriggerInteraction.Collide))
            {
                if(hit.transform.transform.parent != null)
                {
                    ThryMirror mirror = hit.transform.parent.GetComponent<ThryMirror>(); 
                    if (mirror)
                    {
                        OpenInternal(mirror, mask);
                        return;
                    }
                }
            }
            
            TurnOffActive();
        }

        public void Open(ThryMirror mirror, int type)
        {
            switch (type)
            {
                case TYPE_FULL:
                    OpenInternal(mirror, FullMask);
                    break;
                case TYPE_PLAYER:
                    OpenInternal(mirror, AllPlayerMask);
                    break;
                case TYPE_LOCAL_PLAYER:
                    OpenInternal(mirror, LocalPlayerMask);
                    break;
            }
        }

        private void OpenInternal(ThryMirror mirror, int mask)
        {
            if(_activeMirror == mirror)
            {
                if(_lastMask != mask)
                {
                    _activeMirror.Set(mask, DoCutout, Transparency);
                    _lastMask = mask;
                }else
                {
                    TurnOffActive();
                }
            }
            else
            {
                TurnOffActive();
                _activeMirror  = mirror;
                _activeMirror.Set(mask, DoCutout, Transparency);
                _lastMask = mask;
            }
        }

        private void TurnOffActive()
        {
            if(_activeMirror)
            {
                _activeMirror.SetOff();
                _activeMirror = null;
            }
        }
    }
}
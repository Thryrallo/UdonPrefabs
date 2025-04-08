
using System;
using JetBrains.Annotations;
using Thry.CustomAttributes;
using Thry.Udon.SwipeMenu;
using Thry.Udon.UI;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.Mirror
{
    public enum MirrorType
    {
        NONE = 0,
        FULL = 1,
        PLAYER = 2,
        LOCAL_PLAYER = 3
    }

    [Singleton] 
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
        private LayerMask[] _typeToMask = new LayerMask[4];

        [NonSerialized] public float Transparency = 1;
        private ThryMirror _activeMirror;
        private ThryMirrorMenu _activeMenu;
        private int _mask;
        private MirrorType _type;
        private Transform _lastTransform;
        [NonSerialized] public bool DoCutout = true;

        private TextMeshProUGUI _mirrorInformation;
        private ThryMirrorMenu[] _allMenus = new ThryMirrorMenu[2];
        private int _allMenusLength = 0;

        [SerializeField, HideInInspector] Clapper _clapper;
        [SerializeField, HideInInspector] SwipeMenuManager _sao;
        
        private string _initalMirrorInformationText;

        [PublicAPI]
        /// <summary>Get a reference to the Mirror Manager component</summary>
        /// <returns>Mirror Manager component</returns>
        /// <remarks>Cache the result of this function for performance reasons</remarks>
        public static Mirror_Manager Get()
        {
            GameObject go = GameObject.Find("[Thry]Mirror_Manager");
            if (go != null)
                return go.GetComponent<Mirror_Manager>();
            return null;
        }

        private void Start()
        {
            if (_clapper)
            {
                _clapper.RegisterClapperAction(this, nameof(OnClap), RequiredClaps, ClapperKey);
            }

            _typeToMask[0] = 0;
            _typeToMask[1] = FullMask;
            _typeToMask[2] = AllPlayerMask;
            _typeToMask[3] = LocalPlayerMask;

            _mask = FullMask;
            _type = MirrorType.FULL;

            GameObject go = GameObject.Find("[Thry]Mirror_Information_Text");
            if (go != null)
            {
                _mirrorInformation = go.GetComponent<TextMeshProUGUI>();
                if (_mirrorInformation)
                    _initalMirrorInformationText = _mirrorInformation.text;
            }
            UpdateMirrorInformation();
        }

        void UpdateMirrorInformation()
        {
            if (_mirrorInformation)
            {
                string text = _initalMirrorInformationText;
                //if(_allMenusLength > 0)
                //{
                //    text += $"\n    > Using the Mirror's Menu";
                //}
                if (_clapper)
                {
                    text += $"\n    > Clap {RequiredClaps} times";
                    text += $"\n    > Press the '{ClapperKey.ToUpper()}' key";
                }
                if (_sao)
                {
                    text += $"\n    > Use the SAO Menu";
                    text += $"\n        - Point Finger & Swipe down";
                    text += $"\n        - Press the '{_sao.GestureRecognizer.MenuKeyCode}'";
                }
                _mirrorInformation.text = text;
            }
        }

        public void RegisterMirrorMenu(ThryMirrorMenu menu)
        {
            Collections.AssertArrayLength(ref _allMenus, _allMenusLength + 1);
            _allMenus[_allMenusLength++] = menu;
        }

        public void OnClap()
        {
            FindMirror(_type);
        }

        public void FullMirror()
        {
            FindMirror(MirrorType.FULL);
        }

        public void PlayerMirror()
        {
            FindMirror(MirrorType.PLAYER);
        }

        public void LocalPlayerMirror()
        {
            FindMirror(MirrorType.LOCAL_PLAYER);
        }

        public void MirrorSettingsChanged()
        {
            if (_activeMirror)
            {
                _activeMirror.Set(_mask, _type, DoCutout, Transparency);
            }
            if (_activeMenu)
            {
                _activeMenu.SetMirrorBorder(_type);
            }
            for (int i = 0; i < _allMenusLength; i++)
            {
                _allMenus[i].UpdateUI();
            }
        }

        private void FindMirror(MirrorType type)
        {
            VRCPlayerApi.TrackingData trackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

            Vector3 lookDirection = (trackingData.rotation * Vector3.forward).normalized;
            Ray lookRay = new Ray(trackingData.position, lookDirection);

            RaycastHit hit;
            if (Physics.Raycast(lookRay, out hit, MaximumOpeningDistance, 16, QueryTriggerInteraction.Collide))
            {
                if (hit.transform.parent != null)
                {
                    ThryMirror mirror = hit.transform.parent.GetComponent<ThryMirror>();
                    if (mirror)
                    {
                        bool didTypeChange = SetType(type);
                        HandleInternal(hit.transform, mirror, didTypeChange);
                        return;
                    }
                }
            }

            DisableActive();
        }

        private void FindMenu()
        {
            if (_activeMirror)
            {
                foreach (ThryMirrorMenu menu in _allMenus)
                {
                    if (menu && menu.Target == _activeMirror)
                    {
                        _activeMenu = menu;
                        return;
                    }
                }
            }
        }

        [PublicAPI]
        public bool SetType(MirrorType type)
        {
            bool didChange = _type != type;
            _type = type;
            _mask = _typeToMask[(int)type];
            return didChange;
        }

        [PublicAPI]
        public void Toggle(Transform transform, ThryMirror mirror)
        {
            HandleInternal(transform, mirror, false);
        }

        [PublicAPI]
        public void Disable()
        {
            DisableActive();
        }

        private void HandleInternal(Transform transform, ThryMirror mirror, bool didTypeChange)
        {
            if (_activeMirror == mirror)
            {
                if (didTypeChange)
                    MirrorSettingsChanged();
                else if (_activeMirror.Toggle(transform))
                    DisableActive();
                else if (_activeMenu)
                    _activeMenu.SetMirrorBorder(_type);
            }
            else
            {
                DisableActive();
                _activeMirror = mirror;
                _activeMirror.Set(_mask, _type, DoCutout, Transparency);
                _activeMirror.Toggle(transform);
                FindMenu();
                if (_activeMenu)
                    _activeMenu.SetMirrorBorder(_type);
            }
        }

        private void DisableActive()
        {
            if (_activeMirror)
            {
                _activeMirror.Disable();
                _activeMirror = null;
            }
            if (_activeMenu)
            {
                _activeMenu.SetMirrorBorder(MirrorType.NONE);
                _activeMenu = null;
            }

        }
    }
}
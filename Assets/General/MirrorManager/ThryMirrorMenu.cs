
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ThryMirrorMenu : UdonSharpBehaviour
    {
        public ThryMirror Target;

        [Space(200)]
        public UnityEngine.UI.Slider TransparencySlider;
        public UnityEngine.UI.Toggle CutoutToggle;
        public Animator CutoutAnimator;
        public UnityEngine.UI.Image FullButtonBorder;
        public UnityEngine.UI.Image PlayerButtonBorder;
        public UnityEngine.UI.Image LocalPlayerButtonBorder;
        public Color ActivatedBorderColor = Color.green;

        Mirror_Manager _manager;
        Color _defaultBorderColor;

        void Start()
        {
            _manager = Mirror_Manager.Get();
            if(_manager)
            {
                _manager.RegisterMirrorMenu(this);
                UpdateUI();
            }
            if(FullButtonBorder)
                _defaultBorderColor = FullButtonBorder.color;
        }

        void OnEnable()
        {
            if(_manager)
                UpdateUI();
        }

        public void OpenFull()
        {
            _manager.Open(Target, Mirror_Manager.TYPE_FULL);
        }

        public void OpenPlayer()
        {
            _manager.Open(Target, Mirror_Manager.TYPE_PLAYER);
        }

        public void OpenLocalPlayer()
        {
            _manager.Open(Target, Mirror_Manager.TYPE_LOCAL_PLAYER);
        }

        public void SetTransparency()
        {
            _manager.Transparency = TransparencySlider.value;
            _manager.MirrorSettingsChanged();
        }

        public void SetCutout()
        {
            _manager.DoCutout = CutoutToggle.isOn;
            _manager.MirrorSettingsChanged();
        }

        public void UpdateUI()
        {
            TransparencySlider.SetValueWithoutNotify(_manager.Transparency);
            CutoutToggle.SetIsOnWithoutNotify(_manager.DoCutout);
            CutoutAnimator.SetBool("isOn", _manager.DoCutout);
        }

        public void SetMirrorBorder(int type)
        {
            FullButtonBorder.color = _defaultBorderColor;
            PlayerButtonBorder.color = _defaultBorderColor;
            LocalPlayerButtonBorder.color = _defaultBorderColor;
            switch (type)
            {
                case Mirror_Manager.TYPE_FULL:
                    FullButtonBorder.color = ActivatedBorderColor;
                    break;
                case Mirror_Manager.TYPE_PLAYER:
                    PlayerButtonBorder.color = ActivatedBorderColor;
                    break;
                case Mirror_Manager.TYPE_LOCAL_PLAYER:
                    LocalPlayerButtonBorder.color = ActivatedBorderColor;
                    break;
            }
        }
    }
}
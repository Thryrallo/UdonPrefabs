
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common;

namespace Thry.Udon.Mirror
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
        
        [Space(10)]
        public GameObject MultiMirrorSection;
        public UnityEngine.UI.Image MirrorButtonFront;
        public UnityEngine.UI.Image MirrorButtonTop;
        public UnityEngine.UI.Image MirrorButtonLeft;
        public UnityEngine.UI.Image MirrorButtonRight;

        Mirror_Manager _manager;
        Color _defaultBorderColor;
        private HandType _lastUseHand = HandType.LEFT;

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

            if (MultiMirrorSection && Target)
            {
                MultiMirrorSection.SetActive(Target.IsMappedMultiMirror);
                if (Target.IsMappedMultiMirror)
                {
                    MirrorButtonFront.gameObject.SetActive(Target.Front);
                    MirrorButtonTop.gameObject.SetActive(Target.Top);
                    MirrorButtonLeft.gameObject.SetActive(Target.Left);
                    MirrorButtonRight.gameObject.SetActive(Target.Right);
                }
            }
        }

        void OnEnable()
        {
            if(_manager)
                UpdateUI();
        }

        public void TypeHigh()
        {
            Open(MirrorType.FULL);
            PlayHaptics();
        }

        public void TypeLow()
        {
            Open(MirrorType.PLAYER);
            PlayHaptics();
        }

        public void TypeVeryLow()
        {
            Open(MirrorType.LOCAL_PLAYER);
            PlayHaptics();
        }

        private void Open(MirrorType type)
        {
            bool didTypeChange = _manager.SetType(type);
            if (!Target.IsAnyActive)
            {
                _manager.Toggle(Target.Main.transform, Target);
            }
            else if (didTypeChange)
            {
                _manager.MirrorSettingsChanged();
            }
            else if(!didTypeChange)
            {
                _manager.Disable();
            }
        }

        public void Front()
        {
            _manager.Toggle(Target.Front.transform, Target);
            PlayHaptics();
        }

        public void Top()
        {
            _manager.Toggle(Target.Top.transform, Target);
            PlayHaptics();
        }

        public void Left()
        {
            _manager.Toggle(Target.Left.transform, Target);
            PlayHaptics();
        }

        public void Right()
        {
            _manager.Toggle(Target.Right.transform, Target);
            PlayHaptics();
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
            PlayHaptics();
        }

        public void UpdateUI()
        {
            TransparencySlider.SetValueWithoutNotify(_manager.Transparency);
            CutoutToggle.SetIsOnWithoutNotify(_manager.DoCutout);
            CutoutAnimator.SetBool("isOn", _manager.DoCutout);
        }

        public void SetMirrorBorder(MirrorType type)
        {
            FullButtonBorder.color = _defaultBorderColor;
            PlayerButtonBorder.color = _defaultBorderColor;
            LocalPlayerButtonBorder.color = _defaultBorderColor;
            switch (type)
            {
                case MirrorType.FULL:
                    FullButtonBorder.color = ActivatedBorderColor;
                    break;
                case MirrorType.PLAYER:
                    PlayerButtonBorder.color = ActivatedBorderColor;
                    break;
                case MirrorType.LOCAL_PLAYER:
                    LocalPlayerButtonBorder.color = ActivatedBorderColor;
                    break;
            }
            if(Target.IsMappedMultiMirror)
            {
                MirrorButtonFront.color = Target.Front.enabled ? ActivatedBorderColor : _defaultBorderColor;
                MirrorButtonTop.color = Target.Top.enabled ? ActivatedBorderColor : _defaultBorderColor;
                MirrorButtonLeft.color = Target.Left.enabled ? ActivatedBorderColor : _defaultBorderColor;
                MirrorButtonRight.color = Target.Right.enabled ? ActivatedBorderColor : _defaultBorderColor;
            }
        }

        void PlayHaptics()
        {
            Networking.LocalPlayer.PlayHapticEventInHand(_lastUseHand == HandType.LEFT ? VRC_Pickup.PickupHand.Left : VRC_Pickup.PickupHand.Right, 0.5f, 0.8f, 1f);
        }
        
        public override void InputUse(bool value, UdonInputEventArgs args)
        {
            _lastUseHand = args.handType;
        }
    }
}
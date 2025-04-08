
using JetBrains.Annotations;
using Thry.CustomAttributes;
using Thry.Udon.AvatarTheme;
using Thry.UI;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using UnityEngine.XR;



#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
#endif

namespace Thry.Udon.SwipeMenu
{
    [Singleton]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SwipeMenuManager : ThryBehaviour
    {
        protected override string LogPrefix => "Thry.SwipeMenuManager";

        [Space(20)]

        [SerializeField] private bool _selectableColorsUseAvatarTheme = true;
        [SerializeField] private ColorBlockApplyFlag _selectableColorsAvatarThemeFlag = (ColorBlockApplyFlag)0b11111;
        [Header("Selectable Color 1")]
        [SerializeField] private ColorBlock _selectableColors1 = new ColorBlock()
            {
                normalColor = new Color(0.69f, 0.69f, 0.69f, 1),
                highlightedColor = new Color(1, 0.79f, 0.5f, 1),
                pressedColor = new Color(1, 0.58f, 0, 1),
                selectedColor = new Color(1, 0.79f, 0.5f, 1),
                disabledColor = new Color(0.79f, 0.79f, 0.70f, 0.5f),
                fadeDuration = 0.1f,
                colorMultiplier = 1,
            };
        [Header("Selectable Color 2")]
        [SerializeField] private ColorBlock _selectableColors2 = new ColorBlock()
            {
                normalColor = new Color(0.69f, 0.69f, 0.69f, 1),
                highlightedColor = new Color(1, 0.79f, 0.5f, 1),
                pressedColor = new Color(1, 0.58f, 0, 1),
                selectedColor = new Color(1, 0.79f, 0.5f, 1),
                disabledColor = new Color(0.79f, 0.79f, 0.70f, 0.5f),
                fadeDuration = 0.1f,
                colorMultiplier = 1,
            };

        private ColorBlock _appliedSelectableColors1;
        private ColorBlock _appliedSelectableColors2;

        public ColorBlock SelectableColors1 => _appliedSelectableColors1;
        public ColorBlock SelectableColors2 => _appliedSelectableColors2;


        [Space(20)]
        [Header("References")]
        public Gestures GestureRecognizer;
        public AudioSource OpenSound;

        [Header("Animation")]
        public Vector3 startPosition = new Vector3(0, 450, 0);
        public float ANIMATION_DURATION = 0.5f;
        public float SUBMENU_ANIMATION_DURATION = 0.2f;
        public float ANIMATION_Y_MODIFIER = 0.001f;
        public float ANIMATION_Y_OFFSET = 0.15f;

        public float MENU_HEIGHT = 0.2f;

        public float CLOSE_VELOCITY = 0.1f;

        public Transform Canvas;

        public BoxCollider raycastCollider;
        public ToggleGroup MainToggleGroup;
        public Toggle HomeToggle;

        [Tooltip("Enables these gameobjects while menu is open. Suggested use indicators.")]
        public GameObject[] inWorldIndicators;
        [Tooltip("These get disabled when the scene is started.")]
        public GameObject[] disableOnLoad;
        
        [Header("Test")]
        [SerializeField] bool _simulateVR = false;
        [SerializeField, HideInInspector] private ClickSound _clickSound;
        [SerializeField, HideInInspector] private AvatarThemeColor _avatarThemeColor;

        private bool _forceLaserInteraction = false;

        private float _animationStartTime;
        private int _animationDirection = 1;

        private bool _isFinished = false;
        private bool _isInitialized = false;
        private Vector3[] _targetPositions;
        private Vector3[] _targetScale;

        public void Start()
        {
            Init();
            OnDisable();
            this.gameObject.SetActive(false);
            foreach (GameObject o in disableOnLoad) o.SetActive(false);
            _appliedSelectableColors1 = _selectableColors1;
            _appliedSelectableColors2 = _selectableColors2;
            if(_avatarThemeColor)
            {
                _avatarThemeColor.RegisterListener(this.GetComponent<UdonBehaviour>());
            }
            _isFinished = true;
        }

        public void OnEnable()
        {
            Init();
            LogDebugInformation();
            _animationDirection = 1;
            _animationStartTime = Time.time;
            _isFinished = false;
        }

        public void OnDisable()
        {
            if (_isInitialized)
            {
                for (int i = 0; i < Canvas.childCount; i++)
                {
                    Transform child = Canvas.GetChild(i);
                    child.localScale = Vector3.zero;
                    child.localPosition = _targetPositions[i];
                }
            }
        }

        public void OnColorChange()
        {
            if(!_selectableColorsUseAvatarTheme) return;
            _appliedSelectableColors1 = _avatarThemeColor.MixThemeColorIntoColorBlock(_selectableColors1, new Vector3(1, 0, 0), new Vector3(0, 1, 1), _selectableColorsAvatarThemeFlag);
            _appliedSelectableColors2 = _avatarThemeColor.MixThemeColorIntoColorBlock(_selectableColors2, new Vector3(1, 0, 0), new Vector3(0, 1, 1), _selectableColorsAvatarThemeFlag);
            ApplyThemeColors();
        }

        private void ApplyThemeColors()
        {
            GlobalSelectableColor[] globalSelectableColors = GetComponentsInChildren<GlobalSelectableColor>(true);
            foreach (GlobalSelectableColor gColorSel in globalSelectableColors)
            {
                Selectable selectable = gColorSel.GetComponent<Selectable>();
                if (selectable != null)
                {
                    selectable.colors = GetColorBlock(gColorSel.Index);
                }else
                {
                    Graphic graphic = gColorSel.GetComponent<Graphic>();
                    if (graphic != null)
                    {
                        graphic.color = GetColor(gColorSel.Index, gColorSel.ColorState);
                    }
                }
            }
        }

        private ColorBlock GetColorBlock(GLobalColorIndex index)
        {
            if (index == GLobalColorIndex.Color1)
                return _appliedSelectableColors1;
            else
                return _appliedSelectableColors2;
        }

        private Color GetColor(GLobalColorIndex index, ColorBlockType type)
        {
            ColorBlock colorBlock = GetColorBlock(index);
            switch (type)
            {
                case ColorBlockType.Normal:
                    return colorBlock.normalColor;
                case ColorBlockType.Highlighted:
                    return colorBlock.highlightedColor;
                case ColorBlockType.Pressed:
                    return colorBlock.pressedColor;
                case ColorBlockType.Selected:
                    return colorBlock.selectedColor;
                case ColorBlockType.Disabled:
                    return colorBlock.disabledColor;
            }
            return colorBlock.normalColor;
        }

        private void LogDebugInformation()
        {
            if (Networking.LocalPlayer == null) return;
            Log(LogLevel.Vervose, "IsVR:" + Networking.LocalPlayer.IsUserInVR() +
                ", InputMethod: " + InputManager.GetLastUsedInputMethod() +
                ", Avatar Height: " + Networking.LocalPlayer.GetAvatarEyeHeightAsMeters());
        }

        private bool _wasHandInteractionEnabled = false;
        private void Init()
        {
            if (!_isInitialized)
            {
                Log(LogLevel.Log, " INITILITING...");
                _targetPositions = new Vector3[Canvas.childCount];
                _targetScale = new Vector3[Canvas.childCount];
                for (int i = 0; i < Canvas.childCount; i++)
                {
                    Transform child = Canvas.GetChild(i);
                    _targetPositions[i] = child.localPosition;
                    _targetScale[i] = child.localScale;
                    child.localScale = Vector3.zero;
                    child.localPosition = startPosition;
                }
                _isInitialized = true;
            }
            //Checking for changed vr value every time the menu opens, because vrchat only sets the value to true for vr users awhile after joining and Init() was called the first time
            bool enableHandInteraction = _simulateVR || 
                (!_forceLaserInteraction &&
                 Networking.LocalPlayer != null && 
                 Networking.LocalPlayer.IsUserInVR() && 
                 Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head) != Vector3.zero); // Check that VR user is also humanoid
            if (_wasHandInteractionEnabled != enableHandInteraction)
            {
                ToggleBoxColliders(transform, enableHandInteraction);
                _wasHandInteractionEnabled = enableHandInteraction;
            }
        }

        private void ToggleBoxColliders(Transform parent, bool on)
        {
            Log(LogLevel.Vervose, " Turn " + (on ? "on" : "off") + " box colliders for: " + parent.name);
            foreach (BoxCollider collider in parent.GetComponentsInChildren<BoxCollider>(true))
            {
                collider.enabled = on;
            }
            raycastCollider.enabled = !on;
        }

        float RELATIVE_MENU_POSITION = 0.07f;
        float RELATIVE_MENU_POSITION_DESKTOP = 0.3f;

        //open menu
        //only if menu is completly closed
        public void OpenMenu(HandType handType, bool forceOpenOnHead, bool forcePointerInteraction)
        {
            if (this.gameObject.activeSelf == false)
            {
                _forceLaserInteraction = forcePointerInteraction;
                SetMenuPosition(handType, forceOpenOnHead);
                _animationDirection = 1;
                _animationStartTime = Time.time;
                this.gameObject.SetActive(true);
                OpenSound.Play();
                //enable inworld indicators
                foreach (GameObject o in inWorldIndicators) o.SetActive(true);
            }
        }

        private void SetMenuPosition(HandType handType, bool forceOpenOnHead)
        {
            float size = Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();
            this.gameObject.transform.localScale = new Vector3(size, size, size);
            VRCPlayerApi.TrackingData head = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
            VRCPlayerApi.TrackingData hand;
            if (handType == HandType.RIGHT)
                hand = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand);
            else
                hand = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand);
            if ((_simulateVR || Networking.LocalPlayer.IsUserInVR()) && !forceOpenOnHead)
            {
                Vector3 relativePosition = (head.rotation * Vector3.forward).normalized * RELATIVE_MENU_POSITION * size;
                relativePosition.y += MENU_HEIGHT / 2 * size;
                this.transform.position = (hand.position) + relativePosition;
            }
            else
            {
                Vector3 relativePosition = (head.rotation * Vector3.forward).normalized * RELATIVE_MENU_POSITION_DESKTOP * size;
                this.transform.position = head.position + relativePosition;
            }
            this.transform.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0);
        }

        public void CloseMenu()
        {
            if (_isFinished && this.gameObject.activeSelf)
            {
                _animationDirection = -1;
                _animationStartTime = Time.time;
                _isFinished = false;
                //disable inworld indicators
                foreach (GameObject o in inWorldIndicators) o.SetActive(false);
            }
        }

        //startPosition - targetPos.y
        //min startPositon
        //max targetPos

        private void Update()
        {
            if (!_isFinished)
            {
                Animate();
            }
            if (Networking.LocalPlayer != null)
            {
                if (VRC.SDKBase.Networking.LocalPlayer.GetVelocity().magnitude > CLOSE_VELOCITY)
                {
                    CloseMenu();
                }
            }
        }

        private void Animate()
        {
            if (RescaleObjects())
            {
                _isFinished = true;
            }
            if (_isFinished && _animationDirection == -1)
            {
                MainToggleGroup.SetAllTogglesOff(false);
                this.gameObject.SetActive(false);
            }
            if(_isFinished && _animationDirection == 1)
            {
                if(HomeToggle)
                {
                    if(_clickSound) _clickSound.SupressNextFeedback();
                    HomeToggle.isOn = true;
                }
            }
        }

        private bool RescaleObjects()
        {
            float scale = (Time.time - _animationStartTime) / ANIMATION_DURATION;
            if (_animationDirection == -1)
                scale = 1 - scale;
            bool allDone = true;
            for (int i = 0; i < Canvas.childCount; i++)
            {
                float localScale = (scale - (_targetPositions[i].y) * ANIMATION_Y_MODIFIER - ANIMATION_Y_OFFSET);
                localScale = Mathf.Clamp01(localScale);
                if ((localScale < 1 && _animationDirection == 1) || (localScale > 0 && _animationDirection == -1))
                    allDone = false;
                Transform child = Canvas.GetChild(i);
                child.localScale = _targetScale[i] * localScale;
                child.localPosition = (startPosition + (_targetPositions[i] - startPosition) * localScale);
            }
            return allDone;
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP 

     [CustomEditor(typeof(SwipeMenuManager), true, isFallback = true)]
        public class SwipeMenuManagerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                SwipeMenuManager menu = (SwipeMenuManager)target;
                if (GUILayout.Button("Apply Theme Colors"))
                {
                    menu._appliedSelectableColors1 = menu._selectableColors1;
                    menu._appliedSelectableColors2 = menu._selectableColors2;
                    menu.ApplyThemeColors();
                }
            }
        }
#endif
    }
}
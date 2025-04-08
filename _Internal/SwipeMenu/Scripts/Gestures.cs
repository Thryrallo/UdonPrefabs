
using Thry.Udon.PrivateRoom;
using UdonSharp;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common;
using static VRC.SDKBase.VRCPlayerApi;

namespace Thry.Udon.SwipeMenu
{
    public class Gestures : UdonSharpBehaviour
    {
        [Tooltip("SAO Menu script reference")]
        public SwipeMenuManager menu;

        [Header("Optional")]
        [Tooltip("Debug Text Object. Used for development.")]
        public GameObject DebugGameObject;
        public UnityEngine.UI.Text DebugText;
        public bool DoDebuggingLeft;
        public bool DoDebuggingRight;

        public float MAX_ANGLE = 30f;

        public float REQUIRED_OPENING_DISTANCE = 0.2f;
        public float REQUIRED_CLOSING_DISTANCE = 0.2f;
        public KeyCode MenuKeyCode = KeyCode.E;

        public float FallbackControllerInputRequiredTime = 1;

        const float SLOW_UPDATE_RATE = 0.5f;
        private float _last_slow_update = 0;

        private const string OCULUS_INDEX_TRIGGER_R = "Oculus_CrossPlatform_SecondaryIndexTrigger";
        private const string OCULUS_INDEX_TRIGGER_L = "Oculus_CrossPlatform_PrimaryIndexTrigger";
        private const string OCULUS_HAND_TRIGGER_R = "Oculus_CrossPlatform_SecondaryHandTrigger";
        private const string OCULUS_HAND_TRIGGER_L = "Oculus_CrossPlatform_PrimaryHandTrigger";
        private const string OCULUS_THUMBSTICK_VERTICAL_R = "Oculus_CrossPlatform_SecondaryThumbstickVertical";
        private const string OCULUS_THUMBSTICK_VERTICAL_L = "Oculus_CrossPlatform_PrimaryThumbstickVertical";
        private const string OCULUS_THUMBSTICK_HORIZONTAL_R = "Oculus_CrossPlatform_SecondaryThumbstickHorizontal";
        private const string OCULUS_THUMBSTICK_HORIZONTAL_L = "Oculus_CrossPlatform_PrimaryThumbstickHorizontal";

        const float GESTURE_REQUIRED_BUTTON_FORCE = 0.9f;
        const float GESTURE_INDEX_REQUIRED_BUTTON_FORCE = 0.7f;
        const float GESTURE_NO_PRESS_MAX_BUTTON_FORCE = 0.9f;
        const float GESTURE_NO_PRESS_MIN_BUTTON_FORCE = 0.2f;
        const float GESTURE_VIVE_Y_MIN = 0.7f;

        const int GESTURE_FINGER_POINT = 0;
        const int GESTURE_HAND_OPEN = 1;

        private Vector3 _rightIndexFingerPosition;
        private Vector3 _leftIndexFingerPosition;
        private VRCPlayerApi.TrackingData _rightHand;
        private VRCPlayerApi.TrackingData _prev_rightHand;
        private VRCPlayerApi.TrackingData _leftHand;
        private VRCPlayerApi.TrackingData _prev_leftHand;
        private VRCPlayerApi.TrackingData _head;

        private int _leftHandGesture = -1;
        private int _rightHandGesture = -1;
        private bool _isPlayerMoving = false;

        private Vector3 _leftMovement = Vector3.zero;
        private Vector3 _rightMovement = Vector3.zero;

        private bool _isLookUpInput;
        private bool _isLookDownInput;
        private float _lookInputStart;
        
        private Collider[] _vrcUIColliders = new Collider[20];

        private void Update()
        {
            UpdateGestureControl();

            if (Time.time - _last_slow_update > SLOW_UPDATE_RATE)
            {
                _last_slow_update = Time.time;
                SlowUpdate();
            }
        }

        private void SlowUpdate()
        {
        }

        public void UpdateDebugging()
        {
            if(DebugGameObject && DebugText)
            {
                DebugGameObject.SetActive(DoDebuggingLeft || DoDebuggingRight);
            }
        }

        private void UpdateGestureControl()
        {
            if (VRC.SDKBase.Networking.LocalPlayer == null)
                return;
            if (VRC.SDKBase.Networking.LocalPlayer.IsUserInVR())
            {
                PopulateFields();
                if (_isPlayerMoving == false)
                {
                    if(DidSwipeWithGesture(HandType.LEFT, Vector3.down, GESTURE_FINGER_POINT, true, 0, REQUIRED_OPENING_DISTANCE))
                        menu.OpenMenu(HandType.LEFT, false, false);
                    if (DidSwipeWithGesture(HandType.RIGHT, Vector3.down, GESTURE_FINGER_POINT, true, 1, REQUIRED_OPENING_DISTANCE))
                        menu.OpenMenu(HandType.RIGHT, false, false);
                    if (DidSwipeWithGestureAnyHand(Vector3.up, GESTURE_HAND_OPEN, false, 2, REQUIRED_CLOSING_DISTANCE))
                        menu.CloseMenu();

                    if (!VRCLib.IsVRCMenuOpen(_vrcUIColliders))
                    {
                        if (_isLookDownInput && Time.time - _lookInputStart > FallbackControllerInputRequiredTime)
                            menu.OpenMenu(HandType.RIGHT, true, true);
                        if (_isLookUpInput && Time.time - _lookInputStart > FallbackControllerInputRequiredTime)
                            menu.CloseMenu();
                    }
                }
                else
                {
                    menu.CloseMenu();
                }
                _prev_rightHand = _rightHand;
                _prev_leftHand = _leftHand;
            }
            else
            {
                if (Input.GetKeyDown(MenuKeyCode) && !VRCLib.IsVRCMenuOpen(_vrcUIColliders))
                {
                    menu.OpenMenu(HandType.LEFT, false, false);
                    menu.CloseMenu();
                }
            }
        }

        public override void InputLookVertical(float value, UdonInputEventArgs args)
        {
            _isLookUpInput = value > 0.6f;
            _isLookDownInput = value < -0.6f;
            if(!_isLookUpInput && !_isLookDownInput)
                _lookInputStart = Time.time;
        }

        private TrackingData GetTrackingData(HandType hand)
        {
            if(hand == HandType.LEFT)
                return Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.LeftHand);
            else
                return Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.RightHand);
        }

        private VRC_Pickup GetPickupInHand(HandType hand)
        {
            if(hand == HandType.LEFT)
                return Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Left);
            else
                return Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Right);
        }

        private void PopulateFields()
        {
            _rightHand = GetTrackingData(HandType.RIGHT);
            _leftHand = GetTrackingData(HandType.LEFT);
            _head = Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.Head);
            _rightIndexFingerPosition = GetMostPreciseRightIndexFingerPosition();
            _leftIndexFingerPosition = GetMostPreciseLeftIndexFingerPosition();
            _isPlayerMoving = IsPlayerMoving();

            _leftHandGesture = GetHandGesture(HandType.LEFT);
            _rightHandGesture = GetHandGesture(HandType.RIGHT);

            _leftMovement = _leftHand.position - _prev_leftHand.position;
            _rightMovement = _rightHand.position - _prev_rightHand.position;
        }

        private bool IsPlayerMoving()
        {
            return Networking.LocalPlayer.GetVelocity().magnitude > 0;
        }
        
        /**
         * <param name="direction">Direction of the swipe to check</param>
         * <param name="gesture">Int related to gesture to check for</param>
         * <param name="gestureDefault">Gesture default value in case gesture could not be determined. If true swipe will be executed if gesture cannot be determined.</param>
         * */
        private bool DidSwipeWithGestureAnyHand(Vector3 direction, int gesture, bool gestureDefault, int cacheIndex, float requiredDistance)
        {
            return (DidSwipeWithGesture(HandType.LEFT, direction, gesture, gestureDefault, cacheIndex, requiredDistance) ||
                DidSwipeWithGesture(HandType.RIGHT, direction, gesture, gestureDefault, cacheIndex+1, requiredDistance));
        }

        private float[] movementCache = new float[20];
        /**
         * <param name="hand">Hand to check for. possible values are HAND_LEFT, HAND_RIGHT</param>
         * <param name="targetDirection">Direction of the swipe to check</param>
         * <param name="targetGesture">Int related to gesture to check for</param>
         * <param name="gestureDefault">Gesture default value in case gesture could not be determined. If true swipe will be executed if gesture cannot be determined.</param>
         * */
        private bool DidSwipeWithGesture(HandType hand, Vector3 targetDirection, int targetGesture, bool gestureDefault, int cacheIndex, float requiredDistance)
        {
            Vector3 realMovement = hand == HandType.RIGHT ? _rightMovement : _leftMovement;
            int realGesture = hand == HandType.RIGHT ? _rightHandGesture : _leftHandGesture;
            bool isHoldingPickup = GetPickupInHand(hand) != null;
            float angleTargetDirectionMovement = Vector3.Angle(targetDirection, realMovement);

            if (angleTargetDirectionMovement < MAX_ANGLE && realGesture == targetGesture && !isHoldingPickup)
            {
                movementCache[cacheIndex] += realMovement.magnitude;
                if (movementCache[cacheIndex] > requiredDistance * Networking.LocalPlayer.GetAvatarEyeHeightAsMeters())
                {
                    movementCache[cacheIndex] = 0;
                    return true;
                }
            }
            else
            {
                movementCache[cacheIndex] = 0;
            }
            return false;
        }

        private int GetHandGesture(HandType hand)
        {
            VRCInputMethod inputMethod = InputManager.GetLastUsedInputMethod();
            if (inputMethod == VRCInputMethod.Oculus) return GetHandGestureOculus(hand);
            else if (inputMethod == VRCInputMethod.Vive) return GetHandGestureVive(hand);
            return GetHandGestureIndex(hand);
        }

        private int GetHandGestureOculus(HandType hand)
        {
            float handTrigger;
            float indexTrigger;
            if (hand == HandType.LEFT)
            {
                handTrigger = UnityEngine.Input.GetAxis(OCULUS_HAND_TRIGGER_L);
                indexTrigger = UnityEngine.Input.GetAxis(OCULUS_INDEX_TRIGGER_L);
            }
            else
            {
                handTrigger = UnityEngine.Input.GetAxis(OCULUS_HAND_TRIGGER_R);
                indexTrigger = UnityEngine.Input.GetAxis(OCULUS_INDEX_TRIGGER_R);
            }
            if (handTrigger > GESTURE_REQUIRED_BUTTON_FORCE && indexTrigger < GESTURE_NO_PRESS_MAX_BUTTON_FORCE) return GESTURE_FINGER_POINT;
            if (handTrigger < GESTURE_NO_PRESS_MIN_BUTTON_FORCE && indexTrigger < GESTURE_NO_PRESS_MAX_BUTTON_FORCE) return GESTURE_HAND_OPEN;
            return -1;
        }

        private int GetHandGestureVive(HandType hand)
        {
            float handTrigger;
            float thumbstickVertical;
            float thumbstickHorionzal;
            if (hand == HandType.LEFT)
            {
                handTrigger = UnityEngine.Input.GetAxis(OCULUS_HAND_TRIGGER_L);
                thumbstickVertical = UnityEngine.Input.GetAxis(OCULUS_THUMBSTICK_VERTICAL_L);
                thumbstickHorionzal = UnityEngine.Input.GetAxis(OCULUS_THUMBSTICK_HORIZONTAL_L);
            }
            else
            {
                handTrigger = UnityEngine.Input.GetAxis(OCULUS_HAND_TRIGGER_R);
                thumbstickVertical = UnityEngine.Input.GetAxis(OCULUS_THUMBSTICK_VERTICAL_R);
                thumbstickHorionzal = UnityEngine.Input.GetAxis(OCULUS_THUMBSTICK_HORIZONTAL_R);
            }
            if (thumbstickVertical > GESTURE_VIVE_Y_MIN && thumbstickHorionzal > -0.35 && thumbstickHorionzal < 0.35) return GESTURE_FINGER_POINT;
            if (handTrigger > GESTURE_REQUIRED_BUTTON_FORCE) return GESTURE_HAND_OPEN;
            return -1;
        }

        private bool IsLookingAtHand(VRCPlayerApi.TrackingData hand)
        {
            Quaternion rotation = Quaternion.LookRotation(hand.position - _head.position, Vector3.up);
            Quaternion headrot = Quaternion.LookRotation(_head.rotation * Vector3.forward, Vector3.up);
            Quaternion difference = headrot * Quaternion.Inverse(rotation);
            bool result = (difference.eulerAngles.x > 320 || difference.eulerAngles.x < 40) && (difference.eulerAngles.y > 325 || difference.eulerAngles.y < 35);
            return result;
        }

        private Vector3 GetMostPreciseLeftIndexFingerPosition()
        {
            return GetMostPreciseFingerPosition(_leftHand, HumanBodyBones.LeftIndexDistal, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexProximal);
        }

        private Vector3 GetMostPreciseRightIndexFingerPosition()
        {
            return GetMostPreciseFingerPosition(_rightHand, HumanBodyBones.RightIndexDistal, HumanBodyBones.RightIndexIntermediate, HumanBodyBones.RightIndexProximal);
        }

        private Vector3 GetMostPreciseFingerPosition(VRCPlayerApi.TrackingData trackingData, HumanBodyBones bone1, HumanBodyBones bone2, HumanBodyBones bone3)
        {
            Vector3 position = VRC.SDKBase.Networking.LocalPlayer.GetBonePosition(bone1);
            if (position != Vector3.zero)
                return position;
            else
                position = VRC.SDKBase.Networking.LocalPlayer.GetBonePosition(bone2);
            if (position != Vector3.zero)
                return position;
            else
                position = VRC.SDKBase.Networking.LocalPlayer.GetBonePosition(bone3);
            if (position != Vector3.zero)
                return position;
            return trackingData.position;
        }

        private Vector2 _indexLimits;
        private Vector2 _middleLimits;
        private Vector2 _littleLimits;

        static void FingerLimitsUpdate(ref Vector2 finger, float value)
        {
            if (value < finger.x) finger.x = value;
            if (value > finger.y) finger.y = value;
        }

        static float FingerLimitsEvaluate(ref Vector2 finger, float value)
        {
            return (value - finger.x) / (finger.y - finger.x);
        }

        static float FingerLimitsUpdateAndEvaluate(ref Vector2 finger, float value)
        {
            FingerLimitsUpdate(ref finger, value);
            return FingerLimitsEvaluate(ref finger, value);
        }

        public override void OnAvatarChanged(VRCPlayerApi player)
        {
            if(player == Networking.LocalPlayer)
            {
                _indexLimits = new Vector2(float.MaxValue, float.MinValue);
                _middleLimits = new Vector2(float.MaxValue, float.MinValue);
                _littleLimits = new Vector2(float.MaxValue, float.MinValue);
            }
        }

        static float GetFingerBend(HumanBodyBones hand, HumanBodyBones promoxial, HumanBodyBones intermediate)
        {
            Vector3 proximalPos = Networking.LocalPlayer.GetBonePosition(promoxial);
            Vector3 intermediatePos = Networking.LocalPlayer.GetBonePosition(intermediate);
            Vector3 handPos = Networking.LocalPlayer.GetBonePosition(hand);
            return Vector3.Angle(intermediatePos - proximalPos, proximalPos - handPos);
        }

        private int GetHandGestureIndex(HandType hand)
        {
            float rawBendIndex, rawBendMiddle, rawBendLittle;
            if(hand == HandType.LEFT)
            {
                rawBendIndex = GetFingerBend(HumanBodyBones.LeftHand, HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftIndexIntermediate);
                rawBendMiddle = GetFingerBend(HumanBodyBones.LeftHand, HumanBodyBones.LeftMiddleProximal, HumanBodyBones.LeftMiddleIntermediate);
                rawBendLittle = GetFingerBend(HumanBodyBones.LeftHand, HumanBodyBones.LeftRingProximal, HumanBodyBones.LeftRingIntermediate);
            }
            else
            {
                rawBendIndex = GetFingerBend(HumanBodyBones.RightHand, HumanBodyBones.RightIndexProximal, HumanBodyBones.RightIndexIntermediate);
                rawBendMiddle = GetFingerBend(HumanBodyBones.RightHand, HumanBodyBones.RightMiddleProximal, HumanBodyBones.RightMiddleIntermediate);
                rawBendLittle = GetFingerBend(HumanBodyBones.RightHand, HumanBodyBones.RightRingProximal, HumanBodyBones.RightRingIntermediate);
            }
            //hand inside +, hand outside is -

            float indexBend = FingerLimitsUpdateAndEvaluate(ref _indexLimits, rawBendIndex);
            float middleBend = FingerLimitsUpdateAndEvaluate(ref _middleLimits, rawBendMiddle);
            float littleBend = FingerLimitsUpdateAndEvaluate(ref _littleLimits, rawBendLittle);

            if (DebugText && (DoDebuggingLeft && hand == HandType.LEFT) || (DoDebuggingRight && hand == HandType.RIGHT))
            {
                string t = "";
                string[] joystickNames = Input.GetJoystickNames();
                t += "Joysticks: ";
                foreach (string j in joystickNames)
                {
                    t += j + ", ";
                }
                t += "\n\n";
                t += "index: " + indexBend + " : " + rawBendIndex + "\n";
                t += "middle: " + middleBend + " : " + rawBendMiddle + "\n";
                t += "little: " + littleBend + " : " + rawBendLittle + "\n";
                DebugText.text = t;
            }

            bool indexStraight = indexBend < 0.5;
            bool middleStraight = middleBend < 0.5;
            bool littleStraight = littleBend < 0.5;

            if (indexStraight && middleStraight && littleStraight) return GESTURE_HAND_OPEN;
            if (indexStraight && !middleStraight && !littleStraight) return GESTURE_FINGER_POINT;

            return -1;
        }
    }
}

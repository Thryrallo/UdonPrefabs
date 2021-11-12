
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SAO
{
    public class Gestures : UdonSharpBehaviour
    {
        [Tooltip("SAO Menu script reference")]
        public Menu menu;
        [HideInInspector]
        public float local_player_height = 1;

        [Header("Optional")]
        [Tooltip("Debug Text Object. Used for development.")]
        public UnityEngine.UI.Text debugText;
        public bool doDebuggingLeft;
        public bool doDebuggingRight;

        const float SLOW_UPDATE_RATE = 0.5f;
        private float last_slow_update = 0;

        private void Update()
        {
            UpdateGestureControl();

            if (Time.time - last_slow_update > SLOW_UPDATE_RATE)
            {
                last_slow_update = Time.time;
                SlowUpdate();
            }
        }

        private void SlowUpdate()
        {
            UpdateLocalPlayerHeight();
        }

        private void UpdateLocalPlayerHeight()
        {
            local_player_height = GetLocalAvatarHeight();
        }
        public float GetAvatarHeight(VRCPlayerApi player)
        {
            float height = 0;
            Vector3 postition1 = player.GetBonePosition(HumanBodyBones.Head);
            Vector3 postition2 = player.GetBonePosition(HumanBodyBones.Neck);
            height += (postition1 - postition2).magnitude;
            postition1 = postition2;
            postition2 = player.GetBonePosition(HumanBodyBones.Hips);
            height += (postition1 - postition2).magnitude;
            postition1 = postition2;
            postition2 = player.GetBonePosition(HumanBodyBones.RightLowerLeg);
            height += (postition1 - postition2).magnitude;
            postition1 = postition2;
            postition2 = player.GetBonePosition(HumanBodyBones.RightFoot);
            height += (postition1 - postition2).magnitude;
            return height;
        }
        private float GetLocalAvatarHeight()
        {
            if (Networking.LocalPlayer == null)
                return 1;
            return GetAvatarHeight(Networking.LocalPlayer);
        }

        //-----------Gesture tracking---------------

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

        const int HAND_LEFT = 0;
        const int HAND_RIGHT = 1;

        public float MAX_ANGLE = 30f;

        public float REQUIRED_OPENING_DISTANCE = 0.2f;
        public float REQUIRED_CLOSING_DISTANCE = 0.2f;

        private Vector3 rightIndexFingerPosition;
        private Vector3 prev_RightIndexFingerPosition;
        private Vector3 start_RightIndexFingerPosition;
        private Vector3 leftIndexFingerPosition;
        private Vector3 prev_LeftIndexFingerPosition;
        private Vector3 start_LeftIndexFingerPosition;
        private VRCPlayerApi.TrackingData rightHand;
        private VRCPlayerApi.TrackingData prev_rightHand;
        private VRCPlayerApi.TrackingData leftHand;
        private VRCPlayerApi.TrackingData prev_leftHand;
        private VRCPlayerApi.TrackingData head;

        [HideInInspector]
        public int leftHandGesture = -1;
        [HideInInspector]
        public int rightHandGesture = -1;

        private float[] movementDistance;
        private bool isPlayerMoving = false;

        private Vector3 leftMovement = Vector3.zero;
        private Vector3 rightMovement = Vector3.zero;

        private void UpdateGestureControl()
        {
            if (VRC.SDKBase.Networking.LocalPlayer == null)
                return;
            if (VRC.SDKBase.Networking.LocalPlayer.IsUserInVR())
            {
                PopulateFields();
                if (isPlayerMoving == false)
                {
                    if(DidSwipeWithGesture(HAND_LEFT, Vector3.down, GESTURE_FINGER_POINT, true, 0, REQUIRED_OPENING_DISTANCE))
                        menu.OpenMenu(false);
                    if (DidSwipeWithGesture(HAND_RIGHT, Vector3.down, GESTURE_FINGER_POINT, true, 1, REQUIRED_OPENING_DISTANCE))
                        menu.OpenMenu(true);
                    if (DidSwipeWithGestureAnyHand(Vector3.up, GESTURE_HAND_OPEN, false, 2, REQUIRED_CLOSING_DISTANCE))
                        menu.CloseMenu();
                    /*if (menu.gameObject.activeInHierarchy && mirror != null)
                    {
                        if (DidSwipeWithGestureAnyHand(Networking.LocalPlayer.GetRotation() * Vector3.left, GESTURE_HAND_OPEN, true, 4, REQUIRED_OPENING_DISTANCE))
                            mirror.OpenLookedAtMirror();
                        if (DidSwipeWithGestureAnyHand(Networking.LocalPlayer.GetRotation() * Vector3.right, GESTURE_HAND_OPEN, true, 6, REQUIRED_CLOSING_DISTANCE))
                            mirror.CloseMirror();
                    }*/
                }
                else
                {
                    menu.CloseMenu();
                }
                prev_RightIndexFingerPosition = rightIndexFingerPosition;
                prev_LeftIndexFingerPosition = leftIndexFingerPosition;
                prev_rightHand = rightHand;
                prev_leftHand = leftHand;
            }
            else
            {
                if (UnityEngine.Input.GetKeyDown("e"))
                {
                    menu.OpenMenu(false);
                    menu.CloseMenu();
                }
            }
        }

        private void PopulateFields()
        {
            rightHand = VRC.SDKBase.Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.RightHand);
            leftHand = VRC.SDKBase.Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.LeftHand);
            head = VRC.SDKBase.Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.Head);
            rightIndexFingerPosition = GetMostPreciseRightIndexFingerPosition();
            leftIndexFingerPosition = GetMostPreciseLeftIndexFingerPosition();
            isPlayerMoving = IsPlayerMoving();

            leftHandGesture = GetHandGesture(HAND_LEFT);
            rightHandGesture = GetHandGesture(HAND_RIGHT);

            leftMovement = leftHand.position - prev_leftHand.position;
            rightMovement = rightHand.position - prev_rightHand.position;
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
            return (DidSwipeWithGesture(HAND_LEFT, direction, gesture, gestureDefault, cacheIndex, requiredDistance) ||
                DidSwipeWithGesture(HAND_RIGHT, direction, gesture, gestureDefault, cacheIndex+1, requiredDistance));
        }

        private float[] movementCache = new float[20];
        /**
         * <param name="hand">Hand to check for. possible values are HAND_LEFT, HAND_RIGHT</param>
         * <param name="direction">Direction of the swipe to check</param>
         * <param name="gesture">Int related to gesture to check for</param>
         * <param name="gestureDefault">Gesture default value in case gesture could not be determined. If true swipe will be executed if gesture cannot be determined.</param>
         * */
        private bool DidSwipeWithGesture(int hand, Vector3 direction, int gesture, bool gestureDefault, int cacheIndex, float requiredDistance)
        {
            Vector3 movement = hand == HAND_RIGHT ? rightMovement : leftMovement;
            float angle = Vector3.Angle(direction, movement);
            //if (angle < MAX_ANGLE && IsDoingGesture(hand, gesture, gestureDefault))
            if (angle < MAX_ANGLE && (hand == HAND_RIGHT ? rightHandGesture : leftHandGesture) == gesture
                && Networking.LocalPlayer.GetPickupInHand(hand == HAND_RIGHT ? VRC_Pickup.PickupHand.Right : VRC_Pickup.PickupHand.Left) == null)
            {
                movementCache[cacheIndex] += movement.magnitude;
                if (movementCache[cacheIndex] > requiredDistance * local_player_height)
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

        private int GetHandGesture(int hand)
        {
            VRCInputMethod inputMethod = VRC.SDKBase.InputManager.GetLastUsedInputMethod();
            if (inputMethod == VRCInputMethod.Oculus) return GetHandGestureOculus(hand);
            else if (inputMethod == VRCInputMethod.Vive) return GetHandGestureVive(hand);
            else if ((int)inputMethod == 10) return GetHandGestureIndex(hand);
            return -1;
        }

        private int GetHandGestureOculus(int hand)
        {
            float handTrigger;
            float indexTrigger;
            if (hand == HAND_LEFT)
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

        private int GetHandGestureVive(int hand)
        {
            float handTrigger;
            float thumbstickVertical;
            float thumbstickHorionzal;
            if (hand == HAND_LEFT)
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
            Quaternion rotation = Quaternion.LookRotation(hand.position - head.position, Vector3.up);
            Quaternion headrot = Quaternion.LookRotation(head.rotation * Vector3.forward, Vector3.up);
            Quaternion difference = headrot * Quaternion.Inverse(rotation);
            bool result = (difference.eulerAngles.x > 320 || difference.eulerAngles.x < 40) && (difference.eulerAngles.y > 325 || difference.eulerAngles.y < 35);
            return result;
        }

        private Vector3 GetMostPreciseLeftIndexFingerPosition()
        {
            return GetMostPreciseFingerPosition(leftHand, HumanBodyBones.LeftIndexDistal, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexProximal);
        }

        private Vector3 GetMostPreciseRightIndexFingerPosition()
        {
            return GetMostPreciseFingerPosition(rightHand, HumanBodyBones.RightIndexDistal, HumanBodyBones.RightIndexIntermediate, HumanBodyBones.RightIndexProximal);
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

        public void OnAvatarChange()
        {
            local_player_height = GetLocalAvatarHeight();
            messured_min_index_distance = 0.05f * local_player_height;
            messured_min_middle_distance = 0.05f * local_player_height;
            messured_min_ring_distance = 0.05f * local_player_height;
            messured_min_little_distance = 0.05f * local_player_height;
        }

        //const float MAX_FINGER_DISTANCE = 0.015f;
        const float MAX_FINGER_DISTANCE = 0.005f;
        private float messured_min_index_distance = 0.05f;
        private float messured_min_middle_distance = 0.05f;
        private float messured_min_ring_distance = 0.05f;
        private float messured_min_little_distance = 0.05f;
        private int GetHandGestureIndex(int hand)
        {
            Vector3 indexS;
            Vector3 indexE;
            Vector3 middleE;
            Vector3 ringE;
            Vector3 littleS;
            Vector3 littleE;
            Vector3 handS;
            Plane handPlane;
            if(hand == HAND_LEFT)
            {
                indexS = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftIndexProximal);
                indexE = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftIndexDistal);
                middleE = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftMiddleDistal);
                ringE = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftRingDistal);
                littleS = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftLittleProximal);
                littleE = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftLittleDistal);
                handS = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftHand);
                handPlane = new Plane(indexS, littleS, handS);
            }
            else
            {
                indexS = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightIndexProximal);
                indexE = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightIndexDistal);
                middleE = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightMiddleDistal);
                ringE = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightRingDistal);
                littleS = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightLittleProximal);
                littleE = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightLittleDistal);
                handS = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightHand);
                handPlane = new Plane(littleS, indexS, handS);
            }
            //hand inside +, hand outside is -

            float maxFingerDistance = MAX_FINGER_DISTANCE * local_player_height;

            float indexDistance = handPlane.GetDistanceToPoint(indexE) / local_player_height;
            if (indexDistance < messured_min_index_distance) messured_min_index_distance = indexDistance;
            bool indexStraight = indexDistance < messured_min_index_distance + maxFingerDistance;

            float middleDistance = handPlane.GetDistanceToPoint(middleE) / local_player_height;
            if (middleDistance < messured_min_middle_distance) messured_min_middle_distance = middleDistance;
            bool middleStraight = middleDistance < messured_min_middle_distance + maxFingerDistance;

            float ringDistance = handPlane.GetDistanceToPoint(ringE) / local_player_height;
            if (ringDistance < messured_min_ring_distance) messured_min_ring_distance = ringDistance;
            bool ringStraight = ringDistance < messured_min_ring_distance + maxFingerDistance;

            float littleDistance = handPlane.GetDistanceToPoint(littleE) / local_player_height;
            if (littleDistance < messured_min_little_distance) messured_min_little_distance = littleDistance;
            bool littleStraight = littleDistance < messured_min_little_distance + maxFingerDistance;

            //float maxFingerDistance = MAX_FINGER_DISTANCE * local_player_height;
            /*bool middleStraight = Mathf.Abs(handPlane.GetDistanceToPoint(middleE)) < maxFingerDistance;
            bool ringStraight = Mathf.Abs(handPlane.GetDistanceToPoint(ringE)) < maxFingerDistance;
            bool littleStraight = Mathf.Abs(handPlane.GetDistanceToPoint(littleE)) < maxFingerDistance;*/

            if (doDebuggingLeft && debugText != null && hand == HAND_LEFT)
            {
                string t = "";
                /*t += "index: " + indexStraight + " : " +  Mathf.Abs(handPlane.GetDistanceToPoint(indexE) / local_player_height) + "\n";
                t += "middle: " + middleStraight + " : " +  Mathf.Abs(handPlane.GetDistanceToPoint(middleE) / local_player_height) + "\n";
                t += "ring: " + ringStraight + " : " +  Mathf.Abs(handPlane.GetDistanceToPoint(ringE) / local_player_height) + "\n";
                t += "little: " + littleStraight + " : " +  Mathf.Abs(handPlane.GetDistanceToPoint(littleE) / local_player_height) + "\n";*/
                t += "index: " + indexStraight + " : " + indexDistance + "<"+ (messured_min_index_distance + maxFingerDistance) + "\n";
                t += "middle: " + middleStraight + " : " + middleDistance + "<" + (messured_min_middle_distance + maxFingerDistance) + "\n";
                t += "ring: " + ringStraight + " : " + ringDistance + "<" + (messured_min_ring_distance + maxFingerDistance) + "\n";
                t += "little: " + littleStraight + " : " + littleDistance + "<" + (messured_min_little_distance + maxFingerDistance) + "\n";
                debugText.text = t;
            }
            if (doDebuggingRight && debugText != null && hand == HAND_RIGHT)
            {
                string t = "";
                /*t += "index: " + indexStraight + " : " +  Mathf.Abs(handPlane.GetDistanceToPoint(indexE) / local_player_height) + "\n";
                t += "middle: " + middleStraight + " : " +  Mathf.Abs(handPlane.GetDistanceToPoint(middleE) / local_player_height) + "\n";
                t += "ring: " + ringStraight + " : " +  Mathf.Abs(handPlane.GetDistanceToPoint(ringE) / local_player_height) + "\n";
                t += "little: " + littleStraight + " : " +  Mathf.Abs(handPlane.GetDistanceToPoint(littleE) / local_player_height) + "\n";*/
                t += "index: " + indexStraight + " : " + indexDistance + "<" + (messured_min_index_distance + maxFingerDistance) + "\n";
                t += "middle: " + middleStraight + " : " + middleDistance + "<" + (messured_min_middle_distance + maxFingerDistance) + "\n";
                t += "ring: " + ringStraight + " : " + ringDistance + "<" + (messured_min_ring_distance + maxFingerDistance) + "\n";
                t += "little: " + littleStraight + " : " + littleDistance + "<" + (messured_min_little_distance + maxFingerDistance) + "\n";
                debugText.text = t;
            }

            if (indexStraight && middleStraight && ringStraight && littleStraight) return GESTURE_HAND_OPEN;
            if (indexStraight && !middleStraight && !ringStraight && !littleStraight) return GESTURE_FINGER_POINT;

            return -1;
        }
    }
}
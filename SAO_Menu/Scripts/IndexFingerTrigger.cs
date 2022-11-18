
using Thry.General;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Thry.SAO
{
    public class IndexFingerTrigger : UdonSharpBehaviour
    {
        [Header("Hand Side")]
        public bool isRightHand;

        [Header("Settings")]
        public float scrollSpeed = 4;
        public float scrollThresholdSquare = 0.001f;

        [Header("Resources")]
        public AudioClip onClickSound;

        private Collider[] currentColliders = new Collider[10];
        private float[] currentCollidersEnterTime = new float[10];
        private Vector3[] currentCollidersEnterPosition = new Vector3[10];
        private bool[] currentCollidersBlockPress = new bool[10];
        private bool[] currentCollidersSupportsDragInteraction = new bool[10];

        private bool isScrolling = false;
        private Collider scrollCollider;

        private Vector3 currentPosition;
        private Vector3 lastPosition;

        [HideInInspector]
        public bool _blockInput = false;

        AvatarHeightTracker _avatarHeightTracker;

        bool _isNotVR = true;
        VRCPlayerApi _player;
        AudioSource _audioSource;

        private void Start()
        {
            //Find Height Tracker
            GameObject o = GameObject.Find("[Thry]AvatarHeightTracker");
            if (o == null)
            {
                Debug.LogError("[Thry][FingerTrigger]Can't Find AvatarHeightTracker");
                return;
            }
            _avatarHeightTracker = o.GetComponent<AvatarHeightTracker>();
            _audioSource = this.GetComponent<AudioSource>();

            _player = Networking.LocalPlayer;
            if (_player == null) return;
            _isNotVR = !_player.IsUserInVR();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_isNotVR) return;
            if (other == null)
                return;
            if (other.gameObject.layer != 17) return;
            if (IsObjectWithUI(other.gameObject) == false) return;
            //Debug.Log("[Thry] [Index Trigger] Collision enter: " + other.name);
            float earlierstEnter = float.MaxValue;
            int earliestIndex = 0;
            int foundIndex = -1;
            for (int i = 0; i < currentColliders.Length; i++)
            {
                if (currentColliders[i] == null)
                {
                    foundIndex = i;
                    break;
                }
                if (currentCollidersEnterTime[i] < earlierstEnter)
                {
                    earlierstEnter = currentCollidersEnterTime[i];
                    earliestIndex = i;
                }
            }
            if (foundIndex == -1)
                foundIndex = earliestIndex;
            currentColliders[foundIndex] = other;
            currentCollidersEnterTime[foundIndex] = Time.time;
            currentCollidersEnterPosition[foundIndex] = this.transform.position;
            currentCollidersBlockPress[foundIndex] = isScrolling;
            currentCollidersSupportsDragInteraction[foundIndex] = other.gameObject.GetComponent<ScrollRect>() || other.gameObject.GetComponent<Slider>();
        }

        private bool IsObjectWithUI(GameObject o)
        {
            return o.GetComponent<ScrollRect>() || o.GetComponent<Slider>() || o.GetComponent<Toggle>() || o.GetComponent<UnityEngine.UI.Button>();
        }

        public void OnTriggerExit(Collider other)
        {
            if (_isNotVR) return;
            if (other == null)
                return;
            if (other.gameObject.layer != 17) return;
            if (IsObjectWithUI(other.gameObject) == false) return;
            int index = -1;
            for (int i = 0; i < currentColliders.Length; i++)
                if (currentColliders[i] == other)
                    index = i;
            if (index == -1)
                return;
            //Debug.Log("[Thry] [Index Trigger] Collision exit: " + other.name + " enter time: " + (Time.time - currentCollidersEnterTime[index])+ ", isScrolling: "+ isScrolling+", block: "+ currentCollidersBlockPress[index]);
            if(isScrolling == false && currentCollidersBlockPress[index] == false && (transform.position - currentCollidersEnterPosition[index]).sqrMagnitude < scrollThresholdSquare)
            {
                Press(other);
            }
            if(other == scrollCollider)
            {
                isScrolling = false;
            }
            currentColliders[index] = null;
        }

        UnityEngine.UI.Button clickedButton;
        ColorBlock clickedButtonPrevColorBlock;
        float clickedButtonTime = 0;
        private void Press(Collider other)
        {
            bool isMovingBack = IsBackwardMoving(other);
            if (!_blockInput && isMovingBack)
            {
                //ThryAction action = other.gameObject.GetComponent<ThryAction>();
                Toggle toggle = other.gameObject.GetComponent<Toggle>();
                if (toggle != null)
                {
                    toggle.isOn = !toggle.isOn;
                }
                else
                {
                    UnityEngine.UI.Button button = other.gameObject.GetComponent<UnityEngine.UI.Button>();
                    UdonBehaviour action = (UdonBehaviour)other.gameObject.GetComponent(typeof(UdonBehaviour));
                    if (action != null)
                    {
                        //action.OnInteraction();
                        action.SendCustomEvent("OnInteraction");
                    }
                    if(button != null)
                    {
                        if (clickedButton != null) clickedButton.colors = clickedButtonPrevColorBlock;
                        SetClickedButtonColor(button);
                        if (_audioSource && onClickSound) _audioSource.PlayOneShot(onClickSound);
                    }
                }
            }
        }

        public void SetClickedButtonColor(UnityEngine.UI.Button button)
        {
            clickedButton = button;
            ColorBlock block = button.colors;
            clickedButtonPrevColorBlock = block;
            block.normalColor = block.pressedColor;
            block.selectedColor = block.pressedColor;
            button.colors = block;
            clickedButtonTime = Time.time;
            SendCustomEventDelayedSeconds(nameof(ResetClickedButtonColor), 0.3f);
        }

        public void ResetClickedButtonColor()
        {
            if(clickedButton != null && Time.time - clickedButtonTime > 0.2f)
            {
                clickedButton.colors = clickedButtonPrevColorBlock;
                clickedButton = null;
            }
        }

        private void HandleDrag()
        {
            Vector3 delta = currentPosition - lastPosition;
            for (int i = 0; i < currentColliders.Length; i++)
            {
                if (currentColliders[i] != null && currentCollidersSupportsDragInteraction[i])
                {
                    Drag(currentColliders[i], i, delta, currentCollidersEnterPosition[i] ,Time.time - currentCollidersEnterTime[i]);
                }
            }
        }

        //maybe should rename pressTimeConstraint to dragThreshold
        private void Drag(Collider other, int index, Vector3 delta, Vector3 startPosition ,float timeSinceClick)
        {
            ScrollRect scrollRect = other.GetComponent<ScrollRect>();
            //executes if moved over threshold distance away from enter position
            if (scrollRect && (transform.position - startPosition).sqrMagnitude > scrollThresholdSquare * _avatarHeightTracker.GetHeight())
            {
                //isScrolling is used to block touch inputs
                if (!isScrolling)
                {
                    isScrolling = true;
                    scrollCollider = other;
                    for (int i = 0; i < currentColliders.Length; i++) currentCollidersBlockPress[i] = true;
                }
                scrollRect.verticalNormalizedPosition += -delta.y * scrollSpeed * scrollRect.verticalScrollbar.size / _avatarHeightTracker.GetHeight();
            }
            Slider slider = other.GetComponent<Slider>();
            BoxCollider boxCollider = other.GetComponent<BoxCollider>();
            Transform otherTransform = other.transform;
            if(slider != null && boxCollider != null)
            {
                //move slider
                float sizeX = boxCollider.size.x * other.transform.lossyScale.x;
                Vector3 relativePosition = Quaternion.Inverse(otherTransform.rotation) * (transform.position - otherTransform.position);
                float value = (relativePosition.x + 0.5f * sizeX) / sizeX;
                slider.value = Mathf.Clamp01(value) * (slider.maxValue - slider.minValue) + slider.minValue;
            }
        }

        private bool IsForwardMoving(Collider other)
        {
            Vector3 movementVec = (currentPosition - lastPosition).normalized;
            float dot = Vector3.Dot(movementVec, other.transform.forward);
            return dot > 0.5;
        }

        private bool IsBackwardMoving(Collider other)
        {
            Vector3 movementVec = (currentPosition - lastPosition).normalized;
            float dot = Vector3.Dot(movementVec, other.transform.forward);
            return dot < -0.5;
        }

        public override void PostLateUpdate()
        {
            if (_isNotVR) return;
            HandleDrag();
            if (isRightHand) SetToMostPreciseRightIndexFingerPosition(transform);
            else SetToMostPreciseLeftIndexFingerPosition(transform);
            transform.localScale = Vector3.one * (0.01f * _avatarHeightTracker.GetHeight());
            lastPosition = currentPosition;
            currentPosition = this.transform.position;
        }

        private void SetToMostPreciseLeftIndexFingerPosition(Transform target)
        {
            SetToMostPreciseFingerPosition(target, VRCPlayerApi.TrackingDataType.LeftHand, HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexDistal);
        }

        private void SetToMostPreciseRightIndexFingerPosition(Transform target)
        {
            SetToMostPreciseFingerPosition(target, VRCPlayerApi.TrackingDataType.RightHand, HumanBodyBones.RightIndexProximal, HumanBodyBones.RightIndexIntermediate, HumanBodyBones.RightIndexDistal);
        }

        private void SetToMostPreciseFingerPosition(Transform target, VRCPlayerApi.TrackingDataType trackingDataType, HumanBodyBones bone1, HumanBodyBones bone2, HumanBodyBones bone3)
        {
            Vector3 positionHead = _player.GetBonePosition(bone3);
            Vector3 positionTail = _player.GetBonePosition(bone2);
            if (positionHead != Vector3.zero)
            {
                target.position = positionHead;
                target.rotation = Quaternion.LookRotation(positionHead - positionTail);
                return;
            }
            positionHead = positionTail;
            positionTail = _player.GetBonePosition(bone1);
            if (positionHead != Vector3.zero)
            {
                target.position = positionHead;
                target.rotation = Quaternion.LookRotation(positionHead - positionTail);
                return;
            }
            VRCPlayerApi.TrackingData trackingData = _player.GetTrackingData(trackingDataType);
            positionHead = positionTail;
            positionTail = trackingData.position;
            if (positionHead != Vector3.zero)
            {
                target.position = positionHead;
                target.rotation = Quaternion.LookRotation(positionHead - positionTail);
                return;
            }
            target.position = trackingData.position;
            target.rotation = trackingData.rotation;
        }
    }
}
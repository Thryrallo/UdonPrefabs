
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

        private Collider[] _currentColliders = new Collider[10];
        private float[] _currentCollidersEnterTime = new float[10];
        private Vector3[] _currentCollidersEnterPosition = new Vector3[10];
        private bool[] _currentCollidersBlockPress = new bool[10];
        private bool[] _currentCollidersSupportsDragInteraction = new bool[10];

        private bool _isScrolling = false;
        private Collider _scrollCollider;

        private Vector3 _currentPosition;
        private Vector3 _lastPosition;

        [HideInInspector]
        public bool _blockInput = false;

        bool _isNotVR = true;
        VRCPlayerApi _player;
        AudioSource _audioSource;

        private void Start()
        {
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
            for (int i = 0; i < _currentColliders.Length; i++)
            {
                if (_currentColliders[i] == null)
                {
                    foundIndex = i;
                    break;
                }
                if (_currentCollidersEnterTime[i] < earlierstEnter)
                {
                    earlierstEnter = _currentCollidersEnterTime[i];
                    earliestIndex = i;
                }
            }
            if (foundIndex == -1)
                foundIndex = earliestIndex;
            _currentColliders[foundIndex] = other;
            _currentCollidersEnterTime[foundIndex] = Time.time;
            _currentCollidersEnterPosition[foundIndex] = this.transform.position;
            _currentCollidersBlockPress[foundIndex] = _isScrolling;
            _currentCollidersSupportsDragInteraction[foundIndex] = other.gameObject.GetComponent<ScrollRect>() || other.gameObject.GetComponent<Slider>();
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
            for (int i = 0; i < _currentColliders.Length; i++)
                if (_currentColliders[i] == other)
                    index = i;
            if (index == -1)
                return;
            //Debug.Log("[Thry] [Index Trigger] Collision exit: " + other.name + " enter time: " + (Time.time - currentCollidersEnterTime[index])+ ", isScrolling: "+ isScrolling+", block: "+ currentCollidersBlockPress[index]);
            if(_isScrolling == false && _currentCollidersBlockPress[index] == false && (transform.position - _currentCollidersEnterPosition[index]).sqrMagnitude < scrollThresholdSquare)
            {
                Press(other);
            }
            if(other == _scrollCollider)
            {
                _isScrolling = false;
            }
            _currentColliders[index] = null;
        }

        UnityEngine.UI.Button _clickedButton;
        ColorBlock _clickedButtonPrevColorBlock;
        float _clickedButtonTime = 0;
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
                        if (_clickedButton != null) _clickedButton.colors = _clickedButtonPrevColorBlock;
                        SetClickedButtonColor(button);
                        if (_audioSource && onClickSound) _audioSource.PlayOneShot(onClickSound);
                    }
                }
            }
        }

        public void SetClickedButtonColor(UnityEngine.UI.Button button)
        {
            _clickedButton = button;
            ColorBlock block = button.colors;
            _clickedButtonPrevColorBlock = block;
            block.normalColor = block.pressedColor;
            block.selectedColor = block.pressedColor;
            button.colors = block;
            _clickedButtonTime = Time.time;
            SendCustomEventDelayedSeconds(nameof(ResetClickedButtonColor), 0.3f);
        }

        public void ResetClickedButtonColor()
        {
            if(_clickedButton != null && Time.time - _clickedButtonTime > 0.2f)
            {
                _clickedButton.colors = _clickedButtonPrevColorBlock;
                _clickedButton = null;
            }
        }

        private void HandleDrag()
        {
            Vector3 delta = _currentPosition - _lastPosition;
            for (int i = 0; i < _currentColliders.Length; i++)
            {
                if (_currentColliders[i] != null && _currentCollidersSupportsDragInteraction[i])
                {
                    Drag(_currentColliders[i], i, delta, _currentCollidersEnterPosition[i] ,Time.time - _currentCollidersEnterTime[i]);
                }
            }
        }

        //maybe should rename pressTimeConstraint to dragThreshold
        private void Drag(Collider other, int index, Vector3 delta, Vector3 startPosition ,float timeSinceClick)
        {
            ScrollRect scrollRect = other.GetComponent<ScrollRect>();
            //executes if moved over threshold distance away from enter position
            float height = Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();
            if (scrollRect && (transform.position - startPosition).sqrMagnitude > scrollThresholdSquare * height)
            {
                //isScrolling is used to block touch inputs
                if (!_isScrolling)
                {
                    _isScrolling = true;
                    _scrollCollider = other;
                    for (int i = 0; i < _currentColliders.Length; i++) _currentCollidersBlockPress[i] = true;
                }
                scrollRect.verticalNormalizedPosition += -delta.y * scrollSpeed * scrollRect.verticalScrollbar.size / height;
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
            Vector3 movementVec = (_currentPosition - _lastPosition).normalized;
            float dot = Vector3.Dot(movementVec, other.transform.forward);
            return dot > 0.5;
        }

        private bool IsBackwardMoving(Collider other)
        {
            Vector3 movementVec = (_currentPosition - _lastPosition).normalized;
            float dot = Vector3.Dot(movementVec, other.transform.forward);
            return dot < -0.5;
        }

        public override void PostLateUpdate()
        {
            if (_isNotVR) return;
            HandleDrag();
            if (isRightHand) SetToMostPreciseRightIndexFingerPosition(transform);
            else SetToMostPreciseLeftIndexFingerPosition(transform);
            transform.localScale = Vector3.one * (0.01f * Networking.LocalPlayer.GetAvatarEyeHeightAsMeters());
            _lastPosition = _currentPosition;
            _currentPosition = this.transform.position;
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
            if (positionHead != Vector3.zero && positionHead != positionTail)
            {
                target.position = positionHead;
                target.rotation = Quaternion.LookRotation(positionHead - positionTail);
                return;
            }
            positionHead = positionTail;
            positionTail = _player.GetBonePosition(bone1);
            if (positionHead != Vector3.zero && positionHead != positionTail)
            {
                target.position = positionHead;
                target.rotation = Quaternion.LookRotation(positionHead - positionTail);
                return;
            }
            VRCPlayerApi.TrackingData trackingData = _player.GetTrackingData(trackingDataType);
            positionHead = positionTail;
            positionTail = trackingData.position;
            if (positionHead != Vector3.zero && positionHead != positionTail)
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
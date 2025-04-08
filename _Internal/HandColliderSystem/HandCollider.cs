
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.UI
{
    public abstract class HandCollider : UdonSharpBehaviour
    {
        public abstract bool IsRightHand { get; }
        HumanBodyBones finger;
        HumanBodyBones hand;
        VRCPlayerApi.TrackingDataType trackingDataType;

        [HideInInspector]
        public Vector3 _velocity;

        GameObject[] clapCallbacks = new GameObject[0];

        public bool DEBUG = false;

        const float MIN_CLAP_VELOCITY = 0.4f;
        const float COLLIDER_SCALING_UPDATE_RATE = 5;

        VRCPlayerApi _player;

        void Start()
        {
            _player = Networking.LocalPlayer;
            if (IsRightHand)
            {
                hand = HumanBodyBones.RightHand;
                finger = HumanBodyBones.RightMiddleProximal;
                trackingDataType = VRCPlayerApi.TrackingDataType.RightHand;
            }
            else
            {
                hand = HumanBodyBones.LeftHand;
                finger = HumanBodyBones.LeftMiddleProximal;
                trackingDataType = VRCPlayerApi.TrackingDataType.LeftHand;
            }
            Scale();
        }

        public void Scale()
        {
            if (!Utilities.IsValid(Networking.LocalPlayer)) return;
            transform.localScale = Vector3.one * Networking.LocalPlayer.GetAvatarEyeHeightAsMeters() * 0.11f;
            SendCustomEventDelayedSeconds(nameof(Scale), COLLIDER_SCALING_UPDATE_RATE);
        }

        public void RegisterClapCallback(GameObject u)
        {
            Collections.Add(ref clapCallbacks, u);
        }

        public override void PostLateUpdate()
        {
            if (!Utilities.IsValid(_player)) return;
            //Roation
            transform.rotation = _player.GetTrackingData(trackingDataType).rotation;
            transform.Rotate(0, 55, 0);
            //Position
            Vector3 handPosition = _player.GetBonePosition(finger);
            if (handPosition == Vector3.zero) handPosition = _player.GetBonePosition(hand);
            if (handPosition == Vector3.zero) handPosition = _player.GetTrackingData(trackingDataType).position;
            //Velcoity
            _velocity = (transform.position - handPosition) / Time.deltaTime;
            //Set position
            transform.position = handPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsRightHand)
            {
                if (other != null && Utilities.IsValid(other.gameObject))
                {
                    if (other.gameObject.GetComponent<HandCollider>())
                    {
                        _HandCollision(other.gameObject.GetComponent<HandCollider>());
                    }
                }
            }
        }

        private void _HandCollision(HandCollider _left)
        {
            if (Networking.LocalPlayer.IsUserInVR() == false) return;
            if (Networking.LocalPlayer.GetVelocity().sqrMagnitude > 0) return;
            
            //Velocity actually going in the direction of hand palms
            if (Vector3.Angle(_left.transform.rotation * Vector3.down, _left._velocity) < 45 && Vector3.Angle(this.transform.rotation * Vector3.up, this._velocity) < 45)
            {
                //Multiply speeds depending on how much the vectors are going towards eachother
                float velocityAngleMultiplier = Mathf.Abs(-Vector3.Dot(_left._velocity.normalized, this._velocity.normalized));
                float finalMultiplier = velocityAngleMultiplier / Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();

                if (DEBUG) Debug.Log($"[Clapper] Hand Collision. Left: {finalMultiplier * _left._velocity.magnitude} Right: {finalMultiplier * this._velocity.magnitude} angleMultiplier: {velocityAngleMultiplier} avatarHeight: {Networking.LocalPlayer.GetAvatarEyeHeightAsMeters()}");
                if (finalMultiplier * _left._velocity.magnitude > MIN_CLAP_VELOCITY && finalMultiplier * this._velocity.magnitude > MIN_CLAP_VELOCITY)
                {
                    foreach(GameObject o in clapCallbacks)
                    {
                        ((UdonBehaviour)o.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("_Clap");
                    }
                }
            }
        }
    }
}

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.BeerPong
{
    public class ThryBP_HandCollider : UdonSharpBehaviour
    {
        public bool isRight;
        Vector3 lastPosition;

        VRCPlayerApi.TrackingDataType type;
        private void Start()
        {
            type = isRight ? VRCPlayerApi.TrackingDataType.RightHand : VRCPlayerApi.TrackingDataType.LeftHand;
        }

        private void Update()
        {
            lastPosition = transform.position;
            VRCPlayerApi.TrackingData d = Networking.LocalPlayer.GetTrackingData(type);
            transform.position = d.position;
            transform.rotation = d.rotation;
        }

        float GetVelocity()
        {
            return Vector3.Distance(lastPosition, transform.position) / Time.deltaTime;
        }

        public Vector3 GetSlapVector()
        {
            //float v = GetVelocity();
            //Vector3 vec = transform.rotation * Vector3.right * v;
            return (lastPosition - transform.position) / Time.deltaTime;
        }
    }
}
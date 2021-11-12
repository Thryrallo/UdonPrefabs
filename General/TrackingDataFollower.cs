
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.General
{
    public class TrackingDataFollower : UdonSharpBehaviour
    {
        public VRCPlayerApi.TrackingDataType headType = VRCPlayerApi.TrackingDataType.Head;

        VRCPlayerApi _player;
        VRCPlayerApi.TrackingData trackingData;

        private void Start()
        {
            _player = Networking.LocalPlayer;
        }

        private void Update()
        {
            trackingData = _player.GetTrackingData(headType);
            transform.SetPositionAndRotation(trackingData.position, trackingData.rotation);
        }
    }
}
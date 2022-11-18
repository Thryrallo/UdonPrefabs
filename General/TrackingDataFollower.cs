
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.General
{
    public class TrackingDataFollower : UdonSharpBehaviour
    {
        public VRCPlayerApi.TrackingDataType headType = VRCPlayerApi.TrackingDataType.Head;

        bool _isInit;
        VRCPlayerApi _player;
        VRCPlayerApi.TrackingData trackingData;

        private void Start()
        {
            _player = Networking.LocalPlayer;
            _isInit = true;
        }

        private void Update()
        {
            if (_isInit)
            {
                trackingData = _player.GetTrackingData(headType);
                transform.SetPositionAndRotation(trackingData.position, trackingData.rotation);
            }
        }
    }
}
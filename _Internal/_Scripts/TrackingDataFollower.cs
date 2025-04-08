
using UdonSharp;
using VRC.SDKBase;

namespace Thry.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TrackingDataFollower : UdonSharpBehaviour
    {
        public VRCPlayerApi.TrackingDataType headType = VRCPlayerApi.TrackingDataType.Head;

        bool _isInit;
        VRCPlayerApi _player;
        VRCPlayerApi.TrackingData trackingData;

        private void Start()
        {
            _player = Networking.LocalPlayer;
            _isInit = Networking.LocalPlayer != null;
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
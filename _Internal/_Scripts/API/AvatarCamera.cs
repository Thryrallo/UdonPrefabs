
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.API
{
    public partial class Avatar
    {
        public static void TakeHeadshotPicture(VRCPlayerApi player, Camera camera)
        {
            if(camera == null || player == null) return;
            VRCPlayerApi.TrackingData trackingData = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
            Vector3 headPosition = player.GetBonePosition(HumanBodyBones.Head);
            float pictureSize = 0.5f;
            if (headPosition == Vector3.zero)
            {
                // Fallback for non-humanoid avatars
                headPosition = (trackingData.position + player.GetPosition()) / 2f;
                pictureSize = 0.75f;
            }
            Vector3 cameraPosition = headPosition + trackingData.rotation * Vector3.forward * player.GetAvatarEyeHeightAsMeters() * 0.4f;
            camera.transform.SetPositionAndRotation(cameraPosition, Quaternion.LookRotation(headPosition - cameraPosition));
            camera.orthographicSize = pictureSize / 2 * player.GetAvatarEyeHeightAsMeters();
            camera.cullingMask = player.isLocal ? 1 << 18 : 1 << 9;
            camera.Render();
        }
    }
}
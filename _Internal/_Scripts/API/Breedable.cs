using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.API
{
    public partial class Avatar
    {
        public static string GetBreedabilityLabel(float score)
        {
            if (score < 20) return "Not Breedable";
            else if (score < 50) return "Maybe Breedable";
            else if (score < 65) return "Kinda Breedable";
            else if (score < 85) return "Super Breedable";
            else if (score < 100) return "Mega Breedable";
            else if (score < 250) return "Hyper Breedable";
            else return "Omega Breedable";
        }

        public static float GetBreedabilityScore(VRCPlayerApi player)
        {
            if (player.GetBonePosition(HumanBodyBones.Hips) == Vector3.zero) return 0;
            float height = player.GetAvatarEyeHeightAsMeters();
            float hips = GetHipsSize(player);
            //normalize
            hips = hips / height * 8;

            //evalutate
            float heightscore = BreedableTransformHeight(height);
            float s = hips * heightscore * 120;

            //score is usually between 70 and 130 so this scales it
            float score = (int)Mathf.Max(0, ((s - 70) / 60) * 100);
            Logger.Log("Thry.Udon.API.Avatar", $"GetBreedabilityScore({player.displayName}) {{height: {height}, hips: {hips}}} => {score}");
            return score;
        }

        private static float BreedableTransformHeight(float h)
        {
            if(h < 0.8)
            {
                return Mathf.Pow(h / 0.8f,2);
            }
            else if(h < 1.6)
            {
                return 1;
            }
            else
            {
                return -0.05f * h + 1.1f;
            }
        }

        private static float GetHipsSize(VRCPlayerApi player)
        {
            return Vector3.Distance(player.GetBonePosition(HumanBodyBones.LeftUpperLeg), player.GetBonePosition(HumanBodyBones.RightUpperLeg));
        }

        private static float GetRelativeHipsSize(VRCPlayerApi player)
        {
            return GetHipsSize(player) / player.GetAvatarEyeHeightAsMeters();
        }
        
        private static float GetTorsoLength(VRCPlayerApi player)
        {
            return Vector3.Distance(player.GetBonePosition(HumanBodyBones.Hips), player.GetBonePosition(HumanBodyBones.Head));
        }

        private static float GetLegLength(VRCPlayerApi player)
        {
            return Vector3.Distance(player.GetBonePosition(HumanBodyBones.Hips), player.GetBonePosition(HumanBodyBones.LeftUpperLeg))
                 + Vector3.Distance(player.GetBonePosition(HumanBodyBones.LeftUpperLeg), player.GetBonePosition(HumanBodyBones.LeftFoot));
        }

        private static float GetUpperBodyLowerBodyRatio(VRCPlayerApi player)
        {
            return GetTorsoLength(player) / GetLegLength(player);
        }
    }
}
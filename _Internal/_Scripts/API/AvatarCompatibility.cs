
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.API
{
    public partial class Avatar
    {
        public static float GetCompatibility(VRCPlayerApi player1, VRCPlayerApi player2)
        {
            float height1 = player1.GetAvatarEyeHeightAsMeters();
            float height2 = player2.GetAvatarEyeHeightAsMeters();
            float hips1 = GetRelativeHipsSize(player1);
            float hips2 = GetRelativeHipsSize(player2);
            float upperLowerRatio1 = GetUpperBodyLowerBodyRatio(player1);
            float upperLowerRatio2 = GetUpperBodyLowerBodyRatio(player2);
            Vector2 heightCompatibility = CompatibilityHelper.GetCompatibilityBetweenNumbers(height1, height2, 0.5f, 3.0f, 3);
            Vector2 hipsCompatibility = CompatibilityHelper.GetCompatibilityBetweenNumbers(hips1, hips2, 0.05f, 0.3f, 3);
            Vector2 upperLowerRatioCompatibility = CompatibilityHelper.GetCompatibilityBetweenNumbers(upperLowerRatio1, upperLowerRatio2, 0.4f, 1.2f, 3);
            float similarity = hipsCompatibility.x * upperLowerRatioCompatibility.x;
            similarity = (similarity + heightCompatibility.x + hipsCompatibility.x + upperLowerRatioCompatibility.x) / 4f;
            float opposite = hipsCompatibility.y * upperLowerRatioCompatibility.y;
            opposite = (opposite + heightCompatibility.y + hipsCompatibility.y + upperLowerRatioCompatibility.y) / 4f;
            Logger.Log("Thry.Udon.API.Avatar.GetCompatibility", $"HeightCompatibility({height1}, {height2}) => {heightCompatibility}");
            Logger.Log("Thry.Udon.API.Avatar.GetCompatibility", $"HipsCompatibility({hips1}, {hips2}) => {hipsCompatibility}");
            Logger.Log("Thry.Udon.API.Avatar.GetCompatibility", $"UpperLowerRatioCompatibility({upperLowerRatio1}, {upperLowerRatio2}) => {upperLowerRatioCompatibility}");
            Logger.Log("Thry.Udon.API.Avatar", $"GetCompatibility({player1.displayName}, {player2.displayName}) => {similarity}, {opposite}");
            return Mathf.Max(similarity, opposite) * 100;
        }
    }
}
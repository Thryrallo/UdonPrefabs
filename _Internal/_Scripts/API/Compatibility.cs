
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.API
{
    class CompatibilityHelper
    {
        public static Vector2 GetCompatibilityBetweenNumbers(float score1, float score2, float min, float max, float power)
        {
            float range = max - min;
            score1 = Mathf.Clamp01((score1 - min) / range);
            score2 = Mathf.Clamp01((score2 - min) / range);
            // Compatibility is high if the scores are close to each other, opposite of each other
            float similar = Mathf.Pow(1 - Mathf.Abs(score1 - score2), power);
            float opposite = Mathf.Pow(Mathf.Clamp01(Mathf.Abs(score1 - score2) + 0.2f), power);
            return new Vector2(similar, opposite);
        }
    }
}
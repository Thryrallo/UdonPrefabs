using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.API
{
    enum HeightRange
    {
        Tiny,
        Short,
        Normal,
        Tall
    }
    enum BreedabilityRange
    {
        None, Some, Very, Extreme
    }

    public partial class Avatar
    {
        public static string GetPersonality(VRCPlayerApi player)
        {
            int inputtype = (int)InputManager.GetLastUsedInputMethod();

            HeightRange height = GetHeightRange(player.GetAvatarEyeHeightAsMeters());
            BreedabilityRange breedability = GetBreedabilityRange(GetBreedabilityScore(player));
            bool isVR = inputtype > 4 && inputtype < 15;
            // lewd, cute, shy, dominant
            switch(height)
            {
                case HeightRange.Tiny:
                    return isVR ? "cute" : "adorable";
                case HeightRange.Short:
                    switch(breedability)
                    {
                        case BreedabilityRange.None:
                            return isVR ? "shy" : "timid";
                        case BreedabilityRange.Some:
                            return isVR ? "playful" : "fun";
                        case BreedabilityRange.Very:
                            return isVR ? "cheeky" : "mischievous";
                        case BreedabilityRange.Extreme:
                            return isVR ? "bratty" : "naughty";
                    }
                    break;
                case HeightRange.Normal:
                    switch(breedability)
                    {
                        case BreedabilityRange.None:
                            return isVR ? "shy" : "timid";
                        case BreedabilityRange.Some:
                            return isVR ? "friendly" : "kind";
                        case BreedabilityRange.Very:
                            return isVR ? "sexy" : "hot";
                        case BreedabilityRange.Extreme:
                            return isVR ? "slutty" : "lewd";
                    }
                    break;
                case HeightRange.Tall:
                    switch(breedability)
                    {
                        case BreedabilityRange.None:
                            return isVR ? "shy" : "timid";
                        case BreedabilityRange.Some:
                            return isVR ? "cool" : "chill";
                        case BreedabilityRange.Very:
                            return isVR ? "dominant" : "strong";
                        case BreedabilityRange.Extreme:
                            return isVR ? "slutty" : "lewd";
                    }
                    break;
            }
            return "normal";
        }

        private static HeightRange GetHeightRange(float height)
        {
            if (height < 0.5) return HeightRange.Tiny;
            else if (height < 1.2) return HeightRange.Short;
            else if (height < 1.8) return HeightRange.Normal;
            else return HeightRange.Tall;
        }

        private static BreedabilityRange GetBreedabilityRange(float score)
        {
            if (score < 45) return BreedabilityRange.None;
            else if (score < 70) return BreedabilityRange.Some;
            else if (score < 100) return BreedabilityRange.Very;
            else return BreedabilityRange.Extreme;
        }
    }
}
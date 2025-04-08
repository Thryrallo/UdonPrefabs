
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.API
{
    public partial class Player
    {
        public static float GetCompatibility(VRCPlayerApi player1, VRCPlayerApi player2)
        {
            int name1 = player1.displayName.GetHashCode();
            int name2 = player2.displayName.GetHashCode();
            Random.InitState(name1 + name2);
            return Random.value * 100;
        }
    }
}

using Thry.CustomAttributes;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.UI
{
    [Singleton(false)]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class HandCollider_Left : HandCollider
    {
        public override bool IsRightHand => false;
    }
}
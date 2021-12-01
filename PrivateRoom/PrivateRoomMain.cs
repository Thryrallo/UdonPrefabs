
using Thry.General;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.PrivateRoom
{
    public class PrivateRoomMain : UdonSharpBehaviour
    {
        public ThryAction doorOutside;
        public ThryAction doorInside;

        private void Start()
        {
            doorOutside.teleportTarget = doorInside.transform;
            doorInside.teleportTarget = doorOutside.transform;
        }
    }
}
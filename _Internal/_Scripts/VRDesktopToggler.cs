
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VRDesktopToggler : UdonSharpBehaviour
    {
        public GameObject[] vrObjects;
        public GameObject[] desktopObjects;

        private void Start()
        {
            bool isVR = Networking.LocalPlayer != null && Networking.LocalPlayer.IsUserInVR();
            foreach (GameObject o in vrObjects) o.SetActive(isVR);
            foreach (GameObject o in desktopObjects) o.SetActive(!isVR);
        }
    }
}
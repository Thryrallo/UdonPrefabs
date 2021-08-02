
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry
{
    public class VRDesktopToggler : UdonSharpBehaviour
    {
        public GameObject[] vrObjects;
        public GameObject[] desktopObjects;

        private void Start()
        {
            SendCustomEventDelayedSeconds(nameof(_DelayedStart), 2);
        }

        public void _DelayedStart()
        {
            bool isVR = Networking.LocalPlayer.IsUserInVR();
            foreach (GameObject o in vrObjects) o.SetActive(isVR);
            foreach (GameObject o in desktopObjects) o.SetActive(!isVR);
        }

    }
}
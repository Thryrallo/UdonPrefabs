
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.PrivateRoom
{
    public class PrivateRoomRequest : UdonSharpBehaviour
    {
        public UnityEngine.UI.Text nameTextfield;
        public GameObject roomRquestObject;
        public Camera doorCamera;

        [UdonSynced]
        string playername;

        [UdonSynced]
        bool makeSureNoLateJoiner;
        bool l_makeSureNoLateJoiner;

        float lastRequest = 0;

        public override void OnDeserialization()
        {
            if (makeSureNoLateJoiner == l_makeSureNoLateJoiner) return;
            nameTextfield.text = "by " + playername;
            roomRquestObject.SetActive(true);
            doorCamera.Render();
            lastRequest = Time.time;
            SendCustomEventDelayedSeconds(nameof(Hide), 30);
            l_makeSureNoLateJoiner = makeSureNoLateJoiner;
        }

        public void Hide()
        {
            if (Time.time - lastRequest < 29) return; 
            roomRquestObject.SetActive(false);
        }

        public void ForceHide()
        {
            roomRquestObject.SetActive(false);
        }

        public void OnInteraction()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            playername = Networking.LocalPlayer.displayName;
            makeSureNoLateJoiner = !makeSureNoLateJoiner;
            RequestSerialization();
            OnDeserialization();
        }
    }
}
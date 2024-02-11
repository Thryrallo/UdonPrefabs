
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SAO
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RemotePlayerButton : UdonSharpBehaviour
    {
        public Playerlist PlayerListManager;
        public int PlayerId;
        public UnityEngine.UI.Text PlayerNameText;

        public void Execute()
        {   
            PlayerListManager.OpenRemotePlayerDetails(PlayerId);
        }
    }
}
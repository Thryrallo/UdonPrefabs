
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SpinTheBottle
{
    public class PlayerTracker : UdonSharpBehaviour
    {
        [HideInInspector]
        public VRCPlayerApi[] players;

        [HideInInspector]
        public int length = 0;

        void Start()
        {
            players = new VRCPlayerApi[90];
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            players[length] = player;
            length++;
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            RemovePlayer(player);
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            RemovePlayer(player);
        }

        public void DebugTracker()
        {
            Debug.Log($"[PlayerTracker] Player Count: {length}");
            for(int i = 0; i < length; i++)
            {
                if(Utilities.IsValid(players[i])) Debug.Log($"[PlayerTracker][{i}] {players[i].displayName}");
                else Debug.Log($"[PlayerTracker][{i}] IS INVALID");
            }
        }

        public void RemoveAtIndex(int i)
        {
            length--;
            players[i] = players[length];
        }

        public void RemovePlayer(VRCPlayerApi player)
        {
            for (int i = 0; i < length; i++)
            {
                if (players[i] == player)
                {
                    length--;
                    players[i] = players[length];
                    return;
                }
            }
        }

        public void ValidatePlayers()
        {
            for (int i = 0; i < length; i++)
            {
                if (Utilities.IsValid(players[i]) == false)
                {
                    length--;
                    players[i] = players[length];
                }
            }
        }
    }
}
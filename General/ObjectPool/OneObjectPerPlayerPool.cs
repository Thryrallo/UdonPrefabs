
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.ObjectPool
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class OneObjectPerPlayerPool : UdonSharpBehaviour
    {
        public GameObject[] Pool;

        [UdonSynced] int[] _ownerIds;

        [HideInInspector]
        public GameObject LocalGameObject;

        [HideInInspector]
        public UdonBehaviour LocalBehaviour;

        UdonBehaviour[] _behavioursToCallOnceEstablished = new UdonBehaviour[0];
        string[] _behavioursToCallOnceEstablishedMethods = new string[0];

        bool hasntBeenInit = true;

        void Start()
        {
            for (int i = 0; i < Pool.Length; i++)
            {
                Pool[i].SetActive(false);
            }
        }

        public override void OnDeserialization()
        {
            for(int i = 0;i<Pool.Length;i++)
            {
                if(Pool[i] != null)
                {
                    Pool[i].SetActive(_ownerIds[i] > -1);
                    if(_ownerIds[i] == Networking.LocalPlayer.playerId)
                    {
                        Networking.SetOwner(Networking.LocalPlayer, Pool[i]);
                        LocalGameObject = Pool[i];
                        LocalBehaviour = Pool[i].GetComponent<UdonBehaviour>();
                        if(hasntBeenInit)
                        {
                            hasntBeenInit = false;
                            CallSavedMethods();
                        }
                    }
                }
            }
        }

        public void CallOnceEstablished(UdonBehaviour behaviour, string methodName)
        {
            if (hasntBeenInit)
            {
                UdonBehaviour[] temp = _behavioursToCallOnceEstablished;
                string[] temp2 = _behavioursToCallOnceEstablishedMethods;
                _behavioursToCallOnceEstablished = new UdonBehaviour[temp.Length + 1];
                _behavioursToCallOnceEstablishedMethods = new string[temp2.Length + 1];
                Array.Copy(temp, _behavioursToCallOnceEstablished, temp.Length);
                Array.Copy(temp2, _behavioursToCallOnceEstablishedMethods, temp2.Length);
                _behavioursToCallOnceEstablished[temp.Length] = behaviour;
                _behavioursToCallOnceEstablishedMethods[temp2.Length] = methodName;
                return;
            }
            behaviour.SendCustomEvent(methodName);
        }

        void CallSavedMethods()
        {
            if(_behavioursToCallOnceEstablished.Length == 0) return;
            Debug.Log("[Thry][OneObjectPerPlayerPool] CallSavedMethods");
            for(int i = 0; i < _behavioursToCallOnceEstablished.Length; i++)
            {
                _behavioursToCallOnceEstablished[i].SendCustomEvent(_behavioursToCallOnceEstablishedMethods[i]);
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (Networking.IsOwner(gameObject))
            {
                if(_ownerIds == null || _ownerIds.Length != Pool.Length)
                {
                    _ownerIds = new int[Pool.Length];
                    for(int i = 0; i < Pool.Length; i++)
                    {
                        _ownerIds[i] = -1;
                    }
                } 
                for(int i = 0; i < Pool.Length; i++)
                {
                    if(_ownerIds[i] == -1)
                    {
                        _ownerIds[i] = player.playerId;
                        Networking.SetOwner(player, Pool[i]); // Setting here so it is also set for master so networking methods will instantly work
                        break;
                    }
                }
                RequestSerialization();
                OnDeserialization();
            }
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (Networking.IsOwner(gameObject))
            {
                for(int i = 0; i < Pool.Length; i++)
                {
                    if(_ownerIds[i] == player.playerId || 
                        (_ownerIds[i] != -1 && Pool[i] != LocalGameObject && Networking.IsOwner(Pool[i])))
                    {
                        _ownerIds[i] = -1;
                        Pool[i].GetComponent<UdonBehaviour>().SendCustomEvent("OnReturn");
                    }
                }
                RequestSerialization();
                OnDeserialization();
            }
        }
    }
}
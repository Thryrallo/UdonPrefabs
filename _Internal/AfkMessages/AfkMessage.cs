
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using UnityEngine.UI;
using System;

namespace Thry.Udon.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class AfkMessage : UdonSharpBehaviour
    {
        public GameObject ForOthers;
        public GameObject ForMaster;
        public AfkMessageListItem AfkMessageListItemPrefab;

        public Text Placeholder;
        public InputField InputField;

        [UdonSynced] string[] _messages = new string[0];
        [UdonSynced] string[] _users = new string[0];
        string[] _time = new string[0];
        AfkMessageListItem[] _afkMessageList = new AfkMessageListItem[0];

        string _masterName = "";

        void Start()
        {
            DetermineMaster();
        }

        public void DetermineMaster()
        {
            if (Networking.LocalPlayer == null) return;
            VRCPlayerApi[] players = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
            VRCPlayerApi.GetPlayers(players);
            foreach (VRCPlayerApi p in players)
                if (p.isMaster && p.displayName != _masterName)
                {
                    _masterName = p.displayName;
                }
            Placeholder.text = "Is " + _masterName + " AFK? Leave a message!";
            ForOthers.SetActive(!Networking.LocalPlayer.isMaster);
            ForMaster.SetActive(Networking.LocalPlayer.isMaster);
        }

        public override void OnDeserialization()
        {
            if (!Networking.IsMaster)
                return;
            if (_messages.Length > _afkMessageList.Length)
            {
                AfkMessageListItem[] temp = new AfkMessageListItem[_messages.Length];
                Array.Copy(_afkMessageList, temp, _afkMessageList.Length);
                _afkMessageList = temp;
                _afkMessageList[_afkMessageList.Length - 1] = Instantiate(AfkMessageListItemPrefab.gameObject, AfkMessageListItemPrefab.transform.parent).GetComponent<AfkMessageListItem>();
                _afkMessageList[_afkMessageList.Length - 1].gameObject.SetActive(true);
            }
            else if (_messages.Length < _afkMessageList.Length)
            {
                for (int i = _messages.Length; i < _afkMessageList.Length; i++)
                {
                    Destroy(_afkMessageList[i].gameObject);
                }
                AfkMessageListItem[] temp = new AfkMessageListItem[_messages.Length];
                Array.Copy(_afkMessageList, temp, _messages.Length);
                _afkMessageList = temp;
            }
            if (_time.Length != _messages.Length)
            {
                string[] temp = new string[_messages.Length];
                Array.Copy(_time, temp, Mathf.Min(_time.Length, temp.Length));
                _time = temp;
            }
            int index = Array.IndexOf(_users, Networking.GetOwner(gameObject).displayName);
            if (index != -1)
            {
                // get local time
                _time[index] = DateTime.Now.ToShortTimeString();
            }
            // configre all the list items
            for (int i = 0; i < _messages.Length; i++)
            {
                _afkMessageList[i].Set(_users[i], _messages[i], _time[i]);
            }
        }

        public void OnTextChanged()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            string username = Networking.LocalPlayer.displayName;
            int index = Array.IndexOf(_users, username);
            if (index == -1)
            {
                string[] temp = _messages;
                _messages = new string[temp.Length + 1];
                Array.Copy(temp, _messages, temp.Length);
                temp = _users;
                _users = new string[temp.Length + 1];
                Array.Copy(temp, _users, temp.Length);
                _users[temp.Length] = username;
                index = temp.Length;
            }
            _messages[index] = InputField.text;
            RequestSerialization();
        }

        public void Clear()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _messages = new string[0];
            _users = new string[0];
            _time = new string[0];
            RequestSerialization();
            OnDeserialization();
        }

        // Clear the message list if master leaves
        override public void OnPlayerLeft(VRCPlayerApi player)
        {
            if (player != null && player.displayName == _masterName)
            {
                if (Networking.IsOwner(gameObject))
                    Clear();
                DetermineMaster();
            }
        }
    }
}
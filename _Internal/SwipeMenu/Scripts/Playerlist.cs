
using Thry.Udon.AvatarTheme;
using Thry.Udon.SwipeMenu;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Thry.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Playerlist : ThryBehaviour
    {
        protected override string LogPrefix => "Thry.Playerlist";

        [SerializeField] GameObject _playerPrefab;
        [SerializeField] Transform _list;

        [SerializeField] Button _remotePlayerTeleportButton;
        [SerializeField] Animator _remotePlayerDetailsAnimator;
        [SerializeField] Camera _remotePlayerCamera;
        [SerializeField] UnityEngine.UI.Text _remotePlayerDetailsDisplayNameText;
        [SerializeField] UnityEngine.UI.Text _remotePlayerDetailsText;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColorDisplay1;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColorDisplay2;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColorDisplay3;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColorDisplay4;
        [SerializeField] UnityEngine.UI.Image _playerDetailsColorDisplay5;

        // Kinda want to use this to make a log in the future
        // private string[] _logs = new string[0];
        // private int[] _logTimes = new int[0];
        // private byte[] _logTypes = new byte[0];

        [SerializeField, HideInInspector] AvatarThemeColor _themeColor;
        [SerializeField, HideInInspector] SwipeMenuManager _swipeMenu;
        [SerializeField, HideInInspector] Notification _notification;

        [UdonSynced] private long[] _playerJoinTimes = new long[0];
        [UdonSynced] private long[] _playerGameStartTimes = new long[0];

        private long _localPlayerJoinTime;
        private VRCPlayerApi _detailsPlayer;

        void Start()
        {
            _localPlayerJoinTime = System.DateTimeOffset.Now.ToUnixTimeSeconds();
            SendCustomEventDelayedSeconds(nameof(CheckSynced), Random.value * 10 + 1);
        }

        public void CheckSynced()
        {
            if (Networking.LocalPlayer == null) return;
            int myId = Networking.LocalPlayer.playerId;
            if (myId >= _playerJoinTimes.Length || _playerJoinTimes[myId] != _localPlayerJoinTime)
            {
                Sync();
            }else
            {
                this.SendCustomEventDelayedSeconds(nameof(CheckSynced), 60);
            }
        }

        void Sync()
        {
            int myId = Networking.LocalPlayer.playerId;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

            Collections.AssertArrayLength(ref _playerJoinTimes, myId + 1);
            _playerJoinTimes[myId] = _localPlayerJoinTime;

            Collections.AssertArrayLength(ref _playerGameStartTimes, myId + 1);
            _playerGameStartTimes[myId] = System.DateTimeOffset.Now.ToUnixTimeSeconds() - (long)Time.time;

            RequestSerialization();
            this.SendCustomEventDelayedSeconds(nameof(CheckSynced), Random.value * 15 + 10);
        }

        public void AddTestPlayer()
        {
            Add("Test Player", -1);
        }

        public override void OnPlayerJoined(VRCPlayerApi joinedPlayerApi)
        {
            Add(joinedPlayerApi.displayName, joinedPlayerApi.playerId);
        }
        public override void OnPlayerLeft(VRCPlayerApi leftPlayerApi)
        {
            Remove(leftPlayerApi.displayName);
        }

        private void Add(string displayname, int playerId)
        {
            Log(LogLevel.Vervose, "Add Player: " + displayname);
            GameObject playerButton = Instantiate(_playerPrefab);
            playerButton.transform.position = Vector3.zero;
            playerButton.transform.localScale = Vector3.one;
            playerButton.name = displayname;
            RemotePlayerButton remotePlayerButton = playerButton.GetComponent<RemotePlayerButton>();
            remotePlayerButton.PlayerNameText.text = displayname;
            remotePlayerButton.PlayerId = playerId;

            int addAtIndex = _list.childCount;
            for (int i = _list.childCount - 1; i >= 0; i--)
            {
                if (System.String.Compare(_list.GetChild(i).name, displayname, System.StringComparison.CurrentCultureIgnoreCase) == 1)
                    addAtIndex = i;
            }
            playerButton.transform.SetParent(_list, false);
            playerButton.transform.SetSiblingIndex(addAtIndex);
            playerButton.SetActive(true);
        }

        private void Remove(string displayname)
        {
            Log(LogLevel.Vervose, "Remove Player: " + displayname);
            for (int i = _list.childCount - 1; i >= 0; i--)
            {
                if (_list.GetChild(i).name == displayname)
                    Destroy(_list.GetChild(i).gameObject);
            }
        }

        public void OpenRemotePlayerDetails(int playerId)
        {
            VRCPlayerApi player = VRCPlayerApi.GetPlayerById(playerId);
            if(Utilities.IsValid(player) == false || player == _detailsPlayer)
            {
                CloseRemotePlayerDetails();
                return;
            }
            _detailsPlayer = player;

            Log(LogLevel.Vervose, "Open Remote Player Details: " + playerId);
            float height = _detailsPlayer.GetAvatarEyeHeightAsMeters();
            string joinTimeInLocalTimeZone = "N/A";
            string playTime = "N/A";
            // HH:mm (time since in minutes)
            if(playerId < _playerJoinTimes.Length)
            {
                System.DateTimeOffset joinTime = System.DateTimeOffset.FromUnixTimeSeconds(_playerJoinTimes[playerId]);
                joinTimeInLocalTimeZone = joinTime.ToLocalTime().ToString("HH:mm") + " (" + (System.DateTimeOffset.Now.ToUnixTimeSeconds() - _playerJoinTimes[playerId]) / 60 + "min)";
                float pT = System.DateTimeOffset.Now.ToUnixTimeSeconds() - _playerGameStartTimes[playerId];
                playTime = $"{(int)(pT / 3600)}h {(int)(pT % 3600) / 60}min";
            }
            string personality = API.Avatar.GetPersonality(_detailsPlayer);
            // float avatarCompatibility = API.Avatar.GetCompatibility(Networking.LocalPlayer, _detailsPlayer);
            // float playerCompatibility = API.Player.GetCompatibility(Networking.LocalPlayer, _detailsPlayer);
            _remotePlayerDetailsDisplayNameText.text = _detailsPlayer.displayName;
            _remotePlayerDetailsText.text = $"@{playerId}\n{height.ToString("F1")}m\n{joinTimeInLocalTimeZone}\n{playTime}\n{personality}";
            API.Avatar.TakeHeadshotPicture(_detailsPlayer, _remotePlayerCamera);

            if(_themeColor && _themeColor.GetColors(_detailsPlayer, out Color[] colors))
            {
                _playerDetailsColorDisplay1.color = colors[0];
                _playerDetailsColorDisplay2.color = colors[1];
                _playerDetailsColorDisplay3.color = colors[2];
                _playerDetailsColorDisplay4.color = colors[3];
                _playerDetailsColorDisplay5.color = colors[4];
                _themeColor.ApplyPlayerColor(_remotePlayerTeleportButton, _detailsPlayer, ColorBlockApplyFlag.Normal.Invert());
            }

            if(_remotePlayerDetailsAnimator.gameObject.activeInHierarchy)
                _remotePlayerDetailsAnimator.SetBool("isOn", true);
        }

        public void TeleportToDetailsPlayer()
        {
            if(Utilities.IsValid(_detailsPlayer) == false) return;
            TeleportHelper.TeleportToPlayer(_swipeMenu, _notification, _detailsPlayer, Vector3.back);
        }

        public void CloseRemotePlayerDetails()
        {
            Log(LogLevel.Vervose, "Close Remote Player Details");
            if(_remotePlayerDetailsAnimator.gameObject.activeInHierarchy)
                _remotePlayerDetailsAnimator.SetBool("isOn", false);
            _detailsPlayer = null;
        }

        public void Test()
        {
            Log(LogLevel.Log, "Test");
            string[] testPlayers = new string[] { "Thry", "Katy", "Serah", "Mr Doodleasack", "Fuckboi420", "Zer0", "Big Tiddy Goth GF" };
            foreach (string player in testPlayers)
            {
                Add(player, -1);
            }
        }

        public void Test2()
        {
            Log(LogLevel.Log, "Test 2");
            string[] testPlayers = new string[] { "Mr Doodleasack", "Zer0" };
            foreach (string player in testPlayers)
            {
                Remove(player);
            }
        }
    }
}
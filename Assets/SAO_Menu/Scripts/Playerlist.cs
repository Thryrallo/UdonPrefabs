
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

using UnityEngine.UI;

namespace Thry.SAO
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Playerlist : UdonSharpBehaviour
    {

        public GameObject playerPrefab;
        public Transform list;

        public Animator RemotePlayerDetailsAnimator;
        public Camera RemotePlayerCamera;
        public UnityEngine.UI.Text RemotePlayerDetailsDisplayName;
        public UnityEngine.UI.Text RemotePlayerDetailsText;
        public UnityEngine.UI.Image PlayerDetailsColor1;
        public UnityEngine.UI.Image PlayerDetailsColor2;
        public UnityEngine.UI.Image PlayerDetailsColor3;
        public UnityEngine.UI.Image PlayerDetailsColor4;
        public UnityEngine.UI.Image PlayerDetailsColor5;


        public bool TEST;

        // Kinda want to use this to make a log in the future
        // private string[] _logs = new string[0];
        // private int[] _logTimes = new int[0];
        // private byte[] _logTypes = new byte[0];

        private AvatarThemeColor _themeColor;
        [UdonSynced]
        private long[] _playerJoinTimes = new long[2];
        private VRCPlayerApi _detailsPlayer;
        

        void Start()
        {
            _themeColor = AvatarThemeColor.Get();
            if (TEST)
                Test();
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
            Debug.Log("[Thry][SOA][Player-List] Add Player: " + displayname);
            GameObject playerButton = Instantiate(playerPrefab);
            playerButton.transform.position = Vector3.zero;
            playerButton.transform.localScale = Vector3.one;
            playerButton.name = displayname;
            RemotePlayerButton remotePlayerButton = playerButton.GetComponent<RemotePlayerButton>();
            remotePlayerButton.PlayerNameText.text = displayname;
            remotePlayerButton.PlayerId = playerId;

            int addAtIndex = list.childCount;
            for (int i = list.childCount - 1; i >= 0; i--)
            {
                if (System.String.Compare(list.GetChild(i).name, displayname, System.StringComparison.CurrentCultureIgnoreCase) == 1)
                    addAtIndex = i;
            }
            playerButton.transform.SetParent(list, false);
            playerButton.transform.SetSiblingIndex(addAtIndex);
            playerButton.SetActive(true);

            if(playerId != -1 && Networking.IsOwner(gameObject))
            {
                _playerJoinTimes = Helpers.AssertArrayLength(_playerJoinTimes, playerId + 1);
                _playerJoinTimes[playerId] = System.DateTimeOffset.Now.ToUnixTimeSeconds();
                RequestSerialization();
            }
        }

        private void Remove(string displayname)
        {
            Debug.Log("[Thry][SOA][Player-List] Remove Player: " + displayname);
            for (int i = list.childCount - 1; i >= 0; i--)
            {
                if (list.GetChild(i).name == displayname)
                    Destroy(list.GetChild(i).gameObject);
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

            Debug.Log("[Thry][SOA][Player-List] Open Remote Player Details: " + playerId);
            float height = _detailsPlayer.GetAvatarEyeHeightAsMeters();
            string joinTimeInLocalTimeZone = "N/A";
            // HH:mm (time since in minutes)
            if(playerId < _playerJoinTimes.Length)
            {
                System.DateTimeOffset joinTime = System.DateTimeOffset.FromUnixTimeSeconds(_playerJoinTimes[playerId]);
                joinTimeInLocalTimeZone = joinTime.ToLocalTime().ToString("HH:mm") + " (" + (System.DateTimeOffset.Now.ToUnixTimeSeconds() - _playerJoinTimes[playerId]) / 60 + "min)";
            }
            RemotePlayerDetailsDisplayName.text = _detailsPlayer.displayName;
            RemotePlayerDetailsText.text = $"@{playerId}\n{height.ToString("F2")}m\n{joinTimeInLocalTimeZone}";
            TakeRemotePicture(_detailsPlayer);

            if(_themeColor)
            {
                Color[] colors = _themeColor.GetColors(_detailsPlayer);
                PlayerDetailsColor1.color = colors[0];
                PlayerDetailsColor2.color = colors[1];
                PlayerDetailsColor3.color = colors[2];
                PlayerDetailsColor4.color = colors[3];
                PlayerDetailsColor5.color = colors[4];
            }

            RemotePlayerDetailsAnimator.SetBool("isOn", true);
        }

        public void TeleportToDetailsPlayer()
        {
            if(Utilities.IsValid(_detailsPlayer) == false) return;
            TeleportHelper.TeleportToPlayer(_detailsPlayer, Vector3.back);
        }

        public void CloseRemotePlayerDetails()
        {
            Debug.Log("[Thry][SOA][Player-List] Close Remote Player Details");
            RemotePlayerDetailsAnimator.SetBool("isOn", false);
            _detailsPlayer = null;
        }

        private void TakeRemotePicture(VRCPlayerApi player)
        {
            if(RemotePlayerCamera == null) return;
            VRCPlayerApi.TrackingData data = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
            Vector3 headPos = player.GetBonePosition(HumanBodyBones.Head);
            if (headPos == Vector3.zero)
                headPos = data.position;
            Vector3 cPos = headPos + data.rotation * Vector3.forward * 2;
            RemotePlayerCamera.transform.SetPositionAndRotation(cPos, Quaternion.LookRotation(headPos - cPos));
            RemotePlayerCamera.orthographicSize = 0.25f * player.GetAvatarEyeHeightAsMeters();
            RemotePlayerCamera.Render();
        }

        public void Test()
        {
            Debug.Log("[Thry][SOA][Player-List] Test");
            string[] testPlayers = new string[] { "Thry", "Katy", "Serah", "Mr Doodleasack", "Fuckboi420", "Zer0", "Big Tiddy Goth GF" };
            foreach (string player in testPlayers)
            {
                Add(player, -1);
            }
        }

        public void Test2()
        {
            Debug.Log("[Thry][SOA][Player-List] Test 2");
            string[] testPlayers = new string[] { "Mr Doodleasack", "Zer0" };
            foreach (string player in testPlayers)
            {
                Remove(player);
            }
        }
    }
}
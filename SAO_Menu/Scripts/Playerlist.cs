
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

using UnityEngine.UI;

namespace Thry.SAO
{
    public class Playerlist : UdonSharpBehaviour
    {

        public GameObject playerPrefab;
        public Transform list;

        public bool TEST;

        void Start()
        {
            if (TEST)
                Test();
        }

        public void AddTestPlayer()
        {
            Add("Test Player");
        }

        public override void OnPlayerJoined(VRCPlayerApi joinedPlayerApi)
        {
            Add(joinedPlayerApi.displayName);
        }
        public override void OnPlayerLeft(VRCPlayerApi leftPlayerApi)
        {
            Remove(leftPlayerApi.displayName);
        }

        private void Add(string displayname)
        {
            Debug.Log("[Thry] [Player-List] Add Player: " + displayname);
            GameObject playerButton = VRCInstantiate(playerPrefab);
            playerButton.transform.position = Vector3.zero;
            playerButton.transform.localScale = Vector3.one;
            playerButton.name = displayname;
            ((Text)playerButton.GetComponentInChildren(typeof(Text))).text = displayname;

            int addAtIndex = list.childCount;
            for (int i = list.childCount - 1; i >= 0; i--)
            {
                if (System.String.Compare(list.GetChild(i).name, displayname, System.StringComparison.CurrentCultureIgnoreCase) == 1)
                    addAtIndex = i;
            }
            playerButton.transform.SetParent(list, false);
            playerButton.transform.SetSiblingIndex(addAtIndex);
            playerButton.SetActive(true);

            foreach (Component c in playerButton.GetComponentsInChildren(typeof(UdonBehaviour), true))
            {
                if (c.gameObject != playerButton)
                    c.name = displayname;
                UdonBehaviour u = (UdonBehaviour)c;
                if(u.GetProgramVariableType("collapseAfterInstanciate") == typeof(bool) && (bool)u.GetProgramVariable("collapseAfterInstanciate") == true)
                    ((GameObject)u.GetProgramVariable("content")).SetActive(false);
            }
        }

        private void Remove(string displayname)
        {
            Debug.Log("[Thry] [Player-List] Remove Player: " + displayname);
            for (int i = list.childCount - 1; i >= 0; i--)
            {
                if (list.GetChild(i).name == displayname)
                    Destroy(list.GetChild(i).gameObject);
            }
        }

        public void Test()
        {
            Debug.Log("[Thry] [Player-List] Test");
            string[] testPlayers = new string[] { "Thry", "Katy", "Serah", "Mr Doodleasack", "Fuckboi420", "Zer0", "Big Tiddy Goth GF" };
            foreach (string player in testPlayers)
            {
                Add(player);
            }
        }

        public void Test2()
        {
            Debug.Log("[Thry] [Player-List] Test 2");
            string[] testPlayers = new string[] { "Mr Doodleasack", "Zer0" };
            foreach (string player in testPlayers)
            {
                Remove(player);
            }
        }
    }
}
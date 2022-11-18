using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Thry.Udon.Action;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
#endif

namespace Thry.SAO.Button
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Teleport : UdonSharpBehaviour
    {
        [Header("Type of Teleport")]
        public bool isPlayerTeleport;

        [Header("Point Teleport")]
        public Transform target;

        [Header("Optional Reference")]
        public Menu menu;

        Notification notificationSystem;

        private void Start()
        {
            GameObject o = GameObject.Find("[Thry][NotificationSystem]");
            if (o) notificationSystem = o.GetComponent<Notification>();
        }

        public void OnInteraction()
        {
            Debug.Log("[Thry] Try Teleport \"" + name + "\"");
            if (Networking.LocalPlayer != null)
            {
                if (isPlayerTeleport)
                {
                    VRCPlayerApi target = null;
                    VRCPlayerApi[] players = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
                    VRCPlayerApi.GetPlayers(players);
                    foreach (VRCPlayerApi p in players) if (p.displayName == name) target = p;
                    if (target != null)
                    {
                        Debug.Log("[Thry] Execute Teleport to \"" + target.displayName + "\" at " + target.GetPosition());
                        Ray ray = new Ray(target.GetPosition() + target.GetRotation() * Vector3.back + Vector3.up * 2, Vector3.down);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 5))
                        {
                            ExecuteTeleport(hit.point, target.GetRotation());
                        }
                        else
                        {
                            Debug.Log("[Thry] Could not find floor to teleport to.");
                        }

                    }
                }
                else if (target != null)
                {
                    ExecuteTeleport(target.position, target.rotation);
                }
                else
                {
                    Debug.Log("[Thry] Teleport Point not specified.");
                }
            }
        }

        private void ExecuteTeleport(Vector3 position, Quaternion rotation)
        {
            bool isInRestrictedArea = false;
            foreach(RaycastHit hit in Physics.RaycastAll(position + Vector3.up * 10, Vector3.down, 10))
            {
                if(hit.collider && hit.collider.gameObject)
                {
                    IsEmptyChecker isEmptyChecker = hit.collider.gameObject.GetComponent<IsEmptyChecker>();
                    if (isEmptyChecker && isEmptyChecker._locker)
                    {
                        if (isEmptyChecker._locker.GetBool())
                        {
                            isInRestrictedArea = true;
                            break;
                        }
                    }
                }
            }
            if (isInRestrictedArea)
            {
                Debug.Log("[Thry] Teleport denied.");
                if (notificationSystem) notificationSystem.Deny("Denied.", "Target is in a locked area.");
            }
            else
            {
                Networking.LocalPlayer.TeleportTo(position, rotation);
                if (menu != null) menu.CloseMenu();
            }
        }
    }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    [CustomEditor(typeof(Teleport))]
    internal class TeleportInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Teleport teleport = (Teleport)target;
            if(teleport.menu == null)
            {
                teleport.SetProgramVariable("menu", GameObject.Find("[SAO]Menu").GetComponent<Menu>());
            }
        }
    }
#endif
}

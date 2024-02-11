

using Thry.Udon.Action;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.SAO
{
    public class Helpers
    {
        /** Checks if array is below needed length and if so doubles the length of the array */
        public static T[] AssertArrayLength<T>(T[] ar, int neededLength)
        {
            if (ar.Length < neededLength)
            {
                return DoubleArray(ar);
            }
            return ar;
        }

        /** 
         * Returns a new array with the same elements as the given array but with double the length
         */
        public static T[] DoubleArray<T>(T[] ar)
        {
            
            T[] newAr = new T[ar.Length * 2];
            System.Array.Copy(ar, newAr, ar.Length);
            return newAr;
        }
    }

    public class TeleportHelper
    {
        public static void TeleportToPlayer(VRCPlayerApi target, Vector3 offset)
        {
            Debug.Log("[Thry] Try Teleport to \"" + target + "\"");
            if( Networking.LocalPlayer == null)
            {
                Debug.Log("[Thry] No Local Player found");
                return;
            }
            if(Utilities.IsValid(target) == false)
            {
                Debug.Log("[Thry] Player is not valid");
                return;
            }

            Ray ray = new Ray(target.GetPosition() + target.GetRotation() * offset + Vector3.up * 2, Vector3.down);
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

        private static void ExecuteTeleport(Vector3 position, Quaternion rotation)
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
                Notification notificationSystem = Notification.Get();
                if (notificationSystem) notificationSystem.Deny("Denied.", "Target is in a locked area.");
            }
            else
            {
                Networking.LocalPlayer.TeleportTo(position, rotation);
                Menu menu = Menu.Get();
                if (menu) menu.CloseMenu();
            }
        }
    }
}
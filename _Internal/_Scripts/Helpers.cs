

using UnityEngine;
using VRC.SDKBase;
using Thry.Udon.UI;
using System.Xml.Linq;
using VRC.Udon;




#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
#endif

namespace Thry.Udon
{
    public static class Collections
    {
        /// <returns>Checks if array is below needed length and if so doubles the length of the array</returns>
        public static void AssertArrayLength<T>(ref T[] ar, int neededLength)
        {
            if (ar == null)
            {
                ar = new T[neededLength * 2];
                return;
            }
            if (ar.Length < neededLength)
            {
                ChangeArrayLength(ref ar, neededLength * 2);
            }
        }

        /// <returns>Returns a new array with the same elements as the given array but with double the length</returns>
        public static void ChangeArrayLength<T>(ref T[] ar, int newLength)
        {
            T[] newAr = new T[newLength];
            if(ar != null)
                System.Array.Copy(ar, newAr, Mathf.Min(ar.Length, newLength));
            ar = newAr;
        }

        /// <summary>
        /// Adds an element to the end of the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="element"></param>

        public static void Add<T>(ref T[] ar, T element)
        {
            if (ar == null)
            {
                ar = new T[1];
                ar[0] = element;
                return;
            }
            T[] newAr = new T[ar.Length + 1];
            System.Array.Copy(ar, newAr, ar.Length);
            newAr[ar.Length] = element;
            ar = newAr;
        }

        /// <summary>
        /// Removes the last element of the array and returns it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <returns></returns>
        public static T Pop<T>(ref T[] ar)
        {
            T element = ar[ar.Length - 1];
            T[] newAr = new T[ar.Length - 1];
            System.Array.Copy(ar, newAr, ar.Length - 1);
            ar = newAr;
            return element;
        }

        /// <summary>
        /// Removes the element of the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="element"></param>
        public static void Remove<T>(T[] ar, T element)
        {
            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i].Equals(element))
                {
                    RemoveAt(ar, i);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes the element at index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="index"></param>
        public static T RemoveAt<T>(T[] ar, int index)
        {
            T[] newAr = new T[ar.Length - 1];
            System.Array.Copy(ar, newAr, index);
            System.Array.Copy(ar, index + 1, newAr, index, ar.Length - index - 1);
            ar = newAr;
            return ar[index];
        }

        /// <summary>
        /// Checks if the array contains the element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool Contains<T>(T[] ar, T element)
        {
            int index = System.Array.IndexOf(ar, element);
            return index != -1;
        }

        /// <summary>
        /// Returns a new array with the elements from start with length
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] SubArray<T>(T[] ar, int start, int length)
        {
            T[] newAr = new T[length];
            System.Array.Copy(ar, start, newAr, 0, length);
            return newAr;
        }

        public static T[] Duplicate<T>(T[] ar)
        {
            T[] newAr = new T[ar.Length];
            System.Array.Copy(ar, newAr, ar.Length);
            return newAr;
        }
    }
    
    public static class VRCLib
    {
        // Technique pulled from https://github.com/Superbstingray/UdonPlayerPlatformHook
        // private Collider[] _vrcUIColliders = new Collider[20];
        /// <summary>
        /// Utility method to detect main menu status.
        /// </summary>
        /// <returns>True if the any vrc menu is open, false otherwise</returns>
        public static bool IsVRCMenuOpen(Collider[] vrcUIColliders)
        {
            // Using OverlapSphereNonAlloc as it does not allocate memory each time it is called,
            if (!Utilities.IsValid(Networking.LocalPlayer)) return false;
            int uiColliderCount =
                Physics.OverlapSphereNonAlloc(Networking.LocalPlayer.GetPosition(), 10f, vrcUIColliders, 524288);
            return uiColliderCount > 6;
        }
    }

    public class TeleportHelper
    {
        public static void TeleportToPlayer(SwipeMenu.SwipeMenuManager swipeMenu, Notification notificationSystem, VRCPlayerApi target, Vector3 offset)
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
                ExecuteTeleport(swipeMenu, notificationSystem, hit.point, target.GetRotation());
            }
            else
            {
                Debug.Log("[Thry] Could not find floor to teleport to.");
            }
        }

        private static void ExecuteTeleport(SwipeMenu.SwipeMenuManager swipeMenu, Notification notificationSystem, Vector3 position, Quaternion rotation)
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
                if (swipeMenu) swipeMenu.CloseMenu();
            }
        }
    }
#if UNITY_EDITOR && !COMPILER_UDONSHARP
    public class EditorHelper
    {
        [MenuItem("Thry/Udon/Disable Navigation on all UI")]
        static void DisableNavigation()
        {
            UnityEngine.UI.Selectable[] selectables = GameObject.FindObjectsOfType<UnityEngine.UI.Selectable>();
            foreach (UnityEngine.UI.Selectable selectable in selectables)
            {
                selectable.navigation = new UnityEngine.UI.Navigation() { mode = UnityEngine.UI.Navigation.Mode.None };
            }
        }

        public static void AddTag(string tagname)
        {
            UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    if (tags.GetArrayElementAtIndex(i).stringValue == tagname)
                    {
                        return;     // Tag already present, nothing to do.
                    }
                }
                
                tags.InsertArrayElementAtIndex(0);
                tags.GetArrayElementAtIndex(0).stringValue = tagname;
                so.ApplyModifiedProperties();
                so.Update();
            }
        }
    }
#endif
}
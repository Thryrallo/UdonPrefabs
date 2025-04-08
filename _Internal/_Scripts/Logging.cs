
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon
{
    public class Logger
    {
        public static void Log(string classname, string message)
        {
            Debug.Log($"[<color=#ffb3de>{classname}</color>] {message}");
        }

        public static void LogWarning(string classname, string message)
        {
            Debug.LogWarning($"[<color=#ffb3de>{classname}</color>] {message}");
        }

        public static void LogError(string classname, string message)
        {
            Debug.LogError($"[<color=#ffb3de>{classname}</color>] {message}");
        }
    }
}
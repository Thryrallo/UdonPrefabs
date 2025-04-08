using UdonSharp;
using UnityEngine;

namespace Thry.Udon
{
    public enum LogLevel
    {
        Error,
        Warning,
        Log,
        Vervose
    }

    public abstract class ThryBehaviour : UdonSharpBehaviour
    {
        protected abstract string LogPrefix { get; }
        [SerializeField] protected LogLevel LogMinLevel = LogLevel.Warning;

        protected void Log(LogLevel level, string message)
        {
            if ((int)level > (int)LogMinLevel) return;
            if(level == LogLevel.Warning)
                Logger.LogWarning(LogPrefix, message);
            else if(level == LogLevel.Error)
                Logger.LogError(LogPrefix, message);
            else
                Logger.Log(LogPrefix, message);
        }
    }
}
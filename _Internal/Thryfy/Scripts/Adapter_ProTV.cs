using UnityEngine;
using VRC.SDKBase;
using JetBrains.Annotations;
using UdonSharp;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
#endif

namespace Thry.Udon.YTDB
{
#if AVPRO_IMPORTED
    using ArchiTech.ProTV;
    using VRC.SDK3.Video.Components.Base;
    using static UnityEngine.PlayerLoop.PostLateUpdate;

    public class Adapter_ProTV : VideoPlayerAdapter
    {
        public TVManager VideoPlayer;

        protected void Start()
        {
            Connect(VideoPlayer.name);
            VideoPlayer._RegisterListener(this);
            _prevVolume = VideoPlayer.volume;
        }

        // Hack for OnVolumeChange
        private void Update()
        {
            if (SkipUpdate) return;

            float volume = VideoPlayer.volume;
            
            if (volume != _prevVolume)
            {
                _TvVolumeChange();
            }

            if (!VideoPlayer.ActiveManager) return;

            BaseVRCVideoPlayer actualPlayer = VideoPlayer.ActiveManager.videoPlayer;

            if (actualPlayer &&
                actualPlayer.IsPlaying
                && !_supressNext
                && !VideoPlayer.IsLoadingMedia
                && Networking.IsOwner(Networking.LocalPlayer, VideoPlayer.gameObject)
                && ThryfyMain.VideoUrl.Equals(VideoPlayer.url)
                && actualPlayer.GetTime() > (actualPlayer.GetDuration() - 4))
            {
                Debug.Log("[Thryfy][Adapter_ProTV] Next Song");
                ThryfyMain.Next();
                _supressNext = true;
            }
            
            _prevVolume = volume;
        }

        [PublicAPI]
        public override void Play(VRCUrl url, string title)
        {
            if(!(string.IsNullOrEmpty(url.Get())))
            {
                Debug.Log($"[Thryfy][Adapter_ProTV] Play {url}");
                Networking.SetOwner(Networking.LocalPlayer, VideoPlayer.gameObject);
                VideoPlayer._ChangeMedia(url, url, title);
                VideoPlayer._RefreshMedia();
                VideoPlayer._Play();
            }
        }

        [PublicAPI]
        public override void Pause()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoPlayer.gameObject);
            VideoPlayer._Pause();
        }

        [PublicAPI]
        public override void Resume()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoPlayer.gameObject);
            VideoPlayer._Play();
        }

        [PublicAPI]
        public override void TakeControl()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoPlayer.gameObject);
        }

        [PublicAPI]
        public override void ForceVideoSync()
        {
            VideoPlayer._Sync();
        }

        [PublicAPI]
        public override Texture GetVideoTexture()
        {
            return VideoPlayer._GetVideoTexture();
        }

        [PublicAPI]
        public override VRCUrl GetVideoUrl()
        {
            return VideoPlayer.url;
        }

        [PublicAPI]
        public override void UpdateVolumeFromTablet()
        {
            if (_waitingForInit) return;
            VideoPlayer._ChangeVolume(ThryfyMain.GetVolume());
        }

        [PublicAPI]
        public override void UpdateVolumeFromVideoPlayer()
        {
            if (_waitingForInit) return;
            //ThryfyManager.SetVolume(VideoPlayer.volume, false);
        }

        public void _TvPause()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenPaused();
        }

        public void _TvPlay()
        {
            if (_waitingForInit) return;
            Debug.Log("[Thryfy][Adapter_ProTV] _TvPlay");
            ThryfyMain.VideoPlayerHasBeenResumed();
        }

        public void _TvVolumeChange()
        {
            if (_waitingForInit) return;
            ThryfyMain.SetVolume(VideoPlayer.volume, false);
        }

        public void _TvStop()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenPaused();
        }

        public void _TvMediaEnd()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenPaused();
        }

        public void _TvMediaChange()
        {
            if (_waitingForInit) return;
            _supressNext = false;
        }
#else
    public class Adapter_ProTV : UdonSharpBehaviour
    {
#endif

// #if UNITY_EDITOR && !COMPILER_UDONSHARP

//         [InitializeOnLoadMethod]
//         private static void InitializeOnLoad()
//         {
//             // Check if class UdonSharp.Video.USharpVideoPlayer in any assembly
//             bool hasUSharpVideoPlayer = System.AppDomain.CurrentDomain.GetAssemblies()
//                 .SelectMany(x => x.GetTypes())
//                 .Any(x => x.FullName == "ArchiTech.ProTV.TVManager");
//             // Set Scripting Define ARCHI_TECH_PRO_TV if found
//             if (hasUSharpVideoPlayer)
//             {
//                 string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
//                 if (!defines.Contains("ARCHI_TECH_PRO_TV"))
//                 {
//                     defines += ";ARCHI_TECH_PRO_TV";
//                     PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
//                 }
//             }
//         }
// #endif
    }
}

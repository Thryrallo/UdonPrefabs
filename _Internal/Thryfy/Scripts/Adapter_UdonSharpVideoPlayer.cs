using UnityEngine;
using VRC.SDKBase;
using JetBrains.Annotations;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
#endif

namespace Thry.Udon.YTDB
{
#if USHARP_VIDEO_PLAYER
    using UdonSharp.Video;
    using VRC.SDK3.Video.Components.Base;

    public class Adapter_UdonSharpVideoPlayer : VideoPlayerAdapter
    {
        public USharpVideoPlayer VideoPlayer;

        protected void Start()
        {
            Connect(VideoPlayer.name);
            VideoPlayer.RegisterCallbackReceiver(this);
        }

        private void Update()
        {
            if (SkipUpdate) return;

            if (!VideoPlayer.GetVideoManager()) return;

            UpdateVolumeFromVideoPlayer();

            BaseVRCVideoPlayer actualPlayer = VideoPlayer.GetVideoManager().unityVideoPlayer;

            if (actualPlayer.IsPlaying
                && !_supressNext
                && actualPlayer
                && Networking.IsOwner(Networking.LocalPlayer, VideoPlayer.gameObject)
                && ThryfyMain.VideoUrl.Equals(VideoPlayer.GetCurrentURL())
                && actualPlayer.GetTime() > (actualPlayer.GetDuration() - 3))
            {
                Debug.Log("[Thryfy][Adapter_UdonSharpVideoPlayer] Next Song");
                ThryfyMain.Next();
                _supressNext = true;
            }
        }

        [PublicAPI]
        public override void Play(VRCUrl url, string title)
        {
            if(!(string.IsNullOrEmpty(url.Get())))
            {
                VideoPlayer.PlayVideo(url);
            }
        }

        [PublicAPI]
        public override void Pause()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoPlayer.gameObject);
            VideoPlayer.SetPaused(true);
        }

        [PublicAPI]
        public override void Resume()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoPlayer.gameObject);
            VideoPlayer.SetPaused(false);
        }

        [PublicAPI]
        public override void TakeControl()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoPlayer.gameObject);
        }

        [PublicAPI]
        public override void ForceVideoSync()
        {
            VideoPlayer.ForceSyncVideo();
        }

        [PublicAPI]
        public override VRCUrl GetVideoUrl()
        {
            return VideoPlayer.GetCurrentURL();
        }

        [PublicAPI]
        public override Texture GetVideoTexture()
        {
            return VideoPlayer.GetVideoManager().GetVideoTexture();
        }

        [PublicAPI]
        public override void UpdateVolumeFromTablet()
        {
            if (_waitingForInit) return;
            VideoPlayer.SetVolume(ThryfyMain.GetVolume());
        }

        [PublicAPI]
        public override void UpdateVolumeFromVideoPlayer()
        {
            if (_waitingForInit) return;
            ThryfyMain.SetVolume(VideoPlayer.GetVolume(), false);
        }

        public void OnUSharpVideoPause()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenPaused();
        }

        public void OnUSharpVideoUnpause()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenResumed();
        }

        public void OnUSharpVideoPlay()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenResumed();
            _supressNext = false;
        }

        public void OnUSharpVideoEnd()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenPaused();
            _supressNext = false;
        }
#else
    public class Adapter_UdonSharpVideoPlayer : UdonSharpBehaviour
    {
#endif

#if UNITY_EDITOR && !COMPILER_UDONSHARP

    [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            // Check if class UdonSharp.Video.USharpVideoPlayer in any assembly
            bool hasUSharpVideoPlayer = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Any(x => x.FullName == "UdonSharp.Video.USharpVideoPlayer");
            // Set Scripting Define USHARP_VIDEO_PLAYER if found
            if (hasUSharpVideoPlayer)
            {
                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                if (!defines.Contains("USHARP_VIDEO_PLAYER"))
                {
                    defines += ";USHARP_VIDEO_PLAYER";
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
                }
            }
        }
#endif
    }
}

using UnityEngine;
using VRC.SDKBase;
using JetBrains.Annotations;
using VRC.SDK3.Video.Components.Base;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
#endif

namespace Thry.Udon.YTDB
{
#if WOLFE_VIDEO_PLAYER
    public class Adapter_Wolfe : VideoPlayerAdapter
    {
        public WolfePlayerController VideoController;
        WolfePlayerPanel _panel;
        BaseVRCVideoPlayer _videoPlayer;

        protected void Start()
        {
            Connect(VideoController.name);
            _videoPlayer = VideoController.GetComponent<BaseVRCVideoPlayer>();
            _panel = VideoController.GetComponentInChildren<WolfePlayerPanel>(true);
            _prevVolume = GetVideoPlayerVolume();
        }

        private void Update()
        {
            if (SkipUpdate) return;

            if (_waitingForInit || _videoPlayer == null) return;
            float volume = GetVideoPlayerVolume();
            bool isPlaying = _videoPlayer.IsPlaying;
            VRCUrl url = VideoController.SyncedUrlProperty;
            if (volume != _prevVolume)
            {
                OnVolumeChange();
            }
            if (isPlaying && !_wasPlaying)
                OnVideoPlay();
            if (!isPlaying && _wasPlaying)
                OnVideoPause();

            if (_videoPlayer.IsPlaying
                && !_supressNext
                && Networking.IsOwner(Networking.LocalPlayer, VideoController.gameObject)
                && ThryfyMain.VideoUrl.Equals(url)
                && _videoPlayer.GetTime() > (_videoPlayer.GetDuration() - 3))
            {
                Debug.Log("[Thryfy][Adapter_Wolfe] Next Song");
                ThryfyMain.Next();
                _supressNext = true;
            }

            _prevVolume = volume;
            _wasPlaying = isPlaying;
        }

        [PublicAPI]
        public override void Play(VRCUrl url, string title)
        {
            if(!(string.IsNullOrEmpty(url.Get())))
            {
                Networking.SetOwner(Networking.LocalPlayer, VideoController.gameObject);
                VideoController.SetSyncedUrl(url);
            }
        }

        [PublicAPI]
        public override void Pause()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoController.gameObject);
            VideoController.Pause();
        }

        [PublicAPI]
        public override void Resume()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoController.gameObject);
            VideoController.Play();
        }

        [PublicAPI]
        public override void TakeControl()
        {
            Networking.SetOwner(Networking.LocalPlayer, VideoController.gameObject);
        }

        [PublicAPI]
        public override void ForceVideoSync()
        {
            VideoController.ForceSync();
        }

        [PublicAPI]
        public override Texture GetVideoTexture()
        {
            _isVideoTextureFlippedVertically = true;
            return VideoController.screenMaterials[0].mainTexture;
        }

        [PublicAPI]
        public override VRCUrl GetVideoUrl()
        {
            return VideoController.SyncedUrlProperty;
        }

        [PublicAPI]
        public override void UpdateVolumeFromTablet()
        {
            if (_waitingForInit) return;
            VideoController.SetVolume(ThryfyMain.GetVolume());
        }

        [PublicAPI]
        public override void UpdateVolumeFromVideoPlayer()
        {
            if (_waitingForInit) return;
            ThryfyMain.SetVolume(_panel.sliderVolume.value, false);
        }

        public override void OnVideoPause()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenPaused();
        }

        public override void OnVideoPlay()
        {
            if (_waitingForInit) return;
            ThryfyMain.VideoPlayerHasBeenResumed();
            _supressNext = false;
        }

        public void OnVolumeChange()
        {
            if (_waitingForInit) return;
            UpdateVolumeFromVideoPlayer();
        }

        private float GetVideoPlayerVolume()
        {
            return _panel.sliderVolume.value;
        }
#else
    public class Adapter_Wolfe : UdonSharpBehaviour
    {
#endif

#if UNITY_EDITOR && !COMPILER_UDONSHARP

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            // Check if class UdonSharp.Video.USharpVideoPlayer in any assembly
            bool hasUSharpVideoPlayer = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Any(x => x.FullName == "WolfePlayerController");
            // Set Scripting Define WOLFE_VIDEO_PLAYER if found
            if (hasUSharpVideoPlayer)
            {
                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                if (!defines.Contains("WOLFE_VIDEO_PLAYER"))
                {
                    defines += ";WOLFE_VIDEO_PLAYER";
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
                }
            }
        }
#endif
    }
}

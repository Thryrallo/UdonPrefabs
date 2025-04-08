
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.YTDB
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class VideoPlayerAdapter : UdonSharpBehaviour
    {
        const int FRAME_UPDATE_RATE = 10;

        public Thryfy ThryfyMain;
        protected bool _waitingForInit = true;
        protected bool _isVideoTextureFlippedVertically = false;
        protected float _prevVolume = 0;
        protected bool _wasPlaying = false;
        protected bool _supressNext = false;

        protected void Connect(string videoPlayerName)
        {
            ThryfyMain.SetAdapter(this, videoPlayerName);
            SendCustomEventDelayedFrames(nameof(SetInit), 1); // delay by 1 frame to allow all other scripts to init
        }

        public void SetInit()
        {
            _waitingForInit = false;
        }
        
        protected bool SkipUpdate
        {
            get
            {
                if (_waitingForInit) return true;
                if (Time.frameCount % FRAME_UPDATE_RATE != 0) return true;
                return false;
            }
        }

        [PublicAPI]
        public abstract void Play(VRCUrl url, string title);

        [PublicAPI]
        public abstract void Pause();

        [PublicAPI]
        public abstract void Resume();

        [PublicAPI]
        public abstract void TakeControl();

        [PublicAPI]
        public abstract void UpdateVolumeFromTablet();

        [PublicAPI]
        public abstract void UpdateVolumeFromVideoPlayer();

        [PublicAPI]
        public abstract void ForceVideoSync();

        [PublicAPI]
        public abstract Texture GetVideoTexture();
        [PublicAPI]
        public bool IsVideoTextureFlippedVertically => _isVideoTextureFlippedVertically;

        [PublicAPI]
        public abstract VRCUrl GetVideoUrl();
    }
}

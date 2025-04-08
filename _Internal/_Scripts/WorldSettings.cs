using UnityEngine;
using Thry.Udon.UI;
using VRC.SDKBase;
using VRC.SDK3.Persistence;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor.Callbacks;
using System.Linq;
#endif

#if USHARP_VIDEO_PLAYER
using UdonSharp.Video;
#endif

#if AVPRO_IMPORTED
using ArchiTech.ProTV;
#endif

namespace Thry.Udon
{
    public class WorldSettings : SimpleUISetterExtension
    {
        const string PLAYER_DATA_PREFIX = "Thry.WorldSettings.";

        public bool EnableChairs = true;
        public bool EnableColliders = true;
        public float BackgroundVolume = 1;
        public float VideoPlayerVolume = 1;

        [SerializeField] Collider[] _colliders;
        [SerializeField] GameObject[] _chairs;
        [SerializeField] AudioSource[] _backgroundAudioSources;
        [SerializeField, HideInInspector] GameObject[] _udonSharpVideoPlayersObjs;
        [SerializeField, HideInInspector] GameObject[] _proTvVideoPlayersObjs;
        [SerializeField, HideInInspector] GameObject[] _wolfeVideoPlayersObjs;
#if USHARP_VIDEO_PLAYER
        private USharpVideoPlayer[] _udonSharpVideoPlayers;
#endif
#if AVPRO_IMPORTED
        private TVManager[] _proTvVideoPlayers;
#endif
#if WOLFE_VIDEO_PLAYER
        private WolfePlayerPanel[] _wolfeVideoPlayers;
#endif

        private float[] _initalAudioVolumes;

        protected override string LogPrefix => "Thry.WorldSettings";

        public void Start()
        {
            _initalAudioVolumes = new float[_backgroundAudioSources.Length];
            for (int i = 0; i < _backgroundAudioSources.Length; i++)
            {
                _initalAudioVolumes[i] = _backgroundAudioSources[i].volume;
            }
#if USHARP_VIDEO_PLAYER
            _udonSharpVideoPlayers = new USharpVideoPlayer[_udonSharpVideoPlayersObjs.Length];
            for (int i = 0; i < _udonSharpVideoPlayersObjs.Length; i++)
                _udonSharpVideoPlayers[i] = _udonSharpVideoPlayersObjs[i].GetComponent<USharpVideoPlayer>();
#endif
#if AVPRO_IMPORTED
            _proTvVideoPlayers = new TVManager[_proTvVideoPlayersObjs.Length];
            for (int i = 0; i < _proTvVideoPlayersObjs.Length; i++)
                _proTvVideoPlayers[i] = _proTvVideoPlayersObjs[i].GetComponent<TVManager>();
#endif
#if WOLFE_VIDEO_PLAYER
            _wolfeVideoPlayers = new WolfePlayerPanel[_wolfeVideoPlayersObjs.Length];
            for (int i = 0; i < _wolfeVideoPlayersObjs.Length; i++)
                _wolfeVideoPlayers[i] = _wolfeVideoPlayersObjs[i].GetComponentInChildren<WolfePlayerPanel>(true);
#endif
        }

        public override void OnPlayerRestored(VRCPlayerApi player)
        {
            if(!player.isLocal) return;
            TryLoadBool(nameof(EnableChairs));
            TryLoadBool(nameof(EnableColliders));
            TryLoadFloat(nameof(BackgroundVolume));
            TryLoadFloat(nameof(VideoPlayerVolume));

            CollidersChanged();
            ChairsChanged();
            BackgroundVolumeChanged();
            OnChangedVideoPlayerVolume();

            CheckForChangedVideoPlayerVolume();
        }

        private void TryLoadFloat(string key)
        {
            if(PlayerData.TryGetFloat(Networking.LocalPlayer, PLAYER_DATA_PREFIX + key, out float val))
            {
                Log(LogLevel.Vervose, "Loaded " + key + " " + val);
                this.SetProgramVariable(key, val);
                VariableChangedFromBehaviour(key);
            }
        }

        private void TryLoadBool(string key)
        {
            if(PlayerData.TryGetBool(Networking.LocalPlayer, PLAYER_DATA_PREFIX + key, out bool val))
            {
                Log(LogLevel.Vervose, "Loaded " + key + " " + val);
                this.SetProgramVariable(key, val);
                VariableChangedFromBehaviour(key);
            }
        }

        public void CollidersChanged()
        {
            foreach (Collider collider in _colliders)
            {
                collider.enabled = EnableColliders;
            }
            PlayerData.SetBool(PLAYER_DATA_PREFIX + nameof(EnableColliders), EnableColliders);
        }

        public void ChairsChanged()
        {
            foreach (GameObject chair in _chairs)
            {
                chair.SetActive(EnableChairs);
            }
            PlayerData.SetBool(PLAYER_DATA_PREFIX + nameof(EnableChairs), EnableChairs);
        }

        public void BackgroundVolumeChanged()
        {
            for (int i = 0; i < _backgroundAudioSources.Length; i++)
            {
                _backgroundAudioSources[i].volume = _initalAudioVolumes[i] * BackgroundVolume;
            }
            PlayerData.SetFloat(PLAYER_DATA_PREFIX + nameof(BackgroundVolume), BackgroundVolume);
        }

        public void CheckForChangedVideoPlayerVolume()
        {
#if USHARP_VIDEO_PLAYER
            foreach(USharpVideoPlayer player in _udonSharpVideoPlayers)
            {
                if (player.GetVolume() != VideoPlayerVolume)
                {
                    VideoPlayerVolume = player.GetVolume();
                    Log(LogLevel.Vervose, "USharpVideoPlayer volume changed to " + VideoPlayerVolume);
                    OnChangedVideoPlayerVolume(true);
                }
            }
#endif
#if AVPRO_IMPORTED
            foreach (TVManager player in _proTvVideoPlayers)
            {
                if (player.volume != VideoPlayerVolume)
                {
                    VideoPlayerVolume = player.volume;
                    Log(LogLevel.Vervose, "TVManager volume changed to " + VideoPlayerVolume);
                    OnChangedVideoPlayerVolume(true);
                }
            }
#endif
#if WOLFE_VIDEO_PLAYER
            foreach (WolfePlayerPanel player in _wolfeVideoPlayers)
            {
                if (player.sliderVolume.value != VideoPlayerVolume)
                {
                    VideoPlayerVolume = player.sliderVolume.value;
                    Log(LogLevel.Vervose, "WolfePlayerPanel volume changed to " + VideoPlayerVolume);
                    OnChangedVideoPlayerVolume(true);
                }
            }
#endif
            SendCustomEventDelayedSeconds(nameof(CheckForChangedVideoPlayerVolume), 10);
        }

        public void OnChangedVideoPlayerVolume()
        {
            OnChangedVideoPlayerVolume(false);
        }
        
        private void OnChangedVideoPlayerVolume(bool fromVideoPlayer)
        {
#if USHARP_VIDEO_PLAYER
            foreach (USharpVideoPlayer player in _udonSharpVideoPlayers)
            {
                player.SetVolume(VideoPlayerVolume);
            }
#endif
#if AVPRO_IMPORTED
            foreach (TVManager player in _proTvVideoPlayers)
            {
                if(!player.isReady)
                {
                    Log(LogLevel.Warning, "TVManager is not ready yet, engaging hackerman...");
                    player.isReady = true;
                    player._ChangeVolume(VideoPlayerVolume);
                    player.isReady = false;
                }else
                {
                    player._ChangeVolume(VideoPlayerVolume);
                }
            }
#endif
#if WOLFE_VIDEO_PLAYER
            foreach (WolfePlayerPanel player in _wolfeVideoPlayers)
            {
                player.SetVolumeSlider(VideoPlayerVolume);
                player.SetVolume();
            }
#endif
            if(fromVideoPlayer)
            {
                VariableChangedFromBehaviour(nameof(VideoPlayerVolume));
            }
            PlayerData.SetFloat(PLAYER_DATA_PREFIX + nameof(VideoPlayerVolume), VideoPlayerVolume);
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        [PostProcessSceneAttribute(-40)]
        public static void OnPostprocessScene()
        {
            WorldSettings worldSettings = FindObjectOfType<WorldSettings>(true);
            if (worldSettings == null) return;
#if USHARP_VIDEO_PLAYER
            USharpVideoPlayer[] uSharpVideoPlayers = FindObjectsOfType<USharpVideoPlayer>(true);
            worldSettings._udonSharpVideoPlayersObjs = uSharpVideoPlayers.Select(v => v.gameObject).ToArray();
#endif

#if AVPRO_IMPORTED
            TVManager[] proTvVideoPlayers = FindObjectsOfType<TVManager>(true);
            worldSettings._proTvVideoPlayersObjs = proTvVideoPlayers.Select(v => v.gameObject).ToArray();
#endif

#if WOLFE_VIDEO_PLAYER
            WolfePlayerController[] wolfeVideoPlayers = FindObjectsOfType<WolfePlayerController>(true);
            worldSettings._wolfeVideoPlayersObjs = wolfeVideoPlayers.Select(v => v.gameObject).ToArray();
#endif
        }
#endif
    }
}
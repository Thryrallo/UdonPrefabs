
using Thry.Udon.AvatarTheme;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.UI
{
    public class ClapperEffect : ThryBehaviour
    {
        ParticleSystem _particleSystem;
        AudioSource _audioSource;
        bool isNotInit = true;
        float lastAvatarHeightUpdate = 0;
        float avatarHeight = 1;
        [SerializeField, HideInInspector] AvatarThemeColor _avatarThemeColor;

        protected override string LogPrefix => "[Clapper]";

        private void Start()
        {
            _particleSystem = transform.GetComponent<ParticleSystem>();
            _audioSource = transform.GetComponent<AudioSource>();
            isNotInit = false;
        }

        public void NetworkPlay()
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(Play));
        }

        public void Play()
        {
            if (isNotInit) return;
            VRCPlayerApi player = Networking.GetOwner(gameObject);
            // change color
            if (_avatarThemeColor && _avatarThemeColor.GetColor(player, out Color c))
            {
                ParticleSystem.MainModule module = _particleSystem.main;
                module.startColor = c;
            }
            //Update avatar height
            if (Time.time - lastAvatarHeightUpdate > 10)
            {
                avatarHeight = player.GetAvatarEyeHeightAsMeters();
                lastAvatarHeightUpdate = Time.time;
                transform.localScale = Vector3.one * avatarHeight;
                float pitch = -Mathf.Log(avatarHeight * 0.1f) * 0.4f + 0.2f;
                pitch = Mathf.Clamp(pitch, 0.2f, 3f);
                float volume = avatarHeight * 0.3f + 0.5f;
                volume = Mathf.Clamp(volume, 0.5f, 1f);
                float distance = avatarHeight * 2;
                distance = Mathf.Clamp(distance, 1, 10);
                _audioSource.pitch = pitch;
                _audioSource.maxDistance = distance;
                _audioSource.volume = volume;
                Log(LogLevel.Vervose, $"[Clapper] Height: {avatarHeight} => pitch: {pitch} => volume: {volume} => distance: {distance} | Color: {_particleSystem.main.startColor}");
            }
            //Set position
            bool isHumanoid = player.GetBonePosition(HumanBodyBones.Chest) != Vector3.zero;
            if (player.IsUserInVR())
                transform.position = Vector3.Lerp(player.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position,
                    player.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position, 0.5f);
            else
                transform.position = isHumanoid ? player.GetBonePosition(HumanBodyBones.Chest) :
                    Vector3.Lerp(player.GetPosition(), player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position, 0.7f);
            transform.rotation = player.GetRotation();
            //Play particle and audio source
            _particleSystem.Emit(1);
            _audioSource.Play();
        }
    }
}
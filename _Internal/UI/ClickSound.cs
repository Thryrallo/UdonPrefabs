
using Thry.CustomAttributes;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

namespace Thry.UI
{
    [Singleton(false)]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ClickSound : UdonSharpBehaviour
    {
        private AudioSource _audioSource;

        // Set for interaction haptics on callbacks
        private HandType _lastUseHand = HandType.LEFT;
        private bool _supressNextPlay = false;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Play(Vector3 position, bool vibrate)
        {
            if(_supressNextPlay)
            {
                _supressNextPlay = false;
                return;
            }

            _audioSource.transform.position = position;
            _audioSource.Play();

            if (vibrate && Networking.LocalPlayer.IsUserInVR())
            {
                Debug.Log($"Vibrate {_lastUseHand}");
                Networking.LocalPlayer.PlayHapticEventInHand(_lastUseHand == HandType.LEFT ? VRC_Pickup.PickupHand.Left : VRC_Pickup.PickupHand.Right, 0.5f, 0.8f, 1f);
            }
        }

        public override void InputUse(bool value, UdonInputEventArgs args)
        {
            _lastUseHand = args.handType;
        }

        public void SupressNextFeedback()
        {
            _supressNextPlay = true;
        }
    }
}
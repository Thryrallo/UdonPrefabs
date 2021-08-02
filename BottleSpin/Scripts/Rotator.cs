
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SpinTheBottle
{
    public class Rotator : UdonSharpBehaviour
    {
        //===========Public Config fields===========

        public int SPIN_COUNT = 5;
        [Tooltip("Degrees per second")]
        public float SPIN_SPEED = 720;

        public PlayerTracker playerTracker;
        public bool doTargetPlayers = true;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        //===========Synced vars===========

        [UdonSynced()]
        float targetPosition;

        [UdonSynced]
        int lastTargetedPlayer;

        //===========Local vars===========

        private float localTargetPosition;
        private float fullRotationsToDoPercentage = 1;

        //===========State machine vars===========

        private int state = 0;
        private float stateStartTime = 0;

        const int S_IDLE = 0;
        const int S_ROTATE = 2;
        const int S_SLOWDOWN = 3;

        //===========Interaction===========

        public override void Interact()
        {
            Spin();
        }

        public void Spin()
        {
            if (Networking.IsOwner(gameObject) == false) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            targetPosition = GetTargetPosition();
            //if (playerTracker != null) playerTracker.DebugTracker();
            RequestSerialization();
            OnDeserialization();
            //Debug.Log($"[ThryBottle] Set new target rotation to: {targetPosition}");
            SetState(S_ROTATE);
        }

        public float GetTargetPosition()
        {
            if (playerTracker == null || doTargetPlayers == false)
            {
                return Random.Range(0, 360);
            }
            else
            {
                int i = 0;
                VRCPlayerApi selected;
                while (playerTracker.length > 0)
                {
                    i = Random.Range(0, playerTracker.length);
                    selected = playerTracker.players[i];
                    if(Utilities.IsValid(selected) == false)
                    {
                        playerTracker.RemoveAtIndex(i);
                    }else if(i != lastTargetedPlayer || playerTracker.length == 1)
                    {
                        lastTargetedPlayer = i;
                        return PlayerToAngle(selected);
                    }
                }
                return Random.Range(0, 360);
            }
        }

        private float PlayerToAngle(VRCPlayerApi player)
        {
            if (Utilities.IsValid(player) == false)
            {
                playerTracker.ValidatePlayers();
                return Random.Range(0, 360);
            }

            Vector3 playerDir = player.GetPosition() - transform.position;
            playerDir.y = 0;
            Vector3 bottleIdleDir = transform.parent.rotation * Vector3.forward;
            float a = Vector3.Angle(bottleIdleDir, playerDir);

            if ((transform.parent.rotation * playerDir).x < 0) a = 360 - a;

            return a;
        }

        //===========Variable serialization===========

        public override void OnDeserialization()
        {
            if (targetPosition != localTargetPosition)
            {
                localTargetPosition = targetPosition;
                SetState(S_ROTATE);
            }
        }

        //===========State machine + Spinning===========

        private void SetState(int i)
        {
            state = i;
            stateStartTime = Time.time;
        }

        private void Update()
        {
            if (state == S_ROTATE)
            {
                //rotates to target pos and then starts slowing down
                bool wasBeforeTarget = transform.localEulerAngles.y < localTargetPosition;
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime);
                //if roated onto or past target rotation, transition to slow down state
                if (wasBeforeTarget && transform.localEulerAngles.y >= localTargetPosition)
                {
                    fullRotationsToDoPercentage = 1 - 1.0f / SPIN_COUNT;
                    SetState(S_SLOWDOWN);
                }
                if (_audioSource != null)
                {
                    _audioSource.enabled = true;
                    _audioSource.volume = 1;
                }
            }
            else if (state == S_SLOWDOWN)
            {
                // ("degrees to rotate" / "360") => percentage of full rotation towards target rotation
                float completion = 0;
                if (transform.localEulerAngles.y > localTargetPosition) completion = 360 - transform.localEulerAngles.y + localTargetPosition;
                else completion = localTargetPosition - transform.localEulerAngles.y;
                completion = completion / 360;

                //add full rotations percentage
                completion = fullRotationsToDoPercentage + completion / SPIN_COUNT;

                //apply a exponentiation funtion to make a nicer slowdown curve
                completion = Mathf.Pow(completion, 0.5f);

                if (_audioSource != null)
                {
                    _audioSource.enabled = true;
                    _audioSource.volume = completion;
                }

                bool wasBeforeTarget = transform.localEulerAngles.y < localTargetPosition;
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime * completion);
                if (wasBeforeTarget && transform.localEulerAngles.y >= localTargetPosition)
                {
                    //increment rotation count
                    fullRotationsToDoPercentage -= 1.0f / SPIN_COUNT;

                    //if in last rotation and reached target, transition to idle
                    if (fullRotationsToDoPercentage <= 0)
                    {
                        //Debug.Log($"[ThryBottle] Reached target: {localTargetPosition}");
                        SetState(S_IDLE);
                        transform.localEulerAngles = new Vector3(0, localTargetPosition, 0);
                        if (_audioSource != null)
                        {
                            _audioSource.enabled = false;
                            _audioSource.volume = 0;
                        }
                    }
                }
            }
        }

    }
}
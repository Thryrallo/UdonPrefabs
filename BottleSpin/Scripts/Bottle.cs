
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SpinTheBottle
{
    public class Bottle : UdonSharpBehaviour
    {
        //===========Public Config fields===========

        //Settings
        public int SPIN_COUNT = 5;
        [Tooltip("Degrees per second")]
        public float SPIN_SPEED = 720;
        
        [FieldChangeCallback(nameof(DoTargetPlayers))]
        public bool _doTargetPlayers = true;

        //References
        public PlayerTracker playerTracker;
        public UnityEngine.UI.Text targetedPlayerText;
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            if (Networking.IsOwner(gameObject))
            {
                TargetPlayerId = -1;
                TargetAngle = 0.1f;
                LateJoingerAngle = 0.1f;
                RequestSerialization();
            }
            targetedPlayerText.text = "";
        }

        //===========Synced vars===========

        [UdonSynced, FieldChangeCallback(nameof(TargetAngle))]
        float _targetAngle;

        [UdonSynced, FieldChangeCallback(nameof(TargetPlayerId))]
        int _targetedPlayerId;

        [UdonSynced, FieldChangeCallback(nameof(LateJoingerAngle))]
        float _lateJoinerAngle;

        //===========Local vars===========

        private VRCPlayerApi targetedPlayer;
        private float localTargetAngle;
        private float fullRotationsToDoPercentage = 1;
        private bool isInit;
        private int initCount = 0;

        //===========State machine vars===========

        private int state = 0;
        private float stateStartTime = 0;

        const int S_IDLE = 0;
        const int S_ROTATE = 2;
        const int S_SLOWDOWN = 3;

        //Player Targeting

        public bool DoTargetPlayers
        {
            set
            {
                _doTargetPlayers = value;
                if(_doTargetPlayers == false && targetedPlayerText != null) targetedPlayerText.text = "";
            }
            get => _doTargetPlayers && playerTracker != null;
        }

        public void ClearPlayerText()
        {
            if (targetedPlayerText != null) targetedPlayerText.text = "";
        }

        public void SetPlayerText()
        {
            if (targetedPlayerText != null && DoTargetPlayers && Utilities.IsValid(targetedPlayer)) targetedPlayerText.text = "Points to: " + targetedPlayer.displayName;
        }

        private VRCPlayerApi SelectRandomPlayer()
        {
            int i = 0;
            VRCPlayerApi selected;
            while (playerTracker.length > 0)
            {
                i = Random.Range(0, playerTracker.length);
                selected = playerTracker.players[i];
                if (Utilities.IsValid(selected) == false)
                {
                    playerTracker.RemoveAtIndex(i);
                }
                else if (selected.playerId != TargetPlayerId || playerTracker.length == 1)
                {
                    return selected;
                }
            }
            return null;
        }

        private float PlayerToAngle(VRCPlayerApi player)
        {
            if (Utilities.IsValid(player) == false)
            {
                playerTracker.ValidatePlayers();
                return TargetAngle;
            }

            Vector3 playerDir = player.GetPosition() - transform.position;
            playerDir.y = 0;
            Vector3 bottleIdleDir = transform.parent.rotation * Vector3.forward;
            float a = Vector3.Angle(bottleIdleDir, playerDir);

            if ((transform.parent.rotation * playerDir).x < 0) a = 360 - a;

            return a;
        }

        //Networking

        private void InitCount()
        {
            initCount++;
            if (initCount == 3) isInit = true;
        }

        public float TargetAngle
        {
            set
            {
                _targetAngle = value;
                if (isInit)
                {
                    localTargetAngle = _targetAngle;
                    SetState(S_ROTATE);
                }
                InitCount();
            }
            get => _targetAngle;
        }

        public int TargetPlayerId
        {
            set
            {
                _targetedPlayerId = value;
                if (isInit)
                {
                    targetedPlayer = VRCPlayerApi.GetPlayerById(_targetedPlayerId);
                    localTargetAngle = PlayerToAngle(targetedPlayer);
                    SetState(S_ROTATE);
                }
                InitCount();
            }
            get => _targetedPlayerId;
        }

        public float LateJoingerAngle
        {
            set
            {
                _lateJoinerAngle = value;
                if (!isInit)
                {
                    transform.localEulerAngles = new Vector3(0, _lateJoinerAngle, 0);
                }
                InitCount();
            }
            get => _lateJoinerAngle;
        }

        //===========Interaction===========

        public override void Interact()
        {
            Spin();
        }

        public void Spin()
        {
            if (Networking.IsOwner(gameObject) == false) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            TargetAngle = Random.Range(0, 360);
            if (DoTargetPlayers) {
                targetedPlayer = SelectRandomPlayer();
                if (Utilities.IsValid(targetedPlayer))
                {
                    TargetPlayerId = targetedPlayer.playerId;
                    TargetAngle = PlayerToAngle(targetedPlayer);
                }
            }
            else
            {
                TargetPlayerId = -1;
            }
            LateJoingerAngle = TargetAngle;
            RequestSerialization();
            SetState(S_ROTATE);
        }

        //===========Variable serialization===========

        public void UpdateLocalTargetAngle()
        {
            if (TargetPlayerId > -1)
            {
                localTargetAngle = PlayerToAngle(targetedPlayer);
            }
            else
            {
                localTargetAngle = TargetAngle;
            }
        }

        //===========State machine + Spinning===========

        private void SetState(int i)
        {
            state = i;
            stateStartTime = Time.time;
        }

        private int _frame = 0;
        private void Update()
        {
            if (state == S_ROTATE)
            {
                //rotates to target pos and then starts slowing down
                bool wasBeforeTarget = transform.localEulerAngles.y < localTargetAngle;
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime);
                //if roated onto or past target rotation, transition to slow down state
                if (wasBeforeTarget && transform.localEulerAngles.y >= localTargetAngle)
                {
                    fullRotationsToDoPercentage = 1 - 1.0f / SPIN_COUNT;
                    SetState(S_SLOWDOWN);
                }
                if (_audioSource != null)
                {
                    _audioSource.enabled = true;
                    _audioSource.volume = 1;
                }

                if (_frame == 0) UpdateLocalTargetAngle();
                _frame = (_frame + 1) % 10;
            }
            else if (state == S_SLOWDOWN)
            {
                // ("degrees to rotate" / "360") => percentage of full rotation towards target rotation
                float completion = 0;
                if (transform.localEulerAngles.y > localTargetAngle) completion = 360 - transform.localEulerAngles.y + localTargetAngle;
                else completion = localTargetAngle - transform.localEulerAngles.y;
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

                bool wasBeforeTarget = transform.localEulerAngles.y < localTargetAngle;
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime * completion);
                if (wasBeforeTarget && transform.localEulerAngles.y >= localTargetAngle)
                {
                    //increment rotation count
                    fullRotationsToDoPercentage -= 1.0f / SPIN_COUNT;

                    //if in last rotation and reached target, transition to idle
                    if (fullRotationsToDoPercentage <= 0)
                    {
                        //Debug.Log($"[ThryBottle] Reached target: {localTargetPosition}");
                        SetState(S_IDLE);
                        transform.localEulerAngles = new Vector3(0, localTargetAngle, 0);
                        if (_audioSource != null)
                        {
                            _audioSource.enabled = false;
                            _audioSource.volume = 0;
                        }
                        if (Networking.IsOwner(gameObject))
                        {
                            LateJoingerAngle = localTargetAngle;
                            RequestSerialization();
                        }
                        SetPlayerText();
                    }
                }

                if (_frame == 0) UpdateLocalTargetAngle();
                _frame = (_frame + 1) % 10;
            }
        }

    }
}
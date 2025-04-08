
using System;
using Thry.Udon.AvatarTheme;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.BeerPong
{
    enum BallState
    {
        Idle,
        InHand,
        Locked,
        Flying,
        Rimming,
        InCup
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Ball : UdonSharpBehaviour
    {
        const float REDUCE_UPDATE_AT_DISTANCE = 15;
        const float SKIP_UPDATE_AT_DISTANCE = 30;
        //Rimming
        const float RIMMING_TOTAL_AGULAR_ROTATION = 3000;
        const float RIMMING_SPEED = 1300;
        float _rimming = 0;

        //Desktop strength
        const float MAX_MOUSE_DOWN_THROW_TIME = 1;
        const float MAX_THROW_STRENGTH = 5f;
        float _rightMouseDownStart;
        float _throwStrengthPercent = 0;

        //Phsyiscs
        const float BOUND_DRAG = 0.9f;

        //References and Settings
        [SerializeField] Transform _throwIndicator;
        [SerializeField] float _throwVelocityMultiplierVR = 1.5f;
        [SerializeField] float _throwVelocityMultiplierDekstop = 1f;


        [SerializeField] Game _gameManager;
        [SerializeField] VRC_Pickup _pickup;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Renderer _renderer;
        [SerializeField] ParticleSystem _splashParticles;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] Renderer _strengthIndicatorDesktop;

        //State
        [UdonSynced]
        int[] _state = new int[] { 0, 0, 0, 0, 0, 0 };
        BallState _localState = 0;
        float _stateStartTime = 0;
        float _physicsSimulationStartTime = 0;
        bool _countedHit;

        [NonSerialized, UdonSynced]
        public int CurrentPlayer = 0;

        [UdonSynced]
        Vector3 _start_position;
        [UdonSynced]
        Quaternion _start_rotation;
        [UdonSynced]
        bool _isRightHand;
        [UdonSynced]
        Vector3 _start_velocity;

        Vector3 _startPositionLocal;
        Vector3 _startVelocityLocal;

        //other
        float _timeStartBeingStill = 0;
        float _radius = 1;
        float tableHeight;

        //References set by main script
        private Transform _respawnHeight;

        //Velocity tacking
        Vector3 _lastPosition;
        Quaternion _lastRotation;
        const int VELOCITY_BUFFER_LENGTH = 10;
        int _lastvelociesHead = 0;
        Vector3[] _lastvelocies = new Vector3[VELOCITY_BUFFER_LENGTH];
        float[] _lastAngularVelocities = new float[VELOCITY_BUFFER_LENGTH];

        //Sounds
        [Header("Sounds")]
        [SerializeField] AudioClip _audio_collision_table;
        [SerializeField] AudioClip _audio_collision_glass;
        [SerializeField] AudioClip _audio_water_splash;

        [SerializeField, HideInInspector] AvatarThemeColor _avatarThemeColor;

        public AudioSource BallAudio => _audioSource;

        public void Init(Transform respawnHeight)
        {
            if(Networking.LocalPlayer == null) return;

            _respawnHeight = respawnHeight;

            _rigidbody.isKinematic = true;
            _radius = (_renderer.bounds.extents.x + _renderer.bounds.extents.y + _renderer.bounds.extents.z) / 3;

            _isDev = Array.IndexOf(_devUsers, Networking.LocalPlayer.displayName) != -1;

            _SetColor();
            if (Networking.IsOwner(gameObject))
            {
                _start_position = transform.position;
                _start_rotation = transform.rotation;
                RequestSerialization();
            }
            GetComponent<Collider>().enabled = true;

            tableHeight = _gameManager.TableHeight.position.y;
        }

        private void PlayAudio(AudioClip clip, float strength)
        {
            if(clip != null) _audioSource.PlayOneShot(clip, Mathf.Clamp01(strength));
        }

        public override void OnDeserialization()
        {
            _pickup.pickupable = _state[0] == (int)BallState.Idle || (_state[0] == (int)BallState.InCup && !_gameManager.DoAutoRespawnBalls);
            _SetColor();

            if (_state[0] == (int)BallState.Idle)
            {
                SetStatic();
                transform.position = _start_position;
                transform.rotation = _start_rotation;
                _startVelocityLocal = Vector3.zero;
            }
            else if (_state[0] == (int)BallState.InHand)
            {
                SetStatic();
                _throwIndicator.gameObject.SetActive(false);
            }
            else if (_state[0] == (int)BallState.Locked)
            {
                transform.position = _start_position;
                transform.rotation = _start_rotation;
                _throwIndicator.SetPositionAndRotation(transform.position, transform.rotation);
                _throwIndicator.gameObject.SetActive(true);
            }
            else if (_state[0] == (int)BallState.Flying)
            {
                transform.position = _start_position;
                transform.rotation = _start_rotation;
                _startPositionLocal = _start_position;
                _startVelocityLocal = _start_velocity;
                _physicsSimulationStartTime = Time.time;
                SetStaticCollisions();
                _throwIndicator.gameObject.SetActive(false);
            }else if(_state[0] == (int)BallState.Rimming)
            {
                _rimming = _state[4] * 2;
            }else if(_state[0] == (int)BallState.InCup)
            {
                Cup cup = _gameManager.GetCup(_state[1], _state[2], _state[3]);
                if (Utilities.IsValid(cup))
                {
                    cup.ColliderForThrow.enabled = false;
                    cup.ColliderInside.SetActive(true);
                }
            }

            if (_state[0] != (int)_localState)
            {
                _localState = (BallState)_state[0];
                _stateStartTime = Time.time;

                if(_localState == BallState.Idle || _localState == BallState.Flying)
                {
                    ResetLastCupsColliders();
                }
                if(_localState == BallState.InCup)
                {
                    _TriggerSplash();
                }
            }
        }

        private void _TriggerSplash()
        {
            _splashParticles.transform.position = transform.position;
            _splashParticles.Emit(20);
            PlayAudio(_audio_water_splash, 1);
        }

        public override void PostLateUpdate()
        {
            switch (_localState)
            {
                case BallState.InHand:
                    if (Networking.IsOwner(gameObject)) ThrowStrengthCheck();
                    else UpdateBallInHandPositionIfRemote();
                    break;
                case BallState.Locked:
                    if (Networking.IsOwner(gameObject)) ThrowStrengthCheck();
                    break;
                case BallState.Flying:
                    UpdateVelocityTracking();
                    if (_isStatic) transform.position = PositionAtTime(_startPositionLocal, _startVelocityLocal, Time.time - _physicsSimulationStartTime);
                    // if (_isStatic) transform.position = PositionAtTime(startPositionLocal, startVelocityLocal, (Time.time - physicsSimulationStartTime) * 0.2f); // for testing
                    if (Networking.IsOwner(gameObject)) BallRespawnChecks();
                    break;
                case BallState.Rimming:
                    DoRimming();
                    break;
                case BallState.InCup:
                    DoInCup();
                    break;
            }
        }

        void UpdateVelocityTracking()
        {
            _lastvelociesHead = (++_lastvelociesHead) % VELOCITY_BUFFER_LENGTH;
            _lastvelocies[_lastvelociesHead] = (transform.position - _lastPosition) / Time.deltaTime;
            _lastAngularVelocities[_lastvelociesHead] = Quaternion.Angle(transform.rotation,_lastRotation) / Time.deltaTime;
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
        }

        void ThrowStrengthCheck()
        {
            //For vr
            UpdateVelocityTracking();
            //For desktop
            if (!Input.GetMouseButton(1)) _rightMouseDownStart = Time.time;
            else
            {
                _throwStrengthPercent = (Time.time - _rightMouseDownStart) / MAX_MOUSE_DOWN_THROW_TIME;
                _strengthIndicatorDesktop.material.SetFloat("_Strength", _throwStrengthPercent);
            }
        }

        void UpdateBallInHandPositionIfRemote()
        {
            if (_isRightHand)
            {
                Quaternion q = Networking.GetOwner(gameObject).GetBoneRotation(HumanBodyBones.RightHand);
                transform.SetPositionAndRotation(Networking.GetOwner(gameObject).GetBonePosition(HumanBodyBones.RightHand) + q * _start_position,
                     q * _start_rotation);
            }
            else
            {
                Quaternion q = Networking.GetOwner(gameObject).GetBoneRotation(HumanBodyBones.LeftHand);
                transform.SetPositionAndRotation(Networking.GetOwner(gameObject).GetBonePosition(HumanBodyBones.LeftHand) + q * _start_position,
                    q * _start_rotation);
            }
        }

        void BallRespawnChecks()
        {
            //respawn ball for other team if falls off table
            if (transform.position.y < _respawnHeight.position.y)
            {
                Debug.Log("[ThryBP] Ball below respawn point. Respawning for other team.");
                NextTeam();
                Respawn();
            }
            if (_lastvelocies[_lastvelociesHead].sqrMagnitude > 0.5f)
            {
                _timeStartBeingStill = Time.time;
            }
            else if (Time.time - _timeStartBeingStill > 1.0f && Time.time - _stateStartTime > 1.0f)
            {
                Debug.Log("[ThryBP] Ball idle for more than 1 second. Respawning for other team.");
                NextTeam();
                Respawn();
            }
            else if (Time.time - _stateStartTime > 15f)
            {
                Debug.Log("[ThryBP] Ball Timeout. Respawning for other team.");
                NextTeam();
                Respawn();
            }
        }

        void DoRimming()
        {
            if (_rimming < RIMMING_TOTAL_AGULAR_ROTATION)
            {
                Cup cup = _gameManager.GetCup(_state[1], _state[2], _state[3]);
                if (Utilities.IsValid(cup))
                {
                    _rimming += RIMMING_SPEED * Time.deltaTime;

                    float inside = 0.9f - _rimming / RIMMING_TOTAL_AGULAR_ROTATION * 0.15f;
                    float downwards = 1 - _rimming / RIMMING_TOTAL_AGULAR_ROTATION * 0.3f;
                    float angle = (_rimming - Mathf.Pow(_rimming * 0.009f, 2)) / cup.GlobalCircumfrence * 0.4f;
                    transform.position = cup.transform.position + Vector3.up * (cup.GlobalHeight * downwards)
                        + Quaternion.Euler(0, angle, 0) * Vector3.forward * (cup.GlobalRadius * cup.transform.lossyScale.x * inside - _radius);
                }
                else
                {
                    Respawn();
                }
            }
            else
            {
                Cup cup = _gameManager.GetCup(_state[1], _state[2], _state[3]);
                if (Utilities.IsValid(cup))
                {
                    cup.ColliderInside.SetActive(true);
                    cup.ColliderForThrow.enabled = false;
                    Vector3 newVelocity = Vector3.zero;
                    newVelocity = Quaternion.Euler(0, 90 + _rimming, 0) * Vector3.forward * 0.3f; //in circle
                    newVelocity += Quaternion.Euler(0, _rimming + 180, 0) * Vector3.forward * 0.1f; //inwards
                    newVelocity += Vector3.down * 0.1f; //downwards
                    SetDynamic();
                    SetVelocity(newVelocity);
                    _localState = BallState.InCup;
                    _stateStartTime = Time.time;
                    _TriggerSplash();
                    _pickup.pickupable = !_gameManager.DoAutoRespawnBalls;
                }
                else
                {
                    Respawn();
                }
            }
        }

        void DoInCup()
        {
            Cup cup = _gameManager.GetCup(_state[1], _state[2], _state[3]);
            if (Utilities.IsValid(cup))
            {

                //if ball is not in cup teleport ball in cup
                Collider c = cup.ColliderForThrow;
                c.enabled = true;
                Vector3 cPoint = c.ClosestPoint(transform.position);
                c.enabled = false;
                Bounds b = cup.GetBounds();
                b.size = b.size += Vector3.up;
                if (Vector3.Distance(cPoint, transform.position) > 0.01f && ((_rigidbody.velocity.sqrMagnitude == 0) || (b.Contains(transform.position) == false)))
                {
                    //Debug.Log($"cPoint: {cPoint.ToString("F3")} point: {transform.position.ToString("F3")}");
                    //Debug.Log($"[ThryBP] Teleported ball into cup ({state[1]},{state[2]},{state[3]})");
                    transform.position = cup.GetBounds().center;
                }
                else if (Networking.IsOwner(gameObject))
                {
                    if (_gameManager.Gamemode == GameMode.Normal)
                    {
                        if(!_countedHit)
                        {
                            _countedHit = true;
                            _gameManager.CountCupHit(cup.PlayerCupOwner.PlayerIndex, CurrentPlayer, _state[0] == (int)BallState.Rimming ? 1 : 0);
                            if(!_gameManager.DoAutoRespawnBalls)
                            {
                                NextTeam();
                                SendCustomEventDelayedSeconds(nameof(SendItsYourTurn), 2);
                            } 
                        }
                        if (Time.time - _stateStartTime > 2)
                        {
                            if(_gameManager.DoAutoRespawnBalls || Time.time - _stateStartTime > 20)
                            {
                                _gameManager.RemoveCup(cup, CurrentPlayer);
                                Respawn();
                            }
                        }
                    }
                    else if (_gameManager.Gamemode == GameMode.KingOfTheHill || _gameManager.Gamemode == GameMode.Mayham)
                    {
                        if (Time.time - _stateStartTime > 1)
                        {
                            _gameManager.CountCupHit(cup.PlayerCupOwner.PlayerIndex, CurrentPlayer, _state[0] == (int)BallState.Rimming ? 1 : 0);
                            _gameManager.RemoveCup(cup, CurrentPlayer);
                            Respawn();
                        }
                    }
                    else if (_gameManager.Gamemode == GameMode.BigPong)
                    {
                        if (Time.time - _stateStartTime > 3)
                        {
                            _gameManager.CountCupHit(cup.PlayerCupOwner.PlayerIndex, CurrentPlayer, _state[0] == (int)BallState.Rimming ? 1 : 0);
                            Respawn();
                        }
                    }
                }
            }
            else
            {
                Respawn();
            }
        }

        void SetState(BallState s)
        {
            _state[0] = (int)s;
            _localState = s;
            _stateStartTime = Time.time;
        }

        public void SetPositionRotation(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            _start_position = position;
            _start_rotation = rotation;
            RequestSerialization();
        }

        //Set ownership
        public override void OnPickup()
        {
            _countedHit = false;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SendCustomEventDelayedFrames(nameof(OnPickupDelayed), 1);

            if (_localState == BallState.InCup)
            {
                SetVelocity(Vector3.zero);
                SetAngularVelocity(Vector3.zero);
                SetStatic();

                Cup cup = _gameManager.GetCup(_state[1], _state[2], _state[3]);
                _gameManager.RemoveCup(cup, CurrentPlayer);
            }

            if (_avatarThemeColor && _gameManager.UseAvatarThemeColor)
            {
                _gameManager.Players[CurrentPlayer].SetColor(_avatarThemeColor.GetColor());
            }

            SetState(BallState.InHand);
        }

        public void OnPickupAI()
        {
            _countedHit = false;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

            if (_localState == BallState.InCup)
            {
                SetVelocity(Vector3.zero);
                SetAngularVelocity(Vector3.zero);
                SetStatic();

                Cup cup = _gameManager.GetCup(_state[1], _state[2], _state[3]);
                _gameManager.RemoveCup(cup, CurrentPlayer);
            }
            SetState(BallState.Idle);
        }

        public void OnPickupDelayed()
        {
            SerializePositionRelativeToHand();
        }

        private void SerializePositionRelativeToHand()
        {
            SendCustomEventDelayedFrames(nameof(SerializePositionRelativeToHandConcrete), 1);
            SendCustomEventDelayedSeconds(nameof(SerializePositionRelativeToHandConcrete), 1);
        }

        public void SerializePositionRelativeToHandConcrete()
        {
            if (_state[0] != (int)BallState.InHand) return;
            _isRightHand = _pickup.currentHand == VRC_Pickup.PickupHand.Right;
            if (_isRightHand)
            {
                _start_position = transform.position - Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightHand);
                Quaternion q = Quaternion.Inverse(Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.RightHand));
                _start_rotation = q * transform.rotation;
                _start_position = q * _start_position;
            }
            else
            {
                _start_position = transform.position - Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftHand);
                Quaternion q = Quaternion.Inverse(Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.LeftHand));
                _start_rotation = q * transform.rotation;
                _start_position = q * _start_position;
            }
            RequestSerialization();
        }


        bool _isDev;
        int _special;

        public override void OnPickupUseDown()
        {
            _special++;
        }

        public override void OnPickupUseUp()
        {
            if ((int)InputManager.GetLastUsedInputMethod() == 10) //if index, release
            {
                _pickup.Drop();
            }
        }

        Vector3 dropVelocity;
        public override void OnDrop()
        {
            //Drop velocity is avg velocity over last few frames
            dropVelocity = Vector3.zero;
            int count = 0;
            float angularAddition = 0;
            for (int i = 0; i < VELOCITY_BUFFER_LENGTH; i++)
            {
                dropVelocity += _lastvelocies[i];
                if (_lastvelocies[i] != Vector3.zero) count++;
                if (_lastAngularVelocities[i] > angularAddition) angularAddition = _lastAngularVelocities[i];
            }
            dropVelocity = dropVelocity / count;

            angularAddition = Mathf.Min(1.5f, angularAddition / 2000);
            dropVelocity = (dropVelocity.magnitude + angularAddition) * dropVelocity.normalized;

            
            SendCustomEventDelayedFrames(nameof(OnDropDelayed), 1);
        }

        private string[] _devUsers = new string[] { "Thryrallo", "Thry", "Katy" };
        public void OnDropDelayed()
        {
            if (Networking.LocalPlayer.IsUserInVR())
            {
                Debug.Log("[Thry][BP] DropVelocity: " + dropVelocity);
                //Transfer the velocity to the selected vector
                if (_state[0] == (int)BallState.Locked)
                {
                    transform.SetPositionAndRotation(_throwIndicator.position, _throwIndicator.rotation);
                    _start_velocity = _throwIndicator.rotation * Vector3.forward * dropVelocity.magnitude * _throwVelocityMultiplierVR;
                }
                //throw normally
                else
                {
                    _start_velocity = dropVelocity * _throwVelocityMultiplierVR;
                }
            }
            else
            {
                float strength = Mathf.Min(MAX_THROW_STRENGTH, _throwStrengthPercent * MAX_THROW_STRENGTH);
                _strengthIndicatorDesktop.material.SetFloat("_Strength", 0);
                //Transfer the velocity to the selected vector
                if (_state[0] == (int)BallState.Locked)
                {
                    transform.SetPositionAndRotation(_throwIndicator.position, _throwIndicator.rotation);
                    _start_velocity = _throwIndicator.rotation * Vector3.forward * strength * _throwVelocityMultiplierDekstop;
                }
                //throw normally
                else
                {
                    _start_velocity = transform.rotation * Vector3.forward * strength * _throwVelocityMultiplierDekstop;
                }
            }

            _start_position = transform.position;
            _start_rotation = transform.rotation;
            _startPositionLocal = _start_position;
            _startVelocityLocal = _start_velocity;
            _physicsSimulationStartTime = Time.time;

            _pickup.pickupable = false;
            SetStaticCollisions();
            _throwIndicator.gameObject.SetActive(false);

            //Aim assist
            bool doFull = _isDev && (_special >= 3 || (Networking.LocalPlayer.IsUserInVR() && Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0.9f));
            _special = 0;
            float aimAssist = _gameManager.AimAssist; // reducing cross script calls
            if (_gameManager.AllowAimAssist && (aimAssist > 0 || doFull))
            {
                _start_velocity = DoAimAssistVectoring(_start_velocity, transform.position, doFull?1000:aimAssist);
                _startVelocityLocal = _start_velocity;
            }

            _gameManager.Players[CurrentPlayer].LastAimAssistStrength = aimAssist;
            _gameManager.SetLastAimAssist(aimAssist);

            SetState(BallState.Flying);
            RequestSerialization();

            _gameManager.CountThrow(CurrentPlayer);
        }

        public void ShootAI(float skill)
        {
            // calculate rough velocity to throw ball in that direction
            _start_velocity = _gameManager.GetAIThrowVector(transform.position, CurrentPlayer) * MAX_THROW_STRENGTH * _throwVelocityMultiplierDekstop;

            _start_position = transform.position;
            _start_rotation = transform.rotation;
            _startPositionLocal = _start_position;
            _startVelocityLocal = _start_velocity;
            _physicsSimulationStartTime = Time.time;

            _pickup.pickupable = false;
            SetStaticCollisions();
            _throwIndicator.gameObject.SetActive(false);

            _start_velocity = DoAIVectoring(_start_velocity, transform.position, skill);
            _startVelocityLocal = _start_velocity;

            SetState(BallState.Flying);
            RequestSerialization();
        }

        bool _isStatic = true;
        void SetStatic()
        {
            _rigidbody.isKinematic = true;
            _isStatic = true;
        }

        void SetStaticCollisions()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.drag = float.MaxValue;
            _rigidbody.angularDrag = float.MaxValue;
            _rigidbody.useGravity = false;
            _isStatic = true;
        }

        void SetDynamic()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.drag = 0;
            _rigidbody.angularDrag = 0.5f;
            _rigidbody.useGravity = true;
            _isStatic = false;
        }

        //=========Prediction==========

        private Vector3 PositionAtTime(Vector3 start, Vector3 startVelocity, float time)
        {
            float y = start.y + Physics.gravity.y * time * time / 2 + startVelocity.y * time;
            start = start + startVelocity * time;
            start.y = y;
            return start;
        }

        private Vector3 VelocityAtTime(Vector3 startVelocity, float time)
        {
            startVelocity.y = Physics.gravity.y * time + startVelocity.y;
            return startVelocity;
        }

        private Vector3 DoAimAssistVectoring(Vector3 velocity, Vector3 position, float strength)
        {
            if (strength == 0) return velocity;
            //Get Point where ball will hit table
            float predictedTableHitTime = PredictTableHitTime(velocity, position, tableHeight);
            Vector3 predictionTableHit = GetTableHitPosition(velocity, position, tableHeight, predictedTableHitTime);
            Cup cup = GetClosestGlassToPredictedTableCollision(predictionTableHit);
            if (cup == null) return velocity;
            Vector3 cupOpening = cup.transform.position + Vector3.up * (cup.GlobalHeight + _radius);
            Vector3 hitOnCupPlane = PredictTableHit(velocity, position, cupOpening.y);
            // If hit not in range, check if ball will bounce close to cup
            if (Vector3.Distance(cupOpening, hitOnCupPlane) > strength)
            {
                // Calculate the position of the first bounce
                Vector3 reflectVel = Vector3.Reflect(VelocityAtTime(velocity, predictedTableHitTime), Vector3.up) * BOUND_DRAG;
                float predictedCupHeightTime2 = PredictTableHitTime(reflectVel, predictionTableHit, cupOpening.y);
                Vector3 predictionCupHeightPosition2 = GetTableHitPosition(reflectVel, predictionTableHit, cupOpening.y, predictedCupHeightTime2);
                // Get new closest cup
                cup = GetClosestGlassToPredictedTableCollision(predictionCupHeightPosition2);
                cupOpening = cup.transform.position + Vector3.up * (cup.GlobalHeight + _radius);
                // If first bounce is close to cup, aim for that
                if (Vector3.Distance(cupOpening, predictionCupHeightPosition2) < strength)
                {
                    Vector3 correctDirection = OptimalVectorJustChangeY(velocity, position, cup);
                    // Adjust xz velocity to hit cup
                    float xzThrowDistance = XZDistance(position, predictionCupHeightPosition2);
                    float targetXZDistance = XZDistance(position, cupOpening);
                    float multiplier = targetXZDistance / xzThrowDistance;
                    correctDirection.x *= multiplier;
                    correctDirection.z *= multiplier;
                    return correctDirection;
                }
                //Check if ball close to going past, if so adjust angle for bounce aim assist
                float distance = Vector3.Cross(new Vector3(velocity.x,0, velocity.z).normalized, new Vector3(cupOpening.x,0,cupOpening.z) - new Vector3(position.x, 0, position.z)).magnitude;
                if (distance < strength) return OptimalVectorJustChangeY(velocity, position, cup);
                return velocity;
            }
            return OptimalVectorChangeStrength(velocity, position, cup);
        }

        private float XZDistance(Vector3 a, Vector3 b)
        {
            a.y = 0;
            b.y = 0;
            return Vector3.Distance(a, b);
        }

        private Vector3 DoAIVectoring(Vector3 velocity, Vector3 position, float skill)
        {
            if (skill == 0) return velocity;
            //Get Point where ball will hit table
            Vector3 predictionTableHit = PredictTableHit(velocity, position, tableHeight);
            Cup cup = GetClosestGlassToPredictedTableCollision(predictionTableHit);
            if (cup == null) return velocity;
            Vector3 optimalVector = OptimalVectorChangeStrength(velocity, position, cup);

            float lerping = UnityEngine.Random.Range(0f, 1f);
            if (lerping < skill) lerping = 1; // hit
            else lerping = (1 - lerping) * 0.85f; // miss
            velocity = Vector3.Lerp(velocity, optimalVector, lerping);

            return velocity;
        }

        private Vector3 OptimalVectorChangeStrength(Vector3 velocity, Vector3 position, Cup cup)
        {
            Vector3 cupOpening = cup.transform.position + Vector3.up * (cup.GlobalHeight + _radius);
            // Debug.DrawLine(position, cupOpening, Color.red, 10);
            Vector3 horizonzalVector = cupOpening - position;
            horizonzalVector.y = 0;
            Vector3 horizonzalVelocity = velocity;
            horizonzalVelocity.y = 0;
            //https://www.youtube.com/watch?v=mOYJKv22qeQ&list=PLX2gX-ftPVXUUlf-9Eo_6ut4kP6wKaSWh&index=7&ab_channel=MichelvanBiezen
            // d = v0 * cos(w) * t
            // => t = d / (v0 * cos(w))
            // y = y0 + v0 * sin(w) * t - 0.5 * g * t2
            // y = y0 + v0 * sin(w) * ( d / (v0 * cos(w)) ) - 0.5 * g * ( d / (v0 * cos(w)) )2
            // y = y0 + tan(w) * d - 0.5 * g * ( d / (v0 * cos(w)) )2
            // y0 - y + tan(w) * d = 0.5 * g * ( d / (v0 * cos(w)) )2
            // 2(y0 - y + tan(w) * d) / g = ( d / (v0 * cos(w)) )2
            // sqrt( 2(y0 - y + tan(w) * d) / g) = d / (v0 * cos(w))
            // v0 * cos(w) = d / sqrt( 2(y0 - y + tan(w) * d) / g)
            // v0 = d / sqrt( 2(y0 - y + tan(w) * d) / g) / cos(w)
            float d = horizonzalVector.magnitude;
            float y0 = position.y;
            float y = cupOpening.y;
            float w = Mathf.Deg2Rad * Vector3.Angle(horizonzalVelocity, velocity);
            float g = -Physics.gravity.y;

            float v0 = d / Mathf.Sqrt(2 * (y0 - y + Mathf.Tan(w) * d) / g) / Mathf.Cos(w);
            if (float.IsNaN(v0)) return velocity;

            Vector3 newDirection = horizonzalVector.normalized * horizonzalVelocity.magnitude;
            newDirection.y = velocity.y;

            return newDirection.normalized * v0;
        }

        private Vector3 OptimalVectorJustChangeY(Vector3 velocity, Vector3 position, Cup cup)
        {
            Vector3 horizonzalVector = cup.transform.position - position;
            horizonzalVector.y = 0;
            Vector3 horizonzalVelocity = velocity;
            horizonzalVelocity.y = 0;
            Vector3 newDirection = horizonzalVector.normalized * horizonzalVelocity.magnitude;
            newDirection.y = velocity.y;
            return newDirection;
        }

        private Vector3 OptimalVectorChangeAngle(Vector3 velocity, Vector3 position, float tableHeight, Cup aimedCup)
        {
            //Calculate trajectory to that cup
            Vector3 horizonzalDistance = aimedCup.transform.position - position;
            horizonzalDistance.y = 0;
            //https://www.youtube.com/watch?v=bqYtNrhdDAY&ab_channel=MichelvanBiezen
            //https://www.youtube.com/watch?v=pQ23Eb-bXvQ&ab_channel=MichelvanBiezen
            float g = -Physics.gravity.y;
            float v2 = velocity.sqrMagnitude;
            float x = horizonzalDistance.magnitude;
            float h = position.y - (tableHeight + aimedCup.GlobalHeight);
            float phi = Mathf.Atan(x / h);
            float theta = (Mathf.Acos(((g * x * x / v2) - h) / Mathf.Sqrt(h * h + x * x)) + phi) / 2;

            Vector3 newVel = (horizonzalDistance.normalized * Mathf.Cos(theta) + Vector3.up * Mathf.Sin(theta)) * velocity.magnitude;
            if (float.IsNaN(theta))
            {
                //cant reacht cup with current velocity
                //just aim in it's direction instead
                Vector3 horizontalVel = velocity;
                horizontalVel.y = 0;
                newVel = horizonzalDistance.normalized * horizontalVel.magnitude;
                newVel.y = velocity.y;
            }
            return newVel;
        }

        private Cup GetClosestGlassToPredictedTableCollision(Vector3 prediction)
        {
            //Get Cup closest to that point
            Cup aimedCup = null;
            float closestDistance = float.MaxValue;
            bool disallowOwnCups = _gameManager.ShouldAimAssistAimForOwnCups() == false;
            bool disallowOwnSide = _gameManager.ShouldAimAssistAimAtOwnSide() == false;
            for (int p = 0; p < _gameManager.PlayerCount; p++)
            {
                float d = 0;
                int length = _gameManager.Players[p].CupsManager.Length;
                Cup[] cups = _gameManager.Players[p].CupsManager.List;
                for(int i = 0; i < length; i++)
                {
                    Cup cup = cups[i];
                    if (cup == null) continue;
                    if (disallowOwnCups && cup.PlayerCupOwner.PlayerIndex == CurrentPlayer) continue;
                    if (disallowOwnSide && cup.PlayerAnchorSide.PlayerIndex == CurrentPlayer) continue;
                    d = Vector3.Distance(cup.transform.position, prediction);
                    if (d < closestDistance)
                    {
                        closestDistance = d;
                        aimedCup = cup;
                    }
                }
            }
            return aimedCup;
        }

        private Vector3 PredictTableHit(Vector3 orignVelocity, Vector3 orignPosition, float tableHeight)
        {
            float t = PredictTableHitTime(orignVelocity, orignPosition, tableHeight);
            return GetTableHitPosition(orignVelocity, orignPosition, tableHeight, t);
        }

        private Vector3 GetTableHitPosition(Vector3 orignVelocity, Vector3 orignPosition, float tableHeight, float t)
        {
            return new Vector3(orignPosition.x + orignVelocity.x * t, tableHeight + _radius, orignPosition.z + orignVelocity.z * t);
        }

        private float PredictTableHitTime(Vector3 orignVelocity, Vector3 orignPosition, float tableHeight)
        {
            // Solve quadratic equation: c0*x^2 + c1*x + c2. 
            float c0 = Physics.gravity.y / 2;
            float c1 = orignVelocity.y;
            float c2 = orignPosition.y - (tableHeight + _radius);
            float[] quadraticOut = new float[2];
            int quadratic = SolveQuadric(c0, c1, c2, quadraticOut);
            float t = 0;
            if (quadratic == 1) t = quadraticOut[0];
            else if(quadratic == 2 && quadraticOut[0] > 0 && quadraticOut[0] > quadraticOut[1]) t = quadraticOut[0];
            else if(quadratic == 2 && quadraticOut[1] > 0) t = quadraticOut[1];
            return t;
        }

        public int SolveQuadric(float c0, float c1, float c2, float[] s)
        {
            float p, q, D;

            /* normal form: x^2 + px + q = 0 */
            p = c1 / (2 * c0);
            q = c2 / c0;

            D = p * p - q;

            if (D == 0)
            {
                s[0] = -p;
                return 1;
            }
            else if (D < 0)
            {
                return 0;
            }
            else /* if (D > 0) */
            {
                float sqrt_D = Mathf.Sqrt(D);

                s[0] = sqrt_D - p;
                s[1] = -sqrt_D - p;
                return 2;
            }
        }

        //=========Collision Code======

        private void ResetLastCupsColliders()
        {
            Cup cup = _gameManager.GetCup(_state[1], _state[2], _state[3]);
            if (Utilities.IsValid(cup))
            {
                cup.ColliderForThrow.enabled = true;
                cup.ColliderInside.SetActive(false);
            }
        }

        public void Respawn()
        {
            SetVelocity(Vector3.zero);
            SetAngularVelocity(Vector3.zero);
            SetStatic();
            SetState(BallState.Idle);
            transform.position = _gameManager.GetBallSpawn(CurrentPlayer).position;
            transform.rotation = _gameManager.GetBallSpawn(CurrentPlayer).rotation;
            _start_position = transform.position;
            _start_rotation = transform.rotation;
            _pickup.pickupable = true;
            _gameManager.Players[CurrentPlayer].ItsYourTurn(this);
            ResetLastCupsColliders();
            RequestSerialization();
        }

        public void SendItsYourTurn()
        {
            _gameManager.Players[CurrentPlayer].ItsYourTurn(this);
        }

        private void NextTeam()
        {
            CurrentPlayer = _gameManager.GetNextPlayer(CurrentPlayer);
            if(CurrentPlayer >= _gameManager.PlayerCount)
            {
                CurrentPlayer = 0;
            }
            _SetColor();
            RequestSerialization();
        }

        public void _SetColor()
        {
            _renderer.material.color = _gameManager.Players[CurrentPlayer].ActiveColor;
            _gameManager.SetActivePlayerColor(_renderer.material.color);
        }

        const float COLLISION_MAXIMUM_DISTANCE_FROM_CENTER = 0.95f;

        private void BallGlassCollision(Collision collision, Cup hitGlass)
        {
            Bounds b = hitGlass.GetBounds();
            bool isCollisionOnTop = (b.max.y < transform.position.y);

            SetDynamic();

            if (isCollisionOnTop)
            {
                Vector3 p1 = transform.position;
                p1.y = 0;
                Vector3 p2 = hitGlass.transform.position;
                p2.y = 0;
                float distanceFromCenter = Vector3.Distance(p1, p2) / hitGlass.GlobalRadius;

                if (distanceFromCenter < COLLISION_MAXIMUM_DISTANCE_FROM_CENTER)
                {
                    float angle = Vector3.Angle(_lastvelocies[_lastvelociesHead], Vector3.down);
                    //bool tripOnEdge = distanceFromCenter > Random.Range(0, 0.85f);
                    bool tripOnEdge = distanceFromCenter > UnityEngine.Random.Range(0.4f,0.7f) && angle > UnityEngine.Random.Range(45, 80); //if it lands really flat chances are higher for it to ride edge

                    if (tripOnEdge)
                    {
                        SetState(BallState.Rimming);
                        _state[1] = (int)hitGlass.PlayerAnchorSide.PlayerIndex;
                        _state[2] = (int)hitGlass.Row;
                        _state[3] = (int)hitGlass.Column;
                        _rimming = Vector3.Angle(hitGlass.transform.rotation * Vector3.forward, p2 - p1);
                        if ((p2 - p1).x > 0) _rimming = 360-_rimming;
                        _rimming = _rimming % 360;
                        _state[4] = (int)(_rimming / 2);
                        _state[5] = (int)(distanceFromCenter * 100);
                    }
                    else
                    {
                        hitGlass.ColliderForThrow.enabled = false;
                        hitGlass.ColliderInside.SetActive(true);
                        _pickup.pickupable = !_gameManager.DoAutoRespawnBalls;
                        SetState(BallState.InCup);
                        _state[1] = (int)hitGlass.PlayerAnchorSide.PlayerIndex;
                        _state[2] = (int)hitGlass.Row;
                        _state[3] = (int)hitGlass.Column;

                        _TriggerSplash();
                        SetVelocity(Vector3.down * _startVelocityLocal.magnitude);
                    }
                    RequestSerialization();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider == null || collision.collider.gameObject == null) return;

            bool hasHitGlass = collision.collider.gameObject.GetComponent<Cup>() != null;
            if (hasHitGlass)
            {
                PlayAudio(_audio_collision_glass, _startVelocityLocal.sqrMagnitude / 10);
            }
            else
            {
                PlayAudio(_audio_collision_table, _startVelocityLocal.sqrMagnitude / 10);
            }

            _startPositionLocal = transform.position;
            _startVelocityLocal = Vector3.Reflect(VelocityAtTime(_startVelocityLocal, Time.time - _physicsSimulationStartTime), Vector3.up) * BOUND_DRAG;
            _physicsSimulationStartTime = Time.time;

            if (Networking.IsOwner(gameObject))
            {
                if(_state[0] == (int)BallState.Flying)
                {
                    if (hasHitGlass)
                    {
                        Cup hitGlass = collision.collider.gameObject.GetComponent<Cup>();
                        //check if collision with own glass and if friendly fire is turned on
                        if (_gameManager.AllowCollision(CurrentPlayer, hitGlass.PlayerCupOwner, hitGlass.PlayerAnchorSide))
                        {
                            BallGlassCollision(collision, hitGlass);
                        }
                    }
                    //none beer pong object => miss
                    else if (collision.collider.gameObject.name.StartsWith("[ThryBP]") == false)
                    {
                        Debug.Log("[ThryBP] Ball hit non game object. Respawning for other team.");
                        NextTeam();
                        Respawn();
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider == null || collider.gameObject == null) return;

            if (_localState == BallState.Rimming && collider.gameObject.GetComponent<HandCollider>() != null)
            {
                HandCollider hand = collider.gameObject.GetComponent<HandCollider>();
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                SetState(BallState.Flying);
                SetVelocity(hand._velocity);

                _start_velocity = _rigidbody.velocity;
                _start_position = transform.position;
                _start_rotation = transform.rotation;
                RequestSerialization();
            }
        }

        private void SetVelocity(Vector3 velocity)
        {
            if(_rigidbody.isKinematic) return;
            _rigidbody.velocity = velocity;
        }

        private void SetAngularVelocity(Vector3 velocity)
        {
            if(_rigidbody.isKinematic) return;
            _rigidbody.angularVelocity = velocity;
        }
    }
}
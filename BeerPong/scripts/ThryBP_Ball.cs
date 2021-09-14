
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.BeerPong
{
    public class ThryBP_Ball : UdonSharpBehaviour
    {
        public Transform throwIndicator;
        public float velocityMultiplier = 1.5f;

        [HideInInspector]
        public bool autoRespawnBallAfterCupHit = true;

        [HideInInspector]
        public Transform respawnHeight;

        public VRC_Pickup _pickup;
        Rigidbody _rigidbody;
        Renderer _renderer;

        public ParticleSystem _splashParticles;
        private AudioSource _splashAudio;

        [HideInInspector]
        public ThryBP_Main _mainScript;

        [UdonSynced]
        byte[] state = new byte[] { 0 , 0 , 0 , 0 , 0, 0};
        byte localState = 0;
        float stateStartTime = 0;

        const byte STATE_IDLE = 0;
        const byte STATE_IN_HAND = 1;
        const byte STATE_LOCKED = 2;
        const byte STATE_FYING = 3;
        const byte STATE_RIMING = 4;
        const byte STATE_IN_CUP = 5;

        [UdonSynced]
        int currentPlayer = 0;

        [UdonSynced]
        Vector3 s_position;
        [UdonSynced]
        Quaternion s_rotation;
        [UdonSynced]
        bool isRightHand;
        [UdonSynced]
        Vector3 velocity;

        Vector3 velocityLocal;

        float timeStartBeingStill = 0;

        float rimming = 0;
        const float RIMMING_SPEED = 1300;

        float _radius = 1;

        Vector3 _lastPosition;
        Vector3 _velocity;

        [Header("Sounds")]
        public AudioClip audio_collision_table;
        public AudioClip audio_collision_glass;
        public AudioClip audio_water_splash;

        private AudioSource _audioSource;

        public void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _renderer = GetComponent<Renderer>();
            _audioSource = GetComponent<AudioSource>();
            _splashAudio = _splashParticles.transform.GetComponent<AudioSource>();

            _rigidbody.isKinematic = true;
            _radius = (_renderer.bounds.extents.x + _renderer.bounds.extents.y + _renderer.bounds.extents.z) / 3;

            SetColor();
            if (Networking.IsOwner(gameObject))
            {
                s_position = transform.position;
                s_rotation = transform.rotation;
                RequestSerialization();
            }
            GetComponent<Collider>().enabled = true;
        }

        private void PlayAudio(AudioClip clip, float strength)
        {
            if(clip != null) _audioSource.PlayOneShot(clip, strength);
        }

        public override void OnDeserialization()
        {
            _pickup.pickupable = state[0] == STATE_IDLE || (state[0] == STATE_IN_CUP && !autoRespawnBallAfterCupHit);
            SetColor();

            if (state[0] == STATE_IDLE)
            {
                _rigidbody.isKinematic = true;
                transform.position = s_position;
                transform.rotation = s_rotation;
                velocityLocal = Vector3.zero;
            }
            else if (state[0] == STATE_IN_HAND)
            {
                throwIndicator.gameObject.SetActive(false);
            }
            else if (state[0] == STATE_LOCKED)
            {
                transform.position = s_position;
                transform.rotation = s_rotation;
                throwIndicator.SetPositionAndRotation(transform.position, transform.rotation);
                throwIndicator.gameObject.SetActive(true);
            }
            else if (state[0] == STATE_FYING)
            {
                if (velocityLocal != velocity)
                {
                    transform.position = s_position;
                    transform.rotation = s_rotation;
                    _rigidbody.velocity = velocity;
                    _rigidbody.angularVelocity = Vector3.zero;
                    _rigidbody.isKinematic = false;
                    velocityLocal = velocity;
                    throwIndicator.gameObject.SetActive(false);
                }
            }else if(state[0] == STATE_RIMING)
            {
                rimming = state[4] * 2;
            }else if(state[0] == STATE_IN_CUP)
            {
                ThryBP_Glass cup = _mainScript.GetCup(state[1], state[2], state[3]);
                if (Utilities.IsValid(cup))
                {
                    cup.colliderForThrow.enabled = false;
                    cup.colliderInside.SetActive(true);
                }
            }

            if (state[0] != localState)
            {
                localState = state[0];
                stateStartTime = Time.time;

                if(localState == STATE_IDLE || localState == STATE_FYING)
                {
                    ResetLastCupsColliders();
                }
                if(localState == STATE_IN_CUP)
                {
                    _TriggerSplash();
                }
            }
        }

        private void _TriggerSplash()
        {
            _splashParticles.transform.position = transform.position;
            _splashParticles.Emit(20);
            PlayAudio(audio_water_splash, 1);
        }

        const float RIMMING_TOTAL_AGULAR_ROTATION = 3000;
        float lastVelocity;

        private void PostLateUpdate()
        {
            _velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            if (localState == STATE_IN_HAND)
            {
                //move ball to hand of player holding it
                if (!Networking.IsOwner(gameObject))
                {
                    if (isRightHand)
                    {
                        Quaternion q = Networking.GetOwner(gameObject).GetBoneRotation(HumanBodyBones.RightHand);
                        transform.SetPositionAndRotation(Networking.GetOwner(gameObject).GetBonePosition(HumanBodyBones.RightHand) + q * s_position,
                             q * s_rotation);
                    }
                    else
                    {
                        Quaternion q = Networking.GetOwner(gameObject).GetBoneRotation(HumanBodyBones.LeftHand);
                        transform.SetPositionAndRotation(Networking.GetOwner(gameObject).GetBonePosition(HumanBodyBones.LeftHand) + q * s_position,
                            q * s_rotation);
                    }
                }
            }
            else if (localState == STATE_FYING)
            {
                //respawn ball for other team if falls off table
                if (Networking.IsOwner(gameObject))
                {
                    if (transform.position.y < respawnHeight.position.y)
                    {
                        Debug.Log("[ThryBP] Ball below respawn point. Respawning for other team.");
                        NextTeam();
                        Respawn();
                    }
                    if(_rigidbody.velocity.sqrMagnitude > 0.1f)
                    {
                        timeStartBeingStill = Time.time;
                    }
                    else if(Time.time - timeStartBeingStill > 1.0f && Time.time - stateStartTime > 1.0f)
                    {
                        Debug.Log("[ThryBP] Ball idle for more than 1 second. Respawning for other team.");
                        NextTeam();
                        Respawn();
                    }
                }
                lastVelocity = _rigidbody.velocity.magnitude;
            }
            else if (localState == STATE_RIMING)
            {
                if (rimming < RIMMING_TOTAL_AGULAR_ROTATION)
                {
                    ThryBP_Glass cup = _mainScript.GetCup(state[1], state[2], state[3]);
                    if (Utilities.IsValid(cup))
                    {
                        rimming += RIMMING_SPEED * Time.deltaTime;

                        float inside = 0.9f - rimming / RIMMING_TOTAL_AGULAR_ROTATION * 0.15f;
                        float downwards = 1 - rimming / RIMMING_TOTAL_AGULAR_ROTATION * 0.3f;
                        float angle = (rimming - Mathf.Pow(rimming * 0.009f, 2)) / cup.circumfrence * 0.4f;
                        transform.position = cup.transform.position + Vector3.up * (cup.GetBounds().size.y * downwards)
                            + Quaternion.Euler(0, angle, 0) * Vector3.forward * (cup.radius * cup.transform.lossyScale.x * inside - _radius);
                    }
                }
                else
                {
                    ThryBP_Glass cup = _mainScript.GetCup(state[1], state[2], state[3]);
                    if (Utilities.IsValid(cup))
                    {
                        cup.colliderInside.SetActive(true);
                        cup.colliderForThrow.enabled = false;
                        _rigidbody.velocity = Quaternion.Euler(0, 90 + rimming, 0) * Vector3.forward * 0.3f; //in circle
                        _rigidbody.velocity += Quaternion.Euler(0, rimming + 180, 0) * Vector3.forward * 0.1f; //inwards
                        _rigidbody.velocity += Vector3.down * 0.1f; //downwards
                        localState = STATE_IN_CUP;
                        stateStartTime = Time.time;
                        _TriggerSplash();
                        _pickup.pickupable = !autoRespawnBallAfterCupHit;
                    }
                }
            }else if(localState == STATE_IN_CUP)
            {
                ThryBP_Glass cup = _mainScript.GetCup(state[1], state[2], state[3]);
                if (Utilities.IsValid(cup))
                {
                    
                    //if ball is not in cup teleport ball in cup
                    Collider c = cup.colliderForThrow;
                    c.enabled = true;
                    Vector3 cPoint = c.ClosestPoint(transform.position);
                    c.enabled = false;
                    Bounds b = cup.GetBounds();
                    b.size = b.size += Vector3.up;
                    if (Vector3.Distance(cPoint,transform.position) > 0.01f && ((_rigidbody.velocity.sqrMagnitude == 0) || (b.Contains(transform.position) == false)))
                    {
                        //Debug.Log($"cPoint: {cPoint.ToString("F3")} point: {transform.position.ToString("F3")}");
                        Debug.Log($"[ThryBP] Teleported ball into cup ({state[1]},{state[2]},{state[3]})");
                        transform.position = cup.GetBounds().center;
                    }
                    else if(Networking.IsOwner(gameObject))
                    {
                        if(_mainScript.gamemode == 0)
                        {
                            if(Time.time - stateStartTime > (autoRespawnBallAfterCupHit?2:20))
                            {
                                _mainScript.CountCupHit(state[1], state[2], state[3], currentPlayer, state[0] == STATE_RIMING?1:0);
                                _mainScript.RemoveCup(state[1], state[2], state[3], currentPlayer);
                                Respawn();
                            }
                        }else if (_mainScript.gamemode == 1)
                        {
                            if(Time.time - stateStartTime > 1)
                            {
                                _mainScript.CountCupHit(state[1], state[2], state[3], currentPlayer, state[0] == STATE_RIMING ? 1 : 0);
                                _mainScript.RemoveCup(state[1], state[2], state[3], currentPlayer);
                                Respawn();
                            }
                        }
                        else if (_mainScript.gamemode == 2)
                        {
                            if (Time.time - stateStartTime > 3)
                            {
                                _mainScript.CountCupHit(state[1], state[2], state[3], currentPlayer, state[0] == STATE_RIMING ? 1 : 0);
                                Respawn();
                            }
                        }
                    }
                }   
            }
        }

        void SetState(byte s)
        {
            state[0] = s;
            localState = s;
            stateStartTime = Time.time;
        }

        //Set ownership
        public override void OnPickup()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _rigidbody.isKinematic = false;
            SendCustomEventDelayedFrames(nameof(OnPickupDelayed), 1);

            if (localState == STATE_IN_CUP)
            {
                _mainScript.CountCupHit(state[1], state[2], state[3], currentPlayer, state[0] == STATE_RIMING ? 1 : 0);
                _mainScript.RemoveCup(state[1], state[2], state[3], currentPlayer);
                NextTeam();
            }
        }

        public void OnPickupDelayed()
        {
            EnableAndMoveIndicator();
        }

        private void SerializePositionRelativeToHand()
        {
            SendCustomEventDelayedFrames(nameof(SerializePositionRelativeToHandConcrete), 1);
            SendCustomEventDelayedSeconds(nameof(SerializePositionRelativeToHandConcrete), 1);
        }

        public void SerializePositionRelativeToHandConcrete()
        {
            if (state[0] != STATE_IN_HAND) return;
            isRightHand = _pickup.currentHand == VRC_Pickup.PickupHand.Right;
            if (isRightHand)
            {
                s_position = transform.position - Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightHand);
                Quaternion q = Quaternion.Inverse(Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.RightHand));
                s_rotation = q * transform.rotation;
                s_position = q * s_position;
            }
            else
            {
                s_position = transform.position - Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftHand);
                Quaternion q = Quaternion.Inverse(Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.LeftHand));
                s_rotation = q * transform.rotation;
                s_position = q * s_position;
            }
            RequestSerialization();
        }

        //Parent indicator
        public void EnableAndMoveIndicator()
        {
            throwIndicator.SetParent(transform);
            throwIndicator.localPosition = Vector3.zero;
            throwIndicator.localRotation = Quaternion.identity;
            SetState(STATE_IN_HAND);
            RequestSerialization();

            SerializePositionRelativeToHand();
        }

        public void LockIndicator()
        {
            throwIndicator.SetParent(null);
            throwIndicator.SetPositionAndRotation(transform.position, transform.rotation);
            SetState(STATE_LOCKED);
            s_position = transform.position;
            s_rotation = transform.rotation;
            RequestSerialization();
        }

        public override void OnPickupUseDown()
        {
            throwIndicator.gameObject.SetActive(true);
        }

        public override void OnPickupUseUp()
        {
            bool isLocked = throwIndicator.parent != transform;
            if (isLocked)
            {
                EnableAndMoveIndicator();
                throwIndicator.gameObject.SetActive(false);
            }
            else
            {
                LockIndicator();
            }
        }

        public override void OnDrop()
        {
            SendCustomEventDelayedFrames(nameof(OnDropDelayed), 1);
        }


        public void OnDropDelayed()
        {
            //Transfer the velocity to the selected vector
            if (state[0] == STATE_LOCKED)
            {
                transform.SetPositionAndRotation(throwIndicator.position, throwIndicator.rotation);
                _rigidbody.velocity = throwIndicator.rotation * Vector3.forward * _rigidbody.velocity.magnitude * velocityMultiplier;
            }
            //throw normally
            else
            {
                s_position = transform.position;
                s_rotation = transform.rotation;
            }
            _pickup.pickupable = false;
            _rigidbody.angularVelocity = Vector3.zero;
            throwIndicator.gameObject.SetActive(false);

            velocity = _rigidbody.velocity;
            SetState(STATE_FYING);
            RequestSerialization();
        }

        //=========Collision Code======

        private void ResetLastCupsColliders()
        {
            ThryBP_Glass cup = _mainScript.GetCup(state[1], state[2], state[3]);
            if (Utilities.IsValid(cup))
            {
                cup.colliderForThrow.enabled = true;
                cup.colliderInside.SetActive(false);
            }
        }

        public void Respawn()
        {
            if(currentPlayer >= _mainScript.playerCountSlider.value)
            {
                currentPlayer = 0;
                SetColor();
            }
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            transform.position = _mainScript.GetBallSpawn(currentPlayer).position;
            transform.rotation = _mainScript.GetBallSpawn(currentPlayer).rotation;
            s_position = transform.position;
            s_rotation = transform.rotation;
            _pickup.pickupable = true;
            SetState(STATE_IDLE);
            ResetLastCupsColliders();
            RequestSerialization();
        }

        private void NextTeam()
        {
            currentPlayer = _mainScript.GetNextPlayer(currentPlayer);
            SetColor();
            RequestSerialization();
        }

        private void SetColor()
        {
            _renderer.material.color = _mainScript.GetPlayerColor(currentPlayer);
            _mainScript.teamIndicatorMaterial.color = _renderer.material.color;
            _mainScript.teamIndicatorMaterial.SetColor("_EmissionColor",_renderer.material.color);
        }

        const float COLLISION_MAXIMUM_DISTANCE_FROM_CENTER = 0.95f;

        private void BallGlassCollision(Collision collision, ThryBP_Glass hitGlass)
        {
            Bounds b = hitGlass.GetBounds();
            bool isCollisionOnTop = (b.max.y < transform.position.y);

            if (isCollisionOnTop)
            {
                Vector3 p1 = transform.position;
                p1.y = 0;
                Vector3 p2 = hitGlass.transform.position;
                p2.y = 0;
                float distanceFromCenter = Vector3.Distance(p1, p2) / ((b.extents.x + b.extents.z) / 2);

                if (distanceFromCenter < COLLISION_MAXIMUM_DISTANCE_FROM_CENTER)
                {
                    float angle = Vector3.Angle(_velocity, Vector3.down);
                    //bool tripOnEdge = distanceFromCenter > Random.Range(0, 0.85f);
                    bool tripOnEdge = distanceFromCenter > Random.Range(0.4f,0.7f) && angle > Random.Range(45, 80); //if it lands really flat chances are higher for it to ride edge

                    if (tripOnEdge)
                    {
                        SetState(STATE_RIMING);
                        state[1] = (byte)hitGlass.player.playerIndex;
                        state[2] = (byte)hitGlass.row;
                        state[3] = (byte)hitGlass.collum;
                        rimming = Vector3.Angle(hitGlass.transform.rotation * Vector3.forward, p2 - p1);
                        if ((p2 - p1).x > 0) rimming = 360-rimming;
                        rimming = rimming % 360;
                        state[4] = (byte)(rimming / 2);
                        state[5] = (byte)(distanceFromCenter * 100);
                    }
                    else
                    {
                        hitGlass.colliderForThrow.enabled = false;
                        hitGlass.colliderInside.SetActive(true);
                        _pickup.pickupable = !autoRespawnBallAfterCupHit;
                        SetState(STATE_IN_CUP);
                        state[1] = (byte)hitGlass.player.playerIndex;
                        state[2] = (byte)hitGlass.row;
                        state[3] = (byte)hitGlass.collum;

                        _TriggerSplash();
                        _rigidbody.velocity = Vector3.down * _rigidbody.velocity.magnitude;
                    }
                    RequestSerialization();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider == null || collision.collider.gameObject == null) return;

            bool hasHitGlass = collision.collider.gameObject.GetComponent<ThryBP_Glass>() != null;
            if (hasHitGlass)
            {
                PlayAudio(audio_collision_glass, lastVelocity / 10);
            }
            else
            {
                PlayAudio(audio_collision_table, lastVelocity / 10);
            }

            if (Networking.IsOwner(gameObject))
            {
                if(state[0] == STATE_FYING)
                {
                    if (hasHitGlass)
                    {
                        ThryBP_Glass hitGlass = collision.collider.gameObject.GetComponent<ThryBP_Glass>();
                        //check if collision with own glass and if friendly fire is turned on
                        if (_mainScript.AllowCollision(currentPlayer, hitGlass.player))
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

            if (localState == STATE_RIMING && collider.gameObject.GetComponent<ThryBP_HandCollider>() != null)
            {
                ThryBP_HandCollider hand = collider.gameObject.GetComponent<ThryBP_HandCollider>();
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                SetState(STATE_FYING);
                _rigidbody.velocity = hand.GetSlapVector();

                velocity = _rigidbody.velocity;
                s_position = transform.position;
                s_rotation = transform.rotation;
                RequestSerialization();
            }
        }
    }
}
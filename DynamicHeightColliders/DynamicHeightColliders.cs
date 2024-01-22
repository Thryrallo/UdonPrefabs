
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DynamicHeightColliders : UdonSharpBehaviour
{
    float _uncorrectedHeight = 0;
    float _height = 0;
    float _viewpointToTop = 0;
    float[] _particalHeights = new float[7];
    int _nextPartical = 0;

    VRCPlayerApi _player;
    float playerHighestY = 0;
    float playerY;
    int _raycastMask;

    Collider[] _colliders = new Collider[100];
    Collider[] _immobilizedBy = new Collider[100];

    void Start()
    {
        _player = Networking.LocalPlayer;
        _raycastMask = LayerMask.GetMask("Default", "Environment");
    }

    public override void OnPlayerRespawn(VRCPlayerApi player)
    {
        // Reset all _colliders triggers
        for(int j = 0; j < _colliders.Length; j++)
        {
            if(_colliders[j] == null) continue;
            _colliders[j].isTrigger = false;
        }
        // Clear _colliders
        Array.Clear(_colliders, 0, _colliders.Length);
        // Clear _immobilizedBy
        Array.Clear(_immobilizedBy, 0, _immobilizedBy.Length);
        Mobilize();
    }
    void ColliderCausesImmobilitze(Collider c)
    {
       int currentlyImmoblilizing = Array.IndexOf(_immobilizedBy, c);
        if(currentlyImmoblilizing != -1) return;

        int free = Array.IndexOf(_immobilizedBy, null);
        if(free != -1)
        {
            Immobilize();
            Debug.Log($"{c.name} => Immobilize");
            _immobilizedBy[free] = c;
        }
    }

    void ColliderNotImmobolizing(Collider c)
    {
        int currentlyImmoblilizing = Array.IndexOf(_immobilizedBy, c);
        if(currentlyImmoblilizing != -1)
        {
            _immobilizedBy[currentlyImmoblilizing] = null;
            // is _immobilizedBy empty ? 
            bool empty = Array.IndexOf(_immobilizedBy, null) == 0;
            if(empty)
            {
                Array.Reverse(_immobilizedBy);
                empty = Array.IndexOf(_immobilizedBy, null) == 0;
                if(empty)
                {
                    Mobilize();
                    Debug.Log($"{c.name} => Mobilize");
                }
            }
        }
    }

    void Mobilize()
    {
        _player.SetWalkSpeed(_walkSpeed);
        _player.SetRunSpeed(_runSpeed);
        _player.SetJumpImpulse(_jumpPower);
        _player.SetStrafeSpeed(_straveSpeed);
    }

    float _walkSpeed = 0;
    float _runSpeed = 0;
    float _jumpPower = 0;
    float _straveSpeed = 0;
    void Immobilize()
    {
        _walkSpeed = _player.GetWalkSpeed();
        _runSpeed = _player.GetRunSpeed();
        _jumpPower = _player.GetJumpImpulse();
        _straveSpeed = _player.GetStrafeSpeed();
        _player.SetWalkSpeed(0);
        _player.SetRunSpeed(0);
        _player.SetJumpImpulse(0);
        _player.SetStrafeSpeed(0);
        // remove horizontal velocity && postive up velocity
        Vector3 vel = _player.GetVelocity();
        vel.x = 0;
        vel.z = 0;
        if(vel.y > 0) vel.y = 0;
        _player.SetVelocity(vel);
    }

    void Update() 
    {
        if(_player == null) return;
        transform.position = _player.GetPosition();
        CalcHeight();
        Vector3 head = _player.GetBonePosition(HumanBodyBones.Head);
        Vector3 neck = _player.GetBonePosition(HumanBodyBones.Neck);
        Vector3 topOfHead = head + (head - neck).normalized * _viewpointToTop;
        // Spherecast from the ground to the top of the head
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.25f, Vector3.up, 5, _raycastMask, QueryTriggerInteraction.Collide);
        Collider[] hitColliders = new Collider[hits.Length];
        for(int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if(hit.collider == null) continue;
            if(hit.collider.GetType() == typeof(MeshCollider) && ((MeshCollider)hit.collider).convex == false) continue; // ignore non convex colliders
            Collider other = hit.collider;
            hitColliders[i] = other;
            if(other.isTrigger)
            {
                // check if is currently dynamic
                int index = Array.IndexOf(_colliders, other);
                if(index != -1)
                {
                    if(playerHighestY > hit.point.y && // if player collides -> immobilize
                        other.Raycast(new Ray(transform.position, Vector3.up), out RaycastHit hit1, 6) ) // is player below collider not on the side
                    {
                        ColliderCausesImmobilitze(other);
                    }else
                    {
                        ColliderNotImmobolizing(other);
                    }
                }
            }else
            {
                if(playerHighestY < hit.point.y) // if player is below collider
                {
                    other.isTrigger = true;
                    int free = Array.IndexOf(_colliders, null);
                    if(free != -1)
                    {
                        _colliders[free] = other;
                    }
                }
            }
        }

        for(int i = 0; i < _colliders.Length; i++)
        {
            Collider c = _colliders[i];
            if(c == null) continue;
            if(Array.IndexOf(hitColliders, c) == -1)
            {
                c.isTrigger = false;
                _colliders[i] = null;
                ColliderNotImmobolizing(c);
            }
        }


        playerY = transform.position.y;
        playerHighestY = topOfHead.y;
    }

    void CalcHeight()
    {
        _uncorrectedHeight -= _particalHeights[_nextPartical];
        Vector3 bone1 = Vector3.zero;
        Vector3 bone2 = Vector3.zero;
        if(_nextPartical == 0)
        {
            bone1 = _player.GetBonePosition(HumanBodyBones.Head);
            bone2 = _player.GetBonePosition(HumanBodyBones.Neck);
        }else if(_nextPartical == 1)
        {
            bone1 = _player.GetBonePosition(HumanBodyBones.Neck);
            bone2 = _player.GetBonePosition(HumanBodyBones.Chest);
        }else if(_nextPartical == 2)
        {
            bone1 = _player.GetBonePosition(HumanBodyBones.Chest);
            bone2 = _player.GetBonePosition(HumanBodyBones.Spine);
        }else if(_nextPartical == 3)
        {
            bone1 = _player.GetBonePosition(HumanBodyBones.Spine);
            bone2 = _player.GetBonePosition(HumanBodyBones.Hips);
        }else if(_nextPartical == 4)
        {
            bone1 = _player.GetBonePosition(HumanBodyBones.Hips);
            bone2 = _player.GetBonePosition(HumanBodyBones.RightUpperLeg);
        }else if(_nextPartical == 5)
        {
            bone1 = _player.GetBonePosition(HumanBodyBones.RightUpperLeg);
            bone2 = _player.GetBonePosition(HumanBodyBones.RightLowerLeg);
        }else if(_nextPartical == 6)
        {
            bone1 = _player.GetBonePosition(HumanBodyBones.RightLowerLeg);
            bone2 = _player.GetBonePosition(HumanBodyBones.RightFoot);
        }
        if(bone1 == Vector3.zero || bone2 == Vector3.zero)
        {
            _particalHeights[_nextPartical] = 0;
        }else
        {
            _particalHeights[_nextPartical] = Vector3.Distance(bone1, bone2);
        }
        _uncorrectedHeight += _particalHeights[_nextPartical];
        _height = _uncorrectedHeight * 1.06f; // adjust for head size
        _viewpointToTop = _height * 0.085f;
        _nextPartical = (_nextPartical + 1) % _particalHeights.Length;
    }
}

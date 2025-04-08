
using System;
using UdonSharp;
using UnityEngine;

namespace Thry.Udon.BeerPong
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Cup : UdonSharpBehaviour
    {
        public MeshCollider ColliderForThrow;
        public GameObject ColliderInside;
        public Renderer BoundsRenderer;

        [SerializeField] Renderer[] _playerColorRenderers;
        [SerializeField] public int[] _playerColorMaterialIncicies;

        [NonSerialized] public Player PlayerCupOwner;
        [NonSerialized] public Player PlayerAnchorSide;
        [NonSerialized] public int Row;
        [NonSerialized] public int Column;
        [NonSerialized] public int Anchor_id;
        [NonSerialized] public Vector3 Position_offset;

        [NonSerialized] public float LocalRadius = -1;
        [NonSerialized] public float LocalDiameter = -1;
        [NonSerialized] public float LocalCircumfrence = -1;
        [NonSerialized] public float LocalHeight = -1;

        [NonSerialized] public Transform TableScaleTransform;

        public void UpdateColor(int index)
        {
            if (PlayerCupOwner.DoRandomColor)
            {
                UnityEngine.Random.InitState(index);
                SetColor(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
            }
            else
            {
                SetColor(PlayerCupOwner.ActiveColor);
            }
        }

        public void SetColor(Color c)
        {
            for(int i=0;i< _playerColorRenderers.Length; i++)
            {
                _playerColorRenderers[i].materials[_playerColorMaterialIncicies[i]].color = c;
            }
        }

        public Bounds GetBounds()
        {
            return BoundsRenderer.bounds;
        }

        public float GlobalHeight
        {
            get
            {
                return LocalHeight * TableScaleTransform.lossyScale.y;
            }
        }

        public float GlobalRadius
        {
            get
            {
                return LocalRadius * TableScaleTransform.lossyScale.x;
            }
        }

        public float GlobalDiameter
        {
            get
            {
                return LocalDiameter * TableScaleTransform.lossyScale.x;
            }
        }

        public float GlobalCircumfrence
        {
            get
            {
                return LocalCircumfrence * TableScaleTransform.lossyScale.x;
            }
        }

        public void InitPrefab()
        {
            // Get the bounds with the rotation set to 0
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.identity;

            Bounds bounds = GetBounds();

            transform.rotation = rot;

            float radius = (bounds.extents.x + bounds.extents.z) / 2;
            float height = bounds.extents.y * 2;

            LocalRadius = radius / transform.lossyScale.x;
            LocalHeight = height / transform.lossyScale.y;

            LocalDiameter = 2 * LocalRadius;
            LocalCircumfrence = Mathf.PI * LocalHeight;
        }
    }
}
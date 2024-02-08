
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.BeerPong
{
    public class ThryBP_Glass : UdonSharpBehaviour
    {
        [HideInInspector]
        public ThryBP_Player PlayerCupOwner;
        [HideInInspector]
        public ThryBP_Player PlayerAnchorSide;
        [HideInInspector]
        public int Row;
        [HideInInspector]
        public int Column;
        [HideInInspector]
        public int Anchor_id;
        [HideInInspector]
        public Vector3 Position_offset;

        public MeshCollider colliderForThrow;
        public GameObject colliderInside;
        public Renderer boundsRenderer;

        public Renderer[] _playerColorRenderers;
        public int[] playerColorMaterialIncicies;

        public float LocalRadius = -1;
        public float LocalDiameter = -1;
        public float LocalCircumfrence = -1;
        public float LocalHeight = -1;

        [HideInInspector]
        public Transform TableScaleTransform;

        public void UpdateColor(int index)
        {
            if (PlayerCupOwner.DoRandomCupColor)
            {
                UnityEngine.Random.InitState(index);
                SetColor(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
            }
            else
            {
                SetColor(PlayerCupOwner.playerColor);
            }
        }

        public void SetColor(Color c)
        {
            for(int i=0;i< _playerColorRenderers.Length; i++)
            {
                _playerColorRenderers[i].materials[playerColorMaterialIncicies[i]].color = c;
            }
        }

        public Bounds GetBounds()
        {
            return boundsRenderer.bounds;
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
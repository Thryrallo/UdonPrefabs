
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.BeerPong
{
    public class ThryBP_Glass : UdonSharpBehaviour
    {
        [HideInInspector]
        public ThryBP_Player player;
        [HideInInspector]
        public int row;
        [HideInInspector]
        public int collum;
        [HideInInspector]
        public int anchor_id;
        [HideInInspector]
        public Vector3 position_offset;

        public MeshCollider colliderForThrow;
        public GameObject colliderInside;
        public Renderer boundsRenderer;

        public Renderer[] _playerColorRenderers;
        public int[] playerColorMaterialIncicies;

        public float Radius = -1;
        public float Diameter = -1;
        public float Circumfrence = -1;
        public float Height = -1;

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

        public void InitPrefab()
        {
            // Get the bounds of normal scaled & rotated cup
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            Vector3 scale = transform.localScale;
            Transform parent = transform.parent;
            transform.SetParent(null);
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            Bounds bounds = GetBounds();

            transform.SetParent(parent);
            transform.position = pos;
            transform.rotation = rot;
            transform.localScale = scale;

            Radius = (bounds.extents.x + bounds.extents.z) / 2;
            Radius = Radius / ((transform.lossyScale.x + transform.lossyScale.z) / 2);
            Height = bounds.extents.y * 2 / transform.lossyScale.y;

            Diameter = 2 * Radius;
            Circumfrence = Mathf.PI * Diameter;
        }
    }
}
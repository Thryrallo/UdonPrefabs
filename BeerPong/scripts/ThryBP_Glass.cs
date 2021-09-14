
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

        public float radius = -1;
        public float diameter = -1;
        public float circumfrence = -1;

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
            radius = (GetBounds().extents.x + GetBounds().extents.z) / 2;
            radius = radius / ((transform.lossyScale.x + transform.lossyScale.z) / 2);

            diameter = 2 * radius;
            circumfrence = Mathf.PI * diameter;
        }
    }
}

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.BeerPong
{
    public class ThryBP_Player : UdonSharpBehaviour
    {
        public ThryBP_CupsSpawn cups;

        public Transform ballSpwanpoint;

        public Transform ui;

        public Transform uiButton;
        private Material uiButtonMaterial;

        public UnityEngine.UI.Image[] uiImagesUsingPlayerColor;

        [UdonSynced]
        public Color playerColor;
        [HideInInspector]
        public Color ogColor;

        [UdonSynced]
        [HideInInspector]
        public bool randomColor = false;

        [HideInInspector]
        public int playerIndex;

        [HideInInspector]
        public ThryBP_Main _mainScript;

        public void Init(Transform[] anchors)
        {
            ogColor = playerColor;
            uiButtonMaterial = uiButton.GetComponent<Renderer>().material;
            uiButtonMaterial.color = playerColor;
            foreach (UnityEngine.UI.Image i in uiImagesUsingPlayerColor) i.color = playerColor;
            cups.Init(anchors);
            if (Networking.IsOwner(gameObject)) RequestSerialization();
        }

        public override void OnDeserialization()
        {
            cups.SyncColor();
            uiButtonMaterial.color = playerColor;
            foreach (UnityEngine.UI.Image i in uiImagesUsingPlayerColor) i.color = playerColor;
        }

        public void ResetGlasses()
        {
            cups.ResetGlasses();
        }

        public void RemoveCup(int row, int collum)
        {
            cups.RemoveCup(row, collum);
        }

        public void AddToScore(int a)
        {
            cups.AddToScore(a);
        }

        public void SetColor(Color c)
        {
            if (playerColor != c)
            {
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                playerColor = c;
                OnDeserialization();
                RequestSerialization();
            }
        }

        public void SetRandomColor(bool r)
        {
            if(randomColor != r)
            {
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                randomColor = r;
                OnDeserialization();
                RequestSerialization();
            }
        }
    }
}
 
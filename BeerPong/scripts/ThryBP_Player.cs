
using Thry.General;
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

        public Transform aiBallSpawn;

        public UnityEngine.UI.Image[] uiImagesUsingPlayerColor;

        public bool _isAI;
        public float _aiSkill = 0.3f;
        public ThryAction AiSkillSlider;

        public bool UseAdaptiveAISkill = false;

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

        [UdonSynced] int _numberOfThrows = 0;
        [UdonSynced] int _numberOfHits = 0;

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
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _numberOfHits = 0;
            _numberOfThrows = 0;
            RequestSerialization();
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

        public void AddThrow()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _numberOfThrows++;
            RequestSerialization();
        }

        public void AddHit()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _numberOfHits++;
            RequestSerialization();
        }

        public float GetSkill()
        {
            if(_isAI) return _aiSkill;
            if(_numberOfThrows == 0) return 0.5f;
            return (float)_numberOfHits / (float)_numberOfThrows;
        }

        private ThryBP_Ball currentBall;
        public void ItsYourTurn(ThryBP_Ball ball)
        {
            currentBall = ball;
            if (_isAI)
            {
                if(UseAdaptiveAISkill) AiSkillSlider.SetFloat(_mainScript.GetAdaptiveAIStrength(playerIndex));
                SendCustomEventDelayedSeconds(nameof(AI_AIM), 2);
            }else
            {
                _mainScript.UpdateAdaptiveAimAssist();
            }
        }

        public void AI_AIM()
        {
            currentBall.SetPositionRotation(aiBallSpawn.position, aiBallSpawn.rotation);
            currentBall.EnableAndMoveIndicator();
            SendCustomEventDelayedSeconds(nameof(AI_SHOOT), 1);
        }

        public void AI_SHOOT()
        {
            currentBall.ShootAI(_aiSkill);
        }
    }
}
 
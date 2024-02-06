
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
        public bool DoRandomCupColor = false;

        [HideInInspector]
        public int PlayerIndex;

        [HideInInspector]
        public ThryBP_Main MainScript;

        [UdonSynced] int _numberOfThrows = 0;
        [UdonSynced] int _numberOfHits = 0;

        
        [UdonSynced, HideInInspector]
        public float LastAimAssistStrength = 0;

        public void Init(Transform[] anchors)
        {
            ogColor = playerColor;
            uiButtonMaterial = uiButton.GetComponent<Renderer>().material;
            uiButtonMaterial.color = playerColor;
            foreach (UnityEngine.UI.Image i in uiImagesUsingPlayerColor)
            {
                // Keep original alpha
                Color c = playerColor;
                c.a = i.color.a;
                i.color = c;
            }
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
            if(DoRandomCupColor != r)
            {
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                DoRandomCupColor = r;
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
                if(UseAdaptiveAISkill) AiSkillSlider.SetFloat(MainScript.GetAdaptiveAIStrength(PlayerIndex));
                SendCustomEventDelayedSeconds(nameof(AI_AIM), 2);
            }
        }

        public void AI_AIM()
        {
            currentBall.OnPickupAI(); // For when AutoRespawnBallAfterCupHit is off & the ball is still in the cup state
            currentBall.SetPositionRotation(aiBallSpawn.position, aiBallSpawn.rotation);
            SendCustomEventDelayedSeconds(nameof(AI_SHOOT), 1);
        }

        public void AI_SHOOT()
        {
            currentBall.ShootAI(_aiSkill);
        }

        // For UI Calls
        public void SyncSettingsToAllPlayers()
        {
            ThryBP_CupsSpawn src = this.cups;
            for(int i=0;i<MainScript.players.Length;i++)
            {
                ThryBP_Player p = MainScript.players[i];
                if (p == this) continue;
                p.cups.RowsSlider.value = src.RowsSlider.value;
                p.cups.ShapeSelector.SetFloat(src.ShapeSelector.GetFloat());
            }
        }

        public void ResetAllPlayers()
        {
            for(int i=0;i<MainScript.players.Length;i++)
            {
                ThryBP_Player p = MainScript.players[i];
                p.ResetGlasses();
            }
        }
    }
}
 
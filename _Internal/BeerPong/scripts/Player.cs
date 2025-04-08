
using JetBrains.Annotations;
using System;
using Thry.Udon.AvatarTheme;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.BeerPong
{  
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Player : SimpleUISetterExtension
    {
        public CupsManager CupsManager;
        public PlayerScoreAnnouncement ScoreAnnouncement;
        public Animator UIAnimator;

        [Space(10)]
        public Transform BallSpwanpoint;
        public Transform AiBallSpawn;
        public Transform UiButton;
        private Material _uiButtonMaterial;

        [Space(10)]
        public UnityEngine.UI.Text LastAimAssitDisplay;
        public UnityEngine.UI.Text[] UI_score_boards;
        public UnityEngine.UI.Image[] UiImagesUsingPlayerColor;
        [SerializeField] GameObject _avatarThemeColorToggle;
        [SerializeField] GameObject _aiNavButton;
        [SerializeField] GameObject _aiSettings;
        [SerializeField] GameObject _aimAssistSettings;
        [SerializeField, HideInInspector] AvatarThemeColor _themeColor;

        private int _playerIndex;
        private Game _gameManager;
        
        public int PlayerIndex => _playerIndex;
        public Game GameManager => _gameManager;

        [UdonSynced] float _lastAimAssistStrength = 0;
        [UdonSynced] int _numberOfThrows = 0;
        [UdonSynced] int _numberOfHits = 0; 
        [UdonSynced, FieldChangeCallback(nameof(Score))] int _score;

        [UdonSynced, FieldChangeCallback(nameof(ActiveColor))]
        private Color _activeColor;
        private Color _originalColor;

        [UdonSynced, FieldChangeCallback(nameof(DoRandomColor))] private bool _doRandomCupColor = false;

        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(IsAI))] public bool _isAI = false;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(UseAdaptiveAISkill))] public bool _useAdaptiveAISkill = true;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(AiSkill))] public float _aiSkill = 0.3f;
        
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(AreSettingsOpen))] bool _areSettingsOpen = false;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(SettingsNavbar))] int _settingsNavbar = 0;
        protected override string LogPrefix => "Thry.BeerPong.Player";

        public void Init(Game gameManager, int playerIndex, Transform[] anchors, Color initalColor, bool showAvatarColorOptions, bool enableAI, bool enableAimAssist)
        {
            _playerIndex = playerIndex;
            _gameManager = gameManager;

            _originalColor = initalColor;
            _uiButtonMaterial = UiButton.GetComponent<Renderer>().material;
            _uiButtonMaterial.color = _activeColor;
            foreach (UnityEngine.UI.Image i in UiImagesUsingPlayerColor)
            {
                // Keep original alpha
                Color c = _activeColor;
                c.a = i.color.a;
                i.color = c;
            }
            
            _avatarThemeColorToggle.SetActive(_themeColor && showAvatarColorOptions);
            _aiSettings.SetActive(enableAI);
            _aimAssistSettings.SetActive(enableAimAssist);
            _aiNavButton.SetActive(enableAI || enableAimAssist);

            CupsManager.Init(anchors);
            ActiveColor = initalColor;
            if (Networking.IsOwner(gameObject)) RequestSerialization();
        }

        public bool AreSettingsOpen
        {
            get => _areSettingsOpen;
            set
            {
                _areSettingsOpen = value;
                VariableChangedFromBehaviour(nameof(_areSettingsOpen));
                UIAnimator.SetBool("isOn", _areSettingsOpen);
            }
        }

        public int SettingsNavbar
        {
            get => _settingsNavbar;
            set
            {
                _settingsNavbar = value;
                VariableChangedFromBehaviour(nameof(_settingsNavbar));
            }
        }

        public void ToggleSettings()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            AreSettingsOpen = !AreSettingsOpen;
            RequestSerialization();
        }
        
        public bool IsAI
        {
            get => _isAI;
            set
            {
                _isAI = value;
                VariableChangedFromBehaviour(nameof(_isAI));
            }
        }
        public bool UseAdaptiveAISkill
        {
            get => _useAdaptiveAISkill;
            set
            {
                _useAdaptiveAISkill = value;
                VariableChangedFromBehaviour(nameof(_useAdaptiveAISkill));
            }
        }
        public float AiSkill
        {
            get => _aiSkill;
            set
            {
                _aiSkill = value;
                VariableChangedFromBehaviour(nameof(_aiSkill));
            }
        }

        public float LastAimAssistStrength
        {
            get => _lastAimAssistStrength;
            set
            {
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                _lastAimAssistStrength = value;
                RequestSerialization();
            }
        }

        public Color ActiveColor
        {
            get => _activeColor;
            set
            {
                _activeColor = value;
                _uiButtonMaterial.color = _activeColor;
                CupsManager.SyncColor();
                foreach (UnityEngine.UI.Image i in UiImagesUsingPlayerColor)
                {
                    // Keep original alpha
                    Color c = _activeColor;
                    c.a = i.color.a;
                    i.color = c;
                }
                // sync balls
                foreach (Ball b in GameManager.Balls)
                {
                    if (b.gameObject.activeSelf && b.CurrentPlayer == this.PlayerIndex)
                        b._SetColor();
                }
            }
        }

        public void SetColor(Color c)
        {
            if (ActiveColor == c) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            ActiveColor = c;
            RequestSerialization();
        }

        public void ResetColor()
        {
            SetColor(_originalColor);
        }

        public bool DoRandomColor
        {
            get => _doRandomCupColor;
            set
            {
                _doRandomCupColor = value;
                CupsManager.SyncColor();
            }
        }

        public void SetDoRandomColor(bool doRandom)
        {
            if (doRandom == DoRandomColor) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            DoRandomColor = doRandom;
            RequestSerialization();
        }

        public void PublishAimAssistValue(float value)
        {
            LastAimAssitDisplay.text = (value * 100).ToString("F0") + "%";
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
            if(_isAI) return AiSkill;
            if(_numberOfThrows == 0) return 0.5f;
            return (float)_numberOfHits / (float)_numberOfThrows;
        }

        private Ball currentBall;
        public void ItsYourTurn(Ball ball)
        {
            currentBall = ball;
            if (_isAI)
            {
                SendCustomEventDelayedSeconds(nameof(AI_Aim), 2);
            }
        }

        public void AI_Aim()
        {
            currentBall.OnPickupAI(); // For when AutoRespawnBallAfterCupHit is off & the ball is still in the cup state
            currentBall.SetPositionRotation(AiBallSpawn.position, AiBallSpawn.rotation);
            SendCustomEventDelayedSeconds(nameof(AI_Shoot), 1);
        }

        public void AI_Shoot()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            
            // Update and Publish Skill
            if (UseAdaptiveAISkill)
                AiSkill = GameManager.GetAdaptiveAIStrength(PlayerIndex);
            GameManager.SetLastAimAssist(AiSkill);
            LastAimAssistStrength = AiSkill;

            RequestSerialization();

            currentBall.ShootAI(AiSkill);
        }

        [PublicAPI]
        public void SyncSettingsToAllPlayers()
        {
            CupsManager src = this.CupsManager;
            for(int i=0;i<GameManager.Players.Length;i++)
            {
                Player p = GameManager.Players[i];
                if (p == this) continue;
                p.CupsManager.Rows = src.Rows;
                p.CupsManager.Shape = src.Shape;
                p.CupsManager.NetworkChanges();
            }
        }

        [PublicAPI]
        public void ResetAllPlayers()
        {
            for(int i=0;i<GameManager.Players.Length;i++)
            {
                GameManager.Players[i].ResetPlayer();
            }
        }

        public void ResetPlayer()
        {
            CupsManager.ResetGlasses();
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _numberOfHits = 0;
            _numberOfThrows = 0;
            _score = 0;
            RequestSerialization();
        }


        //=======Score==========

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                if(_score == 0)
                    ScoreAnnouncement.ResetScore();
                UpdateUI();
            }
        }


        [PublicAPI]
        public void AddToScore(int a)
        {
            if (a == 0) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Score += a;
            RequestSerialization();
        }

        private void UpdateUI()
        {
            string s = _score.ToString("D4");
            foreach (UnityEngine.UI.Text t in UI_score_boards) t.text = s;
            if (_score != 0 && GameManager._showPointsPopup)
                ScoreAnnouncement.ShowScore(_score);
        }
    }
}
 
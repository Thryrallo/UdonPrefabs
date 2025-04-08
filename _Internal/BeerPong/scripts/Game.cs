
using System;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.BeerPong
{
    public enum GameMode
    {
        Normal = 0,
        KingOfTheHill = 1,
        BigPong = 2,
        Mayham = 3
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Game : SimpleUISetterExtension
    {
        [Header("Customization")]
        [SerializeField] private Color[] _initialPlayerColors = new Color[4] { Color.red, Color.blue, Color.green, Color.yellow };
        [SerializeField] private bool _allowAvatarThemeColorUsage = true;
        [SerializeField] private bool _allowAimAssist = true;
        [SerializeField] private bool _allowAI = true;

        [NonSerialized, HideInInspector] public Player[] Players;

        [Space(30)]
        [Header("Referecnes")]
        [SerializeField] private Transform _playersParent;
        [SerializeField] private Animator _tablesAnimator;
        [SerializeField] public Ball[] Balls;
        [SerializeField] private int _teamIndicatorMaterialIndex = 3;

        [Space(10)]
        [SerializeField] private GameObject[] _tables;
        [SerializeField] private Transform[] _twoTeamPositions;
        [SerializeField] private Transform[] _threeTeamPositions;
        [SerializeField] private Transform[] _fourTeamPositions;

        
        [Space(10)]
        [SerializeField] private Transform _respawnHeight;
        [SerializeField] public Transform TableHeight;
        [SerializeField] private Transform _cupsCollector;
        [SerializeField] private Transform _kingOfTheHillParent;

        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(Gamemode))] public GameMode _gameMode = GameMode.Normal;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(PlayerCount))] public int _playerCount = 2;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(AimAssist))] public float _aimAssist = AIM_ASSIST_DEFAULT;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(Size))] public float _size = 1;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(UseBarrier))] public bool _useBarrier = false;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(DoTeams))] public bool _doTeams = false;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(DoAdaptiveAimAssist))] public bool _doAdaptiveAimAssist = true;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(AllowSelfFire))] public bool _allowSelfFire = false;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(AllowFriendlyFire))] public bool _allowFriendlyFire = true;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(DoAutoRespawnBalls))] public bool _doAutoRespawnBalls = true;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(UseAvatarThemeColor))] public bool _useAvatarThemeColor = true;
        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(ShowPointsPopup))] public bool _showPointsPopup = true;
        
        [NonSerialized, FieldChangeCallback(nameof(Volume))] public float _volume = 0.5f;

        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(LastAimAssist))] public float _lastAimAssist;

        private Material _teamIndicatorMaterial;

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                foreach (Ball b in Balls)
                    b.BallAudio.volume = _volume;
            }
        }

        public GameMode Gamemode
        {
            get => _gameMode;
            set
            {
                _gameMode = value;
                VariableChangedFromBehaviour(nameof(_gameMode));
                
                ChangeGameMode();
            }
        }

        public int PlayerCount
        {
            get => _playerCount;
            set
            {
                _playerCount = value;
                VariableChangedFromBehaviour(nameof(_playerCount));

                ChangeAmountOfPlayers();
            }
        }

        public float AimAssist
        {
            get => _aimAssist;
            set
            {
                _aimAssist = value;
                VariableChangedFromBehaviour(nameof(_aimAssist));
            }
        }

        public float Size
        {
            get => _size;
            set
            {
                _size = value;
                VariableChangedFromBehaviour(nameof(_aimAssist));

                this.transform.localScale = Vector3.one * _size;
            }
        }

        public bool UseBarrier
        {
            get => _useBarrier;
            set
            {
                _useBarrier = value;
                VariableChangedFromBehaviour(nameof(_useBarrier));

                ChangeBarrier();
            }
        }

        public bool DoTeams
        {
            get => _doTeams;
            set
            {
                _doTeams = value;
                VariableChangedFromBehaviour(nameof(_doTeams));

                ChangeTeams();
            }
        }

        public bool DoAdaptiveAimAssist
        {
            get => _doAdaptiveAimAssist;
            set
            {
                _doAdaptiveAimAssist = value;
                VariableChangedFromBehaviour(nameof(_doAdaptiveAimAssist));
            }
        }

        public bool AllowSelfFire
        {
            get => _allowSelfFire;
            set
            {
                _allowSelfFire = value;
                VariableChangedFromBehaviour(nameof(_allowSelfFire));
            }
        }

        public bool AllowFriendlyFire
        {
            get => _allowFriendlyFire;
            set
            {
                _allowFriendlyFire = value;
                VariableChangedFromBehaviour(nameof(_allowFriendlyFire));
            }
        }

        public bool DoAutoRespawnBalls
        {
            get => _doAutoRespawnBalls;
            set
            {
                _doAutoRespawnBalls = value;
                VariableChangedFromBehaviour(nameof(_doAutoRespawnBalls));
            }
        }

        public float LastAimAssist
        {
            get => _lastAimAssist;
            set
            {
                _lastAimAssist = value;

                foreach (Player p in Players)
                    p.PublishAimAssistValue(_lastAimAssist);
            }
        }

        public bool UseAvatarThemeColor
        {
            get => _useAvatarThemeColor;
            set
            {
                _useAvatarThemeColor = value;
                VariableChangedFromBehaviour(nameof(_useAvatarThemeColor));
                
                if(!_useAvatarThemeColor && Networking.IsOwner(gameObject))
                {
                    foreach (Player p in Players)
                        p.ResetColor();
                }
            }
        }

        public bool ShowPointsPopup
        {
            get => _showPointsPopup;
            set
            {
                _showPointsPopup = value;
                VariableChangedFromBehaviour(nameof(_showPointsPopup));
            }
        }

        protected override string LogPrefix => "Thry.BeerPong.Game";

        public bool AllowAimAssist => _allowAimAssist;
        

        private int _teamSize = 1;

        int[] _recentThrowsHits = new int[12];
        float[] _rencentAimAssitsValues = new float[12];
        int _recentThrowsHitsFillIndex = -1;
        int _recentThrowsHitsFillCount = 0;

        float _myAimAssistValue = 0.1f;

        const float ADAPTIVE_AIM_ASSIST_GOAL = 0.33f;
        const float ADAPTIVE_AIM_ASSSIT_LERP_SPEED = 1f;

        const float AIM_ASSIST_DEFAULT = 0.3f;

        void Start()
        {
            if(Networking.LocalPlayer == null) return;

            Players = new Player[_playersParent.childCount];
            _myAimAssistValue = AIM_ASSIST_DEFAULT;

            Transform[] anchors = new Transform[3];
            anchors[1] = _kingOfTheHillParent;
            anchors[2] = _kingOfTheHillParent;

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i] = _playersParent.GetChild(i).GetComponent<Player>();
                Players[i].Init(this, i, anchors, _initialPlayerColors[i], _allowAvatarThemeColorUsage, _allowAI, _allowAimAssist);
            }

            foreach (Ball b in Balls)
            {
                b.Init(_respawnHeight);
            }

            //Instanciate teamindactor material for multiple tables
            Material teamIndecatorMaterialIntance = null;
            //Gets an instanciated material from the first table type
            //Asignes the same instanciated material to the other tables types
            for (int i = 0; i < _tables.Length; i++)
            {
                if (_tables[i] == null) continue;
                Renderer renderer = _tables[i].GetComponent<Renderer>();
                if (teamIndecatorMaterialIntance == null)
                {
                    teamIndecatorMaterialIntance = renderer.materials[_teamIndicatorMaterialIndex];
                }
                else
                {
                    Material[] materials = renderer.materials;
                    materials[_teamIndicatorMaterialIndex] = teamIndecatorMaterialIntance;
                    renderer.materials = materials;
                }
            }
            _teamIndicatorMaterial = teamIndecatorMaterialIntance;

            Log(LogLevel.Log, $"Initialized. Players: {Players.Length}, Balls: {Balls.Length}, Tables: {_tables.Length}, IsOwner: {Networking.IsOwner(gameObject)}");

            ChangeAmountOfPlayers();
        }

        public void SetLastAimAssist(float assist)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            LastAimAssist = assist;
            RequestSerialization();
        }
        public void UpdateLocalPlayerAdaptiveAimAssist()
        {
            if(!DoAdaptiveAimAssist) return;
            if(_recentThrowsHitsFillCount > 2)// Wait for 3 throws to start adapting
            {
                float hits = 0;
                float count = 0;
                float w_min = Mathf.Min(_rencentAimAssitsValues);
                float w_max = Mathf.Max(_rencentAimAssitsValues);
                for (int i = 0; i < Mathf.Min(_recentThrowsHits.Length, _recentThrowsHitsFillCount); i++)
                {
                    // take into account throws that are closer to the current aim assist value more
                    float weight = Mathf.Abs(_rencentAimAssitsValues[i] - _myAimAssistValue); // between min and max
                    // lerp to between 0 and 0.8
                    weight = Mathf.InverseLerp(w_min, w_max, weight) * 0.8f;
                    // inverse it
                    weight = 1 - weight;

                    hits += _recentThrowsHits[i] * weight;
                    count += weight;
                }

                float hitPercentage = hits / count;
                float lerpVal = ADAPTIVE_AIM_ASSSIT_LERP_SPEED * Mathf.Abs(hitPercentage - ADAPTIVE_AIM_ASSIST_GOAL);
                float prevValue = _myAimAssistValue;
                if (hitPercentage > ADAPTIVE_AIM_ASSIST_GOAL)
                {
                    _myAimAssistValue = Mathf.Lerp(_myAimAssistValue, 0, lerpVal);
                }
                else
                {
                    _myAimAssistValue = Mathf.Lerp(_myAimAssistValue, 1, lerpVal);
                }
                Log(LogLevel.Vervose, $"UpdateLocalPlayerAdaptiveAimAssist: {prevValue} => {_myAimAssistValue} with {hitPercentage} hits");
            }
        }

        public float GetAdaptiveAIStrength(int playerIndex)
        {
            float totalSkill = 0;
            float totalCupsOthers = 0;
            float skilledPlayerCount = PlayerCount;
            for(int i = 0; i < skilledPlayerCount; i++)
            {
                if(i != playerIndex)
                {
                    totalSkill += Players[i].GetSkill();
                    totalCupsOthers += Players[i].CupsManager.SyncedCount;
                }
            }
            float myCups = Players[playerIndex].CupsManager.SyncedCount;
            float avgCups = totalCupsOthers / (skilledPlayerCount-1);
            
            float valueSkill = totalSkill / (skilledPlayerCount-1);
            float valueCups = avgCups / myCups;
            float rdm = UnityEngine.Random.value * 0.3f - 0.15f;
            Log(LogLevel.Vervose, $"GetAdaptiveAIStrength: {valueSkill} * {valueCups} + {rdm}");
            return Mathf.Clamp01(valueSkill * valueCups + rdm);
        }

        public Vector3 GetAITnitalTarget(int playerIndex)
        {
            Vector3 target = Vector3.zero;
            if (Gamemode == GameMode.BigPong)
            {
                CupsManager cupSpawn = Players[0].CupsManager;
                target = _kingOfTheHillParent.position + _kingOfTheHillParent.up * cupSpawn.GlobalHeight;
                Debug.DrawLine(_kingOfTheHillParent.position, target, Color.blue, 5);
            }
            else if(Gamemode == GameMode.KingOfTheHill)
            {
                CupsManager cupSpawn = Players[0].CupsManager;
                target = _kingOfTheHillParent.position + _kingOfTheHillParent.up * cupSpawn.GlobalHeight * 4;
            }
            else
            {
                // get player with most cups that is not me
                int mostCups = -1;
                int mostCupsIndex = 0;
                for (int i = 0; i < PlayerCount; i++)
                {
                    if (i != playerIndex && (!DoTeams || !IsSameTeam(playerIndex, i)))
                    {
                        if (Players[i].CupsManager.SyncedCount > mostCups)
                        {
                            mostCups = Players[i].CupsManager.SyncedCount;
                            mostCupsIndex = i;
                        }
                    }
                }
                Log(LogLevel.Vervose, $"GetAITnitalTarget: Targeting Player: {mostCupsIndex} with {mostCups} cups");
                target = Players[mostCupsIndex].CupsManager.CupAnchor.position;
            }
            // add random xz offset, max 0.15m
            target += new Vector3(UnityEngine.Random.value * 0.3f - 0.15f, 0, UnityEngine.Random.value * 0.3f - 0.15f);
            return target;
        }

        public Vector3 GetAIThrowVector(Vector3 ballPos, int playerIndex)
        {
            Vector3 target = GetAITnitalTarget(playerIndex); // get rough target
            Debug.DrawLine(ballPos, target, Color.red, 5);
            if(Gamemode == GameMode.BigPong)
            {
                return ((target - ballPos).normalized + Vector3.up * 2.0f).normalized;
            }
            else if(Gamemode == GameMode.KingOfTheHill)
            {
                return ((target - ballPos).normalized + Vector3.up * 1.0f).normalized * 0.7f * transform.lossyScale.x;
            }
            else
            {
                return ((target - ballPos).normalized + Vector3.up * 0.5f).normalized * 0.7f * transform.lossyScale.x;
            }
        }

        public void RemoveCup(Cup cup, int scoreingPlayer)
        {
            Log(LogLevel.Vervose, $"RemoveCup: {cup.Row} {cup.Column} {scoreingPlayer}");
            if (cup.PlayerAnchorSide != null)
            {
                if (Gamemode != GameMode.BigPong)
                {
                    cup.PlayerAnchorSide.CupsManager.RemoveCup(cup.Row, cup.Column);
                }
            }
        }

        bool IsSameTeam(int player1, int player2)
        {
            return player1 % _teamSize == player2 % _teamSize;
        }

        public void CountCupHit(int cupOwner, int scoreingPlayer, int scoreType)
        {
            Log(LogLevel.Vervose, $"CountCupHit: {cupOwner} {scoreingPlayer} {scoreType} {Players[scoreingPlayer]._isAI}");
            if (!Players[scoreingPlayer]._isAI)
            {
                _recentThrowsHits[_recentThrowsHitsFillIndex] = 1;
                Players[scoreingPlayer].AddHit();
            }
            if (cupOwner < Players.Length)
            {
                //for teams
                if (DoTeams)
                {
                    int add = AddToScore(scoreingPlayer, cupOwner, scoreType);
                    if (add != 0)
                    {
                        for (int i = scoreingPlayer % _teamSize; i < Players.Length; i = i + _teamSize)
                        {
                            Players[i].AddToScore(add);
                        }
                    }
                }
                //for non teams
                else
                {
                    int add = AddToScore(scoreingPlayer, cupOwner, scoreType);
                    if(add != 0)
                    {
                        Players[scoreingPlayer].AddToScore(add);
                    }
                }
            }
        }

        public void CountThrow(int playerIndex)
        {
            Log(LogLevel.Vervose, $"CountThrow: {playerIndex} {Players[playerIndex]._isAI} {_recentThrowsHitsFillIndex} {_recentThrowsHitsFillCount}");
            if (Players[playerIndex]._isAI) return; // AI throws are not counted
            _recentThrowsHitsFillIndex = (_recentThrowsHitsFillIndex + 1) % _recentThrowsHits.Length;
            _recentThrowsHits[_recentThrowsHitsFillIndex] = 0;
            _recentThrowsHitsFillCount = Mathf.Min(_recentThrowsHitsFillCount + 1, _recentThrowsHits.Length);
            _rencentAimAssitsValues[_recentThrowsHitsFillIndex] = LastAimAssist;
            Players[playerIndex].AddThrow();
        }

        public bool AllowCollision(int ballOwner, Player cupOwner, Player cupSide)
        {
            if (cupOwner != Players[cupOwner.PlayerIndex]) return false; // Ball is from other table
            if (Gamemode == GameMode.KingOfTheHill) return true;
            if (Gamemode == GameMode.BigPong) return true;
            if (Gamemode == GameMode.Mayham) return ballOwner != cupSide.PlayerIndex;  // chedck if is on own side
            if (ballOwner == cupOwner.PlayerIndex) return AllowSelfFire;
            if (DoTeams && ballOwner % _teamSize == cupOwner.PlayerIndex % _teamSize) return AllowFriendlyFire;
            return true;
        }

        public bool ShouldAimAssistAimForOwnCups()
        {
            if (Gamemode == GameMode.KingOfTheHill) return true;
            if (Gamemode == GameMode.BigPong) return true;
            return false;
        }

        public bool ShouldAimAssistAimAtOwnSide()
        {
            if (Gamemode == GameMode.Mayham) return false;
            return true;
        }

        //score type 0: normal
        //           1: rimming
        private int AddToScore(int ballOwner, int cupOwner, int scoreType)
        {
            if (Gamemode == GameMode.KingOfTheHill) return 2 - scoreType;
            if (Gamemode == GameMode.BigPong) return 2 - scoreType;
            if (ballOwner == cupOwner) return -1;
            if (DoTeams && ballOwner % _teamSize == cupOwner % _teamSize) return -3;
            return 1 + scoreType;
        }

        public Cup GetCup(int p, int x, int y)
        {
            return Players[p].CupsManager.GetCup(x, y);
        }

        public Transform GetBallSpawn(int player)
        {
            if (player < Players.Length) return Players[player].BallSpwanpoint;
            return transform;
        }

        public void SetActivePlayerColor(Color col)
        {
            if (Gamemode == GameMode.KingOfTheHill || Gamemode == GameMode.Mayham) return;
            if (!_allowAvatarThemeColorUsage) return;

            if (_teamIndicatorMaterial == null) return;
            _teamIndicatorMaterial.color = col;
            _teamIndicatorMaterial.SetColor("_EmissionColor", col);
        }

        public int PlayersWithCupsLeft()
        {
            int t = 0;
            for (int i = 0; i < Players.Length && i < PlayerCount; i++)
            {
                if (Players[i].CupsManager.SyncedCount > 0) t++;
            }
            return t;
        }

        public int GetNextPlayer(int c)
        {
            if (Gamemode == GameMode.KingOfTheHill || Gamemode == GameMode.Mayham) return c;

            c = (c + 1) % PlayerCount;
            if (PlayersWithCupsLeft() > 1)
            {
                while (Players[c].CupsManager.SyncedCount == 0)
                {
                    c = (c + 1) % PlayerCount;
                }
            }
            return c;
        }

        void ChangeTeams()
        {
            Log(LogLevel.Log, "ChangeTeams");
            if (DoTeams)
            {
                _teamSize = 2;
                for (int i = _teamSize; i < Players.Length; i++)
                {
                    int teamIndex = i % _teamSize;
                    Players[i].SetColor(Players[teamIndex].ActiveColor);
                }
            }
            else
            {
                _teamSize = 1;
                for (int i = _teamSize; i < Players.Length; i++)
                {
                    Players[i].ResetColor();
                }
            }
        }

        void ChangeBarrier()
        {
            Log(LogLevel.Log, "ChangeBarrier");
            SetDividersActive(UseBarrier && Gamemode != GameMode.KingOfTheHill && Gamemode != GameMode.BigPong);
        }

        void SetDividersActive(bool active)
        {
            _tablesAnimator.SetBool("dividers", active);
        }

        void ChangeGameMode()
        {
            Log(LogLevel.Log, "ChangeGameMode");
            //EnableNeededCupManangers();

            bool enableDeviders = false;
            bool doRandomColor = false;

            switch (Gamemode)
            {
                case GameMode.KingOfTheHill:
                    enableDeviders = false;
                    doRandomColor = true;
                    break;
                case GameMode.BigPong:
                    enableDeviders = false;
                    doRandomColor = false;
                    break;
                case GameMode.Mayham:
                case GameMode.Normal:
                    enableDeviders = true;
                    doRandomColor = false;
                    break;
            }

            ConfigureCups();
            
            SetDividersActive(enableDeviders && UseBarrier);

            if(Networking.IsOwner(gameObject))
            {
                for (int i = 0; i < Players.Length; i++)
                {
                    if (Gamemode == GameMode.Mayham) Players[i].CupsManager.SetRows(6);
                    Players[i].CupsManager.ResetGlasses();
                    Players[i].SetDoRandomColor(doRandomColor);
                }
            }

            ResetBalls();
        }

        void ConfigureCups()
        {
            switch (Gamemode)
            {
                case GameMode.KingOfTheHill:
                    ConfigureCupsKingOfTheHill();
                    break;
                case GameMode.BigPong:
                    ConfigureCupsBigPong();
                    break;
                case GameMode.Mayham:
                case GameMode.Normal:
                    ConfigureCupsNormal();
                    break;
            }
        }

        void ConfigureCupsKingOfTheHill()
        {
            float height = 0;
            for (int i = 0; i < Players.Length; i++)
            {

                int rows = 10 - i * 2;
                Vector3 zOffsetPerCup = Vector3.back * Players[i].CupsManager.UnscaledLocalLength;
                Players[i].CupsManager.SetOverwrite(CupsShape.Circle, rows);
                Players[i].CupsManager.ChangeConfiguration(1, 1, Vector3.up * height + zOffsetPerCup * (rows / 2.0f));

                height += Players[i].CupsManager.LocalHeight;
            }
        }

        void ConfigureCupsBigPong()
        {
            for (int i = 1; i < Players.Length; i++)
            {
                Players[i].CupsManager.SetOverwrite(0);
                Players[i].CupsManager.ChangeConfiguration(1, 0, Vector3.zero);
            }
            Players[0].CupsManager.SetOverwrite(1);
            Players[0].CupsManager.ChangeConfiguration(10, 2, Vector3.zero);
        }

        void ConfigureCupsNormal()
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                Players[i].CupsManager.DisableOverwrite();
                Players[i].CupsManager.ChangeConfiguration(1, 0, Vector3.zero);
            }
            for (int i = PlayerCount; i < Players.Length; i++)
            {
                Players[i].CupsManager.SetOverwrite(0);
                Players[i].CupsManager.ChangeConfiguration(1, 0, Vector3.zero);
            }
        }

        //void EnableNeededCupManangers()
        //{
        //    switch(Gamemode)
        //    {
        //        case GameMode.Normal:
        //        case GameMode.Mayham:
        //            for (int i = 0; i < Players.Length; i++)
        //                Players[i].CupsManager.gameObject.SetActive(i < PlayerCount);
        //            break;
        //        case GameMode.BigPong:
        //            for (int i = 0; i < Players.Length; i++)
        //                Players[i].CupsManager.gameObject.SetActive(i == 0);
        //            break;
        //        case GameMode.KingOfTheHill:
        //            for (int i = 0; i < Players.Length; i++)
        //                Players[i].CupsManager.gameObject.SetActive(true);
        //            break;
        //    }
        //}

        void ResetBalls()
        {
            if(Gamemode == GameMode.Normal || Gamemode == GameMode.BigPong)
            {
                for (int i = 1; i < Balls.Length; i++)
                    Balls[i].gameObject.SetActive(false);
                Balls[0].gameObject.SetActive(true);
                Balls[0]._SetColor();
                if (Networking.IsOwner(gameObject)) Balls[0].Respawn();
            }else if(Gamemode == GameMode.KingOfTheHill || Gamemode == GameMode.Mayham)
            {
                for (int i = 0; i < Balls.Length; i++)
                {
                    Balls[i].gameObject.SetActive(i < PlayerCount);
                    Balls[i].CurrentPlayer = i;
                    Balls[i]._SetColor();
                    if (Networking.IsOwner(gameObject)) Balls[i].Respawn();
                }

                if(_allowAvatarThemeColorUsage)
                {
                    _teamIndicatorMaterial.color = Color.white;
                    _teamIndicatorMaterial.SetColor("_EmissionColor", Color.white);
                }
            }
        }

        int DetermineLocalPlayerIndex()
        {
            int index = -1;
            // determine local player index by checking which forward vector is closest to the local player pos to table center vector
            Vector3 compareVec = transform.position - Networking.LocalPlayer.GetPosition();
            // make compareVec is on the xz plane of the table
            compareVec = transform.InverseTransformVector(compareVec);
            compareVec.y = 0;
            compareVec = transform.TransformVector(compareVec);
            float minAngle = 360;
            for (int i = 0; i < Players.Length; i++)
            {
                if(Players[i].gameObject.activeSelf == false) continue;
                float angle = Vector3.Angle(compareVec, Players[i].transform.forward);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    index = i;
                }
            }
            return index;
        }

        void ChangePlayerTransform(Transform playerObjTranform, Vector3 newTablePos, Quaternion newTableRot, bool teleportPlayer)
        {
            if(teleportPlayer)
            {
                Vector3 relativePos = playerObjTranform.InverseTransformPoint(Networking.LocalPlayer.GetPosition());
                Vector3 relativeRot = playerObjTranform.InverseTransformDirection(Networking.LocalPlayer.GetRotation() * Vector3.forward);

                playerObjTranform.SetPositionAndRotation(newTablePos, newTableRot);

                Vector3 newPos = playerObjTranform.TransformPoint(relativePos);
                Vector3 newRot = playerObjTranform.TransformDirection(relativeRot);

                Networking.LocalPlayer.TeleportTo(newPos, Quaternion.LookRotation(newRot));
            }else
            {
                playerObjTranform.SetPositionAndRotation(newTablePos, newTableRot);
            }
            
        }

        void ChangeAmountOfPlayers()
        {
            Log(LogLevel.Log, $"ChangeAmountOfPlayers {PlayerCount}");
            if (PlayerCount <= _tables.Length && PlayerCount <= Players.Length)
            {
                //EnableNeededCupManangers();
                ConfigureCups();
                //activate correct table
                for (int i = 0; i < _tables.Length; i++)
                {
                    if (_tables[i] != null) _tables[i].SetActive(i == PlayerCount);
                }
                // teleport players to correct positions
                bool doTeleportPlayer = Networking.IsOwner(gameObject);
                int localPlayerIndex =  DetermineLocalPlayerIndex();
                //activate teams
                for (int i = 0; i < Players.Length; i++)
                {
                    if (Players[i].gameObject.activeSelf == false && Networking.IsOwner(gameObject))
                    {
                        Players[i].CupsManager.ResetGlasses();
                    }
                    Players[i].gameObject.SetActive(i < PlayerCount);
                    //set positions
                    if (i < PlayerCount)
                    {
                        if (PlayerCount == 2)
                        {
                            ChangePlayerTransform(Players[i].transform, _twoTeamPositions[i].position, _twoTeamPositions[i].rotation, doTeleportPlayer && i == localPlayerIndex);
                        } else if (PlayerCount == 3)
                        {
                            ChangePlayerTransform(Players[i].transform, _threeTeamPositions[i].position, _threeTeamPositions[i].rotation, doTeleportPlayer && i == localPlayerIndex);
                        } else if (PlayerCount == 4)
                        {
                            ChangePlayerTransform(Players[i].transform, _fourTeamPositions[i].position, _fourTeamPositions[i].rotation, doTeleportPlayer && i == localPlayerIndex);
                        }
                    }
                    //Players[i].CupsManager.SyncGlasses();
                }
                ResetBalls();
            }
        }
    }
}
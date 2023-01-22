
using Thry.General;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.BeerPong{
    public class ThryBP_Main : UdonSharpBehaviour
    {
        [Header("Customization")]
        public Color[] initialPlayerColors;

        [HideInInspector]
        public ThryBP_Player[] players;

        [Header("Referecnes")]
        public Transform playersParent;
        public GameObject[] tables;
        public Animator tablesAnimator;
        public Material teamIndicatorMaterial;

        public Transform[] twoTeamPositions;

        public Transform[] threeTeamPositions;

        public Transform[] fourTeamPositions;

        public ThryBP_Ball[] balls;

        public Transform respawnHeight;

        public Transform tableHeight;

        public Transform cupsCollector;

        public bool canHitOwnCups = false;
        public bool friendlyFire = true;

        public ThryAction playerCountSlider;
        public ThryAction toggleTeams;
        public ThryAction toggleBarrier;
        public ThryAction gamemodeSelection;
        public ThryAction AdaptiveAimAssistToggle;
        public ThryAction AimAssistSlider;

        public Transform kingOfTheHillParent;

        [HideInInspector]
        public float aimAssist;
        [HideInInspector]
        public bool EnableAdaptiveAimAssit;

        private int teamSize = 1;
        [HideInInspector]
        public int gamemode;

        public const int GM_NORMAL = 0;
        public const int GM_KING_HILL = 1;
        public const int GM_BIG_PONG = 2;

        int _localPlayerThrows = 0;
        int _localPlayerHits = 0;

        int[] _recentThrowsHits = new int[12];
        float[] _rencentAimAssitsValues = new float[12];
        int _recentThrowsHitsFillIndex = -1;
        int _recentThrowsHitsFillCount = 0;

        float _myAimAssistValue = 0.1f;
        const float ADAPTIVE_AIM_ASSIST_GOAL = 0.33f;
        const float ADAPTIVE_AIM_ASSSIT_LERP_SPEED = 1f;

        float _aimAssistDefault = 0.1f;

        void Start()
        {
            if(Networking.LocalPlayer == null) return;

            players = new ThryBP_Player[playersParent.childCount];
            _aimAssistDefault = AimAssistSlider.local_float;
            _myAimAssistValue = _aimAssistDefault;

            Transform[] anchors = new Transform[3];
            anchors[1] = kingOfTheHillParent;
            anchors[2] = kingOfTheHillParent;

            for (int i = 0; i < players.Length; i++)
            {
                players[i] = playersParent.GetChild(i).GetComponent<ThryBP_Player>();
                players[i].playerIndex = i;
                players[i]._mainScript = this;
                if (i < initialPlayerColors.Length) players[i].playerColor = initialPlayerColors[i];
                players[i].Init(anchors);
                //if (Networking.IsOwner(gameObject)) players[i].ResetGlasses();
            }

            foreach(ThryBP_Ball b in balls)
            {
                b.respawnHeight = respawnHeight;
                b._mainScript = this;
                b.Init();
            }

            //Instanciate teamindactor material for multiple tables
            Material teamIndecatorMaterialIntance = null;
            //Gets an instanciated material from the first table type
            //Asignes the same instanciated material to the other tables types
            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i] == null) continue;
                Renderer renderer = tables[i].GetComponent<Renderer>();
                for (int m=0;m< renderer.sharedMaterials.Length; m++)
                {
                    if (renderer.sharedMaterials[m] == teamIndicatorMaterial)
                    {
                        if(teamIndecatorMaterialIntance == null)
                        {
                            teamIndecatorMaterialIntance = renderer.materials[m];
                        }
                        else
                        {
                            Material[] materials = renderer.materials;
                            materials[m] = teamIndecatorMaterialIntance;
                            renderer.materials = materials;
                        }
                    }
                }
            }
            teamIndicatorMaterial = teamIndecatorMaterialIntance;

            ChangeAmountOfPlayers();
        }

        // Adaptive Aim Assist
        public void OnAdpativAimAssistChanged()
        {
            if(!EnableAdaptiveAimAssit)
            {
                AimAssistSlider.SetFloat(_aimAssistDefault);
            }
        }

        public void UpdateAdaptiveAimAssist()
        {
            if(!EnableAdaptiveAimAssit) return;
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
                    // Debug.Log( i + " => hit: " + _recentThrowsHits[i] + " aimAssist: " + _rencentAimAssitsValues[i] + " weight: " + weight);
                }

                float hitPercentage = hits / count;
                float lerpVal = ADAPTIVE_AIM_ASSSIT_LERP_SPEED * Mathf.Abs(hitPercentage - ADAPTIVE_AIM_ASSIST_GOAL);
                // Debug.Log("Hit Percentage: " + hitPercentage + " LerpVal: " + lerpVal + " AimAssist: " + _myAimAssistValue + " Hits: " + hits + " Count: " + count);
                if(hitPercentage > ADAPTIVE_AIM_ASSIST_GOAL)
                {
                    _myAimAssistValue = Mathf.Lerp(_myAimAssistValue, 0, lerpVal);
                }
                else
                {
                    _myAimAssistValue = Mathf.Lerp(_myAimAssistValue, 1, lerpVal);
                }
                // Debug.Log("=> AimAssist: " + _myAimAssistValue );
            }
            AimAssistSlider.SetFloat(_myAimAssistValue);
        }

        public float GetAdaptiveAIStrength(int playerIndex)
        {
            float totalSkill = 0;
            float totalCupsOthers = 0;
            float skilledPlayerCount = playerCountSlider.local_float;
            for(int i = 0; i < skilledPlayerCount; i++)
            {
                if(i != playerIndex)
                {
                    totalSkill += players[i].GetSkill();
                    totalCupsOthers += players[i].cups.existing_glasses_count;
                }
            }
            float myCups = players[playerIndex].cups.existing_glasses_count;
            float avgCups = totalCupsOthers / (skilledPlayerCount-1);
            
            float valueSkill = totalSkill / (skilledPlayerCount-1);
            float valueCups = avgCups / myCups;
            float rdm = Random.value * 0.3f - 0.15f;
            // Debug.Log("AI Strength: " + valueSkill + " * " + valueCups + " + " + rdm);
            return Mathf.Clamp01(valueSkill * valueCups + rdm);
        }

        public Vector3 GetAITnitalTarget(int playerIndex)
        {
            Vector3 target = Vector3.zero;
            if (gamemode == GM_BIG_PONG)
            {
                ThryBP_CupsSpawn cupSpawn = players[0].cups;
                target = kingOfTheHillParent.position + kingOfTheHillParent.up * cupSpawn.GetHeight();
                Debug.DrawLine(kingOfTheHillParent.position, target, Color.blue, 5);
            }
            else if(gamemode == GM_KING_HILL)
            {
                ThryBP_CupsSpawn cupSpawn = players[0].cups;
                target = kingOfTheHillParent.position + kingOfTheHillParent.up * cupSpawn.GetHeight() * 4;
            }
            else
            {
                // get player with most cups that is not me
                int mostCups = -1;
                int mostCupsIndex = 0;
                bool isTeams = toggleTeams.local_bool;
                for (int i = 0; i < playerCountSlider.local_float; i++)
                {
                    if (i != playerIndex && (!isTeams || !IsSameTeam(playerIndex, i)))
                    {
                        if (players[i].cups.existing_glasses_count > mostCups)
                        {
                            mostCups = players[i].cups.existing_glasses_count;
                            mostCupsIndex = i;
                        }
                    }
                }
                Debug.Log("Targeting Player: " + mostCupsIndex + " with " + mostCups + " cups");
                target = players[mostCupsIndex].cups.GetCupAnchor().position;
            }
            // add random xz offset, max 0.15m
            target += new Vector3(Random.value * 0.3f - 0.15f, 0, Random.value * 0.3f - 0.15f);
            return target;
        }

        public Vector3 GetAIThrowVector(Vector3 ballPos, int playerIndex)
        {
            Vector3 target = GetAITnitalTarget(playerIndex); // get rough target
            Debug.DrawLine(ballPos, target, Color.red, 5);
            if(gamemode == GM_BIG_PONG)
            {
                return ((target - ballPos).normalized + Vector3.up * 2.0f).normalized;
            }
            else if(gamemode == GM_KING_HILL)
            {
                return ((target - ballPos).normalized + Vector3.up * 1.0f).normalized * 0.7f * transform.lossyScale.x;
            }
            else
            {
                return ((target - ballPos).normalized + Vector3.up * 0.5f).normalized * 0.7f * transform.lossyScale.x;
            }
        }

        public void RemoveCup(int cupOwner, int row, int collum, int scoreingPlayer)
        {
            if (cupOwner < players.Length)
            {
                if (gamemode != GM_BIG_PONG)
                {
                    players[cupOwner].RemoveCup(row, collum);
                }
            }
        }

        bool IsSameTeam(int player1, int player2)
        {
            return player1 % teamSize == player2 % teamSize;
        }

        public void CountCupHit(int cupOwner, int row, int collum, int scoreingPlayer, int scoreType)
        {
            if(!players[scoreingPlayer]._isAI)
            {
                _localPlayerHits++;
                _recentThrowsHits[_recentThrowsHitsFillIndex] = 1;
                players[scoreingPlayer].AddHit();
            }
            if (cupOwner < players.Length)
            {
                //for teams
                if (toggleTeams.local_bool)
                {
                    int add = AddToScore(scoreingPlayer, cupOwner, scoreType);
                    if (add != 0)
                    {
                        for (int i = scoreingPlayer % teamSize; i < players.Length; i = i + teamSize)
                        {
                            players[i].AddToScore(add);
                        }
                    }
                }
                //for non teams
                else
                {
                    int add = AddToScore(scoreingPlayer, cupOwner, scoreType);
                    if(add != 0)
                    {
                        players[scoreingPlayer].AddToScore(add);
                    }
                }
            }
        }

        public void CountThrow(int playerIndex)
        {
            if(players[playerIndex]._isAI) return; // AI throws are not counted
            _localPlayerThrows++;
            _recentThrowsHitsFillIndex = (_recentThrowsHitsFillIndex + 1) % _recentThrowsHits.Length;
            _recentThrowsHits[_recentThrowsHitsFillIndex] = 0;
            _recentThrowsHitsFillCount = Mathf.Min(_recentThrowsHitsFillCount + 1, _recentThrowsHits.Length);
            _rencentAimAssitsValues[_recentThrowsHitsFillIndex] = AimAssistSlider.local_float;
            players[playerIndex].AddThrow();
        }

        public bool AllowCollision(int ballOwner, ThryBP_Player cupOwner)
        {
            if (cupOwner != players[cupOwner.playerIndex]) return false; // Ball is from other table
            if (gamemode == GM_KING_HILL) return true;
            if (gamemode == GM_BIG_PONG) return true;
            if (ballOwner == cupOwner.playerIndex) return canHitOwnCups;
            if (toggleTeams.local_bool && ballOwner % teamSize == cupOwner.playerIndex % teamSize) return friendlyFire;
            return true;
        }

        //score type 0: normal
        //           1: rimming
        private int AddToScore(int ballOwner, int cupOwner, int scoreType)
        {
            if (gamemode == GM_KING_HILL) return 2 - scoreType;
            if (gamemode == GM_BIG_PONG) return 2 - scoreType;
            if (ballOwner == cupOwner) return 0;
            if (toggleTeams.local_bool && ballOwner % teamSize == cupOwner % teamSize) return -3;
            return 1 + scoreType;
        }

        public ThryBP_Glass GetCup(int p, int x, int y)
        {
            return players[p].cups.GetCup(x, y);
        }

        public Transform GetBallSpawn(int player)
        {
            if (player < players.Length) return players[player].ballSpwanpoint;
            return transform;
        }

        public Color GetPlayerColor(int player)
        {
            if (player < players.Length) return players[player].playerColor;
            return Color.white;
        }

        public void SetActivePlayerColor(Color col)
        {
            if (gamemode == GM_KING_HILL) return;

            teamIndicatorMaterial.color = col;
            teamIndicatorMaterial.SetColor("_EmissionColor", col);
        }

        public int PlayersWithCupsLeft()
        {
            int t = 0;
            for (int i = 0; i < players.Length && i < (int)playerCountSlider.local_float; i++)
            {
                if (players[i].cups.existing_glasses_count > 0) t++;
            }
            return t;
        }

        public int GetNextPlayer(int c)
        {
            if (gamemode == GM_KING_HILL) return c;

            c = (c + 1) % (int)playerCountSlider.local_float;
            if (PlayersWithCupsLeft() > 1)
            {
                while (players[c].cups.existing_glasses_count == 0)
                {
                    c = (c + 1) % (int)playerCountSlider.local_float;
                }
            }
            return c;
        }

        public void ChangeTeams()
        {
            if (toggleTeams.local_bool)
            {
                teamSize = 2;
                for (int i = teamSize; i < players.Length; i++)
                {
                    int teamIndex = i % teamSize;
                    players[i].SetColor(players[teamIndex].playerColor);
                }
            }
            else
            {
                teamSize = 1;
                for (int i = teamSize; i < players.Length; i++)
                {
                    players[i].SetColor(players[i].ogColor);
                }
            }
        }

        public void ChangeBarrier()
        {
            SetDividersActive(toggleBarrier.local_bool && gamemode == GM_NORMAL);
        }

        void SetDividersActive(bool active)
        {
            tablesAnimator.SetBool("dividers", active);
        }

        public void ChangeGameMode()
        {
            gamemode = (int)gamemodeSelection.local_float;
            if(gamemode == GM_NORMAL)
            {
                SetDividersActive(toggleBarrier.local_bool);

                for (int i = 0; i < players.Length; i++)
                {
                    players[i].SetRandomColor(false);

                    //players[i].cups.transform.parent = players[i].cups.anchorGamemodeNormal;
                    //players[i].cups.transform.localPosition = Vector3.zero;
                    //players[i].cups.transform.localRotation = Quaternion.identity;
                    players[i].cups.overwriteShape = -1;

                    players[i].cups.overwriteRows = i < playerCountSlider.local_float ? -1 : 0;
                    players[i].cups.ResetGlassesIfOwner(1, 0, Vector3.zero);
                }
            }else if(gamemode == GM_KING_HILL)
            {
                SetDividersActive(false);

                float height = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].SetRandomColor(true);

                    int rows = 10 - i * 2;
                    Vector3 zOffsetPerCup = Vector3.back * players[i].cups.GetUnscaledLength();
                    //players[i].cups.transform.parent = kingOfTheHillParent;
                    //players[i].cups.transform.localPosition = Vector3.up * height + zOffsetPerCup * (rows / 2.0f);
                    //players[i].cups.transform.localRotation = Quaternion.identity;
                    players[i].cups.overwriteShape = 2;
                    players[i].cups.overwriteRows = rows;

                    players[i].cups.ResetGlassesIfOwner(1, 1, Vector3.up * height + zOffsetPerCup * (rows / 2.0f));

                    height += players[i].cups.GetHeight();
                }
            }else if(gamemode == GM_BIG_PONG)
            {
                SetDividersActive(false);
                for (int i = 1; i < players.Length; i++)
                {
                    players[i].cups.overwriteRows = 0;
                    players[i].cups.ResetGlassesIfOwner(1, 0, Vector3.zero);
                }
                //players[0].cups.transform.parent = kingOfTheHillParent;
                //players[0].cups.transform.localPosition = Vector3.zero;
                //players[0].cups.transform.localRotation = Quaternion.identity;
                players[0].cups.overwriteRows = 1;
                players[0].cups.ResetGlassesIfOwner(10, 2, Vector3.zero);
            }
            ResetBalls();
        }

        void ResetBalls()
        {
            if(gamemode == GM_NORMAL || gamemode == GM_BIG_PONG)
            {
                for (int i = 1; i < balls.Length; i++)
                    balls[i].gameObject.SetActive(false);
                balls[0].gameObject.SetActive(true);
                balls[0]._SetColor();
                if (Networking.LocalPlayer.isMaster) balls[0].Respawn();
            }else if(gamemode == GM_KING_HILL)
            {
                for (int i = 0; i < balls.Length; i++)
                {
                    balls[i].gameObject.SetActive(i < playerCountSlider.local_float);
                    balls[i].currentPlayer = i;
                    balls[i]._SetColor();
                    if (Networking.LocalPlayer.isMaster) balls[i].Respawn();
                }

                teamIndicatorMaterial.color = Color.white;
                teamIndicatorMaterial.SetColor("_EmissionColor", Color.white);
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
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i].gameObject.activeSelf == false) continue;
                float angle = Vector3.Angle(compareVec, players[i].transform.forward);
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

        public void ChangeAmountOfPlayers()
        {
            if (playerCountSlider.local_float <= tables.Length && playerCountSlider.local_float <= players.Length)
            {
                //activate correct table
                for (int i = 0; i < tables.Length; i++)
                {
                    if (tables[i] != null) tables[i].SetActive(i == playerCountSlider.local_float);
                }
                // teleport players to correct positions
                bool doTeleportPlayer = Networking.IsOwner(playerCountSlider.gameObject);
                int localPlayerIndex =  DetermineLocalPlayerIndex();
                //activate teams
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].gameObject.SetActive(i < playerCountSlider.local_float);
                    //set positions
                    if (i < playerCountSlider.local_float)
                    {
                        if (playerCountSlider.local_float == 2)
                        {
                            ChangePlayerTransform(players[i].transform, twoTeamPositions[i].position, twoTeamPositions[i].rotation, doTeleportPlayer && i == localPlayerIndex);
                        } else if (playerCountSlider.local_float == 3)
                        {
                            ChangePlayerTransform(players[i].transform, threeTeamPositions[i].position, threeTeamPositions[i].rotation, doTeleportPlayer && i == localPlayerIndex);
                        } else if (playerCountSlider.local_float == 4)
                        {
                            ChangePlayerTransform(players[i].transform, fourTeamPositions[i].position, fourTeamPositions[i].rotation, doTeleportPlayer && i == localPlayerIndex);
                        }
                    }
                    //Setup cups
                    if(gamemode == GM_NORMAL && i < playerCountSlider.local_float && players[i].cups.existing_glasses_count == 0)
                    {
                        players[i].cups.overwriteRows = -1;
                        players[i].cups.ResetGlassesIfOwner(1, 0, Vector3.zero);
                    }
                }
                ResetBalls();
            }
        }
    }
}
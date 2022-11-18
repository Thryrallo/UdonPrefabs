
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

        public Transform kingOfTheHillParent;

        [HideInInspector]
        public float aimAssist;

        private int teamSize = 1;
        [HideInInspector]
        public int gamemode;

        public const int GM_NORMAL = 0;
        public const int GM_KING_HILL = 1;
        public const int GM_BIG_PONG = 2;

        void Start()
        {
            players = new ThryBP_Player[playersParent.childCount];

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

        public void CountCupHit(int cupOwner, int row, int collum, int scoreingPlayer, int scoreType)
        {
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
                    Vector3 zOffsetPerCup = Vector3.back * players[i].cups.GetLength();
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

        public void ChangeAmountOfPlayers()
        {
            if (playerCountSlider.local_float <= tables.Length && playerCountSlider.local_float <= players.Length)
            {
                //activate correct table
                for (int i = 0; i < tables.Length; i++)
                {
                    if (tables[i] != null) tables[i].SetActive(i == playerCountSlider.local_float);
                }
                //activate teams
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].gameObject.SetActive(i < playerCountSlider.local_float);
                    //set positions
                    if (i < playerCountSlider.local_float)
                    {
                        if (playerCountSlider.local_float == 2)
                        {
                            players[i].transform.SetPositionAndRotation(twoTeamPositions[i].position, twoTeamPositions[i].rotation);
                        } else if (playerCountSlider.local_float == 3)
                        {
                            players[i].transform.SetPositionAndRotation(threeTeamPositions[i].position, threeTeamPositions[i].rotation);
                        } else if (playerCountSlider.local_float == 4)
                        {
                            players[i].transform.SetPositionAndRotation(fourTeamPositions[i].position, fourTeamPositions[i].rotation);
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
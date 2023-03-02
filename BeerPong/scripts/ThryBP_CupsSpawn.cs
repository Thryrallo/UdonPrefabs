
using System;
using Thry.General;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.BeerPong
{
    public class ThryBP_CupsSpawn : UdonSharpBehaviour
    {
        public ThryBP_Glass Glass;

        public ThryBP_Player Player;

        // glasses as an array of 
        [UdonSynced]
        byte[] _cupOwnersAtPositions = new byte[0];

        [HideInInspector]
        public ThryBP_Glass[] ActiveGlassesGameObjects;

        [UdonSynced]
        public float Scale = 1;

        [UdonSynced]
        public int Anchor_id;

        [UdonSynced]
        public Vector3 Position_offset;

        Transform[] _anchors;

        //hit counting

        [UdonSynced]
        int _hit = 0;

        [HideInInspector]
        [UdonSynced]
        public int Existing_glasses_count = 0;

        //shape
        public ThryAction ShapeSelector;

        public const int SHAPE_TRIANGLE = 0;
        public const int SHAPE_SQUARE = 1;
        public const int SHAPE_CIRCLE = 2;

        //rows
        public UnityEngine.UI.Slider RowsSlider;

        const int MAX_ROWS = 11;
        int X_PER_ROW = 0;

        [HideInInspector]
        public int OverwriteShape = -1;
        [HideInInspector]
        public int OverwriteRows = -1;

        //score (is counted here cause the owner of this object is the one who hits a glass it)
        [UdonSynced]
        int _score;
        public UnityEngine.UI.Text[] UI_score_boards;

        const byte PLAYER_VALUE_NONE = 255;

        public void Init(Transform[] anchors)
        {
            Glass.InitPrefab();
            Glass.gameObject.SetActive(false);

            this._anchors = new Transform[anchors.Length];
            Array.Copy(anchors, this._anchors, anchors.Length);
            this._anchors[0] = transform.parent;

            //Reparenting to always active gameobject, else if parent is not active this will not sync values
            transform.SetParent(transform.parent.parent.parent, true);
            transform.name = Player.name + "_Cups";

            X_PER_ROW = MAX_ROWS + MAX_ROWS;
            _cupOwnersAtPositions = new byte[MAX_ROWS * X_PER_ROW];
            ActiveGlassesGameObjects = new ThryBP_Glass[MAX_ROWS * X_PER_ROW];
            // fill _glassOwnersAtPositions with PLAYER_VALUE_NONE
            for (int i = 0; i < _cupOwnersAtPositions.Length; i++)
            {
                _cupOwnersAtPositions[i] = PLAYER_VALUE_NONE;
            }
        }

        public float GetGlobalHeight()
        {
            return Glass.GlobalHeight;
        }

        public float GetLocalHeight()
        {
            return Glass.LocalHeight * Scale;
        }

        public float GetUnscaledLocalLength()
        {
            return Glass.LocalDiameter;
        }

        public ThryBP_Glass GetCup(int r, int c)
        {
            return ActiveGlassesGameObjects[r * X_PER_ROW + c];
        }

        public void RemoveCup(int row, int collum)
        {
            //Null check. I am not sure if it is feasible that this is even called before doesGlassExist is initilized, but just to be sure
            if (_cupOwnersAtPositions == null) return;
            if (Networking.IsOwner(gameObject) == false) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            int index = row * X_PER_ROW + collum;
            if (index < _cupOwnersAtPositions.Length)
            {
                _hit++;
                Existing_glasses_count--;
                _cupOwnersAtPositions[index] = PLAYER_VALUE_NONE;
                SyncGlasses();
                RequestSerialization();
            }
        }

        public Transform GetCupAnchor()
        {
            return _anchors[Anchor_id];
        }

        //========Networking========
        public override void OnDeserialization()
        {
            UpdateUI();
            //Check is done in case doesGlassExist was not synced yet and is instead null, which will cause problems
            if (_cupOwnersAtPositions != null) SyncGlasses();
        }

        //=======Score==========

        public void AddToScore(int a)
        {
            if (Networking.IsOwner(gameObject) == false) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _score += a;
            UpdateUI();
            RequestSerialization();
        }

        private void UpdateUI()
        {
            string s = _score.ToString("D4");
            foreach (UnityEngine.UI.Text t in UI_score_boards) t.text = s;
        }

        //=======Array reseting========

        public void Rerack()
        {
            if (Player.MainScript.Gamemode != 0) return;
            if (Networking.IsOwner(gameObject)) RerackOwner();
            else SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(RerackOwner));
        }

        public void RerackOwner()
        {
            //Remove all glasses
            for(int i = 0; i < _cupOwnersAtPositions.Length; i++)
            {
                _cupOwnersAtPositions[i] = PLAYER_VALUE_NONE;
            }

            //Replace glasses
            int shape = (int)ShapeSelector.local_float;
            int remove = 0;
            int rows = 0;
            switch (shape)
            {
                case SHAPE_SQUARE:
                    rows = (int)Mathf.Sqrt(Existing_glasses_count - 0.1f) + 1;
                    remove = PlaceCupsAsSquare(rows) - Existing_glasses_count;
                    break;
                case SHAPE_CIRCLE:
                    rows = (int)Mathf.Sqrt((Existing_glasses_count-0.1f) * 2) + 1;
                    remove = PlaceCupsAsCircle(rows) - Existing_glasses_count;
                    break;
                default:
                    //sqrt(2 x+0.25)-0.5
                    rows = (int)(Mathf.Sqrt(2 * (Existing_glasses_count - 0.1f) + 0.25f) - 0.5f) + 1;
                    remove = PlaceCupsAsPyramid(rows) - Existing_glasses_count;
                    break;
            }

            //Remove glasses placed too many
            for (int i = _cupOwnersAtPositions.Length-1; i >= 0; i--)
            {
                if(_cupOwnersAtPositions[i] != PLAYER_VALUE_NONE && remove > 0)
                {
                    _cupOwnersAtPositions[i] = PLAYER_VALUE_NONE;
                    remove--;
                    if (remove == 0) break;
                }
            }

            SyncGlasses();
            RequestSerialization();
        }

        public void ResetGlasses()
        {
            if (Networking.IsOwner(gameObject)) ResetGlassesOwner();
            else SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(ResetGlassesOwner));
        }

        public void ResetGlassesIfNoneHit()
        {
            if (!Networking.IsOwner(gameObject)) return;
            if (_hit > 0) return;
            ResetGlassesOwner();
        }

        public void ResetCupsIfOwner(float scale, int anchor_id, Vector3 position_offset)
        {
            if (!Networking.IsOwner(gameObject)) return;
            this.Scale = scale;
            this.Anchor_id = anchor_id;
            this.Position_offset = position_offset;
            ResetGlassesOwner();
        }

        public void ResetGlassesOwner()
        {
            if(Networking.LocalPlayer == null) return; // prevent null ref on upload
            //Remove all glasses
            for(int i = 0; i < _cupOwnersAtPositions.Length; i++)
            {
                _cupOwnersAtPositions[i] = PLAYER_VALUE_NONE;
            }

            int placeRows = (int)RowsSlider.value;
            int shape = (int)ShapeSelector.local_float;

            if (OverwriteRows > -1) placeRows = OverwriteRows;
            if (OverwriteShape > -1) shape = OverwriteShape;

            switch (shape)
            {
                case SHAPE_SQUARE:
                    Existing_glasses_count = PlaceCupsAsSquare(placeRows);
                    break;
                case SHAPE_CIRCLE:
                    Existing_glasses_count = PlaceCupsAsCircle(placeRows);
                    break;
                default:
                    Existing_glasses_count = PlaceCupsAsPyramid(placeRows);
                    break;
            }
            if(Player.MainScript.Gamemode == ThryBP_Main.GM_MAYHEM)
            {
                RandomizeCupOwnersOfExistingCups();
            }
            _score = 0;
            _hit = 0;
            UpdateUI();
            SyncGlasses();
            RequestSerialization();
        }

        private void RandomizeCupOwnersOfExistingCups()
        {
            int max = Player.MainScript.players.Length;
            for (int i = 0; i < _cupOwnersAtPositions.Length; i++)
            {
                if (_cupOwnersAtPositions[i] != PLAYER_VALUE_NONE)
                {
                    _cupOwnersAtPositions[i] = (byte)UnityEngine.Random.Range(0, max);
                }
            }
        }

        private int PlaceCupsAsPyramid(int placeRows)
        {
            int existing_glasses_count = 0;
            for (int r = 0; r < MAX_ROWS; r++)
            {
                bool evenRow = (placeRows - r) % 2 == 0;
                int cupsInRow = (placeRows - r + 1);

                for (int c = 0; c < X_PER_ROW; c++)
                {
                    if (r < placeRows)
                    {
                        bool evenCollum = c % 2 == 0;

                        bool isInTriangle = Mathf.Abs(c - MAX_ROWS) < cupsInRow;
                        bool isCup = (evenRow == evenCollum) && isInTriangle;

                        if (isCup)
                        {
                            _cupOwnersAtPositions[r * X_PER_ROW + c] = (byte)Player.PlayerIndex;
                            existing_glasses_count++;
                        }
                    }
                }
            }
            return existing_glasses_count;
        }

        private int PlaceCupsAsSquare(int placeRows)
        {
            int existing_glasses_count = 0;
            bool placeOnEvens = placeRows % 2 == 0;
            for (int r = 0; r < MAX_ROWS; r++)
            {
                for (int c = 0; c < X_PER_ROW; c++)
                {
                    if (r < placeRows)
                    {
                        bool evenCollum = c % 2 == 0;

                        bool isCup;
                        bool isInSquare = Mathf.Abs(c - MAX_ROWS) < placeRows;
                        isCup = (evenCollum == placeOnEvens) && isInSquare;

                        if (isCup)
                        {
                            _cupOwnersAtPositions[r * X_PER_ROW + c] = (byte)Player.PlayerIndex;
                            existing_glasses_count++;
                        }
                    }
                }
            }
            return existing_glasses_count;
        }

        private int PlaceCupsAsCircle(int placeRows)
        {
            int existing_glasses_count = 0;
            bool isRowsCountEven = placeRows % 2 == 0;
            float radius = placeRows / 2;
            float middleOfCircleY = radius;
            if (isRowsCountEven)
            {
                middleOfCircleY -= 0.5f;
            }
            bool placeOnEven;
            for (int r = 0; r < MAX_ROWS; r++)
            {
                bool evenRow = (placeRows - r) % 2 == 0;
                if (isRowsCountEven)
                {
                    placeOnEven = isRowsCountEven == evenRow;
                    if (r > middleOfCircleY) placeOnEven = !placeOnEven;//mirror for rows bigger than radius
                }
                else
                {
                    placeOnEven = isRowsCountEven == !evenRow;
                }
                for (int c = 0; c < X_PER_ROW; c++)
                {
                    if (r < placeRows && (c % 2 == 0) == placeOnEven)
                    {
                        float x = (c - MAX_ROWS) / 2.0f;
                        float y = Mathf.Abs(r - middleOfCircleY);
                        if (y < 1) y = 0; // done to prevent weird bumps in the sides

                        bool isInCircle = Mathf.Pow(x, 2) + Mathf.Pow(y, 2) <= Mathf.Pow(radius, 2);

                        if (isInCircle)
                        {
                            _cupOwnersAtPositions[r * X_PER_ROW + c] = (byte)Player.PlayerIndex;
                            existing_glasses_count++;
                        }
                    }
                }
            }
            return existing_glasses_count;
        }

        //=======Glasses syncing========

        int syncR = 0;
        int syncC = 0;
        bool syncing = false;
        public void SyncGlasses()
        {
            syncR = 0;
            syncC = 0;
            if (syncing == false)
            {
                _SyncGlass();
            }
            syncing = true;
        }

        public void _SyncGlass()
        {
            int index = syncR * X_PER_ROW + syncC;
            if(index >= _cupOwnersAtPositions.Length)
            {
                syncing = false;
                Debug.Log("[BeerPong][Error] doesGlassExist index > doesGlassExist.length");
                return;
            }
            if (_cupOwnersAtPositions[index] != PLAYER_VALUE_NONE)
            {
                if (Utilities.IsValid(ActiveGlassesGameObjects[index]))
                {
                    // Cup is Incorrect
                    if( ActiveGlassesGameObjects[index].PlayerCupOwner.PlayerIndex != _cupOwnersAtPositions[index]
                        || ActiveGlassesGameObjects[index].transform.localScale.x != Scale 
                        || ActiveGlassesGameObjects[index].Anchor_id != Anchor_id
                        || ActiveGlassesGameObjects[index].Position_offset != Position_offset)
                    {
                        Destroy(ActiveGlassesGameObjects[index].gameObject);
                        InstanciateCup(syncR, syncC, index, _cupOwnersAtPositions[index]);
                    }
                }
                else
                {
                    InstanciateCup(syncR, syncC, index, _cupOwnersAtPositions[index]);
                }
            }
            else
            {
                if (Utilities.IsValid(ActiveGlassesGameObjects[index]))
                {
                    Destroy(ActiveGlassesGameObjects[index].gameObject);
                }
            }
            syncC++;
            if (syncC == X_PER_ROW) { syncR++; syncC = 0; }
            if (syncR < MAX_ROWS) SendCustomEventDelayedFrames(nameof(_SyncGlass), 1);
            else syncing = false;
        }

        public void RemoveLocalGlasses()
        {
            for (int r = 0; r < MAX_ROWS; r++)
            {
                for (int c = 0; c < X_PER_ROW; c++)
                {
                    int index = r * X_PER_ROW + c;
                    if (Utilities.IsValid(ActiveGlassesGameObjects[index]))
                    {
                        Destroy(ActiveGlassesGameObjects[index].gameObject);
                    }
                }
            }
        }

        public void SyncColor()
        {
            if (Player.DoRandomCupColor) return; //Dont sync colors if color should be random => doenst really need to be in those cases. makes for better effect sometimes if it doesnt swap
            for (int r = 0; r < MAX_ROWS; r++)
            {
                for (int c = 0; c < X_PER_ROW; c++)
                {
                    int index = r * X_PER_ROW + c;
                    if (_cupOwnersAtPositions[index] != PLAYER_VALUE_NONE && Utilities.IsValid(ActiveGlassesGameObjects[index]))
                    {
                        ActiveGlassesGameObjects[index].UpdateColor(index);
                    }
                }
            }
        }

        //Instanciating

        public void InstanciateCup(int row, int collum, int index, int cupOwner)
        {
            float relativeX = (float)collum - MAX_ROWS;
            Vector3 position = new Vector3(relativeX * Glass.LocalRadius * Scale, 0, row * Glass.LocalDiameter * Scale);
            
            GameObject instance = Instantiate(Glass.gameObject);
            instance.transform.SetParent(_anchors[Anchor_id]);
            instance.transform.localPosition = position + Position_offset;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one * Scale;
            instance.SetActive(true);

            ThryBP_Glass cup = instance.GetComponent<ThryBP_Glass>();
            cup.Row = row;
            cup.Column = collum;
            cup.PlayerCupOwner = Player.MainScript.players[cupOwner];
            cup.PlayerAnchorSide = Player;
            cup.Anchor_id = Anchor_id;
            cup.Position_offset = Position_offset;
            cup.TableScaleTransform = Player.MainScript.transform;

            cup.LocalRadius = Glass.LocalRadius * Scale;
            cup.LocalDiameter = Glass.LocalDiameter * Scale;
            cup.LocalCircumfrence = Glass.LocalCircumfrence * Scale;
            cup.LocalHeight = Glass.LocalHeight * Scale;
            
            ActiveGlassesGameObjects[index] = cup;

            cup.UpdateColor(index);
        }
    }
}

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
        public ThryBP_Glass glass;

        public ThryBP_Player _player;

        //glasses
        [UdonSynced]
        bool[] doesGlassExist = new bool[0];

        [HideInInspector]
        public ThryBP_Glass[] activeGlassesGameObjects;

        [UdonSynced]
        public float scale = 1;

        [UdonSynced]
        public int anchor_id;

        [UdonSynced]
        public Vector3 position_offset;

        Transform[] anchors;

        //hit counting

        [UdonSynced]
        int hit = 0;

        [HideInInspector]
        [UdonSynced]
        public int existing_glasses_count = 0;

        //shape
        public ThryAction shapeSelector;

        const int SHAPE_TRIANGLE = 0;
        const int SHAPE_SQUARE = 1;
        const int SHAPE_CIRCLE = 2;

        //rows
        public UnityEngine.UI.Slider rowsSlider;

        const int MAX_ROWS = 11;
        int X_PER_ROW = 0;

        [HideInInspector]
        public int overwriteShape = -1;
        [HideInInspector]
        public int overwriteRows = -1;

        //score (is counted here cause the owner of this object is the one who hits a glass it)
        [UdonSynced]
        int score;
        public UnityEngine.UI.Text[] ui_score_boards;

        public void Init(Transform[] anchors)
        {
            glass.InitPrefab();
            glass.gameObject.SetActive(false);

            this.anchors = new Transform[anchors.Length];
            Array.Copy(anchors, this.anchors, anchors.Length);
            this.anchors[0] = transform.parent;

            //Reparenting to always active gameobject, else if parent is not active this will not sync values
            transform.SetParent(transform.parent.parent.parent, true);
            transform.name = _player.name + "_Cups";

            X_PER_ROW = MAX_ROWS + MAX_ROWS;
            doesGlassExist = new bool[MAX_ROWS * X_PER_ROW];
            activeGlassesGameObjects = new ThryBP_Glass[MAX_ROWS * X_PER_ROW];
        }

        public float GetHeight()
        {
            return glass.GetBounds().size.y;
        }

        public float GetLength()
        {
            return glass.GetBounds().size.z;
        }

        public ThryBP_Glass GetCup(int r, int c)
        {
            return activeGlassesGameObjects[r * X_PER_ROW + c];
        }

        public void RemoveCup(int row, int collum)
        {
            //Null check. I am not sure if it is feasible that this is even called before doesGlassExist is initilized, but just to be sure
            if (doesGlassExist == null) return;
            if (Networking.IsOwner(gameObject) == false) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            int index = row * X_PER_ROW + collum;
            if (index < doesGlassExist.Length)
            {
                hit++;
                existing_glasses_count--;
                doesGlassExist[index] = false;
                SyncGlasses();
                RequestSerialization();
            }
        }

        //========Networking========
        public override void OnDeserialization()
        {
            UpdateUI();
            //Check is done in case doesGlassExist was not synced yet and is instead null, which will cause problems
            if (doesGlassExist != null) SyncGlasses();
        }

        //=======Score==========

        public void AddToScore(int a)
        {
            if (Networking.IsOwner(gameObject) == false) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            score += a;
            UpdateUI();
            RequestSerialization();
        }

        private void UpdateUI()
        {
            string s = score.ToString();
            for (int i = s.Length; i < 4; i++) s = "0" + s;
            foreach (UnityEngine.UI.Text t in ui_score_boards) t.text = s;
        }

        //=======Array reseting========

        public void Rerack()
        {
            if (_player._mainScript.gamemode != 0) return;
            if (Networking.IsOwner(gameObject)) RerackOwner();
            else SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(RerackOwner));
        }

        public void RerackOwner()
        {
            //Remove all glasses
            for(int i = 0; i < doesGlassExist.Length; i++)
            {
                doesGlassExist[i] = false;
            }

            //Replace glasses
            int shape = (int)shapeSelector.local_float;
            int remove = 0;
            int rows = 0;
            switch (shape)
            {
                case SHAPE_SQUARE:
                    rows = (int)Mathf.Sqrt(existing_glasses_count - 0.1f) + 1;
                    remove = PlaceCupsAsSquare(rows) - existing_glasses_count;
                    break;
                case SHAPE_CIRCLE:
                    rows = (int)Mathf.Sqrt((existing_glasses_count-0.1f) * 2) + 1;
                    remove = PlaceCupsAsCircle(rows) - existing_glasses_count;
                    break;
                default:
                    //sqrt(2 x+0.25)-0.5
                    rows = (int)(Mathf.Sqrt(2 * (existing_glasses_count - 0.1f) + 0.25f) - 0.5f) + 1;
                    remove = PlaceCupsAsPyramid(rows) - existing_glasses_count;
                    break;
            }

            //Remove glasses placed too many
            for (int i = doesGlassExist.Length-1; i >= 0; i--)
            {
                if(doesGlassExist[i] && remove > 0)
                {
                    doesGlassExist[i] = false;
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
            if (hit > 0) return;
            ResetGlassesOwner();
        }

        public void ResetGlassesIfOwner(float scale, int anchor_id, Vector3 position_offset)
        {
            if (!Networking.IsOwner(gameObject)) return;
            this.scale = scale;
            this.anchor_id = anchor_id;
            this.position_offset = position_offset;
            ResetGlassesOwner();
        }

        public void ResetGlassesOwner()
        {
            int placeRows = (int)rowsSlider.value;
            int shape = (int)shapeSelector.local_float;

            if (overwriteRows > -1) placeRows = overwriteRows;
            if (overwriteShape > -1) shape = overwriteShape;

            switch (shape)
            {
                case SHAPE_SQUARE:
                    existing_glasses_count = PlaceCupsAsSquare(placeRows);
                    break;
                case SHAPE_CIRCLE:
                    existing_glasses_count = PlaceCupsAsCircle(placeRows);
                    break;
                default:
                    existing_glasses_count = PlaceCupsAsPyramid(placeRows);
                    break;
            }
            score = 0;
            hit = 0;
            UpdateUI();
            SyncGlasses();
            RequestSerialization();
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

                        doesGlassExist[r * X_PER_ROW + c] = isCup;
                        if (isCup) existing_glasses_count++;
                    }
                    else
                    {
                        doesGlassExist[r * X_PER_ROW + c] = false;
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

                        doesGlassExist[r * X_PER_ROW + c] = isCup;
                        if (isCup) existing_glasses_count++;
                    }
                    else
                    {
                        doesGlassExist[r * X_PER_ROW + c] = false;
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

                        doesGlassExist[r * X_PER_ROW + c] = isInCircle;
                        if (isInCircle) existing_glasses_count++;
                    }
                    else
                    {
                        doesGlassExist[r * X_PER_ROW + c] = false;
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
            if(index >= doesGlassExist.Length)
            {
                syncing = false;
                Debug.Log("[BeerPong][Error] doesGlassExist index > doesGlassExist.length");
                return;
            }
            if (doesGlassExist[index])
            {
                if (Utilities.IsValid(activeGlassesGameObjects[index]))
                {
                    //Scale incorrect
                    if(activeGlassesGameObjects[index].transform.localScale.x != scale 
                        || activeGlassesGameObjects[index].anchor_id != anchor_id
                        || activeGlassesGameObjects[index].position_offset != position_offset)
                    {
                        Destroy(activeGlassesGameObjects[index].gameObject);
                        InstanciateGlass(syncR, syncC, index);
                    }
                }
                else
                {
                    InstanciateGlass(syncR, syncC, index);
                }
            }
            else
            {
                if (Utilities.IsValid(activeGlassesGameObjects[index]))
                {
                    Destroy(activeGlassesGameObjects[index].gameObject);
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
                    if (Utilities.IsValid(activeGlassesGameObjects[index]))
                    {
                        Destroy(activeGlassesGameObjects[index].gameObject);
                    }
                }
            }
        }

        public void SyncColor()
        {
            if (_player.randomColor) return; //Dont sync colors if color should be random => doenst really need to be in those cases. makes for better effect sometimes if it doesnt swap
            for (int r = 0; r < MAX_ROWS; r++)
            {
                for (int c = 0; c < X_PER_ROW; c++)
                {
                    int index = r * X_PER_ROW + c;
                    if (doesGlassExist[index] && Utilities.IsValid(activeGlassesGameObjects[index]))
                    {
                        SetColor(activeGlassesGameObjects[index], index);
                    }
                }
            }
        }

        //Instanciating

        public void InstanciateGlass(int row, int collum, int index)
        {
            float relativeX = (float)collum - MAX_ROWS;
            Vector3 position = new Vector3(relativeX * glass.radius * scale, 0, row * glass.diameter * scale);
            
            GameObject instance = VRCInstantiate(glass.gameObject);
            instance.transform.SetParent(anchors[anchor_id]);
            instance.transform.localPosition = position + position_offset;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one * scale;
            instance.SetActive(true);

            ThryBP_Glass thryglass = instance.GetComponent<ThryBP_Glass>();
            thryglass.row = row;
            thryglass.collum = collum;
            thryglass.player = _player;
            thryglass.anchor_id = anchor_id;
            thryglass.position_offset = position_offset;
            thryglass.radius = glass.radius;
            thryglass.diameter = glass.diameter;
            thryglass.circumfrence = glass.circumfrence;
            
            activeGlassesGameObjects[index] = thryglass;

            SetColor(thryglass, index);
        }

        //=======Array syncing========

        public void SetColor(ThryBP_Glass g, int index)
        {
            if (_player.randomColor)
            {
                UnityEngine.Random.InitState(index);
                g.SetColor(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
            }
            else
            {
                g.SetColor(_player.playerColor);
            }
        }

    }
}
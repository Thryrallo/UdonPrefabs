
using JetBrains.Annotations;
using System;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.BeerPong
{
    public enum CupsShape
    {
        None = -1,
        Triangle = 0,
        Square = 1,
        Circle = 2,
        HashTag = 3,
    }
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class CupsManager : SimpleUISetterExtension
    {
        const byte PLAYER_VALUE_NONE = 255;
        const int MAX_ROWS = 11;
        int X_PER_ROW = 0;

        public Cup Glass;
        public Player Player;

        Cup[] _activeCupsByPosition; // ordered by position
        Cup[] _activeCupsList; // ordered active cups at the sart if list
        int _activeCupsLength = 0;
        
        // glasses as an array of 
        [UdonSynced] byte[] _cupOwnersAtPositions = new byte[0];
        [UdonSynced] float _scale = 1;
        [UdonSynced] int _anchor_id;
        [UdonSynced] Vector3 _position_offset;
        [UdonSynced] int _hit = 0;
        [UdonSynced] int _existing_glasses_count = 0;

        Transform[] _anchors;

        CupsShape _overwriteShape = CupsShape.None;
        int _overwriteRows = -1;

        public Cup[] List => _activeCupsList;
        public int Length => _activeCupsLength;
        public int SyncedCount => _existing_glasses_count;
        public float GlobalHeight => Glass.GlobalHeight;
        public float LocalHeight => Glass.LocalHeight * _scale;
        public float UnscaledLocalLength => Glass.LocalDiameter;
        public Transform CupAnchor => _anchors[_anchor_id];
        
        protected override string LogPrefix => "Thry.BeerPong.CupsManager";
        
        public void Init(Transform[] anchors)
        {
            Glass.InitPrefab();
            Glass.gameObject.SetActive(false);

            this._anchors = Collections.Duplicate(anchors);
            this._anchors[0] = transform.parent;

            //Reparenting to always active gameobject, else if parent is not active this will not sync values
            transform.SetParent(transform.parent.parent.parent, true);
            transform.name = Player.name + "_Cups";

            X_PER_ROW = MAX_ROWS + MAX_ROWS;
            _cupOwnersAtPositions = new byte[MAX_ROWS * X_PER_ROW];
            _activeCupsByPosition = new Cup[MAX_ROWS * X_PER_ROW];
            _activeCupsList = new Cup[MAX_ROWS * X_PER_ROW];
            // fill _glassOwnersAtPositions with PLAYER_VALUE_NONE
            for (int i = 0; i < _cupOwnersAtPositions.Length; i++)
            {
                _cupOwnersAtPositions[i] = PLAYER_VALUE_NONE;
            }

            if(Networking.IsOwner(gameObject))
            {
                ResetGlasses();
            }
        }

        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(Shape))] public CupsShape _shape = 0;
        public CupsShape Shape
        {
            get => (CupsShape)_shape;
            set
            {
                _shape = value;
                VariableChangedFromBehaviour(nameof(_shape));
            }
        }

        [NonSerialized, UdonSynced, FieldChangeCallback(nameof(Rows))] public int _rows = 5;
        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                VariableChangedFromBehaviour(nameof(_rows));
            }
        }

        [PublicAPI]
        public void SetRows(int r)
        {
            Rows = r;
        }

        [PublicAPI]
        public void ResetGlasses()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            ResetGlassesInternal();
        }

        [PublicAPI]
        public void NetworkChanges()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            RequestSerialization();
        }

        [PublicAPI]
        public void Rerack()
        {
            if (Player.GameManager.Gamemode != GameMode.Normal) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            RerackInternal();
        }

        [PublicAPI]
        public void ChangeConfiguration(float scale, int anchor_id, Vector3 position_offset)
        {
            this._scale = scale;
            this._anchor_id = anchor_id;
            this._position_offset = position_offset;
        }

        [PublicAPI]
        public void SetOverwrite(CupsShape shape, int rows)
        {
            _overwriteRows = rows;
            _overwriteShape = shape;
        }

        [PublicAPI]
        public void SetOverwrite(int rows)
        {
            _overwriteRows = rows;
            _overwriteShape = CupsShape.None;
        }

        [PublicAPI]
        public void DisableOverwrite()
        {
            _overwriteRows = -1;
            _overwriteShape = CupsShape.None;
        }

        [PublicAPI]
        public Cup GetCup(int r, int c)
        {
            return _activeCupsByPosition[r * X_PER_ROW + c];
        }

        [PublicAPI]
        public void RemoveCup(int row, int collum)
        {
            //Null check. I am not sure if it is feasible that this is even called before doesGlassExist is initilized, but just to be sure
            if (_cupOwnersAtPositions == null) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            int index = row * X_PER_ROW + collum;
            if (index < _cupOwnersAtPositions.Length)
            {
                _hit++;
                _existing_glasses_count--;
                _cupOwnersAtPositions[index] = PLAYER_VALUE_NONE;
                SyncGlasses();
                RequestSerialization();
            }
        }

        //========Networking========
        public override void OnDeserialization()
        {
            //Check is done in case doesGlassExist was not synced yet and is instead null, which will cause problems
            if (_cupOwnersAtPositions != null) SyncGlasses();
        }

        //=======Array reseting========

        private void RerackInternal()
        {
            //Remove all glasses
            for(int i = 0; i < _cupOwnersAtPositions.Length; i++)
            {
                _cupOwnersAtPositions[i] = PLAYER_VALUE_NONE;
            }

            //Replace glasses
            int remove = 0;
            int rows = 0;
            switch (Shape)
            {
                case CupsShape.Square:
                    rows = (int)Mathf.Sqrt(_existing_glasses_count - 0.1f) + 1;
                    remove = PlaceCupsAsSquare(rows) - _existing_glasses_count;
                    break;
                case CupsShape.Circle:
                    rows = (int)Mathf.Sqrt((_existing_glasses_count-0.1f) * 2) + 1;
                    remove = PlaceCupsAsCircle(rows) - _existing_glasses_count;
                    break;
                case CupsShape.HashTag:
                    rows = (int)Mathf.Sqrt(_existing_glasses_count - 0.1f) + 1;
                    remove = PlaceCupsAsHashtag(rows) - _existing_glasses_count;
                    break;
                default:
                    //sqrt(2 x+0.25)-0.5
                    rows = (int)(Mathf.Sqrt(2 * (_existing_glasses_count - 0.1f) + 0.25f) - 0.5f) + 1;
                    remove = PlaceCupsAsPyramid(rows) - _existing_glasses_count;
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

        private void ResetGlassesInternal()
        {
            if(Networking.LocalPlayer == null) return; // prevent null ref on upload
            //Remove all glasses
            for(int i = 0; i < _cupOwnersAtPositions.Length; i++)
            {
                _cupOwnersAtPositions[i] = PLAYER_VALUE_NONE;
            }

            int placeRows = Rows;
            CupsShape shape = Shape;

            if (_overwriteRows > -1) placeRows = _overwriteRows;
            if (_overwriteShape != CupsShape.None) shape = _overwriteShape;

            switch (shape)
            {
                case CupsShape.Square:
                    _existing_glasses_count = PlaceCupsAsSquare(placeRows);
                    break;
                case CupsShape.Circle:
                    _existing_glasses_count = PlaceCupsAsCircle(placeRows);
                    break;
                case CupsShape.HashTag:
                    _existing_glasses_count = PlaceCupsAsHashtag(placeRows);
                    break;
                default:
                    _existing_glasses_count = PlaceCupsAsPyramid(placeRows);
                    break;
            }
            if(Player.GameManager.Gamemode == GameMode.Mayham)
            {
                RandomizeCupOwnersOfExistingCups();
            }
            
            _hit = 0;
            SyncGlasses();
            RequestSerialization();
        }

        private void RandomizeCupOwnersOfExistingCups()
        {
            int max = Player.GameManager.Players.Length;
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

        private int PlaceCupsAsHashtag(int placeRows)
        {
            int existing_glasses_count = 0;
            // always odd number of rows
            placeRows = (int)(placeRows / 2) * 2 + 1;
            bool oddCollumns = ((placeRows - 1) / 2) % 2 == 0;
            for (int r = 0; r < MAX_ROWS; r++)
            {
                for (int c = 0; c < X_PER_ROW; c++)
                {
                    if (r < placeRows)
                    {
                        bool isInSquare = Mathf.Abs(c - MAX_ROWS) < placeRows;
                        bool isCorrectRow = r % 2 == 1;
                        bool isCorrectCollumn = oddCollumns ? c % 4 == 1 : c % 4 == 3;
                        bool isInCorrectRowCollumn = c % 2 == 1;

                        if (((isCorrectRow && isInCorrectRowCollumn) || (!isCorrectRow && isCorrectCollumn)) && isInSquare)
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
                if (Utilities.IsValid(_activeCupsByPosition[index]))
                {
                    // Cup is Incorrect
                    if( _activeCupsByPosition[index].PlayerCupOwner.PlayerIndex != _cupOwnersAtPositions[index]
                        || _activeCupsByPosition[index].transform.localScale.x != _scale 
                        || _activeCupsByPosition[index].Anchor_id != _anchor_id
                        || _activeCupsByPosition[index].Position_offset != _position_offset)
                    {
                        int indexList = Array.IndexOf(_activeCupsList, _activeCupsByPosition[index]);
                        Destroy(_activeCupsByPosition[index].gameObject);
                        InstanciateCup(syncR, syncC, index, _cupOwnersAtPositions[index]);
                        _activeCupsList[indexList] = _activeCupsByPosition[index];
                    }
                }
                else
                {
                    InstanciateCup(syncR, syncC, index, _cupOwnersAtPositions[index]);
                }
            }
            else
            {
                if (Utilities.IsValid(_activeCupsByPosition[index]))
                {
                    DestroyCup(_activeCupsByPosition[index]);
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
                    if (Utilities.IsValid(_activeCupsByPosition[index]))
                    {
                        DestroyCup(_activeCupsByPosition[index]);
                    }
                }
            }
        }

        public void SyncColor()
        {
            if (Player.DoRandomColor) return; //Dont sync colors if color should be random => doenst really need to be in those cases. makes for better effect sometimes if it doesnt swap
            for (int r = 0; r < MAX_ROWS; r++)
            {
                for (int c = 0; c < X_PER_ROW; c++)
                {
                    int index = r * X_PER_ROW + c;
                    if (_cupOwnersAtPositions[index] != PLAYER_VALUE_NONE && Utilities.IsValid(_activeCupsByPosition[index]))
                    {
                        _activeCupsByPosition[index].UpdateColor(index);
                    }
                }
            }
        }

        //Instanciating

        public void DestroyCup(Cup cup)
        {
            // Remove from list
            int index = Array.IndexOf(_activeCupsList, cup);
            _activeCupsList[index] = _activeCupsList[--_activeCupsLength];
            Destroy(cup.gameObject);
        }

        public void InstanciateCup(int row, int collum, int index, int cupOwner)
        {
            float relativeX = (float)collum - MAX_ROWS;
            Vector3 position = new Vector3(relativeX * Glass.LocalRadius * _scale, 0, row * Glass.LocalDiameter * _scale);
            
            GameObject instance = Instantiate(Glass.gameObject);
            instance.transform.SetParent(_anchors[_anchor_id]);
            instance.transform.localPosition = position + _position_offset;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one * _scale;
            instance.SetActive(true);

            Cup cup = instance.GetComponent<Cup>();
            cup.Row = row;
            cup.Column = collum;
            cup.PlayerCupOwner = Player.GameManager.Players[cupOwner];
            cup.PlayerAnchorSide = Player;
            cup.Anchor_id = _anchor_id;
            cup.Position_offset = _position_offset;
            cup.TableScaleTransform = Player.GameManager.transform;

            cup.LocalRadius = Glass.LocalRadius * _scale;
            cup.LocalDiameter = Glass.LocalDiameter * _scale;
            cup.LocalCircumfrence = Glass.LocalCircumfrence * _scale;
            cup.LocalHeight = Glass.LocalHeight * _scale;
            
            _activeCupsByPosition[index] = cup;
            _activeCupsList[_activeCupsLength++] = cup;

            cup.UpdateColor(index);
        }
    }
}
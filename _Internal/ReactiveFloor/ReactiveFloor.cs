using Thry.Udon.AvatarTheme;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon
{
    public class ReactiveFloor : UdonSharpBehaviour
    {
        readonly string PROP_NAME_ARRAY_LENTH = "_RF_ArrayLength";
        readonly string PROP_NAME_ARRAY = "_ReactivePositions";
        readonly string PROP_NAME_COLOR1_ARRAY = "_ReactiveColors1";
        readonly string PROP_NAME_COLOR2_ARRAY = "_ReactiveColors2";
        readonly string PROP_NAME_SPECIAL = "_ReactiveSpecial";

        private VRCPlayerApi[] _playerList = new VRCPlayerApi[84];
        private Vector4[] _positionList = new Vector4[100];
        private Color[] _colorList1 = new Color[100];
        private Color[] _colorList2 = new Color[100];
        private Vector4[] _specialList = new Vector4[100];
        private int _length = 0;

        [SerializeField]
        private MeshRenderer _renderer;
        [SerializeField]
        private Material _material;
        [SerializeField]
        private Collider _collider;

        private Material _targetMaterial;
        [SerializeField, HideInInspector] AvatarThemeColor _avatarTheme;

        private string[] SPECIAL_PLAYERS = new string[] {
        "Thryrallo",
        "Thry",
        "Katy"
    };
        private float[] SPECIAL_PLAYERS_SPEED = new float[] {
        0.2f,
        -1f,
        2
    };
        const bool ENABLE_SPECIAL = true;

        private void Start()
        {
            for (int i = 0; i < _renderer.sharedMaterials.Length; i++)
            {
                if (_renderer.materials[i].name.StartsWith(_material.name))
                {
                    _targetMaterial = _renderer.materials[i];
                    break;
                }
            }
        }

        private void Update()
        {
            for (int i = 0; i < _length; i++)
            {
                if (_playerList[i].IsValid() == false)
                {
                    RemoveIndex(i);
                    break;
                }
                _positionList[i] = _playerList[i].GetPosition();
            }

            // validate positions, one each frame, 
            // (respawns or other teleports can cause players to be outside of the collider without triggering the exit event)
            // if(length > 0)
            // {
            //     _validateIndex = (_validateIndex + 1) % length;
            //     Vector3 pos = positionList[_validateIndex];
            //     if(_collider.bounds.Contains(pos) == false)
            //     {
            //         RemoveIndex(_validateIndex);
            //     }
            // }

            _targetMaterial.SetVectorArray(PROP_NAME_ARRAY, _positionList);
        }

        public override void OnPlayerTriggerEnter(VRC.SDKBase.VRCPlayerApi player)
        {
            Add(player);
        }
        public override void OnPlayerTriggerExit(VRC.SDKBase.VRCPlayerApi player)
        {
            Remove(player);
        }

        public override void OnPlayerLeft(VRC.SDKBase.VRCPlayerApi player)
        {
            Remove(player);
        }

        private void Add(VRCPlayerApi player)
        {
            string name = player.displayName;
            double sum = 1;
            for (int i = 0; i < name.Length; i++)
            {
                sum *= (name[i] + 1) / 100.0;
            }
            sum = sum % 1;
            Random.InitState((int)(sum * 100000));
            float hue = Random.Range(0f, 1f);
            _colorList1[_length] = Color.HSVToRGB(hue, 1f, 1f);
            if (_avatarTheme && _avatarTheme.GetColor(player, out Color c))
            {
                _colorList1[_length] = c;
                float s, v;
                Color.RGBToHSV(_colorList1[_length], out hue, out s, out v);
            }
            float hue2 = (hue + Random.Range(0.05f, 0.3f) * (Random.Range(0, 1) > 0.5 ? 1 : -1) + 1) % 1;
            _colorList2[_length] = Color.HSVToRGB(hue2, 1f, 1f);

            int index = System.Array.IndexOf(SPECIAL_PLAYERS, name);
            if (index >= 0 && ENABLE_SPECIAL)
            {
                float speed = Mathf.Clamp01(SPECIAL_PLAYERS_SPEED[index] / 20 + 0.5f);
                _specialList[_length] = new Vector4(1, speed, 0, 0);
            }
            else
                _specialList[_length] = Vector4.zero;

            _playerList[_length] = player;
            _length++;

            _targetMaterial.SetColorArray(PROP_NAME_COLOR1_ARRAY, _colorList1);
            _targetMaterial.SetColorArray(PROP_NAME_COLOR2_ARRAY, _colorList2);
            _targetMaterial.SetVectorArray(PROP_NAME_SPECIAL, _specialList);
            _targetMaterial.SetInteger(PROP_NAME_ARRAY_LENTH, _length);
        }

        private void Remove(VRCPlayerApi player)
        {
            for (int i = 0; i < _length; i++)
            {
                if (_playerList[i] == player)
                {
                    RemoveIndex(i);
                    return;
                }
            }
        }

        private void RemoveIndex(int i)
        {
            if (i < _length)
            {
                _playerList[i] = _playerList[--_length];
                _colorList1[i] = _colorList1[_length];
                _colorList2[i] = _colorList2[_length];
                _specialList[i] = _specialList[_length];
            }
            _targetMaterial.SetColorArray(PROP_NAME_COLOR1_ARRAY, _colorList1);
            _targetMaterial.SetColorArray(PROP_NAME_COLOR2_ARRAY, _colorList2);
            _targetMaterial.SetVectorArray(PROP_NAME_SPECIAL, _specialList);
            _targetMaterial.SetInteger(PROP_NAME_ARRAY_LENTH, _length);
        }
    }
}
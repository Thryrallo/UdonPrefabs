
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Thry.Udon.Mirror
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ThryMirror : UdonSharpBehaviour
    {
        public VRC_MirrorReflection[] MirrorReflections;
        protected Renderer[] _renderers;
        
        public Renderer[] Icons;
        public Animator MirrorAnimator;

        public Material MaterialNormal;
        public Material MaterialCutout;

        protected Shader _shaderNormal;
        protected Shader _shaderCutout;
        protected bool _useTransparencyShader;

        protected ThryMirrorMenu _menu;

        private bool _isInit = false;

        [HideInInspector] public VRC_MirrorReflection Main;
        [HideInInspector] public VRC_MirrorReflection Front;
        [HideInInspector] public VRC_MirrorReflection Top;
        [HideInInspector] public VRC_MirrorReflection Left;
        [HideInInspector] public VRC_MirrorReflection Right;

        void Start()
        {
            Init();
        }

        void Init()
        {
            if (_isInit)
                return;
            _shaderNormal = MaterialNormal.shader;
            _shaderCutout = MaterialCutout.shader;
            _renderers = new Renderer[MirrorReflections.Length];
            for (int i = 0; i < MirrorReflections.Length; i++)
            {
                _renderers[i] = MirrorReflections[i].GetComponent<Renderer>();
                if (_renderers[i].name.Contains("Front"))
                    Front = MirrorReflections[i];
                else if (_renderers[i].name.Contains("Top"))
                    Top = MirrorReflections[i];
                else if (_renderers[i].name.Contains("Left"))
                    Left = MirrorReflections[i];
                else if (_renderers[i].name.Contains("Right"))
                    Right = MirrorReflections[i];
            }
            if (Front)
                Main = Front;
            else
                Main = MirrorReflections[0];

            _isInit = true;
        }

        public ThryMirrorMenu Menu
        {
            get { return _menu; }
            set { _menu = value; }
        }

        public bool IsAnyActive
        {
            get
            {
                Init();
                foreach (VRC_MirrorReflection mirror in MirrorReflections)
                {
                    if (mirror.enabled)
                        return true;
                }
                return false;
            }
        }

        public bool IsMappedMultiMirror
        {
            get
            {
                Init();
                return Top != null && Left != null && Right != null;
            }
        }

        [PublicAPI]
        public void Set(int mask, MirrorType type, bool doCutout, float transparency)
        {
            transparency = Mathf.Clamp01(transparency * 1.1f);
            doCutout = doCutout && type != MirrorType.FULL;
            _useTransparencyShader = transparency < 1 || doCutout;

            SetLayerMask(mask);
            ApplyMaterialSettings(transparency, doCutout);
            // okay, look. this is dumb, but when a mirror is initially enabled the vrc mirror script overrides the shader setting
            // but it seems not consistent in the first frame, so i just do it multiple times
            SendCustomEventDelayedFrames(nameof(SetShader), 1);
            SendCustomEventDelayedFrames(nameof(SetShader), 2);
            SendCustomEventDelayedFrames(nameof(SetShader), 3);
            SendCustomEventDelayedFrames(nameof(SetShader), 4);
            SendCustomEventDelayedFrames(nameof(SetShader), 5);
            // SendCustomEventDelayedFrames: I think what happens is:
            // When the mirror is enabled the OnEnable method of MirrorReflection overwrites the material
            // And causes the shader to be reset to the default shader
            // This only happens of a closed new mirror is opend with transparent / cutout settings
            // So I delay the shader reset by one frame
            // I default to mirror to transparent so there is no hickup when the mirror is opened

            MirrorAnimator.SetBool("transparentClearFlags", doCutout);
        }

        public void SetShader()
        {
            foreach (Renderer renderer in _renderers)
                renderer.sharedMaterial.shader = _useTransparencyShader ? _shaderCutout : _shaderNormal;
        }

        [PublicAPI]
        /// <summary> Toggles the mirror of the given transform </summary>
        /// <returns> Returns true of all mirrors are turned off</returns>
        public bool Toggle(Transform mirrorTransform)
        {
            bool allOff = true;
            for (int i = 0; i < MirrorReflections.Length; i++)
            {
                if (MirrorReflections[i].transform == mirrorTransform)
                {
                    bool enalbe = !MirrorReflections[i].enabled;
                    MirrorReflections[i].enabled = enalbe;
                    _renderers[i].enabled = enalbe;
                }
                if (MirrorReflections[i].enabled)
                    allOff = false;
            }
            for (int i = 0; i < Icons.Length; i++)
                Icons[i].enabled = !MirrorReflections[i].enabled;
            return allOff;
        }

        [PublicAPI]
        public void Disable()
        {
            foreach (VRC_MirrorReflection mirror in MirrorReflections)
                mirror.enabled = false;
            foreach (Renderer renderer in _renderers)
                renderer.enabled = false;
            foreach (Renderer icon in Icons)
                icon.enabled = true;
        }

        protected void SetLayerMask(int mask)
        {
            foreach (VRC_MirrorReflection mirror in MirrorReflections)
                mirror.m_ReflectLayers = mask;
        }

        protected void ApplyMaterialSettings(float transparency, bool doCutout)
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.sharedMaterial.SetFloat("_Transparency", transparency);
                renderer.sharedMaterial.SetFloat("_BackgroundTransparency", doCutout ? 0 : 1);
            }
        }
    }
}
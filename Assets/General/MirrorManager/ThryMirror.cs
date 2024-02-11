
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ThryMirror : UdonSharpBehaviour
    {
        public VRC_MirrorReflection MirrorMain;
        public Renderer RendererMain;
        public Renderer Icon;
        public Animator MirrorAnimator;

        public Material MaterialNormal;
        public Material MaterialCutout;

        private Shader _shaderNormal;
        private Shader _shaderCutout;
        private bool _useSpecialShader;

        void Start()
        {
            _shaderNormal = MaterialNormal.shader;
            _shaderCutout = MaterialCutout.shader;
        }

        public void Set(int mask, bool doCutout, float transparency)
        {
            transparency = Mathf.Clamp01(transparency * 1.1f);
            _useSpecialShader = transparency < 1 || doCutout;

            MirrorMain.enabled = true;
            RendererMain.enabled = true;

            SetLayerMask(mask);
            ApplyTransparency(transparency);
            SendCustomEventDelayedFrames(nameof(SetShader), 1);
            // SendCustomEventDelayedFrames: I think what happens is:
            // When the mirror is enabled the OnEnable method of MirrorReflection overwrites the material
            // And causes the shader to be reset to the default shader
            // This only happens of a closed new mirror is opend with transparent / cutout settings
            // So I delay the shader reset by one frame
            // I default to mirror to transparent so there is no hickup when the mirror is opened

            MirrorAnimator.SetBool("isCutout", doCutout);

            if (Icon)
                Icon.enabled = false;
        }

        public void SetShader()
        {
            RendererMain.sharedMaterial.shader = _useSpecialShader ? _shaderCutout : _shaderNormal;
        }

        public void SetOff()
        {
            MirrorMain.enabled = false;
            RendererMain.enabled = false;
            if (Icon)
                Icon.enabled = true;
        }

        private void SetLayerMask(int mask)
        {
            MirrorMain.m_ReflectLayers = mask;
        }

        private void ApplyTransparency(float value)
        {
            RendererMain.sharedMaterial.SetFloat("_Transparency", value);
        }
    }
}
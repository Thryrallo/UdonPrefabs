
using UdonSharp;
using UnityEngine.Rendering.PostProcessing;
using Thry.Udon.UI;
using Thry.CustomAttributes;

namespace Thry.Udon
{
    [Singleton]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PostFX : SimpleUISetterExtension
    {
        public PostProcessVolume BloomVolume;
        public PostProcessVolume ColorGradingVolume;
        public PostProcessVolume AutoExpoureVolume;
        public PostProcessVolume DarkmodeVolume;

        public float BloomIntensity;
        public bool ColorGradingEnabled;
        public bool AutoExposureEnabled;
        public float DarkmodeIntensity;

        protected override string LogPrefix => "Thry.PostFX";

        public void Start()
        {
            PostFXSettingsChanged();
        }

        public void PostFXSettingsChanged()
        {
            Log(LogLevel.Log, "PostFXSettingsChanged");
            if (BloomVolume)
            {
                BloomVolume.weight = BloomIntensity;
                BloomVolume.enabled = BloomIntensity > 0;
            }
            if (DarkmodeVolume)
            {
                DarkmodeVolume.weight = DarkmodeIntensity;
                DarkmodeVolume.enabled = DarkmodeIntensity > 0;
            }
            if (ColorGradingVolume)
            {
                ColorGradingVolume.enabled = ColorGradingEnabled;
            }
            if (AutoExpoureVolume)
            {
                AutoExpoureVolume.enabled = AutoExposureEnabled;
            }
        }
    }
}

using System.Linq;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [RequireComponent(typeof(ToggleGroup))]
    [DefaultExecutionOrder(100)] // Ensure this runs after the ToggleGroup
    public class ToggleGroupTurnAllOffOnDisable : UdonSharpBehaviour
    {
        [SerializeField] private ToggleGroup _toggleGroup;

        void OnEnable()
        {
            SendCustomEventDelayedFrames(nameof(DisableAllToggles), 1);
        }

        public void DisableAllToggles()
        {
            if (_toggleGroup)
            {
                _toggleGroup.SetAllTogglesOff();
            }
        }
    }
}
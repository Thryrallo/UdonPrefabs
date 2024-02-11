
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SpinTheBottle
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class BottleSpinMenuButton : UdonSharpBehaviour
    {
        public Bottle MainScript;

        public override void Interact()
        {
            MainScript.ToggleMenu();
        }
    }
}
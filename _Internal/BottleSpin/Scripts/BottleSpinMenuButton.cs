using UdonSharp;

namespace Thry.Udon.SpinTheBottle
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
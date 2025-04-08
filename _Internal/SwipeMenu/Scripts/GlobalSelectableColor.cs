
using UdonSharp;
using Thry.Udon.AvatarTheme;

namespace Thry.Udon.SwipeMenu
{
    public enum GLobalColorIndex
    {
        Color1,
        Color2
    }

    public enum ColorBlockType
    {
        Normal,
        Highlighted,
        Pressed,
        Selected,
        Disabled
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class GlobalSelectableColor : UdonSharpBehaviour
    {
        public GLobalColorIndex Index = GLobalColorIndex.Color1;
        public ColorBlockType ColorState = ColorBlockType.Normal;
    }
}
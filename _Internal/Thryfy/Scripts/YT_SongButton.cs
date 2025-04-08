
using UdonSharp;

namespace Thry.Udon.YTDB
{
    public class YT_SongButton : UdonSharpBehaviour
    {
        int _songIndex;
        Thryfy _tablet;

        public UnityEngine.UI.Text SongNameText;
        public UnityEngine.UI.Text ArtistText;
        public UnityEngine.UI.Text LengthText;
        public UnityEngine.UI.Text IndexText;
        public UnityEngine.UI.Text RequestedByText;

        public void Setup(Thryfy tablet, int songIndex, string songName, string artist, string requestedBy, int length, int listIndex)
        {
            _tablet = tablet;
            _songIndex = songIndex;
            SongNameText.text = songName;
            ArtistText.text = artist;
            LengthText.text = length / 60 + ":" + (length % 60).ToString("00");
            IndexText.text = (listIndex + 1).ToString();
            RequestedByText.text = requestedBy;
        }

        public void Play()
        {
            _tablet.JumpTo(_songIndex);
        }
    }

}

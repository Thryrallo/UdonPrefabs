
using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace Thry.Udon.YTDB
{
    public class YT_ListItem : UdonSharpBehaviour
    {
        public ThumbnailLoader ThumbnailLoaderManager;
        public UnityEngine.UI.Text TextIndex;
        public UnityEngine.UI.Text TextName;
        public UnityEngine.UI.Text TextArtist;
        public UnityEngine.UI.Text TextDuration;
        public UnityEngine.UI.RawImage Thumbnail;

        public UdonBehaviour CallbackBehaviour;
        public string CallbackOnClick;
        public string CallbackOnFirstButton;
        public string CallbackOnSecondButton;
        
        int _songIndex;

        public void Setup(Transform parent, int listingIndex, int songIndex, string name, string artist, string duration)
        {
            GameObject instance = Instantiate(gameObject, parent);
            YT_ListItem item = instance.GetComponent<YT_ListItem>();
            item.TextIndex.text = (listingIndex + 1).ToString();
            item.TextName.text = name;
            item.TextArtist.text = artist;
            item.TextDuration.text = duration;
            item._songIndex = songIndex;
            instance.SetActive(true);
        }

        public void OnClick()
        {
            if(CallbackBehaviour != null && string.IsNullOrWhiteSpace(CallbackOnClick) == false)
            {
                CallbackBehaviour.SetProgramVariable("param_" + CallbackOnClick, _songIndex);
                CallbackBehaviour.SendCustomEvent(CallbackOnClick);
            }
        }

        public void OnFirst()
        {
            if(CallbackBehaviour != null && string.IsNullOrWhiteSpace(CallbackOnFirstButton) == false)
            {
                CallbackBehaviour.SetProgramVariable("param_" + CallbackOnFirstButton, _songIndex);
                CallbackBehaviour.SendCustomEvent(CallbackOnFirstButton);
            }
        }

        public void OnSecond()
        {
            if(CallbackBehaviour != null && string.IsNullOrWhiteSpace(CallbackOnSecondButton) == false)
            {
                CallbackBehaviour.SetProgramVariable("param_" + CallbackOnSecondButton, _songIndex);
                CallbackBehaviour.SendCustomEvent(CallbackOnSecondButton);
            }
        }

        public void SetThumbnailTexture(RenderTexture rt)
        {
            Thumbnail.texture = rt;
        }
    }

}
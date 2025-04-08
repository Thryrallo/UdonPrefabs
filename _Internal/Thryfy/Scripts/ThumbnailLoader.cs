
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Image;

namespace Thry.Udon.YTDB
{
    public class ThumbnailLoader : UdonSharpBehaviour
    {
        private VRCImageDownloader _imageDownloader;
        private YT_DB _database;

        // private bool _isInit = false;

        public void SetDatabase(YT_DB db)
        {
            _database = db;
            _imageDownloader = new VRCImageDownloader();
            // _isInit = true;
        }

        public void RequestTexture(int artistIndex, UdonBehaviour callback)
        {
            VRCUrl url = _database.GetArtistURL(artistIndex);
            IVRCImageDownload dl = _imageDownloader.DownloadImage(url, null, callback);
        }
    }

}
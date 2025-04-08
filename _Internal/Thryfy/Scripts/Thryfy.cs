
using JetBrains.Annotations;
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.Udon.YTDB
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Thryfy : ThryBehaviour
    {
        protected override string LogPrefix => "Thry.Thryfy";

        const int PLAYLIST_AUTO_GEN_LENGTH = 10;
        const int MAX_PLAYLIST_LENGTH = 100;
        const int SONGS_PER_LOAD_STEP = 20;
        const int ARTISTS_PER_LOAD_STEP = 5;

        [Header("On Setup")]
        [SerializeField, HideInInspector] YT_DB _database;
        public string InitialSong = "\"Slut!";

        [Space(50)]
        public Animator LoadBarAnimator;
        public Animator PlaylistAnimator;
        public Animator AddedToQueuePopup;

        [Space(5)]
        public Image LogoIcon;
        public Color InControlColor;

        [Space(5)]
        public VRC.SDK3.Components.VRCUrlInputField SearchField;
        public Text SearchFieldOverwritePlaceholder;
        public Text SearchFieldOverwriteText;

        [Space(5)]
        public RawImage CurrentlyPlayingTexture;
        public TextMeshProUGUI CurrentlyPlayingName;
        public TextMeshProUGUI CurrentlyPlayingArtist;

        [Space(5)]
        public ScrollRect ScrollbarSearch;
        public ScrollRect ScrollbarPlaylist;

        [Space(5)]
        public Transform PlaylistContainer;
        public YT_ListItem ListItemPrefab;
        [Space(5)]

        public Transform ArtistsContainer;
        public YT_Card CardPrefab;
        public Transform SongsContainer;
        public YT_SongButton SongButtonPrefab;
        public GameObject ButtonSongsShowMore;
        public GameObject ButtonArtistsShowMore;

        [Space(5)]
        public Image PlayImage;
        public Image PauseImage;
        public UnityEngine.UI.Slider VolumeSlider;
        public Image VolumeIcon;
        public Sprite[] VolumeSprites;
        
        private ThumbnailLoader _thumbnailLoader;

        [HideInInspector]
        [UdonSynced] public VRCUrl VideoUrl;

        [UdonSynced] string _searchTerm = "";
        [UdonSynced] int[] _playlist = new int[MAX_PLAYLIST_LENGTH];
        [UdonSynced] bool[] _playlistIsUserRequest = new bool[MAX_PLAYLIST_LENGTH];
        [UdonSynced] string[] _playlistRequestedBy = new string[MAX_PLAYLIST_LENGTH];
        [UdonSynced] VRCUrl[] _playlistCustomURL = new VRCUrl[MAX_PLAYLIST_LENGTH];
        [UdonSynced] int _playlistLength = 0;
        [UdonSynced] int[] _previousSongs = new int[MAX_PLAYLIST_LENGTH];
        [UdonSynced] VRCUrl[] _previousUrls = new VRCUrl[MAX_PLAYLIST_LENGTH];
        [UdonSynced] int _previousSongsHead = 0;
        [UdonSynced] int _previousSongsTail = 0;
        int[] _localPlaylistIndex = new int[MAX_PLAYLIST_LENGTH];
        int _localPlaylistLength = 0;

        bool _isPlaylistOpen = false;

        float _scrollPlaylistAbsolute = 0.0f;
        float _scrollSearchAbsolute = 0.0f;

        float _lastPlaylistScrollHeight = 0;
        float _lastSearchScrollHeight = 0;

        //bool _supressPlaylistScrollCallback = false;
        //bool _supressPlaylistSearchCallback = false;

        int _search_songs_loadSteps = 0;
        int _local_search_song_loadSteps = 0;
        int _search_artists_loadSteps = 0;
        int _local_search_artists_loadSteps = 0;

        bool _isSongByArtistSearch = false;

        int[] _resultsSongs = new int[]{ 0, 0, 0};
        int[] _resultsArtists = new int[]{ 0, 0, 0};
        int _songsOffset = 0;
        int _artistsOffset = 0;
        bool _isPlaying = false;
        UdonBehaviour _self;
        YT_SongButton[] _playlistButtons = new YT_SongButton[MAX_PLAYLIST_LENGTH];
        int _skipCallCount = 0;

        float _volume;
        float _preMuteVolume;
        
        private VideoPlayerAdapter _adapter;

        private void Start()
        {
            SendCustomEventDelayedFrames(nameof(Init), 1); // Delayed to allow for the UdonBehaviours to be initialized
        }

        public void SetAdapter(VideoPlayerAdapter adapter, string videoPlayerName)
        {
            Log(LogLevel.Log, $"Connected to {videoPlayerName} via {adapter.name}");
            _adapter = adapter;
        }

        public void Init() 
        {
            if(_adapter == null)
            {
                Log(LogLevel.Error, "Adapter is missing!");
                return;
            }
            
            Log(LogLevel.Log, $"Initializing with {_adapter.name}");
            
            _self = this.GetComponent<UdonBehaviour>();

            bool isOwner = Networking.IsOwner(gameObject);
            LogoIcon.color = isOwner ? InControlColor : Color.white;
            
            for(int i = 0; i < MAX_PLAYLIST_LENGTH; i++)
            {
                _playlistRequestedBy[i] = ""; // Initialize to empty string
                _playlistButtons[i] = Instantiate(SongButtonPrefab.gameObject, PlaylistContainer).GetComponent<YT_SongButton>();
                _playlistButtons[i].gameObject.SetActive(false);
                _localPlaylistIndex[i] = -1;
                _playlistCustomURL[i] = VRCUrl.Empty;

                _previousUrls[i] = VRCUrl.Empty;
            }
            
            ArtistsContainer.parent.gameObject.SetActive(false);
            SongsContainer.parent.gameObject.SetActive(false);

            ClearSearchObjects();
            _isPlaylistOpen = true;
            UpdatePlaylistAnimator();
            UpdateCurrentlyPlaying();

            _volume = VolumeSlider.value;
            UpdateVolumeIcon();

            _thumbnailLoader = GetComponentInChildren<ThumbnailLoader>(true);
            _thumbnailLoader.SetDatabase(_database);

            if (isOwner && !string.IsNullOrWhiteSpace(InitialSong))
            {
                // Wait for video players to def be ready (needed for e.g. ProTV)
                SendCustomEventDelayedSeconds(nameof(CreateInitialQueue), 3);
            }
        }

        public void CreateInitialQueue()
        {
            // Search for the initial song
            int[] ids = _database.SearchByName(InitialSong);
            if (ids[0] > 0)
            {
                PlayAndStartNewPlaylist(ids[1], false);
            }
        }


        [PublicAPI]
        public void TogglePlaylist()
        {
            _isPlaylistOpen = !_isPlaylistOpen;
            UpdatePlaylistAnimator();
        }

        [PublicAPI]
        public void ForceVideoSync()
        {
            _adapter.ForceVideoSync();
        }

        // ===================== UI Callbacks =====================

        public void OnSearchChanged()
        {
            SetSearchField(SearchField.GetUrl().ToString());
        }

        public void OnSearchSubmit()
        {
            if (_searchTerm == SearchField.GetUrl().ToString())
            {
                return;
            }

            if(SearchField.GetUrl().ToString().StartsWith("http"))
            {
                // Add video to queue
                Log(LogLevel.Log, "OnCustomUrlAdded");
                VRCUrl newUrl = SearchField.GetUrl();
                if (newUrl != null && newUrl != VideoUrl)
                {
                    EnqueueCustom(newUrl);
                    AddedToQueuePopup.SetTrigger("show");
                }
            }
            else
            {
                // Search
                _searchTerm = SearchField.GetUrl().ToString();
                _isSongByArtistSearch = false;

                SetSearchField();

                _isPlaylistOpen = false;
                UpdatePlaylistAnimator();
                
                ExecuteSearch();
            }
        }

        private void SetSearchField()
        {
            SetSearchField(_searchTerm);
        }

        private void SetSearchField(string s)
        {
            SearchFieldOverwritePlaceholder.enabled = s == "";
            SearchFieldOverwriteText.enabled = s != "";
            SearchFieldOverwriteText.text = s;
        }
        

        float ScrollbarNormalizedToAbsolute(ScrollRect bar)
        {
            return (1 - bar.verticalNormalizedPosition) * (bar.content.rect.height - bar.viewport.rect.height);
        }

        float ScrollbarAbsoluteToNormalized(float absolute, ScrollRect bar)
        {
            return 1 - absolute / (bar.content.rect.height - bar.viewport.rect.height);
        }

        public void OnSearchScrollbarChanged()
        {
            float height = ScrollbarSearch.content.rect.height;
            bool heightChanged = _lastSearchScrollHeight != height;
            if (heightChanged)
            {
                _lastSearchScrollHeight = height;
                ScrollbarSearch.verticalNormalizedPosition = ScrollbarAbsoluteToNormalized(_scrollSearchAbsolute, ScrollbarSearch);
                return;
            }
            _scrollSearchAbsolute = ScrollbarNormalizedToAbsolute(ScrollbarSearch);
        }

        public void OnPlaylistScrollbarChanged()
        {
            float height = ScrollbarPlaylist.content.rect.height;
            bool heightChanged = _lastPlaylistScrollHeight != height;
            if (heightChanged)
            {
                _lastPlaylistScrollHeight = height;
                ScrollbarPlaylist.verticalNormalizedPosition = ScrollbarAbsoluteToNormalized(_scrollPlaylistAbsolute, ScrollbarPlaylist);
                return;
            }
            _scrollPlaylistAbsolute = ScrollbarNormalizedToAbsolute(ScrollbarPlaylist);
        }

        [PublicAPI]
        public void SetVolume(float newVolume, bool updateVideoplayer)
        {
            if (_volume == newVolume) return;
            _volume = newVolume;
            
            VolumeSlider.SetValueWithoutNotify(_volume);
            UpdateVolumeIcon();

            if (updateVideoplayer)
                _adapter.UpdateVolumeFromTablet();
        }

        [PublicAPI]
        public float GetVolume()
        {
            return _volume;
        }

        public void OnVolumeChange()
        {
            float newVolume = VolumeSlider.value;
            if (_volume > 0 && newVolume == 0)
            {
                _preMuteVolume = 0.5f;
            }
            SetVolume(newVolume, true);
        }

        public void ToggleMute()
        {
            bool isMuted = _volume == 0f;
            isMuted = !isMuted;

            if (isMuted)
            {
                _preMuteVolume = _volume;
                SetVolume(0, true);
            }
            else
            {
                SetVolume(_preMuteVolume, true);
            }
        }

        public void LoadMoreSongs()
        {
            _search_songs_loadSteps++;
            ShowMoreSongs();
        }

        public void LoadMoreArtists()
        {
            _search_artists_loadSteps++;
            ShowMoreArtists();
        }

        // ===================== Networking =====================

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            Log(LogLevel.Log, "New Owner: " + player.displayName);
            LogoIcon.color = player.isLocal ? InControlColor : Color.white;
        }

        public override void OnDeserialization()
        {
            Log(LogLevel.Vervose, "OnDeserialization");
            UpdatePlaylist();
        }


        // ===================== Internals =====================

        void UpdateVolumeIcon()
        {
            if (_volume <= 0)
                VolumeIcon.sprite = VolumeSprites[0];
            else if (_volume < 0.1f)
                VolumeIcon.sprite = VolumeSprites[1];
            else if (_volume < 0.5f)
                VolumeIcon.sprite = VolumeSprites[2];
            else
                VolumeIcon.sprite = VolumeSprites[3];
        }

        void UpdatePlaylistAnimator()
        {
            PlaylistAnimator.SetBool("IsOpen", _isPlaylistOpen);
        }

        void UpdatePlaylist()
        {
            Log(LogLevel.Vervose, "UpdatePlaylist");
            UpdatePlaylistAnimator();
            // enable / disable buttons
            if (_localPlaylistLength != _playlistLength)
            {
                for (int i = _playlistLength; i < _localPlaylistLength; i++)
                {
                    _playlistButtons[i].gameObject.SetActive(false);
                }
                for (int i = _localPlaylistLength; i < _playlistLength; i++)
                {
                    _playlistButtons[i].gameObject.SetActive(true);
                }
                _localPlaylistLength = _playlistLength;
                _scrollPlaylistAbsolute = ScrollbarNormalizedToAbsolute(ScrollbarPlaylist);
            }
            // update values
            for (int i = 0; i < _playlistLength; i++)
            {
                if (_localPlaylistIndex[i] != _playlist[i] || (_playlist[i] == -1))
                {
                    _localPlaylistIndex[i] = _playlist[i];
                    if (_playlist[i] == -1)
                    {
                        string name = "";
                        string artist = "Unknown";
                        NameAndArtistFromUrl(_playlistCustomURL[i].Get(), ref name, ref artist);
                        _playlistButtons[i].Setup(this, -1, name, artist, _playlistRequestedBy[i], 0, i);
                    }
                    else
                    {
                        int songIndex = _playlist[i];
                        _playlistButtons[i].Setup(this, songIndex, _database.GetSongName(songIndex), _database.GetSongArtist(songIndex), _playlistRequestedBy[i], _database.GetSongLength(songIndex), i);
                    }
                }
            }
            if (Networking.IsOwner(gameObject))
                RequestSerialization();
        }

        void NameAndArtistFromUrl(string url, ref string name, ref string artist)
        {
            int index = url.IndexOf("youtube.com/watch?v=");
            if (index != -1)
            {
                name = url.Substring(index + 20).Split('&', '?')[0];
                artist = "YouTube";
                index = url.IndexOf("ab_channel=");
                if(index != -1)
                {
                    artist = url.Substring(index + 11).Split('&', '?')[0];
                }
                return;
            }
            index = url.IndexOf("youtu.be/");
            if (index != -1)
            {
                name = url.Substring(index + 9).Split('&', '?')[0];
                return;
            }

            index = url.IndexOf("www.");
            if(index != -1)
            {
                name = url.Substring(index + 4);
                return;
            }
            name = url;
            name = name.Replace("http://", "");
            name = name.Replace("https://", "");
        }

        void UpdateCurrentlyPlaying()
        {
            Log(LogLevel.Vervose, "UpdateCurrentlyPlaying");
            VRCUrl videoPlayerUrl = _adapter.GetVideoUrl();

            if(videoPlayerUrl == null || videoPlayerUrl == VRCUrl.Empty || string.IsNullOrWhiteSpace(videoPlayerUrl.Get()))
            {
                CurrentlyPlayingTexture.enabled = false;
                CurrentlyPlayingName.enabled = false;
                CurrentlyPlayingArtist.enabled = false;
                return;
            }else
            {
                CurrentlyPlayingTexture.enabled = true;
                CurrentlyPlayingName.enabled = true;
                CurrentlyPlayingArtist.enabled = true;
            }

            string name = "";
            string artist = "Unknown";
            bool getFromUrl = true;
            if (_playlistLength > 0)
            {
                if (_playlist[0] > -1 && GetPlaylistUrl(0).Equals(videoPlayerUrl))
                {
                    name = _database.GetSongName(_playlist[0]);
                    artist = _database.GetSongArtist(_playlist[0]);
                    getFromUrl = false;
                }
            }
            if(getFromUrl)
            {
                NameAndArtistFromUrl(videoPlayerUrl.Get(), ref name, ref artist);
            }
            CurrentlyPlayingName.text = name;
            CurrentlyPlayingArtist.text = artist;
            UpdateVideoTexture();
            SendCustomEventDelayedSeconds(nameof(UpdateVideoTexture), 2); // sometimes the texture is not ready yet
        }

        public void UpdateVideoTexture()
        {
            CurrentlyPlayingTexture.texture = _adapter.GetVideoTexture();

            if (_adapter.IsVideoTextureFlippedVertically)
            {
                CurrentlyPlayingTexture.transform.localScale = new Vector3(1, -1, 1);
            }
        }


        // ===================== Search =====================

        void ExecuteSearch()
        {
            Log(LogLevel.Log, $"ExecuteSearch {_searchTerm} " + (_isSongByArtistSearch ? " (Songs by Artist)" : ""));
            if (_isSongByArtistSearch)
            {
                _resultsSongs = _database.SearchByArtist(_searchTerm);
                _resultsArtists = new int[]{0,0,0};
            }
            else
            {
                _resultsSongs = _database.SearchByName(_searchTerm);
                _resultsArtists = _database.SearchArtist(_searchTerm);
            }

            ClearSearchObjects();

            _songsOffset = 0;
            _artistsOffset = 0;

            _local_search_artists_loadSteps = 0;
            _local_search_song_loadSteps = 0;
            
            _search_songs_loadSteps = 1;
            _search_artists_loadSteps = 1;
            
            _scrollSearchAbsolute = 0;
            
            if (_isSongByArtistSearch)
            {
                _search_songs_loadSteps = 3; // 60 Songs for artists per step
            }

            ArtistsContainer.parent.gameObject.SetActive(_resultsArtists[0] > 0);
            SongsContainer.parent.gameObject.SetActive(_resultsSongs[0] > 0);

            ShowMoreArtists();
            ShowMoreSongs();
        }

        void ClearSearchObjects()
        {
            // clear old songs
            foreach (Transform child in SongsContainer)
            {
                Destroy(child.gameObject);
            }
            // clear artists
            foreach (Transform child in ArtistsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public void ShowMoreSongs()
        {
            if(_local_search_song_loadSteps >= _search_songs_loadSteps) return; // Search result length syncing
            
            int listIndex = _songsOffset;
            for(int i = _resultsSongs[1] + _songsOffset; i < _resultsSongs[2] && i < _resultsSongs[1] + _songsOffset + SONGS_PER_LOAD_STEP; i++)
            {
                int index = i;
                if(_isSongByArtistSearch) index = _database.GetSongIdFromAristIndices(index);
                ListItemPrefab.Setup(SongsContainer, listIndex, index, _database.GetSongName(index), _database.GetSongArtist(index), _database.GetSongLengthString(index));
                listIndex++;
            }
            _songsOffset += SONGS_PER_LOAD_STEP;
            AdjustContainerHeight(SongsContainer, Mathf.Min(_resultsSongs[0], _songsOffset), 1, 80, 5);
            ButtonSongsShowMore.SetActive(_songsOffset < _resultsSongs[0]);

            // Search result length syncing. Check done after call to handle value changing between frames
            _local_search_song_loadSteps = _songsOffset / SONGS_PER_LOAD_STEP;
            SendCustomEventDelayedFrames(nameof(ShowMoreSongs), 1);
        }

        public void ShowMoreArtists()
        {
            if(_local_search_artists_loadSteps >= _search_artists_loadSteps) return; // Search result length syncing

            for (int i = _resultsArtists[1] + _artistsOffset; i < _resultsArtists[2] && i < _resultsArtists[1] + _artistsOffset + ARTISTS_PER_LOAD_STEP; i++)
            {
                string artist = _database.GetArtistName(i);
                CardPrefab.Setup(ArtistsContainer, artist, "Artist", false, i, 
                    _self, nameof(OnArtistSelected), nameof(param_OnArtistSelected), i,
                    null, null, null, null);
            }
            _artistsOffset += ARTISTS_PER_LOAD_STEP;
            AdjustContainerHeight(ArtistsContainer, Mathf.Min(_resultsArtists[0], _artistsOffset), 5, 300, 30);
            ButtonArtistsShowMore.SetActive(_artistsOffset < _resultsArtists[0]);

            // Search result length syncing. Check done after call to handle value changing between frames
            _local_search_artists_loadSteps = _artistsOffset / ARTISTS_PER_LOAD_STEP;
            SendCustomEventDelayedFrames(nameof(ShowMoreArtists), 1);
        }

        void AdjustContainerHeight(Transform contrainer, int elmCount, int countPerRow, int heightPerElm, int spacing)
        {
            int rowCount = (elmCount + countPerRow - 1) / countPerRow;
            RectTransform rect = contrainer.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rowCount * heightPerElm + Mathf.Max(0, rowCount - 1) * spacing);

            int height = 0;
            foreach(Transform child in contrainer.parent)
            {
                height += (int)child.GetComponent<RectTransform>().sizeDelta.y;
            }
            rect = contrainer.parent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
        }

        // ================== Callbacks ==================
        [HideInInspector] public int param_OnArtistSelected;
        public void OnArtistSelected()
        {
            _searchTerm = _database.GetArtistName(param_OnArtistSelected);
            _isSongByArtistSearch = true;
            SetSearchField();
            ExecuteSearch();
        }

        [HideInInspector] public int param_OnSongShuffle;
        public void OnSongShuffle()
        {
            _isPlaylistOpen = true;
            PlayAndStartNewPlaylist(param_OnSongShuffle, true);
        }

        [HideInInspector] public int param_OnSongEqueue;
        public void OnSongEqueue()
        {
            if(_playlistLength == 0)
            {
                PlayAndStartNewPlaylist(param_OnSongEqueue, true);
                return;
            }
            AddedToQueuePopup.SetTrigger("show");
            Enqueue(param_OnSongEqueue);
        }

        [HideInInspector] public int param_OnReplaceFirst;
        public void OnReplaceFirst()
        {
            if(_playlistLength == 0)
            {
                PlayAndStartNewPlaylist(param_OnReplaceFirst, true);
                return;
            }
            ReplacePlaylistHead(param_OnReplaceFirst);
        }

        // ================== Play Functions ==================

        VRCUrl GetPlaylistUrl(int index)
        {
            if (_playlist[index] == -1) return _playlistCustomURL[index];
            return _database.GetSongURL(_playlist[index]);
        }

        void Play()
        {
            if (_playlist[0] == -1)
            {
                string name = "", artist = "";
                NameAndArtistFromUrl(_playlistCustomURL[0].Get(), ref name, ref artist);
                PlayUrl(_playlistCustomURL[0], name + " - " + artist);
            }
            else Play(_playlist[0]);
        }

        void Play(int index)
        {
            if (index < 0) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            PlayUrl(_database.GetSongURL(index), _database.GetSongName(index) + " - " + _database.GetSongArtist(index));
        }

        void PlayUrl(VRCUrl url, string title)
        {
            VideoUrl = url;
            RequestSerialization();
            _adapter.Play(url, title);
        }

        void TriggerLoadAnimation(float time)
        {
            LoadBarAnimator.SetFloat("speed", 1 / time);
            LoadBarAnimator.SetTrigger("load");
        }

        public void JumpTo(int index)
        {
            if(index < 0) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            int playlistIndex = Array.IndexOf(_playlist, index);
            if(playlistIndex >= 0)
            {
                Play(index);
                ShiftPlaylistBy(playlistIndex);
            }else
            {
                PlayAndStartNewPlaylist(index, true);
            }
        }

        void Enqueue(int index)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            int playlistIndex = Array.IndexOf(_playlist, index);
            int earlistFreeUserIndex = -1;
            for(int i=1;i<_playlist.Length;i++)
            {
                if(_playlistIsUserRequest[i] == false)
                {
                    earlistFreeUserIndex = i;
                    break;
                }
            }
            if (earlistFreeUserIndex >= _playlistLength) earlistFreeUserIndex = _playlistLength;
            if (earlistFreeUserIndex == -1 || earlistFreeUserIndex >= MAX_PLAYLIST_LENGTH) return;
            // if song is already in playlist, move it to earlist position
            if (playlistIndex >= 0)
            {
                if(earlistFreeUserIndex < playlistIndex)
                {
                    int temp = _playlist[earlistFreeUserIndex];
                    _playlist[earlistFreeUserIndex] = _playlist[playlistIndex];
                    _playlistIsUserRequest[earlistFreeUserIndex] = true;
                    _playlistRequestedBy[earlistFreeUserIndex] = Networking.LocalPlayer.displayName;
                    _playlist[playlistIndex] = temp;
                } 
            }else
            {
                _playlist[earlistFreeUserIndex] = index;
                _playlistIsUserRequest[earlistFreeUserIndex] = true;
                _playlistRequestedBy[earlistFreeUserIndex] = Networking.LocalPlayer.displayName;
                if(earlistFreeUserIndex == _playlistLength)
                {
                    _playlistLength++;
                }
            }
            // start playing if nothing is playing
            if(!_isPlaying)
            {
                Play();
            }
            UpdatePlaylist();
        }

        void EnqueueCustom(VRCUrl url)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            int earlistFreeUserIndex = -1;
            for (int i = 1; i < _playlist.Length; i++)
            {
                if (_playlistIsUserRequest[i] == false)
                {
                    earlistFreeUserIndex = i;
                    break;
                }
            }
            if (earlistFreeUserIndex >= _playlistLength) earlistFreeUserIndex = _playlistLength;
            if (earlistFreeUserIndex == -1 || earlistFreeUserIndex >= MAX_PLAYLIST_LENGTH) return;
            // if song is already in playlist, move it to earlist position
            _playlist[earlistFreeUserIndex] = -1;
            _playlistIsUserRequest[earlistFreeUserIndex] = true;
            _playlistRequestedBy[earlistFreeUserIndex] = Networking.LocalPlayer.displayName;
            _playlistCustomURL[earlistFreeUserIndex] = url;
            if (earlistFreeUserIndex == _playlistLength)
            {
                _playlistLength++;
            }
            // start playing if nothing is playing
            if (!_isPlaying)
            {
                Play();
            }
            UpdatePlaylist();
        }

        void ReplacePlaylistHead(int newHead)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _playlist[0] = param_OnReplaceFirst;
            _playlistIsUserRequest[0] = true;
            _playlistRequestedBy[0] = Networking.LocalPlayer.displayName;
            Play(param_OnReplaceFirst);
            UpdatePlaylist();
        }

        void PlayAndStartNewPlaylist(int index, bool isUserRequest)
        {
            Log(LogLevel.Log, $"Starting Playlit with {_database.GetSongName(index)}");
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            _playlist[0] = index;
            _playlistIsUserRequest[0] = isUserRequest;
            _playlistRequestedBy[0] = isUserRequest ? Networking.LocalPlayer.displayName : "";
            _playlistLength = 1;
            Play(index);
            ShiftPlaylistBy(0);
        }

        void PrevSongEnqueue(int song, VRCUrl url)
        {
            _previousSongs[_previousSongsHead] = song;
            _previousUrls[_previousSongsHead] = url;
            _previousSongsHead = (_previousSongsHead + 1) % _previousSongs.Length;
            if(_previousSongsHead == _previousSongsTail)
            {
                _previousSongsTail = (_previousSongsTail + 1) % _previousSongs.Length;
            }
        }

        int PrevSongLength()
        {
            if(_previousSongsHead == _previousSongsTail) return 0;
            if(_previousSongsHead > _previousSongsTail) return _previousSongsHead - _previousSongsTail;
            return _previousSongs.Length - _previousSongsTail + _previousSongsHead;
        }

        VRCUrl _previous_url_result;
        int PrevSongDequeue()
        {
            if(_previousSongsHead == _previousSongsTail) return -1;
            _previousSongsHead = (_previousSongsHead - 1 + _previousSongs.Length) % _previousSongs.Length;
            int song = _previousSongs[_previousSongsHead];
            _previous_url_result = _previousUrls[_previousSongsHead];
            return song;
        }

        int[] GetNextAutogenSong()
        {
            // prioritize songs requested by users
            for(int i=0;i<_playlistLength;i++)
            {
                if(_playlist[i] >= 0 && _playlistIsUserRequest[i])
                {
                    int songId = _database.GetRandomRealtedNotInList(_playlist[i], _playlist, _playlistLength);
                    if (songId >= 0)
                    {
                        return new int[] { songId, i };
                    }
                }
            }
            // else get get random related song from any song in playlist not in list
            int start = UnityEngine.Random.Range(0, _playlistLength);
            for(int i=0;i< _playlistLength; i++)
            {
                int j = (start + i) % _playlistLength;
                if(_playlist[j] < 0) continue;
                int songId = _database.GetRandomRealtedNotInList(_playlist[j], _playlist, _playlistLength);
                if (songId >= 0)
                {
                    return new int[] { songId, j };
                }
            }
            // else get first related song from any song in playlist
            for(int i=0;i< _playlistLength; i++)
            {
                if(_playlist[i] < 0) continue;
                int songId = _database.GetRandomRelated(i);
                if (songId >= 0)
                {
                    return new int[] { songId, i };
                }
            }
            return new int[]{ -1, -1 };
        }

        void ShiftPlaylistBy(int count)
        {
            for(int i = 0; i < count; i++)
            {
                PrevSongEnqueue(_playlist[i], _playlistCustomURL[i]);
            }
            _playlistLength = _playlistLength - count;
            for(int i = 0; i < _playlistLength; i++)
            {
                _playlist[i] = _playlist[i + count];
                _playlistIsUserRequest[i] = _playlistIsUserRequest[i + count];
                _playlistRequestedBy[i] = _playlistRequestedBy[i + count];
                _playlistCustomURL[i] = _playlistCustomURL[i + count];
            }
            for(int i = _playlistLength; i < PLAYLIST_AUTO_GEN_LENGTH; i++)
            {
                int[] autoGen = GetNextAutogenSong();
                _playlist[i] = autoGen[0];
                if(autoGen[0] < 0) break; // this should never happen
                _playlistIsUserRequest[i] = false;
                _playlistRequestedBy[i] = "";
                //_playlistRequestedBy[i] = "Auto: " + Database.GetSongArtist(_playlist[autoGen[1]]); // list artist as requester
                _playlistLength = i + 1;
            }
            UpdatePlaylist();
        }

        [PublicAPI]
        public void VideoPlayerHasBeenPaused()
        {
            _isPlaying = false;
            PauseImage.gameObject.SetActive(_isPlaying);
            PlayImage.gameObject.SetActive(!_isPlaying);
        }

        [PublicAPI]
        public void VideoPlayerHasBeenResumed()
        {
            _isPlaying = true;
            PauseImage.gameObject.SetActive(_isPlaying);
            PlayImage.gameObject.SetActive(!_isPlaying);
            UpdateCurrentlyPlaying();
        }

        [PublicAPI]
        public void OnPauseReumeButton()
        {
            if (_isPlaying) _adapter.Pause();
            else _adapter.Resume();
        }

        [PublicAPI]
        public void Next()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            if(_playlistLength == 0) return;
            ShiftPlaylistBy(1);
            SendPlayRequest();
        }

        public void Previous()
        {
            if(PrevSongLength() > 0)
            {
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                int index = PrevSongDequeue();
                _playlistLength = Mathf.Min(_playlistLength + 1, _playlist.Length);
                for(int i = _playlistLength - 1; i > 0; i--)
                {
                    _playlist[i] = _playlist[i - 1];
                    _playlistIsUserRequest[i] = _playlistIsUserRequest[i - 1];
                    _playlistRequestedBy[i] = _playlistRequestedBy[i - 1];
                    _playlistCustomURL[i] = _playlistCustomURL[i - 1];
                }
                _playlist[0] = index;
                _playlistIsUserRequest[0] = true;
                _playlistRequestedBy[0] = Networking.LocalPlayer.displayName;
                _playlistCustomURL[0] = _previous_url_result;
                SendPlayRequest();
                UpdatePlaylist();
            }
        }

        // Rate Limiting Skips
        // Adding a delay to manual skip requests to prevent spamming of video player which results in the wrong video playing
        void SendPlayRequest()
        {
            _skipCallCount++;
            SendCustomEventDelayedSeconds(nameof(HandleSkipPlayRequest), 0.5f);
        }

        public void HandleSkipPlayRequest()
        {
            _skipCallCount--;
            if(_skipCallCount == 0)
            {
                Play();
            }
        }
    }
}

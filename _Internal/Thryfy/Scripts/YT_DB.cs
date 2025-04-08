
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using System;
using Thry.CustomAttributes;


#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UdonSharpEditor;
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace Thry.Udon.YTDB
{
    [Singleton(false)]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class YT_DB : ThryBehaviour
    {
        protected override string LogPrefix => "[YT_DB]";

        public bool LOAD_IN_PLAYMODE;
        // data is sorted by name in alphabetical order
        [SerializeField, HideInInspector] private string[] _names;
        [SerializeField, HideInInspector] private string[] _artists;

        [SerializeField, HideInInspector] private VRCUrl[] _artistUrls;
        [SerializeField, HideInInspector] private VRCUrl[] _urls;
        [SerializeField, HideInInspector] private int[] _artistIndices;
        [SerializeField, HideInInspector] private int[] _related; // 5 per song
        [SerializeField, HideInInspector] private ushort[] _length;

        [SerializeField, HideInInspector] private int[] _artistToSongIndices_artistIds;
        [SerializeField, HideInInspector] private int[] _artistToSongIndices_songIndices;


        public int[] SearchByName(string name)
        {
            name = name.ToLower();
            return BinarySearch(_names, name);
        }

        public int[] SearchByArtist(string artist)
        {
            artist = artist.ToLower();
            return BinarySearchByArtist(artist);
        }

        public int[] SearchArtist(string name)
        {
            name = name.ToLower();
            return BinarySearch(_artists, name);
        }

        int[] BinarySearch(string[] ar, string term)
        {
            // binary search, match beggining of string
            int min = 0;
            int max = ar.Length - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                string midVal = ar[mid].ToLower();
                //int cmp = midVal.CompareTo(term);
                int cmp = string.CompareOrdinal(midVal, term);
                bool match = midVal.StartsWith(term, StringComparison.Ordinal);
                if (cmp == 0 || match)
                {
                    int minIndex = BinarySearchMin(ar, term, min, max, mid);
                    int maxIndex = BinarySearchMax(ar, term, min, max, mid);
                    if(minIndex == -1 || maxIndex == -1)
                    {
                        return new int[]{0,0,0};
                    }
                    return new int[] { maxIndex - minIndex + 1, minIndex, maxIndex + 1 };
                }
                else if (cmp < 0)
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return new int[]{0,0,0};
        }

        int BinarySearchMin(string[] ar, string term, int min, int max, int mid)
        {
            max = mid;
            while(min <= max)
            {
                mid = (min + max) / 2;
                string midVal = ar[mid].ToLower();
                int cmp = string.CompareOrdinal(midVal, term);
                bool match = midVal.StartsWith(term, StringComparison.Ordinal);
                if (cmp == 0 || match)
                {
                    if(mid == 0 || !ar[mid - 1].ToLower().StartsWith(term, StringComparison.Ordinal))
                    {
                        return mid;
                    }
                    else
                    {
                        max = mid - 1;
                    }
                }
                else if (cmp < 0)
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return -1;
        }
        int BinarySearchMax(string[] ar, string term, int min, int max, int mid)
        {
            min = mid;
            while(min <= max)
            {
                mid = (min + max) / 2;
                string midVal = ar[mid].ToLower();
                int cmp = string.CompareOrdinal(midVal, term);
                bool match = midVal.StartsWith(term, StringComparison.Ordinal);
                if (cmp == 0 || match)
                {
                    if(mid == ar.Length - 1 || !ar[mid + 1].ToLower().StartsWith(term, StringComparison.Ordinal))
                    {
                        return mid;
                    }
                    else
                    {
                        min = mid + 1;
                    }
                }
                else if (cmp < 0)
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return -1;
        }

        int[] BinarySearchByArtist(string artist)
        {
            // binary search, match beggining of string
            int min = 0;
            int max = _artistToSongIndices_artistIds.Length - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                string midVal = _artists[_artistToSongIndices_artistIds[mid]].ToLower();
                int cmp = string.CompareOrdinal(midVal, artist);
                bool match = midVal.StartsWith(artist, StringComparison.Ordinal);
                if (cmp == 0 || match)
                {
                    int minIndex = BinarySearchByArtistMin(artist, min, max, mid);
                    int maxIndex = BinarySearchByArtistMax(artist, min, max, mid);
                    if(minIndex == -1 || maxIndex == -1)
                    {
                        return new int[]{0,0,0};
                    }
                    return new int[] { maxIndex - minIndex + 1, minIndex, maxIndex + 1 };
                }
                else if (cmp < 0)
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return new int[]{0,0,0};
        }

        int BinarySearchByArtistMin(string artist, int min, int max, int mid)
        {
            max = mid;
            while(min <= max)
            {
                mid = (min + max) / 2;
                string midVal = _artists[_artistToSongIndices_artistIds[mid]].ToLower();
                int cmp = string.CompareOrdinal(midVal, artist);
                bool match = midVal.StartsWith(artist, StringComparison.Ordinal);
                if (cmp == 0 || match)
                {
                    if(mid == 0 || !_artists[_artistToSongIndices_artistIds[mid - 1]].ToLower().StartsWith(artist, StringComparison.Ordinal))
                    {
                        return mid;
                    }
                    else
                    {
                        max = mid - 1;
                    }
                }
                else if (cmp < 0)
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return -1;
        }

        int BinarySearchByArtistMax(string artist, int min, int max, int mid)
        {
            min = mid;
            while(min <= max)
            {
                mid = (min + max) / 2;
                string midVal = _artists[_artistToSongIndices_artistIds[mid]].ToLower();
                int cmp = string.CompareOrdinal(midVal, artist);
                bool match = midVal.StartsWith(artist, StringComparison.Ordinal);
                if (cmp == 0 || match)
                {
                    if(mid == _artistToSongIndices_artistIds.Length - 1 || !_artists[_artistToSongIndices_artistIds[mid + 1]].ToLower().StartsWith(artist, StringComparison.Ordinal))
                    {
                        return mid;
                    }
                    else
                    {
                        min = mid + 1;
                    }
                }
                else if (cmp < 0)
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return -1;
        }

        public int GetSongIdFromAristIndices(int index)
        {
            return _artistToSongIndices_songIndices[index];
        }

        public string GetSongName(int index)
        {
            return _names[index];
        }

        public VRCUrl GetSongURL(int index)
        {
            return _urls[index];
        }

        public string GetSongArtist(int index)
        {
            return _artists[_artistIndices[index]];
        }

        public int GetSongLength(int index)
        {
            return _length[index];
        }

        public string GetSongLengthString(int index)
        {
            int length = _length[index];
            return length / 60 + ":" + (length % 60).ToString("00");
        }

        public int[] GetSongRelated(int index)
        {
            int count = 0;
            for(int i = index * 5; i < index * 5 + 5; i++)
            {
                if(_related[i] != -1)
                {
                    count++;
                }
            }
            int[] related = new int[count];
            Array.Copy(_related, index * 5, related, 0, count);
            return related;
        }

        public int GetRandomRelated(int index)
        {
            int count = 0;
            for(int i = index * 5; i < index * 5 + 5; i++)
            {
                if(_related[i] != -1)
                {
                    count++;
                }
            }
            if(count == 0)
            {
                return (index + 1) % _names.Length;
            }
            return _related[index * 5 + UnityEngine.Random.Range(0, count)];
        }

        public int GetRandomRealtedNotInList(int index, int[] list, int listLength)
        {
            int[] related = GetSongRelated(index);
            int start = UnityEngine.Random.Range(0, related.Length);
            for(int i = 0; i < related.Length; i++)
            {
                int j = (i + start) % related.Length;
                if(Array.IndexOf(list, related[j]) == -1)
                {
                    return related[j];
                }
            }
            return -1;
        }

        public string GetArtistName(int index)
        {
            return _artists[index];
        }

        public VRCUrl GetArtistURL(int index)
        {
            return _artistUrls[index];
        }

        public int GetSongCount()
        {
            return _names.Length;
        }

        public int GetArtistCount()
        {
            return _artists.Length;
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP

        class Song
        {
            public string sortName;
            public string sortArtist;
            public string name;
            public string artists;
            public string artist;
            public string id;
            public string[] related;
            public int index;
            public ushort length;
            public int views;
        }

        public class BuildHook
        {

            [InitializeOnLoadMethod]
            public static void PlayModeStateChangedHook()
            {
                ClearDB(); // Make sure the DB is cleared. Could happen in case of crash / bug.
            }
            
            [PostProcessSceneAttribute(-10)]
            public static void OnPostprocessScene()
            {
                PrepareDB();
            }

            private static void ClearDB()
            {
                YT_DB[] yt_dbs = GameObject.FindObjectsOfType<YT_DB>(true);
                foreach (YT_DB db in yt_dbs)
                {
                    db.Log(LogLevel.Vervose, "Clearing YT_DB");
                    db._names = new string[0];
                    db._artists = new string[0];
                    db._artistIndices = new int[0];
                    db._artistToSongIndices_artistIds = new int[0];
                    db._artistToSongIndices_songIndices = new int[0];
                    db._urls = new VRCUrl[0];
                    db._length = new ushort[0];
                    db._related = new int[0];
                    db._artistUrls = new VRCUrl[0];

                    UdonSharpEditorUtility.CopyProxyToUdon(db);
                }
            }

            private static bool PrepareDB()
            {
                // Find YT_DB in scene
                YT_DB[] yt_dbs = GameObject.FindObjectsOfType<YT_DB>(true);
                if (yt_dbs.Length == 0)
                {
                    return true;
                }
                if(yt_dbs.Length > 1)
                {
                    yt_dbs[0].Log(LogLevel.Error, "Multiple YT_DBs found in scene. Only one is allowed.");
                }
                YT_DB yt_db = yt_dbs[0];

                yt_db.Log(LogLevel.Vervose, "Building YT_DB");

                // find yt_songs.txt
                string[] guids = AssetDatabase.FindAssets("t:TextAsset yt_songs");
                if (guids.Length == 0)
                {
                    yt_db.Log(LogLevel.Error, "YT_DB: yt_songs.txt not found");
                    return false;
                }
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                TextAsset songsAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                guids = AssetDatabase.FindAssets("t:TextAsset yt_artists");
                if (guids.Length == 0)
                {
                    yt_db.Log(LogLevel.Error, "YT_DB: yt_artists.txt not found");
                    return false;
                }
                path = AssetDatabase.GUIDToAssetPath(guids[0]);
                TextAsset artistsAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                // Load Artists
                string[] artist_lines = artistsAsset.text.Split('\n');
                string[] song_lines = songsAsset.text.Split('\n');
                yt_db._artists = new string[artist_lines.Length];
                yt_db._artistUrls = new VRCUrl[artist_lines.Length];
                yt_db._artistToSongIndices_artistIds = new int[song_lines.Length];
                yt_db._artistToSongIndices_songIndices = new int[song_lines.Length];
                int artistToSongIndicesIndex = 0;
                for (int i = 0; i < artist_lines.Length; i++)
                {
                    string[] parts = artist_lines[i].Split(';');
                    yt_db._artists[i] = parts[0];
                    yt_db._artistUrls[i] = new VRCUrl($"https://img.youtube.com/vi/{parts[1]}/0.jpg");
                    int[] songIndices = Array.ConvertAll(parts[2].Split(','), int.Parse);
                    Array.Copy(songIndices, 0, yt_db._artistToSongIndices_songIndices, artistToSongIndicesIndex, songIndices.Length);
                    Array.Fill(yt_db._artistToSongIndices_artistIds, i, artistToSongIndicesIndex, songIndices.Length);
                    artistToSongIndicesIndex += songIndices.Length;
                }

                // Load Songs
                yt_db._names = new string[song_lines.Length];
                yt_db._urls = new VRCUrl[song_lines.Length];
                yt_db._artistIndices = new int[song_lines.Length];
                yt_db._related = new int[song_lines.Length * 5];
                yt_db._length = new ushort[song_lines.Length];

                for (int i = 0; i < song_lines.Length; i++)
                {
                    if (i % 100 == 0)
                        EditorUtility.DisplayProgressBar("YT_DB", "Loading Data", (float)i / song_lines.Length);
                    string[] parts = song_lines[i].Split(';');
                    yt_db._urls[i] = new VRCUrl("https://youtu.be/" + parts[0]);
                    yt_db._names[i] = parts[1];
                    yt_db._artistIndices[i] = int.Parse(parts[2]);
                    yt_db._length[i] = ushort.Parse(parts[3]);
                    yt_db._related[i * 5 + 0] = int.Parse(parts[4]);
                    yt_db._related[i * 5 + 1] = int.Parse(parts[5]);
                    yt_db._related[i * 5 + 2] = int.Parse(parts[6]);
                    yt_db._related[i * 5 + 3] = int.Parse(parts[7]);
                    yt_db._related[i * 5 + 4] = int.Parse(parts[8]);
                }

                EditorUtility.ClearProgressBar();
                UdonSharpEditorUtility.CopyProxyToUdon(yt_db);

                return true;
            }
        }
#endif
    }
}
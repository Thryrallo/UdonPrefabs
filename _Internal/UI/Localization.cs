
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Thry.CustomAttributes;
using System.Text;
using VRC.SDKBase;
using VRC.Udon;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using UdonSharpEditor;
using System.Collections;
using System.Reflection;
#endif

namespace Thry.Udon.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Localization : SimpleUISetterExtension
    {
        const string DEBUG_PREFIX = "[Thry][Locale]";
        // en,de,fr,it,ja,ko,es,pl,ru,pt-BR,zh-CN,zh-HK

        [SerializeField, HideInInspector] private Text[] _labels;
        [SerializeField, HideInInspector] private int[] _labelsIndices;
        [SerializeField, HideInInspector] private TextMeshProUGUI[] _tMProLabels;
        [SerializeField, HideInInspector] private int[] _tMProLabelsIndices;

        [SerializeField, HideInInspector] private UdonBehaviour[] _behavioursStrings;
        [SerializeField, HideInInspector] private string[] variableStrings;
        [SerializeField, HideInInspector] private int[] _indicesStrings;

        [SerializeField, HideInInspector] private UdonBehaviour[] _behavioursArrays;
        [SerializeField, HideInInspector] private string[] _variableArrays;
        [SerializeField, HideInInspector] private int[] _indicesArrays;

        [SerializeField] TextAsset[] _data;
        [SerializeField, HideInInspector] string[] _languages;
        [SerializeField, HideInInspector] string[] _values;
        [SerializeField, HideInInspector] int[] _arrayLength;

        [SerializeField, HideInInspector] bool _disabled;
        [NonSerialized, FieldChangeCallback(nameof(CurrentLaguage_Internal))] public int CurrentLaguage = 0;

        int CurrentLaguage_Internal
        {
            get => CurrentLaguage;
            set
            {
                CurrentLaguage = value;
                if (_disabled) return;
                Log(LogLevel.Log, $"Language changed: {_languages[value]}");
                UpdateLabels();
            }
        }

        protected override string LogPrefix => "Thry.Localization";

        private void Start()
        {
            if (_disabled) return;
            string currentLang = VRCPlayerApi.GetCurrentLanguage();
            int index = Array.IndexOf(_languages, currentLang);
            if (LogMinLevel == LogLevel.Log)
            {
                string availableLanguages = "";
                foreach (string l in _languages)
                    availableLanguages += l + ",";
                availableLanguages = availableLanguages.Trim(new char[] { ',' });
                Log(LogLevel.Log, $"Available languages: {availableLanguages}. vrc language: {currentLang} => {index}");
            }
            if (index != -1)
            {
                CurrentLaguage_Internal = index;
                VariableChangedFromBehaviour(nameof(CurrentLaguage));
            }
            else
            {
                UpdateLabels();
            }

            //string[] langs = VRCPlayerApi.GetAvailableLanguages();
            //foreach(string l in langs)
            //{
            //    Log(LogLevel.Vervose, $"Available language: {l}");
            //}
            //Log(LogLevel.Vervose, $"Current language: {VRCPlayerApi.GetCurrentLanguage()}");
        }

        void UpdateLabels()
        {
            for (int i = 0; i < _labels.Length; i++)
            {
                string val = _values[_labelsIndices[i] + CurrentLaguage_Internal];
                _labels[i].text = val;
            }
            for (int i = 0; i < _tMProLabels.Length; i++)
            {
                string val = _values[_tMProLabelsIndices[i] + CurrentLaguage_Internal];
                _tMProLabels[i].text = val;
            }

            for (int i = 0; i < _behavioursStrings.Length; i++)
            {
                string val = _values[_indicesStrings[i] + CurrentLaguage_Internal];
                _behavioursStrings[i].SetProgramVariable(variableStrings[i], val);
                _behavioursStrings[i].SendCustomEvent("OnLocalizationChanged");
            }
            for (int i = 0; i < _behavioursArrays.Length; i++)
            {
                string[] ar = new string[_arrayLength[_indicesArrays[i]]];
                Array.Copy(_values, _indicesArrays[i] + ar.Length * CurrentLaguage_Internal, ar, 0, ar.Length);
                _behavioursArrays[i].SetProgramVariable(_variableArrays[i], ar);
                _behavioursArrays[i].SendCustomEvent("OnLocalizationChanged");
            }
        }


#if UNITY_EDITOR && !COMPILER_UDONSHARP

        [PostProcessSceneAttribute(-10)]
        static void OnPostprocessScene()
        {
            Localization[] scripts = GameObject.FindObjectsOfType<Localization>(true);
            Text[] labels = GameObject.FindObjectsOfType<Text>(true);
            TextMeshProUGUI[] tmProLabels = GameObject.FindObjectsOfType<TextMeshProUGUI>(true);

            if (scripts.Length == 0) return;

            // order scripts by hirachy depth, so active script is highest (e.g. world locaization if used). Just cause it's nicer for debugging
            scripts = scripts.OrderBy(s =>
                {
                    int depth = 0;
                    Transform t = s.transform;
                    while (t != null)
                    {
                        depth++;
                        t = t.parent;
                    }
                    return depth;
                }).ToArray();


            UdonSharpBehaviour[] usbs = GameObject.FindObjectsOfType<UdonSharpBehaviour>(true);
            Type[] usbTypes = usbs.Select(u => u.GetType()).Distinct().ToArray();
            Dictionary<Type, List<FieldInfo>> stringFields = new Dictionary<Type, List<FieldInfo>>();
            Dictionary<Type, List<FieldInfo>> arrayFields = new Dictionary<Type, List<FieldInfo>>();
            foreach (Type t in usbTypes)
            {
                stringFields[t] = t.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => f.FieldType == typeof(string)).ToList();
                arrayFields[t] = t.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => f.FieldType.IsArray && f.FieldType.GetElementType() == typeof(string)).ToList();
            }

            // Parse all scripts, but only fill first to be active, so that data isnt double saved / doesnt get double applies when multiple of the same prefab are inte scene



            Dictionary<string, List<string>[]> entries = new Dictionary<string, List<string>[]>();
            Dictionary<string, int> keyIndices = new Dictionary<string, int>();

            HashSet<string> languages = new HashSet<string>();
            IEnumerable<TextAsset> data = scripts.SelectMany(l => l._data).Distinct();
            foreach (TextAsset a in data)
                ParseForLanguages(a, languages);

            string[] languagesArray = languages.ToArray();

            int index = 0;
            foreach (TextAsset a in data)
                ParseTextAsset(a, languagesArray, entries, keyIndices, ref index);

            Localization loc = scripts[0];
            loc._languages = languagesArray;

            loc._values = entries.Values.SelectMany(v => v.SelectMany(l => l)).ToArray();
            loc._arrayLength = entries.Values.SelectMany(kv => Enumerable.Repeat(kv[0].Count, kv.Length * kv[0].Count)).ToArray();

            loc._labels = labels.Where(l => entries.ContainsKey(l.text.Trim())).ToArray();
            loc._tMProLabels = tmProLabels.Where(l => entries.ContainsKey(l.text.Trim())).ToArray();
            loc._labelsIndices = loc._labels.Select(l => keyIndices[l.text.Trim()]).ToArray();
            loc._tMProLabelsIndices = loc._tMProLabels.Select(l => keyIndices[l.text.Trim()]).ToArray();

            List<UdonBehaviour> behaviours = new List<UdonBehaviour>();
            List<string> varNames = new List<string>();
            List<int> varIndices = new List<int>();

            foreach (UdonSharpBehaviour b in usbs)
            {
                Type t = b.GetType();
                if (stringFields.ContainsKey(t))
                {
                    foreach (FieldInfo f in stringFields[t])
                    {
                        if (entries.ContainsKey(f.GetValue(b) as string))
                        {
                            behaviours.Add(UdonSharpEditorUtility.GetBackingUdonBehaviour(b));
                            varNames.Add(f.Name);
                            varIndices.Add(keyIndices[f.GetValue(b) as string]);
                        }
                    }
                }
            }

            loc._behavioursStrings = behaviours.ToArray();
            loc.variableStrings = varNames.ToArray();
            loc._indicesStrings = varIndices.ToArray();

            behaviours.Clear();
            varNames.Clear();
            varIndices.Clear();

            foreach (UdonSharpBehaviour b in usbs)
            {
                Type t = b.GetType();
                if (arrayFields.ContainsKey(t))
                {
                    foreach (FieldInfo f in arrayFields[t])
                    {
                        string[] arr = f.GetValue(b) as string[];
                        if (arr != null && arr.Length > 0 && entries.ContainsKey(arr[0]))
                        {
                            behaviours.Add(UdonSharpEditorUtility.GetBackingUdonBehaviour(b));
                            varNames.Add(f.Name);
                            varIndices.Add(keyIndices[arr[0]]);
                        }
                    }
                }
            }

            loc._behavioursArrays = behaviours.ToArray();
            loc._variableArrays = varNames.ToArray();
            loc._indicesArrays = varIndices.ToArray();

            // clear all scripts but script[0]
            loc._disabled = false;
            loc.gameObject.SetActive(true);
            
            for (int i=1;i<scripts.Length;i++)
            {
                scripts[i].gameObject.SetActive(false);
                scripts[i]._disabled = true;
                scripts[i]._labels = new Text[0];
                scripts[i]._tMProLabels = new TextMeshProUGUI[0];
                scripts[i]._behavioursStrings = new UdonBehaviour[0];
                scripts[i]._behavioursArrays = new UdonBehaviour[0];
            }
        }

        static void ParseForLanguages(TextAsset asset, HashSet<string> languages)
        {
            if (asset == null) return;

            string[] lines = asset.text.Split('\n');
            string[] langs = lines[0].Split(",").Skip(1).Select(l => l.Trim()).ToArray();
            foreach (string l in langs)
            {
                if (languages.Contains(l) == false)
                    languages.Add(l);
            }

        }

        static void ParseTextAsset(TextAsset asset, string[] languages, Dictionary<string, List<string>[]> entries, Dictionary<string, int> keyIndices, ref int index)
        {
            if (asset == null) return;

            string[] lines = asset.text.Split('\n');
            string[] langs = lines[0].Split(",").Skip(1).Select(l => l.Trim()).ToArray();

            int[] langInidices = new int[langs.Length];
            for (int i = 0; i < langs.Length; i++)
                langInidices[i] = Array.IndexOf(languages, langs[i]);

            for (int i = 1; i < lines.Length; i++)
            {
                IEnumerable<string> parts = ParseLine(lines[i]);
                string key = parts.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(key)) continue;
                string[] fileValues = parts.Skip(1).ToArray();
                string[] values = new string[languages.Length];
                for (int j = 0; j < fileValues.Length; j++)
                {
                    if (langInidices[j] != -1)
                        values[langInidices[j]] = fileValues[j];
                }
                for (int j = 0; j < values.Length; j++)
                    if (string.IsNullOrWhiteSpace(values[j]))
                        values[j] = values[0];

                bool isDuplicate = entries.ContainsKey(key);
                bool isArray = key.EndsWith("[]", StringComparison.Ordinal);
                if(isArray)
                    key = key.Substring(0, key.Length - 2);

                if (entries.ContainsKey(key) == false)
                {
                    entries[key] = new List<string>[languages.Length];
                    for (int j = 0; j < languages.Length; j++)
                        entries[key][j] = new List<string>();
                    keyIndices[key] = index;
                }

                if(!isDuplicate || isArray)
                {
                    for (int j = 0; j < languages.Length; j++)
                        entries[key][j].Add(values[j]);
                    index += languages.Length;
                }
            }
        }

        static IEnumerable<string> ParseLine(string line)
        {
            StringBuilder current = new StringBuilder();
            List<string> result = new List<string>();
            bool inApostrophes = false;
            foreach (char c in line)
            {
                if (c == '"') inApostrophes = !inApostrophes;
                else if (!inApostrophes && c == ',')
                {
                    result.Add(current.ToString().Replace("\\n", "\n"));
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }
            result.Add(current.ToString().Replace("\\n", "\n"));
            return result;
        }
#endif
    }
}
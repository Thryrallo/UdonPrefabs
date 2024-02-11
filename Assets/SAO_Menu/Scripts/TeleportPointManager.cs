
using Thry.SAO;
using Thry.SAO.Button;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;



#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

namespace Thry
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TeleportPointManager : UdonSharpBehaviour
    {
        public Transform Container;
        public GameObject Prefab;
        public Thry.SAO.Menu SAOMenu;

        public Transform[] Targets;

        public void Start()
        {
            foreach (Transform target in Targets)
            {
                GameObject go = Instantiate(Prefab, Container);
                Teleport tp = go.GetComponent<Teleport>();
                tp.Target = target;
                tp.Menu = SAOMenu;
                tp.Header.text = target.name.Replace("[TP]", "");
                go.SetActive(true);
            }
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    // register scene saving callback on compile
    [InitializeOnLoadMethod]
    private static void RegisterSceneSavingCallback()
    {
        EditorSceneManager.sceneSaving += OnBeforeSave;
        // Add tag 'TeleportPoint' if it does not exist to unity using unity functions
        AddTag("TeleportPoint");
    }

    static void OnBeforeSave(Scene scene, string path)
    {
        // Debug.Log("OnBeforeSave");
        TeleportPointManager[] scipts = GameObject.FindObjectsByType<TeleportPointManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        GameObject[] gos = GameObject.FindGameObjectsWithTag("TeleportPoint");
        Transform[] targets = gos.Select(go => go.transform).OrderBy(t => t.name.Replace("[TP]", "")).ToArray();
        foreach (TeleportPointManager script in scipts)
        {
            script.Targets = targets;
        }
    }

    static void AddTag(string tagname)
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty tags = so.FindProperty("tags");

            for (int i = 0; i < tags.arraySize; ++i)
            {
                if (tags.GetArrayElementAtIndex(i).stringValue == tagname)
                {
                    return;     // Tag already present, nothing to do.
                }
            }
            
            tags.InsertArrayElementAtIndex(0);
            tags.GetArrayElementAtIndex(0).stringValue = tagname;
            so.ApplyModifiedProperties();
            so.Update();
        }
    }


#endif
    }
}
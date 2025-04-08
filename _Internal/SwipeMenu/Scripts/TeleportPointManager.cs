
using UdonSharp;
using UnityEngine;
using Thry.Udon.UI.Buttons;
using Thry.Udon.SwipeMenu;
using Thry.Udon.AvatarTheme;
using UnityEngine.UI;






#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

namespace Thry.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TeleportPointManager : UdonSharpBehaviour
    {
        public Transform Container;
        public GameObject Prefab;
        public SwipeMenuManager SAOMenu;

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
        EditorHelper.AddTag("TeleportPoint");
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
#endif
    }
}
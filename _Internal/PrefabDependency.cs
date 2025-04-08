using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDKBase;

namespace Thry
{
    [ExecuteInEditMode]
    public class PrefabDependency : MonoBehaviour, IEditorOnly
    {
        public GameObject[] Dependencies;

        void Awake()
        {
#if UNITY_EDITOR
            // Check if the dependencies are missing
            Scene myScene = gameObject.scene;
            if (Dependencies == null) return;
            foreach (GameObject dependency in Dependencies)
            {
                bool isPrefabInScene = PrefabUtility.FindAllInstancesOfPrefab(dependency).Any(x => x.scene == myScene);
                // Find instances of the prefab in the scene
                if(!isPrefabInScene)
                {
                    Debug.LogWarning("PrefabDependency: " + dependency.name + " is missing in scene " + myScene.name);
                    // Instantiate the prefab
                    GameObject newGO = PrefabUtility.InstantiatePrefab(dependency) as GameObject;
                    SceneManager.MoveGameObjectToScene(newGO, myScene);
                    newGO.transform.position = Vector3.zero;
                    newGO.transform.rotation = Quaternion.identity;
                }
            }
#endif
        }
    }
}

using UdonSharp;
using UnityEngine;
using Thry.General;
using VRC.Udon;

#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor;
using UdonSharpEditor;
#endif

namespace Thry
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Clapper : UdonSharpBehaviour
    {
        ThryAction[] _clapperActions;

        ClapperEffect _localEffect;
        Thry.ObjectPool.OneObjectPerPlayerPool _effectPool;
        bool hasNetworkedEffect = false;
        bool hasLocalEffect = false;
        HandCollider _left;
        HandCollider _right;
        int _claps = 0;
        float _clastClapTime = 0;

        const float MAX_TIME_BETWEEN_CLAPS = 1;

        const bool DEBUG = true;

        object[][] _otherActions = new object[0][];

        public static Clapper Get()
        {
            GameObject go = GameObject.Find("[Thry]Clapper");
            if (go != null)
                return go.GetComponent<Clapper>();
            return null;
        }

        private void Start()
        {
            //Find hand Colliders
            _left = HandCollider.GetLeft();
            _right = HandCollider.GetRight();

            //Find clapper actions in children
            int actionsCount = 0;
            for(int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.GetComponent<ClapperEffect>() != null)
                {
                    _localEffect = transform.GetChild(i).GetComponent<ClapperEffect>();
                    if (!hasNetworkedEffect) hasLocalEffect = true;
                }
                if (child.GetComponent<ObjectPool.OneObjectPerPlayerPool>() != null)
                {
                    _effectPool = transform.GetChild(i).GetComponent<ObjectPool.OneObjectPerPlayerPool>();
                    hasNetworkedEffect = true;
                    hasLocalEffect = false;
                }
                if (child.GetComponent<ThryAction>() != null && child.GetComponent<ThryAction>().isClapperAction)
                {
                    actionsCount++;
                }
            }
            _clapperActions = new ThryAction[actionsCount];
            actionsCount = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<ThryAction>() != null && transform.GetChild(i).GetComponent<ThryAction>().isClapperAction)
                {
                    _clapperActions[actionsCount++] = transform.GetChild(i).GetComponent<ThryAction>();
                }
            }

            //Register clap callback on hands
            _right.RegisterClapCallback(this.gameObject);
            _left.RegisterClapCallback(this.gameObject);
        }

        public void RegisterClapperAction(UdonSharpBehaviour udonBehaviour, string eventName, int claps, string desktopKey)
        {
            object[] data = new object[] { claps, desktopKey, udonBehaviour, eventName };
            object[][] temp = new object[_otherActions.Length + 1][];
            System.Array.Copy(_otherActions, temp, _otherActions.Length);
            temp[_otherActions.Length] = data;
            _otherActions = temp;
        }

        private void Update()
        {
            foreach(ThryAction action in _clapperActions)
            {
                if (action.desktopKey != "" && Input.GetKeyDown(action.desktopKey))
                {
                    action.OnInteraction();
                }
            }
            foreach (object[] action in _otherActions)
            {
                if (action[1] != "" && Input.GetKeyDown((string)action[1]))
                {
                    ((UdonBehaviour)action[2]).SendCustomEvent((string)action[3]);
                }
            }
            //Dekstop claps
            if (Input.GetKeyDown(KeyCode.F))
            {
                _Clap();
            }
        }

        private void _PlayClapEffect()
        {
            if (hasNetworkedEffect)
            {
                if(_effectPool.LocalBehaviour != null)
                {
                    _effectPool.LocalBehaviour.SendCustomEvent("NetworkPlay");
                }
                else
                {
                    Debug.Log("[Clapper] No Object Pool Object assigned.");
                    if(hasLocalEffect) _localEffect.Play();
                }
            }else if (hasLocalEffect)
            {
                _localEffect.Play();
            }
        }

        public void _Clap()
        {
            _CountClap();
            _PlayClapEffect();
        }

        private void _CountClap()
        {
            _claps++;
            _clastClapTime = Time.time;
            SendCustomEventDelayedSeconds(nameof(_CheckClap), MAX_TIME_BETWEEN_CLAPS + 0.1f);
        }

        public void _CheckClap()
        {
            if(Time.time - _clastClapTime > MAX_TIME_BETWEEN_CLAPS)
            {
                foreach (ThryAction action in _clapperActions)
                {
                    if(action.requiredClaps == _claps)
                    {
                        action.OnInteraction();
                    }
                }
                foreach (object[] action in _otherActions)
                {
                    if ((int)action[0] == _claps)
                    {
                        ((UdonBehaviour)action[2]).SendCustomEvent((string)action[3]);
                    }
                }
                _claps = 0;
            }
        }
    }

#if !COMPILER_UDONSHARP && UNITY_EDITOR
    [CustomEditor(typeof(Clapper))]
    public class ClapperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
            EditorGUILayout.HelpBox("Requires [Thry]HandColliders!!", MessageType.Warning);
            EditorGUILayout.HelpBox("Place any clapper behaviour as child of this gameobject.", MessageType.Info);
        }
    }
#endif
}
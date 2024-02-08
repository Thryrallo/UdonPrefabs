
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Thry.SAO
{
    public class Menu : UdonSharpBehaviour
    {
        const string DEBUG_PREFIX = "[Thry][SAO]";
        
        public Gestures GestureRecognizer;

        [Header("Animation")]
        public Vector3 startPosition = new Vector3(0, 450, 0);
        public float ANIMATION_DURATION = 0.5f;
        public float SUBMENU_ANIMATION_DURATION = 0.2f;
        public float ANIMATION_Y_MODIFIER = 0.001f;
        public float ANIMATION_Y_OFFSET = 0.15f;

        public float MENU_HEIGHT = 0.2f;

        public float CLOSE_VELOCITY = 0.1f;

        private float animation_start_time;
        private int animation_direction = 1;

        private bool finished = false;
        private bool init = false;
        private Vector3[] targetPositions;
        private Vector3[] targetScale;

        public Transform Canvas;

        [Header("Test")]
        public bool TEST_VR;
        public bool OUTPUT_EXTRA_DEBUG;
        
        public BoxCollider raycastCollider;
        public ToggleGroup MainToggleGroup;

        [Header("Optional References")]
        public Camera localPlayerFaceCamera;
        public float localPlayerCameraSize = 0.25f;
        [Tooltip("Enables these gameobjects while menu is open. Suggested use indicators.")]
        public GameObject[] inWorldIndicators;
        [Tooltip("These get disabled when the scene is started.")]
        public GameObject[] disableOnLoad;

        [Header("Local Player Profile")]
        public UnityEngine.UI.Text localPlayerNameUI;

        public static Menu Get()
        {
            GameObject go = GameObject.Find("[Thry]SAO_Menu");
            if (go != null)
                return go.GetComponent<Menu>();
            return null;
        }

        public void Start()
        {
            Init();
            OnDisable();
            this.gameObject.SetActive(false);
            foreach (GameObject o in disableOnLoad) o.SetActive(false);
            finished = true;
            if (localPlayerNameUI != null && Networking.LocalPlayer!= null) localPlayerNameUI.text = Networking.LocalPlayer.displayName;
        }

        public void OnEnable()
        {
            Init();
            LogDebugInformation();
            animation_direction = 1;
            animation_start_time = Time.time;
            finished = false;
        }

        public void OnDisable()
        {
            if (init)
            {
                for (int i = 0; i < Canvas.childCount; i++)
                {
                    Transform child = Canvas.GetChild(i);
                    child.localScale = Vector3.zero;
                    child.localPosition = targetPositions[i];
                }
            }
        }

        private void LogDebugInformation()
        {
            if (Networking.LocalPlayer == null) return;
            Debug.Log(DEBUG_PREFIX+"[Debug Information] IsVR:" + Networking.LocalPlayer.IsUserInVR() +
                ", InputMethod: "+InputManager.GetLastUsedInputMethod()+
                ", Avazar Height: "+ Networking.LocalPlayer.GetAvatarEyeHeightAsMeters());
        }

        private bool prevVRValue = false;
        private void Init()
        {
            if (!init)
            {
                Debug.Log(DEBUG_PREFIX + " INITILITING...");
                targetPositions = new Vector3[Canvas.childCount];
                targetScale = new Vector3[Canvas.childCount];
                for (int i = 0; i < Canvas.childCount; i++)
                {
                    Transform child = Canvas.GetChild(i);
                    targetPositions[i] = child.localPosition;
                    targetScale[i] = child.localScale;
                    child.localScale = Vector3.zero;
                    child.localPosition = startPosition;
                }
                init = true;
            }
            //Checking for changed vr value every time the menu opens, because vrchat only sets the value to true for vr users awhile after joining and Init() was called the first time
            bool vr = TEST_VR || (VRC.SDKBase.Networking.LocalPlayer != null && VRC.SDKBase.Networking.LocalPlayer.IsUserInVR());
            if (prevVRValue != vr)
            {
                ToggleBoxColliders(transform, vr);
                prevVRValue = vr;
            }
        }

        private void ToggleBoxColliders(Transform parent, bool on)
        {
            if(OUTPUT_EXTRA_DEBUG) Debug.Log(DEBUG_PREFIX + " Turn " + (on ? "on" : "off") + " box colliders for: " + parent.name);
            foreach(BoxCollider collider in parent.GetComponentsInChildren<BoxCollider>(true))
            {
                collider.enabled = on;
            }
            raycastCollider.enabled = !on;
            //bool raycast = !on;
            //foreach (Text u in parent.GetComponentsInChildren<Text>(true)) u.raycastTarget = raycast;
            //foreach (Image u in parent.GetComponentsInChildren<Image>(true)) u.raycastTarget = raycast;
            //foreach (RawImage u in parent.GetComponentsInChildren<RawImage>(true)) u.raycastTarget = raycast;
        }

        float RELATIVE_MENU_POSITION = 0.07f;
        float RELATIVE_MENU_POSITION_DESKTOP = 0.3f;

        public void OpenOnRightHand()
        {
            OpenMenu(true);
        }

        //open menu
        //only if menu is completly closed
        public void OpenMenu(bool isRightHand)
        {
            if (this.gameObject.activeSelf == false)
            {
                SetMenuPosition(isRightHand);
                animation_direction = 1;
                animation_start_time = Time.time;
                this.gameObject.SetActive(true);
                //enable inworld indicators
                foreach (GameObject o in inWorldIndicators) o.SetActive(true);
            }
        }

        private void SetMenuPosition(bool isRightHand)
        {
            float size = Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();
            this.gameObject.transform.localScale = new Vector3(size, size, size);
            VRCPlayerApi.TrackingData head = VRC.SDKBase.Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.Head);
            VRCPlayerApi.TrackingData hand;
            if (isRightHand == true)
                hand = VRC.SDKBase.Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.RightHand);
            else
                hand = VRC.SDKBase.Networking.LocalPlayer.GetTrackingData(VRC.SDKBase.VRCPlayerApi.TrackingDataType.LeftHand);
            if (TEST_VR || VRC.SDKBase.Networking.LocalPlayer.IsUserInVR())
            {
                Vector3 relativePosition = (head.rotation * Vector3.forward).normalized * RELATIVE_MENU_POSITION * size;
                relativePosition.y += MENU_HEIGHT / 2 * size;
                this.transform.position = (hand.position) + relativePosition;
            }
            else
            {
                Vector3 relativePosition = (head.rotation * Vector3.forward).normalized * RELATIVE_MENU_POSITION_DESKTOP * size;
                this.transform.position = head.position + relativePosition;
            }
            this.transform.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0);
        }

        public void CloseMenu()
        {
            if (finished)
            {
                animation_direction = -1;
                animation_start_time = Time.time;
                finished = false;
                //disable inworld indicators
                foreach (GameObject o in inWorldIndicators) o.SetActive(false);
            }
        }

        //startPosition - targetPos.y
        //min startPositon
        //max targetPos

        private void Update()
        {
            if (!finished)
            {
                Animate();
            }
            if (Networking.LocalPlayer != null)
            {
                if (VRC.SDKBase.Networking.LocalPlayer.GetVelocity().magnitude > CLOSE_VELOCITY)
                {
                    CloseMenu();
                }
            }
        }

        private void Animate()
        {
            if (RescaleObjects())
            {
                //take picture of player if menu is done opening
                if(animation_direction == 1 && Networking.LocalPlayer != null)
                {
                    if (localPlayerFaceCamera)
                    {
                        VRCPlayerApi.TrackingData data = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                        Vector3 headPos = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head);
                        if (headPos == Vector3.zero)
                            headPos = data.position;
                        Vector3 cPos = headPos + data.rotation * Vector3.forward * 2;
                        localPlayerFaceCamera.transform.SetPositionAndRotation(cPos, Quaternion.LookRotation(headPos - cPos));
                        localPlayerFaceCamera.orthographicSize = localPlayerCameraSize * Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();
                        localPlayerFaceCamera.Render();
                    }
                }
                finished = true;
            }
            if (finished && animation_direction == -1)
            {
                MainToggleGroup.SetAllTogglesOff();
                this.gameObject.SetActive(false);
            }
        }

        private bool RescaleObjects()
        {
            float scale = (Time.time - animation_start_time) / ANIMATION_DURATION;
            if (animation_direction == -1)
                scale = 1 - scale;
            bool allDone = true;
            for (int i = 0; i < Canvas.childCount; i++)
            {
                float localScale = (scale - (targetPositions[i].y) * ANIMATION_Y_MODIFIER - ANIMATION_Y_OFFSET);
                localScale = Mathf.Clamp01(localScale);
                if ((localScale < 1 && animation_direction == 1) || (localScale > 0 && animation_direction == -1))
                    allDone = false;
                Transform child = Canvas.GetChild(i);
                child.localScale = targetScale[i] * localScale;
                child.localPosition = (startPosition + (targetPositions[i] - startPosition) * localScale);
            }
            return allDone;
        }
    }
}
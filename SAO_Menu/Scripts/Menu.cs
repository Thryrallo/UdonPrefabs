
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SAO
{
    public class Menu : UdonSharpBehaviour
    {
        const string DEBUG_PREFIX = "[Thry][SAO]";

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

        private Transform canvas;

        [Header("Test")]
        public bool TEST_VR;
        public bool OUTPUT_EXTRA_DEBUG;
        
        public BoxCollider raycastCollider;

        [Header("Optional References")]
        public Camera localPlayerCamera;
        public Animator localPlayerCameraAnimator;
        public float localPlayerCameraSize = 0.25f;
        [Tooltip("Enables these gameobjects while menu is open. Suggested use indicators.")]
        public GameObject[] inWorldIndicators;
        [Tooltip("These get disabled when the scene is started.")]
        public GameObject[] disableOnLoad;

        [Header("Local Player Profile")]
        public UnityEngine.UI.Text localPlayerNameUI;

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
                for (int i = 0; i < canvas.childCount; i++)
                {
                    Transform child = canvas.GetChild(i);
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
                ", Avazar Height: "+GetLocalAvatarHeight());
        }

        private bool prevVRValue = false;
        private void Init()
        {
            if (!init)
            {
                Debug.Log(DEBUG_PREFIX + " INITILITING...");
                canvas = transform.GetChild(0);
                targetPositions = new Vector3[canvas.childCount];
                targetScale = new Vector3[canvas.childCount];
                for (int i = 0; i < canvas.childCount; i++)
                {
                    Transform child = canvas.GetChild(i);
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
            float size = GetLocalAvatarHeight();
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
                if(localPlayerCamera != null && animation_direction == 1)
                {
                    if (Networking.LocalPlayer != null)
                    {
                        VRCPlayerApi.TrackingData data = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                        Vector3 cPos = data.position + data.rotation * Vector3.forward * 2;
                        localPlayerCamera.transform.SetPositionAndRotation(cPos, Quaternion.LookRotation(data.position - cPos));
                        localPlayerCamera.orthographicSize = localPlayerCameraSize * GetLocalAvatarHeight();
                    }
                    localPlayerCameraAnimator.SetTrigger("trigger");
                }
                finished = true;
            }
            if (finished && animation_direction == -1)
            {
                this.gameObject.SetActive(false);
            }
        }

        private bool RescaleObjects()
        {
            float scale = (Time.time - animation_start_time) / ANIMATION_DURATION;
            if (animation_direction == -1)
                scale = 1 - scale;
            bool allDone = true;
            for (int i = 0; i < canvas.childCount; i++)
            {
                float localScale = (scale - (targetPositions[i].y) * ANIMATION_Y_MODIFIER - ANIMATION_Y_OFFSET);
                localScale = Mathf.Min(1, Mathf.Max(localScale, 0));
                if ((localScale < 1 && animation_direction == 1) || (localScale > 0 && animation_direction == -1))
                    allDone = false;
                Transform child = canvas.GetChild(i);
                child.localScale = targetScale[i] * localScale;
                child.localPosition = (startPosition + (targetPositions[i] - startPosition) * localScale);
            }
            return allDone;
        }

        //--------------List Helpers--------------

        private object[] ListAdd(object[] array, object o)
        {
            object[] newArray = new object[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
                newArray[i] = array[i];
            newArray[array.Length] = o;
            return newArray;
        }

        private object[] ListRemoveObject(object[] array, object o)
        {
            object[] newArray = new object[array.Length - 1];
            int newArrayIndex = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != o)
                    newArray[newArrayIndex++] = array[i];
            }
            return newArray;
        }

        private object[] ListRemoveIndex(object[] array, int removeIndex)
        {
            object[] newArray = new object[array.Length - 1];
            int newArrayIndex = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (i != removeIndex)
                    newArray[newArrayIndex++] = array[i];
            }
            return newArray;
        }

        private bool ListContains(object[] array, object o)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == o)
                    return true;
            }
            return false;
        }

        private int ListGetIndex(object[] array, object o)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == o)
                    return i;
            }
            return -1;
        }

        //--------------PLAYER HEIGHT----------------

        public float GetAvatarHeight(VRCPlayerApi player)
        {
            float height = 0;
            Vector3 postition1 = player.GetBonePosition(HumanBodyBones.Head);
            Vector3 postition2 = player.GetBonePosition(HumanBodyBones.Neck);
            height += (postition1 - postition2).magnitude;
            postition1 = postition2;
            postition2 = player.GetBonePosition(HumanBodyBones.Hips);
            height += (postition1 - postition2).magnitude;
            postition1 = postition2;
            postition2 = player.GetBonePosition(HumanBodyBones.RightLowerLeg);
            height += (postition1 - postition2).magnitude;
            postition1 = postition2;
            postition2 = player.GetBonePosition(HumanBodyBones.RightFoot);
            height += (postition1 - postition2).magnitude;
            return height > 0 ? height : 1;
        }
        private float GetLocalAvatarHeight()
        {
            if (Networking.LocalPlayer == null)
                return 1;
            return GetAvatarHeight(Networking.LocalPlayer);
        }
    }
}

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SAO
{
    public class Notification : UdonSharpBehaviour
    {
        [Header("Settings")]
        public bool DO_ANNOUNCE_PLAYERS;
        public Texture playerJoinTexture;
        public Texture playerLeaveTexture;

        [Header("Textures")]
        public Texture tex_Allow;
        public Texture tex_Deny;

        [Header("References")]
        public GameObject canvas;
        public Animator animator;

        public UnityEngine.UI.Text textUI;
        public UnityEngine.UI.RawImage imageUI;

        public UnityEngine.UI.Text details_textUI;
        public GameObject details_background;
        public GameObject details_forground;

        public UnityEngine.UI.Image background;

        private string[] titleQueue = new string[100];
        private string[] detailsQueue = new string[100];
        private Texture[] textureQueue = new Texture[100];
        private Color[] iconColorQueue = new Color[100];
        private Color[] backgroundColorQueue = new Color[100];
        private float[] durationQueue = new float[100];
        private int[] typeQueue = new int[100];

        private int queueStart = 0;
        private int queueEnd = 0;

        private Color defaultColor = new Color(0, 0.8f, 1);

        private const int TYPE_NONE = 0;
        private const int TYPE_JOIN = 1;
        private const int TYPE_LEAVE = 2;

        private int queuedJoinNotifications = 0;
        private int queuedLeaveNotifications = 0;
        private bool sentALotJoinNotification;
        private bool sentALotLeaveNotification;

        private float startTime;
        private void Start()
        {
            startTime = Time.time;
        }

        [HideInInspector]
        public string QueueWithParamText_param0;
        public void QueueWithParamText()
        {
            Queue(QueueWithParamText_param0, null, null, defaultColor, Color.white, 10, TYPE_NONE);
        }

        [HideInInspector]
        public string QueueWithParamTextDuration_param0;
        [HideInInspector]
        public string QueueWithParamTextDuration_param1;
        public void QueueWithParamTextDuration()
        {
            Queue(QueueWithParamTextDuration_param0, null, null, defaultColor, Color.white, float.Parse(QueueWithParamTextDuration_param1), TYPE_NONE);
        }

        public void Queue(string title, string content, Texture texture, Color textureColor, Color backgroundColor, float duration, int type)
        {
            titleQueue[queueEnd] = title;
            detailsQueue[queueEnd] = content;
            textureQueue[queueEnd] = texture;
            iconColorQueue[queueEnd] = textureColor;
            backgroundColorQueue[queueEnd] = backgroundColor;
            durationQueue[queueEnd] = duration;
            typeQueue[queueEnd] = type;
            queueEnd = (queueEnd + 1) % titleQueue.Length;
            //check if no notfication is currently being displayed and none is queued
            Debug.Log("[Thry] [Notification] Length: " + GetLength() + ", canvas inactive: " + (canvas.activeSelf == false));
            if (GetLength() == 1 && canvas.activeSelf == false)
            {
                Next();
            }
        }

        public void Next()
        {
            if (GetLength() > 0)
            {
                textUI.text = titleQueue[queueStart];
                imageUI.texture = textureQueue[queueStart];
                animator.SetFloat("speed", 1.0f / ScaleDuration(durationQueue[queueStart]));
                if (typeQueue[queueStart] == TYPE_JOIN) queuedJoinNotifications -= 1;
                else if (typeQueue[queueStart] == TYPE_LEAVE) queuedLeaveNotifications -= 1;

                bool hasDetails = detailsQueue[queueStart] != null;
                details_background.SetActive(hasDetails);
                details_forground.SetActive(hasDetails);
                if(hasDetails) details_textUI.text = detailsQueue[queueStart];

                //icon color
                imageUI.color = iconColorQueue[queueStart];

                //background color
                background.material.color = backgroundColorQueue[queueStart];

                //text color
                float texColVal = 0.1f;
                if (backgroundColorQueue[queueStart].maxColorComponent < 0.4f) texColVal = 0.9f;
                textUI.color = new Color(texColVal, texColVal, texColVal);
                if (hasDetails) details_textUI.color = textUI.color;

                animator.SetTrigger("open");

                queueStart = (queueStart + 1) % titleQueue.Length;
            }
        }

        private int GetLength()
        {
            return (queueEnd + titleQueue.Length - queueStart) % titleQueue.Length;
        }

        public override void OnPlayerJoined(VRCPlayerApi joinedPlayerApi)
        {
            if (DO_ANNOUNCE_PLAYERS && Time.time - startTime > 10)
            {
                if (queuedJoinNotifications == 0)
                {
                    sentALotJoinNotification = false;
                }
                if (!sentALotJoinNotification)
                {
                    if (queuedJoinNotifications > 5)
                    {
                        Queue("A lot of players", null, playerJoinTexture, new Color(0.25f, 1, 0), Color.white, 5f, TYPE_NONE);
                        sentALotJoinNotification = true;
                    }
                    else
                    {
                        queuedJoinNotifications += 1;
                        Queue(joinedPlayerApi.displayName, null, playerJoinTexture, new Color(0.25f, 1, 0), Color.white, 5f, TYPE_JOIN);
                    }
                }
            }
        }
        public override void OnPlayerLeft(VRCPlayerApi leftPlayerApi)
        {
            if (DO_ANNOUNCE_PLAYERS)
            {
                if (queuedLeaveNotifications == 0)
                {
                    sentALotLeaveNotification = false;
                }
                if (!sentALotLeaveNotification)
                {
                    if (queuedLeaveNotifications > 5)
                    {
                        Queue("A lot of players", null, playerLeaveTexture, new Color(1, 0.5f, 0), Color.white, 5f, TYPE_NONE);
                        sentALotLeaveNotification = true;
                    }
                    else
                    {
                        queuedLeaveNotifications += 1;
                        Queue(leftPlayerApi.displayName, null, playerLeaveTexture, new Color(1, 0.5f, 0), Color.white, 5f, TYPE_LEAVE);
                    }
                }
            }
        }
        private float ScaleDuration(float f)
        {
            return Mathf.Max(1, f * (-0.05f * GetLength() + 1));
        }

        public void Deny(string title, string details)
        {
            Queue(title, details, tex_Deny, Color.red, Color.white, 5, 0);
        }

        public void Allow(string title, string details)
        {
            Queue(title, details, tex_Allow, Color.green, Color.white, 5, 0);
        }

        public void TestNormalNotification()
        {
            Queue("Test Notification", null, null, Color.green, Color.red, 10, 0);
        }

        public void TestNormalNotification2()
        {
            Queue("Test Notification", "This has details for the notification", null, Color.blue, Color.black, 10, 0);
        }

        public void TestPlayerJoin()
        {
            if (DO_ANNOUNCE_PLAYERS && Time.time - startTime > 10)
            {
                if (queuedJoinNotifications == 0)
                {
                    sentALotJoinNotification = false;
                }
                if (!sentALotJoinNotification)
                {
                    if (queuedJoinNotifications > 5)
                    {
                        Queue("A lot of players", null, playerJoinTexture, new Color(0.25f, 1, 0), Color.white, 5f, TYPE_NONE);
                        sentALotJoinNotification = true;
                    }
                    else
                    {
                        queuedJoinNotifications += 1;
                        Queue("Test Player", null, playerJoinTexture, new Color(0.25f, 1, 0), Color.white, 5f, TYPE_JOIN);
                    }
                }
            }
        }
    }
}
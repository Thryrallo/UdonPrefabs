
using Thry.General;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.SpinTheBottle
{
    public class Bottle : UdonSharpBehaviour
    {
        //===========Public Config fields===========

        //Settings
        public int SPIN_COUNT = 5;
        [Tooltip("Degrees per second")]
        public float SPIN_SPEED = 720;
        // 0 = Random
        // 1 = Targeted
        // 2 = Tasks

        //References
        public PlayerTracker playerTracker;
        public UnityEngine.UI.Text targetedPlayerText;
        private AudioSource _audioSource;
        public Animator Menu;
        public Toggle ToggleRandom;
        public Toggle ToggleTargeted;
        public Toggle ToggleTasks;

        public Animator TaskPopup;
        public Text TaskHeader;
        public Text TaskText;
        public string[] Tasks;
        public string[] GeneralTasks;
        private string[] _balancedTasks;

        private AvatarThemeColor _themeColor;
        private Color _defaultTaskHeaderColor;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _themeColor = AvatarThemeColor.Get();
            _defaultTaskHeaderColor = TaskHeader.color;
            if (Networking.IsOwner(gameObject))
            {
                TargetPlayerId = -1;
                TargetAngle = 0.1f;
                LateJoingerAngle = 0.1f;
                RequestSerialization();
            }
            targetedPlayerText.text = "";
            Menu.SetBool("Open", true);
            TaskPopup.SetBool("Open", false);

            // balance tasks by adding them multiple times depending on the amount of variables
            int total = 0;
            int[] count = new int[Tasks.Length + GeneralTasks.Length];
            for(int i = 0; i < Tasks.Length; i++)
            {
                count[i] = CountVariables(Tasks[i]);
                total += count[i];
            }
            for(int i = 0; i < GeneralTasks.Length; i++)
            {
                count[i + Tasks.Length] = CountVariables(GeneralTasks[i]);
                total += count[i + Tasks.Length];
            }

            _balancedTasks = new string[total];
            int index = 0;
            for(int i = 0; i < Tasks.Length; i++)
            {
                for(int j = 0; j < count[i]; j++)
                {
                    _balancedTasks[index++] = Tasks[i];
                }
            }
            for(int i = 0; i < GeneralTasks.Length; i++)
            {
                for(int j = 0; j < count[i + Tasks.Length]; j++)
                {
                    _balancedTasks[index++] = "@all"+GeneralTasks[i];
                }
            }
        }

        //===========Synced vars===========

        [UdonSynced, FieldChangeCallback(nameof(TargetAngle))]
        float _targetAngle;

        [UdonSynced, FieldChangeCallback(nameof(TargetPlayerId))]
        int _targetedPlayerId;

        [UdonSynced, FieldChangeCallback(nameof(LateJoingerAngle))]
        float _lateJoinerAngle;
        
        [UdonSynced, FieldChangeCallback(nameof(GameMode))]
        int _gameMode = 1;

        [UdonSynced]
        string _task = "";
        [UdonSynced]
        string _taskHeader = "";
        [UdonSynced]
        bool _colorHeader = false;

        //===========Local vars===========

        private VRCPlayerApi targetedPlayer;
        private float localTargetAngle;
        private float fullRotationsToDoPercentage = 1;
        private bool isInit;
        private int initCount = 0;

        //===========State machine vars===========

        private int state = 0;
        private float stateStartTime = 0;

        const int S_IDLE = 0;
        const int S_ROTATE = 2;
        const int S_SLOWDOWN = 3;

        //Player Targeting

        public int GameMode
        {
            set
            {
                _gameMode = value;
                ToggleRandom.SetIsOnWithoutNotify(_gameMode == 0);
                ToggleTargeted.SetIsOnWithoutNotify(_gameMode == 1);
                ToggleTasks.SetIsOnWithoutNotify(_gameMode == 2);
                if(_gameMode == 0) targetedPlayerText.text = "";
            }
            get => _gameMode;
        }

        public void ClearPlayerText()
        {
            targetedPlayerText.text = "";
        }

        public void SetResultText()
        {
            if (Utilities.IsValid(targetedPlayer)) targetedPlayerText.text = "Points to: " + targetedPlayer.displayName;
            if(GameMode == 2)
            {
                TaskHeader.text = _taskHeader;
                TaskText.text = _task;
                if(_themeColor && _colorHeader && Utilities.IsValid(targetedPlayer))
                    TaskHeader.color = _themeColor.GetColor(targetedPlayer, _defaultTaskHeaderColor);
                else
                    TaskHeader.color = _defaultTaskHeaderColor;
                OpenTaskPopup();
            }
        }

        private VRCPlayerApi SelectRandomPlayer()
        {
            int i = 0;
            VRCPlayerApi selected;
            while (playerTracker.length > 0)
            {
                i = Random.Range(0, playerTracker.length);
                selected = playerTracker.players[i];
                if (Utilities.IsValid(selected) == false)
                {
                    playerTracker.RemoveAtIndex(i);
                }
                else if (selected.playerId != TargetPlayerId || playerTracker.length == 1)
                {
                    return selected;
                }
            }
            return null;
        }

        private float PlayerToAngle(VRCPlayerApi player)
        {
            if (Utilities.IsValid(player) == false)
            {
                playerTracker.ValidatePlayers();
                return TargetAngle;
            }

            Vector3 playerDir = player.GetPosition() - transform.position;
            playerDir.y = 0;
            Vector3 bottleIdleDir = transform.parent.rotation * Vector3.forward;
            float a = Vector3.Angle(bottleIdleDir, playerDir);

            if ((transform.parent.rotation * playerDir).x < 0) a = 360 - a;

            return a;
        }

        //Networking

        private void InitCount()
        {
            initCount++;
            if (initCount == 3) isInit = true;
        }

        public float TargetAngle
        {
            set
            {
                _targetAngle = value;
                if (isInit)
                {
                    localTargetAngle = _targetAngle;
                    SetState(S_ROTATE);
                }
                InitCount();
            }
            get => _targetAngle;
        }

        public int TargetPlayerId
        {
            set
            {
                _targetedPlayerId = value;
                if (isInit)
                {
                    targetedPlayer = VRCPlayerApi.GetPlayerById(_targetedPlayerId);
                    localTargetAngle = PlayerToAngle(targetedPlayer);
                    SetState(S_ROTATE);
                }
                InitCount();
            }
            get => _targetedPlayerId;
        }

        public float LateJoingerAngle
        {
            set
            {
                _lateJoinerAngle = value;
                if (!isInit)
                {
                    transform.localEulerAngles = new Vector3(0, _lateJoinerAngle, 0);
                }
                InitCount();
            }
            get => _lateJoinerAngle;
        }

        //===========Interaction===========

        public override void Interact()
        {
            Spin();
        }

        public void Spin()
        {
            if (Networking.IsOwner(gameObject) == false) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            TargetAngle = Random.Range(0, 360);
            if (GameMode > 0) {
                targetedPlayer = SelectRandomPlayer();
                if (Utilities.IsValid(targetedPlayer))
                {
                    TargetPlayerId = targetedPlayer.playerId;
                    TargetAngle = PlayerToAngle(targetedPlayer);
                }
            }
            else
            {
                TargetPlayerId = -1;
            }
            LateJoingerAngle = TargetAngle;
            if(GameMode == 2) _task = NextTask();
            RequestSerialization();
            SetState(S_ROTATE);
            string targetName = targetedPlayer != null ? targetedPlayer.displayName : "-";
            Debug.Log($"[Thry][Bottle] Spin to: {TargetAngle} | Target: {TargetPlayerId} ({targetName}) | GameMode: {GameMode} | Task: {_task}");
        }

        //===========Variable serialization===========

        public void UpdateLocalTargetAngle()
        {
            if (TargetPlayerId > -1)
            {
                localTargetAngle = PlayerToAngle(targetedPlayer);
            }
            else
            {
                localTargetAngle = TargetAngle;
            }
        }

        //===========State machine + Spinning===========

        private void SetState(int i)
        {
            state = i;
            stateStartTime = Time.time;
        }

        private int _frame = 0;
        private void Update()
        {
            if (state == S_ROTATE)
            {
                // Close Menu 
                Menu.SetBool("Open", false);
                TaskPopup.SetBool("Open", false);
                //rotates to target pos and then starts slowing down
                bool wasBeforeTarget = transform.localEulerAngles.y < localTargetAngle;
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime);
                //if roated onto or past target rotation, transition to slow down state
                if (wasBeforeTarget && transform.localEulerAngles.y >= localTargetAngle)
                {
                    fullRotationsToDoPercentage = 1 - 1.0f / SPIN_COUNT;
                    SetState(S_SLOWDOWN);
                }
                if (_audioSource != null)
                {
                    _audioSource.enabled = true;
                    _audioSource.volume = 1;
                }

                if (_frame == 0) UpdateLocalTargetAngle();
                _frame = (_frame + 1) % 10;
            }
            else if (state == S_SLOWDOWN)
            {
                // ("degrees to rotate" / "360") => percentage of full rotation towards target rotation
                float completion = 0;
                if (transform.localEulerAngles.y > localTargetAngle) completion = 360 - transform.localEulerAngles.y + localTargetAngle;
                else completion = localTargetAngle - transform.localEulerAngles.y;
                completion = completion / 360;

                //add full rotations percentage
                completion = fullRotationsToDoPercentage + completion / SPIN_COUNT;

                //apply a exponentiation funtion to make a nicer slowdown curve
                completion = Mathf.Pow(completion, 0.5f);

                if (_audioSource != null)
                {
                    _audioSource.enabled = true;
                    _audioSource.volume = completion;
                }

                bool wasBeforeTarget = transform.localEulerAngles.y < localTargetAngle;
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime * completion);
                if (wasBeforeTarget && transform.localEulerAngles.y >= localTargetAngle)
                {
                    //increment rotation count
                    fullRotationsToDoPercentage -= 1.0f / SPIN_COUNT;

                    //if in last rotation and reached target, transition to idle
                    if (fullRotationsToDoPercentage <= 0)
                    {
                        //Debug.Log($"[ThryBottle] Reached target: {localTargetPosition}");
                        SetState(S_IDLE);
                        transform.localEulerAngles = new Vector3(0, localTargetAngle, 0);
                        if (_audioSource != null)
                        {
                            _audioSource.enabled = false;
                            _audioSource.volume = 0;
                        }
                        if (Networking.IsOwner(gameObject))
                        {
                            LateJoingerAngle = localTargetAngle;
                            RequestSerialization();
                        }
                        SetResultText();
                    }
                }

                if (_frame == 0) UpdateLocalTargetAngle();
                _frame = (_frame + 1) % 10;
            }
        }

        public void SetGameModeRandom()
        {
            SetGameMode(0);
        }

        public void SetGameModeTargeted()
        {
            SetGameMode(1);
        }

        public void SetGameModeTasks()
        {
            SetGameMode(2);
        }

        private void SetGameMode(int i)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            GameMode = i;
            RequestSerialization();
        }

        public void ToggleMenu()
        {
            if(TaskPopup.GetBool("Open"))
                TaskPopup.SetBool("Open", false);
            if(Menu.GetBool("Open"))
                Menu.SetBool("Open", false);
            else
                OpenMenu();
        }

        private string GetTargetDisplayname()
        {
            if(Utilities.IsValid(targetedPlayer))
            {
                return targetedPlayer.displayName;
            }
            return "?target?";
        }

        private string[][] VARIABLES = new string[][]
        {
            new string[] { "@color", "red", "green", "blue", "yellow", "orange", "purple", "pink", "black", "white", "brown", "grey" },
            new string[] { "@continent", "Africa", "Asia", "Europe", "America", "Oceania" },
            new string[] { "@pet", "dog", "cat", "fish", "bird", "hamster", "snake", "lizard", "insect" },
            new string[] { "@scary_animal", "spider", "snake", "shark", "bear", "lion", "tiger", "crocodile", "scorpion" },
            new string[] { "@alcohol", "beer", "wine", "vodka", "whiskey", "rum", "gin", "tequila" },
            new string[] { "@vrc_category", "femboy", "egirl", "eboy", "avatar creator", "world creator", "anime enjoyer", "weeb", "programmer", "student" },
            new string[] { "@vrc_hardware", "tundra tracker", "lovense device", "vive tracker" },
            new string[] { "@vrc_clothing", "collar", "thigh highs", "maid outfit", "school uniform", "skirt", "lingerie", "stockings", "panties" },
            new string[] { "@timeframe", "today", "last week", "last month", "last year", "ever" },
            new string[] { "@vrc_phrase", "good girl", "good boy", "mirror dweller", "Portal!", "poggers", "owo", "uwu", "pog", "I'm pogging!", "Tupper", "VRAM", "I am lagging." },
            new string[] { "@vrc_platform_type", "Desktop", "VR", "Fullbody", "Quest" },
            new string[] { "@weird_food", "eel", "snail", "frog", "insect", "octopus", "squid", "kangaroo","crocodile", "horse" }
        };

        // Random weird / unique / unusual phrases
        private string[] RANDOM_PHRASE_LIST = new string[] { "Blahaj", "Ikea", "Bleep", "Make the frogs gay!", "femboy friday", "I'm a catgirl", "I'm a catboy", "Furry hangout", "Squirtle gang", "Tupper", "VRRat", "VRChat Yacht", "Fix the game!", "Damn, i love pizza", "Pizza, pizza in my tummy, me so hungy, me so hungy", "The One Piece is real!", "raid shadow legends", "Hunter Biden", "Donald Trump", "Libtard", "Chemtrails", "Bigfoot", "Illuminati", "Flat Earth", "Moon landing", "Aliens", "Area 51", "9/11", "Bush did 9/11", "Jet fuel can't melt steel beams", "Vaccines cause autism", "Pizzagate", "QAnon", "Pizza", "Pasta", "Pasta la vista, baby", "Donald Duck", "Micky Mouse", "Kim Possible", "Doofenschmirtz", "Perry the Platypus", "No, this is Patrick!" };

        private string NextTask()
        {
            _task = _balancedTasks[Random.Range(0, _balancedTasks.Length)];
            // task target
            _taskHeader = "@" + GetTargetDisplayname();
            _colorHeader = true;
            if (_task.StartsWith("@all"))
            {
                _task = _task.Remove(0, 4);
                _taskHeader = "@everyone";
                _colorHeader = false;
            }
            // replace varialbes
            int index = -1;
            // random variables
            foreach(string[] var in VARIABLES)
            {
                _task = ReplaceVarialbe(_task, var);
            }
            // random player: @player
            while((index = _task.IndexOf("@p") ) > -1)
            {
                string name = SelectRandomPlayer().displayName;
                _task = _task.Remove(index, 2).Insert(index, name);
            }
            // tartget player: @target
            while((index = _task.IndexOf("@target") ) > -1)
            {
                string name = GetTargetDisplayname();
                _task = _task.Remove(index, 7).Insert(index, name);
            }
            // random number in int range inclusive: @range(min,max)
            while((index = _task.IndexOf("@range") ) > -1)
            {
                int start = _task.IndexOf("(", index);
                int end = _task.IndexOf(")", index);
                string[] range = _task.Substring(start + 1, end - start - 1).Split(',');
                int min = int.Parse(range[0]);
                int max = int.Parse(range[1]);
                int number = Random.Range(min, max + 1);
                _task = _task.Remove(index, end - index + 1).Insert(index, number.ToString());
            }
            // random letter: @letter
            while((index = _task.IndexOf("@letter") ) > -1)
            {
                char letter = (char)('a' + Random.Range(0, 26));
                _task = _task.Remove(index, 7).Insert(index, letter.ToString());
            }
            // random phrase: @phrase
            while((index = _task.IndexOf("@phrase") ) > -1)
            {
                string phrase = RANDOM_PHRASE_LIST[Random.Range(0, RANDOM_PHRASE_LIST.Length)];
                _task = _task.Remove(index, 7).Insert(index, phrase);
            }
            // different word for target player
            // syntax: @iftarget(sentence target|sentence not target)
            while((index = _task.IndexOf("@iftarget") ) > -1)
            {
                int start = _task.IndexOf("(", index);
                int end = _task.IndexOf(")", index);
                string[] sentences = _task.Substring(start + 1, end - start - 1).Split('|');
                bool isTarget = _targetedPlayerId == Networking.LocalPlayer.playerId;
                string sentence = sentences[isTarget ? 0 : 1];
                _task = _task.Remove(index, end - index + 1).Insert(index, sentence);
            }

            return _task;
        }

        private int CountVariables(string task)
        {
            int count = 1;
            foreach(string[] var in VARIABLES)
            {
                if(task.Contains(var[0]))
                {
                    count += VARIABLES.Length - 1;
                }
            }
            return count;
        }
        
        private string ReplaceVarialbe(string task, string[] variable)
        {
            int index = -1;
            while((index = task.IndexOf(variable[0]) ) > -1)
            {
                string value = variable[Random.Range(1, variable.Length)];
                task = task.Remove(index, variable[0].Length).Insert(index, value);
            }
            return task;
        }

        private void OpenMenu()
        {
            // align Menu Y Rotation so it faces the player
            Vector3 playerDir = Menu.transform.position - Networking.LocalPlayer.GetPosition();
            playerDir.y = 0;
            Menu.transform.LookAt(Menu.transform.position + playerDir);
            Menu.SetBool("Open", true);
        }

        private void OpenTaskPopup()
        {
            // align Menu Y Rotation so it faces the player
            Vector3 playerDir = TaskPopup.transform.position - Networking.LocalPlayer.GetPosition();
            playerDir.y = 0;
            TaskPopup.transform.LookAt(TaskPopup.transform.position + playerDir);
            TaskPopup.SetBool("Open", true);
        }
    }
}
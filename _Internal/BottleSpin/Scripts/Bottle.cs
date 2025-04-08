using Thry.Udon.AvatarTheme;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Thry.Udon.SpinTheBottle
{
    enum BottleState
    {
        Idle,
        Spinning,
        Slowdown,
        SlowSpin
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
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
        [SerializeField] TextAsset _taskFile;
        [SerializeField] PlayerTracker _playerTracker;
        [SerializeField] UnityEngine.UI.Text _targetedPlayerText;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] Animator _menu;
        [SerializeField] Toggle _toggleRandom;
        [SerializeField] Toggle _toggleTargeted;
        [SerializeField] Toggle _toggleTasks;

        [SerializeField] Animator _taskPopup;
        [SerializeField] Text _taskHeaderLabel;
        [SerializeField] Text _taskTextLabel;

        [SerializeField, HideInInspector] AvatarThemeColor _themeColor;
        private Color _defaultTaskHeaderColor;

        string[] _indi_tasks = new string[0];
        string[] _group_tasks = new string[0];
        string[] _variableNames = new string[0];
        string[][] _variableValues = new string[0][];

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _defaultTaskHeaderColor = _taskHeaderLabel.color;
            if (Networking.IsOwner(gameObject))
            {
                TargetPlayerId = -1;
                TargetAngle = 0.1f;
                LateJoingerAngle = 0.1f;
                RequestSerialization();
            }
            _targetedPlayerText.text = "";
            _menu.SetBool("Open", true);
            _taskPopup.SetBool("Open", false);

            // Load tasks
            bool parseVariables = true;
            int variableIndex = 0;
            int indiIndex = 0;
            int groupIndex = 0;
            bool isGroupTask = false;
            foreach(string l in _taskFile.text.Split('\n'))
            {
                string line = l.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                if(parseVariables)
                {
                    if(line.StartsWith("@"))
                    {
                        string[] parts = line.Split(',');
                        Collections.AssertArrayLength(ref _variableNames, variableIndex + 1);
                        Collections.AssertArrayLength(ref _variableValues, variableIndex + 1);
                        _variableNames[variableIndex] = parts[0].Trim();
                        _variableValues[variableIndex] = new string[parts.Length - 1];
                        for (int i = 1; i < parts.Length; i++)
                            _variableValues[variableIndex][i - 1] = parts[i].Trim();
                        variableIndex++;
                    }
                    else
                    {
                        parseVariables = false;
                    }
                }

                if(!parseVariables)
                {
                    if(line == "__Individual__")
                    {
                        isGroupTask = false;
                    }
                    else if(line == "__Group__")
                    {
                        isGroupTask = true;
                    }else if(!isGroupTask)
                    {
                        Collections.AssertArrayLength(ref _indi_tasks, indiIndex + 1);
                        _indi_tasks[indiIndex++] = line.Trim();
                    }
                    else
                    {
                        Collections.AssertArrayLength(ref _group_tasks, groupIndex + 1);
                        _group_tasks[groupIndex++] = line.Trim();
                    }

                }
            }

            _variableNames = Collections.SubArray(_variableNames, 0, variableIndex);
            _variableValues = Collections.SubArray(_variableValues, 0, variableIndex);
            _indi_tasks = Collections.SubArray(_indi_tasks, 0, indiIndex);
            _group_tasks = Collections.SubArray(_group_tasks, 0, groupIndex);

            // balance tasks by adding them multiple times depending on the amount of variables
            //int total = 0;
            //int[] count = new int[Tasks.Length + GeneralTasks.Length];
            //for(int i = 0; i < Tasks.Length; i++)
            //{
            //    count[i] = CountVariables(Tasks[i]);
            //    total += count[i];
            //}
            //for(int i = 0; i < GeneralTasks.Length; i++)
            //{
            //    count[i + Tasks.Length] = CountVariables(GeneralTasks[i]);
            //    total += count[i + Tasks.Length];
            //}

            //_balancedTasks = new string[total];
            //int index = 0;
            //for(int i = 0; i < Tasks.Length; i++)
            //{
            //    for(int j = 0; j < count[i]; j++)
            //    {
            //        _balancedTasks[index++] = Tasks[i];
            //    }
            //}
            //for(int i = 0; i < GeneralTasks.Length; i++)
            //{
            //    for(int j = 0; j < count[i + Tasks.Length]; j++)
            //    {
            //        _balancedTasks[index++] = "@all"+GeneralTasks[i];
            //    }
            //}
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
        bool _isGroupTask = false;

        //===========Local vars===========

        private VRCPlayerApi _targetedPlayer;
        private float _localTargetAngle;
        private float _fullRotationsToDoPercentage = 1;
        private bool _isInit;
        private int _initCount = 0;

        //===========State machine vars===========

        private BottleState _state = BottleState.Idle;
        private float _stateStartTime = 0;
        private float _slowSpinSpeed = 0;

        //Player Targeting

        public int GameMode
        {
            set
            {
                _gameMode = value;
                _toggleRandom.SetIsOnWithoutNotify(_gameMode == 0);
                _toggleTargeted.SetIsOnWithoutNotify(_gameMode == 1);
                _toggleTasks.SetIsOnWithoutNotify(_gameMode == 2);
                if(_gameMode == 0) _targetedPlayerText.text = "";
            }
            get => _gameMode;
        }

        public void ClearPlayerText()
        {
            _targetedPlayerText.text = "";
        }

        public void SetResultText()
        {
            if (Utilities.IsValid(_targetedPlayer)) _targetedPlayerText.text = "Points to: " + _targetedPlayer.displayName;
            if(GameMode == 2)
            {
                // different word for target player
                // syntax: @iftarget(sentence target|sentence not target)
                int index;
                while ((index = _task.IndexOf("@iftarget")) > -1)
                {
                    int start = _task.IndexOf("(", index);
                    int end = _task.IndexOf(")", index);
                    string[] sentences = _task.Substring(start + 1, end - start - 1).Split('|');
                    bool isTarget = _targetedPlayerId == Networking.LocalPlayer.playerId;
                    string sentence = sentences[isTarget ? 0 : 1];
                    _task = _task.Remove(index, end - index + 1).Insert(index, sentence);
                }

                _taskHeaderLabel.text = _taskHeader;
                _taskTextLabel.text = _task;
                if (!_isGroupTask && _themeColor && Utilities.IsValid(_targetedPlayer) && _themeColor.GetColor(_targetedPlayer, out Color c))
                    _taskHeaderLabel.color = c;
                else
                    _taskHeaderLabel.color = _defaultTaskHeaderColor;
                OpenTaskPopup();
            }
        }

        private VRCPlayerApi SelectRandomPlayer()
        {
            int i = 0;
            VRCPlayerApi selected;
            while (_playerTracker.length > 0)
            {
                i = Random.Range(0, _playerTracker.length);
                selected = _playerTracker.players[i];
                if (Utilities.IsValid(selected) == false)
                {
                    _playerTracker.RemoveAtIndex(i);
                }
                else if (selected.playerId != TargetPlayerId || _playerTracker.length == 1)
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
                _playerTracker.ValidatePlayers();
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
            _initCount++;
            if (_initCount == 3) _isInit = true;
        }

        public float TargetAngle
        {
            set
            {
                _targetAngle = value;
                if (_isInit)
                {
                    _localTargetAngle = _targetAngle;
                    SetState(BottleState.Spinning);
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
                if (_isInit)
                {
                    _targetedPlayer = VRCPlayerApi.GetPlayerById(_targetedPlayerId);
                    _localTargetAngle = PlayerToAngle(_targetedPlayer);
                    SetState(BottleState.Spinning);
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
                if (!_isInit)
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
                _targetedPlayer = SelectRandomPlayer();
                if (Utilities.IsValid(_targetedPlayer))
                {
                    TargetPlayerId = _targetedPlayer.playerId;
                    TargetAngle = PlayerToAngle(_targetedPlayer);
                }
            }
            else
            {
                TargetPlayerId = -1;
            }
            LateJoingerAngle = TargetAngle;
            if(GameMode == 2) NextTask();
            RequestSerialization();
            SetState(BottleState.Spinning);
            string targetName = _targetedPlayer != null ? _targetedPlayer.displayName : "-";
            Debug.Log($"[Thry][Bottle] Spin to: {TargetAngle} | Target: {TargetPlayerId} ({targetName}) | GameMode: {GameMode} | Task: {_task}");
        }

        //===========Variable serialization===========

        public void UpdateLocalTargetAngle()
        {
            if (TargetPlayerId > -1)
            {
                _localTargetAngle = PlayerToAngle(_targetedPlayer);
            }
            else
            {
                _localTargetAngle = TargetAngle;
            }
        }

        //===========State machine + Spinning===========

        private void SetState(BottleState i)
        {
            _state = i;
            _stateStartTime = Time.time;
        }

        private int _frame = 0;
        private void Update()
        {
            if (_state == BottleState.Spinning)
            {
                // Close Menu 
                _menu.SetBool("Open", false);
                _taskPopup.SetBool("Open", false);
                //rotates to target pos and then starts slowing down
                bool wasBeforeTarget = transform.localEulerAngles.y < _localTargetAngle;
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime);
                //if roated onto or past target rotation, transition to slow down state
                if (wasBeforeTarget && transform.localEulerAngles.y >= _localTargetAngle)
                {
                    _fullRotationsToDoPercentage = 1 - 1.0f / SPIN_COUNT;
                    SetState(BottleState.Slowdown);
                }
                if (_audioSource != null)
                {
                    _audioSource.enabled = true;
                    _audioSource.volume = 1;
                }

                if (_frame == 0) UpdateLocalTargetAngle();
                _frame = (_frame + 1) % 10;
            }
            else if (_state == BottleState.Slowdown)
            {
                // ("degrees to rotate" / "360") => percentage of full rotation towards target rotation
                float completion = 0;
                if (transform.localEulerAngles.y > _localTargetAngle) completion = 360 - transform.localEulerAngles.y + _localTargetAngle;
                else completion = _localTargetAngle - transform.localEulerAngles.y;
                completion = completion / 360;

                //add full rotations percentage
                completion = _fullRotationsToDoPercentage + completion / SPIN_COUNT;

                // For group tasks go to slowly spinning state while task is being shown
                if (GameMode == 2 && _isGroupTask && completion <= 0.05)
                {
                    _slowSpinSpeed = completion;
                    SetState(BottleState.SlowSpin);
                    SetResultText();
                }

                //apply a exponentiation funtion to make a nicer slowdown curve
                completion = Mathf.Pow(completion, 0.5f);

                if (_audioSource != null)
                {
                    _audioSource.enabled = true;
                    _audioSource.volume = completion;
                }

                bool wasBeforeTarget = transform.localEulerAngles.y < _localTargetAngle;
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime * completion);
                if (wasBeforeTarget && transform.localEulerAngles.y >= _localTargetAngle)
                {
                    //increment rotation count
                    _fullRotationsToDoPercentage -= 1.0f / SPIN_COUNT;
                    //if in last rotation and reached target, transition to idle
                    if (_fullRotationsToDoPercentage <= 0)
                    {
                        //Debug.Log($"[ThryBottle] Reached target: {localTargetPosition}");
                        SetState(BottleState.Idle);
                        transform.localEulerAngles = new Vector3(0, _localTargetAngle, 0);
                        if (_audioSource != null)
                        {
                            _audioSource.enabled = false;
                            _audioSource.volume = 0;
                        }
                        if (Networking.IsOwner(gameObject))
                        {
                            LateJoingerAngle = _localTargetAngle;
                            RequestSerialization();
                        }
                        SetResultText();
                    }
                }

                if (_frame == 0) UpdateLocalTargetAngle();
                _frame = (_frame + 1) % 10;
            }else if(_state == BottleState.SlowSpin)
            {
                float speed = Mathf.Max(0, 1 - (Time.time - _stateStartTime) / 60) * _slowSpinSpeed;
                speed = Mathf.Pow(speed, 0.5f);
                transform.Rotate(Vector3.up * SPIN_SPEED * Time.deltaTime * speed);
                if (speed == 0)
                {
                    if (_audioSource != null)
                    {
                        _audioSource.enabled = false;
                        _audioSource.volume = 0;
                    }
                    SetState(BottleState.Idle);
                    if (Networking.IsOwner(gameObject))
                    {
                        LateJoingerAngle = transform.localEulerAngles.y;
                        RequestSerialization();
                    }
                }  
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
            if(_taskPopup.GetBool("Open"))
                _taskPopup.SetBool("Open", false);
            if(_menu.GetBool("Open"))
                _menu.SetBool("Open", false);
            else
                OpenMenu();
        }

        private string GetTargetDisplayname()
        {
            if(Utilities.IsValid(_targetedPlayer))
            {
                return _targetedPlayer.displayName;
            }
            return "?target?";
        }

        private void NextTask()
        {
            int taskIndex = Random.Range(0, _indi_tasks.Length + _group_tasks.Length);
            if(taskIndex >= _indi_tasks.Length)
            {
                _task = _group_tasks[taskIndex - _indi_tasks.Length];
                _isGroupTask = true;
                _taskHeader = "Group Task";
            }
            else
            {
                _task = _indi_tasks[taskIndex];
                _isGroupTask = false;
                _taskHeader = "@" + GetTargetDisplayname();
            }
            // replace varialbes
            int index = -1;
            // random variables
            for(int i=0;i<_variableNames.Length;i++)
            {
                _task = ReplaceVarialbe(_task, _variableNames[i], _variableValues[i]);
            }
            // random player: @player
            while((index = _task.IndexOf("@random_player") ) > -1)
            {
                string name = SelectRandomPlayer().displayName;
                _task = _task.Remove(index, 14).Insert(index, name);
            }
            // tartget player: @target
            while((index = _task.IndexOf("@target") ) > -1)
            {
                string name = GetTargetDisplayname();
                _task = _task.Remove(index, 7).Insert(index, name);
            }
            // random number in int random_integer inclusive: @random_integer(min,max)
            while ((index = _task.IndexOf("@random_integer") ) > -1)
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
        }

        //private int CountVariables(string task)
        //{
        //    int count = 1;
        //    foreach(string[] var in VARIABLES)
        //    {
        //        if(task.Contains(var[0]))
        //        {
        //            count += VARIABLES.Length - 1;
        //        }
        //    }
        //    return count;
        //}
        
        private string ReplaceVarialbe(string task, string vairalbleName, string[] values)
        {
            int index = -1;
            while((index = task.IndexOf(vairalbleName) ) > -1)
            {
                string value = values[Random.Range(0, values.Length)];
                task = task.Remove(index, vairalbleName.Length).Insert(index, value);
            }
            return task;
        }

        private void OpenMenu()
        {
            // align Menu Y Rotation so it faces the player
            Vector3 playerDir = _menu.transform.position - Networking.LocalPlayer.GetPosition();
            playerDir.y = 0;
            _menu.transform.LookAt(_menu.transform.position + playerDir);
            _menu.SetBool("Open", true);
        }

        private void OpenTaskPopup()
        {
            // align Menu Y Rotation so it faces the player
            Vector3 playerDir = _taskPopup.transform.position - Networking.LocalPlayer.GetPosition();
            playerDir.y = 0;
            _taskPopup.transform.LookAt(_taskPopup.transform.position + playerDir);
            _taskPopup.SetBool("Open", true);
        }
    }
}
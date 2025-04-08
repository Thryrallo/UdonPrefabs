
using UdonSharp;
using UnityEngine;

namespace Thry.Udon.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TimeDisplay : UdonSharpBehaviour
    {
        [SerializeField] UnityEngine.UI.Text _textComponent;

        private void Start()
        {
            if(_textComponent == null)
                _textComponent = GetComponent<UnityEngine.UI.Text>();
        }

        readonly string[] _formats = new string[] { "%h:mm tt", "%h mm tt", "HH:mm", "HH mm" };
        System.DateTime _time;
        void Update()
        {
            _time = System.DateTime.Now;
            int formatIndex = (_time.Second % 20 < 10 ? 2 : 0) + (_time.Millisecond < 500 ? 0 : 1);
            _textComponent.text = _time.ToString(_formats[formatIndex]);
        }
    }
}

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Thry.General
{
    public class TimeDisplay : UdonSharpBehaviour
    {
        public UnityEngine.UI.Text textComponent;

        private void Start()
        {
            if(textComponent == null)
                textComponent = GetComponent<UnityEngine.UI.Text>();
        }

        System.DateTime time;
        void Update()
        {
            time = System.DateTime.Now;
            if(time.Millisecond < 500)
                textComponent.text = System.DateTime.Now.ToShortTimeString();
            else
                textComponent.text = System.DateTime.Now.ToShortTimeString().Replace(":", " ");
        }
    }
}
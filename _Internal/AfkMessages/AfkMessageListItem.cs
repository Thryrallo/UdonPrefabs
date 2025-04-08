
using UdonSharp;
using UnityEngine.UI;

namespace Thry.Udon.UI
{
    public class AfkMessageListItem : UdonSharpBehaviour
    {
        public Text User;
        public Text Message;
        public Text Time;

        public void Set(string user, string message, string time)
        {
            User.text = user;
            Message.text = message;
            Time.text = time;
        }
    }
}
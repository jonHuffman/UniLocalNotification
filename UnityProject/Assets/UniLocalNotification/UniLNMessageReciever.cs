using UnityEngine;

namespace Sanukin39
{
    public class UniLNMessageReciever : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Assert(gameObject.name == "UniLNMessageReciever", "[UniLNMessageReciever] - The GameObject with this Component must be named UniLNMessageReciever!");
            DontDestroyOnLoad(gameObject);
        }

        public void LogMessage(string message)
        {
            Debug.Log(string.Format("<color=#A4C639>{0}</color>", message));
        }
    }
}

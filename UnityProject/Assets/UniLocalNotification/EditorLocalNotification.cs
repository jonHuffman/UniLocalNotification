namespace Sanukin39
{
    public class EditorLocalNotification : ILocalNotification
    {
        public void Initialize()
        {
        }

        public bool IsNotificationPermitted()
        {
            return true;
        }

        public void OpenAppSetting()
        {
        }

        int ILocalNotification.RegisterSimple(int delayTime, string message, string title)
        {
            return 0;
        }

        public void Register(int requestCode, int delayTime, string message, string title)
        {
        }

        public void CancelAllSimpleNotifications()
        {
        }

        public void CancelNotification(int requestCode)
        {
        }
    }
}
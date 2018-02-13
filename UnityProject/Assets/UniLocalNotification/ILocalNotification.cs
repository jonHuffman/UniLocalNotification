namespace Sanukin39
{
    /// <summary>
    /// Interface of using local notification
    /// </summary>
    public interface ILocalNotification
    {
        void Initialize();
        bool IsNotificationPermitted();
        void OpenAppSetting();
        int RegisterSimple(int delayTime, string message, string title);
        void Register(int requestCode, int delayTime, string message, string title);
        void CancelAllSimpleNotifications();
        void CancelNotification(int requestCode);
    }
}
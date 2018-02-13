using Sanukin39;

/// <summary>
/// Local Notification Manager
/// </summary>
public class UniLocalNotification
{

    private static ILocalNotification notification;

    /// <summary>
    /// Initialize this instance.
    /// </summary>
    public static void Initialize()
    {
#if UNITY_EDITOR
        notification = new EditorLocalNotification();
#elif UNITY_ANDROID
        notification = new AndroidLocalNotification();
#elif UNITY_IOS
        notification = new IOSLocalNotification();
#endif
        notification.Initialize();
    }

    /// <summary>
    /// Ises the local notification permitted.
    /// </summary>
    /// <returns><c>true</c>, if local notification was permitted, <c>false</c> otherwise.</returns>
    public static bool IsLocalNotificationPermitted()
    {
        return notification.IsNotificationPermitted();
    }

    /// <summary>
    /// Opens the app setting.
    /// </summary>
    public static void OpenAppSetting()
    {
        notification.OpenAppSetting();
    }

    /// <summary>
    /// Register a notification with the specified delayTime, message, and title.
    /// Only 5 concurrent simple notifications can be registered at a time
    /// </summary>
    /// <param name="delayTime">Time in seconds from DateTime.Now to schedule the notification at</param>
    /// <param name="message">Message.</param>
    /// <param name="title">Title. (Optional)</param>
    public static int RegisterSimple(int delayTime, string message, string title = "")
    {
        return notification.RegisterSimple(delayTime, message, title);
    }

    /// <summary>
    /// Registers a notification with the specified ID, delayTime, message, and title.
    /// </summary>
    /// <param name="requestCode">The ID of the notification to register</param>
    /// <param name="delayTime">Time in seconds from DateTime.Now to schedule the notification at</param>
    /// <param name="message">Message.</param>
    /// <param name="title">Title. (Optional)</param>
    public static void Register(int requestCode, int delayTime, string message, string title = "")
    {
        notification.Register(requestCode, delayTime, message, title);
    }

    /// <summary>
    /// Cancels all simple local notification
    /// </summary>
    public static void CancelAllSimpleNotifications()
    {
        notification.CancelAllSimpleNotifications();
    }

    /// <summary>
    /// Cancels a specific notification
    /// </summary>
    /// <param name="requestCode">The ID of the notification to cancel</param>
    public static void CancelNotification(int requestCode)
    {
        notification.CancelNotification(requestCode);
    }
}


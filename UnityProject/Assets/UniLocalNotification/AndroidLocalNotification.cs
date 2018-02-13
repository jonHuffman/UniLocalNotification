#if UNITY_ANDROID
using UnityEngine;

namespace Sanukin39
{
    public class AndroidLocalNotification : ILocalNotification
    {
        const int MaxSimpleNotifications = 5;
        const string PackageName = "net.sanukin.unilocalnotification.NotificationSender";

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize()
        {
            // Do nothing...
        }

        /// <summary>
        /// Check notification is permitted
        /// </summary>
        /// <returns><c>true</c>, if notification is permitted , <c>false</c> otherwise.</returns>
        public bool IsNotificationPermitted()
        {
            var c = new AndroidJavaClass(PackageName);
            return c.CallStatic<bool>("isNotificationPermitted");
        }

        /// <summary>
        /// Open app setting
        /// </summary>
        public void OpenAppSetting()
        {
            var c = new AndroidJavaClass(PackageName);
            c.CallStatic("openAppSettings");
        }

        /// <summary>
        /// Register local notification and returns the ID. Used for quickly scheduling a notification.
        /// </summary>
        /// <param name="delayTime">Delay time.</param>
        /// <param name="message">Notification Message.</param>
        /// <param name="title">Notification Title.</param>
        /// <returns>ID of the registered notification.</returns>
        public int RegisterSimple(int delayTime, string message, string title = "")
        {
            var c = new AndroidJavaClass(PackageName);
            for (int i = 0; i < MaxSimpleNotifications; i++)
            {
                if (!c.CallStatic<bool>("hasPendingIntent", int.MaxValue - i))
                {
                    c.CallStatic("setNotification", title, message, delayTime, int.MaxValue - i);
                    return i;
                }
            }

            Debug.LogError("[UniLocalNotification] - Exceeded maximum concurrent simple notifications!");
            return -1;
        }

        /// <summary>
        /// Register local notification with the specified ID. 
        /// WARNING: You must track and manage these yourself, <see cref="CancelAllSimpleNotifications" /> cannot cancel notifications registerd in this fashion.
        /// </summary>
        /// <param name="requestCode">Notification ID. IDs 2,147,483,643 - 2,147,483,647 are reserved for simple notifications</param>
        /// <param name="delayTime">Delay time.</param>
        /// <param name="message">Notification Message.</param>
        /// <param name="title">Notification Title.</param>
        public void Register(int requestCode, int delayTime, string message, string title = "")
        {
            var c = new AndroidJavaClass(PackageName);

            if (!c.CallStatic<bool>("hasPendingIntent", requestCode))
            {
                c.CallStatic("setNotification", title, message, delayTime, requestCode);
            }
            else
            {
                Debug.LogWarningFormat("[UniLocalNotification] - A notification with ID {0} is already registered. Cancel it before attemtping to reschedule it.");
            }
        }

        /// <summary>
        /// Cancels all simple notifications
        /// </summary>
        public void CancelAllSimpleNotifications()
        {
            var c = new AndroidJavaClass(PackageName);
            for (int i = 0; i < MaxSimpleNotifications; i++)
            {
                if (c.CallStatic<bool>("hasPendingIntent", i))
                {
                    c.CallStatic("cancel", i);
                }
            }
        }

        /// <summary>
        /// Cancels a specific scheduled notification
        /// </summary>
        /// <param name="requestCode">Notification ID.</param>
        public void CancelNotification(int requestCode)
        {
            var c = new AndroidJavaClass(PackageName);

            if (c.CallStatic<bool>("hasPendingIntent", requestCode))
            {
                c.CallStatic("cancel", requestCode);
            }
        }
    }
}
#endif

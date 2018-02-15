#if !UNITY_IOS
using System.Runtime.InteropServices;
using NotificationType = UnityEngine.iOS.NotificationType;
using LocalNotification = UnityEngine.iOS.LocalNotification;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Sanukin39 {
    public class IOSLocalNotification : ILocalNotification
    {
        [DllImport("__Internal")]
        static extern void OpenAppSetting_();

        [DllImport("__Internal")]
        static extern bool IsNotificationPermitted_();

        const int MaxSimpleNotifications = 5;
        const string NotificationID = "NOTIFICATION_ID";

        /// <summary>
        /// Ask for permission to notify the user
        /// </summary>
        public void Initialize()
        {
            NotificationServices.RegisterForNotifications(
                NotificationType.Alert |
                NotificationType.Badge |
                NotificationType.Sound);
        }

        /// <summary>
        /// Check notification is permitted
        /// </summary>
        /// <returns><c>true</c>, if notification is permitted , <c>false</c> otherwise.</returns>
        public bool IsNotificationPermitted()
        {
            return IsNotificationPermitted_();
        }

        /// <summary>
        /// Open app setting
        /// </summary>
        public void OpenAppSetting()
        {
            OpenAppSetting_();
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
            // Needed a guaranteed unique ID
            int requestCode;

            for (int i = 0; i < MaxSimpleNotifications; i++)
            {
                requestCode = int.MaxValue - i;

                if(!NotificationExists(requestCode))
                {
                    LocalNotification notification = new LocalNotification();
                    notification.fireDate = DateTime.Now.AddSeconds(delayTime);
                    notification.alertBody = message;
                    notification.userInfo = new Dictionary<string, int>() { { NotificationID, requestCode } };
                    NotificationServices.ScheduleLocalNotification(notification);
                    return requestCode;
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
            if (!NotificationExists(requestCode))
            {
                LocalNotification notification = new LocalNotification();
                notification.fireDate = DateTime.Now.AddSeconds(delayTime);
                notification.alertBody = message;
                notification.userInfo = new Dictionary<string, int>() { { NotificationID, requestCode } };
                NotificationServices.ScheduleLocalNotification(notification);
            }
            else
            {
                Debug.LogWarningFormat("[UniLocalNotification] - A notification with ID {0} is already registered. Cancel it before attemtping to reschedule it.", requestCode.ToString());
            }
        }

        /// <summary>
        /// Cancel all simple notifications
        /// </summary>
        public void CancelAllSimpleNotifications()
        {
            for (int i = 0; i < MaxSimpleNotifications; i++)
            {
                InternalCancelNotification(int.MaxValue - i);
            }
        }

        /// <summary>
        /// Cancels a specific scheduled notification
        /// </summary>
        /// <param name="requestCode">Notification ID.</param>
        public void CancelNotification(int requestCode)
        {
            InternalCancelNotification(requestCode);
        }

        private bool NotificationExists(int requestCode)
        {
            foreach(LocalNotification notification in NotificationServices.scheduledLocalNotifications)
            {
                if (notification.userInfo.Contains(NotificationID) && (int)notification.userInfo[NotificationID] == requestCode)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks the Notifications for one that matches the ID and cancels it if it exists.
        /// </summary>
        /// <param name="requestCode">Notification ID.</param>
        private void InternalCancelNotification(int requestCode)
        {
            int index = 0;

            while (NotificationServices.GetLocalNotification(index) != null)
            {
                index++;
                LocalNotification notification = NotificationServices.GetLocalNotification(index);

                if (notification.userInfo.Contains(NotificationID) && (int)notification.userInfo[NotificationID] == requestCode)
                {
                    NotificationServices.CancelLocalNotification(notification);
                    break;
                }
            }
        }
    }
}
#endif
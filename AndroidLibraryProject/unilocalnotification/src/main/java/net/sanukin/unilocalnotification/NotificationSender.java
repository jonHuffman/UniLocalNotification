package net.sanukin.unilocalnotification;

import android.app.Activity;
import android.app.AlarmManager;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.os.Build;
import android.support.v4.app.NotificationManagerCompat;

import java.util.Calendar;

import com.unity3d.player.UnityPlayer;

/**
 * Created by sanukiwataru on 2017/09/17.
 */

public class NotificationSender {

    private static boolean ChannelCreated = false;

    /**
     * Set local notification
     * @param title notification title
     * @param message notification message
     * @param delay seconds to notify when counting from now
     * @param requestCode
     */
    public static void setNotification(String title, String message, int delay, int requestCode) {
        try {
            CreateChannel();

            // Get Context
            Context context = getContext();

            // Create intent
            Intent intent = new Intent(context, NotificationReceiver.class);

            // Register notification info
            intent.putExtra("MESSAGE", message);
            intent.putExtra("TITLE", title);
            intent.putExtra("REQUEST_CODE", requestCode);
            intent.putExtra("channel", "defaultChannel");

            // create sender
            PendingIntent sender = PendingIntent.getBroadcast(context, requestCode, intent, PendingIntent.FLAG_UPDATE_CURRENT);

            // Create notification firing time
            Calendar calendar = Calendar.getInstance();
            calendar.setTimeInMillis(System.currentTimeMillis());
            calendar.add(Calendar.SECOND, delay);

            // Register notification
            AlarmManager alarmManager = (AlarmManager) context.getSystemService(Context.ALARM_SERVICE);
            alarmManager.set(AlarmManager.RTC_WAKEUP, calendar.getTimeInMillis(), sender);
        } catch(Exception e){
            System.err.println("Caught Exception: " + e.getMessage());
            LogToUnity(e.getMessage());
        }
    }

    /**
     * Notification permitted check
     * @return if notification permitted
     */
    public static boolean isNotificationPermitted() {
        return NotificationManagerCompat.from(getContext()).areNotificationsEnabled();
    }

    /**
     * Return if pending intent registered
     * @param requestCode request code
     * @return if notification registered
     */
    public static boolean hasPendingIntent(int requestCode) {

        Context context = getContext();

        boolean hasPendingIntent = (PendingIntent.getBroadcast(context, requestCode,
                new Intent(context, NotificationReceiver.class),
                PendingIntent.FLAG_NO_CREATE) != null);

        return  hasPendingIntent;
    }

    /**
     * Cancel registered notification
     * @param requestCode Pending intent request code you want to cancel
     */
    public static void cancel(int requestCode) {

        Context context = getContext();

        // get pending intent
        AlarmManager alarmManager = (AlarmManager)context.getSystemService(Context.ALARM_SERVICE);
        Intent intent = new Intent(context, NotificationReceiver.class);
        PendingIntent sender = PendingIntent.getBroadcast(context, requestCode, intent, PendingIntent.FLAG_UPDATE_CURRENT);

        // cancel it
        alarmManager.cancel(sender);
    }

    /**
     * Open app notification settings
     */
    public static void openAppSettings(){

        Activity activity = UnityPlayer.currentActivity;

        Intent intent = new Intent();
        intent.setAction("android.settings.APP_NOTIFICATION_SETTINGS");

        //for Android 5-7
        intent.putExtra("app_package", activity.getPackageName());
        intent.putExtra("app_uid", activity.getApplicationInfo().uid);

        // for Android O
        intent.putExtra("android.provider.extra.APP_PACKAGE", activity.getPackageName());

        activity.startActivity(intent);
    }

    /**
     * Get current context
     * @return current context
     */
    private static Context getContext() {
        return UnityPlayer.currentActivity.getApplicationContext();
    }

    private static void CreateChannel()
    {
        if(ChannelCreated){
            return;
        }

        if(Build.VERSION.SDK_INT < Build.VERSION_CODES.O) {
            return;
        }

        NotificationManager notificationManager = (NotificationManager) UnityPlayer.currentActivity.getSystemService(Context.NOTIFICATION_SERVICE);
        NotificationChannel channel = new NotificationChannel("defaultChannel", "defaultName", NotificationManager.IMPORTANCE_DEFAULT);
        channel.setDescription("The default channel description");
        channel.enableLights(true);
        channel.setLightColor(Color.WHITE);
        channel.enableVibration(true);
        channel.setVibrationPattern(new long[]{ 300L, 300L});
        notificationManager.createNotificationChannel(channel);

        ChannelCreated = true;
    }

    private static void LogToUnity(String message)
    {
        UnityPlayer.UnitySendMessage("UniLNMessageReciever", "LogMessage", message);
    }
}

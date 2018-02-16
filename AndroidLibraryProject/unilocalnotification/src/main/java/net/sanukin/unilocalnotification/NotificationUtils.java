package net.sanukin.unilocalnotification;

import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Context;
import android.graphics.Color;
import android.os.Build;

import com.unity3d.player.UnityPlayer;

/**
 * Created by JonathanH on 2/16/2018.
 */

public class NotificationUtils {

    public static void CreateChannel()
    {
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
    }
}

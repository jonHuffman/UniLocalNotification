package net.sanukin.unilocalnotification;

import android.annotation.TargetApi;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Build;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

/**
 * Created by sanukiwataru on 2017/09/17.
 */

public class NotificationReceiver extends BroadcastReceiver {

    /**
     * Receiving notification event
     * @param context current context
     * @param intent received intent
     */
    @Override
    public void onReceive(Context context, Intent intent) {

        if(context == null) {
            Log.w("NotificationReceiver","Unity's scheduled notification provided null context to onReceive");
            context = NotificationUtils.GetAppContext();
        } else {
            Log.d("NotificationReceiver", "context is NOT null in onReceive");
        }

        NotificationManager manager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);

        // get notification info
        String message = intent.getStringExtra("MESSAGE");
        String title = intent.getStringExtra("TITLE");
        int requestCode = intent.getIntExtra("REQUEST_CODE", 0);
        String channel;

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            channel = intent.getStringExtra("CHANNEL");
        }
        else {
            channel = "";
        }


        // create intent for taping notification
        final PackageManager packageManager = context.getPackageManager();
        Intent intentCustom = packageManager.getLaunchIntentForPackage(context.getPackageName());

        PendingIntent contentIntent = PendingIntent.getActivity(context, 0, intentCustom,
                PendingIntent.FLAG_UPDATE_CURRENT);

        NotificationUtils.CreateChannel();

        // Create notification builder
        NotificationCompat.Builder builder = new NotificationCompat.Builder(context, channel);

        builder.setContentIntent(contentIntent)
                .setTicker("")
                .setContentTitle(title)
                .setContentText(message);

        // Create and set large icon bitmap in builder
        ApplicationInfo applicationInfo = null;
        try {
            applicationInfo = packageManager.getApplicationInfo(context.getPackageName(),PackageManager.GET_META_DATA);
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
            return;
        }
        final int appIconResId=applicationInfo.icon;
        Bitmap largeIcon = BitmapFactory.decodeResource(context.getResources(), appIconResId);
        builder.setLargeIcon(largeIcon);

        // Set small icon bitmap in builder
        builder.setSmallIcon(context.getResources().getIdentifier("notification_icon", "drawable", context.getPackageName()));

        // fire now
        builder.setWhen(System.currentTimeMillis());

        // set device notification settings
        builder.setDefaults(Notification.DEFAULT_SOUND
                | Notification.DEFAULT_VIBRATE
                | Notification.DEFAULT_LIGHTS);

        // tap to cancel
        builder.setAutoCancel(true);

        // fire notification
        manager.notify(requestCode, builder.build());
    }
}

using Firebase.Messaging;
using Android.App;
using Android.Content;
using Android;

[Service]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class MyFirebaseMessagingService : FirebaseMessagingService
{
 

    [System.Obsolete]
    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);

      
        string title = message.GetNotification()?.Title;
        string body = message.GetNotification()?.Body;

       
        ShowNotification(title, body);
    }

    [System.Obsolete]
    private void ShowNotification(string title, string body)
    {
      
        Notification.Builder builder = new Notification.Builder(this)
            .SetContentTitle(title)
            .SetContentText(body)
            .SetSmallIcon(Resource.Drawable.notification)
            .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
            .SetPriority((int)NotificationPriority.High);

    
        NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
        notificationManager.Notify(0, builder.Build());
    }
}
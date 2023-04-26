using Firebase.Messaging;
using Wassilni_App; // Replace this with the namespace of your shared project
using Wassilni_App.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Wassilni_App.Droid.PushNotificationHelper))]

namespace Wassilni_App.Droid // Replace this with the namespace of your Android project
{
    public class PushNotificationHelper : IPushNotificationHelper
    {
        public void SubscribeToTopic(string topic)
        {
            FirebaseMessaging.Instance.SubscribeToTopic(topic);
        }

        public void UnsubscribeFromTopic(string topic)
        {
            FirebaseMessaging.Instance.UnsubscribeFromTopic(topic);
        }
    }
}
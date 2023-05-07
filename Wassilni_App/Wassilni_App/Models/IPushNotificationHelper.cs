using System;
using System.Collections.Generic;
using System.Text;

namespace Wassilni_App.Models
{
    public interface IPushNotificationHelper
    {
        void SubscribeToTopic(string topic);
        void UnsubscribeFromTopic(string topic);
    }
}

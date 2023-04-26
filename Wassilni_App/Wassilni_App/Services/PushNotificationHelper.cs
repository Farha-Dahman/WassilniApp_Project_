using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Firebase.Database;
using Wassilni_App.Models;
using Firebase.Database.Query;

public class PushNotificationHelper
{
    private const string FirebaseServerKey = "AAAAZziWK4w:APA91bHlRikBZj_VUPyz9sq7N32PMyimEuD5JthJzjhipnAEEFbJvELfDJ_SgHsZYRA8IT7g_LCYKoXYPKCXsDZ3wv-_Y_SKljwFXy_A2_Qum0Pb6qls9Ahsd6qOIs7sglYseH05Qnft";
    private const string FirebaseSenderId = "443330997132";
    private const string FirebaseNotificationUrl = "https://fcm.googleapis.com/fcm/send";
    FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");

    public async Task SendNotificationAsync(string title, string body, string fcmToken, string recipientUserId,string PhotoUrl)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={FirebaseServerKey}");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={FirebaseSenderId}");
            var messageData = new
            {
                to = fcmToken,
                priority = "high",
                notification = new
                {

                    title = title,
                    body = body,
                    sound = "default", 
                    icon = "notification", 

                
                },
                data = new
                {
                    title = title,
                    body = body
                }
            };
            var jsonMessage = JsonConvert.SerializeObject(messageData);
            Console.WriteLine($"Sending notification: {jsonMessage}"); 

            var response = await httpClient.PostAsync(
                FirebaseNotificationUrl,
                new StringContent(jsonMessage, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response: {response.StatusCode} - {response.Content.ReadAsStringAsync().Result}");
                throw new Exception($"Error sending notification: {response.StatusCode}");
            }
            var newNotification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Title = title,
                Message = body,
                Timestamp = DateTime.UtcNow,
                IsRead = false,
                PhotoUrl= PhotoUrl,

            };
            await SaveNotificationToBackend(newNotification, recipientUserId); 

        }
    }
    private async Task SaveNotificationToBackend(Notification notification, string recipientUserId)
    {
      
        await firebaseClient
            .Child("UserNotifications")
            .Child(recipientUserId)
            .Child(notification.Id)
            .PutAsync(notification);
    }
}
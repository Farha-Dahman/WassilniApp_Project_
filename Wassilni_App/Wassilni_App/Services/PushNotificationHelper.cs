using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

public class PushNotificationHelper
{
    private const string FirebaseServerKey = "AAAAZziWK4w:APA91bHlRikBZj_VUPyz9sq7N32PMyimEuD5JthJzjhipnAEEFbJvELfDJ_SgHsZYRA8IT7g_LCYKoXYPKCXsDZ3wv-_Y_SKljwFXy_A2_Qum0Pb6qls9Ahsd6qOIs7sglYseH05Qnft";
    private const string FirebaseSenderId = "443330997132";
    private const string FirebaseNotificationUrl = "https://fcm.googleapis.com/fcm/send";

    public async Task SendNotificationAsync(string title, string body, string fcmToken)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={FirebaseServerKey}");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={FirebaseSenderId}");

            var messageData = new
            {
                to = fcmToken,
                notification = new
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
        }
    }
}
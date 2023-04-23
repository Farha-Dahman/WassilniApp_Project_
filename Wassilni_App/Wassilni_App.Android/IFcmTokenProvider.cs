using System.Threading.Tasks;
using Android.Gms.Extensions;
using Firebase.Iid;
using Wassilni_App.Models; // Replace with your shared project namespace where IFcmTokenProvider is located
using Xamarin.Forms;

[assembly: Dependency(typeof(Wassilni_App.Droid.FcmTokenProvider))] // Replace with your Android project namespace

namespace Wassilni_App.Droid // Replace with your Android project namespace
{
    public class FcmTokenProvider : IFcmTokenProvider
    {
        public async Task<string> GetFcmTokenAsync()
        {
            var instanceId = await FirebaseInstanceId.Instance.GetInstanceId().AsAsync<IInstanceIdResult>();
            return instanceId.Token;
        }
    }
}
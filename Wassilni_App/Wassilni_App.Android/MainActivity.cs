using System;

using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Hardware.Camera2.Params;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Wassilni_App.Models;
using Firebase;

namespace Wassilni_App.Droid
{
    [Activity(Label = "Wassilni_App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState) 
        {
            Rg.Plugins.Popup.Popup.Init(this);
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            FirebaseApp.InitializeApp(Application.Context);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());


        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {

            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                GoogleManager.Instance.OnAuthCompleted(result);

            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }

        [Obsolete]
        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);

        }
    }

}
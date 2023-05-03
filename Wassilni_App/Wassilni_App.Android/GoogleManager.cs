using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Apache.Http.Authentication;
using Wassilni_App.Models;
using Xamarin.Forms;
using Wassilni_App;
using Wassilni_App.Droid;
using Wassilni_App.views;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using static Java.Util.Jar.Attributes;
using Android.Gms.Auth;

[assembly: Dependency(typeof(GoogleManager))]
namespace Wassilni_App.Droid
{
    public class GoogleManager : Java.Lang.Object, IGoogleManager, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {

        public Action<GoogleUser, string> _onLoginComplete;
        public static GoogleApiClient _googleApiClient { get; set; }
        public static GoogleManager Instance { get; private set; }
        Context _context;


        public GoogleManager()
        {
            _context = global::Android.App.Application.Context;
            Instance = this;
        }

        public void Login(Action<GoogleUser, string> onLoginComplete)
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                             .RequestEmail()
                                                             .Build();
            _googleApiClient = new GoogleApiClient.Builder((_context).ApplicationContext)
                .AddConnectionCallbacks(this)
            .AddOnConnectionFailedListener(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .AddScope(new Scope(Scopes.Profile))
                .Build();

            _onLoginComplete = onLoginComplete;
            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);
            ((MainActivity)Forms.Context).StartActivityForResult(signInIntent, 1);
            _googleApiClient.Connect();
        }

        public void Logout()
        {
            try
            {
                var gsoBuilder = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn).RequestEmail();

                GoogleSignIn.GetClient(_context, gsoBuilder.Build())?.SignOut();

                _googleApiClient.Disconnect();
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");

            }

        }

        public async void OnAuthCompleted(GoogleSignInResult result)
        {
            try
            {
                if (result.IsSuccess)
                {

                    GoogleSignInAccount accountt = result.SignInAccount;

                    _onLoginComplete?.Invoke(new GoogleUser()
                    {
                        FullName = accountt.DisplayName,
                        Email = accountt.Email,
                        Picture = new Uri((accountt.PhotoUrl != null ? $"{accountt.PhotoUrl}" : $"https://autisticdating.net/imgs/profile-placeholder.jpg"))
                    }, string.Empty);

                
            }
                else
                {
                    _onLoginComplete?.Invoke(null, "An error occured!");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public void OnConnected(Bundle connectionHint)
        {

        }

        public void OnConnectionSuspended(int cause)
        {
            _onLoginComplete?.Invoke(null, "Canceled!");
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            _onLoginComplete?.Invoke(null, result.ErrorMessage);
        }
    }
}
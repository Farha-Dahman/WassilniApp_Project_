using System;
using System.IO;
using Xamarin.Forms.Xaml;
using Wassilni_App.viewModels;
using Wassilni_App.Models;
using Rg.Plugins.Popup.Services;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Plugin.Media;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.ComponentModel;
using Xamarin.Forms.Internals;
using Plugin.Media.Abstractions;



using Firebase.Auth;
using Firebase.Database.Query;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reactive.Disposables;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using User = Firebase.Auth.User;

namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpConfirmChanges : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopUpConfirmChanges()
        {
            InitializeComponent();
            this.BindingContext = new EditProfileViewModel();
        }


        MediaFile file;
        public ImageSource profileImage;
        readonly EditProfileViewModel editProfileviewModel = new EditProfileViewModel();
        readonly string userId = Preferences.Get("userId", string.Empty);
        readonly FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
        readonly FirebaseStorage firebaseStorage = new FirebaseStorage("wassilni-app.appspot.com");
        private async void ConfirmChanges_ConfirmClicked(object sender, EventArgs e)
        {


            var user = await firebaseClient.Child("User").Child(userId).OnceSingleAsync<Models.User>();
            if (file != null)
            {
                string image = await editProfileviewModel.Upload(file.GetStream(), Path.GetFileName(file.Path));
                user.PhotoUrl = image;
            }
            await CrossMedia.Current.Initialize();
            try
            {
                /*UserImage.Source = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                });*/
                if (true)
                {
                    var stream = file.GetStream();
                    string imageUrl = await firebaseStorage.Child("profileImages").Child(userId).Child("profile.jpg").GetDownloadUrlAsync();
                    var databaseReference = firebaseClient.Child("User").Child(userId).Child("PhotoUrl");
                    await databaseReference.PutAsync(imageUrl);
                    user.PhotoUrl = imageUrl;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error selecting file: {ex.Message}");
            }
            // Hide the confirmation popup
            await PopupNavigation.Instance.PopAsync();

        }
        private async void ConfirmChanges_CancelClicked(object sender, EventArgs e)
        {
            // Hide the confirmation popup
            await PopupNavigation.Instance.PopAsync();
        }


    }
}
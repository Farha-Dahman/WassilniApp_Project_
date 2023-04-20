using System;
using System.Diagnostics;
using System.IO;
using Wassilni_App.viewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FilePicker;
using Firebase.Storage;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;

namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        private ImageSource profileImage;

        public EditProfilePage()
        {
            InitializeComponent();
            this.BindingContext = new EditProfileViewModel();
        }


        string webAPIkey = "AIzaSyClVyVHgbXooKCTyoKMg6RgfBcnkkFKTX0";
        string userId = Preferences.Get("userId", string.Empty);
        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");


        private async void ChangeProfilePicture_Clicked(object sender, EventArgs e)
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile();

                if (file != null)
                {
                    var firebaseStorage = new FirebaseStorage("gs://wassilni-app.appspot.com");
                    var imageUrl = await firebaseStorage.Child("PhotoUrl")
                        .Child($"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}")
                        .PutAsync(file.GetStream());

                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));

                    var user = await firebaseClient.Child("User").Child(userId).OnceSingleAsync<Models.User>();
                    user.PhotoUrl = imageUrl;
                    await firebaseClient.Child("User").Child(userId).PutAsync(user);

                    ((EditProfileViewModel)BindingContext).personalPhoto = ImageSource.FromUri(new Uri(imageUrl)).ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}

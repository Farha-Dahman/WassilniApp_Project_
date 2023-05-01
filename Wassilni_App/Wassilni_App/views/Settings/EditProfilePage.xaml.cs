﻿using System;

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
using Plugin.Media;
using Plugin.Media.Abstractions;
using User = Firebase.Auth.User;
using System.Drawing.Imaging;

namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        MediaFile file;
        public ImageSource profileImage;
        EditProfileViewModel editProfileviewModel = new EditProfileViewModel();
        public EditProfilePage()
        {
            InitializeComponent();
            this.BindingContext = new EditProfileViewModel();
        }
        string userId = Preferences.Get("userId", string.Empty);
        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
        FirebaseStorage firebaseStorage = new FirebaseStorage("//wassilni-app.appspot.com");
        private async void IMageTap_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file != null)
                {
                    return;
                }
                UserImage.Source = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                });
                var user = await firebaseClient.Child("User").Child(userId).OnceSingleAsync<Models.User>();

                if (true)
                {
                    var stream = file.GetStream();
                    var imageUri = await firebaseStorage.Child("profileImages").Child(userId).Child("profile.jpg").PutAsync(stream);
                    string imageUrl = await firebaseStorage.Child("profileImages").Child(userId).Child("profile.jpg").GetDownloadUrlAsync();
                    var databaseReference = firebaseClient.Child("User").Child(userId).Child("PhotoUrl");
                    await databaseReference.PutAsync(imageUrl);
                    user.PhotoUrl = imageUrl;
                }
                else
                {
                    await DisplayAlert("Error", "Update Photo Failed", "Cancel");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error selecting file: {ex.Message}");
            }

        }
        private async void ConfirmChanges_Clicked(object sender, EventArgs e)
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
                UserImage.Source = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                });

                if (true)
                {
                    var stream = file.GetStream();

                    string imageUrl = await firebaseStorage.Child("profileImages").Child(userId).Child("profile.jpg").GetDownloadUrlAsync();
                    var databaseReference = firebaseClient.Child("User").Child(userId).Child("PhotoUrl");
                    await databaseReference.PutAsync(imageUrl);
                    user.PhotoUrl = imageUrl;
                }
                else
                {
                    await DisplayAlert("Error", "Update Photo Failed", "Cancel");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error selecting file: {ex.Message}");
            }
        }
    }
}
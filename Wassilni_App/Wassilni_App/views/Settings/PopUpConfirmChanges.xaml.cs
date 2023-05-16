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
        private async void Ok_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.viewModels;
using Wassilni_App.views.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(String UserId)
        {
            InitializeComponent();
            this.BindingContext = new ProfileViewModel(UserId);
            Settings.Clicked += GoToSettingsPage;
        }
        async private void GoToSettingsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());

        }
    }
}
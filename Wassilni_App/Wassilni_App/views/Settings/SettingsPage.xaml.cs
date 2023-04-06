using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Wassilni_App.Models;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Wassilni_App.views;
using Android.Content.Res;

namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public SettingsPage()
        {
            InitializeComponent();
            this.BindingContext = new SettingsViewModel();
        }

        async private void GoToEditProfilePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfilePage());

        }
        async private void GoToFAQsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FAQsPage());

        }
        async private void GoToContactUsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactUsPage());

        }
        async private void GoToPrivacy_TermsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Privacy_TermsPage());

        }
        async private void GoToChangePassPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }

        async private void btnLogout_Clicked(object sender, EventArgs e) 
        { 
            await Navigation.PushAsync(new LoginPage());
        }


    }
}
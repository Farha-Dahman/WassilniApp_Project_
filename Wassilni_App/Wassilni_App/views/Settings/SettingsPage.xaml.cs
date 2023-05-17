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
namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public SettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetTitleView(this, new Label { Text = "Settings", HorizontalOptions = LayoutOptions.StartAndExpand });

            this.BindingContext = new SettingsViewModel();
            NavigationPage.SetTitleView(this, new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
            {
                new Label
                {
                    Text = "Settings",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    TextColor = Color.White,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                }
            }
            });

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
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

        async private void BtnLogout_Clicked(object sender, EventArgs e) 
        { 
            await Navigation.PushAsync(new LoginPage());
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
            this.BindingContext = new SignupViewModel();
            // BuSignup.Clicked += GoToVerificationPage;
        }
        async private void GoToVerificationPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerificationCodePage());
            //await Navigation.PushModalAsync(new NavigationPage(new VerificationCodePage()));
        }
        async private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
            //await Navigation.PushModalAsync(new NavigationPage(new VerificationCodePage()));
        }
        async private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
            //await Navigation.PushModalAsync(new NavigationPage(new VerificationCodePage()));
        }
        Dictionary<string, string> countryPhonePrefixes = new Dictionary<string, string>
        {
            { "United States", "+1" },
            { "Canada", "+2" },
            { "Mexico", "+52" },
            { "Palestine", "+970" },
            { "occupied Palestinian territories", "+972" },
            { "Jordan","962+" }
        };
        void OnCountryPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCountry = (string)countryPicker.SelectedItem;
            string phonePrefix = countryPhonePrefixes[selectedCountry];
            phoneCodeLabel.Text = phonePrefix;
        }

    }
}
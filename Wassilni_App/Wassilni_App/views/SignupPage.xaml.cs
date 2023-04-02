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
        }

        async private void GoToVerificationPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerificationCodePage());
            //await Navigation.PushModalAsync(new NavigationPage(new VerificationCodePage()));

        }
    }
}
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{ 
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ResetPasswordPopUp()
        {
            InitializeComponent();
        }
        async private void Ok_Clicked(object sender, EventArgs e)
        {
            // go to the login page
            await Navigation.PushAsync(new LoginPage());
            //close the pop up page
            await PopupNavigation.Instance.PopAsync();

        }
    }
}
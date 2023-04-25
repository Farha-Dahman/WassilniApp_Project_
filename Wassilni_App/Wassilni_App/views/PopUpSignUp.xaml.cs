using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Wassilni_App.viewModels;
using Wassilni_App.views.Settings;
using Wassilni_App.views;
using Rg.Plugins.Popup.Services;
namespace Wassilni_App
{
   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpSignUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopUpSignUp()

        {

            InitializeComponent();
            
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {

            try
            {
                // Close the popup page
                await PopupNavigation.Instance.PopAsync();

                // Go to the login page
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


        }
    }
}
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
    public partial class PopUpCreatePool : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopUpCreatePool()

        {

            InitializeComponent();

        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            // go to the login page
            await Navigation.PushAsync(new MyTripsPage());
            await PopupNavigation.Instance.PopAsync();
            // Close the popup page


        }
    }
}
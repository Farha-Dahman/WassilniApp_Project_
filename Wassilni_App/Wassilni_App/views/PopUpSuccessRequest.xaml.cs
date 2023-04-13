using Android.Content.Res;
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
    public partial class PopUpSuccessRequest : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopUpSuccessRequest()

        {

            InitializeComponent();

        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
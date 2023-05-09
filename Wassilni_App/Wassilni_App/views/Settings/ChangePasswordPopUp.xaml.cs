
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ChangePasswordPopUp()
        {
            InitializeComponent();
        }

        private async void Ok_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.viewModels;
using Wassilni_App.views.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            this.BindingContext = new HomeViewModel();
        }

        async private void GoToCreatePoolPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new CreatePoolPage()));

        }
        async private void GoToFindPoolPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new FindPoolPage()));

        }
    }
}
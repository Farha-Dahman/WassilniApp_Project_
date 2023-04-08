using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Wassilni_App.viewModels;
using Wassilni_App.views.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        public HomePage(string userid)
        {
            string id = userid;
            InitializeComponent();
            this.BindingContext = new HomeViewModel();
            createPool.Clicked += (sender, e) => CreatePoolButton_Clicked(sender, e, id);
        }


        async private void GoToCreatePoolPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new CreatePoolPage()));


        }
        async private void GoToFindPoolPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindPoolPage()));
            
        }
        async private void CreatePoolButton_Clicked(object sender, EventArgs e, string id)
        {


            await Navigation.PushModalAsync(new CreatePoolPage(id));

        }
    }
}
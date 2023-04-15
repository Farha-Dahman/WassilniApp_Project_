using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FAQsPage : ContentPage
    {
        public FAQsPage()
        {
            InitializeComponent();
            this.BindingContext = new FAQsViewModel();
        }
        private void OnSourceFrameTapped(object sender, EventArgs e)
        {
            Answer_one.IsVisible = !Answer_one.IsVisible;
        }
    }
}
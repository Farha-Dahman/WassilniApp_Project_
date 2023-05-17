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
    public partial class ContactUsPage : ContentPage
    {
        public ContactUsPage()
        {
            InitializeComponent();
            this.BindingContext = new ContactUsViewModel();

        }
    }
}
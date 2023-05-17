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
    public partial class VerificationCodePage : ContentPage
    {
        public VerificationCodePage()
        {
            InitializeComponent();
            this.BindingContext = new VerificationCodeViewModel();
        }
    }
}
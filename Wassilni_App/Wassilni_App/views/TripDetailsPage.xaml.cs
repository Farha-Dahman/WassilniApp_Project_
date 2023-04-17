using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripDetailsPage : ContentPage
    {

        public TripDetailsPage(string rideId)
        {
            InitializeComponent();
            this.BindingContext = new TripDetailsViewModel(rideId);
        }

    }
}
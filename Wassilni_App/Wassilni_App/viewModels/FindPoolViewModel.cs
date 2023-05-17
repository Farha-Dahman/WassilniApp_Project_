using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.views;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{
    public class FindPoolViewModel: BaseViewModel
    {
        public ICommand OnItemTapped { get; private set; }


        public ObservableCollection<RideViewModel> MatchingPools { get; set; }
        public FindPoolViewModel(List<Ride> matchingRides)
        {
            MatchingPools = new ObservableCollection<RideViewModel>(matchingRides.Select(r => new RideViewModel(r)));

        }


    }
}

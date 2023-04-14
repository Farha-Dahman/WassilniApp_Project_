using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wassilni_App.Models;

namespace Wassilni_App.viewModels
{
    public class FindPoolViewModel
    {
        public ObservableCollection<RideViewModel> MatchingPools { get; set; }
        public FindPoolViewModel(List<Ride> matchingRides)
        {
            MatchingPools = new ObservableCollection<RideViewModel>(matchingRides.Select(r => new RideViewModel(r)));
        }

    }
}

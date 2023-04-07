using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{
    public class CreatePoolViewModel : BaseViewModel
    {
        private string _locationFrom;
        private string _locationTo;
        private string _EmptyErrorMessage;

        public string EmptyErrorMessage
        {
            get { return _EmptyErrorMessage; }
            set { SetProperty(ref _EmptyErrorMessage, value); }
        }
        public string LocationFrom
        {
            get { return _locationFrom; }
            set { SetProperty(ref _locationFrom, value); }
        }
        public string LocationTo
        {
            get { return _locationTo; }
            set { SetProperty(ref _locationTo, value); }
        }
      
    }
}

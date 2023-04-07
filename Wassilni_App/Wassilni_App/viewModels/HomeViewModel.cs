using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{
    public class HomeViewModel : BaseViewModel
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

        public ICommand CreatePoolCommand { get; set; }
        public ICommand FindPoolCommand { get; set; }

        public HomeViewModel()
        {
            CreatePoolCommand = new Command(async () => await ExecuteCreatePoolCommand());
            FindPoolCommand = new Command(async () => await ExecuteFindPoolCommand());

        }

        private async Task ExecuteCreatePoolCommand()
        {
            try
            {
                if (string.IsNullOrEmpty(LocationFrom) || (string.IsNullOrEmpty(LocationTo)))
                {
                    EmptyErrorMessage = "Please don't leave any fields Empty";

                }
                else
                {
                    EmptyErrorMessage = "";
                }
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
            }
        }

        private async Task ExecuteFindPoolCommand()
        {
            try
            {
                if (string.IsNullOrEmpty(LocationFrom) || (string.IsNullOrEmpty(LocationTo)))
                {
                    EmptyErrorMessage = "Please don't leave any fields Empty";

                }
                else
                {
                    EmptyErrorMessage = "";
                }
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
            }
        }
    }


}


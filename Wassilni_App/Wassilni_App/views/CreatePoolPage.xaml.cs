using Android.Text.Format;
using Firebase.Auth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePoolPage : ContentPage
    {
        private readonly string _userId;
        private List<Frame> errorFrames;
        private bool isErrorMessageVisible = false;


        public CreatePoolPage(string id)
        {
            InitializeComponent();
            _userId = id;

            this.BindingContext = new CreatePoolViewModel(_userId);

            var viewModel = BindingContext as CreatePoolViewModel;
            viewModel.ShowTopErrorMessage += ViewModel_ShowTopErrorMessage;
        }
        private async void ViewModel_ShowTopErrorMessage(object sender, EventArgs e)
        {
            TopErrorFrame.Opacity = 0;
            TopErrorFrame.TranslationY = -30;
            TopErrorFrame.IsVisible = true;

            uint duration = 500;
            uint shakeDuration = 50;
            int shakeCount = 5;
            double shakeOffset = 5;

            await Task.WhenAll(
                TopErrorFrame.FadeTo(1, duration, Easing.CubicInOut),
                TopErrorFrame.TranslateTo(0, 0, duration, Easing.CubicInOut)
            );


            for (int i = 0; i < shakeCount; i++)
            {
                await Task.WhenAll(
                    TopErrorFrame.TranslateTo(shakeOffset, 0, shakeDuration, Easing.Linear),
                    TopErrorFrame.TranslateTo(-shakeOffset, 0, shakeDuration, Easing.Linear)
                );
            }
            await TopErrorFrame.TranslateTo(0, 0, shakeDuration, Easing.Linear);


            await Task.Delay(3000);


            await Task.WhenAll(
                TopErrorFrame.FadeTo(0, duration, Easing.CubicInOut),
                TopErrorFrame.TranslateTo(0, -30, duration, Easing.CubicInOut)
            );
            TopErrorFrame.IsVisible = false;
        }




    }
}
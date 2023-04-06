using System;
using Wassilni_App.views;
using Wassilni_App.views.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SplashScreen());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

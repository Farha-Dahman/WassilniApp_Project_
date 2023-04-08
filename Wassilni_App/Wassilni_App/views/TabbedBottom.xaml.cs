using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedBottom : TabbedPage
    {

        public TabbedBottom()
        {
            InitializeComponent();
        }

            public TabbedBottom(string userId)
        {
            InitializeComponent();

            var homePage = new HomePage(userId)
            {
                Title = "Home",
                IconImageSource = ImageSource.FromFile("Home.png")
            };
            Children.Add(homePage);


            var MyTripsPage = new MyTripsPage()
            {
                Title = "MyTrips",
                IconImageSource = ImageSource.FromFile("Car.png")
            };
            Children.Add(MyTripsPage);

            var RequestsPage = new RequestsPage()
            {
                Title = "Requests",
                IconImageSource = ImageSource.FromFile("Request.png")
            };
            Children.Add(RequestsPage);

            var profilePage = new ProfilePage()
            {
                Title = "Profile",
                IconImageSource = ImageSource.FromFile("Profile.png")
            };
            Children.Add(profilePage);



        }
    }
}
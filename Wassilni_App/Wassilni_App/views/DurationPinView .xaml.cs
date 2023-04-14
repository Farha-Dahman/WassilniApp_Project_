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
    public partial class DurationPinView : ContentView
    {
        public DurationPinView(TimeSpan duration)
        {
            Content = new Frame
            {
                BackgroundColor = Color.White,
                Content = new Label
                {
                    Text = $"Estimated travel time: {duration}",
                    FontSize = 14,
                    TextColor = Color.Black,
                },
                Padding = 5,
                HasShadow = true,
                CornerRadius = 5,
            };
        }
    }
}
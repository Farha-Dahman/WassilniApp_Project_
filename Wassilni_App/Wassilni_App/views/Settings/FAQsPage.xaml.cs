using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FAQsPage : ContentPage
    {
        public FAQsPage()
        {
            InitializeComponent();
            this.BindingContext = new FAQsViewModel();
        }
        private void OnSourceFrameTapped(object sender, EventArgs e)
        {
            Answer_one.IsVisible = !Answer_one.IsVisible;
            arrow_1.Text = Answer_one.IsVisible ? "\uf077" : "\uf078";
        }
        private void OnSourceFrameTapped2(object sender, EventArgs e)
        {
            Answer_two.IsVisible = !Answer_two.IsVisible;
            arrow_2.Text = Answer_two.IsVisible ? "\uf077" : "\uf078";
        }
        private void OnSourceFrameTapped3(object sender, EventArgs e)
        {
            Answer_three.IsVisible = !Answer_three.IsVisible;
            arrow_3.Text = Answer_three.IsVisible ? "\uf077" : "\uf078";
        }
        private void OnSourceFrameTapped4(object sender, EventArgs e)
        {
            Answer_four.IsVisible = !Answer_four.IsVisible;
            arrow_4.Text = Answer_four.IsVisible ? "\uf077" : "\uf078";
        }
        private void OnSourceFrameTapped5(object sender, EventArgs e)
        {
            Answer_five.IsVisible = !Answer_five.IsVisible;
            arrow_5.Text = Answer_five.IsVisible ? "\uf077" : "\uf078";
        }
        private void OnSourceFrameTapped6(object sender, EventArgs e)
        {
            Answer_six.IsVisible = !Answer_six.IsVisible;
            arrow_6.Text = Answer_six.IsVisible ? "\uf077" : "\uf078";
        }



    }
}
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
    public partial class Privacy_TermsPage : ContentPage
    {
        public Privacy_TermsPage()
        {
            InitializeComponent();
            this.BindingContext = new Privacy_TermsViewModel();
        }
        private void OnSubjectArrowTapped(object sender, EventArgs e)
        {
            SubjectLayout.IsVisible = !SubjectLayout.IsVisible;
            DefinitionLayout.IsVisible = false;
        }

        private void OnDefinitionArrowTapped(object sender, EventArgs e)
        {
            DefinitionLayout.IsVisible = !DefinitionLayout.IsVisible;
            SubjectLayout.IsVisible = false;
        }
    }
}
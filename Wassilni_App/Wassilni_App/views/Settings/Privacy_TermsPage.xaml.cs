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
            DefinitionRidesharing.IsVisible = false;
            DefinitionHowWeUseYourInformation.IsVisible = false;
            DefinitionSharingOfInformation.IsVisible = false;
            DefinitionTermsOfService.IsVisible = false;
        }

        private void OnDefinitionArrowTapped(object sender, EventArgs e)
        {
            DefinitionLayout.IsVisible = !DefinitionLayout.IsVisible;
            SubjectLayout.IsVisible = false;
            DefinitionRidesharing.IsVisible = false;
            DefinitionHowWeUseYourInformation.IsVisible = false;
            DefinitionSharingOfInformation.IsVisible = false;
            DefinitionTermsOfService.IsVisible = false;
        }
        private void OnHowWeUseYourInformationTapped(object sender, EventArgs e)
        {
            DefinitionHowWeUseYourInformation.IsVisible = !DefinitionHowWeUseYourInformation.IsVisible;
            SubjectLayout.IsVisible = false;
            DefinitionLayout.IsVisible = false;
            DefinitionRidesharing.IsVisible = false;
            DefinitionSharingOfInformation.IsVisible = false;
            DefinitionTermsOfService.IsVisible = false;
        }
        private void OnSharingOfInformationTapped(object sender, EventArgs e)
        {
            DefinitionSharingOfInformation.IsVisible = !DefinitionSharingOfInformation.IsVisible;
            SubjectLayout.IsVisible = false;
            DefinitionLayout.IsVisible = false;
            DefinitionHowWeUseYourInformation.IsVisible = false;
            DefinitionRidesharing.IsVisible = false;
            DefinitionTermsOfService.IsVisible = false;
        }
        private void OnTermsOfServiceTapped(object sender, EventArgs e)
        {
            DefinitionTermsOfService.IsVisible = !DefinitionTermsOfService.IsVisible;
            SubjectLayout.IsVisible = false;
            DefinitionLayout.IsVisible = false;
            DefinitionHowWeUseYourInformation.IsVisible = false;
            DefinitionSharingOfInformation.IsVisible = false;
            DefinitionRidesharing.IsVisible = false;

        }
        private void OnRidesharingTapped(object sender, EventArgs e)
        {
            DefinitionRidesharing.IsVisible = !DefinitionRidesharing.IsVisible;
            SubjectLayout.IsVisible = false;
            DefinitionLayout.IsVisible = false;
            DefinitionHowWeUseYourInformation.IsVisible = false;
            DefinitionSharingOfInformation.IsVisible = false;
            DefinitionTermsOfService.IsVisible = false;
        }
    }
}
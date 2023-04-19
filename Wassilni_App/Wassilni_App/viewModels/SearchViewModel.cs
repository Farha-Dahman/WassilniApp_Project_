using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Wassilni_App.views;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseClient firebaseClient;
        private List<Models.User> allUsers;
        private List<Models.User> filteredUsers;
        private string searchTerm;


        public event PropertyChangedEventHandler PropertyChanged;
        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                searchTerm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchTerm)));
                SearchUsers();
            }
        }

        public SearchViewModel()
        {
            firebaseClient = new FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
            allUsers = new List<Models.User>();
            filteredUsers = new List<Models.User>();
            searchTerm = "";
            SearchUsersCommand = new Command(SearchUsers);
            LoadUsers();
            ViewProfileCommand = new Command<Models.User>(ExecuteViewProfileCommand);
        }
        private async void NavigateToUserProfile(Models.User user)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewProfilePage(user.Email));
        }
        public List<Models.User> Users => filteredUsers;

        public ICommand SearchUsersCommand { get; }
        public ICommand ViewProfileCommand { get; }

        private async void LoadUsers()
        {
            var users = await firebaseClient.Child("User").OnceAsync<Models.User>();
            allUsers = users.Select(u => u.Object).ToList();
        }
        private void SearchUsers()
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredUsers.Clear();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Users)));
                return;
            }
            filteredUsers = allUsers.Where(user => user.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
             (user.FirstName.ToLower() + " " + user.LastName.ToLower()).Contains(searchTerm.ToLower())
    ).ToList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Users)));
        }


        private async void ExecuteViewProfileCommand(Models.User user)
        {
            var userRef = firebaseClient.Child("User").OrderBy("Email").EqualTo(user.Email);
            var users = await userRef.OnceAsync<Models.User>();
            var selectedUser = users.Select(u => u.Object).FirstOrDefault();

            if (selectedUser != null)
            {
                // Navigate to the user profile page using the user's key
                await Application.Current.MainPage.Navigation.PushAsync(new ViewProfilePage(selectedUser.Email));
            }
        }
    }
}
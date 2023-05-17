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

        public List<Models.User> Users => filteredUsers;

        public ICommand SearchUsersCommand { get; }
        public ICommand ViewProfileCommand { get; }


        public event PropertyChangedEventHandler PropertyChanged;
        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
                SearchUsers();
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
       
        private async void LoadUsers()
        {
            var users = await firebaseClient.Child("User").OnceAsync<Models.User>();
            allUsers = users.Select(u => u.Object).ToList();
        }
        private void SearchUsers()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    filteredUsers.Clear();
                }
                else
                {
                    filteredUsers = allUsers.Where(user =>
                      (user.FullName != null && user.FullName.ToLower().Contains(searchTerm.ToLower())) ||
                      (user.FirstName != null && user.FirstName.ToLower().Contains(searchTerm.ToLower())) ||
                      (user.LastName != null && user.LastName.ToLower().Contains(searchTerm.ToLower())) ||
                      (user.FirstName != null && user.LastName != null && (user.FirstName.ToLower() + " " + user.LastName.ToLower()).Contains(searchTerm.ToLower()))
                         ).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in SearchUsers: {0}", ex);
            }
            finally
            {
                OnPropertyChanged(nameof(Users));
            }
            if (string.IsNullOrWhiteSpace(searchTerm) && filteredUsers.Count == 0)
            {
                filteredUsers = new List<Models.User>(); // Create an empty list
                OnPropertyChanged(nameof(Users));
            }
        }


        private async void NavigateToUserProfile(Models.User user)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewProfilePage(user.Email));
        }
        private async void ExecuteViewProfileCommand(Models.User user)
        {
            var users = await firebaseClient.Child("User").OrderBy("Email").EqualTo(user.Email).OnceAsync<Models.User>();
            var selectedUser = users.Select(u => u.Object).FirstOrDefault();

            if (selectedUser != null)
            {
                // Navigate to the user profile page using the user's key
                NavigateToUserProfile(selectedUser);
            }
        }
    }
}
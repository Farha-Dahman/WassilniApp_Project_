using System;
using System.Collections.Generic;
using System.Text;

namespace Wassilni_App.Models
{
    public class User
    {
        public string FullName { get; set; }
        private string _email;
        private string _password;
        private string _firstName;
        private string _lastName;
        private string _birthdate;
        private string _selectedGender;
        private string phoneNumber;
        private string photoUrl;

        public string FullName { get; set; }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

       
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }

        public string SelectedGender
        {
            get { return _selectedGender; }
            set { _selectedGender = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string PhotoUrl
        {
            get { return photoUrl; }
            set { photoUrl = value; }
        }
        public string FCMToken { get; set; }

    }
}

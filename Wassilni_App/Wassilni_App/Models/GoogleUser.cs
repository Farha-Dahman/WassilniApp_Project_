using System;
using System.Collections.Generic;
using System.Text;

namespace Wassilni_App.Models
{
    public class GoogleUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Uri Picture { get; set; }
        public string UserId { get; set; }
        public string PhotoUrl { get; set; }
        public string FirebaseKey { get; set; }
        //public string FirebaseUserId { get; set; }
    }
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);

        void Logout();
    }
}

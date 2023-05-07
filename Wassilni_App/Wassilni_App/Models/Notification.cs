using System;
using System.Collections.Generic;
using System.Text;

namespace Wassilni_App.Models
{
    public class Notification
    {
        public bool IsNew;
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public string PhotoUrl { get; set; }
    
    }
}

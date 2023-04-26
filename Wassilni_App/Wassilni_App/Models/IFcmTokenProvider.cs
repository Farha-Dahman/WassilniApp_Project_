using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wassilni_App.Models
{
    public interface IFcmTokenProvider
    {
        Task<string> GetFcmTokenAsync();
    }
}

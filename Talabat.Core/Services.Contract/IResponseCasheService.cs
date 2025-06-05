using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Contract
{
    public interface IResponseCasheService
    {
        // first one to set the cashe
        Task SetResponseCasheAsync(string key, object value, TimeSpan timeToLive);
        // second one to get the cashe
        Task<string?> GetResponseCasheAsync(string key);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IcasheService
    {
        Task SetCasheValueAsync(string Key, object value, TimeSpan duration);

        Task<string?> GetCasheValueAsync(string Key);

    }
}

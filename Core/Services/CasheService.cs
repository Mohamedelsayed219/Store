using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Services.Abstractions;

namespace Services
{
    public class CasheService(ICasheRepository casheRepository) : IcasheService
    {
        public async Task<string?> GetCasheValueAsync(string Key)
        {
           var value = await casheRepository.GetAsync(Key);
            return value == null ? null : value;
        }

        public async Task SetCasheValueAsync(string Key, object value, TimeSpan duration)
        {
            await casheRepository.SetAsync(Key, value, duration);
        }
    }
}

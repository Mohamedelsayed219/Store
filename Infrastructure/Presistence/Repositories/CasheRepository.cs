using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using StackExchange.Redis;

namespace Presistence.Repositories
{
    public class CasheRepository(IConnectionMultiplexer connection) : ICasheRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public async Task<string?> GetAsync(string Key)
        {
            var value = await _database.StringGetAsync(Key);

            return !value.IsNullOrEmpty ? value : default;

        }

        public async Task SetAsync(string Key, object value, TimeSpan duration)
        {
            var redisValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(Key, redisValue, duration);


        }
    }
}

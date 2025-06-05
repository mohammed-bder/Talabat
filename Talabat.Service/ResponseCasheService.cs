using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class ResponseCasheService : IResponseCasheService
    {
        private readonly IDatabase _database;
        public ResponseCasheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task SetResponseCasheAsync(string key, object value, TimeSpan timeToLive)
        {
            if (value is null) return;

            var serializeOption = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedResponse = JsonSerializer.Serialize(value, serializeOption);

            await _database.StringSetAsync(key, serializedResponse, timeToLive);

        }
        public async Task<string?> GetResponseCasheAsync(string key)
        {
            var cashedResponse = await _database.StringGetAsync(key);

            if (cashedResponse.IsNullOrEmpty) return null;
            return cashedResponse;
        }

    }
}

using Domain.General.Interfaces.External;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.General.External
{
    /// <summary>Implementación del servicio de caché utilizando Redis.</summary>
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;

        /// <summary>Inicializa una nueva instancia del servicio CacheService.</summary>
        /// <param name="connectionMultiplexer">Conexión activa a Redis.</param>
        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        /// <inheritdoc />
        public async Task<T?> GetAsync<T>(string key)
        {
            RedisValue value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!);
        }

        /// <inheritdoc />
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, expiration);
        }

        /// <inheritdoc />
        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}

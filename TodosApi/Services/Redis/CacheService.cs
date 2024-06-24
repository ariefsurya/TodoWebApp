
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json;

namespace TodosApi.Services.Redis
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value);
        bool RemoveData(string key);
    }

    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;

        public CacheService(IConnectionMultiplexer redis) {
            _cacheDb = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            //if (!string.IsNullOrEmpty(value))
            //    return JsonSerializer.Deserialize<T>(value);
            if (!string.IsNullOrEmpty(value))
                return JsonConvert.DeserializeObject<T>(value);
            return default;
        }

        public bool RemoveData(string key)
        {
            var _exists = _cacheDb.KeyExists(key);
            if (_exists)
                return _cacheDb.KeyDelete(key);
            return false;
        }

        public bool SetData<T>(string key, T value)
        {
            TimeSpan expiryTime = TimeSpan.FromDays(7);
            var isSet = _cacheDb.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
    }
}

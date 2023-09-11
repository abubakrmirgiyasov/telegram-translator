using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WebHook.Translator.Infrastructure.Redis.Interfaces;

namespace WebHook.Translator.Infrastructure.Redis;

public class RedisCacheManager : IDistributedCacheManager
{
    private readonly RedisClient _redisClient;

    public RedisCacheManager(RedisClient redisClient)
    {
        _redisClient = redisClient;
    }

    public byte[]? Get(string key)
    {
        try
        {
            byte[]? items = _redisClient.Cache.Get(key)
                ?? throw new ArgumentNullException(nameof(RedisClient), $"Redis client does not exists item with key: {key}");

            return _redisClient.Cache.Get(key);
        }
        catch (ArgumentNullException ex)
        {
            throw new ArgumentNullException(nameof(RedisClient), ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public T GetT<T>(string key)
    {
        throw new NotImplementedException();
    }

    public void Set<T>(string key, T value)
    {
        try
        {
            var serialized = JsonConvert.SerializeObject(value);
            _redisClient.Cache.SetString(key, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30),
                SlidingExpiration = TimeSpan.FromSeconds(30),
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public void Update(string key)
    {
        throw new NotImplementedException();
    }

    public void Delete(string key)
    {
        _redisClient.Cache.Remove(key);
    }
}

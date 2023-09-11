using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using WebHook.Translator.Common;

namespace WebHook.Translator.Infrastructure.Redis;

public class RedisClient
{
    private readonly RedisCache _cache;
    private readonly AppSettings _appSettings;

    public RedisClient(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;

        var redisOptions = new RedisCacheOptions()
        {
            ConfigurationOptions = new ConfigurationOptions()
            {
                EndPoints = { { _appSettings.RedisSettings.Host, _appSettings.RedisSettings.Port } },
            }
        };

        var options = Options.Create(redisOptions);
        _cache = new RedisCache(options);
    }

    public RedisCache Cache { get => _cache; }
}

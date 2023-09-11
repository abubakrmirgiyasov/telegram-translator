namespace WebHook.Translator.Infrastructure.Redis.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDistributedCacheManager
{
    /// <summary>
    /// Gets items from cache if exists else throw ArgumentNullException
    /// </summary>
    /// <param name="key"></param>
    /// <returns>byte[]?</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    byte[]? Get(string key);

    T GetT<T>(string key);

    void Set<T>(string key, T value);

    void Update(string key);

    void Delete(string key);
}

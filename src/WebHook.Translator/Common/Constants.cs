using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SharpCompress.Archives.GZip;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using WebHook.Translator.Services;

namespace WebHook.Translator.Common;

/// <summary>
/// Extensions Constants
/// </summary>
public static partial class Constants
{
    /// <summary>
    /// Delegate to configure commands
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="builder"></param>
    public delegate void ConfigureCommands(IServiceProvider serviceProvider, CommandManagerBuilder builder);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="multicastDelegate"></param>
    /// <param name="invocationFunctions"></param>
    /// <returns></returns>
    public static IEnumerable<Task>? InvokeAll<T>(this T multicastDelegate, Func<T, Task> invocationFunctions)
        where T : MulticastDelegate
    {
        return multicastDelegate?
            .GetInvocationList()
            .Cast<T>()
            .Select(invocationFunctions.Invoke)
            .ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configurationManager"></param>
    /// <param name="sectionPath"></param>
    /// <returns></returns>
    public static T GetRequiredConfigurationInstance<T>(this ConfigurationManager configurationManager, string sectionPath)
        where T : new()
    {
        var configurationSection = configurationManager.GetSection(sectionPath);
        var configurationInstance = new T();
        configurationSection.Bind(configurationInstance);
        return configurationInstance;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureCommands"></param>
    /// <returns></returns>
    public static IServiceCollection AddCommandManager(this IServiceCollection services, ConfigureCommands configureCommands)
    {
        services.AddScoped(serviceProvider =>
        {
            var scope = serviceProvider
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var builder = new CommandManagerBuilder(scope);
            configureCommands(scope.ServiceProvider, builder);
            return builder.Build();
        });
        return services;
    }
}

/// <summary>
/// Common Constants
/// </summary>
public static partial class Constants
{
    public const int KEYBOARD_COLUMNS = 3;
}

/// <summary>
/// 
/// </summary>
public static partial class Constants
{
    public static async Task<string?> AddImage(string? folder, IFormFile? file)
    {
        if (string.IsNullOrEmpty(folder) && file == null)
            return null;

        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder!);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        path = Path.Combine(path, file!.FileName);

        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return path;
    }

    public static byte[] GetSalt()
    {
        using var random = RandomNumberGenerator.Create();
        byte[] salt = new byte[128 / 8];
        random.GetBytes(salt);
        return salt;
    }

    public static string GetHash(string password, byte[] salt)
    {
        return Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
    }
}

public enum MarkupType : byte
{
    Language = 0,
    Game = 1,
    Poll = 2,
    Image = 3,
}

public enum GameType : byte
{
    Test = 1,
    Image = 2,
}

using System.Runtime.CompilerServices;
using WebHook.Translator.Services;

namespace WebHook.Translator.Common;

public static partial class Constants
{
    public delegate void ConfigureCommands(IServiceProvider serviceProvider, CommandManagerBuilder builder);

    public static IEnumerable<Task>? InvokeAll<T>(this T multicastDelegate, Func<T, Task> invocationFunctions)
        where T : MulticastDelegate
    {
        return multicastDelegate?
            .GetInvocationList()
            .Cast<T>()
            .Select(invocationFunctions.Invoke)
            .ToArray();
    }

    public static T GetRequiredConfigurationInstance<T>(this ConfigurationManager configurationManager, string sectionPath)
        where T : new()
    {
        var configurationSection = configurationManager.GetSection(sectionPath);
        var configurationInstance = new T();
        configurationSection.Bind(configurationInstance);
        return configurationInstance;
    }

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

public static partial class Constants
{
    public const int KEYBOARD_COLUMNS = 3;
}

public enum KeyboardDirection : byte
{
    Source = 0,
    Target = 1,
}

public enum MarkupType : byte
{
    Language = 0,
    Game = 1,
}

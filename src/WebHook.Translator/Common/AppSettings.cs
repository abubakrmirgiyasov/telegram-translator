#nullable disable

namespace WebHook.Translator.Common;

public class AppSettings
{
    public MongoDbSettings MongoDbSettings { get; set; }

    public TelegramSettings TelegramSettings { get; set; }

    public ChatGPTSettings ChatGPTSettings { get; set; }
}

public class ChatGPTSettings
{
    public string Key { get; set; }

    public int MaxTokens { get; set; }
}

public class MongoDbSettings
{
    public string DatabaseName { get; set; }

    public string ConnectionString { get; set; }
}

public class TelegramSettings : ConfigurationBase
{
    public static readonly string SectionPath = nameof(TelegramSettings);

    public string BotToken { get; set; }

    public string Route { get; set; }

    public string HostAddress { get; set; }

    public string SecretToken { get; set; }

    public override void Validate()
    {
        ValidationNotNull(SectionPath, nameof(BotToken), BotToken);
        ValidationNotNull(SectionPath, nameof(Route), Route);
        ValidationNotNull(SectionPath, nameof(HostAddress), HostAddress);
        ValidationNotNull(SectionPath, nameof(SecretToken), SecretToken);
    }
}

public abstract class ConfigurationBase
{
    public abstract void Validate();

    protected void ValidationNotNull<T>(string path, string name, T value)
    {
        bool isValid;

        if (value is string v)
            isValid = !string.IsNullOrWhiteSpace(v);
        else
            isValid = value is not null;

        if (!isValid)
            throw new InvalidOperationException(
                $"'[{path}:{name}]' configuration value is not provided");
    }
}
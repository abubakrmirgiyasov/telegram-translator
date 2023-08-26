using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Managers.Interfaces;

public interface ILanguageManager
{
    Task<IEnumerable<Language>> GetLanguagesAsync();

    Task<Language> GetLanguageByCodeAsync(string code);
}

using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Managers;

public class LanguageManager : ILanguageManager
{
    private readonly Language[] _supportedLanguages =
    {
        new()
        {
            Code = "en",
            Name = "English",
            Flag = "\ud83c\uddfa\ud83c\uddf8",
        },
        new()
        {
            Code = "ru",
            Name = "Русский",
            Flag = "\ud83c\uddf7\ud83c\uddfa",
        }
    };

    public Task<Language> GetLanguageByCodeAsync(string code)
    {
        return Task.FromResult(_supportedLanguages.Single(x => x.Code
            .ToLowerInvariant()
            .Equals(code.ToLowerInvariant())));
    }

    public Task<IEnumerable<Language>> GetLanguagesAsync()
    {
        return Task.FromResult<IEnumerable<Language>>(_supportedLanguages);
    }
}

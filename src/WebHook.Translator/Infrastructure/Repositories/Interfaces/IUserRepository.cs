using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetOrCreateUserAsync(long chatId);

    Task UpdateSourceLanguage(User user, string newLanguage);

    Task UpdateTargetLanguage(User user, string newLanguage);
}

using WebHook.Translator.Infrastructure.Repositories.Interfaces;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User> GetOrCreateUserAsync(long chatId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSourceLanguage(User user, string newLanguage)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTargetLanguage(User user, string newLanguage)
    {
        throw new NotImplementedException();
    }
}

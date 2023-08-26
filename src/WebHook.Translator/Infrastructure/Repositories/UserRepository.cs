using Microsoft.Extensions.Options;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class UserRepository : Repository<User>
{
    public UserRepository(IOptions<AppSettings> settings) 
        : base(settings) { }

    public async Task<User> GetOrCreateUserAsync(long chatId, string? language = null)
    {
        var user = FilterBy(x => x.ChatId == chatId).FirstOrDefault();
        if (user is null)
        {
            user = new User()
            {
                ChatId = chatId,
                SourceLanguage = language,
            };
            await InsertOneAsync(user);
        }
        return user;
    }

    public Task UpdateSourceLanguage(User user,  string language)
    {
        user.SourceLanguage = language;
        return Task.CompletedTask; ////
    }

    public Task UpdateTargetLanguage(User user, string language)
    {
        user.TargetLanguage = language;
        return Task.CompletedTask; ////
    }
}

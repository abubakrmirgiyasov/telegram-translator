using Microsoft.Extensions.Options;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class UserTestRepository : Repository<UserTest>
{
    public UserTestRepository(IOptions<AppSettings> settings) 
        : base(settings) { }

    public new void InsertOne(UserTest model)
    {
        _Collection.InsertOne(model);
    }

    public new async Task InsertOneAsync(UserTest model)
    {
        await _Collection.InsertOneAsync(model);
    }
}

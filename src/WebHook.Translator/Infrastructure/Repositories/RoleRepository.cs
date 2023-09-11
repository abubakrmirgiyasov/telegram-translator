using Microsoft.Extensions.Options;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>
{
    public RoleRepository(IOptions<AppSettings> settings) 
        : base(settings) { }
}

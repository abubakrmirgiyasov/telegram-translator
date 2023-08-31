using Microsoft.Extensions.Options;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class TestRepository : Repository<Test>
{
    public TestRepository(IOptions<AppSettings> settings) 
        : base(settings) { }

    public Task<TestViewModel> GetSingleRandomTestAsync(long chatId, CancellationToken cancellationToken = default)
    {
        var test = FilterBy(x => x.Question != "");

        return new Task<TestViewModel>()
        {

        };
    }
}

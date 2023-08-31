using Microsoft.Extensions.Options;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class TestRepository : Repository<Test>
{
    public TestRepository(IOptions<AppSettings> settings) 
        : base(settings) { }

    public async Task<TestViewModel> GetSingleRandomTestAsync(long chatId, CancellationToken cancellationToken = default)
    {
        var test = await Task.FromResult(FilterBy(x => x.Question != "").ToList());

        int random = new Random().Next(0, test.Count);

        return new TestViewModel()
        {
            OptionId = test[random].CorrectOption,
            Question = test[random].Question,
            
        };
    }
}

using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class TestRepository : Repository<Test>
{
    public TestRepository(IOptions<AppSettings> settings) 
        : base(settings) { }

    public async Task<List<TestViewModel>> GetSingleRandomTestAsync(ObjectId id)
    {
        var question = await Task.FromResult(FilterBy(x => x.Id != id).FirstOrDefault());
        var list = new List<TestViewModel>();

        for (int i = 0; i < question?.Options.Length; i++)
        {
            list.Add(new TestViewModel()
            {
                Code = question.Id.ToString(),
                Option = question.Options[i],
                Ico = "\U00002753",
                OptionId = i,
                Question = question.Question,
            });
        }

        return list;
    }
}

using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Managers;

public class TestManager : ITestManager
{
    private readonly TestRepository _testRepository;

    public TestManager(TestRepository testRepository)
    {
        _testRepository = testRepository;
    }

    public async Task<TestViewModel> GetTestByCodeAsync(string code)
    {
        var question = await _testRepository.FindOneByIdAsync(code);

        return new TestViewModel()
        {
            Code = code,
            Ico = "\U00002753",
            Option = question.Options[question.CorrectOption],
        };
    }

    public Task<IEnumerable<TestViewModel>> GetTestsAsync()
    {
        var questions = _testRepository.FilterBy(x => x.Question != "").ToList();
        var extractedModels = new List<TestViewModel>();

        for (int i = 0; i < questions.Count;)
        {
            for (int j = 0; j < questions[i].Options.Length; j++)
            {
                extractedModels.Add(new TestViewModel()
                {
                    Code = questions[i].Id.ToString(),
                    Option = questions[i].Options[j],
                    Question = questions[i].Question,
                    OptionId = j,
                    Ico = "\U00002753",
                });
            }
        }

        return Task.FromResult<IEnumerable<TestViewModel>>(extractedModels);
    }
}

using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Managers.Interfaces;

public interface ITestManager
{
    Task<IEnumerable<TestViewModel>> GetTestsAsync();

    Task<TestViewModel> GetTestByCodeAsync(string code);
}

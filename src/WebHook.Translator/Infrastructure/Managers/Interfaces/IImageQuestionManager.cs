using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Managers.Interfaces;

public interface IImageQuestionManager
{
    Task<IEnumerable<ImageQuestionViewModel>> GetImageQuestionsAsync();

    Task<ImageQuestionViewModel> GetImageQuestionByCodeAsync(string code);
}

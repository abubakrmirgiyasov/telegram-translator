using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Managers;

public class ImageQuestionManager : IImageQuestionManager
{
    private readonly ImageQuestionRepository _imageQuestionRepository;

    public ImageQuestionManager(ImageQuestionRepository imageQuestionRepository)
    {
        _imageQuestionRepository = imageQuestionRepository;
    }

    public async Task<ImageQuestionViewModel> GetImageQuestionByCodeAsync(string code)
    {
        try
        {
            var imageQuestion = await _imageQuestionRepository.FindOneByIdAsync(code);

            return new ImageQuestionViewModel()
            {
                Code = code,
                Ico = "\U00002753",
                Question = imageQuestion.Question,
                OptionId = imageQuestion.CorrectAnswer,
                Hint = imageQuestion.Hint,
                Option = imageQuestion.Options[0],
                Image = imageQuestion.ImagePath,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public Task<IEnumerable<ImageQuestionViewModel>> GetImageQuestionsAsync()
    {
        throw new NotImplementedException();
    }
}

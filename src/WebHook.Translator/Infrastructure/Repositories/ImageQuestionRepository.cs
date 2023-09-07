using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class ImageQuestionRepository : Repository<ImageQuestion>
{
    public ImageQuestionRepository(IOptions<AppSettings> settings)
        : base(settings) { }

    public async Task<List<ImageQuestionViewModel>> GetImageQuestionsAsync()
    {
        try
        {
            var image = await Task.FromResult(FilterBy(x => x.Question != "").FirstOrDefault());
            var list = new List<ImageQuestionViewModel>();

            for (int i = 0; i < image?.Options.Length; i++)
            {
                list.Add(new ImageQuestionViewModel()
                {
                    Code = image.Id.ToString(),
                    Ico = "\U00002753",
                    OptionId = image.CorrectAnswer,
                    Hint = image.Hint,
                    Image = image.ImagePath,
                    Option = image.Options[i],
                    Question = image.Question,
                });
            }
            return list;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task CreateImageQuestionAsync(ImageQuestionBindingModel model)
    {
        try
        {
            string path = await AddImage(model.ImageFolder, model.Image);

            var image = new ImageQuestion()
            {
                ImageFolder = model.ImageFolder,
                ImageSize = model.Image.Length,
                ImagePath = path,
                CorrectAnswer = model.CorrectAnswer,
                Hint = model.Hint,
                ImageTitle = model.Image.FileName,
                ImageType = model.Image.ContentType,
                Question = model.Question,
                Options = JsonConvert.DeserializeObject<string[]>(model.Options)!,
            };

            await InsertOneAsync(image);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    private static async Task<string> AddImage(string folder, IFormFile file)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        path = Path.Combine(path, file.FileName);

        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return path;
    }
}

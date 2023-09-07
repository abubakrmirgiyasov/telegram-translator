using WebHook.Translator.Common;
using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Models;

[BsonCollection("image_questions")]
public class ImageQuestion : Document
{
    public string ImagePath { get; set; } = null!;

    public string ImageTitle { get; set; } = null!;

    public string ImageType { get; set; } = null!;

    public string ImageFolder { get; set; } = null!;

    public long ImageSize { get; set; }

    public string Question { get; set; } = null!;

    public string[] Options { get; set; } = null!;

    public string? Hint { get; set; }

    public int CorrectAnswer { get; set; }
}

public class ImageQuestionViewModel : IBase
{
    public string Code { get; set; } = null!;

    public string Ico { get; set; } = null!;

    public string Question { get; set; } = null!;

    public string Option { get; set; } = null!;

    public string? Hint { get; set; }

    public int OptionId { get; set; }

    public string Image { get; set; } = null!;

    public override string ToString()
    {
        return $"{Ico} {Option}";
    }
}

public class ImageQuestionBindingModel
{
    public string Question { get; set; } = null!;

    public string Options { get; set; } = null!;

    public string? Hint { get; set; }

    public int CorrectAnswer { get; set; }

    public string ImageFolder { get; set; } = null!;

    public IFormFile Image { get; set; } = null!;
}

using WebHook.Translator.Common;
using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Models;

[BsonCollection("questions")]
public class Test : Document
{
    public string Question { get; set; } = null!;

    public string[] Options { get; set; } = null!;

    public int CorrectOption { get; set; }

    public string Hint { get; set; } = null!;
}

public class TestViewModel : IBase
{
    public string Code { get; set; } = null!;

    public string Ico { get; set; } = null!;

    public string Option { get; set; } = null!;

    public string Question { get; set; } = null!;

    public int OptionId { get; set; }

    public override string ToString()
    {
        return $"{Ico} {Option}";
    }
}

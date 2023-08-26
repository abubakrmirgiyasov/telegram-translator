using WebHook.Translator.Common;

namespace WebHook.Translator.Models;

[BsonCollection("questions")]
public class Test : Document
{
    public string Question { get; set; } = null!;

    public string[] Options { get; set; } = null!;

    public int CorrectOption { get; set; }
}

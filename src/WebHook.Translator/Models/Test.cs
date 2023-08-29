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

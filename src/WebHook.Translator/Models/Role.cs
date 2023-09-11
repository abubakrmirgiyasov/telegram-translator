using WebHook.Translator.Common;
using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Models;

[BsonCollection("roles")]
public class Role : Document
{
    public string Name { get; set; } = null!;

    public string Key { get; set; } = null!;
}

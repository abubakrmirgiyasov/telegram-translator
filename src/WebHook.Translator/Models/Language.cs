#nullable disable

using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Models;

public class Language : IBase
{
    public string Code { get; set; }

    public string Name { get; set; }

    public string Flag { get; set; }

    public override string ToString()
    {
        return $"{Name}{Flag}";
    }
}

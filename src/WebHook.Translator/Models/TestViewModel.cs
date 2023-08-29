#nullable disable

using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Models;

public class TestViewModel : IBase
{
    public string Code { get; set; }

    public string Ico { get; set; }

    public string Option { get; set; }

    public string Question { get; set; }

    public override string ToString()
    {
        return $"{Ico} {Option}";
    }
}

namespace WebHook.Translator.Models;

public class User
{
    public Guid Id { get; set; }

    public long ChatId { get; set; }

    public string? SourceLanguage { get; set; }

    public string? TargetLanguage { get; set; }

    public DateTime CreationDate { get; } = DateTime.Now;

    public DateTime? UpdateDate { get; set; }
}

using MongoDB.Bson;
using WebHook.Translator.Common;
using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Models;

[BsonCollection("users")]
public class User : Document
{
    public long ChatId { get; set; }

    public string? SourceLanguage { get; set; }

    public string? TargetLanguage { get; set; }

    public ObjectId[]? AnsweredQuestions { get; set; }

    public ObjectId[]? SessionQuestions { get; set; }
}

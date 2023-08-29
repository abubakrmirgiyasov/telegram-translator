using MongoDB.Bson;

namespace WebHook.Translator.Models.Interfaces;

public abstract class Document : IDocument
{
    public ObjectId Id { get; set; }

    public DateTimeOffset CreationDate { get; } = DateTimeOffset.Now;

    public DateTimeOffset? UpdateDate { get; set; }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebHook.Translator.Models;

public interface IBase
{
    string Code { get; set; }
}

public interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    ObjectId Id { get; set; }

    DateTimeOffset CreationDate { get; }

    DateTimeOffset? UpdateDate { get; set; }
}

public abstract class Document : IDocument
{
    public ObjectId Id { get; set; }

    public DateTimeOffset CreationDate { get; } = DateTimeOffset.Now;

    public DateTimeOffset? UpdateDate { get; set; }
}
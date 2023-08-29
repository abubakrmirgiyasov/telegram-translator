using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebHook.Translator.Models.Interfaces;

public interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    ObjectId Id { get; set; }

    DateTimeOffset CreationDate { get; }

    DateTimeOffset? UpdateDate { get; set; }
}

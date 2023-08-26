#nullable disable

using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext.Interfaces;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.DbContext;

public class Repository<TDocument> : IRepository<TDocument>
    where TDocument : IDocument
{
    protected readonly IMongoCollection<TDocument> _Collection;

    public Repository(IOptions<AppSettings> settings)
    {
        var db = new MongoClient(settings.Value.MongoDbSettings.ConnectionString).GetDatabase(settings.Value.MongoDbSettings.DatabaseName);
        _Collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
    }

    private protected static string GetCollectionName(Type type)
    {
        return ((BsonCollectionAttribute)type
            .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault())?.CollectionName;
    }

    public IQueryable<TDocument> AsQueryable()
    {
        return _Collection.AsQueryable();
    }
    
    public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _Collection
            .Find(filterExpression)
            .ToEnumerable();
    }

    public IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression, 
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        return _Collection
            .Find(filterExpression)
            .Project(projectionExpression)
            .ToEnumerable();
    }

    public void InsertOne(TDocument document)
    {
        _Collection.InsertOne(document);
    }

    public Task InsertOneAsync(TDocument document)
    {
        return _Collection.InsertOneAsync(document);
    }

    public void InsertMany(ICollection<TDocument> entities)
    {
        _Collection.InsertMany(entities);
    }

    public Task InsertManyAsync(ICollection<TDocument> entities)
    {
        return _Collection.InsertManyAsync(entities);
    }

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        _Collection.FindOneAndDelete(filterExpression);
    }

    public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _Collection.FindOneAndDeleteAsync(filterExpression);
    }

    public void DeleteById(string id)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, objectId);
        _Collection.FindOneAndDelete(filter);
    }

    public Task DeleteByIdAsync(string id)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, objectId);
        return _Collection.FindOneAndDeleteAsync(filter);
    }

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        _Collection.DeleteMany(filterExpression);
    }

    public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _Collection.DeleteManyAsync(filterExpression);
    }
}

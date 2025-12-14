using MongoDB.Driver;
using System.Linq.Expressions;
using static secretFriend.Api.Infrastructure.Persistence.MongoDb.MongoConstants;

namespace secretFriend.Api.Infrastructure.Persistence.MongoDb;

public abstract class MongoRepositoryBase<TEntity>(IMongoDatabase database, string collectionName) : IMongoRepository<TEntity>
{
    private const string PROPERTY_FIELD_ID = "Id";

    protected readonly IMongoCollection<TEntity> collection = database.GetCollection<TEntity>(collectionName);

    public async Task<TEntity?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(FIELD_ID, id);
        return await collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await collection.Find(predicate).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await collection.Find(predicate).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await collection.InsertOneAsync(entity, null, cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await collection.InsertManyAsync(entities, null, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var id = typeof(TEntity).GetProperty(PROPERTY_FIELD_ID)?.GetValue(entity) as string;

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new InvalidOperationException("Missing Id field for update operation");
        }

        var filter = Builders<TEntity>.Filter.Eq(FIELD_ID, id);
        await collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(FIELD_ID, id);
        await collection.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await collection.CountDocumentsAsync(predicate, null, cancellationToken);
    }
}
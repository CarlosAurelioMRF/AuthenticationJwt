using AuthenticationJwt.Core.Base.Interface;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AuthenticationJwt.Core.Base
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity<TEntity>, new()
    {
        #region Members
        protected readonly IMongoCollection<TEntity> _entity;
        #endregion

        #region Constructor
        public GenericRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _entity = database.GetCollection<TEntity>(BsonClassMap.LookupClassMap(typeof(TEntity)).Discriminator);
        }
        #endregion

        private IQueryable<TEntity> FindAll() => _entity.AsQueryable();

        public void Insert(TEntity entity) => _entity.InsertOne(entity);

        public void Delete(string id) => _entity.DeleteOne(c => c.Id == id);

        public IEnumerable<TEntity> GetAll() => _entity.Find(_ => true).ToList();

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate) => _entity.Find(predicate).ToList();

        public long Count(Expression<Func<TEntity, bool>> predicate) => FindAll().Where(predicate).Count();

        public TEntity GetById(string id) => _entity.Find(c => c.Id == id).FirstOrDefault();

        public void Update(TEntity entity) => _entity.ReplaceOne(m => m.Id == entity.Id, entity);

        public void Dispose()
        {
        }
    }
}

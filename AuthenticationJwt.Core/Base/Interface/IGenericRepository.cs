using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AuthenticationJwt.Core.Base.Interface
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : EntityBase<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        long Count(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(string id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(string id);
    }
}

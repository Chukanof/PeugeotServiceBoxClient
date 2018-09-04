using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Infrastructure.EntityBase;

namespace WpfClient.Infrastructure.DataAccess
{
    /// <summary>Реализация репозитория для EF</summary>
    public sealed class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public EfRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Select(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet;
            }

            return _dbSet.Where(predicate);
        }

        public IQueryable<TEntity> Query()
        {
            return _dbSet;
        }

        public TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            Attach(entity);
            _dbSet.Remove(entity);
        }

        public TEntity Update(TEntity entity)
        {
            Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        private void Attach(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
        }
    }
}
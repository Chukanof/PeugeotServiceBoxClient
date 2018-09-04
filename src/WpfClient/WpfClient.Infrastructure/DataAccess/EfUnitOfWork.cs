using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WpfClient.Infrastructure.EntityBase;

namespace WpfClient.Infrastructure.DataAccess
{
    /// <summary>
    /// Реализация UnitOfWork на базе Entity framework
    /// </summary>
    public sealed class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private bool _disposed = false;
        private DbContextTransaction _transaction;

        public EfUnitOfWork(DbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Возвращает репозиторий для работы с сущностями
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности, с которой работает репозиторий</typeparam>

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return new EfRepository<TEntity>(_dbContext);
        }

        /// <summary>
        /// Синхронно сохраняет изменения
        /// </summary>
        /// <exception cref="UnitOfWorkException">
        /// Возникает если не удалось по каким-либо причинам сохранить изменения
        /// </exception>

        public void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkException("SaveChanges failed. See innerException", ex);
            }
        }

        /// <summary>
        /// Асинхронно сохраняет изменения
        /// </summary>
        /// <exception cref="UnitOfWorkException">
        /// Возникает если не удалось по каким-либо причинам сохранить изменения
        /// </exception>
        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkException("SaveChangesAsync failed. See innerException", ex);
            }
        }

        /// <summary>
        /// Открывает транзакцию
        /// </summary>

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _dbContext.Database.BeginTransaction();
            }
        }

        /// <summary>
        /// Открывает транзакцию
        /// </summary>
        /// <param name="isolationLevel">Поведение транзакции</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_transaction == null)
            {
                _transaction = _dbContext.Database.BeginTransaction(isolationLevel);
            }
        }

        /// <summary>
        /// Откатывает транзакцию
        /// </summary>
        public void RollBackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }
        }

        /// <summary>
        /// Коммитит транзакцию
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        #region IDisposable

        private void Dispose(bool disposeContext)
        {
            if (!_disposed && disposeContext)
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction = null;
                }
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
using System;
using System.Data;
using System.Threading.Tasks;
using WpfClient.Infrastructure.EntityBase;

namespace WpfClient.Infrastructure.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Возвращает репозиторий для работы с сущностями
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности, с которой работает репозиторий</typeparam>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;

        /// <summary>
        /// Синхронно сохраняет изменения
        /// </summary>
        /// <exception cref="UnitOfWorkException">
        /// Возникает если не удалось по каким-либо причинам сохранить изменения
        /// </exception>

        void SaveChanges();

        /// <summary>
        /// Асинхронно сохраняет изменения
        /// </summary>
        /// <exception cref="UnitOfWorkException">
        /// Возникает если не удалось по каким-либо причинам сохранить изменения
        /// </exception>

        Task SaveChangesAsync();

        /// <summary>
        /// Открывает транзакцию
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Открывает транзакцию
        /// </summary>
        /// <param name="isolationLevel">Поведение транзакции</param>
        void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Откатывает транзакцию
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// Коммитит транзакцию
        /// </summary>
        void CommitTransaction();
    }
}
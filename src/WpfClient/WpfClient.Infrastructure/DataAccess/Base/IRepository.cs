using System;
using System.Linq;
using System.Linq.Expressions;
using WpfClient.Infrastructure.EntityBase;

namespace WpfClient.Infrastructure.DataAccess
{
    /// <summary>
    /// Репозиторий для работы с сущностями
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности, с которой работает репозиторий</typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>Возвращает IQueryable коллекцию сущностей</summary>
        /// <param name="predicate">Условие фильтрации</param>
        IQueryable<TEntity> Select(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Возвращает IQueryable коллекцию сущностей
        /// </summary>
        IQueryable<TEntity> Query();

        /// <summary>
        /// Добавляет новую сущность в коллекцию (без сохранения)
        /// </summary>

        TEntity Add(TEntity entity);

        /// <summary>
        /// Удаляет сущность из коллекции (без сохранения)
        /// </summary>

        void Remove(TEntity entity);

        /// <summary>
        /// Обновляет сущность в коллекции (без сохранения)
        /// </summary>

        TEntity Update(TEntity entity);
    }
}
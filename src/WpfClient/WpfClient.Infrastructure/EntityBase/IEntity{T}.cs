using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Infrastructure.EntityBase
{
    /// <summary>
    /// Представляет обобщенный интерфейс для класса сущности с Id
    /// </summary>
    /// <typeparam name="TKey">Тип данных первичного ключа Id.</typeparam>
    public interface IEntity<TKey> : IEntity
        where TKey : IEquatable<TKey>
    { 
        TKey Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Infrastructure.DataAccess
{
    public interface IUnitOfWork<TContext> : IUnitOfWork
    {
    }
}
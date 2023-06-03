using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.Model.Common;

namespace UpdateTransaction.Core.Interfaces.RepositoryInterface
{
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {
        Task<T?> Get(Guid id);
        Task<T?> Get(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> expression);

        Task<bool> Exists(Expression<Func<T, bool>> expression);

        IQueryable<T> Query();

        Task<T> Add(T entity);


    }
}

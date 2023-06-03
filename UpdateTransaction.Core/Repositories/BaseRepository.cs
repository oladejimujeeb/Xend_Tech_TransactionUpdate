using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UpdateTransaction.Core.Context;
using UpdateTransaction.Core.Interfaces.RepositoryInterface;
using UpdateTransaction.Core.Model.Common;

namespace UpdateTransaction.Core.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        protected  AppDbContext _context;
        public async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
             return await _context.Set<T>().AnyAsync(expression);   
        }

        public async Task<T?> Get(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T?> Get(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await  _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>();
        }
      
    }
}

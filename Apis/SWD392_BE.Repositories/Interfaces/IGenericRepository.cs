using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        ICollection<T> GetAll();
        ICollection<T> GetList(Expression<Func<T, bool>> expression);
        T Get(Expression<Func<T, bool>> expression);
        T Add(T entity);
        void AddRange(ICollection<T> entities);
        void Update(T entity);
        void Delete(Guid id);
        void ClearTrackers();
        int SaveChanges();
        Task SaveChangesAsync();
        void Dispose();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.DAL.Repositories.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        Task<T> AddAsync(T item);

        T Update(T item);

        void Delete(T item);

        IEnumerable<T> GetAll();

        IQueryable<T> GetQueryAll();

        Task<T> GetByIdAsync(int id);

        IEnumerable<T> FindByPredicate(Func<T, bool> predicate);
    }
}

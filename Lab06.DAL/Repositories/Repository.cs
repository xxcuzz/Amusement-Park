using Lab06.DAL.Context;
using Lab06.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.DAL.Repositories
{
    public class Repository<T> : IRepository<T>, IDisposable
        where T : class
    {
        private readonly ParkContext _context;

        public Repository(ParkContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T item)
        {
            var addedItem = await _context.Set<T>().AddAsync(item);
            return addedItem.Entity;
        }

        public T Update(T item)
        {
            var updatedItem = _context.Set<T>().Update(item);
            return updatedItem.Entity;
        }

        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public IEnumerable<T> GetAll()
        {
            return GetQueryAll().ToList();
        }

        public IQueryable<T> GetQueryAll()
        {
            return _context.Set<T>();
        }

        public IEnumerable<T> FindByPredicate(Func<T, bool> predicate)
        {
            return GetQueryAll().Where(predicate);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }
    }
}

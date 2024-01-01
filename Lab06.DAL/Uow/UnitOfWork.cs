using Lab06.DAL.Context;
using Lab06.DAL.Repositories;
using Lab06.DAL.Repositories.Interfaces;
using System;
using System.Collections;

namespace Lab06.DAL.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ParkContext _context;
        private bool disposed;
        private readonly Hashtable _repositories;

        public UnitOfWork(ParkContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        public IRepository<TData> Repository<TData>() where TData : class
        {
            var type = typeof(TData).Name;

            if (!_repositories.ContainsKey(type))
            {
                _repositories.Add(type, new Repository<TData>(_context));
            }
            return (IRepository<TData>)_repositories[type];
        }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

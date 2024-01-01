using Lab06.DAL.Repositories.Interfaces;
using System;

namespace Lab06.DAL.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        int Complete();
    }
}

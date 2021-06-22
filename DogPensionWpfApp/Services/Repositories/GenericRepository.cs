using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services.Repositories
{
    public class GenericRepository<TEntity, TConttext> : IGenericRepository<TEntity>, IDisposable
        where TEntity : class
        where TConttext : DbContext
    {
        protected readonly TConttext Context;

        protected GenericRepository(TConttext context)
        {
            Context = context;
        }

        public virtual void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public virtual bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public virtual void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public void Update(TEntity model)
        {
            Context.Set<TEntity>().Update(model);
        }
        public virtual void Dispose()
        {
            Context.Dispose();
        }
    }
}

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_ECommerce.Persistence.Contexts;
using Mini_ECommerce.Application.Abstractions.Repositories;

namespace Mini_ECommerce.Persistence.Concretes.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        readonly private MiniECommerceDbContext _context;
        public WriteRepository(MiniECommerceDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();
        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }
        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await Table.AddRangeAsync(entities);
            return true;
        }
        public bool Remove(T? entity)
        {
            if (entity != null)
            {
                EntityEntry<T> entityEntry = Table.Remove(entity);
                return entityEntry.State == EntityState.Deleted;
            }

            else return false;
        }
        public bool RemoveRange(List<T> entities)
        {
            Table.RemoveRange(entities);
            return true;
        }
        public async Task<bool> RemoveAsync(string id)
        {
            T? entity = await Table.FirstOrDefaultAsync(e => e.Id == Guid.Parse(id));

            return Remove(entity);
        }
        public bool Update(T entity)
        {
            EntityEntry entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();
    }
}

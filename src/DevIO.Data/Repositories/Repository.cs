using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity , new()
    {
        protected readonly MyDbContext Db;
        protected readonly DbSet<TEntity> Dbset;
        public Repository(MyDbContext db)
        {
            Db = db;
            Dbset = db.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Dbset.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await Dbset.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Dbset.FindAsync(id);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            Dbset.Add(entity);
            await SaveChanges();
        }
        
        public virtual async Task UpdateAsync(TEntity entity)
        {
            Dbset.Update(entity);
            await SaveChanges();
        }
        public virtual async Task RemoveAsync(Guid id)
        {
            Dbset.Remove(new TEntity {Id = id });
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }
        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
